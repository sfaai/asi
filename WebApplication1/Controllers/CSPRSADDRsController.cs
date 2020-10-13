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
    public class CSPRSADDRsController : Controller
    {
        private ASIDBConnection db = new ASIDBConnection();

        // GET: CSPRSADDRs
        public ActionResult Index()
        {
            var cSPRSADDRs = db.CSPRSADDRs.Include(c => c.HKCITY).Include(c => c.HKSTATE).Include(c => c.HKCTRY);
            return View(cSPRSADDRs.ToList());
        }

        // GET: CSPRSADDRs/Details/5
        public ActionResult Details(string id, int row)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSPRSADDR cSPRSADDR = db.CSPRSADDRs.Find(MyHtmlHelpers.ConvertByteStrToId(id), row);
            if (cSPRSADDR == null)
            {
                return HttpNotFound();
            }
            ViewBag.ADDRTYPE = cSPRSADDR.addrtypeList;
            ViewBag.CITYCODE = new SelectList(db.HKCITies.OrderBy(x => x.CITYCODE), "CITYCODE", "CITYDESC", cSPRSADDR.CITYCODE);
            ViewBag.STATECODE = new SelectList(db.HKSTATEs.OrderBy(x => x.STATECODE), "STATECODE", "STATEDESC", cSPRSADDR.STATECODE);
            ViewBag.CTRYCODE = new SelectList(db.HKCTRies.OrderBy(x => x.CTRYCODE), "CTRYCODE", "CTRYDESC", cSPRSADDR.CTRYCODE);
            return View(cSPRSADDR);
        }

        // GET: CSPRSADDRs/Create
        public ActionResult Create(string id)
        {
            var cSPRSADDR = new CSPRSADDR();
            cSPRSADDR.PRSCODE = MyHtmlHelpers.ConvertByteStrToId(id);
            ViewBag.ADDRTYPE = cSPRSADDR.addrtypeList;
            ViewBag.CITYCODE = new SelectList(db.HKCITies.OrderBy(x => x.CITYCODE), "CITYCODE", "CITYDESC", cSPRSADDR.CITYCODE);
            ViewBag.STATECODE = new SelectList(db.HKSTATEs.OrderBy(x => x.STATECODE), "STATECODE", "STATEDESC", cSPRSADDR.STATECODE);
            ViewBag.CTRYCODE = new SelectList(db.HKCTRies.OrderBy(x => x.CTRYCODE), "CTRYCODE", "CTRYDESC", cSPRSADDR.CTRYCODE);
            return View(cSPRSADDR);
        }

        // POST: CSPRSADDRs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PRSCODE,ADDRID,MAILADDR,MAILADDRBool,ADDRTYPE,ADDR1,ADDR2,ADDR3,POSTAL,CITYCODE,STATECODE,CTRYCODE,PHONE1,PHONE2,FAX1,FAX2,REM,STAMP")] CSPRSADDR cSPRSADDR)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    cSPRSADDR.STAMP = 0;
                    short max = 0;
                    try
                    {
                        max = db.CSPRSADDRs.Where(x => x.PRSCODE == cSPRSADDR.PRSCODE).Max(x => x.ADDRID);
                    }
                    catch { max = 0; }
                    max++;
                    cSPRSADDR.ADDRID = max;
                    db.CSPRSADDRs.Add(cSPRSADDR);
                    db.SaveChanges();
                    return RedirectToAction("Edit", "CSPRs", new { id = MyHtmlHelpers.ConvertIdToByteStr(cSPRSADDR.PRSCODE) });
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
            ViewBag.ADDRTYPE = cSPRSADDR.addrtypeList;
            ViewBag.CITYCODE = new SelectList(db.HKCITies.OrderBy(x => x.CITYCODE), "CITYCODE", "CITYDESC", cSPRSADDR.CITYCODE);
            ViewBag.STATECODE = new SelectList(db.HKSTATEs.OrderBy(x => x.STATECODE), "STATECODE", "STATEDESC", cSPRSADDR.STATECODE);
            ViewBag.CTRYCODE = new SelectList(db.HKCTRies.OrderBy(x => x.CTRYCODE), "CTRYCODE", "CTRYDESC", cSPRSADDR.CTRYCODE);
            return View(cSPRSADDR);
        }

        // GET: CSPRSADDRs/Edit/5
        public ActionResult Edit(string id, int row)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSPRSADDR cSPRSADDR = db.CSPRSADDRs.Find(MyHtmlHelpers.ConvertByteStrToId(id), row);
            if (cSPRSADDR == null)
            {
                return HttpNotFound();
            }
            ViewBag.ADDRTYPE = cSPRSADDR.addrtypeList;
            ViewBag.CITYCODE = new SelectList(db.HKCITies.OrderBy(x => x.CITYCODE), "CITYCODE", "CITYDESC", cSPRSADDR.CITYCODE);
            ViewBag.STATECODE = new SelectList(db.HKSTATEs.OrderBy(x => x.STATECODE), "STATECODE", "STATEDESC", cSPRSADDR.STATECODE);
            ViewBag.CTRYCODE = new SelectList(db.HKCTRies.OrderBy(x => x.CTRYCODE), "CTRYCODE", "CTRYDESC", cSPRSADDR.CTRYCODE);
            return View(cSPRSADDR);
        }

        // POST: CSPRSADDRs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PRSCODE,ADDRID,MAILADDR,MAILADDRBool,ADDRTYPE,ADDR1,ADDR2,ADDR3,POSTAL,CITYCODE,STATECODE,CTRYCODE,PHONE1,PHONE2,FAX1,FAX2,REM,STAMP")] CSPRSADDR cSPRSADDR)
        {
            if (ModelState.IsValid)
            {try
                {
                    cSPRSADDR.STAMP = cSPRSADDR.STAMP + 1;
                    db.Entry(cSPRSADDR).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Edit", "CSPRs", new { id = MyHtmlHelpers.ConvertIdToByteStr(cSPRSADDR.PRSCODE) });
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
            ViewBag.ADDRTYPE = cSPRSADDR.addrtypeList;
            ViewBag.CITYCODE = new SelectList(db.HKCITies.OrderBy(x => x.CITYCODE), "CITYCODE", "CITYDESC", cSPRSADDR.CITYCODE);
            ViewBag.STATECODE = new SelectList(db.HKSTATEs.OrderBy(x => x.STATECODE), "STATECODE", "STATEDESC", cSPRSADDR.STATECODE);
            ViewBag.CTRYCODE = new SelectList(db.HKCTRies.OrderBy(x => x.CTRYCODE), "CTRYCODE", "CTRYDESC", cSPRSADDR.CTRYCODE);
            return View(cSPRSADDR);
        }

        // GET: CSPRSADDRs/Delete/5
        public ActionResult Delete(string id, int row)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSPRSADDR cSPRSADDR = db.CSPRSADDRs.Find(MyHtmlHelpers.ConvertByteStrToId(id), row);
            if (cSPRSADDR == null)
            {
                return HttpNotFound();
            }
            return View(cSPRSADDR);
        }

        // POST: CSPRSADDRs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id, int row)
        {
            CSPRSADDR cSPRSADDR = db.CSPRSADDRs.Find(MyHtmlHelpers.ConvertByteStrToId(id), row);
            try
            {
              
                db.CSPRSADDRs.Remove(cSPRSADDR);
                db.SaveChanges();
                return RedirectToAction("Edit", "CSPRs", new { id = MyHtmlHelpers.ConvertIdToByteStr(cSPRSADDR.PRSCODE) });
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
            return View(cSPRSADDR);
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
