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
    public class CSCOPICsController : Controller
    {
        private ASIDBConnection db = new ASIDBConnection();

        // GET: CSCOPICs
        public ActionResult Index()
        {
            var cSCOPICs = db.CSCOPICs.Include(c => c.CSPR);
            return View(cSCOPICs.ToList());
        }

        // GET: CSCOPICs/Details/5
        public ActionResult Details(string id, string person)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSCOPIC cSCOPIC = db.CSCOPICs.Find(MyHtmlHelpers.ConvertByteStrToId(id), person);
            if (cSCOPIC == null)
            {
                return HttpNotFound();
            }
            ViewBag.PRSCODE = new SelectList(db.CSPRS.Where(x => x.HKCONST.CONSTTYPE == "Individual").OrderBy(x => x.PRSNAME), "PRSCODE", "PRSNAME", cSCOPIC.PRSCODE);
            return View(cSCOPIC);
        }

        // GET: CSCOPICs/Create
        public ActionResult Create(string id)
        {
            CSCOPIC cSCOPIC =  new CSCOPIC();
            cSCOPIC.CONO = MyHtmlHelpers.ConvertByteStrToId(id);
            cSCOPIC.CSCOMSTR = db.CSCOMSTRs.Find(cSCOPIC.CONO);
            ViewBag.PRSCODE = new SelectList(db.CSPRS.Where(x => x.HKCONST.CONSTTYPE == "Individual").OrderBy(x => x.PRSNAME), "PRSCODE", "PRSNAME", cSCOPIC.PRSCODE);
            return View(cSCOPIC);
        }

        // POST: CSCOPICs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CONO,PRSCODE,DESIG,REM,STAMP")] CSCOPIC cSCOPIC)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    cSCOPIC.STAMP = 0;
                    db.CSCOPICs.Add(cSCOPIC);
                    db.SaveChanges();
                    return new RedirectResult(Url.Action("Edit", "CSCOMSTRs", new { id = MyHtmlHelpers.ConvertIdToByteStr(cSCOPIC.CONO) }) + "#Contact");
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
            cSCOPIC.CSCOMSTR = db.CSCOMSTRs.Find(cSCOPIC.CONO);
            ViewBag.PRSCODE = new SelectList(db.CSPRS.Where(x => x.HKCONST.CONSTTYPE == "Individual").OrderBy(x => x.PRSNAME), "PRSCODE", "PRSNAME", cSCOPIC.PRSCODE);
            return View(cSCOPIC);
        }

        // GET: CSCOPICs/Edit/5
        public ActionResult Edit(string id, string person)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSCOPIC cSCOPIC = db.CSCOPICs.Find(MyHtmlHelpers.ConvertByteStrToId(id), person);
            if (cSCOPIC == null)
            {
                return HttpNotFound();
            }
            Session["CSCOPIC_ORIG"] = cSCOPIC;
            ViewBag.PRSCODE = new SelectList(db.CSPRS.Where(x => x.HKCONST.CONSTTYPE == "Individual").OrderBy(x => x.PRSNAME), "PRSCODE", "PRSNAME", cSCOPIC.PRSCODE);
            return View(cSCOPIC);
        }

        // POST: CSCOPICs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CONO,PRSCODE,DESIG,REM,STAMP")] CSCOPIC cSCOPIC)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    cSCOPIC.STAMP = cSCOPIC.STAMP + 1;
                    bool changed = false;
                    CSCOPIC csOrig = (CSCOPIC)Session["CSCOPIC_ORIG"];
                    if (csOrig.PRSCODE != cSCOPIC.PRSCODE) { changed = true; }

                    if (changed)
                    {
                        CSCOPIC csDel = db.CSCOPICs.Find(csOrig.CONO, csOrig.PRSCODE);
                        db.CSCOPICs.Remove(csDel);
                        db.CSCOPICs.Add(cSCOPIC);
                    }
                    else
                    {
                        db.Entry(cSCOPIC).State = EntityState.Modified;
                    }
                  
                    db.SaveChanges();
                    return new RedirectResult(Url.Action("Edit", "CSCOMSTRs", new { id = MyHtmlHelpers.ConvertIdToByteStr(cSCOPIC.CONO) }) + "#Contact");
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
                    } else
                    {
                        ModelState.AddModelError(string.Empty, updateException.Message);
                    }
                }
            }
            cSCOPIC.CSCOMSTR = db.CSCOMSTRs.Find(cSCOPIC.CONO);
            ViewBag.PRSCODE = new SelectList(db.CSPRS.Where(x => x.HKCONST.CONSTTYPE == "Individual").OrderBy(x => x.PRSNAME), "PRSCODE", "PRSNAME", cSCOPIC.PRSCODE);
            return View(cSCOPIC);
        }

        // GET: CSCOPICs/Delete/5
        public ActionResult Delete(string id, string person)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSCOPIC cSCOPIC = db.CSCOPICs.Find(MyHtmlHelpers.ConvertByteStrToId(id), person);
            if (cSCOPIC == null)
            {
                return HttpNotFound();
            }
            cSCOPIC.CSCOMSTR = db.CSCOMSTRs.Find(cSCOPIC.CONO);
            return View(cSCOPIC);
        }

        // POST: CSCOPICs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id, string person)
        {
            CSCOPIC cSCOPIC = db.CSCOPICs.Find(MyHtmlHelpers.ConvertByteStrToId(id), person);
            try
            {
                db.CSCOPICs.Remove(cSCOPIC);
                db.SaveChanges();
                return new RedirectResult(Url.Action("Edit", "CSCOMSTRs", new { id = MyHtmlHelpers.ConvertIdToByteStr(cSCOPIC.CONO) }) + "#Contact");
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
            cSCOPIC.CSCOMSTR = db.CSCOMSTRs.Find(cSCOPIC.CONO);
            return View(cSCOPIC);
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
