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
    [Authorize(Roles ="Administrator,CS-SEC,CS-A/C,CS-AS")]
    public class CSGRPMsController : Controller
    {
        private ASIDBConnection db = new ASIDBConnection();

        // GET: CSGRPMs
        public ActionResult Index()
        {
            return View(db.CSGRPMs.OrderBy(x => x.GRPCODE).ToList());
        }

        public ActionResult Listing()
        {
            SACOMP profileRec = db.SACOMPs.FirstOrDefault();
            ViewBag.Company = profileRec.CONAME.ToUpper() + " (" + profileRec.COREGNO + ") ";

            return View(db.CSGRPMs.OrderBy(x => x.GRPCODE));
        }

        // GET: CSGRPMs/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSGRPM cSGRPM = db.CSGRPMs.Find(MyHtmlHelpers.ConvertByteStrToId(id));
            if (cSGRPM == null)
            {
                return HttpNotFound();
            }
            return View(cSGRPM);
        }

        // GET: CSGRPMs/Create
        public ActionResult Create()
        {
            CSGRPM cSGRPM = new CSGRPM();
            cSGRPM.STAMP = 0;
            return View(cSGRPM);
        }

        // POST: CSGRPMs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "GRPCODE,GRPDESC,STAMP")] CSGRPM cSGRPM)
        {
            cSGRPM.STAMP = 0;
            if (ModelState.IsValid)
            {
                try
                {
                 
                    db.CSGRPMs.Add(cSGRPM);
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

            return View(cSGRPM);
        }

        // GET: CSGRPMs/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSGRPM cSGRPM = db.CSGRPMs.Find(MyHtmlHelpers.ConvertByteStrToId(id));
            if (cSGRPM == null)
            {
                return HttpNotFound();
            }
            return View(cSGRPM);
        }

        // POST: CSGRPMs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "GRPCODE,GRPDESC,STAMP")] CSGRPM cSGRPM)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    cSGRPM.STAMP = cSGRPM.STAMP + 1;
                    db.Entry(cSGRPM).State = EntityState.Modified;
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
            return View(cSGRPM);
        }

        // GET: CSGRPMs/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSGRPM cSGRPM = db.CSGRPMs.Find(MyHtmlHelpers.ConvertByteStrToId(id));
            if (cSGRPM == null)
            {
                return HttpNotFound();
            }
            return View(cSGRPM);
        }

        // POST: CSGRPMs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            CSGRPM cSGRPM = db.CSGRPMs.Find(MyHtmlHelpers.ConvertByteStrToId(id));
            try
            {
                var myList = cSGRPM.CSGRPDs.ToList();
                foreach(var item in myList)
                {
                    var csDel = db.CSGRPDs.Find(item.GRPCODE, item.CONO);
                    db.CSGRPDs.Remove(csDel);
                }

                db.CSGRPMs.Remove(cSGRPM);


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

            return View(cSGRPM);
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
