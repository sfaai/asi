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
    public class CSCOAGTsController : Controller
    {
        private ASIDBConnection db = new ASIDBConnection();

        // GET: CSCOAGTs
        public ActionResult Index()
        {
            var cSCOAGTs = db.CSCOAGTs.Include(c => c.CSPR);
            return View(cSCOAGTs.ToList());
        }

        public PartialViewResult EditRegChg( string id, string person,DateTime adate)
        {

            string sid = MyHtmlHelpers.ConvertByteStrToId(id);
            ViewBag.Parent = sid;
            ViewBag.CONO = sid;
            ViewBag.PRSCODE = person;
            ViewBag.ADATE = adate;

            var cSCOAGTCHG = db.CSCOAGTCHGs.Where(x => x.CONO == sid && x.PRSCODE == person && x.ADATE == adate).ToList();
            return PartialView("Partial/PartialChg", cSCOAGTCHG);
        }

        // GET: CSCOAGTs/Details/5
        public ActionResult Details(string id, string person, DateTime adate)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSCOAGT cSCOAGT = db.CSCOAGTs.Find(MyHtmlHelpers.ConvertByteStrToId(id), person, adate);
            if (cSCOAGT == null)
            {
                return HttpNotFound();
            }
            
            ViewBag.PRSCODE = new SelectList(db.CSPRS.OrderBy(x => x.PRSNAME), "PRSCODE", "PRSNAME", cSCOAGT.PRSCODE);
            return View(cSCOAGT);
        }

        // GET: CSCOAGTs/Create
        public ActionResult Create(string id)
        {
            ViewBag.Parent = MyHtmlHelpers.ConvertByteStrToId(id);
            string sid = ViewBag.Parent;

            CSCOAGT curRec = new CSCOAGT();
            curRec.CONO = sid;
            curRec.ADATE = DateTime.Today;
            curRec.ENDDATE = new DateTime(3000, 1, 1);
            curRec.CSCOMSTR = db.CSCOMSTRs.Find(curRec.CONO);
            ViewBag.PRSCODE = new SelectList(db.CSPRS.OrderBy(x => x.PRSNAME), "PRSCODE", "PRSNAME", curRec.PRSCODE);
            return View(curRec);
        }

        // POST: CSCOAGTs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CONO,PRSCODE,ADATE,RDATE,ENDDATE,REM,ROWNO,STAMP")] CSCOAGT cSCOAGT)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    int lastRowNo = 0;
                    try
                    {
                        lastRowNo = db.CSCOAGTs.Where(m => m.CONO == cSCOAGT.CONO).Max(n => n.ROWNO);
                    }
                    catch (Exception e) { lastRowNo = 0; }
                    finally { };


                    cSCOAGT.STAMP = 0;
                    cSCOAGT.ROWNO = lastRowNo + 1;
                    db.CSCOAGTs.Add(cSCOAGT);
                    db.SaveChanges();
                    return new RedirectResult(Url.Action("Edit", "CSCOMSTRs", new { id = MyHtmlHelpers.ConvertIdToByteStr(cSCOAGT.CONO) }) + "#Agent");
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
            cSCOAGT.CSCOMSTR = db.CSCOMSTRs.Find(cSCOAGT.CONO);         
            ViewBag.PRSCODE = new SelectList(db.CSPRS.OrderBy(x => x.PRSNAME), "PRSCODE", "PRSNAME", cSCOAGT.PRSCODE);
            return View(cSCOAGT);
        }

        // GET: CSCOAGTs/Edit/5
        public ActionResult Edit(string id, string person, DateTime adate)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSCOAGT cSCOAGT = db.CSCOAGTs.Find(MyHtmlHelpers.ConvertByteStrToId(id), person, adate);
            if (cSCOAGT == null)
            {
                return HttpNotFound();
            }
            Session["CSCOAGT_ORIG"] = cSCOAGT;
            ViewBag.PRSCODE = new SelectList(db.CSPRS.OrderBy(x => x.PRSNAME), "PRSCODE", "PRSNAME", cSCOAGT.PRSCODE);
            return View(cSCOAGT);
        }

        // POST: CSCOAGTs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CONO,PRSCODE,ADATE,RDATE,ENDDATE,REM,ROWNO,STAMP")] CSCOAGT cSCOAGT)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    bool changed = false;
                    CSCOAGT csOrig = (CSCOAGT)Session["CSCOAGT_ORIG"];
                    if (csOrig.PRSCODE != cSCOAGT.PRSCODE) { changed = true; }
                    if (csOrig.ADATE != cSCOAGT.ADATE) { changed = true; }


                    if (changed)
                    {
                        CSCOAGT csDel = db.CSCOAGTs.Find(csOrig.CONO, csOrig.PRSCODE, csOrig.ADATE);
                        db.CSCOAGTs.Remove(csDel);
                        db.CSCOAGTs.Add(cSCOAGT);
                    }
                    else
                    {
                        db.Entry(cSCOAGT).State = EntityState.Modified;
                    }

                    cSCOAGT.STAMP = cSCOAGT.STAMP + 1;
                    db.SaveChanges();
                    return new RedirectResult(Url.Action("Edit", "CSCOMSTRs", new { id = MyHtmlHelpers.ConvertIdToByteStr(cSCOAGT.CONO) }) + "#Agent");
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
            cSCOAGT.CSCOMSTR = db.CSCOMSTRs.Find(cSCOAGT.CONO);
            cSCOAGT.CSCOAGTCHGs = db.CSCOAGTCHGs.Where(x => x.CONO == cSCOAGT.CONO && x.PRSCODE == cSCOAGT.PRSCODE && x.ADATE == cSCOAGT.ADATE).ToList();
            ViewBag.PRSCODE = new SelectList(db.CSPRS.OrderBy(x => x.PRSNAME), "PRSCODE", "PRSNAME", cSCOAGT.PRSCODE);
            return View(cSCOAGT);
        }

        // GET: CSCOAGTs/Delete/5
        public ActionResult Delete(string id, string person, DateTime adate)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSCOAGT cSCOAGT = db.CSCOAGTs.Find(MyHtmlHelpers.ConvertByteStrToId(id), person, adate);
            if (cSCOAGT == null)
            {
                return HttpNotFound();
            }
            return View(cSCOAGT);
        }

        // POST: CSCOAGTs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id, string person, DateTime adate)
        {
            CSCOAGT cSCOAGT = db.CSCOAGTs.Find(MyHtmlHelpers.ConvertByteStrToId(id), person, adate);
            try
            {
              
                db.CSCOAGTs.Remove(cSCOAGT);
                db.SaveChanges();
                return new RedirectResult(Url.Action("Edit", "CSCOMSTRs", new { id = MyHtmlHelpers.ConvertIdToByteStr(cSCOAGT.CONO) }) + "#Agent");
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
            return View(cSCOAGT);
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
