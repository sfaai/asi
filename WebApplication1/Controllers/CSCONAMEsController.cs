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
    public class CSCONAMEsController : Controller
    {
        private ASIDBConnection db = new ASIDBConnection();

        // GET: CSCONAMEs
        public ActionResult Index()
        {
            return View(db.CSCONAMEs.ToList());
        }

        private bool UpdateCompany(CSCONAME cSCONAME)
        {
            bool result = false;

            CSCOMSTR cSCOMSTR = db.CSCOMSTRs.Find(cSCONAME.CONO);
            if (cSCOMSTR != null)
            {
                cSCOMSTR.CONAME = cSCONAME.CONAME;
                cSCOMSTR.STAMP = cSCOMSTR.STAMP + 1;
                db.Entry(cSCOMSTR).State = EntityState.Modified;
                result = true;
            }

            return result;
        }

        private CSCONAME UpdatePreviousRow(CSCONAME cSCONAME)
        {
            bool result = false;
            CSCONAME curRec = db.CSCONAMEs.Where(m => m.CONO == cSCONAME.CONO && m.ROWNO < cSCONAME.ROWNO).OrderByDescending(n => n.ROWNO).FirstOrDefault();
            if (curRec != null)
            {
                System.DateTime lastDate = (cSCONAME.EFFDATE).AddDays(-1);

                curRec.ENDDATE = lastDate;
                curRec.STAMP = curRec.STAMP + 1;
                db.Entry(curRec).State = EntityState.Modified;
                result = true;
            }
            return curRec;

        }

        // GET: CSCONAMEs/Details/5
        public ActionResult Details(string id, int row)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSCONAME cSCONAME = db.CSCONAMEs.Find(MyHtmlHelpers.ConvertByteStrToId(id), row);
            if (cSCONAME == null)
            {
                return HttpNotFound();
            }

            return View(cSCONAME);
        }

        // GET: CSCONAMEs/Create
        public ActionResult Create(string id)
        {
            ViewBag.Parent = MyHtmlHelpers.ConvertByteStrToId(id);
            CSCONAME cSCONAME = new CSCONAME();
            cSCONAME.CONO = ViewBag.Parent;

            cSCONAME.CSCOMSTR = db.CSCOMSTRs.Find(cSCONAME.CONO);

            return View(cSCONAME);
        }

        // POST: CSCONAMEs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CONO,EFFDATE,CONAME,ENDDATE,ROWNO,STAMP")] CSCONAME cSCONAME)
        {
            if (ModelState.IsValid)
            {
                int lastRowNo = 0;
                try
                {
                    lastRowNo = db.CSCONAMEs.Where(m => m.CONO == cSCONAME.CONO).Max(n => n.ROWNO);
                }
                catch (Exception e) { lastRowNo = 0; }
                finally { };

                try
                {
                    cSCONAME.STAMP = 0;
                    cSCONAME.ENDDATE = new DateTime(3000, 1, 1);
                    cSCONAME.ROWNO = lastRowNo + 1;
                    UpdateCompany(cSCONAME);
                    UpdatePreviousRow(cSCONAME);

                    db.CSCONAMEs.Add(cSCONAME);
                    db.SaveChanges();
                    return new RedirectResult(Url.Action("Edit", "CSCOMSTRs", new { id = MyHtmlHelpers.ConvertIdToByteStr(cSCONAME.CONO) }) + "#Name");
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
            cSCONAME.CSCOMSTR = db.CSCOMSTRs.Find(cSCONAME.CONO);
            return View(cSCONAME);
        }

        // GET: CSCONAMEs/Edit/5
        public ActionResult Edit(string id, int row)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSCONAME cSCONAME = db.CSCONAMEs.Find(MyHtmlHelpers.ConvertByteStrToId(id), row);
            if (cSCONAME == null)
            {
                return HttpNotFound();
            }

            return View(cSCONAME);
        }

        // POST: CSCONAMEs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CONO,EFFDATE,CONAME,ENDDATE,ROWNO,STAMP")] CSCONAME cSCONAME)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (db.CSCONAMEs.Where(x => x.CONO == cSCONAME.CONO).OrderByDescending(x => x.ROWNO).Select(x => x.ROWNO).FirstOrDefault() == cSCONAME.ROWNO)
                    {
                        UpdateCompany(cSCONAME);
                    }
                    UpdatePreviousRow(cSCONAME);

                    cSCONAME.STAMP = cSCONAME.STAMP + 1;
                    db.Entry(cSCONAME).State = EntityState.Modified;
                    db.SaveChanges();
                    return new RedirectResult(Url.Action("Edit", "CSCOMSTRs", new { id = MyHtmlHelpers.ConvertIdToByteStr(cSCONAME.CONO) }) + "#Name");
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
            cSCONAME.CSCOMSTR = db.CSCOMSTRs.Find(cSCONAME.CONO);
            return View(cSCONAME);
        }

        // GET: CSCONAMEs/Delete/5
        public ActionResult Delete(string id, int row)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSCONAME cSCONAME = db.CSCONAMEs.Find(MyHtmlHelpers.ConvertByteStrToId(id), row);
            if (cSCONAME == null)
            {
                return HttpNotFound();
            }
            cSCONAME.CSCOMSTR = db.CSCOMSTRs.Find(cSCONAME.CONO);
            return View(cSCONAME);
        }

        // POST: CSCONAMEs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id, int row)
        {
            CSCONAME cSCONAME = db.CSCONAMEs.Find(MyHtmlHelpers.ConvertByteStrToId(id), row);

            CSCONAME curRec = UpdatePreviousRow(cSCONAME);
            if (curRec != null)
            {
                try
                {
                    if (db.CSCONAMEs.Where(x => x.CONO == cSCONAME.CONO).OrderByDescending(x => x.ROWNO).Select(x => x.ROWNO).FirstOrDefault() == cSCONAME.ROWNO)
                    {
                        UpdateCompany(curRec);
                    }

                    db.CSCONAMEs.Remove(cSCONAME);
                    db.SaveChanges();
                    return new RedirectResult(Url.Action("Edit", "CSCOMSTRs", new { id = MyHtmlHelpers.ConvertIdToByteStr(cSCONAME.CONO) }) + "#Name");
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
            else
            {
                ModelState.AddModelError(string.Empty, "Cannot delete the only Name record");
            }
            cSCONAME.CSCOMSTR = db.CSCOMSTRs.Find(cSCONAME.CONO);
            return View(cSCONAME);
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
