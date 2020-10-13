using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Utility;
using System.Data.Entity.Core;
using System.Data.Entity.Validation;
using System.Data.Entity.Infrastructure;
using FirebirdSql.Data.Client;
using FirebirdSql.Data.FirebirdClient;

namespace WebApplication1.Controllers
{
    public class CSCOSECsController : Controller
    {
        private ASIDBConnection db = new ASIDBConnection();

        // GET: CSCOSECs
        public ActionResult Index()
        {
            var cSCOSECs = db.CSCOSECs.Include(c => c.CICL).Include(c => c.CSPR);
            return View(cSCOSECs.ToList());
        }

        public PartialViewResult EditRegChg(string id, string person, DateTime adate)
        {

            string sid = MyHtmlHelpers.ConvertByteStrToId(id);
            ViewBag.Parent = sid;
            ViewBag.CONO = sid;
            ViewBag.PRSCODE = person;
            ViewBag.ADATE = adate;

            var cSCOSECCHG = db.CSCOSECCHGs.Where(x => x.CONO == sid && x.PRSCODE == person && x.ADATE == adate).ToList();
            return PartialView("Partial/PartialChg", cSCOSECCHG);
        }

        // GET: CSCOSECs/Details/5
        public ActionResult Details(string id, string person, DateTime adate)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSCOSEC cSCOSEC = db.CSCOSECs.Find(MyHtmlHelpers.ConvertByteStrToId(id), person, adate);
            if (cSCOSEC == null)
            {
                return HttpNotFound();
            }
            ViewBag.CONOSEC = new SelectList(db.CICLs.OrderBy(x => x.CLNAME), "CLCODE", "CLNAME", cSCOSEC.CONOSEC);
            ViewBag.PRSCODE = new SelectList(db.CSPRS.Where(x => x.HKCONST.CONSTTYPE == "Individual").OrderBy(x => x.PRSNAME), "PRSCODE", "PRSNAME", cSCOSEC.PRSCODE);

            return View(cSCOSEC);
        }

        // GET: CSCOSECs/Create
        public ActionResult Create(string id)
        {
            ViewBag.CONOSEC = new SelectList(db.CICLs.OrderBy(x => x.CLNAME), "CLCODE", "CLNAME");
            ViewBag.Parent = MyHtmlHelpers.ConvertByteStrToId(id);
            string sid = ViewBag.Parent;

            CSCOSEC curRec = new CSCOSEC();
            curRec.CONO = sid;
            curRec.ADATE = DateTime.Today;
            curRec.ENDDATE = new DateTime(3000, 1, 1);
            curRec.CSCOMSTR = db.CSCOMSTRs.Find(curRec.CONO);
            ViewBag.PRSCODE = new SelectList(db.CSPRS.Where(x => x.HKCONST.CONSTTYPE == "Individual").OrderBy(x => x.PRSNAME), "PRSCODE", "PRSNAME", curRec.PRSCODE);
            return View(curRec);
        }

        // POST: CSCOSECs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CONO,PRSCODE,CONOSEC,ADATE,RDATE,ENDDATE,REM,ROWNO,STAMP")] CSCOSEC cSCOSEC)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    int lastRowNo = 0;
                    try
                    {
                        lastRowNo = db.CSCOSECs.Where(m => m.CONO == cSCOSEC.CONO).Max(n => n.ROWNO);
                    }
                    catch (Exception e) { lastRowNo = 0; }
                    finally { };

                    cSCOSEC.STAMP = 0;
                    cSCOSEC.ROWNO = lastRowNo + 1;
                    db.CSCOSECs.Add(cSCOSEC);
                    db.SaveChanges();
                    return new RedirectResult(Url.Action("Edit", "CSCOMSTRs", new { id = MyHtmlHelpers.ConvertIdToByteStr(cSCOSEC.CONO) }) + "#Secretary");
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
            }
            cSCOSEC.CSCOMSTR = db.CSCOMSTRs.Find(cSCOSEC.CONO);
            ViewBag.CONOSEC = new SelectList(db.CICLs.OrderBy(x => x.CLNAME), "CLCODE", "CLNAME", cSCOSEC.CONOSEC);
            ViewBag.PRSCODE = new SelectList(db.CSPRS.Where(x => x.HKCONST.CONSTTYPE == "Individual").OrderBy(x => x.PRSNAME), "PRSCODE", "PRSNAME", cSCOSEC.PRSCODE);
            return View(cSCOSEC);
        }

        // GET: CSCOSECs/Edit/5
        public ActionResult Edit(string id, string person, DateTime adate)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSCOSEC cSCOSEC = db.CSCOSECs.Find(MyHtmlHelpers.ConvertByteStrToId(id), person, adate);
            if (cSCOSEC == null)
            {
                return HttpNotFound();
            }
            ViewBag.CONOSEC = new SelectList(db.CICLs.OrderBy(x => x.CLNAME), "CLCODE", "CLNAME", cSCOSEC.CONOSEC);
            ViewBag.PRSCODE = new SelectList(db.CSPRS.Where(x => x.HKCONST.CONSTTYPE == "Individual").OrderBy(x => x.PRSNAME), "PRSCODE", "PRSNAME", cSCOSEC.PRSCODE);
            Session["CSCOSEC_ORIG"] = cSCOSEC;

            return View(cSCOSEC);
        }

        // POST: CSCOSECs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CONO,PRSCODE,CONOSEC,ADATE,RDATE,ENDDATE,REM,ROWNO,STAMP")] CSCOSEC cSCOSEC)
        {
            if (ModelState.IsValid)
            {
                try
                {


                    cSCOSEC.STAMP = cSCOSEC.STAMP + 1;


                    bool changed = false;
                    CSCOSEC csOrig = (CSCOSEC)Session["CSCOSEC_ORIG"];
                    if (csOrig.PRSCODE != cSCOSEC.PRSCODE) { changed = true; }
                    if (csOrig.ADATE != cSCOSEC.ADATE) { changed = true; }


                    if (changed)
                    {
                        CSCOSEC csDel = db.CSCOSECs.Find(csOrig.CONO, csOrig.PRSCODE, csOrig.ADATE);
                        db.CSCOSECs.Remove(csDel);
                        db.CSCOSECs.Add(cSCOSEC);
                    }
                    else
                    {
                        db.Entry(cSCOSEC).State = EntityState.Modified;
                    }
                    db.SaveChanges();
                    return new RedirectResult(Url.Action("Edit", "CSCOMSTRs", new { id = MyHtmlHelpers.ConvertIdToByteStr(cSCOSEC.CONO) }) + "#Secretary");
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
            }
            ViewBag.CONOSEC = new SelectList(db.CICLs.OrderBy(x => x.CLNAME), "CLCODE", "CLNAME", cSCOSEC.CONOSEC);
            ViewBag.PRSCODE = new SelectList(db.CSPRS.Where(x => x.HKCONST.CONSTTYPE == "Individual").OrderBy(x => x.PRSNAME), "PRSCODE", "PRSNAME", cSCOSEC.PRSCODE);
            cSCOSEC.CSCOMSTR = db.CSCOMSTRs.Find(cSCOSEC.CONO);
            cSCOSEC.CSCOSECCHGs = db.CSCOSECCHGs.Where(x => x.CONO == cSCOSEC.CONO && x.PRSCODE == cSCOSEC.PRSCODE && x.ADATE == cSCOSEC.ADATE).ToList();
            return View(cSCOSEC);
        }

        // GET: CSCOSECs/Delete/5
        public ActionResult Delete(string id, string person, DateTime adate)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSCOSEC cSCOSEC = db.CSCOSECs.Find(MyHtmlHelpers.ConvertByteStrToId(id), person, adate);
            if (cSCOSEC == null)
            {
                return HttpNotFound();
            }
            return View(cSCOSEC);
        }

        // POST: CSCOSECs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id, string person, DateTime adate)
        {
            CSCOSEC cSCOSEC = db.CSCOSECs.Find(MyHtmlHelpers.ConvertByteStrToId(id), person, adate);
            try
            {
                db.CSCOSECs.Remove(cSCOSEC);
                db.SaveChanges();
                return new RedirectResult(Url.Action("Edit", "CSCOMSTRs", new { id = MyHtmlHelpers.ConvertIdToByteStr(cSCOSEC.CONO) }) + "#Secretary");
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

            cSCOSEC.CSCOMSTR = db.CSCOMSTRs.Find(cSCOSEC.CONO);
            return View(cSCOSEC);
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
