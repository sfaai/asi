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
    public class CSCOAGTCHGsController : Controller
    {
        private ASIDBConnection db = new ASIDBConnection();

        // GET: CSCOAGTCHGs
        public ActionResult Index()
        {
            var cSCOAGTCHGs = db.CSCOAGTCHGs.Include(c => c.HKNATION).Include(c => c.HKRACE).Include(c => c.CSPRSADDR).Include(c => c.CSPRSREG).Include(c => c.CSPRSREG1);
            return View(cSCOAGTCHGs.ToList());
        }

        // GET: CSCOAGTCHGs/Details/5
        public ActionResult Details(string id, string person, DateTime adate, int row)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSCOAGTCHG cSCOAGTCHG = db.CSCOAGTCHGs.Find(MyHtmlHelpers.ConvertByteStrToId(id), person, adate, row);
            if (cSCOAGTCHG == null)
            {
                return HttpNotFound();
            }
            return View(cSCOAGTCHG);
        }

        // GET: CSCOAGTCHGs/Create
        public ActionResult Create(string id, string person, DateTime adate)
        {
            CSCOAGTCHG cSCOAGTCHG = new CSCOAGTCHG();

            cSCOAGTCHG.CONO = MyHtmlHelpers.ConvertByteStrToId(id);
            cSCOAGTCHG.PRSCODE = person;
            cSCOAGTCHG.ADATE = adate;
            cSCOAGTCHG.CHGEFFDATE = DateTime.Today;
            cSCOAGTCHG.PRSNAME = db.CSPRS.Where(x => x.PRSCODE == person).Select( x => x.PRSNAME).FirstOrDefault();

            ViewBag.NATION = new SelectList(db.HKNATIONs, "NATION", "NATION", cSCOAGTCHG.NATION);
            ViewBag.RACE = new SelectList(db.HKRACEs, "RACE", "RACE", cSCOAGTCHG.RACE);
            ViewBag.CHGREM = new SelectList(db.CSCHGREMs.OrderBy(x => x.CHGREM), "CHGREM", "CHGREM", cSCOAGTCHG.CHGREM);
            ViewBag.ADDRID = new SelectList(db.CSPRSADDRs.Where(x => x.PRSCODE == cSCOAGTCHG.PRSCODE).OrderBy(y => y.ADDRID).Select(z => new { ADDRID = z.ADDRID, MAILDET = z.ADDRID + " | " + z.MAILADDR + " | " + z.ADDRTYPE + " | " + z.ADDR1 + z.ADDR2 + z.ADDR3 }), "ADDRID", "MAILDET", cSCOAGTCHG.ADDRID);
            ViewBag.REGID1 = new SelectList(db.CSPRSREGs.Where(x => x.PRSCODE == cSCOAGTCHG.PRSCODE).Select(x => new { REGNO = x.REGNO, PRSNAME = x.CTRYCODE + " | " + x.REGTYPE + " | " + x.REGNO }), "REGNO", "PRSNAME", cSCOAGTCHG.REGID1);
            ViewBag.REGID2 = new SelectList(db.CSPRSREGs.Where(x => x.PRSCODE == cSCOAGTCHG.PRSCODE).Select(x => new { REGNO = x.REGNO, PRSNAME = x.CTRYCODE + " | " + x.REGTYPE + " | " + x.REGNO }), "REGNO", "PRSNAME", cSCOAGTCHG.REGID2);
            return View(cSCOAGTCHG);
        }

        // POST: CSCOAGTCHGs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CONO,PRSCODE,ADATE,CHGEFFDATE,CHGREM,PRSNAME,NATION,RACE,ADDRID,OCCUPATION,REGCTRY1,REGTYPE1,REGID1,REGCTRY2,REGTYPE2,REGID2,REM,ROWNO,STAMP")] CSCOAGTCHG cSCOAGTCHG)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    int lastRowNo = 0;
                    try
                    {
                        lastRowNo = db.CSCOAGTCHGs.Where(m => m.CONO == cSCOAGTCHG.CONO && m.PRSCODE == cSCOAGTCHG.PRSCODE && m.ADATE == cSCOAGTCHG.ADATE).Max(n => n.ROWNO);
                    }
                    catch (Exception e) { lastRowNo = 0; }
                    finally { };


                    cSCOAGTCHG.ROWNO = lastRowNo + 1;

                    cSCOAGTCHG.STAMP = 0;
                    db.CSCOAGTCHGs.Add(cSCOAGTCHG);
                    db.SaveChanges();
                    return new RedirectResult(Url.Action("Edit", "CSCOAGTs", new { id = MyHtmlHelpers.ConvertIdToByteStr(cSCOAGTCHG.CONO), person = cSCOAGTCHG.PRSCODE, adate = cSCOAGTCHG.ADATE }));
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

            ViewBag.NATION = new SelectList(db.HKNATIONs, "NATION", "NATION", cSCOAGTCHG.NATION);
            ViewBag.RACE = new SelectList(db.HKRACEs, "RACE", "RACE", cSCOAGTCHG.RACE);
            ViewBag.CHGREM = new SelectList(db.CSCHGREMs.OrderBy(x => x.CHGREM), "CHGREM", "CHGREM", cSCOAGTCHG.CHGREM);
            ViewBag.ADDRID = new SelectList(db.CSPRSADDRs.Where(x => x.PRSCODE == cSCOAGTCHG.PRSCODE).OrderBy(y => y.ADDRID).Select(z => new { ADDRID = z.ADDRID, MAILDET = z.ADDRID + " | " + z.MAILADDR + " | " + z.ADDRTYPE + " | " + z.ADDR1 + z.ADDR2 + z.ADDR3 }), "ADDRID", "MAILDET", cSCOAGTCHG.ADDRID);
            ViewBag.REGID1 = new SelectList(db.CSPRSREGs.Where(x => x.PRSCODE == cSCOAGTCHG.PRSCODE).Select ( x => new { REGNO = x.REGNO, PRSNAME = x.CTRYCODE + " | " + x.REGTYPE + " | " + x.REGNO }) ,  "REGNO", "PRSNAME", cSCOAGTCHG.REGID1);
            ViewBag.REGID2 = new SelectList(db.CSPRSREGs.Where(x => x.PRSCODE == cSCOAGTCHG.PRSCODE).Select(x => new { REGNO = x.REGNO, PRSNAME = x.CTRYCODE + " | " + x.REGTYPE + " | " + x.REGNO }),  "REGNO", "PRSNAME", cSCOAGTCHG.REGID2);
            return View(cSCOAGTCHG);
        }

        // GET: CSCOAGTCHGs/Edit/5
        public ActionResult Edit(string id, string person, DateTime adate, int row)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSCOAGTCHG cSCOAGTCHG = db.CSCOAGTCHGs.Find(MyHtmlHelpers.ConvertByteStrToId(id), person, adate, row);
            if (cSCOAGTCHG == null)
            {
                return HttpNotFound();
            }
            ViewBag.NATION = new SelectList(db.HKNATIONs, "NATION", "NATION", cSCOAGTCHG.NATION);
            ViewBag.RACE = new SelectList(db.HKRACEs, "RACE", "RACE", cSCOAGTCHG.RACE);
            ViewBag.CHGREM = new SelectList(db.CSCHGREMs.OrderBy( x => x.CHGREM), "CHGREM", "CHGREM", cSCOAGTCHG.CHGREM);
            ViewBag.ADDRID = new SelectList(db.CSPRSADDRs.Where(x => x.PRSCODE == cSCOAGTCHG.PRSCODE).OrderBy(y => y.ADDRID).Select(z => new { ADDRID = z.ADDRID, MAILDET = z.ADDRID + " | " + z.MAILADDR + " | " + z.ADDRTYPE + " | " + z.ADDR1 + z.ADDR2 + z.ADDR3 }), "ADDRID", "MAILDET", cSCOAGTCHG.ADDRID);
            ViewBag.REGID1 = new SelectList(db.CSPRSREGs.Where(x => x.PRSCODE == cSCOAGTCHG.PRSCODE).Select(x => new { REGNO = x.REGNO, PRSNAME = x.CTRYCODE + " | " + x.REGTYPE + " | " + x.REGNO }), "REGNO", "PRSNAME", cSCOAGTCHG.REGID1);
            ViewBag.REGID2 = new SelectList(db.CSPRSREGs.Where(x => x.PRSCODE == cSCOAGTCHG.PRSCODE).Select(x => new { REGNO = x.REGNO, PRSNAME = x.CTRYCODE + " | " + x.REGTYPE + " | " + x.REGNO }), "REGNO", "PRSNAME", cSCOAGTCHG.REGID2);
            return View(cSCOAGTCHG);
        }

        // POST: CSCOAGTCHGs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CONO,PRSCODE,ADATE,CHGEFFDATE,CHGREM,PRSNAME,NATION,RACE,ADDRID,OCCUPATION,REGCTRY1,REGTYPE1,REGID1,REGCTRY2,REGTYPE2,REGID2,REM,ROWNO,STAMP")] CSCOAGTCHG cSCOAGTCHG)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    cSCOAGTCHG.STAMP = cSCOAGTCHG.STAMP + 1;
                    db.Entry(cSCOAGTCHG).State = EntityState.Modified;
                    db.SaveChanges();
                    return new RedirectResult(Url.Action("Edit", "CSCOAGTs", new { id = MyHtmlHelpers.ConvertIdToByteStr(cSCOAGTCHG.CONO), person = cSCOAGTCHG.PRSCODE, adate = cSCOAGTCHG.ADATE }));
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
            ViewBag.NATION = new SelectList(db.HKNATIONs, "NATION", "NATION", cSCOAGTCHG.NATION);
            ViewBag.RACE = new SelectList(db.HKRACEs, "RACE", "RACE", cSCOAGTCHG.RACE);
            ViewBag.CHGREM = new SelectList(db.CSCHGREMs.OrderBy(x => x.CHGREM), "CHGREM", "CHGREM", cSCOAGTCHG.CHGREM);
            ViewBag.ADDRID = new SelectList(db.CSPRSADDRs.Where(x => x.PRSCODE == cSCOAGTCHG.PRSCODE).OrderBy(y => y.ADDRID).Select(z => new { ADDRID = z.ADDRID, MAILDET = z.ADDRID + " | " + z.MAILADDR + " | " + z.ADDRTYPE + " | " + z.ADDR1 + z.ADDR2 + z.ADDR3 }), "ADDRID", "MAILDET", cSCOAGTCHG.ADDRID);
            ViewBag.REGID1 = new SelectList(db.CSPRSREGs.Where(x => x.PRSCODE == cSCOAGTCHG.PRSCODE).Select(x => new { REGNO = x.REGNO, PRSNAME = x.CTRYCODE + " | " + x.REGTYPE + " | " + x.REGNO }), "REGNO", "PRSNAME", cSCOAGTCHG.REGID1);
            ViewBag.REGID2 = new SelectList(db.CSPRSREGs.Where(x => x.PRSCODE == cSCOAGTCHG.PRSCODE).Select(x => new { REGNO = x.REGNO, PRSNAME = x.CTRYCODE + " | " + x.REGTYPE + " | " + x.REGNO }), "REGNO", "PRSNAME", cSCOAGTCHG.REGID2);            
            return View(cSCOAGTCHG);
        }

        // GET: CSCOAGTCHGs/Delete/5
        public ActionResult Delete(string id, string person, DateTime adate, int row)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSCOAGTCHG cSCOAGTCHG = db.CSCOAGTCHGs.Find(MyHtmlHelpers.ConvertByteStrToId(id), person, adate, row);
            if (cSCOAGTCHG == null)
            {
                return HttpNotFound();
            }
            return View(cSCOAGTCHG);
        }

        // POST: CSCOAGTCHGs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id, string person, DateTime adate, int row)
        {
            CSCOAGTCHG cSCOAGTCHG = db.CSCOAGTCHGs.Find(MyHtmlHelpers.ConvertByteStrToId(id), person, adate, row);
            try
            {
               
                db.CSCOAGTCHGs.Remove(cSCOAGTCHG);
                db.SaveChanges();
                return new RedirectResult(Url.Action("Edit", "CSCOAGTs", new { id = MyHtmlHelpers.ConvertIdToByteStr(cSCOAGTCHG.CONO), person = cSCOAGTCHG.PRSCODE, adate = cSCOAGTCHG.ADATE }));
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

            return View(cSCOAGTCHG);
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
