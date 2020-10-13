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
    public class CSTRANMsController : Controller
    {
        private ASIDBConnection db = new ASIDBConnection();

        // GET: CSTRANMs
        public ActionResult Index()
        {
            return View(db.CSTRANMs.ToList());
        }

        // GET: CSTRANMs/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSTRANM cSTRANM = db.CSTRANMs.Find(id);
            if (cSTRANM == null)
            {
                return HttpNotFound();
            }
            return View(cSTRANM);
        }

        // GET: CSTRANMs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CSTRANMs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SOURCE,SOURCENO,SOURCEID,CONO,JOBNO,CASENO,CASECODE,DUEDATE,ENTDATE,TRTYPE,TRDESC,TRITEM1,TRITEM2,TRITEM,TRTAX1,TRTAX2,TRTAX,TRAMT1,TRAMT2,TRAMT,TRSIGN,TRSITEM1,TRSITEM2,TRSITEM,TRSTAX1,TRSTAX2,TRSTAX,TRSAMT1,TRSAMT2,TRSAMT,TRITEMOS1,TRITEMOS2,TRITEMOS,TRTAXOS1,TRTAXOS2,TRTAXOS,TROS1,TROS2,TROS,APPTYPE,APPNO,APPID,APPITEM1,APPITEM2,APPITEM,APPTAX1,APPTAX2,APPTAX,APPAMT1,APPAMT2,APPAMT,REM,COMPLETE,COMPLETED,SEQNO,REFCNT,STAMP")] CSTRANM cSTRANM)
        {
            if (ModelState.IsValid)
            {
                cSTRANM.STAMP = 0;
                try
                {
                    db.CSTRANMs.Add(cSTRANM);
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

            return View(cSTRANM);
        }

        // GET: CSTRANMs/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSTRANM cSTRANM = db.CSTRANMs.Find(id);
            if (cSTRANM == null)
            {
                return HttpNotFound();
            }
            return View(cSTRANM);
        }

        // POST: CSTRANMs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SOURCE,SOURCENO,SOURCEID,CONO,JOBNO,CASENO,CASECODE,DUEDATE,ENTDATE,TRTYPE,TRDESC,TRITEM1,TRITEM2,TRITEM,TRTAX1,TRTAX2,TRTAX,TRAMT1,TRAMT2,TRAMT,TRSIGN,TRSITEM1,TRSITEM2,TRSITEM,TRSTAX1,TRSTAX2,TRSTAX,TRSAMT1,TRSAMT2,TRSAMT,TRITEMOS1,TRITEMOS2,TRITEMOS,TRTAXOS1,TRTAXOS2,TRTAXOS,TROS1,TROS2,TROS,APPTYPE,APPNO,APPID,APPITEM1,APPITEM2,APPITEM,APPTAX1,APPTAX2,APPTAX,APPAMT1,APPAMT2,APPAMT,REM,COMPLETE,COMPLETED,SEQNO,REFCNT,STAMP")] CSTRANM cSTRANM)
        {
            if (ModelState.IsValid)
            {
                cSTRANM.STAMP = cSTRANM.STAMP + 1;
                try
                {
                    db.Entry(cSTRANM).State = EntityState.Modified;
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
            return View(cSTRANM);
        }

        // GET: CSTRANMs/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSTRANM cSTRANM = db.CSTRANMs.Find(id);
            if (cSTRANM == null)
            {
                return HttpNotFound();
            }
            return View(cSTRANM);
        }

        // POST: CSTRANMs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            CSTRANM cSTRANM = db.CSTRANMs.Find(id);
            try
            {
                db.CSTRANMs.Remove(cSTRANM);
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
            return View(cSTRANM);
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
