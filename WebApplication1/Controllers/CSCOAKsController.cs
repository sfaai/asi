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
    public class CSCOAKsController : Controller
    {
        private ASIDBConnection db = new ASIDBConnection();

        // GET: CSCOAKs
        public ActionResult Index()
        {
            return View(db.CSCOAKs.ToList());
        }

        // GET: CSCOAKs/Details/5
        public ActionResult Details(string id, int row)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSCOAK cSCOAK = db.CSCOAKs.Find(MyHtmlHelpers.ConvertByteStrToId(id), row);
            if (cSCOAK == null)
            {
                return HttpNotFound();
            }
            return View(cSCOAK);
        }

        // GET: CSCOAKs/Create
        public ActionResult Create(string id)
        {
            ViewBag.Parent = MyHtmlHelpers.ConvertByteStrToId(id);
            CSCOAK cSCOAK = new CSCOAK();
            cSCOAK.CONO = ViewBag.Parent;
            cSCOAK.EFFDATE = DateTime.Today;
            cSCOAK.CSCOMSTR = db.CSCOMSTRs.Find(cSCOAK.CONO);

            return View(cSCOAK);
        }

        // POST: CSCOAKs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CONO,EFFDATE,AK,REM,ENDDATE,ROWNO,STAMP")] CSCOAK cSCOAK)
        {
            if (ModelState.IsValid)
            {
                int lastRowNo = 0;
                try
                {
                    lastRowNo = db.CSCOAKs.Where(m => m.CONO == cSCOAK.CONO).Max(n => n.ROWNO);
                }
                catch (Exception e) { lastRowNo = 0; }
                finally { };

                try
                {
                    cSCOAK.STAMP = 0;
                    cSCOAK.ENDDATE = new DateTime(3000, 1, 1);
                    cSCOAK.ROWNO = lastRowNo + 1;

                    db.CSCOAKs.Add(cSCOAK);
                    db.SaveChanges();
                    return new RedirectResult(Url.Action("Edit", "CSCOMSTRs", new { id = MyHtmlHelpers.ConvertIdToByteStr(cSCOAK.CONO) }) + "#Authorised");
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
            cSCOAK.CSCOMSTR = db.CSCOMSTRs.Find(cSCOAK.CONO);
            return View(cSCOAK);
        }

        // GET: CSCOAKs/Edit/5
        public ActionResult Edit(string id, int row)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSCOAK cSCOAK = db.CSCOAKs.Find(MyHtmlHelpers.ConvertByteStrToId(id), row);
            if (cSCOAK == null)
            {
                return HttpNotFound();
            }
            return View(cSCOAK);
        }

        // POST: CSCOAKs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CONO,EFFDATE,AK,REM,ENDDATE,ROWNO,STAMP")] CSCOAK cSCOAK)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    cSCOAK.STAMP = cSCOAK.STAMP + 1;
                    db.Entry(cSCOAK).State = EntityState.Modified;
                    db.SaveChanges();
                    return new RedirectResult(Url.Action("Edit", "CSCOMSTRs", new { id = MyHtmlHelpers.ConvertIdToByteStr(cSCOAK.CONO) }) + "#Authorised");
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
            cSCOAK.CSCOMSTR = db.CSCOMSTRs.Find(cSCOAK.CONO);
            return View(cSCOAK);
        }

        // GET: CSCOAKs/Delete/5
        public ActionResult Delete(string id, int row)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSCOAK cSCOAK = db.CSCOAKs.Find(MyHtmlHelpers.ConvertByteStrToId(id), row);
            if (cSCOAK == null)
            {
                return HttpNotFound();
            }
            return View(cSCOAK);
        }

        // POST: CSCOAKs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id, int row)
        {
            CSCOAK cSCOAK = db.CSCOAKs.Find(MyHtmlHelpers.ConvertByteStrToId(id), row);
            try
            {
                db.CSCOAKs.Remove(cSCOAK);
                db.SaveChanges();
                return new RedirectResult(Url.Action("Edit", "CSCOMSTRs", new { id = MyHtmlHelpers.ConvertIdToByteStr(cSCOAK.CONO) }) + "#Authorised");
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

            cSCOAK.CSCOMSTR = db.CSCOMSTRs.Find(cSCOAK.CONO);
            return View(cSCOAK);
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
