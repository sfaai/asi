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
    public class CSGRPDsController : Controller
    {
        private ASIDBConnection db = new ASIDBConnection();

        // GET: CSGRPDs
        public ActionResult Index()
        {
            var cSGRPDs = db.CSGRPDs.Include(c => c.CSCOMSTR);
            return View(cSGRPDs.ToList());
        }

        // GET: CSGRPDs/Details/5
        public ActionResult Details(string id, string conos)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSGRPD cSGRPD = db.CSGRPDs.Find(MyHtmlHelpers.ConvertByteStrToId(id), MyHtmlHelpers.ConvertByteStrToId(conos));
            if (cSGRPD == null)
            {
                return HttpNotFound();
            }
            ViewBag.CONO = new SelectList(db.CSCOMSTRs.OrderBy(x => x.CONAME), "CONO", "CONAME");
            return View(cSGRPD);
        }

        // GET: CSGRPDs/Create
        public ActionResult Create(string id)
        {
            CSGRPD cSGRPD = new WebApplication1.CSGRPD();
            cSGRPD.GRPCODE = MyHtmlHelpers.ConvertByteStrToId(id);
            ViewBag.CONO = new SelectList(db.CSCOMSTRs.OrderBy(x => x.CONAME), "CONO", "CONAME");
            return View(cSGRPD);
        }

        // POST: CSGRPDs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "GRPCODE,CONO,STAMP")] CSGRPD cSGRPD)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    cSGRPD.STAMP = 0;
                    db.CSGRPDs.Add(cSGRPD);
                    db.SaveChanges();
                    return RedirectToAction("Index","CSGRPMs");
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

            ViewBag.CONO = new SelectList(db.CSCOMSTRs, "CONO", "CONAME", cSGRPD.CONO);
            return View(cSGRPD);
        }

        // GET: CSGRPDs/Edit/5
        public ActionResult Edit(string id, string conos)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSGRPD cSGRPD = db.CSGRPDs.Find(MyHtmlHelpers.ConvertByteStrToId(id), MyHtmlHelpers.ConvertByteStrToId(conos));
            if (cSGRPD == null)
            {
                return HttpNotFound();
            }

            Session["CSGRPD_ORIG"] = cSGRPD;
            ViewBag.CONO = new SelectList(db.CSCOMSTRs.OrderBy(x => x.CONAME), "CONO", "CONAME", cSGRPD.CONO);
            return View(cSGRPD);
        }

        // POST: CSGRPDs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "GRPCODE,CONO,STAMP")] CSGRPD cSGRPD)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    cSGRPD.STAMP = cSGRPD.STAMP + 1;

                    bool changed = false;
                    CSGRPD csOrig = (CSGRPD)Session["CSGRPD_ORIG"];
                    if (csOrig.CONO != cSGRPD.CONO) { changed = true; }

                    if (changed)
                    {
                        CSGRPD csDel = db.CSGRPDs.Find(csOrig.GRPCODE, csOrig.CONO); 
                        db.CSGRPDs.Remove(csDel);
                        db.CSGRPDs.Add(cSGRPD);
                    }
                    else
                    {
                        db.Entry(cSGRPD).State = EntityState.Modified;
                    }

                 
                    db.SaveChanges();
                    return RedirectToAction("Index", "CSGRPMs");
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
            ViewBag.CONO = new SelectList(db.CSCOMSTRs.OrderBy(x => x.CONAME), "CONO", "CONAME", cSGRPD.CONO);
            return View(cSGRPD);
        }

        // GET: CSGRPDs/Delete/5
        public ActionResult Delete(string id, string conos)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSGRPD cSGRPD = db.CSGRPDs.Find(MyHtmlHelpers.ConvertByteStrToId(id), MyHtmlHelpers.ConvertByteStrToId(conos));
            if (cSGRPD == null)
            {
                return HttpNotFound();
            }
            ViewBag.CONO = new SelectList(db.CSCOMSTRs.OrderBy(x => x.CONAME), "CONO", "CONAME", cSGRPD.CONO);
            return View(cSGRPD);
        }

        // POST: CSGRPDs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id, string conos)
        {
            CSGRPD cSGRPD = db.CSGRPDs.Find(MyHtmlHelpers.ConvertByteStrToId(id), MyHtmlHelpers.ConvertByteStrToId(conos));
            try
            {
              
                db.CSGRPDs.Remove(cSGRPD);
                db.SaveChanges();
                return RedirectToAction("Index", "CSGRPMs");
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
            return View(cSGRPD);
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
