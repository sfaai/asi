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
    public class CSCOPARENTsController : Controller
    {
        private ASIDBConnection db = new ASIDBConnection();

        // GET: CSCOPARENTs
        public ActionResult Index()
        {
            var cSCOPARENTs = db.CSCOPARENTs.Include(c => c.CSCOMSTR);
            return View(cSCOPARENTs.ToList());
        }

        // GET: CSCOPARENTs/Details/5
        public ActionResult Details(string id, int row)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSCOPARENT cSCOPARENT = db.CSCOPARENTs.Find(MyHtmlHelpers.ConvertByteStrToId(id), row);
            if (cSCOPARENT == null)
            {
                return HttpNotFound();
            }
            ViewBag.CONAME = db.CSCOMSTRs.Where( x => x.CONO == cSCOPARENT.CONO).Select( x => x.CONAME).FirstOrDefault();
            ViewBag.CONOPARENT = new SelectList(db.CSCOMSTRs.OrderBy(x => x.CONAME), "CONO", "CONAME", cSCOPARENT.CONOPARENT);
            return View(cSCOPARENT);
        }

        // GET: CSCOPARENTs/Create
        public ActionResult Create(string id)
        {

            ViewBag.Parent = MyHtmlHelpers.ConvertByteStrToId(id);
            string sid = ViewBag.Parent;

            CSCOPARENT curRec = new CSCOPARENT();
            curRec.CONO = sid;
            curRec.ADATE = DateTime.Today;
            curRec.ENDDATE = new DateTime(3000, 1, 1);
            curRec.CSCOMSTR = db.CSCOMSTRs.Find(curRec.CONO);
            ViewBag.CONOPARENT = new SelectList(db.CSCOMSTRs.OrderBy( x => x.CONAME), "CONO", "CONAME");
            return View(curRec);
        }

        // POST: CSCOPARENTs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CONO,CONOPARENT,ADATE,RDATE,ENDDATE,REM,ROWNO,STAMP")] CSCOPARENT cSCOPARENT)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    int lastRowNo = 0;
                    try
                    {
                        lastRowNo = db.CSCOPARENTs.Where(m => m.CONO == cSCOPARENT.CONO).Max(n => n.ROWNO);
                    }
                    catch (Exception e) { lastRowNo = 0; }
                    finally { };

                    cSCOPARENT.STAMP = 0;
                    cSCOPARENT.ROWNO = lastRowNo + 1;
                    db.CSCOPARENTs.Add(cSCOPARENT);
                    db.SaveChanges();
                    return new RedirectResult(Url.Action("Edit", "CSCOMSTRs", new { id = MyHtmlHelpers.ConvertIdToByteStr(cSCOPARENT.CONO) }) + "#Parent");
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
            cSCOPARENT.CSCOMSTR = db.CSCOMSTRs.Find(cSCOPARENT.CONO);
            ViewBag.CONOPARENT = new SelectList(db.CSCOMSTRs.OrderBy(x => x.CONAME), "CONO", "CONAME", cSCOPARENT.CONOPARENT);
            return View(cSCOPARENT);
        }

        // GET: CSCOPARENTs/Edit/5
        public ActionResult Edit(string id, int row)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSCOPARENT cSCOPARENT = db.CSCOPARENTs.Find(MyHtmlHelpers.ConvertByteStrToId(id), row);
            if (cSCOPARENT == null)
            {
                return HttpNotFound();
            }
            cSCOPARENT.CSCOMSTR = db.CSCOMSTRs.Find(cSCOPARENT.CONO);
            ViewBag.CONOPARENT = new SelectList(db.CSCOMSTRs.OrderBy(x => x.CONAME), "CONO", "CONAME", cSCOPARENT.CONOPARENT);
            return View(cSCOPARENT);
        }

        // POST: CSCOPARENTs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CONO,CONOPARENT,ADATE,RDATE,ENDDATE,REM,ROWNO,STAMP")] CSCOPARENT cSCOPARENT)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    cSCOPARENT.STAMP = cSCOPARENT.STAMP + 1;
                    db.Entry(cSCOPARENT).State = EntityState.Modified;
                    db.SaveChanges();
                    return new RedirectResult(Url.Action("Edit", "CSCOMSTRs", new { id = MyHtmlHelpers.ConvertIdToByteStr(cSCOPARENT.CONO) }) + "#Parent");
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
            cSCOPARENT.CSCOMSTR = db.CSCOMSTRs.Find(cSCOPARENT.CONO);
            ViewBag.CONOPARENT = new SelectList(db.CSCOMSTRs.OrderBy(x => x.CONAME), "CONO", "CONAME", cSCOPARENT.CONOPARENT);
            return View(cSCOPARENT);
        }

        // GET: CSCOPARENTs/Delete/5
        public ActionResult Delete(string id, int row)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSCOPARENT cSCOPARENT = db.CSCOPARENTs.Find(MyHtmlHelpers.ConvertByteStrToId(id), row);
            if (cSCOPARENT == null)
            {
                return HttpNotFound();
            }
            ViewBag.CONAME = db.CSCOMSTRs.Where(x => x.CONO == cSCOPARENT.CONO).Select(x => x.CONAME).FirstOrDefault();
            return View(cSCOPARENT);
        }

        // POST: CSCOPARENTs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id, int row)
        {
            CSCOPARENT cSCOPARENT = db.CSCOPARENTs.Find(MyHtmlHelpers.ConvertByteStrToId(id), row);
            try
            {
                db.CSCOPARENTs.Remove(cSCOPARENT);
                db.SaveChanges();
                return new RedirectResult(Url.Action("Edit", "CSCOMSTRs", new { id = MyHtmlHelpers.ConvertIdToByteStr(cSCOPARENT.CONO) }) + "#Parent");
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
            ViewBag.CONAME = db.CSCOMSTRs.Where(x => x.CONO == cSCOPARENT.CONO).Select(x => x.CONAME).FirstOrDefault();
            return View(cSCOPARENT);
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
