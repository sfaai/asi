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
    public class CSCASEsController : Controller
    {
        private ASIDBConnection db = new ASIDBConnection();

        // GET: CSCASEs
        public ActionResult Index()
        {
            return View(db.CSCASEs.OrderBy(n => n.CASECODE).ToList());
        }

        public ActionResult Listing()
        {
            SACOMP profileRec = db.SACOMPs.FirstOrDefault();
            ViewBag.Company = profileRec.CONAME.ToUpper() + " (" + profileRec.COREGNO + ") ";

            return View(db.CSCASEs.OrderBy(n => n.CASECODE));
        }

        // GET: CSCASEs/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSCASE cSCASE = db.CSCASEs.Find(MyHtmlHelpers.ConvertByteStrToId(id));
            if (cSCASE == null)
            {
                return HttpNotFound();
            }
            return View(cSCASE);
        }

        // GET: CSCASEs/Create
        public ActionResult Create()
        {
            CSCASE curRec = new CSCASE();
            curRec.STAMP = 0;
            return View(curRec);
        }

        // POST: CSCASEs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CASECODE,CASEDESC,MAPSFIX,STAMP")] CSCASE cSCASE)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.CSCASEs.Add(cSCASE);
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

            return View(cSCASE);
        }

        // GET: CSCASEs/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            string sid = MyHtmlHelpers.ConvertByteStrToId(id);
            CSCASE cSCASE = db.CSCASEs.Find(sid);
            if (cSCASE == null)
            {
                return HttpNotFound();
            }
            return View(cSCASE);
        }

        // POST: CSCASEs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CASECODE,CASEDESC,MAPSFIX,STAMP")] CSCASE cSCASE)
        {
            if (ModelState.IsValid)
            {
                ASIDBConnection newdb = new ASIDBConnection();
                try
                {
                    CSCASE curRec = newdb.CSCASEs.Find(cSCASE.CASECODE);
                    if (curRec.STAMP == cSCASE.STAMP)
                    {
                        cSCASE.STAMP = cSCASE.STAMP + 1;
                        db.Entry(cSCASE).State = EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("Index");
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
            return View(cSCASE);
        }

        // GET: CSCASEs/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSCASE cSCASE = db.CSCASEs.Find(MyHtmlHelpers.ConvertByteStrToId(id));
            if (cSCASE == null)
            {
                return HttpNotFound();
            }
            return View(cSCASE);
        }

        // POST: CSCASEs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            CSCASE cSCASE = db.CSCASEs.Find(MyHtmlHelpers.ConvertByteStrToId(id));
            try
            {
                db.CSCASEs.Remove(cSCASE);
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
            return View(cSCASE);
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
