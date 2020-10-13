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
    public class CSITEMsController : Controller
    {
        private ASIDBConnection db = new ASIDBConnection();

        // GET: CSITEMs
        public ActionResult Index()
        {
            return View(db.CSITEMs.ToList());
        }

        public ActionResult Listing()
        {
            SACOMP profileRec = db.SACOMPs.FirstOrDefault();
            ViewBag.Company = profileRec.CONAME.ToUpper() + " (" + profileRec.COREGNO + ") ";

            return View(db.CSITEMs);
        }

        // GET: CSITEMs/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSITEM cSITEM = db.CSITEMs.Find(MyHtmlHelpers.ConvertByteStrToId(id));
            if (cSITEM == null)
            {
                return HttpNotFound();
            }
            return View(cSITEM);
        }

        // GET: CSITEMs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CSITEMs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ITEMTYPE,ITEMDESC,GSTCODE,GSTRATE,STAMP")] CSITEM cSITEM)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    cSITEM.STAMP = 0;
                    db.CSITEMs.Add(cSITEM);
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

            return View(cSITEM);
        }

        // GET: CSITEMs/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSITEM cSITEM = db.CSITEMs.Find(MyHtmlHelpers.ConvertByteStrToId(id));
            if (cSITEM == null)
            {
                return HttpNotFound();
            }
            return View(cSITEM);
        }

        public string CheckRate(string vdate, string itemtype)
        {
            DateTime myDate = DateTime.Parse(vdate);
            CSITEM cSITEM = db.CSITEMs.Find(itemtype);
            string taxcode = "SSTN01";
            decimal taxrate = 0;
            if (cSITEM != null)
            {
                taxrate = cSITEM.GSTRATE ?? 0;

                taxcode = db.CSTAXTYPEs.Where(x => x.TAXRATE == taxrate && x.EFFECTIVE_START <= myDate && x.EFFECTIVE_END >= myDate).Select(y => y.TAXCODE).FirstOrDefault() ?? taxcode;
               
            }
            return taxcode + "|" + taxrate.ToString("N2");
        }

        // POST: CSITEMs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ITEMTYPE,ITEMDESC,GSTCODE,GSTRATE,STAMP")] CSITEM cSITEM)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    cSITEM.STAMP = cSITEM.STAMP + 1;
                    db.Entry(cSITEM).State = EntityState.Modified;
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
            return View(cSITEM);
        }

        // GET: CSITEMs/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSITEM cSITEM = db.CSITEMs.Find(MyHtmlHelpers.ConvertByteStrToId(id));
            if (cSITEM == null)
            {
                return HttpNotFound();
            }
            return View(cSITEM);
        }

        // POST: CSITEMs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            CSITEM cSITEM = db.CSITEMs.Find(MyHtmlHelpers.ConvertByteStrToId(id));
            try
            {
                db.CSITEMs.Remove(cSITEM);
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
            return View(cSITEM);
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
