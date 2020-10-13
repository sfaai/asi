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
    public class CSCODRsController : Controller
    {
        private ASIDBConnection db = new ASIDBConnection();

        // GET: CSCODRs
        public ActionResult Index()
        {
            var cSCODRs = db.CSCODRs.Include(c => c.CSPR);
            return View(cSCODRs.ToList());
        }

        public PartialViewResult EditRegChg(string id, string person, DateTime adate)
        {

            string sid = MyHtmlHelpers.ConvertByteStrToId(id);
            ViewBag.Parent = sid;
            ViewBag.CONO = sid;
            ViewBag.PRSCODE = person;
            ViewBag.ADATE = adate;

            var cSCODRCHG = db.CSCODRCHGs.Where(x => x.CONO == sid && x.PRSCODE == person && x.ADATE == adate).ToList();
            return PartialView("Partial/PartialChg", cSCODRCHG);
        }

        // GET: CSCODRs/Details/5
        public ActionResult Details(string id, string person, DateTime adate)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSCODR cSCODR = db.CSCODRs.Find(MyHtmlHelpers.ConvertByteStrToId(id), person, adate);
            if (cSCODR == null)
            {
                return HttpNotFound();
            }
            ViewBag.PRSCODE = new SelectList(db.CSPRS.Where(x => x.HKCONST.CONSTTYPE == "Individual").OrderBy(x => x.PRSNAME), "PRSCODE", "PRSNAME", cSCODR.PRSCODE);

            return View(cSCODR);
        }

        // GET: CSCODRs/Create
        public ActionResult Create(string id)
        {
            ViewBag.Parent = MyHtmlHelpers.ConvertByteStrToId(id);
            string sid = ViewBag.Parent;
          
            CSCODR curRec = new CSCODR();
            curRec.CONO = sid;
            curRec.ADATE = DateTime.Today;
            curRec.ENDDATE = new DateTime(3000, 1, 1);
            curRec.CSCOMSTR = db.CSCOMSTRs.Find(curRec.CONO);
            ViewBag.PRSCODE = new SelectList(db.CSPRS.Where(x => x.HKCONST.CONSTTYPE == "Individual").OrderBy(x => x.PRSNAME), "PRSCODE", "PRSNAME", curRec.PRSCODE);
            return View(curRec);
        }

        // POST: CSCODRs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CONO,PRSCODE,ADATE,RDATE,ENDDATE,REM,ROWNO,STAMP")] CSCODR cSCODR)
        {
            if (ModelState.IsValid)
            {
                int lastRowNo = 0;
                try
                {
                    lastRowNo = db.CSCODRs.Where(m => m.CONO == cSCODR.CONO).Max(n => n.ROWNO);
                }
                catch (Exception e) { lastRowNo = 0; }
                finally { };

                cSCODR.STAMP = 0;
                try
                {

                    cSCODR.ROWNO = lastRowNo + 1;
                    db.CSCODRs.Add(cSCODR);
                    db.SaveChanges();
                    return new RedirectResult(Url.Action("Edit", "CSCOMSTRs", new { id = MyHtmlHelpers.ConvertIdToByteStr(cSCODR.CONO) }) + "#Director");
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
            cSCODR.CSCOMSTR = db.CSCOMSTRs.Find(cSCODR.CONO);
            ViewBag.PRSCODE = new SelectList(db.CSPRS.Where(x => x.HKCONST.CONSTTYPE == "Individual").OrderBy(x => x.PRSNAME), "PRSCODE", "PRSNAME", cSCODR.PRSCODE);
            return View(cSCODR);
        }

        // GET: CSCODRs/Edit/5
        public ActionResult Edit(string id, string person, DateTime adate)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSCODR cSCODR = db.CSCODRs.Find(MyHtmlHelpers.ConvertByteStrToId(id), person, adate);
            if (cSCODR == null)
            {
                return HttpNotFound();
            }
            Session["CSCODR_ORIG"] = cSCODR;
            ViewBag.PRSCODE = new SelectList(db.CSPRS.Where(x => x.HKCONST.CONSTTYPE == "Individual").OrderBy(x => x.PRSNAME), "PRSCODE", "PRSNAME", cSCODR.PRSCODE);
            return View(cSCODR);
        }

        // POST: CSCODRs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CONO,PRSCODE,ADATE,RDATE,ENDDATE,REM,ROWNO,STAMP")] CSCODR cSCODR)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    bool changed = false;
                    CSCODR csOrig = (CSCODR)Session["CSCODR_ORIG"];
                    if (csOrig.PRSCODE != cSCODR.PRSCODE) { changed = true; }
                    if (csOrig.ADATE != cSCODR.ADATE) { changed = true; }


                    if (changed)
                    {
                        CSCODR csDel = db.CSCODRs.Find(csOrig.CONO, csOrig.PRSCODE, csOrig.ADATE);
                        db.CSCODRs.Remove(csDel);
                        db.CSCODRs.Add(cSCODR);
                    }
                    else
                    {
                        db.Entry(cSCODR).State = EntityState.Modified;
                    }

                    cSCODR.STAMP = cSCODR.STAMP + 1;
                   
                    db.SaveChanges();
                    return new RedirectResult(Url.Action("Edit", "CSCOMSTRs", new { id = MyHtmlHelpers.ConvertIdToByteStr(cSCODR.CONO) }) + "#Director");
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
            cSCODR.CSCOMSTR = db.CSCOMSTRs.Find(cSCODR.CONO);
            cSCODR.CSCODRCHGs = db.CSCODRCHGs.Where(x => x.CONO == cSCODR.CONO && x.PRSCODE == cSCODR.PRSCODE && x.ADATE == cSCODR.ADATE).ToList();
            ViewBag.PRSCODE = new SelectList(db.CSPRS.Where(x => x.HKCONST.CONSTTYPE == "Individual").OrderBy(x => x.PRSNAME), "PRSCODE", "PRSNAME", cSCODR.PRSCODE);
            return View(cSCODR);
        }

        // GET: CSCODRs/Delete/5
        public ActionResult Delete(string id, string person, DateTime adate)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSCODR cSCODR = db.CSCODRs.Find(MyHtmlHelpers.ConvertByteStrToId(id), person, adate);
            if (cSCODR == null)
            {
                return HttpNotFound();
            }
            return View(cSCODR);
        }

        // POST: CSCODRs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id, string person, DateTime adate)
        {
            CSCODR cSCODR = db.CSCODRs.Find(MyHtmlHelpers.ConvertByteStrToId(id), person, adate);
            try
            {
                db.CSCODRs.Remove(cSCODR);
                db.SaveChanges();
                return new RedirectResult(Url.Action("Edit", "CSCOMSTRs", new { id = MyHtmlHelpers.ConvertIdToByteStr(cSCODR.CONO) }) + "#Director");
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
            cSCODR.CSCOMSTR = db.CSCOMSTRs.Find(cSCODR.CONO);
            return View(cSCODR);
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
