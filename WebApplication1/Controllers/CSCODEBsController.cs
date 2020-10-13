using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Utility;
using System.Data.Entity.Validation;
using System.Data.Entity.Core;
using System.Data.Entity.Infrastructure;
using FirebirdSql.Data.Client;
using FirebirdSql.Data.FirebirdClient;

namespace WebApplication1.Controllers
{
    public class CSCODEBsController : Controller
    {
        private ASIDBConnection db = new ASIDBConnection();

        // GET: CSCODEBs
        public ActionResult Index()
        {
            var cSCODEBs = db.CSCODEBs.Include(c => c.CSPR);
            return View(cSCODEBs.ToList());
        }

        // GET: CSCODEBs/Details/5
        public ActionResult Details(string id, string refno)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSCODEB cSCODEB = db.CSCODEBs.Find(MyHtmlHelpers.ConvertByteStrToId(id), refno);
            if (cSCODEB == null)
            {
                return HttpNotFound();
            }
            ViewBag.PRSCODE = new SelectList(db.CSPRS.OrderBy(x => x.PRSNAME), "PRSCODE", "PRSNAME", cSCODEB.PRSCODE);


            return View(cSCODEB);
        }

        // GET: CSCODEBs/Create
        public ActionResult Create(string id)
        {
            CSCODEB cSCODEB = new CSCODEB();
            cSCODEB.CONO = MyHtmlHelpers.ConvertByteStrToId(id);
            cSCODEB.REFDATE = DateTime.Today;
            cSCODEB.DEEDDATE = DateTime.Today;

            ViewBag.PRSCODE = new SelectList(db.CSPRS.OrderBy(x => x.PRSNAME), "PRSCODE", "PRSNAME");
            
            cSCODEB.CSCOMSTR = db.CSCOMSTRs.Find(cSCODEB.CONO);
            return View(cSCODEB);
        }

        // POST: CSCODEBs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CONO,REFNO,REFDATE,DEEDDATE,PRSCODE,SECUREAMT,ISSUEAMT,DEBCOND,DEBINFO,DEBINFOStr,REM,STAMP")] CSCODEB cSCODEB)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    cSCODEB.STAMP = 0;
                    db.CSCODEBs.Add(cSCODEB);
                    db.SaveChanges();
                    return new RedirectResult(Url.Action("Edit", "CSCOMSTRs", new { id = MyHtmlHelpers.ConvertIdToByteStr(cSCODEB.CONO) }) + "#Debenture");
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

            ViewBag.PRSCODE = new SelectList(db.CSPRS.OrderBy(x => x.PRSNAME), "PRSCODE", "PRSNAME", cSCODEB.PRSCODE);

            cSCODEB.CSCOMSTR = db.CSCOMSTRs.Find(cSCODEB.CONO);
            return View(cSCODEB);
        }

        // GET: CSCODEBs/Edit/5
        public ActionResult Edit(string id, string refno)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSCODEB cSCODEB = db.CSCODEBs.Find(MyHtmlHelpers.ConvertByteStrToId(id), refno);
            if (cSCODEB == null)
            {
                return HttpNotFound();
            }
            Session["CSCODEB_ORIG"] = cSCODEB;
            ViewBag.PRSCODE = new SelectList(db.CSPRS.OrderBy(x => x.PRSNAME), "PRSCODE", "PRSNAME", cSCODEB.PRSCODE);
            return View(cSCODEB);
        }

        // POST: CSCODEBs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CONO,REFNO,REFDATE,DEEDDATE,PRSCODE,SECUREAMT,ISSUEAMT,DEBCOND,DEBINFO,DEBINFOStr,REM,STAMP")] CSCODEB cSCODEB)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    cSCODEB.STAMP = cSCODEB.STAMP + 1;
                    bool changed = false;
                    CSCODEB csOrig = (CSCODEB)Session["CSCODEB_ORIG"];
                    if (csOrig.REFNO != cSCODEB.REFNO) { changed = true; }

                    if (changed)
                    {
                        CSCODEB csDel = db.CSCODEBs.Find(csOrig.CONO, csOrig.REFNO);
                        db.CSCODEBs.Remove(csDel);
                        db.CSCODEBs.Add(cSCODEB);
                    }
                    else
                    {
                        db.Entry(cSCODEB).State = EntityState.Modified;
                    }
                    db.SaveChanges();
                    return new RedirectResult(Url.Action("Edit", "CSCOMSTRs", new { id = MyHtmlHelpers.ConvertIdToByteStr(cSCODEB.CONO) }) + "#Debenture");
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
            ViewBag.PRSCODE = new SelectList(db.CSPRS.OrderBy(x => x.PRSNAME), "PRSCODE", "PRSNAME", cSCODEB.PRSCODE);

            cSCODEB.CSCOMSTR = db.CSCOMSTRs.Find(cSCODEB.CONO);
            return View(cSCODEB);
        }

        // GET: CSCODEBs/Delete/5
        public ActionResult Delete(string id, string refno)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSCODEB cSCODEB = db.CSCODEBs.Find(MyHtmlHelpers.ConvertByteStrToId(id), refno);
            if (cSCODEB == null)
            {
                return HttpNotFound();
            }
            return View(cSCODEB);
        }

        // POST: CSCODEBs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id, string refno)
        {
            CSCODEB cSCODEB = db.CSCODEBs.Find(MyHtmlHelpers.ConvertByteStrToId(id), refno);
            try
            {
                db.CSCODEBs.Remove(cSCODEB);
                db.SaveChanges();
                return new RedirectResult(Url.Action("Edit", "CSCOMSTRs", new { id = MyHtmlHelpers.ConvertIdToByteStr(cSCODEB.CONO) }) + "#Debenture");
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

            return View(cSCODEB);
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
