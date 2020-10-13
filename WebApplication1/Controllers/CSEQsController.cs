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
    public class CSEQsController : Controller
    {
        private ASIDBConnection db = new ASIDBConnection();

        // GET: CSEQs
        public ActionResult Index()
        {
            return View(db.CSEQs.OrderBy(x => x.EQCODE).ToList());
        }

        public ActionResult Listing()
        {
            SACOMP profileRec = db.SACOMPs.FirstOrDefault();
            ViewBag.Company = profileRec.CONAME.ToUpper() + " (" + profileRec.COREGNO + ") ";

            return View(db.CSEQs.OrderBy(x => x.EQCODE));
        }

        // GET: CSEQs/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSEQ cSEQ = db.CSEQs.Find(MyHtmlHelpers.ConvertByteStrToId(id));
            if (cSEQ == null)
            {
                return HttpNotFound();
            }
            return View(cSEQ);
        }

        // GET: CSEQs/Create
        public ActionResult Create()
        {
            CSEQ cSEQ = new CSEQ();
            ViewBag.EQCAT = cSEQ.category;
            return View();
        }

        // POST: CSEQs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EQCODE,EQDESC,EQCAT,STAMP")] CSEQ cSEQ)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    cSEQ.STAMP = 0;
                    db.CSEQs.Add(cSEQ);
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
            ViewBag.EQCAT = cSEQ.category;
            return View(cSEQ);
        }

        // GET: CSEQs/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSEQ cSEQ = db.CSEQs.Find(MyHtmlHelpers.ConvertByteStrToId(id));
            if (cSEQ == null)
            {
                return HttpNotFound();
            }
            ViewBag.EQCAT = cSEQ.category;
            return View(cSEQ);
        }

        // POST: CSEQs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EQCODE,EQDESC,EQCAT,STAMP")] CSEQ cSEQ)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    cSEQ.STAMP = cSEQ.STAMP + 1;
                    db.Entry(cSEQ).State = EntityState.Modified;
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
            ViewBag.EQCAT = cSEQ.category;
            return View(cSEQ);
        }

        // GET: CSEQs/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSEQ cSEQ = db.CSEQs.Find(MyHtmlHelpers.ConvertByteStrToId(id));
            if (cSEQ == null)
            {
                return HttpNotFound();
            }
            return View(cSEQ);
        }

        // POST: CSEQs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            CSEQ cSEQ = db.CSEQs.Find(MyHtmlHelpers.ConvertByteStrToId(id));
            try
            {
                db.CSEQs.Remove(cSEQ);
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

            return View(cSEQ);
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
