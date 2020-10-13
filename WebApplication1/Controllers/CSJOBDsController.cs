using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PagedList;
using WebApplication1.Utility;
using System.Data.Entity.Core;
using System.Data.Entity.Validation;
using System.Data.Entity.Infrastructure;
using FirebirdSql.Data.Client;
using FirebirdSql.Data.FirebirdClient;

namespace WebApplication1.Controllers
{
    [Authorize(Roles = "Administrator,CS-A/C,CS-SEC,CS-AS,ACC")]
    public class CSJOBDsController : Controller
    {
        private ASIDBConnection db = new ASIDBConnection();

        //[HttpGet, ActionName("SearchPost")]
        //public PartialViewResult HackSearch()
        //{
        //    return Search();
        //}


        public PartialViewResult Search()
        {
            CSJOBD searchRec = null;

            if (Session["SearchJOBDRec"] != null)
            {
                searchRec = (CSJOBD)Session["SearchJOBDRec"];

            }
            else
            {
                searchRec = new CSJOBD();
                searchRec.STAGEDATE = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                searchRec.DUEDATE = searchRec.STAGEDATE.AddMonths(1);              
                searchRec.DUEDATE = searchRec.DUEDATE.AddDays(-1);

            }
            if (Session["SearchJOBDSort"] == null)
            {
                Session["SearchJOBDSort"] = "JOBNO";
            };

            List<SelectListItem> listItems = new List<SelectListItem>();
            listItems.Add(new SelectListItem
            {
                Text = "Company Name",
                Value = "CONAME",
                Selected = (string)Session["SearchJOBDSort"] == "CONAME"
            });

            listItems.Add(new SelectListItem
            {
                Text = "Job #",
                Value = "JOBNO",
                Selected = (string)Session["SearchJOBDSort"] == "JOBNO"
            });

            listItems.Add(new SelectListItem
            {
                Text = "Job # Latest",
                Value = "JOBNOLAST",
                Selected = (string)Session["SearchJOBDSort"] == "JOBNOLAST"
            });

            listItems.Add(new SelectListItem
            {
                Text = "Date",
                Value = "STAGEDATE",
                Selected = (string)Session["SearchJOBDSort"] == "STAGEDATE"
            });

            listItems.Add(new SelectListItem
            {
                Text = "Stage",
                Value = "STAGE",
                Selected = (string)Session["SearchJOBDSort"] == "STAGE"
            });

            listItems.Add(new SelectListItem
            {
                Text = "Stage History",
                Value = "STAGEHIST",
                Selected = (string)Session["SearchJOBDSort"] == "STAGEHIST"
            });

            listItems.Add(new SelectListItem
            {
                Text = "Duration",
                Value = "AGE",
                Selected = (string)Session["SearchJOBDSort"] == "AGE"
            });

            listItems.Add(new SelectListItem
            {
                Text = "Staff",
                Value = "JOBSTAFF",
                Selected = (string)Session["SearchJOBDSort"] == "JOBSTAFF"
            });

            listItems.Add(new SelectListItem
            {
                Text = "Archive",
                Value = "ARCHIVE",
                Selected = (string)Session["SearchJOBDSort"] == "ARCHIVE"
            });
            ViewBag.SORTBY = listItems;
            //if (Session["HKSTAFFDB"] == null)
            //{
            //    ViewBag.JOBSTAFF = new SelectList(db.HKSTAFFs.Select(x => new { STAFFCODE = x.STAFFCODE, STAFFDESC = x.STAFFDESC + " (" + x.STAFFCODE + ")" }).OrderBy(x => x.STAFFDESC), "STAFFCODE", "STAFFDESC");
            //} else
            //{
            //    ViewBag.JOBSTAFF = new SelectList(((IEnumerable<HKSTAFF>) Session["HKSTAFFDB"]).Select(x => new { STAFFCODE = x.STAFFCODE, STAFFDESC = x.STAFFDESC + " (" + x.STAFFCODE + ")" }).OrderBy(x => x.STAFFDESC), "STAFFCODE", "STAFFDESC");
            //}
            ViewBag.JOBSTAFF = new SelectList(db.HKSTAFFs.Select(x => new { STAFFCODE = x.STAFFCODE, STAFFDESC = x.STAFFDESC + " (" + x.STAFFCODE + ")" }).OrderBy(x => x.STAFFDESC), "STAFFCODE", "STAFFDESC");
            SelectList myStages = new SelectList(db.CSSTGs.OrderBy(x => x.STAGESEQ), "STAGE", "STAGE");
            List<SelectListItem> newList = new List<SelectListItem>();
            foreach (SelectListItem item in myStages)
            {
                newList.Add(item);
            }

            newList.Add(new SelectListItem
            {
                Text = "Work in Progress",
                Value = "WIP",
            });


            ViewBag.STAGE = newList;
            return PartialView("Partial/Search", searchRec);
        }


        [HttpGet]
        public ActionResult SearchPost()
        {
            return Index(1);
        }

        [HttpPost]
        public ActionResult SearchPost(CSJOBD cSJOBD)
        {

            Session["SearchJOBDRec"] = cSJOBD;
            Session["SearchJOBDSort"] = Request.Params["SORTBY"] ?? "STAGEDATE";
            return Redirect("?page=1");
            //return Index(1);
        }
        public ActionResult Listing()
        {

            //DateTime rptStart;
            //DateTime.TryParse(Session["RPT_START"].ToString(), out rptStart);

            //DateTime rptEnd;
            //DateTime.TryParse(Session["RPT_END"].ToString(), out rptEnd);
            //ViewBag.RPT_START = rptStart.ToString("dd/MM/yyyy");
            //ViewBag.RPT_END = rptEnd.ToString("dd/MM/yyyy");

            SACOMP profileRec = db.SACOMPs.FirstOrDefault();
            ViewBag.Company = profileRec.CONAME.ToUpper() + " (" + profileRec.COREGNO + ") ";

            return View(CurrentSelection());

        }

        // GET: CSJOBDs
        public ActionResult Index(int? page)
        {
            var cSJOBDs = CurrentSelection();
            ViewBag.page = page ?? 1;
            Session["CSJOBDPage"] = page ?? 1;
            return View("Index",cSJOBDs.ToList().ToPagedList(page ?? 1, 30));
        }

        // GET: CSJOBDs called from jobstat with 2 parameters
        public ActionResult Index1(string staff, string stage)
        {
            string pSearchCode = "";
            string pSearchName = "";
            string pSearchJOBD = "";

            DateTime pSearchVdate = DateTime.Parse("01/01/0001");
            DateTime pSearchDdate = DateTime.Parse("01/01/0001");
            string pSearchStaff = staff ?? "";
            string pSearchStage = stage ?? "";
            CSJOBD cSJOBD = new CSJOBD();
            cSJOBD.STAGE = pSearchStage;
            cSJOBD.CSJOBM = new CSJOBM();
            cSJOBD.CSJOBM.CSCOMSTR = new CSCOMSTR();
            cSJOBD.CSJOBM.JOBSTAFF = pSearchStaff;
            Session["SearchJOBDRec"] = cSJOBD;

            var cSJOBDs = CurrentSelection();
            ViewBag.page = 1;
            Session["CSJOBDPage"] = 1;
            return View("Index", cSJOBDs.ToList().ToPagedList(1, 30));
        }

        public IQueryable<CSJOBD> CurrentSelection()
        {
            string pSearchCode = "";
            string pSearchName = "";
            string pSearchJOBD = "";
            string pSearchStaff = "";
            string pSearchStage = "";
            DateTime pSearchVdate = DateTime.Parse("01/01/0001");
            DateTime pSearchDdate = DateTime.Parse("01/01/0001");

            if (Session["SearchJOBDRec"] != null)
            {
                CSJOBD searchRec = (CSJOBD)(Session["SearchJOBDRec"]);
                pSearchCode = searchRec.CSJOBM.CSCOMSTR.COREGNO ?? "";
                pSearchName = searchRec.CSJOBM.CSCOMSTR.CONAME ?? "";
                pSearchVdate = searchRec.STAGEDATE;
                pSearchDdate = searchRec.DUEDATE;
                pSearchJOBD = searchRec.JOBNO ?? "";
                pSearchStaff = searchRec.CSJOBM.JOBSTAFF ?? "";
                pSearchStage = searchRec.STAGE ?? "";
            }
            else
            { // start with current month proforma bills instead of entire list
                pSearchVdate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                pSearchDdate = pSearchVdate.AddMonths(1);
                pSearchDdate = pSearchDdate.AddDays(-1);
            }

            IQueryable<CSJOBD> cSJOBDs = db.CSJOBDs.Include(c => c.CSJOBM);
            //if ((string)Session["SearchJOBDSort"] == "STAGEHIST")
            //{
            //    cSJOBDs = cSJOBDs.Include(d => d.CSJOBSTs);

            //    if (pSearchStage != "")
            //    {
            //        if (pSearchStage == "WIP")
            //        {
            //            cSJOBDs = cSJOBDs.Where(x => x.CSJOBSTs.Where( y =>  "Complete".Contains( y.STAGEFR  ) ) );
            //        }
            //        else
            //        {
            //            cSJOBDs = cSJOBDs.Where(x => x.STAGE == pSearchStage);
            //        }

            //    };
            //}

            if ((string)Session["SearchJOBDSort"] != "ARCHIVE") { cSJOBDs = db.CSJOBDs.Where(x => x.COMPLETE == "N"); }
            else { cSJOBDs = db.CSJOBDs.Where(x => x.COMPLETE == "Y"); }

            if (pSearchCode != "") { cSJOBDs = cSJOBDs.Where(x => x.CSJOBM.CSCOMSTR.COREGNO.Contains(pSearchCode.ToUpper())); };
            if (pSearchName != "") { cSJOBDs = cSJOBDs.Where(x => x.CSJOBM.CSCOMSTR.CONAME.Contains(pSearchName.ToUpper())); };
            if (pSearchStaff != "") { cSJOBDs = cSJOBDs.Where(x => x.CSJOBM.JOBSTAFF == pSearchStaff); }
            else {
                if (Session["HKSTAFFDB"] != null)
                {
                    var staffdb = (IEnumerable<HKSTAFF>)Session["HKSTAFFDB"];
                    List<string> tt = new List<string>();
                    foreach (HKSTAFF item in staffdb)
                    {
                        tt.Add(item.STAFFCODE);
                    }
                 
                    cSJOBDs = cSJOBDs.Where(x => tt.Contains( x.CSJOBM.JOBSTAFF) || x.CSJOBM.JOBSTAFF == null);
                }
            };
            if (pSearchStage != "")
            {
                if (pSearchStage == "WIP")
                {
                    cSJOBDs = cSJOBDs.Where(x => x.STAGE != "Complete" && x.STAGE != "Cancel");
                }
                else
                {
                    cSJOBDs = cSJOBDs.Where(x => x.STAGE == pSearchStage);
                }

            };
            if (pSearchVdate != DateTime.Parse("01/01/0001")) { cSJOBDs = cSJOBDs.Where(x => x.STAGEDATE >= pSearchVdate); };
            if (pSearchDdate != DateTime.Parse("01/01/0001")) { cSJOBDs = cSJOBDs.Where(x => x.STAGEDATE <= pSearchDdate); };
            if (pSearchJOBD != "")
            {
                if (pSearchJOBD.Length > 8)
                {
                    cSJOBDs = cSJOBDs.Where(x => x.JOBNO == pSearchJOBD);
                }
                else
                {
                    cSJOBDs = cSJOBDs.Where(x => x.JOBNO.Contains(pSearchJOBD));
                }
            };

            if ((string)Session["SearchJOBDSort"] == "CONAME")
            {
                cSJOBDs = cSJOBDs.OrderBy(n => n.CSJOBM.CSCOMSTR.CONAME);
            }
            else if ((string)Session["SearchJOBDSort"] == "STAGEDATE")
            {
                cSJOBDs = cSJOBDs.OrderBy(n => n.STAGEDATE);

            }
            else if ((string)Session["SearchJOBDSort"] == "AGE")
            {
                cSJOBDs = cSJOBDs.OrderBy(n => n.STAGEDATE);

            }
            else if ((string)Session["SearchJOBDSort"] == "JOBNOLAST")
            {
                cSJOBDs = cSJOBDs.OrderByDescending(n => n.JOBNO);

            }
            else if ((string)Session["SearchJOBDSort"] == "STAGE")
            {
                cSJOBDs = cSJOBDs.OrderBy(n => n.STAGE);

            }
            else if ((string)Session["SearchJOBDSort"] == "JOBSTAFF")
            {
                cSJOBDs = cSJOBDs.OrderBy(n => n.CSJOBM.JOBSTAFF);

            }
            else
            {
                cSJOBDs = cSJOBDs.OrderBy(n => n.JOBNO);
            }
            DateTime rptStart = pSearchVdate;
            DateTime rptEnd = pSearchDdate;

            ViewBag.RPT_START = rptStart.ToString("dd/MM/yyyy");
            ViewBag.RPT_END = rptEnd.ToString("dd/MM/yyyy");
            ViewBag.SORTBy = Session["SearchJOBDSort"];

            return cSJOBDs;
        }


        // GET: CSJOBDs/Details/5
        public ActionResult Details(string id, int row, int rel, int? page)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSJOBD cSJOBD = db.CSJOBDs.Find(MyHtmlHelpers.ConvertByteStrToId(id), row);
            if (cSJOBD == null)
            {
                return HttpNotFound();
            }

            ViewBag.CASECODE = new SelectList(db.CSCASEs.Select(x => new { CASECODE = x.CASECODE, CASEDESC = x.CASECODE + "\t | \t" + x.CASEDESC }), "CASECODE", "CASEDESC", cSJOBD.CASECODE);
            ViewBag.Title = "View Job Case Details ";
            ViewBag.page = page ?? 1;
            Session["CSJOBDPage"] = page ?? 1;
            return View("Edit", cSJOBD);
        }

        // GET: CSJOBDs/Create
        public ActionResult Create(string id)
        {
            CSJOBD cSJOBD = new CSJOBD();
            cSJOBD.JOBNO = MyHtmlHelpers.ConvertByteStrToId(id);
            cSJOBD.STAGETIME = "";
            cSJOBD.STAGE = "";
            cSJOBD.STAMP = 0;
            cSJOBD.COMPLETE = "N";
            cSJOBD.COMPLETED = new DateTime(3000, 1, 1);

            ViewBag.CASECODE = new SelectList(db.CSCASEs.Select(x => new { CASECODE = x.CASECODE, CASEDESC = x.CASECODE + "\t | \t" + x.CASEDESC }), "CASECODE", "CASEDESC", cSJOBD.CASECODE);

            ViewBag.Title = "Add New Job Case Details ";
            ViewBag.Id = cSJOBD.JOBNO;
            ViewBag.Row = cSJOBD.CASENO;
            return View("Edit", cSJOBD);
        }

        // POST: CSJOBDs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "JOBNO,CASENO,CASECODE,CASEMEMO,CASEREM,STAGE,STAGEDATE,STAGETIME,COMPLETE,COMPLETED,STAMP")] CSJOBD cSJOBD)
        {
            if (ModelState.IsValid)
            {
                CSJOBD lastRec = db.CSJOBDs.Where(m => m.JOBNO == cSJOBD.JOBNO).OrderByDescending(n => n.CASENO).FirstOrDefault();
                if (lastRec == null)
                {
                    cSJOBD.CASENO = 1;
                }
                else
                {
                    cSJOBD.CASENO = lastRec.CASENO + 1;
                }
                if (string.IsNullOrEmpty(cSJOBD.STAGE)) { cSJOBD.STAGE = ""; }
                if (string.IsNullOrEmpty(cSJOBD.STAGETIME)) { cSJOBD.STAGETIME = ""; }
                try
                {
                    db.CSJOBDs.Add(cSJOBD);
                    db.SaveChanges();
                    return RedirectToAction("Edit", "CSJOBMs", new { id = MyHtmlHelpers.ConvertIdToByteStr(cSJOBD.JOBNO) });
                }
                catch (DbEntityValidationException dbEx)
                {
                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            //string message = string.Format("{0}:{1}",
                            //    validationErrors.Entry.Entity.ToString(),
                            //   validationError.ErrorMessage);
                            // raise a new exception nesting
                            // the current instance as InnerException
                            ModelState.AddModelError(validationError.PropertyName, validationError.ErrorMessage);
                        }
                    }
                }
                catch (DbUpdateException ex)
                {
                    UpdateException updateException = (UpdateException)ex.InnerException;
                    if (updateException != null)
                    {
                        if (updateException.InnerException != null)
                        {
                            var sqlException = (FirebirdSql.Data.FirebirdClient.FbException)updateException.InnerException;

                            foreach (var error in sqlException.Errors)
                            {
                                if (error.Message != null)
                                {
                                    ModelState.AddModelError(string.Empty, error.Message);
                                }
                            }
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, updateException.Message);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, updateException.Message);
                    }
                }
            }
            ViewBag.CASECODE = new SelectList(db.CSCASEs.Select(x => new { CASECODE = x.CASECODE, CASEDESC = x.CASECODE + "\t | \t" + x.CASEDESC }), "CASECODE", "CASEDESC", cSJOBD.CASECODE);
            return View(cSJOBD);
        }

        // GET: CSJOBDs/Edit/5
        public ActionResult Edit(string id, int row, int rel, int? page)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSJOBD cSJOBD = db.CSJOBDs.Find(MyHtmlHelpers.ConvertByteStrToId(id), row);
            if (cSJOBD == null)
            {
                return HttpNotFound();
            }
            ViewBag.CASECODE = new SelectList(db.CSCASEs.Select(x => new { CASECODE = x.CASECODE, CASEDESC = x.CASECODE + "\t | \t" + x.CASEDESC }), "CASECODE", "CASEDESC", cSJOBD.CASECODE);
            ViewBag.Id = cSJOBD.JOBNO;
            ViewBag.Row = cSJOBD.CASENO;
            Session["CSJOBDrel"] = rel;
            ViewBag.page = page ?? 1;
            Session["CSJOBDPage"] = ViewBag.page;
            ViewBag.Title = "Edit Job Case Details ";
            return View(cSJOBD);
        }

        // POST: CSJOBDs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "JOBNO,CASENO,CASECODE,CASEMEMO,CASEREM,STAGE,STAGEDATE,STAGETIME,COMPLETE,COMPLETED,STAMP")] CSJOBD cSJOBD)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (string.IsNullOrEmpty(cSJOBD.STAGE)) { cSJOBD.STAGE = ""; }
                    if (string.IsNullOrEmpty(cSJOBD.STAGETIME)) { cSJOBD.STAGETIME = ""; }
                    cSJOBD.STAMP = cSJOBD.STAMP + 1;
                    db.Entry(cSJOBD).State = EntityState.Modified;
                    db.SaveChanges();
                    int rel = (int)Session["CSJOBDrel"];
                    if (rel == 0)
                    {
                        return RedirectToAction("Edit", "CSJOBMs", new { id = MyHtmlHelpers.ConvertIdToByteStr(cSJOBD.JOBNO) });
                    }
                    else
                    {
                        int page = (int)Session["CSJOBDPage"];
                        return RedirectToAction("Index", "CSJOBDs", new { page = page });
                    }
                }
                catch (DbEntityValidationException dbEx)
                {
                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            //string message = string.Format("{0}:{1}",
                            //    validationErrors.Entry.Entity.ToString(),
                            //   validationError.ErrorMessage);
                            // raise a new exception nesting
                            // the current instance as InnerException
                            ModelState.AddModelError(validationError.PropertyName, validationError.ErrorMessage);
                        }
                    }
                }
                catch (DbUpdateException ex)
                {
                    UpdateException updateException = (UpdateException)ex.InnerException;
                    if (updateException != null)
                    {
                        if (updateException.InnerException != null)
                        {
                            var sqlException = (FirebirdSql.Data.FirebirdClient.FbException)updateException.InnerException;

                            foreach (var error in sqlException.Errors)
                            {
                                if (error.Message != null)
                                {
                                    ModelState.AddModelError(string.Empty, error.Message);
                                }
                            }
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, updateException.Message);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, updateException.Message);
                    }
                }
                catch (Exception e) { ModelState.AddModelError(string.Empty, e.Message); }
            }
            ViewBag.CASECODE = new SelectList(db.CSCASEs.Select(x => new { CASECODE = x.CASECODE, CASEDESC = x.CASECODE + "\t | \t" + x.CASEDESC }), "CASECODE", "CASEDESC", cSJOBD.CASECODE);

            return View(cSJOBD);
        }

        // GET: CSJOBDs/Delete/5
        public ActionResult Delete(string id, int row, int rel, int? page)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSJOBD cSJOBD = db.CSJOBDs.Find(MyHtmlHelpers.ConvertByteStrToId(id), row);
            if (cSJOBD == null)
            {
                return HttpNotFound();
            }
            ViewBag.Id = cSJOBD.JOBNO;
            ViewBag.Row = cSJOBD.CASENO;
            Session["CSJOBDrel"] = rel;
            ViewBag.CASECODE = new SelectList(db.CSCASEs.Select(x => new { CASECODE = x.CASECODE, CASEDESC = x.CASECODE + "\t | \t" + x.CASEDESC }), "CASECODE", "CASEDESC", cSJOBD.CASECODE);
            ViewBag.Title = "Delete Job Case Details ";
            ViewBag.page = page ?? 1;
            Session["CSJOBDPage"] = ViewBag.page;
            return View("Edit", cSJOBD);
        }

        // POST: CSJOBDs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id, int row)
        {
            CSJOBD cSJOBD = db.CSJOBDs.Find(MyHtmlHelpers.ConvertByteStrToId(id), row);
            try
            {
                db.CSJOBDs.Remove(cSJOBD);
                db.SaveChanges();
                int rel = (int)Session["CSJOBDrel"];
                if (rel == 0)
                {
                    return RedirectToAction("Edit", "CSJOBMs", new { id = MyHtmlHelpers.ConvertIdToByteStr(cSJOBD.JOBNO) });
                }
                else
                {
                    int page = (int)Session["CSJOBDPage"];
                    return RedirectToAction("Index", "CSJOBDs", new { page = page });
                }
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        //string message = string.Format("{0}:{1}",
                        //    validationErrors.Entry.Entity.ToString(),
                        //   validationError.ErrorMessage);
                        // raise a new exception nesting
                        // the current instance as InnerException
                        ModelState.AddModelError(validationError.PropertyName, validationError.ErrorMessage);
                    }
                }
            }
            catch (DbUpdateException ex)
            {
                UpdateException updateException = (UpdateException)ex.InnerException;
                if (updateException != null)
                {
                    if (updateException.InnerException != null)
                    {
                        var sqlException = (FirebirdSql.Data.FirebirdClient.FbException)updateException.InnerException;

                        foreach (var error in sqlException.Errors)
                        {
                            if (error.Message != null)
                            {
                                ModelState.AddModelError(string.Empty, error.Message);
                            }
                        }
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, updateException.Message);
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, updateException.Message);
                }
            }

            ViewBag.Id = cSJOBD.JOBNO;
            ViewBag.Row = cSJOBD.CASENO;
            ViewBag.CASECODE = new SelectList(db.CSCASEs.Select(x => new { CASECODE = x.CASECODE, CASEDESC = x.CASECODE + "\t | \t" + x.CASEDESC }), "CASECODE", "CASEDESC", cSJOBD.CASECODE);
            ViewBag.Title = "Delete Job Case Details ";
            ViewBag.page = Session["CSJOBDPage"];

            return View("Edit", cSJOBD);
        }

        public ActionResult Post(string id, int row, int rel, int? page)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSJOBD cSJOBD = db.CSJOBDs.Find(MyHtmlHelpers.ConvertByteStrToId(id), row);
            if (cSJOBD == null)
            {
                return HttpNotFound();
            }
            ViewBag.Id = cSJOBD.JOBNO;
            ViewBag.Row = cSJOBD.CASENO;
            Session["CSJOBDrel"] = rel;
            ViewBag.CASECODE = new SelectList(db.CSCASEs.Select(x => new { CASECODE = x.CASECODE, CASEDESC = x.CASECODE + "\t | \t" + x.CASEDESC }), "CASECODE", "CASEDESC", cSJOBD.CASECODE);
            ViewBag.Title = "Post Job Case Details ";
            ViewBag.page = page ?? 1;
            Session["CSJOBDPage"] = ViewBag.page;
            return View("Edit", cSJOBD);
        }

        [HttpPost, ActionName("Post")]
        [ValidateAntiForgeryToken]
        public ActionResult PostConfirmed(string id, int row)
        {
            CSJOBD cSJOBD = db.CSJOBDs.Find(MyHtmlHelpers.ConvertByteStrToId(id), row);
            CSJOBM cSJOBM = db.CSJOBMs.Find(MyHtmlHelpers.ConvertByteStrToId(id));
            try

            {
                if ((cSJOBD.STAGE == "Complete") || (cSJOBD.STAGE == "Cancel"))
                {
                    cSJOBD.COMPLETE = "Y";
                    cSJOBD.COMPLETED = DateTime.Today;
                    cSJOBD.STAMP = cSJOBD.STAMP + 1;

                    cSJOBM.OKCNT = cSJOBM.OKCNT + 1;
                    cSJOBM.STAMP = cSJOBM.STAMP + 1;

                    db.Entry(cSJOBD).State = EntityState.Modified;
                    db.Entry(cSJOBM).State = EntityState.Modified;

                    db.SaveChanges();
                    int rel = (int)Session["CSJOBDrel"];
                    if (rel == 0)
                    {
                        return RedirectToAction("Edit", "CSJOBMs", new { id = MyHtmlHelpers.ConvertIdToByteStr(cSJOBD.JOBNO) });
                    }
                    else
                    {
                        int page = (int)Session["CSJOBDPage"];
                        return RedirectToAction("Index", "CSJOBDs", new { page = page });
                    }
                }
                else { ModelState.AddModelError(string.Empty, "Can Only Post Completed or Cancelled Jobs"); }
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        //string message = string.Format("{0}:{1}",
                        //    validationErrors.Entry.Entity.ToString(),
                        //   validationError.ErrorMessage);
                        // raise a new exception nesting
                        // the current instance as InnerException
                        ModelState.AddModelError(validationError.PropertyName, validationError.ErrorMessage);
                    }
                }
            }
            catch (DbUpdateException ex)
            {
                UpdateException updateException = (UpdateException)ex.InnerException;
                if (updateException != null)
                {
                    if (updateException.InnerException != null)
                    {
                        var sqlException = (FirebirdSql.Data.FirebirdClient.FbException)updateException.InnerException;

                        foreach (var error in sqlException.Errors)
                        {
                            if (error.Message != null)
                            {
                                ModelState.AddModelError(string.Empty, error.Message);
                            }
                        }
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, updateException.Message);
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, updateException.Message);
                }
            }
            catch (Exception e) { ModelState.AddModelError(string.Empty, e.Message); }


            ViewBag.Id = cSJOBD.JOBNO;
            ViewBag.Row = cSJOBD.CASENO;

            ViewBag.CASECODE = new SelectList(db.CSCASEs.Select(x => new { CASECODE = x.CASECODE, CASEDESC = x.CASECODE + "\t | \t" + x.CASEDESC }), "CASECODE", "CASEDESC", cSJOBD.CASECODE);
            ViewBag.Title = "Post Job Case Details ";
            ViewBag.page = Session["CSJOBDPage"];
            return View("Edit", cSJOBD);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
