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
    public class CSCOLASTNOesController : Controller
    {
        private ASIDBConnection db = new ASIDBConnection();

        // GET: CSCOLASTNOes
        public ActionResult Index()
        {
            return View(db.CSCOLASTNOes.ToList());
        }

        // GET: CSCOLASTNOes/Details/5
        public ActionResult Details(string id, string code)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSCOLASTNO cSCOLASTNO = db.CSCOLASTNOes.Find(MyHtmlHelpers.ConvertByteStrToId(id), code);
            if (cSCOLASTNO == null)
            {
                return HttpNotFound();
            }
            return View(cSCOLASTNO);
        }

        // GET: CSCOLASTNOes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CSCOLASTNOes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CONO,LASTCODE,LASTDESC,LASTPFIX,LASTNO,LASTWD,AUTOGEN,STAMP")] CSCOLASTNO cSCOLASTNO)
        {
            if (ModelState.IsValid)
            {
                db.CSCOLASTNOes.Add(cSCOLASTNO);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(cSCOLASTNO);
        }

        // GET: CSCOLASTNOes/Edit/5
        public ActionResult Edit(string id, string code)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSCOLASTNO cSCOLASTNO = db.CSCOLASTNOes.Find(MyHtmlHelpers.ConvertByteStrToId(id), code);
            if (cSCOLASTNO == null)
            {
                return HttpNotFound();
            }
            return View(cSCOLASTNO);
        }

        // POST: CSCOLASTNOes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CONO,LASTCODE,LASTDESC,LASTPFIX,LASTNO,LASTWD,AUTOGENBool,STAMP")] CSCOLASTNO cSCOLASTNO)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    cSCOLASTNO.STAMP = cSCOLASTNO.STAMP + 1;
                    db.Entry(cSCOLASTNO).State = EntityState.Modified;
                    db.SaveChanges();
                    return new RedirectResult(Url.Action("Edit", "CSCOMSTRs", new { id = MyHtmlHelpers.ConvertIdToByteStr(cSCOLASTNO.CONO) }) + "#LastNo");
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
            cSCOLASTNO.CSCOMSTR = db.CSCOMSTRs.Find(cSCOLASTNO.CONO);
            return View(cSCOLASTNO);
        }

        // GET: CSCOLASTNOes/Delete/5
        public ActionResult Delete(string id, string code)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSCOLASTNO cSCOLASTNO = db.CSCOLASTNOes.Find(MyHtmlHelpers.ConvertByteStrToId(id), code);
            if (cSCOLASTNO == null)
            {
                return HttpNotFound();
            }
            return View(cSCOLASTNO);
        }

        // POST: CSCOLASTNOes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id, string code)
        {
            CSCOLASTNO cSCOLASTNO = db.CSCOLASTNOes.Find(MyHtmlHelpers.ConvertByteStrToId(id), code);
            try
            {
                db.CSCOLASTNOes.Remove(cSCOLASTNO);
                db.SaveChanges();
                return new RedirectResult(Url.Action("Edit", "CSCOMSTRs", new { id = MyHtmlHelpers.ConvertIdToByteStr(cSCOLASTNO.CONO) }) + "#LastNo");
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

            return View(cSCOLASTNO);
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
