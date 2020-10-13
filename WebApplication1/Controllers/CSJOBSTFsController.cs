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
    [Authorize(Roles = "Administrator,CS-A/C,CS-SEC,CS-AS")]
    public class CSJOBSTFsController : Controller
    {
        private ASIDBConnection db = new ASIDBConnection();

        // GET: CSJOBSTFs
        public ActionResult Index()
        {
            var cSJOBSTFs = db.CSJOBSTFs.Include(c => c.HKSTAFF);
            return View(cSJOBSTFs.ToList());
        }

        // GET: CSJOBSTFs/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSJOBSTF cSJOBSTF = db.CSJOBSTFs.Find(id);
            if (cSJOBSTF == null)
            {
                return HttpNotFound();
            }
            return View(cSJOBSTF);
        }

        // GET: CSJOBSTFs/Create
        public ActionResult Create(string id)
        {
            CSJOBSTF cSJOBSTF = new CSJOBSTF();
            cSJOBSTF.STAMP = 0;
            cSJOBSTF.JOBNO = MyHtmlHelpers.ConvertByteStrToId(id);
            cSJOBSTF.EDATE = new DateTime(3000, 1, 1);
            cSJOBSTF.SDATE = DateTime.Today;

            ViewBag.STAFFCODE = new SelectList(db.HKSTAFFs, "STAFFCODE", "STAFFDESC");
            ViewBag.Title = "Assign Staff to Job ";
            return View("Edit", cSJOBSTF);
        }

        // POST: CSJOBSTFs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "JOBNO,SDATE,EDATE,STAFFCODE,REM,ROWNO,STAMP")] CSJOBSTF cSJOBSTF)
        {
            if (ModelState.IsValid)
            {
                CSJOBSTF lastRec = db.CSJOBSTFs.Where(x => x.JOBNO == cSJOBSTF.JOBNO).OrderByDescending(z => z.ROWNO).FirstOrDefault();
                int rowno = 1;
                if (lastRec != null)
                {
                    rowno = lastRec.ROWNO + 1;
                    lastRec.EDATE = cSJOBSTF.SDATE;
                    lastRec.STAMP = lastRec.STAMP + 1;
                    db.Entry(lastRec).State = EntityState.Modified;
                }

                cSJOBSTF.STAMP = 0;
                cSJOBSTF.ROWNO = rowno;

                CSJOBM cSJOBM = db.CSJOBMs.Find(cSJOBSTF.JOBNO);
                cSJOBM.JOBSTAFF = cSJOBSTF.STAFFCODE;
                cSJOBM.STAMP = cSJOBM.STAMP + 1;
                try
                {
                    db.Entry(cSJOBM).State = EntityState.Modified;
                    db.CSJOBSTFs.Add(cSJOBSTF);
                    db.SaveChanges();

                    int page = (int)Session["CSJOBDPage"];
                    return RedirectToAction("Index", "CSJOBDs", new { page = page });
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

            ViewBag.STAFFCODE = new SelectList(db.HKSTAFFs, "STAFFCODE", "STAFFDESC", cSJOBSTF.STAFFCODE);
            return View(cSJOBSTF);
        }

        // GET: CSJOBSTFs/Edit/5
        public ActionResult Edit(string id, int row, int ? page)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSJOBSTF cSJOBSTF = db.CSJOBSTFs.Find(MyHtmlHelpers.ConvertByteStrToId(id), row);
            if (cSJOBSTF == null)
            {
                return HttpNotFound();
            }
            ViewBag.STAFFCODE = new SelectList(db.HKSTAFFs, "STAFFCODE", "STAFFDESC", cSJOBSTF.STAFFCODE);
            return View(cSJOBSTF);
        }

        // POST: CSJOBSTFs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "JOBNO,SDATE,EDATE,STAFFCODE,REM,ROWNO,STAMP")] CSJOBSTF cSJOBSTF)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    cSJOBSTF.STAMP = cSJOBSTF.STAMP + 1;
                    db.Entry(cSJOBSTF).State = EntityState.Modified;
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
            ViewBag.STAFFCODE = new SelectList(db.HKSTAFFs, "STAFFCODE", "STAFFDESC", cSJOBSTF.STAFFCODE);
            return View(cSJOBSTF);
        }

        // GET: CSJOBSTFs/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSJOBSTF cSJOBSTF = db.CSJOBSTFs.Find(id);
            if (cSJOBSTF == null)
            {
                return HttpNotFound();
            }
            return View(cSJOBSTF);
        }

        // POST: CSJOBSTFs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            CSJOBSTF cSJOBSTF = db.CSJOBSTFs.Find(id);
            try
            {
                db.CSJOBSTFs.Remove(cSJOBSTF);
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

            return View(cSJOBSTF);
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
