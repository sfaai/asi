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
using System.Data.Entity.Validation;
using System.Data.Entity.Core;
using System.Data.Entity.Infrastructure;
using FirebirdSql.Data.Client;
using FirebirdSql.Data.FirebirdClient;

namespace WebApplication1.Controllers
{

    [Authorize(Roles = "Administrator,CS-A/C,CS-SEC,CS-AS")]
    public class CSCOSTATsController : Controller
    {
        private ASIDBConnection db = new ASIDBConnection();

        public PartialViewResult Search()
        {
            CSCOSTAT searchRec = null;
            //searchRec.CONO = pSearchCode;
            //searchRec.CONAME = pSearchName;
            if (Session["SearchCOSTATRec"] != null)
            {
                searchRec = (CSCOSTAT)Session["SearchCOSTATRec"];

            }
            else
            {
                searchRec = new CSCOSTAT();
                searchRec.SDATE = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                searchRec.EDATE = searchRec.SDATE.AddMonths(1).AddDays(-1);

            }
            if (Session["SearchCOSTATSort"] == null)
            {
                Session["SearchCOSTATSort"] = "COSTAT";
            };

            List<SelectListItem> listItems = new List<SelectListItem>();
            listItems.Add(new SelectListItem
            {
                Text = "Company Name",
                Value = "CONAME",
                Selected = (string)Session["SearchCOSTATSort"] == "CONAME"
            });

            listItems.Add(new SelectListItem
            {
                Text = "Status",
                Value = "COSTAT",
                Selected = (string)Session["SearchCOSTATSort"] == "COSTAT"
            });


            listItems.Add(new SelectListItem
            {
                Text = "Date",
                Value = "SDATE",
                Selected = (string)Session["SearchCOSTATSort"] == "SDATE"
            });

            listItems.Add(new SelectListItem
            {
                Text = "Date Latest",
                Value = "SDATELAST",
                Selected = (string)Session["SearchCOSTATSort"] == "SDATELAST"
            });

            listItems.Add(new SelectListItem
            {
                Text = "Staff",
                Value = "STAFFCODE",
                Selected = (string)Session["SearchCOSTATSort"] == "STAFFCODE"
            });

            listItems.Add(new SelectListItem
            {
                Text = "Archive",
                Value = "ARCHIVE",
                Selected = (string)Session["SearchCOSTATSort"] == "ARCHIVE"
            });
            ViewBag.SORTBY = listItems;
            if (Session["HKSTAFFDB"] == null)
            {
                ViewBag.STAFFCODE = new SelectList(db.HKSTAFFs.Select(x => new { STAFFCODE = x.STAFFCODE, STAFFDESC = x.STAFFDESC + " (" + x.STAFFCODE + ")" }).OrderBy(x => x.STAFFDESC), "STAFFCODE", "STAFFDESC");
            }
            else
            {
                ViewBag.STAFFCODE = new SelectList(((IEnumerable<HKSTAFF>)Session["HKSTAFFDB"]).Select(x => new { STAFFCODE = x.STAFFCODE, STAFFDESC = x.STAFFDESC + " (" + x.STAFFCODE + ")" }).OrderBy(x => x.STAFFDESC), "STAFFCODE", "STAFFDESC");
            }
            ViewBag.COSTAT = new SelectList(db.CSSTATs, "COSTAT", "COSTAT");
            return PartialView("Partial/Search", searchRec);
        }

        [HttpPost]
        public ActionResult SearchPost(CSCOSTAT cSCOSTAT)
        {

            Session["SearchCOSTATRec"] = cSCOSTAT;
            Session["SearchCOSTATSort"] = Request.Params["SORTBY"] ?? "SDATE";
            return Redirect("?page=1");
            //return Index(1);
        }

        // GET: CSCOSTATs
        public ActionResult Index(int? page)
        {
            return View(CurrentSelection().ToList().ToPagedList(page ?? 1, 30));
        }

        public IQueryable<CSCOSTAT> CurrentSelection()
        {
            //return db.CSCOSTATs.Where(x => x.ROWNO == db.CSCOSTATs.Where(y => y.CONO == x.CONO).Max(z => z.ROWNO)).OrderByDescending(x => x.SDATE);

            string pSearchCode = "";
            string pSearchName = "";
            string pSearchCOSTAT = "";
            string pSearchStaff = "";
            string pSearchStage = "";
            DateTime pSearchVdate = DateTime.Parse("01/01/0001");
            DateTime pSearchDdate = DateTime.Parse("01/01/0001");

            if (Session["SearchCOSTATRec"] != null)
            {
                CSCOSTAT searchRec = (CSCOSTAT)(Session["SearchCOSTATRec"]);
                pSearchCode = searchRec.CSCOMSTR.COREGNO ?? "";
                pSearchName = searchRec.CSCOMSTR.CONAME ?? "";
                pSearchVdate = searchRec.SDATE;
                pSearchDdate = searchRec.EDATE;
                pSearchCOSTAT = searchRec.COSTAT ?? "";
                pSearchStaff = searchRec.CSCOMSTR.STAFFCODE ?? "";

            }
            else
            { // start with current month proforma bills instead of entire list
                pSearchVdate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                pSearchDdate = pSearchVdate.AddMonths(1).AddDays(-1);
            }

            IQueryable<CSCOSTAT> cSCOSTATs = db.CSCOSTATs.Include(c => c.CSCOMSTR);

            if ((string)Session["SearchCOSTATSort"] != "ARCHIVE") { cSCOSTATs = db.CSCOSTATs.Where(x => x.ROWNO == db.CSCOSTATs.Where(y => y.CONO == x.CONO).Max(z => z.ROWNO)); }
            else { cSCOSTATs = db.CSCOSTATs.Where(x => x.ROWNO < db.CSCOSTATs.Where(y => y.CONO == x.CONO).Max(z => z.ROWNO)); }

            if (pSearchCode != "") { cSCOSTATs = cSCOSTATs.Where(x => x.CSCOMSTR.COREGNO.Contains(pSearchCode.ToUpper())); };
            if (pSearchName != "") { cSCOSTATs = cSCOSTATs.Where(x => x.CSCOMSTR.CONAME.Contains(pSearchName.ToUpper())); };
            if (pSearchStaff != "") { cSCOSTATs = cSCOSTATs.Where(x => x.CSCOMSTR.STAFFCODE == pSearchStaff); }
            else
            {
                if (Session["HKSTAFFDB"] != null)
                {
                    var staffdb = (IEnumerable<HKSTAFF>)Session["HKSTAFFDB"];
                    List<string> tt = new List<string>();
                    foreach (HKSTAFF item in staffdb)
                    {
                        tt.Add(item.STAFFCODE);
                    }

                    cSCOSTATs = cSCOSTATs.Where(x => tt.Contains(x.CSCOMSTR.STAFFCODE));
                }
            };

            if (pSearchVdate != DateTime.Parse("01/01/0001")) { cSCOSTATs = cSCOSTATs.Where(x => x.SDATE >= pSearchVdate); };
            if (pSearchDdate != DateTime.Parse("01/01/0001")) { cSCOSTATs = cSCOSTATs.Where(x => x.SDATE <= pSearchDdate); };
            if (pSearchCOSTAT != "")
            {
                cSCOSTATs = cSCOSTATs.Where(x => x.COSTAT == pSearchCOSTAT);
            }


            if ((string)Session["SearchCOSTATSort"] == "CONAME")
            {
                cSCOSTATs = cSCOSTATs.OrderBy(n => n.CSCOMSTR.CONAME);
            }

            else if ((string)Session["SearchCOSTATSort"] == "SDATE")
            {
                cSCOSTATs = cSCOSTATs.OrderBy(n => n.SDATE);

            }
            else if ((string)Session["SearchCOSTATSort"] == "SDATELAST")
            {
                cSCOSTATs = cSCOSTATs.OrderByDescending(n => n.SDATE);

            }
            else if ((string)Session["SearchCOSTATSort"] == "COSTAT")
            {
                cSCOSTATs = cSCOSTATs.OrderBy(n => n.COSTAT);

            }
            else if ((string)Session["SearchCOSTATSort"] == "STAFFCODE")
            {
                cSCOSTATs = cSCOSTATs.OrderBy(n => n.CSCOMSTR.STAFFCODE);

            }
            else
            {
                cSCOSTATs = cSCOSTATs.OrderBy(n => n.COSTAT);
            }
            DateTime rptStart = pSearchVdate;
            DateTime rptEnd = pSearchDdate;

            ViewBag.RPT_START = rptStart.ToString("dd/MM/yyyy");
            ViewBag.RPT_END = rptEnd.ToString("dd/MM/yyyy");

            return cSCOSTATs;
        }


        // GET: CSCOSTATs/Details/5
        public ActionResult Details(string id, int row)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSCOSTAT cSCOSTAT = db.CSCOSTATs.Find(MyHtmlHelpers.ConvertByteStrToId(id), row);
            if (cSCOSTAT == null)
            {
                return HttpNotFound();
            }
            ViewBag.COSTAT = new SelectList(db.CSSTATs, "COSTAT", "COSTAT", cSCOSTAT.COSTAT);
            return View(cSCOSTAT);
        }

        public ActionResult Details1(string id, int row, int? page)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSCOSTAT cSCOSTAT = db.CSCOSTATs.Find(MyHtmlHelpers.ConvertByteStrToId(id), row);
            if (cSCOSTAT == null)
            {
                return HttpNotFound();
            }
            Session["CSCOSTATPage"] = page ?? 1;
            Session["CSCOSTAT_OLDCONO"] = cSCOSTAT.CONO; // storing the old no so that can allow change of cono
            ViewBag.CONO = new SelectList(db.CSCOMSTRs.Select(x => new { CONO = x.CONO, CONAME = x.CONAME + "  (" + x.CONO + ")" }).OrderBy(y => y.CONAME), "CONO", "CONAME", cSCOSTAT.CONO);
            ViewBag.COSTAT = new SelectList(db.CSSTATs, "COSTAT", "COSTAT", cSCOSTAT.COSTAT);
            ViewBag.page = page ?? 1;
            ViewBag.Title = "View Company Change Status";
            return View("Edit1", cSCOSTAT);
        }

        // GET: CSCOSTATs/Create
        public ActionResult Create(string id)
        {
            ViewBag.Parent = MyHtmlHelpers.ConvertByteStrToId(id);
            CSCOSTAT curRec = new CSCOSTAT();
            curRec.CONO = ViewBag.Parent;
            curRec.SDATE = DateTime.Now;
            curRec.CSCOMSTR = db.CSCOMSTRs.Find(curRec.CONO);
         
            ViewBag.COSTAT = new SelectList(db.CSSTATs, "COSTAT", "COSTAT", curRec.COSTAT);
            curRec.CSCOMSTR = db.CSCOMSTRs.Find(curRec.CONO);
            return View(curRec);
        }

        public ActionResult Create1(int? page)
        {
            
            CSCOSTAT curRec = new CSCOSTAT();
            curRec.SDATE = DateTime.Now;
            Session["CSCOSTATPage"] = page ?? 1;
            ViewBag.CONO = new SelectList(db.CSCOMSTRs.Select(x => new { CONO = x.CONO, CONAME = x.CONAME + "  (" + x.CONO + ")" }).OrderBy(y => y.CONAME), "CONO", "CONAME");
            ViewBag.COSTAT = new SelectList(db.CSSTATs, "COSTAT", "COSTAT", curRec.COSTAT);
            ViewBag.page = page ?? 1;
            ViewBag.Title = "Add Company Change Status";
            curRec.CSCOMSTR = db.CSCOMSTRs.Find(curRec.CONO);
            return View("Edit1", curRec);
        }

        // POST: CSCOSTATs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CONO,SDATE,COSTAT,FILETYPE,FILELOC,SEALLOC,EDATE,ROWNO,STAMP")] CSCOSTAT cSCOSTAT)
        {
            if (ModelState.IsValid)
            {
                int lastRowNo = 0;
                try
                {
                    lastRowNo = db.CSCOSTATs.Where(m => m.CONO == cSCOSTAT.CONO).Max(n => n.ROWNO);
                }
                catch (Exception e) { lastRowNo = 0; }
                finally { };

                try
                {
                    cSCOSTAT.STAMP = 0;
                    cSCOSTAT.EDATE = new DateTime(3000, 1, 1);
                    cSCOSTAT.ROWNO = lastRowNo + 1;

                    db.CSCOSTATs.Add(cSCOSTAT);
                    UpdateCompany(cSCOSTAT);
                    UpdatePreviousRow(cSCOSTAT);

                    db.SaveChanges();
                    //return RedirectToAction("Details","CSCOMSTRs", new { id = MyHtmlHelpers.ConvertIdToByteStr(cSCOSTAT.CONO) });
                    return new RedirectResult(Url.Action("Edit", "CSCOMSTRs", new { id = MyHtmlHelpers.ConvertIdToByteStr(cSCOSTAT.CONO) }) + "#Status");
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
            cSCOSTAT.CSCOMSTR = db.CSCOMSTRs.Find(cSCOSTAT.CONO);
            return View(cSCOSTAT);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create1([Bind(Include = "CONO,SDATE,COSTAT,FILETYPE,FILELOC,SEALLOC,EDATE,ROWNO,STAMP")] CSCOSTAT cSCOSTAT)
        {
            int page = (int)Session["CSCOSTATPage"];
            if (ModelState.IsValid)
            {
                int lastRowNo = 0;
                try
                {
                    lastRowNo = db.CSCOSTATs.Where(m => m.CONO == cSCOSTAT.CONO).Max(n => n.ROWNO);
                }
                catch (Exception e) { lastRowNo = -1; }
                finally { };

                try
                {
                    cSCOSTAT.STAMP = 0;
                    cSCOSTAT.EDATE = new DateTime(3000, 1, 1);
                    cSCOSTAT.ROWNO = lastRowNo + 1;

                    db.CSCOSTATs.Add(cSCOSTAT);
                    UpdateCompany(cSCOSTAT);
                    UpdatePreviousRow(cSCOSTAT);

                    db.SaveChanges();

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

         
            ViewBag.CONO = new SelectList(db.CSCOMSTRs.Select(x => new { CONO = x.CONO, CONAME = x.CONAME + "  (" + x.CONO + ")" }).OrderBy(y => y.CONAME), "CONO", "CONAME");
            ViewBag.COSTAT = new SelectList(db.CSSTATs, "COSTAT", "COSTAT", cSCOSTAT.COSTAT);
            ViewBag.page = page;
            ViewBag.Title = "Add Company Change Status";
            cSCOSTAT.CSCOMSTR = db.CSCOMSTRs.Find(cSCOSTAT.CONO);
            return View(cSCOSTAT);
        }

        // GET: CSCOSTATs/Edit/5
        public ActionResult Edit(string id, int row)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSCOSTAT cSCOSTAT = db.CSCOSTATs.Find(MyHtmlHelpers.ConvertByteStrToId(id), row);
            if (cSCOSTAT == null)
            {
                return HttpNotFound();
            };


            ViewBag.COSTAT = new SelectList(db.CSSTATs, "COSTAT", "COSTAT", cSCOSTAT.COSTAT);
            return View(cSCOSTAT);
        }
        public ActionResult Edit1(string id, int row, int? page)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSCOSTAT cSCOSTAT = db.CSCOSTATs.Find(MyHtmlHelpers.ConvertByteStrToId(id), row);
            if (cSCOSTAT == null)
            {
                return HttpNotFound();
            };
            Session["CSCOSTATPage"] = page ?? 1;
            Session["CSCOSTAT_OLDCONO"] = cSCOSTAT.CONO; // storing the old no so that can allow change of cono
            ViewBag.CONO = new SelectList(db.CSCOMSTRs.Select(x => new { CONO = x.CONO, CONAME = x.CONAME + "  (" + x.CONO + ")" }).OrderBy(y => y.CONAME), "CONO", "CONAME", cSCOSTAT.CONO);
            ViewBag.COSTAT = new SelectList(db.CSSTATs, "COSTAT", "COSTAT", cSCOSTAT.COSTAT);
            ViewBag.page = page ?? 1;
            ViewBag.Title = "Edit Company Change Status";
            return View(cSCOSTAT);
        }


        public ActionResult EditFileLoc(CSCOSTAT cSCOSTAT)
        {

            CSSTAT csStat = db.CSSTATs.Find(cSCOSTAT.COSTAT);
            //ModelState.Clear(); //apparent this has side-effects and using Remove is preferrable
            ModelState.Remove("FILETYPE");
            ModelState.Remove("FILELOC");
            ModelState.Remove("SEALLOC");

            cSCOSTAT.FILETYPE = csStat.FILENOPFIX;
            if ((csStat.FILENOFR == 0) && (csStat.FILENOTO == 0) && (csStat.SEALNOFR == 0) && (csStat.SEALNOTO == 0)) { cSCOSTAT.FILELOC = ""; return PartialView("Partial/EditNoFileSeal", cSCOSTAT); };
            if ((csStat.FILENOFR == 0) && (csStat.FILENOTO == 0)) { cSCOSTAT.FILELOC = ""; return PartialView("Partial/EditSealOnly", cSCOSTAT); };
            if ((csStat.SEALNOFR == 0) && (csStat.SEALNOTO == 0)) { cSCOSTAT.SEALLOC = ""; return PartialView("Partial/EditFileOnly", cSCOSTAT); };

            //db.CSCOSTATs.Attach(cSCOSTAT);
            //EntityState st = db.Entry(cSCOSTAT).State;
            //db.Entry(cSCOSTAT).State = EntityState.Modified;

            return PartialView("Partial/EditFileLoc", cSCOSTAT);


        }

        // POST: CSCOSTATs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CONO,SDATE,COSTAT,FILETYPE,FILELOC,SEALLOC,EDATE,ROWNO,STAMP")] CSCOSTAT cSCOSTAT)
        {
            if (ModelState.IsValid)
            {
                ASIDBConnection newdb = new ASIDBConnection();
                try
                {
                    CSCOSTAT curRec = newdb.CSCOSTATs.Find(cSCOSTAT.CONO, cSCOSTAT.ROWNO);
                    if (curRec.STAMP == cSCOSTAT.STAMP)
                    {
                        cSCOSTAT.STAMP = cSCOSTAT.STAMP + 1;

                        // only update the latest record
                        if (db.CSCOSTATs.Where(x => x.CONO == cSCOSTAT.CONO).OrderByDescending(x => x.ROWNO).Select(x => x.ROWNO).FirstOrDefault() == cSCOSTAT.ROWNO)
                        {
                            UpdateCompany(cSCOSTAT);                          
                        }
                        UpdatePreviousRow(cSCOSTAT);

                        db.Entry(cSCOSTAT).State = EntityState.Modified;
                        db.SaveChanges();
                        // return RedirectToAction("Details", "CSCOMSTRs", new { id = MyHtmlHelpers.ConvertIdToByteStr(cSCOSTAT.CONO) });
                        return new RedirectResult(Url.Action("Edit", "CSCOMSTRs", new { id = MyHtmlHelpers.ConvertIdToByteStr(cSCOSTAT.CONO) }) + "#Status");
                    }
                    else { ModelState.AddModelError(string.Empty, "Record is modified"); }
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
                    newdb.Dispose();
                }
            }
            ViewBag.COSTAT = new SelectList(db.CSSTATs, "COSTAT", "COSTAT", cSCOSTAT.COSTAT);
            cSCOSTAT.CSCOMSTR = db.CSCOMSTRs.Find(cSCOSTAT.CONO);
            return View(cSCOSTAT);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit1([Bind(Include = "CONO,SDATE,COSTAT,FILETYPE,FILELOC,SEALLOC,EDATE,ROWNO,STAMP")] CSCOSTAT cSCOSTAT)
        {
            if (ModelState.IsValid)
            {
                ASIDBConnection newdb = new ASIDBConnection();
                try
                {
                    string oldKey = (string)Session["CSCOSTAT_OLDCONO"];
                    CSCOSTAT curRec = db.CSCOSTATs.Find(oldKey, cSCOSTAT.ROWNO); //allowing for key change
                    if (curRec.STAMP == cSCOSTAT.STAMP)
                    {
                        cSCOSTAT.STAMP = cSCOSTAT.STAMP + 1;

                        if (oldKey != cSCOSTAT.CONO)
                        {
                            curRec.SDATE = new DateTime(3000,1,2); // make sure the old previous row date gets reversed
                            UpdatePreviousRow(curRec);
                            db.CSCOSTATs.Remove(curRec);

                            int lastRowNo = 0;
                            try
                            {
                                lastRowNo = db.CSCOSTATs.Where(m => m.CONO == cSCOSTAT.CONO).Max(n => n.ROWNO);
                            }
                            catch (Exception e) { lastRowNo = -1; }
                            finally { };

     
                            cSCOSTAT.ROWNO = lastRowNo + 1;


                            db.CSCOSTATs.Add(cSCOSTAT);
                        } else
                        {
                            db.Entry(cSCOSTAT).State = EntityState.Modified;
                        }

                        if (db.CSCOSTATs.Where(x => x.CONO == cSCOSTAT.CONO).OrderByDescending(x => x.ROWNO).Select(x => x.ROWNO).FirstOrDefault() == cSCOSTAT.ROWNO)
                        {
                            UpdateCompany(cSCOSTAT);
                        }

                        UpdatePreviousRow(cSCOSTAT); // will always do this for the current record

                      
                        db.SaveChanges();
                        // return RedirectToAction("Details", "CSCOMSTRs", new { id = MyHtmlHelpers.ConvertIdToByteStr(cSCOSTAT.CONO) });
                        //return new RedirectResult(Url.Action("Details", "CSCOMSTRs", new { id = MyHtmlHelpers.ConvertIdToByteStr(cSCOSTAT.CONO) }) + "#Status");

                   
                        int page = (int) Session["CSCOSTATPage"];
                        return RedirectToAction("Index", new { page =page }); 
                    }
                    else { ModelState.AddModelError(string.Empty, "Record is modified"); }
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
                    newdb.Dispose();
                }
            }
            ViewBag.CONO = new SelectList(db.CSCOMSTRs.Select(x => new { CONO = x.CONO, CONAME = x.CONAME + "  (" + x.CONO + ")" }).OrderBy(y => y.CONAME), "CONO", "CONAME", cSCOSTAT.CONO);
            ViewBag.COSTAT = new SelectList(db.CSSTATs, "COSTAT", "COSTAT", cSCOSTAT.COSTAT);
            cSCOSTAT.CSCOMSTR = db.CSCOMSTRs.Find(cSCOSTAT.CONO);
            return View(cSCOSTAT);
        }

        private bool UpdateCompany(CSCOSTAT cSCOSTAT)
        {
            bool result = false;

            CSCOMSTR cSCOMSTR = db.CSCOMSTRs.Find(cSCOSTAT.CONO);
            if (cSCOMSTR != null)
            {
                cSCOMSTR.COSTAT = cSCOSTAT.COSTAT;
                cSCOMSTR.FILELOC = cSCOSTAT.FILELOC;
                cSCOMSTR.FILETYPE = cSCOSTAT.FILETYPE;
                cSCOMSTR.COSTATD = cSCOSTAT.SDATE;
                cSCOMSTR.SEALLOC = cSCOSTAT.SEALLOC;
                cSCOMSTR.STAMP = cSCOMSTR.STAMP + 1;
                db.Entry(cSCOMSTR).State = EntityState.Modified;
                result = true;
            }

            return result;
        }

        private CSCOSTAT UpdatePreviousRow(CSCOSTAT cSCOSTAT)
        {
            bool result = false;
            CSCOSTAT curRec = db.CSCOSTATs.Where(m => m.CONO == cSCOSTAT.CONO && m.ROWNO < cSCOSTAT.ROWNO).OrderByDescending(n => n.ROWNO).FirstOrDefault();
            if (curRec != null)
            {
                System.DateTime lastDate = (cSCOSTAT.SDATE).AddDays(-1);

                curRec.EDATE = lastDate;
                curRec.STAMP = curRec.STAMP + 1;
                db.Entry(curRec).State = EntityState.Modified;
                result = true;
            }
            return curRec;
         
        }

        // GET: CSCOSTATs/Delete/5
        public ActionResult Delete(string id, int row)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSCOSTAT cSCOSTAT = db.CSCOSTATs.Find(MyHtmlHelpers.ConvertByteStrToId(id), row);
            if (cSCOSTAT == null)
            {
                return HttpNotFound();
            }
            return View(cSCOSTAT);
        }

        public ActionResult Delete1(string id, int row, int? page)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSCOSTAT cSCOSTAT = db.CSCOSTATs.Find(MyHtmlHelpers.ConvertByteStrToId(id), row);
            if (cSCOSTAT == null)
            {
                return HttpNotFound();
            }
            Session["CSCOSTATPage"] = page ?? 1;
            Session["CSCOSTAT_OLDCONO"] = cSCOSTAT.CONO; // storing the old no so that can allow change of cono
            ViewBag.CONO = new SelectList(db.CSCOMSTRs.Select(x => new { CONO = x.CONO, CONAME = x.CONAME + "  (" + x.CONO + ")" }).OrderBy(y => y.CONAME), "CONO", "CONAME", cSCOSTAT.CONO);
            ViewBag.COSTAT = new SelectList(db.CSSTATs, "COSTAT", "COSTAT", cSCOSTAT.COSTAT);
            ViewBag.page = page ?? 1;
            ViewBag.Title = "Delete Company Change Status";
            return View("Edit1", cSCOSTAT);
        }

        // POST: CSCOSTATs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id, int row)
        {
            CSCOSTAT cSCOSTAT = db.CSCOSTATs.Find(MyHtmlHelpers.ConvertByteStrToId(id), row);

            CSCOSTAT curRec = UpdatePreviousRow(cSCOSTAT);
            if (curRec != null)
            {
                try
                {
                    if (db.CSCOSTATs.Where(x => x.CONO == cSCOSTAT.CONO).OrderByDescending(x => x.ROWNO).Select(x => x.ROWNO).FirstOrDefault() == cSCOSTAT.ROWNO)
                    {
                        UpdateCompany(curRec);
                    }
                    db.CSCOSTATs.Remove(cSCOSTAT);
                    db.SaveChanges();
                    // return RedirectToAction("Details", "CSCOMSTRs", new { id = MyHtmlHelpers.ConvertIdToByteStr(cSCOSTAT.CONO) });
                    return new RedirectResult(Url.Action("Edit", "CSCOMSTRs", new { id = MyHtmlHelpers.ConvertIdToByteStr(cSCOSTAT.CONO) }) + "#Status");
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
            else
            {
                ModelState.AddModelError(string.Empty, "Cannot delete the only status record");
            }
            
            ViewBag.COSTAT = new SelectList(db.CSSTATs, "COSTAT", "COSTAT", cSCOSTAT.COSTAT);
            ViewBag.Title = "Delete Company Change Status";
            cSCOSTAT.CSCOMSTR = db.CSCOMSTRs.Find(cSCOSTAT.CONO);
            return View(cSCOSTAT);
        }

        [HttpPost, ActionName("Delete1")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete1Confirmed(string id, int row, int? page)
        {
            CSCOSTAT cSCOSTAT = db.CSCOSTATs.Find(MyHtmlHelpers.ConvertByteStrToId(id), row);

            CSCOSTAT curRec = UpdatePreviousRow(cSCOSTAT);
            if (curRec != null)
            {
                try
                {
                    if (db.CSCOSTATs.Where(x => x.CONO == cSCOSTAT.CONO).OrderByDescending(x => x.ROWNO).Select(x => x.ROWNO).FirstOrDefault() == cSCOSTAT.ROWNO)
                    {
                        UpdateCompany(curRec);
                    }

                    db.CSCOSTATs.Remove(cSCOSTAT);
                    db.SaveChanges();

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
            else
            {
                ModelState.AddModelError(string.Empty, "Cannot delete the only status record");
            }

            Session["CSCOSTATPage"] = page ?? 1;
            Session["CSCOSTAT_OLDCONO"] = cSCOSTAT.CONO; // storing the old no so that can allow change of cono
            ViewBag.CONO = new SelectList(db.CSCOMSTRs.Select(x => new { CONO = x.CONO, CONAME = x.CONAME + "  (" + x.CONO + ")" }).OrderBy(y => y.CONAME), "CONO", "CONAME", cSCOSTAT.CONO);
            ViewBag.COSTAT = new SelectList(db.CSSTATs, "COSTAT", "COSTAT", cSCOSTAT.COSTAT);
            ViewBag.page = page ?? 1;
            ViewBag.Title = "Delete Company Change Status";
            cSCOSTAT.CSCOMSTR = db.CSCOMSTRs.Find(cSCOSTAT.CONO);
            return View("Edit1", cSCOSTAT);
          
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
            ViewBag.Title = "Company Status Listing";
            return View(CurrentSelection());

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
