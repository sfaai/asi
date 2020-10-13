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
    public class CSCOAGMsController : Controller
    {
        private ASIDBConnection db = new ASIDBConnection();

        // GET: CSCOAGMs
        public ActionResult Index()
        {
            return View(db.CSCOAGMs.ToList());
        }

        // GET: CSCOAGMs/Details/5
        public ActionResult Details(string id, int seq)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSCOAGM cSCOAGM = db.CSCOAGMs.Find(MyHtmlHelpers.ConvertByteStrToId(id), seq);
            if (cSCOAGM == null)
            {
                return HttpNotFound();
            }
            return View(cSCOAGM);
        }

        // GET: CSCOAGMs/Create
        public ActionResult Create(string id)
        {
            ViewBag.Parent = MyHtmlHelpers.ConvertByteStrToId(id);
            string sid = ViewBag.Parent;
            CSCOAGM lastRec = db.CSCOAGMs.Where(m => m.CONO == sid).OrderByDescending(n => n.AGMNO).FirstOrDefault();

            CSCOAGM curRec = new CSCOAGM();

            if (lastRec != null)
            {
                curRec = createNextYearRecord(lastRec);
            }
            else
            {
                curRec.CONO = sid;
                curRec.AGMNO = 1;
                curRec.STAMP = 0;
            };

            curRec.CSCOMSTR = db.CSCOMSTRs.Find(curRec.CONO);
            return View(curRec);
        }

        // POST: CSCOAGMs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CONO,AGMNO,LASTFYE,FYETOFILE,FILEDFYE,AGMLAST,AGMEXT,REMINDER1,REMINDER2,REMINDER3,AGMDATE,AGMFILED,REM,STAMP,CIRCDATE")] CSCOAGM cSCOAGM)
        {
            if (ModelState.IsValid)
            {
                cSCOAGM.STAMP = 0;
                try
                {


                    db.CSCOAGMs.Add(cSCOAGM);
                    db.SaveChanges();
                    //return RedirectToAction("Details", "CSCOMSTRs", new { id = MyHtmlHelpers.ConvertIdToByteStr(cSCOAGM.CONO) });
                    return new RedirectResult(Url.Action("Details", "CSCOMSTRs", new { id = MyHtmlHelpers.ConvertIdToByteStr(cSCOAGM.CONO) }) + "#AGM");
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

            cSCOAGM.CSCOMSTR = db.CSCOMSTRs.Find(cSCOAGM.CONO);
            return View(cSCOAGM);
        }

        // GET: CSCOAGMs/Edit/5
        public ActionResult Edit(string id, int seq)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSCOAGM cSCOAGM = db.CSCOAGMs.Find(MyHtmlHelpers.ConvertByteStrToId(id), seq);
            if (cSCOAGM == null)
            {
                return HttpNotFound();
            }
            return View(cSCOAGM);
        }

        private CSCOAGM createNextYearRecord(CSCOAGM curRec)
        {
            string sid = curRec.CONO;

            CSCOAGM newRec = new CSCOAGM();
            newRec.AGMNO = (short)(curRec.AGMNO + 1); // WARNING assuming the next ARNO is available
            newRec.CONO = curRec.CONO;
            newRec.STAMP = 1;
            newRec.LASTFYE = curRec.FILEDFYE;
            newRec.FYETOFILE = curRec.FILEDFYE?.AddYears(1);
            newRec.AGMLAST = newRec.FYETOFILE?.AddMonths(6) < curRec.AGMDATE?.AddMonths(15) ? newRec.FYETOFILE?.AddMonths(6) : curRec.AGMDATE?.AddMonths(15);
            newRec.REMINDER1 = newRec.FYETOFILE?.AddMonths(-1);
            newRec.REMINDER2 = newRec.FYETOFILE?.AddMonths(3);
            newRec.REMINDER3 = newRec.FYETOFILE?.AddMonths(5);

            //reminders are always set on the 1st day of the month
            newRec.REMINDER1 = new DateTime(newRec.REMINDER1?.Year ?? 1, newRec.REMINDER1?.Month ?? 1, 1);
            newRec.REMINDER2 = new DateTime(newRec.REMINDER2?.Year ?? 1, newRec.REMINDER2?.Month ?? 1, 1);
            newRec.REMINDER3 = new DateTime(newRec.REMINDER3?.Year ?? 1, newRec.REMINDER3?.Month ?? 1, 1);

            return newRec;
        }

        // POST: CSCOAGMs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CONO,AGMNO,LASTFYE,FYETOFILE,FILEDFYE,AGMLAST,AGMEXT,REMINDER1,REMINDER2,REMINDER3,AGMDATE,AGMFILED,REM,STAMP,CIRCDATE")] CSCOAGM cSCOAGM)
        {
            if (ModelState.IsValid)
            {
                ASIDBConnection newdb = new ASIDBConnection();
                try
                {
                    CSCOAGM curRec = newdb.CSCOAGMs.Find(cSCOAGM.CONO, cSCOAGM.AGMNO);
                    if (curRec.STAMP == cSCOAGM.STAMP)
                    {
                        cSCOAGM.STAMP = cSCOAGM.STAMP + 1;
                        db.Entry(cSCOAGM).State = EntityState.Modified;


                        if (cSCOAGM.FILEDFYE != null) // create next year record if AR is filed
                        {
                            string sid = cSCOAGM.CONO;
                            CSCOAGM lastRec = db.CSCOAGMs.Where(m => m.CONO == sid).OrderByDescending(n => n.AGMNO).FirstOrDefault();

                            if (cSCOAGM.AGMNO == lastRec.AGMNO) // add next year record only if editing the last record
                            {
                                CSCOAGM csRec = createNextYearRecord(cSCOAGM);
                                db.CSCOAGMs.Add(csRec);
                            }
                        }

                        db.SaveChanges();
                        //return RedirectToAction("Details", "CSCOMSTRs", new { id = MyHtmlHelpers.ConvertIdToByteStr(cSCOAGM.CONO) });
                        return new RedirectResult(Url.Action("Details", "CSCOMSTRs", new { id = MyHtmlHelpers.ConvertIdToByteStr(cSCOAGM.CONO) }) + "#AGM");
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
            cSCOAGM.CSCOMSTR = db.CSCOMSTRs.Find(cSCOAGM.CONO);
            return View(cSCOAGM);
        }

        // GET: CSCOAGMs/Delete/5
        public ActionResult Delete(string id, int seq)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSCOAGM cSCOAGM = db.CSCOAGMs.Find(MyHtmlHelpers.ConvertByteStrToId(id), seq);
            if (cSCOAGM == null)
            {
                return HttpNotFound();
            }
            return View(cSCOAGM);
        }

        // POST: CSCOAGMs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id, int seq)
        {
            CSCOAGM cSCOAGM = db.CSCOAGMs.Find(MyHtmlHelpers.ConvertByteStrToId(id), seq);
            try
            {
                db.CSCOAGMs.Remove(cSCOAGM);
                db.SaveChanges();
                //return RedirectToAction("Details", "CSCOMSTRs", new { id = MyHtmlHelpers.ConvertIdToByteStr(cSCOAGM.CONO) });
                return new RedirectResult(Url.Action("Details", "CSCOMSTRs", new { id = MyHtmlHelpers.ConvertIdToByteStr(cSCOAGM.CONO) }) + "#AGM");
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
            cSCOAGM.CSCOMSTR = db.CSCOMSTRs.Find(cSCOAGM.CONO);
            return View(cSCOAGM);
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
