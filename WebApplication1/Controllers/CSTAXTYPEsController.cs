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
    public class CSTAXTYPEsController : Controller
    {
        private ASIDBConnection db = new ASIDBConnection();

        // GET: CSTAXTYPEs
        public ActionResult Index()
        {
            return View(db.CSTAXTYPEs.ToList());
        }

        public ActionResult Listing()
        {
            SACOMP profileRec = db.SACOMPs.FirstOrDefault();
            ViewBag.Company = profileRec.CONAME.ToUpper() + " (" + profileRec.COREGNO + ") ";

            return View(db.CSTAXTYPEs);
        }

        // GET: CSTAXTYPEs/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSTAXTYPE cSTAXTYPE = db.CSTAXTYPEs.Find(MyHtmlHelpers.ConvertByteStrToId(id));
            if (cSTAXTYPE == null)
            {
                return HttpNotFound();
            }
            return View(cSTAXTYPE);
        }

        // GET: CSTAXTYPEs/Create
        public ActionResult Create()
        {
            CSTAXTYPE cSTAXTYPE = new CSTAXTYPE();
            cSTAXTYPE.STAMP = 1;
            cSTAXTYPE.ACTIVE_FLAG = "N";
            return View(cSTAXTYPE);
        }

        // POST: CSTAXTYPEs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TAXCODE,TAXDESC,TAXTYPE,TAXRCODE,TAXRATE,EFFECTIVE_START,EFFECTIVE_END,ACTIVE_FLAG,STAMP")] CSTAXTYPE cSTAXTYPE)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.CSTAXTYPEs.Add(cSTAXTYPE);
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

            return View(cSTAXTYPE);
        }

        // GET: CSTAXTYPEs/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSTAXTYPE cSTAXTYPE = db.CSTAXTYPEs.Find(MyHtmlHelpers.ConvertByteStrToId(id));
            if (cSTAXTYPE == null)
            {
                return HttpNotFound();
            }
            return View(cSTAXTYPE);
        }

        // POST: CSTAXTYPEs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TAXCODE,TAXDESC,TAXTYPE,TAXRCODE,TAXRATE,EFFECTIVE_START,EFFECTIVE_END,ACTIVE_FLAG,STAMP")] CSTAXTYPE cSTAXTYPE)
        {
            if (ModelState.IsValid)
            {try
                {
                    db.Entry(cSTAXTYPE).State = EntityState.Modified;
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
            return View(cSTAXTYPE);
        }

        // GET: CSTAXTYPEs/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSTAXTYPE cSTAXTYPE = db.CSTAXTYPEs.Find(MyHtmlHelpers.ConvertByteStrToId(id));
            if (cSTAXTYPE == null)
            {
                return HttpNotFound();
            }
            return View(cSTAXTYPE);
        }

        // POST: CSTAXTYPEs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            CSTAXTYPE cSTAXTYPE = db.CSTAXTYPEs.Find(MyHtmlHelpers.ConvertByteStrToId(id));
            try
            {
                db.CSTAXTYPEs.Remove(cSTAXTYPE);
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
            return View(cSTAXTYPE);
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
