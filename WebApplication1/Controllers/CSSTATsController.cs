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
    [Authorize(Roles = "Administrator,CS-SEC,CS-A/C,CS-AS")]
    public class CSSTATsController : Controller
    {
        private ASIDBConnection db = new ASIDBConnection();

        // GET: CSSTATs
        public ActionResult Index()
        {
            return View(db.CSSTATs.OrderBy( x => x.COSTAT).ToList());
        }

        public ActionResult Listing()
        {
            return View(db.CSSTATs.OrderBy(x => x.COSTAT));
        }

        // GET: CSSTATs/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSSTAT cSSTAT = db.CSSTATs.Find((MyHtmlHelpers.ConvertByteStrToId(id)));
            if (cSSTAT == null)
            {
                return HttpNotFound();
            }
            return View(cSSTAT);
        }

        // GET: CSSTATs/Create
        public ActionResult Create()
        {
            CSSTAT cSSTAT = new CSSTAT();
            ViewBag.BlankFile = cSSTAT.blanklist;
            ViewBag.BlankSeal = cSSTAT.blanklist;
            return View();
        }

        // POST: CSSTATs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "COSTAT,BLANKFILE,FILENOPFIX,FILENOFR,FILENOTO,BLANKSEAL,SEALNOFR,SEALNOTO,STAMP")] CSSTAT cSSTAT)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    cSSTAT.STAMP = 0;
                    db.CSSTATs.Add(cSSTAT);
                    db.SaveChanges();
                    return RedirectToAction("Index");

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
            ViewBag.BlankFile = cSSTAT.blanklist;
            ViewBag.BlankSeal = cSSTAT.blanklist;
            return View(cSSTAT);
        }

        // GET: CSSTATs/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSSTAT cSSTAT = db.CSSTATs.Find((MyHtmlHelpers.ConvertByteStrToId(id)));
            if (cSSTAT == null)
            {
                return HttpNotFound();
            }
            ViewBag.BlankFile = cSSTAT.blanklist;
            ViewBag.BlankSeal = cSSTAT.blanklist;
            return View(cSSTAT);
        }

        // POST: CSSTATs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "COSTAT,BLANKFILE,FILENOPFIX,FILENOFR,FILENOTO,BLANKSEAL,SEALNOFR,SEALNOTO,STAMP")] CSSTAT cSSTAT)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    cSSTAT.STAMP = cSSTAT.STAMP + 1;
                    db.Entry(cSSTAT).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
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
            ViewBag.BlankFile = cSSTAT.blanklist;
            ViewBag.BlankSeal = cSSTAT.blanklist;
            return View(cSSTAT);
        }

        // GET: CSSTATs/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSSTAT cSSTAT = db.CSSTATs.Find((MyHtmlHelpers.ConvertByteStrToId(id)));
            if (cSSTAT == null)
            {
                return HttpNotFound();
            }
            return View(cSSTAT);
        }

        // POST: CSSTATs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            CSSTAT cSSTAT = db.CSSTATs.Find((MyHtmlHelpers.ConvertByteStrToId(id)));
            try
            {
               
                db.CSSTATs.Remove(cSSTAT);
                db.SaveChanges();
                return RedirectToAction("Index");
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
            return View(cSSTAT);
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
