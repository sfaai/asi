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
    [Authorize(Roles = "Administrator")]
    public class HKLOGINMAPsController : Controller
    {
        private ASIDBConnection db = new ASIDBConnection();

        // GET: HKLOGINMAPs
        public ActionResult Index()
        {
            var hKLOGINMAPs = db.HKLOGINMAPs.Include(h => h.HKSTAFF);
            return View(hKLOGINMAPs.ToList());
        }

        // GET: HKLOGINMAPs/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HKLOGINMAP hKLOGINMAP = db.HKLOGINMAPs.Find(MyHtmlHelpers.ConvertByteStrToId(id));
            if (hKLOGINMAP == null)
            {
                return HttpNotFound();
            }
            ViewBag.STAFFCODE = new SelectList(db.HKSTAFFs.Select(x => new { STAFFCODE = x.STAFFCODE, STAFFDESC = "(" + x.STAFFCODE + ") " + x.STAFFDESC }), "STAFFCODE", "STAFFDESC", hKLOGINMAP.STAFFCODE);
            return View(hKLOGINMAP);
        }

        // GET: HKLOGINMAPs/Create
        public ActionResult Create()
        {

            ViewBag.STAFFCODE = new SelectList(db.HKSTAFFs.Select(x => new { STAFFCODE = x.STAFFCODE, STAFFDESC = "(" + x.STAFFCODE + ") " + x.STAFFDESC }), "STAFFCODE", "STAFFDESC");
            ViewBag.USERID = new SelectList(db.SAUSERMs, "USERID", "USERID");

            return View();
        }

        // POST: HKLOGINMAPs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "USERID,STAFFCODE,STAMP")] HKLOGINMAP hKLOGINMAP)
        {
            // for some reason ModelState.IsValid is always false 
            //   if (ModelState.IsValid)
            //  {
            try
            {
                hKLOGINMAP.STAMP = 0;

                db.HKLOGINMAPs.Add(hKLOGINMAP);
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
            catch (Exception e) { ModelState.AddModelError(string.Empty, e.Message); }
         //   }
            ViewBag.USERID = new SelectList(db.SAUSERMs, "USERID", "USERID", hKLOGINMAP.USERID);
            ViewBag.STAFFCODE = new SelectList(db.HKSTAFFs.Select( x => new { STAFFCODE = x.STAFFCODE, STAFFDESC = "(" + x.STAFFCODE + ") " + x.STAFFDESC }), "STAFFCODE", "STAFFDESC", hKLOGINMAP.STAFFCODE);
            return View(hKLOGINMAP);
        }

        // GET: HKLOGINMAPs/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HKLOGINMAP hKLOGINMAP = db.HKLOGINMAPs.Find(MyHtmlHelpers.ConvertByteStrToId(id));
            if (hKLOGINMAP == null)
            {
                return HttpNotFound();
            }
            ViewBag.USERID = new SelectList(db.SAUSERMs, "USERID", "USERID", hKLOGINMAP.USERID);
            ViewBag.STAFFCODE = new SelectList(db.HKSTAFFs.Select(x => new { STAFFCODE = x.STAFFCODE, STAFFDESC = "(" + x.STAFFCODE + ") " + x.STAFFDESC }), "STAFFCODE", "STAFFDESC", hKLOGINMAP.STAFFCODE);
            return View(hKLOGINMAP);
        }

        // POST: HKLOGINMAPs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "USERID,STAFFCODE,STAMP")] HKLOGINMAP hKLOGINMAP)
        {
            if (ModelState.IsValid)
            {try
                {
                    hKLOGINMAP.STAMP = hKLOGINMAP.STAMP + 1;
                    db.Entry(hKLOGINMAP).State = EntityState.Modified;
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
            ViewBag.USERID = new SelectList(db.SAUSERMs, "USERID", "USERID", hKLOGINMAP.USERID);
            ViewBag.STAFFCODE = new SelectList(db.HKSTAFFs.Select(x => new { STAFFCODE = x.STAFFCODE, STAFFDESC = "(" + x.STAFFCODE + ") " + x.STAFFDESC }), "STAFFCODE", "STAFFDESC", hKLOGINMAP.STAFFCODE);
            return View(hKLOGINMAP);
        }

        // GET: HKLOGINMAPs/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HKLOGINMAP hKLOGINMAP = db.HKLOGINMAPs.Find(MyHtmlHelpers.ConvertByteStrToId(id));
            if (hKLOGINMAP == null)
            {
                return HttpNotFound();
            }
            ViewBag.STAFFCODE = new SelectList(db.HKSTAFFs.Select(x => new { STAFFCODE = x.STAFFCODE, STAFFDESC = "(" + x.STAFFCODE + ") " + x.STAFFDESC }), "STAFFCODE", "STAFFDESC", hKLOGINMAP.STAFFCODE);
            return View(hKLOGINMAP);
        }

        // POST: HKLOGINMAPs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            HKLOGINMAP hKLOGINMAP = db.HKLOGINMAPs.Find(MyHtmlHelpers.ConvertByteStrToId(id));
            try
            {
                db.HKLOGINMAPs.Remove(hKLOGINMAP);
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

            ViewBag.STAFFCODE = new SelectList(db.HKSTAFFs.Select(x => new { STAFFCODE = x.STAFFCODE, STAFFDESC = "(" + x.STAFFCODE + ") " + x.STAFFDESC }), "STAFFCODE", "STAFFDESC", hKLOGINMAP.STAFFCODE);
            return View(hKLOGINMAP);
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
