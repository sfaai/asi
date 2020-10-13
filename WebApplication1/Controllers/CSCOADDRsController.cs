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
    public class CSCOADDRsController : Controller
    {
        private ASIDBConnection db = new ASIDBConnection();

        // GET: CSCOADDRs
        public ActionResult Index()
        {
            var cSCOADDRs = db.CSCOADDRs.Include(c => c.HKCITY).Include(c => c.HKSTATE).Include(c => c.HKCTRY);
            return View(cSCOADDRs.ToList());
        }

        // GET: CSCOADDRs/Details/5
        public ActionResult Details(string id, short row)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSCOADDR cSCOADDR = db.CSCOADDRs.Find(MyHtmlHelpers.ConvertByteStrToId(id), row);
            if (cSCOADDR == null)
            {
                return HttpNotFound();
            }
            cSCOADDR.CSCOMSTR = db.CSCOMSTRs.Find(cSCOADDR.CONO);
            ViewBag.ADDRTYPE = cSCOADDR.addrtypeList;
            ViewBag.CITYCODE = new SelectList(db.HKCITies.OrderBy(x => x.CITYCODE), "CITYCODE", "CITYDESC", cSCOADDR.CITYCODE);
            ViewBag.STATECODE = new SelectList(db.HKSTATEs.OrderBy(x => x.STATECODE), "STATECODE", "STATEDESC", cSCOADDR.STATECODE);
            ViewBag.CTRYCODE = new SelectList(db.HKCTRies.OrderBy(x => x.CTRYCODE), "CTRYCODE", "CTRYDESC", cSCOADDR.CTRYCODE);
            return View(cSCOADDR);
        }

        // GET: CSCOADDRs/Create
        public ActionResult Create(string id)
        {
            ViewBag.Parent = MyHtmlHelpers.ConvertByteStrToId(id);
            CSCOADDR cSCOADDR = new CSCOADDR();
            cSCOADDR.CONO = ViewBag.Parent;

            cSCOADDR.SDATE = DateTime.Today;         
            cSCOADDR.ENDDATE = new DateTime(3000, 1, 1);
            cSCOADDR.EDATE = cSCOADDR.ENDDATE;
            cSCOADDR.MAILADDRBool = true;
            cSCOADDR.CSCOMSTR = db.CSCOMSTRs.Find(cSCOADDR.CONO);

            ViewBag.ADDRTYPE = cSCOADDR.addrtypeList;
            ViewBag.CITYCODE = new SelectList(db.HKCITies.OrderBy(x => x.CITYCODE), "CITYCODE", "CITYDESC", cSCOADDR.CITYCODE);
            ViewBag.STATECODE = new SelectList(db.HKSTATEs.OrderBy(x => x.STATECODE), "STATECODE", "STATEDESC", cSCOADDR.STATECODE);
            ViewBag.CTRYCODE = new SelectList(db.HKCTRies.OrderBy(x => x.CTRYCODE), "CTRYCODE", "CTRYDESC", cSCOADDR.CTRYCODE);
            return View(cSCOADDR);
        }

        // POST: CSCOADDRs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CONO,ADDRID,MAILADDRBool,ADDRTYPE,ADDR1,ADDR2,ADDR3,POSTAL,CITYCODE,STATECODE,CTRYCODE,PHONE1,PHONE2,FAX1,FAX2,OPRHRS,OPRHRSStr,REM,SDATE,EDATE,ENDDATE,STAMP")] CSCOADDR cSCOADDR)
        {
            if (ModelState.IsValid)
            {
                short lastRowNo = 0;
                try
                {
                    lastRowNo = db.CSCOADDRs.Where(m => m.CONO == cSCOADDR.CONO).Max(n => n.ADDRID);
                }
                catch (Exception e) { lastRowNo = 0; }
                finally { };

                try
                {
                    cSCOADDR.STAMP = 0;
                    cSCOADDR.ADDRID = (short)(lastRowNo + 1);
                    db.CSCOADDRs.Add(cSCOADDR);
                    db.SaveChanges();
                    return new RedirectResult(Url.Action("Edit", "CSCOMSTRs", new { id = MyHtmlHelpers.ConvertIdToByteStr(cSCOADDR.CONO) }) + "#Address");
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
            cSCOADDR.CSCOMSTR = db.CSCOMSTRs.Find(cSCOADDR.CONO);
            ViewBag.ADDRTYPE = cSCOADDR.addrtypeList;
            ViewBag.CITYCODE = new SelectList(db.HKCITies.OrderBy(x => x.CITYCODE), "CITYCODE", "CITYDESC", cSCOADDR.CITYCODE);
            ViewBag.STATECODE = new SelectList(db.HKSTATEs.OrderBy(x => x.STATECODE), "STATECODE", "STATEDESC", cSCOADDR.STATECODE);
            ViewBag.CTRYCODE = new SelectList(db.HKCTRies.OrderBy(x => x.CTRYCODE), "CTRYCODE", "CTRYDESC", cSCOADDR.CTRYCODE);
            return View(cSCOADDR);
        }

        // GET: CSCOADDRs/Edit/5
        public ActionResult Edit(string id, short row)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSCOADDR cSCOADDR = db.CSCOADDRs.Find(MyHtmlHelpers.ConvertByteStrToId(id), row);
            if (cSCOADDR == null)
            {
                return HttpNotFound();
            }
            ViewBag.ADDRTYPE = cSCOADDR.addrtypeList;
            ViewBag.CITYCODE = new SelectList(db.HKCITies.OrderBy(x => x.CITYCODE), "CITYCODE", "CITYDESC", cSCOADDR.CITYCODE);
            ViewBag.STATECODE = new SelectList(db.HKSTATEs.OrderBy(x => x.STATECODE), "STATECODE", "STATEDESC", cSCOADDR.STATECODE);
            ViewBag.CTRYCODE = new SelectList(db.HKCTRies.OrderBy(x => x.CTRYCODE), "CTRYCODE", "CTRYDESC", cSCOADDR.CTRYCODE);
            return View(cSCOADDR);
        }

        // POST: CSCOADDRs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CONO,ADDRID,MAILADDRBool,ADDRTYPE,ADDR1,ADDR2,ADDR3,POSTAL,CITYCODE,STATECODE,CTRYCODE,PHONE1,PHONE2,FAX1,FAX2,OPRHRS,OPRHRSStr,REM,SDATE,EDATE,ENDDATE,STAMP")] CSCOADDR cSCOADDR)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    cSCOADDR.STAMP = cSCOADDR.STAMP + 1;
                    db.Entry(cSCOADDR).State = EntityState.Modified;
                    db.SaveChanges();
                    return new RedirectResult(Url.Action("Edit", "CSCOMSTRs", new { id = MyHtmlHelpers.ConvertIdToByteStr(cSCOADDR.CONO) }) + "#Address");
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
                catch (Exception e)
                {
                    ModelState.AddModelError(string.Empty, e.Message);
                }
            }
            cSCOADDR.CSCOMSTR = db.CSCOMSTRs.Find(cSCOADDR.CONO);
            ViewBag.ADDRTYPE = cSCOADDR.addrtypeList;
            ViewBag.CITYCODE = new SelectList(db.HKCITies.OrderBy(x => x.CITYCODE), "CITYCODE", "CITYDESC", cSCOADDR.CITYCODE);
            ViewBag.STATECODE = new SelectList(db.HKSTATEs.OrderBy(x => x.STATECODE), "STATECODE", "STATEDESC", cSCOADDR.STATECODE);
            ViewBag.CTRYCODE = new SelectList(db.HKCTRies.OrderBy(x => x.CTRYCODE), "CTRYCODE", "CTRYDESC", cSCOADDR.CTRYCODE);
            return View(cSCOADDR);
        }

        // GET: CSCOADDRs/Delete/5
        public ActionResult Delete(string id, short row)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSCOADDR cSCOADDR = db.CSCOADDRs.Find(MyHtmlHelpers.ConvertByteStrToId(id), row);
            if (cSCOADDR == null)
            {
                return HttpNotFound();
            }
            return View(cSCOADDR);
        }

        // POST: CSCOADDRs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id, short row)
        {
            CSCOADDR cSCOADDR = db.CSCOADDRs.Find(MyHtmlHelpers.ConvertByteStrToId(id), row);
            try
            {
                db.CSCOADDRs.Remove(cSCOADDR);
                db.SaveChanges();
                return new RedirectResult(Url.Action("Edit", "CSCOMSTRs", new { id = MyHtmlHelpers.ConvertIdToByteStr(cSCOADDR.CONO) }) + "#Address");
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
            cSCOADDR.CSCOMSTR = db.CSCOMSTRs.Find(cSCOADDR.CONO);
            return View(cSCOADDR);
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
