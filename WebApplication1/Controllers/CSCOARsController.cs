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
    public class CSCOARsController : Controller
    {
        private ASIDBConnection db = new ASIDBConnection();

        // GET: CSCOARs
        public ActionResult Index()
        {
            return View(db.CSCOARs.ToList());
        }

        // GET: CSCOARs/Details/5
        public ActionResult Details(string id, int seq)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSCOAR cSCOAR = db.CSCOARs.Find(MyHtmlHelpers.ConvertByteStrToId(id), seq);
            if (cSCOAR == null)
            {
                return HttpNotFound();
            }
            return View(cSCOAR);
        }

        // GET: CSCOARs/Create
        public ActionResult Create(string id)
        {
            ViewBag.Parent = MyHtmlHelpers.ConvertByteStrToId(id);
            string sid = ViewBag.Parent;

            CSCOAR lastRec = db.CSCOARs.Where(m => m.CONO == sid).OrderByDescending(n => n.ARNO).FirstOrDefault();
            CSCOAR curRec = null;
            if (lastRec != null)
            {
                curRec = createNextYearRecord( lastRec);
            }
            else
            {
                curRec = new CSCOAR();
                curRec.ARNO = 1;
                curRec.CONO = sid;
                curRec.STAMP = 0;
            };
            curRec.CSCOMSTR = db.CSCOMSTRs.Find(curRec.CONO);
            return View(curRec);
           // return View();
        }

        // POST: CSCOARs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CONO,ARNO,LASTAR,ARTOFILE,FILEDAR,REMINDER1,REM,STAMP,SUBMITAR")] CSCOAR cSCOAR)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (cSCOAR.FILEDAR != null) // create next year record if AR is filed
                    {
                        CSCOAR csRec = createNextYearRecord(cSCOAR);
                        db.CSCOARs.Add(csRec);
                    }

                    db.CSCOARs.Add(cSCOAR);
                    db.SaveChanges();
                    //return RedirectToAction("Details", "CSCOMSTRs", new { id = MyHtmlHelpers.ConvertIdToByteStr(cSCOAR.CONO) });
                    return new RedirectResult(Url.Action("Details", "CSCOMSTRs", new { id = MyHtmlHelpers.ConvertIdToByteStr(cSCOAR.CONO) }) + "#Annual");
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
            cSCOAR.CSCOMSTR = db.CSCOMSTRs.Find(cSCOAR.CONO);
            return View(cSCOAR);
        }

        private CSCOAR createNextYearRecord(CSCOAR curRec)
        {
            string sid = curRec.CONO;
            CSCOMSTR cSCOMSTR = db.CSCOMSTRs.Find(sid);

            DateTime indate = cSCOMSTR.INCDATE; // get incorporate date to determine AR due
            int yy = indate.Year;
            int mm = indate.Month;
            int dd = indate.Day;

            CSCOAR newRec = new CSCOAR();
            newRec.ARNO = (short) (curRec.ARNO + 1); // WARNING assuming the next ARNO is available
            newRec.LASTAR = curRec.FILEDAR;
            newRec.CONO = curRec.CONO;
            int lyy = newRec.LASTAR?.Year ?? yy;
            lyy = lyy + 1; // add 1 year to last year File AR Date

            try
            {
                newRec.ARTOFILE = new DateTime(lyy, mm, dd);
            }
            catch
            {
                //only reason to fail is for february 29 and non leap year
                dd = 28;
                newRec.ARTOFILE = new DateTime(lyy, mm, dd);
            }
            dd = 1; // reminder is set to 1 month prior to AR Due Date and on the 1st
            if (mm == 1) { mm = 12; lyy = lyy - 1; } else { mm = mm - 1; };
            newRec.REMINDER1 = new DateTime(lyy, mm, dd);
            newRec.STAMP = 1;
            return newRec;
        }

        // GET: CSCOARs/Edit/5
        public ActionResult Edit(string id, int seq)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSCOAR cSCOAR = db.CSCOARs.Find(MyHtmlHelpers.ConvertByteStrToId(id), seq);
            if (cSCOAR == null)
            {
                return HttpNotFound();
            }
            return View(cSCOAR);
        }

        // POST: CSCOARs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CONO,ARNO,LASTAR,ARTOFILE,FILEDAR,REMINDER1,REM,STAMP,SUBMITAR")] CSCOAR cSCOAR)
        {
            if (ModelState.IsValid)
            {
                ASIDBConnection newdb = new ASIDBConnection();
                try
                {
                    CSCOAR curRec = newdb.CSCOARs.Find(cSCOAR.CONO, cSCOAR.ARNO);
                    if (curRec.STAMP == cSCOAR.STAMP)
                    {
                        cSCOAR.STAMP = cSCOAR.STAMP + 1;
                        db.Entry(cSCOAR).State = EntityState.Modified;

                        if (cSCOAR.FILEDAR != null) // create next year record if AR is filed
                        {
                            string sid = cSCOAR.CONO;
                            CSCOAR lastRec = db.CSCOARs.Where(m => m.CONO == sid).OrderByDescending(n => n.ARNO).FirstOrDefault();

                            if (cSCOAR.ARNO == lastRec.ARNO) // add next year record only if editing the last record
                            {
                                CSCOAR csRec = createNextYearRecord(cSCOAR);
                                db.CSCOARs.Add(csRec);
                            }
                        }

                        db.SaveChanges();
                        //return RedirectToAction("Details", "CSCOMSTRs", new { id = MyHtmlHelpers.ConvertIdToByteStr(cSCOAR.CONO) });
                        return new RedirectResult(Url.Action("Details", "CSCOMSTRs", new { id = MyHtmlHelpers.ConvertIdToByteStr(cSCOAR.CONO) }) + "#Annual");
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
            cSCOAR.CSCOMSTR = db.CSCOMSTRs.Find(cSCOAR.CONO);
            return View(cSCOAR);
        }

        // GET: CSCOARs/Delete/5
        public ActionResult Delete(string id, int seq)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSCOAR cSCOAR = db.CSCOARs.Find(MyHtmlHelpers.ConvertByteStrToId(id), seq);
            if (cSCOAR == null)
            {
                return HttpNotFound();
            }
            return View(cSCOAR);
        }

        // POST: CSCOARs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id, int seq)
        {
            CSCOAR cSCOAR = db.CSCOARs.Find(MyHtmlHelpers.ConvertByteStrToId(id), seq);
            try
            {
                db.CSCOARs.Remove(cSCOAR);
                db.SaveChanges();
                //return RedirectToAction("Details", "CSCOMSTRs", new { id = MyHtmlHelpers.ConvertIdToByteStr(cSCOAR.CONO) });
                return new RedirectResult(Url.Action("Details", "CSCOMSTRs", new { id = MyHtmlHelpers.ConvertIdToByteStr(cSCOAR.CONO) }) + "#Annual");
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
            cSCOAR.CSCOMSTR = db.CSCOMSTRs.Find(cSCOAR.CONO);
            return View(cSCOAR);
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
