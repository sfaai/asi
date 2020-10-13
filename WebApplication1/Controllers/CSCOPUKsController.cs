using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Utility;
using System.Data.Entity.Validation;
using System.Data.Entity.Core;
using System.Data.Entity.Infrastructure;
using FirebirdSql.Data.Client;
using FirebirdSql.Data.FirebirdClient;

namespace WebApplication1.Controllers
{
    public class CSCOPUKsController : Controller
    {
        private ASIDBConnection db = new ASIDBConnection();

        // GET: CSCOPUKs
        public ActionResult Index()
        {
            var cSCOPUKs = db.CSCOPUKs.Include(c => c.CSEQ);
            return View(cSCOPUKs.ToList());
        }

        // GET: CSCOPUKs/Details/5
        public ActionResult Details(string id, int row)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSCOPUK cSCOPUK = db.CSCOPUKs.Find(MyHtmlHelpers.ConvertByteStrToId(id), row);
            if (cSCOPUK == null)
            {
                return HttpNotFound();
            }
            ViewBag.EQCODE = new SelectList(db.CSEQs, "EQCODE", "EQDESC", cSCOPUK.EQCODE);
            ViewBag.EQCONSIDER = cSCOPUK.consideration;
            return View(cSCOPUK);
        }

        // GET: CSCOPUKs/Create
        public ActionResult Create(string id)
        {
           
            ViewBag.Parent = MyHtmlHelpers.ConvertByteStrToId(id);
            CSCOPUK cSCOPUK = new CSCOPUK();
            cSCOPUK.CONO = ViewBag.Parent;
            cSCOPUK.EFFDATE = DateTime.Today;
            cSCOPUK.CSCOMSTR = db.CSCOMSTRs.Find(cSCOPUK.CONO);
            ViewBag.EQCODE = new SelectList(db.CSEQs, "EQCODE", "EQDESC");
            ViewBag.EQCONSIDER = cSCOPUK.consideration;
            return View(cSCOPUK);
        }

        // POST: CSCOPUKs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CONO,EFFDATE,EQCODE,EQCONSIDER,NOOFSHARES,NOMINAL,PAIDAMT,DUEAMT,PREMIUM,NCDETStr,NCDET,ENDDATE,ROWNO,STAMP")] CSCOPUK cSCOPUK)
        {
            if (ModelState.IsValid)
            {
                int lastRowNo = 0;
                try
                {
                    lastRowNo = db.CSCOPUKs.Where(m => m.CONO == cSCOPUK.CONO).Max(n => n.ROWNO);
                }
                catch (Exception e) { lastRowNo = 0; }
                finally { };

                try
                {
                    cSCOPUK.ROWNO = lastRowNo + 1;
                    cSCOPUK.STAMP = 0;
                    db.CSCOPUKs.Add(cSCOPUK);
                    db.SaveChanges();
                    return new RedirectResult(Url.Action("Edit", "CSCOMSTRs", new { id = MyHtmlHelpers.ConvertIdToByteStr(cSCOPUK.CONO) }) + "#PaidUp");
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
            }

            ViewBag.EQCODE = new SelectList(db.CSEQs, "EQCODE", "EQDESC", cSCOPUK.EQCODE);
            ViewBag.EQCONSIDER = cSCOPUK.consideration;
            cSCOPUK.CSCOMSTR = db.CSCOMSTRs.Find(cSCOPUK.CONO);
            return View(cSCOPUK);
        }

        // GET: CSCOPUKs/Edit/5
        public ActionResult Edit(string id, int row)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSCOPUK cSCOPUK = db.CSCOPUKs.Find(MyHtmlHelpers.ConvertByteStrToId(id), row);
            if (cSCOPUK == null)
            {
                return HttpNotFound();
            }
            ViewBag.EQCODE = new SelectList(db.CSEQs, "EQCODE", "EQDESC", cSCOPUK.EQCODE);
            ViewBag.EQCONSIDER = cSCOPUK.consideration;
            return View(cSCOPUK);
        }

        // POST: CSCOPUKs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CONO,EFFDATE,EQCODE,EQCONSIDER,NOOFSHARES,NOMINAL,PAIDAMT,DUEAMT,PREMIUM,NCDET,NCDETStr,ENDDATE,ROWNO,STAMP")] CSCOPUK cSCOPUK)
        {
            if (ModelState.IsValid)
            {try
                {
                    cSCOPUK.STAMP = cSCOPUK.STAMP + 1;
                    db.Entry(cSCOPUK).State = EntityState.Modified;
                    db.SaveChanges();
                    return new RedirectResult(Url.Action("Edit", "CSCOMSTRs", new { id = MyHtmlHelpers.ConvertIdToByteStr(cSCOPUK.CONO) }) + "#PaidUp");
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
            }
            ViewBag.EQCODE = new SelectList(db.CSEQs, "EQCODE", "EQDESC", cSCOPUK.EQCODE);
            ViewBag.EQCONSIDER = cSCOPUK.consideration;
            cSCOPUK.CSCOMSTR = db.CSCOMSTRs.Find(cSCOPUK.CONO);
            return View(cSCOPUK);
        }

        // GET: CSCOPUKs/Delete/5
        public ActionResult Delete(string id, int row)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSCOPUK cSCOPUK = db.CSCOPUKs.Find(MyHtmlHelpers.ConvertByteStrToId(id), row);
            if (cSCOPUK == null)
            {
                return HttpNotFound();
            }
            return View(cSCOPUK);
        }

        // POST: CSCOPUKs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id, int row)
        {
            CSCOPUK cSCOPUK = db.CSCOPUKs.Find(MyHtmlHelpers.ConvertByteStrToId(id), row);
            try
            {
                db.CSCOPUKs.Remove(cSCOPUK);
                db.SaveChanges();

                return new RedirectResult(Url.Action("Edit", "CSCOMSTRs", new { id = MyHtmlHelpers.ConvertIdToByteStr(cSCOPUK.CONO) }) + "#PaidUp");
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
            cSCOPUK.CSCOMSTR = db.CSCOMSTRs.Find(cSCOPUK.CONO);
            return View(cSCOPUK);
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
