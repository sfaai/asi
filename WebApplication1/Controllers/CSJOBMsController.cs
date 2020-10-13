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
    public class CSJOBMsController : Controller
    {
        private ASIDBConnection db = new ASIDBConnection();

        public PartialViewResult Search()
        {
            CSJOBM searchRec = null;

            if (Session["SearchJOBMRec"] != null)
            {
                searchRec = (CSJOBM)Session["SearchJOBMRec"];

            }
            else
            {
                searchRec = new CSJOBM();     
                searchRec.VDATE = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                searchRec.DUEDATE = searchRec.VDATE.AddMonths(1);
                searchRec.DUEDATE = searchRec.DUEDATE.AddDays(-1);
            }
            if (Session["SearchJOBMSort"] == null)
            {
                Session["SearchJOBMSort"] = "JOBNO";
            };

            List<SelectListItem> listItems = new List<SelectListItem>();
            listItems.Add(new SelectListItem
            {
                Text = "Company Name",
                Value = "CONAME",
                Selected = (string)Session["SearchJOBMSort"] == "CONAME"
            });

            listItems.Add(new SelectListItem
            {
                Text = "Job #",
                Value = "JOBNO",
                Selected = (string)Session["SearchJOBMSort"] == "JOBNO"
            });

            listItems.Add(new SelectListItem
            {
                Text = "Job # Latest",
                Value = "JOBNOLAST",
                Selected = (string)Session["SearchJOBMSort"] == "JOBNOLAST"
            });

            listItems.Add(new SelectListItem
            {
                Text = "Staff",
                Value = "JOBSTAFF",
                Selected = (string)Session["SearchJOBMSort"] == "JOBSTAFF"
            });

            listItems.Add(new SelectListItem
            {
                Text = "Date",
                Value = "VDATE",
                Selected = (string)Session["SearchJOBMSort"] == "VDATE"
            });

            listItems.Add(new SelectListItem
            {
                Text = "Archive",
                Value = "ARCHIVE",
                Selected = (string)Session["SearchJOBMSort"] == "ARCHIVE"
            });
            ViewBag.SORTBY = listItems;
            if (Session["HKSTAFFDB"] == null)
            {
                ViewBag.JOBSTAFF = new SelectList(db.HKSTAFFs.Select(x => new { STAFFCODE = x.STAFFCODE, STAFFDESC = x.STAFFDESC + " (" + x.STAFFCODE + ")" }).OrderBy(x => x.STAFFDESC), "STAFFCODE", "STAFFDESC");
            }
            else
            {
                ViewBag.JOBSTAFF = new SelectList(((IEnumerable<HKSTAFF>)Session["HKSTAFFDB"]).Select(x => new { STAFFCODE = x.STAFFCODE, STAFFDESC = x.STAFFDESC + " (" + x.STAFFCODE + ")" }).OrderBy(x => x.STAFFDESC), "STAFFCODE", "STAFFDESC");
            }
            return PartialView("Partial/Search", searchRec);
        }


        [HttpGet]
        public ActionResult SearchPost()
        {
            return Index(1);
        }

        [HttpPost]
        public ActionResult SearchPost(CSJOBM cSJOBM)
        {

            Session["SearchJOBMRec"] = cSJOBM;
            Session["SearchJOBMSort"] = Request.Params["SORTBY"] ?? "VDATE";
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
            ViewBag.Title = "Job Taking Listing";
            return View(CurrentSelection());

        }

        public IQueryable<CSJOBM> CurrentSelection()
        {
            string pSearchCode = "";
            string pSearchName = "";
            string pSearchJOBM = "";
            string pSearchStaff = "";
            DateTime pSearchVdate = DateTime.Parse("01/01/0001");
            DateTime pSearchDdate = DateTime.Parse("01/01/0001");

            if (Session["SearchJOBMRec"] != null)
            {
                CSJOBM searchRec = (CSJOBM)(Session["SearchJOBMRec"]);
                pSearchCode = searchRec.CSCOMSTR.COREGNO ?? "";
                pSearchName = searchRec.CSCOMSTR.CONAME ?? "";
                pSearchVdate = searchRec.VDATE;
                pSearchDdate = searchRec.DUEDATE;
                pSearchJOBM = searchRec.JOBNO ?? "";
                pSearchStaff = searchRec.JOBSTAFF ?? "";
            }
            else
            { // start with current month proforma bills instead of entire list
                pSearchVdate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                pSearchDdate = pSearchVdate.AddMonths(1);
                pSearchDdate = pSearchDdate.AddDays(-1);
            }

            IQueryable<CSJOBM> cSJOBMs = db.CSJOBMs.Include(c => c.CSCOMSTR).Include(c => c.HKSTAFF);

            if ((string)Session["SearchJOBMSort"] != "ARCHIVE") { cSJOBMs = db.CSJOBMs.Where(x => x.JOBPOST == "N"); }
            else { cSJOBMs = db.CSJOBMs.Where(x => x.JOBPOST == "Y"); }

            if (pSearchCode != "") { cSJOBMs = cSJOBMs.Where(x => x.CSCOMSTR.COREGNO.Contains(pSearchCode.ToUpper())); };
            if (pSearchName != "") { cSJOBMs = cSJOBMs.Where(x => x.CSCOMSTR.CONAME.Contains(pSearchName.ToUpper())); };
            if (pSearchStaff != "") { cSJOBMs = cSJOBMs.Where(x => x.JOBSTAFF == pSearchStaff); };
            if (pSearchVdate != DateTime.Parse("01/01/0001")) { cSJOBMs = cSJOBMs.Where(x => x.VDATE >= pSearchVdate); };
            if (pSearchDdate != DateTime.Parse("01/01/0001")) { cSJOBMs = cSJOBMs.Where(x => x.VDATE <= pSearchDdate); };
            if (pSearchJOBM != "")
            {
                if (pSearchJOBM.Length > 8)
                {
                    cSJOBMs = cSJOBMs.Where(x => x.JOBNO == pSearchJOBM);
                }
                else
                {
                    cSJOBMs = cSJOBMs.Where(x => x.JOBNO.Contains(pSearchJOBM));
                }
            };

            if ((string)Session["SearchJOBMSort"] == "CONAME")
            {
                cSJOBMs = cSJOBMs.OrderBy(n => n.CSCOMSTR.CONAME);
            }
            else if ((string)Session["SearchJOBMSort"] == "VDATE")
            {
                cSJOBMs = cSJOBMs.OrderBy(n => n.VDATE);

            }
            else if ((string)Session["SearchJOBMSort"] == "JOBNOLAST")
            {
                cSJOBMs = cSJOBMs.OrderByDescending(n => n.JOBNO);

            }
            else
            {
                cSJOBMs = cSJOBMs.OrderBy(n => n.JOBNO);
            }
            DateTime rptStart = pSearchVdate;
            DateTime rptEnd = pSearchDdate;

            ViewBag.RPT_START = rptStart.ToString("dd/MM/yyyy");
            ViewBag.RPT_END = rptEnd.ToString("dd/MM/yyyy");

            return cSJOBMs;
        }


        // GET: CSJOBMs
        public ActionResult Index(int? page)
        {
            var cSJOBMs = CurrentSelection();

            ViewBag.Title = "Job Taking List";
            ViewBag.page = page ?? 1;
            Session["CSJOBMPage"] = page ?? 1;
            return View(cSJOBMs.ToList().ToPagedList(page ?? 1, 30));
        }

        // GET: CSJOBMs/Details/5
        public ActionResult Details(string id, int? page)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSJOBM cSJOBM = db.CSJOBMs.Find(MyHtmlHelpers.ConvertByteStrToId(id));
            if (cSJOBM == null)
            {
                return HttpNotFound();
            }

            ViewBag.CONO = new SelectList(db.CSCOMSTRs.Select(x => new { CONO = x.CONO, CONAME = x.CONAME + "  (" + x.CONO + ")" }).OrderBy(y => y.CONAME), "CONO", "CONAME", cSJOBM.CONO);
            ViewBag.JOBSTAFF = new SelectList(db.HKSTAFFs, "STAFFCODE", "STAFFDESC", cSJOBM.JOBSTAFF);

            Session["CSJOBMPage"] = page ?? 1;
            ViewBag.Title = "View Job Taking ";
            ViewBag.Id = cSJOBM.JOBNO;
            return View("Edit", cSJOBM);
        }

        // GET: CSJOBMs/Create
        public ActionResult Create()
        {
            CSJOBM cSJOBM = new CSJOBM();
            cSJOBM.VDATE = DateTime.Today;
            cSJOBM.STAMP = 0;
            cSJOBM.JOBPOST = "N";
            cSJOBM.OKCNT = 0;
            cSJOBM.COMPLETE = "N";
            cSJOBM.JOBSTAFF = (string) Session["HKSTAFF"];
            cSJOBM.COMPLETED = new DateTime(3000, 1, 1);

            ViewBag.CONO = new SelectList(db.CSCOMSTRs.Select(x => new { CONO = x.CONO, CONAME = x.CONAME + "  (" + x.CONO + ")" }).OrderBy(y => y.CONAME), "CONO", "CONAME", cSJOBM.CONO);
            ViewBag.JOBSTAFF = new SelectList(db.HKSTAFFs, "STAFFCODE", "STAFFDESC", cSJOBM.JOBSTAFF);
            ViewBag.Title = "Create New Job Taking";
            return View("Edit", cSJOBM);
        }

        // POST: CSJOBMs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "JOBNO,VDATE,CONO,REM,JOBSTAFF,JOBPOST,CASECNT,OKCNT,COMPLETE,COMPLETED,STAMP")] CSJOBM cSJOBM)
        {

            if (ModelState.IsValid)
            {

                SALASTNO serialTbl = db.SALASTNOes.Find("CSJOB");
                if (serialTbl != null)
                {
                    CSCOMSTR cSCOMSTR = db.CSCOMSTRs.Find(cSJOBM.CONO);
                    if (cSCOMSTR != null)
                    {
                        try
                        {
                            string prefix = serialTbl.LASTPFIX;
                            int MaxNo = serialTbl.LASTNOMAX;
                            bool AutoGen = serialTbl.AUTOGEN == "Y";
                            serialTbl.LASTNO = serialTbl.LASTNO + 1;
                            cSJOBM.JOBNO = serialTbl.LASTNO.ToString("D10");

                            serialTbl.STAMP = serialTbl.STAMP + 1;
                            db.Entry(serialTbl).State = EntityState.Modified;


                            db.CSJOBMs.Add(cSJOBM);

                            db.SaveChanges();
                            //return Edit(MyHtmlHelpers.ConvertIdToByteStr(cSRCP.TRNO), 1);
                            return RedirectToAction("Edit", new { id = MyHtmlHelpers.ConvertIdToByteStr(cSJOBM.JOBNO), page = 1 });
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
                        catch (Exception e)
                        {
                            ModelState.AddModelError(string.Empty, e.Message);
                        }
                        finally
                        {

                        }

                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Unable to find company #");
                    }
                }


            }

            ViewBag.CONO = new SelectList(db.CSCOMSTRs.Select(x => new { CONO = x.CONO, CONAME = x.CONAME + "  (" + x.CONO + ")" }).OrderBy(y => y.CONAME), "CONO", "CONAME", cSJOBM.CONO);
            ViewBag.JOBSTAFF = new SelectList(db.HKSTAFFs, "STAFFCODE", "STAFFDESC", cSJOBM.JOBSTAFF);
            ViewBag.Title = "Create New Job Taking";
            return View("Edit", cSJOBM);
        }

        // GET: CSJOBMs/Edit/5
        public ActionResult Edit(string id, int? page)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSJOBM cSJOBM = db.CSJOBMs.Find(MyHtmlHelpers.ConvertByteStrToId(id));
            if (cSJOBM == null)
            {
                return HttpNotFound();
            }
            cSJOBM.CASECNT = cSJOBM.CSJOBDs.Count();
            ViewBag.CONO = new SelectList(db.CSCOMSTRs.Select(x => new { CONO = x.CONO, CONAME = x.CONAME + "  (" + x.CONO + ")" }).OrderBy(y => y.CONAME), "CONO", "CONAME", cSJOBM.CONO);
            ViewBag.JOBSTAFF = new SelectList(db.HKSTAFFs, "STAFFCODE", "STAFFDESC", cSJOBM.JOBSTAFF);
            Session["CSJOBMPage"] = page ?? 1;
            ViewBag.Title = "Edit Job Taking "; 
            ViewBag.Id = cSJOBM.JOBNO;
            ViewBag.page = page ?? 1;
            return View(cSJOBM);
        }

        public ActionResult EditStaff(string id, int? page)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSJOBM cSJOBM = db.CSJOBMs.Find(MyHtmlHelpers.ConvertByteStrToId(id));
            if (cSJOBM == null)
            {
                return HttpNotFound();
            }
            cSJOBM.CASECNT = cSJOBM.CSJOBDs.Count();
            ViewBag.CONO = new SelectList(db.CSCOMSTRs.Select(x => new { CONO = x.CONO, CONAME = x.CONAME + "  (" + x.CONO + ")" }).OrderBy(y => y.CONAME), "CONO", "CONAME", cSJOBM.CONO);
            ViewBag.JOBSTAFF = new SelectList(db.HKSTAFFs, "STAFFCODE", "STAFFDESC", cSJOBM.JOBSTAFF);
            Session["CSJOBMPage"] = page ?? 1;
            ViewBag.Title = "Assign Staff to Job ";
            ViewBag.page = page ?? 1;
            ViewBag.Id = cSJOBM.JOBNO;
            return View(cSJOBM);
        }

        // POST: CSJOBMs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "JOBNO,VDATE,CONO,REM,JOBSTAFF,JOBPOST,CASECNT,OKCNT,COMPLETE,COMPLETED,STAMP")] CSJOBM cSJOBM)
        {
            if (ModelState.IsValid)
            {
                //cSJOBM.CASECNT = cSJOBM.CSJOBDs.Count();
                try
                {
                    cSJOBM.STAMP = cSJOBM.STAMP + 1;
                    db.Entry(cSJOBM).State = EntityState.Modified;
                    db.SaveChanges();

                    int page = (int)Session["CSJOBMPage"];
                    return RedirectToAction("Index", new { page = page });
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
            ViewBag.CONO = new SelectList(db.CSCOMSTRs.Select(x => new { CONO = x.CONO, CONAME = x.CONAME + "  (" + x.CONO + ")" }).OrderBy(y => y.CONAME), "CONO", "CONAME", cSJOBM.CONO);
            ViewBag.JOBSTAFF = new SelectList(db.HKSTAFFs, "STAFFCODE", "STAFFDESC", cSJOBM.JOBSTAFF);
            return View(cSJOBM);
        }

        // GET: CSJOBMs/Delete/5
        public ActionResult Delete(string id, int? page)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSJOBM cSJOBM = db.CSJOBMs.Find(MyHtmlHelpers.ConvertByteStrToId(id));
            if (cSJOBM == null)
            {
                return HttpNotFound();
            }

            ViewBag.CONO = new SelectList(db.CSCOMSTRs.Select(x => new { CONO = x.CONO, CONAME = x.CONAME + "  (" + x.CONO + ")" }).OrderBy(y => y.CONAME), "CONO", "CONAME", cSJOBM.CONO);
            ViewBag.JOBSTAFF = new SelectList(db.HKSTAFFs, "STAFFCODE", "STAFFDESC", cSJOBM.JOBSTAFF);

            Session["CSJOBMPage"] = page ?? 1;
            ViewBag.Title = "Delete Job Taking ";
            return View("Edit", cSJOBM);
        }

        public ActionResult Post(string id, int? page)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSJOBM cSJOBM = db.CSJOBMs.Find(MyHtmlHelpers.ConvertByteStrToId(id));
            if (cSJOBM == null)
            {
                return HttpNotFound();
            }

            ViewBag.CONO = new SelectList(db.CSCOMSTRs.Select(x => new { CONO = x.CONO, CONAME = x.CONAME + "  (" + x.CONO + ")" }).OrderBy(y => y.CONAME), "CONO", "CONAME", cSJOBM.CONO);
            ViewBag.JOBSTAFF = new SelectList(db.HKSTAFFs, "STAFFCODE", "STAFFDESC", cSJOBM.JOBSTAFF);

            Session["CSJOBMPage"] = page ?? 1;
            ViewBag.Title = "Post Job Taking ";
            return View("Edit", cSJOBM);
        }

        // POST: CSJOBMs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            CSJOBM cSJOBM = db.CSJOBMs.Find(MyHtmlHelpers.ConvertByteStrToId(id));

            // Make sure do not remove posted jobs
            if ((cSJOBM.JOBPOST == "Y") || (cSJOBM.COMPLETE == "Y")) { ModelState.AddModelError(string.Empty, "Cannot Delete posted or completed Jobs"); }
            else
            {
                try
                {
                    // Remove all details
                    ICollection<CSJOBD> cSJOBDs = cSJOBM.CSJOBDs;
                    List<CSJOBD> cSJOBDList = cSJOBDs.ToList();
                    CSJOBD tempItem;
                    foreach (CSJOBD item in cSJOBDList)
                    {
                        tempItem = cSJOBDs.Where(x => x.JOBNO == item.JOBNO && x.CASENO == item.CASENO).FirstOrDefault();

                        db.CSJOBDs.Remove(tempItem);
                    }

                    db.CSJOBMs.Remove(cSJOBM);
                    db.SaveChanges();

                    int page = (int)Session["CSJOBMPage"];
                    return RedirectToAction("Index", new { page = page });

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
            ViewBag.CONO = new SelectList(db.CSCOMSTRs.Select(x => new { CONO = x.CONO, CONAME = x.CONAME + "  (" + x.CONO + ")" }).OrderBy(y => y.CONAME), "CONO", "CONAME", cSJOBM.CONO);
            ViewBag.JOBSTAFF = new SelectList(db.HKSTAFFs, "STAFFCODE", "STAFFDESC", cSJOBM.JOBSTAFF);

            ViewBag.Title = "Delete Job Taking ";
            return View("Edit", cSJOBM);
        }

        [HttpPost, ActionName("Post")]
        [ValidateAntiForgeryToken]
        public ActionResult PostConfirmed(string id)
        {
            CSJOBM cSJOBM = db.CSJOBMs.Find(MyHtmlHelpers.ConvertByteStrToId(id));

            // Make sure do not remove posted jobs
            if ((cSJOBM.JOBPOST == "Y") || (cSJOBM.CSJOBDs.Count() == 0) || (cSJOBM.COMPLETE == "Y")) { ModelState.AddModelError(string.Empty, "Cannot repost or post incomplete or completed Jobs"); }
            else
            {
                try
                {
                    // Remove all details

                    cSJOBM.JOBPOST = "Y";
                    cSJOBM.STAMP = cSJOBM.STAMP + 1;


                    var lastRec = db.CSJOBSTFs.Where(x => x.JOBNO == cSJOBM.JOBNO).OrderByDescending(y => y.ROWNO).FirstOrDefault();
                    // Update CSJOBSTF
                    CSJOBSTF cSJOBSTF = new CSJOBSTF();
                    cSJOBSTF.JOBNO = cSJOBM.JOBNO;
                    cSJOBSTF.SDATE = DateTime.Today;
                    cSJOBSTF.EDATE = new DateTime(3000, 1, 1);
                    cSJOBSTF.STAFFCODE = cSJOBM.JOBSTAFF;
                    cSJOBSTF.STAMP = 0;
                    if (lastRec == null)
                    {
                        cSJOBSTF.ROWNO = 1;
                    } else
                    {
                        cSJOBSTF.ROWNO = lastRec.ROWNO + 1;
                    }
                    db.CSJOBSTFs.Add(cSJOBSTF);

                    // Update CSJOBD
                    foreach ( CSJOBD item in cSJOBM.CSJOBDs)
                    {
                        // Update CSJOBST
                        CSJOBST cSJOBST = new CSJOBST();
                        cSJOBST.JOBNO = item.JOBNO;
                        cSJOBST.CASENO = item.CASENO;
                        cSJOBST.STAMP = 0;
                        cSJOBST.STAGEFR = "Pending";
                        cSJOBST.STAGETO = "New";
                        cSJOBST.INDATE = DateTime.Today;
                        cSJOBST.INTIME = DateTime.Now.ToString("HH:mm:ss");
                        cSJOBST.OUTDATE = new DateTime(3000, 1, 1);
                        cSJOBST.INIDX = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                        db.CSJOBSTs.Add(cSJOBST);


                        item.STAGE = "New";
                        item.STAGEDATE = DateTime.Today;
                        item.STAGETIME = cSJOBST.INTIME;
                        item.STAMP = item.STAMP + 1;
                        db.Entry(item).State = EntityState.Modified;
                    }

                    db.SaveChanges();

                    int page = (int)Session["CSJOBMPage"];
                    return RedirectToAction("Index", new { page = page });

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
            ViewBag.CONO = new SelectList(db.CSCOMSTRs.Select(x => new { CONO = x.CONO, CONAME = x.CONAME + "  (" + x.CONO + ")" }).OrderBy(y => y.CONAME), "CONO", "CONAME", cSJOBM.CONO);
            ViewBag.JOBSTAFF = new SelectList(db.HKSTAFFs, "STAFFCODE", "STAFFDESC", cSJOBM.JOBSTAFF);

            ViewBag.Title = "Post Job Taking ";
            return View("Edit", cSJOBM);
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
