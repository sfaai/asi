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
    public class CSCOBANKsController : Controller
    {
        private ASIDBConnection db = new ASIDBConnection();

        // GET: CSCOBANKs
        public ActionResult Index()
        {
            var cSCOBANKs = db.CSCOBANKs.Include(c => c.HKBANK);
            return View(cSCOBANKs.ToList());
        }

        // GET: CSCOBANKs/Details/5
        public ActionResult Details(string id, int row)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSCOBANK cSCOBANK = db.CSCOBANKs.Find(MyHtmlHelpers.ConvertByteStrToId(id), row);
            if (cSCOBANK == null)
            {
                return HttpNotFound();
            }
            ViewBag.BANKCODE = new SelectList(db.HKBANKs, "BANKCODE", "BANKDESC", cSCOBANK.BANKCODE);
            return View(cSCOBANK);
        }

        // GET: CSCOBANKs/Create
        public ActionResult Create(string id)
        {
            ViewBag.BANKCODE = new SelectList(db.HKBANKs, "BANKCODE", "BANKDESC");
            ViewBag.Parent = MyHtmlHelpers.ConvertByteStrToId(id);
            string sid = ViewBag.Parent;

            CSCOBANK curRec = new CSCOBANK();
            curRec.CONO = sid;
            curRec.EFFDATE = DateTime.Today;
            curRec.ENDDATE = new DateTime(3000, 1, 1);
            curRec.CSCOMSTR = db.CSCOMSTRs.Find(curRec.CONO);
            ViewBag.BANKCODE = new SelectList(db.HKBANKs.OrderBy( x => x.BANKDESC), "BANKCODE", "BANKDESC");
         
            return View(curRec);
        }

        // POST: CSCOBANKs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CONO,BANKCODE,ACTYPE,EFFDATE,TERDATE,ENDDATE,REM,ROWNO,STAMP")] CSCOBANK cSCOBANK)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    int lastRowNo = 0;
                    try
                    {
                        lastRowNo = db.CSCOBANKs.Where(m => m.CONO == cSCOBANK.CONO).Max(n => n.ROWNO);
                    }
                    catch (Exception e) { lastRowNo = 0; }
                    finally { };

                    cSCOBANK.STAMP = 0;
                    cSCOBANK.ROWNO = lastRowNo + 1;
                    db.CSCOBANKs.Add(cSCOBANK);
                    db.SaveChanges();
                    return new RedirectResult(Url.Action("Edit", "CSCOMSTRs", new { id = MyHtmlHelpers.ConvertIdToByteStr(cSCOBANK.CONO) }) + "#Banker");
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
            cSCOBANK.CSCOMSTR = db.CSCOMSTRs.Find(cSCOBANK.CONO);
            ViewBag.BANKCODE = new SelectList(db.HKBANKs, "BANKCODE", "BANKDESC", cSCOBANK.BANKCODE);
            return View(cSCOBANK);
        }

        // GET: CSCOBANKs/Edit/5
        public ActionResult Edit(string id, int row)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSCOBANK cSCOBANK = db.CSCOBANKs.Find(MyHtmlHelpers.ConvertByteStrToId(id), row);
            if (cSCOBANK == null)
            {
                return HttpNotFound();
            }
            cSCOBANK.CSCOMSTR = db.CSCOMSTRs.Find(cSCOBANK.CONO);
            ViewBag.BANKCODE = new SelectList(db.HKBANKs, "BANKCODE", "BANKDESC", cSCOBANK.BANKCODE);
            return View(cSCOBANK);
        }

        // POST: CSCOBANKs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CONO,BANKCODE,ACTYPE,EFFDATE,TERDATE,ENDDATE,REM,ROWNO,STAMP")] CSCOBANK cSCOBANK)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    cSCOBANK.STAMP = cSCOBANK.STAMP + 1;
                    db.Entry(cSCOBANK).State = EntityState.Modified;
                    db.SaveChanges();
                    return new RedirectResult(Url.Action("Edit", "CSCOMSTRs", new { id = MyHtmlHelpers.ConvertIdToByteStr(cSCOBANK.CONO) }) + "#Banker");
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
            cSCOBANK.CSCOMSTR = db.CSCOMSTRs.Find(cSCOBANK.CONO);
            ViewBag.BANKCODE = new SelectList(db.HKBANKs, "BANKCODE", "BANKDESC", cSCOBANK.BANKCODE);
            return View(cSCOBANK);
        }

        // GET: CSCOBANKs/Delete/5
        public ActionResult Delete(string id, int row)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSCOBANK cSCOBANK = db.CSCOBANKs.Find(MyHtmlHelpers.ConvertByteStrToId(id), row);
            if (cSCOBANK == null)
            {
                return HttpNotFound();
            }
            return View(cSCOBANK);
        }

        // POST: CSCOBANKs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id, int row)
        {
            CSCOBANK cSCOBANK = db.CSCOBANKs.Find(MyHtmlHelpers.ConvertByteStrToId(id), row);
            try
            {
               
                db.CSCOBANKs.Remove(cSCOBANK);
                db.SaveChanges();
                return new RedirectResult(Url.Action("Edit", "CSCOMSTRs", new { id = MyHtmlHelpers.ConvertIdToByteStr(cSCOBANK.CONO) }) + "#Banker");
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

            return View(cSCOBANK);
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
