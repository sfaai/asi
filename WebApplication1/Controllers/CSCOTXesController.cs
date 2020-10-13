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
    public class CSCOTXesController : Controller
    {
        private ASIDBConnection db = new ASIDBConnection();

        // GET: CSCOTXes
        public ActionResult Index()
        {
            var cSCOTXes = db.CSCOTXes.Include(c => c.CSTX);
            return View(cSCOTXes.ToList());
        }

        // GET: CSCOTXes/Details/5
        public ActionResult Details(string id, int row)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSCOTX cSCOTX = db.CSCOTXes.Find(MyHtmlHelpers.ConvertByteStrToId(id), row);
            if (cSCOTX == null)
            {
                return HttpNotFound();
            }
            ViewBag.TXCODE = new SelectList(db.CSTXes.OrderBy(x => x.TXDESC), "TXCODE", "TXDESC", cSCOTX.TXCODE);
            return View(cSCOTX);
        }

        // GET: CSCOTXes/Create
        public ActionResult Create(string id)
        {
            ViewBag.Parent = MyHtmlHelpers.ConvertByteStrToId(id);
            CSCOTX cSCOTX = new CSCOTX();
            cSCOTX.CONO = ViewBag.Parent;
            cSCOTX.ADATE = DateTime.Today;
            cSCOTX.ENDDATE = new DateTime(3000, 1, 1);
            cSCOTX.CSCOMSTR = db.CSCOMSTRs.Find(cSCOTX.CONO);
            ViewBag.TXCODE = new SelectList(db.CSTXes.OrderBy( x => x.TXDESC), "TXCODE", "TXDESC");
            return View(cSCOTX);
        }

        // POST: CSCOTXes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CONO,TXCODE,ADATE,RDATE,ENDDATE,REM,ROWNO,STAMP")] CSCOTX cSCOTX)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    int lastRowNo = 0;
                    try
                    {
                        lastRowNo = db.CSCOTXes.Where(m => m.CONO == cSCOTX.CONO).Max(n => n.ROWNO);
                    }
                    catch (Exception e) { lastRowNo = 0; }
                    finally { };

                    cSCOTX.ROWNO = lastRowNo + 1;
                    cSCOTX.STAMP = 0;
                    db.CSCOTXes.Add(cSCOTX);
                    db.SaveChanges();
                    return new RedirectResult(Url.Action("Edit", "CSCOMSTRs", new { id = MyHtmlHelpers.ConvertIdToByteStr(cSCOTX.CONO) }) + "#TaxAgent");
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

            cSCOTX.CSCOMSTR = db.CSCOMSTRs.Find(cSCOTX.CONO);
            ViewBag.TXCODE = new SelectList(db.CSTXes.OrderBy(x => x.TXDESC), "TXCODE", "TXDESC", cSCOTX.TXCODE);
            return View(cSCOTX);
        }

        // GET: CSCOTXes/Edit/5
        public ActionResult Edit(string id, int row)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSCOTX cSCOTX = db.CSCOTXes.Find(MyHtmlHelpers.ConvertByteStrToId(id), row);
            if (cSCOTX == null)
            {
                return HttpNotFound();
            }
            ViewBag.TXCODE = new SelectList(db.CSTXes.OrderBy(x => x.TXDESC), "TXCODE", "TXDESC", cSCOTX.TXCODE);
            return View(cSCOTX);
        }

        // POST: CSCOTXes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CONO,TXCODE,ADATE,RDATE,ENDDATE,REM,ROWNO,STAMP")] CSCOTX cSCOTX)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    cSCOTX.STAMP = cSCOTX.STAMP + 1;
                    db.Entry(cSCOTX).State = EntityState.Modified;
                    db.SaveChanges();
                    return new RedirectResult(Url.Action("Edit", "CSCOMSTRs", new { id = MyHtmlHelpers.ConvertIdToByteStr(cSCOTX.CONO) }) + "#TaxAgent");
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
            cSCOTX.CSCOMSTR = db.CSCOMSTRs.Find(cSCOTX.CONO);
            ViewBag.TXCODE = new SelectList(db.CSTXes.OrderBy(x => x.TXDESC), "TXCODE", "TXDESC", cSCOTX.TXCODE);
            return View(cSCOTX);
        }

        // GET: CSCOTXes/Delete/5
        public ActionResult Delete(string id, int row)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSCOTX cSCOTX = db.CSCOTXes.Find(MyHtmlHelpers.ConvertByteStrToId(id), row);
            if (cSCOTX == null)
            {
                return HttpNotFound();
            }
            return View(cSCOTX);
        }

        // POST: CSCOTXes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id, int row)
        {
            CSCOTX cSCOTX = db.CSCOTXes.Find(MyHtmlHelpers.ConvertByteStrToId(id), row);
            try
            {
          
                db.CSCOTXes.Remove(cSCOTX);
                db.SaveChanges();
                return new RedirectResult(Url.Action("Edit", "CSCOMSTRs", new { id = MyHtmlHelpers.ConvertIdToByteStr(cSCOTX.CONO) }) + "#TaxAgent");
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
            return View(cSCOTX);
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
