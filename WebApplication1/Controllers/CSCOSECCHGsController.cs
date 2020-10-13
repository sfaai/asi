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
    public class CSCOSECCHGsController : Controller
    {
        private ASIDBConnection db = new ASIDBConnection();

        // GET: CSCOSECCHGs
        public ActionResult Index()
        {
            var cSCOSECCHGs = db.CSCOSECCHGs.Include(c => c.HKNATION).Include(c => c.HKRACE).Include(c => c.CSPRSADDR).Include(c => c.CSPRSREG).Include(c => c.CSPRSREG1).Include(c => c.CSPRSREG2);
            return View(cSCOSECCHGs.ToList());
        }

        // GET: CSCOSECCHGs/Details/5
        public ActionResult Details(string id, string person, DateTime adate, int row)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSCOSECCHG cSCOSECCHG = db.CSCOSECCHGs.Find(MyHtmlHelpers.ConvertByteStrToId(id), person, adate, row);
            if (cSCOSECCHG == null)
            {
                return HttpNotFound();
            }
            return View(cSCOSECCHG);
        }

        // GET: CSCOSECCHGs/Create
        public ActionResult Create(string id, string person, DateTime adate)
        {
            CSCOSECCHG cSCOSECCHG = new CSCOSECCHG();

            cSCOSECCHG.CONO = MyHtmlHelpers.ConvertByteStrToId(id);
            cSCOSECCHG.PRSCODE = person;
            cSCOSECCHG.ADATE = adate;
            cSCOSECCHG.CHGEFFDATE = DateTime.Today;
            cSCOSECCHG.PRSNAME = db.CSPRS.Where(x => x.PRSCODE == person).Select(x => x.PRSNAME).FirstOrDefault();

            ViewBag.NATION = new SelectList(db.HKNATIONs, "NATION", "NATION", cSCOSECCHG.NATION);
            ViewBag.RACE = new SelectList(db.HKRACEs, "RACE", "RACE", cSCOSECCHG.RACE);
            ViewBag.CHGREM = new SelectList(db.CSCHGREMs.OrderBy(x => x.CHGREM), "CHGREM", "CHGREM", cSCOSECCHG.CHGREM);
            ViewBag.ADDRID = new SelectList(db.CSPRSADDRs.Where(x => x.PRSCODE == cSCOSECCHG.PRSCODE).OrderBy(y => y.ADDRID).Select(z => new { ADDRID = z.ADDRID, MAILDET = z.ADDRID + " | " + z.MAILADDR + " | " + z.ADDRTYPE + " | " + z.ADDR1 + z.ADDR2 + z.ADDR3 }), "ADDRID", "MAILDET", cSCOSECCHG.ADDRID);
            ViewBag.REGID1 = new SelectList(db.CSPRSREGs.Where(x => x.PRSCODE == cSCOSECCHG.PRSCODE).Select(x => new { REGNO = x.REGNO, PRSNAME = x.CTRYCODE + " | " + x.REGTYPE + " | " + x.REGNO }), "REGNO", "PRSNAME", cSCOSECCHG.REGID1);
            ViewBag.REGID2 = new SelectList(db.CSPRSREGs.Where(x => x.PRSCODE == cSCOSECCHG.PRSCODE).Select(x => new { REGNO = x.REGNO, PRSNAME = x.CTRYCODE + " | " + x.REGTYPE + " | " + x.REGNO }), "REGNO", "PRSNAME", cSCOSECCHG.REGID2);
            ViewBag.REGID3 = new SelectList(db.CSPRSREGs.Where(x => x.PRSCODE == cSCOSECCHG.PRSCODE).Select(x => new { REGNO = x.REGNO, PRSNAME = x.CTRYCODE + " | " + x.REGTYPE + " | " + x.REGNO }), "REGNO", "PRSNAME", cSCOSECCHG.REGID3);
            return View(cSCOSECCHG);
        }

        // POST: CSCOSECCHGs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CONO,PRSCODE,ADATE,CHGEFFDATE,CHGREM,PRSNAME,NATION,RACE,ADDRID,OCCUPATION,REGCTRY1,REGTYPE1,REGID1,REGCTRY2,REGTYPE2,REGID2,REGCTRY3,REGTYPE3,REGID3,REM,ROWNO,STAMP")] CSCOSECCHG cSCOSECCHG)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    int lastRowNo = 0;
                    try
                    {
                        lastRowNo = db.CSCOSECCHGs.Where(m => m.CONO == cSCOSECCHG.CONO && m.PRSCODE == cSCOSECCHG.PRSCODE && m.ADATE == cSCOSECCHG.ADATE).Max(n => n.ROWNO);
                    }
                    catch (Exception e) { lastRowNo = 0; }
                    finally { };


                    cSCOSECCHG.ROWNO = lastRowNo + 1;
                    cSCOSECCHG.STAMP = 0;

                    db.CSCOSECCHGs.Add(cSCOSECCHG);
                    db.SaveChanges();
                    return new RedirectResult(Url.Action("Edit", "CSCOSECs", new { id = MyHtmlHelpers.ConvertIdToByteStr(cSCOSECCHG.CONO), person = cSCOSECCHG.PRSCODE, adate = cSCOSECCHG.ADATE }));
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

            ViewBag.NATION = new SelectList(db.HKNATIONs, "NATION", "NATION", cSCOSECCHG.NATION);
            ViewBag.RACE = new SelectList(db.HKRACEs, "RACE", "RACE", cSCOSECCHG.RACE);
            ViewBag.CHGREM = new SelectList(db.CSCHGREMs.OrderBy(x => x.CHGREM), "CHGREM", "CHGREM", cSCOSECCHG.CHGREM);
            ViewBag.ADDRID = new SelectList(db.CSPRSADDRs.Where(x => x.PRSCODE == cSCOSECCHG.PRSCODE).OrderBy(y => y.ADDRID).Select(z => new { ADDRID = z.ADDRID, MAILDET = z.ADDRID + " | " + z.MAILADDR + " | " + z.ADDRTYPE + " | " + z.ADDR1 + z.ADDR2 + z.ADDR3 }), "ADDRID", "MAILDET", cSCOSECCHG.ADDRID);
            ViewBag.REGID1 = new SelectList(db.CSPRSREGs.Where(x => x.PRSCODE == cSCOSECCHG.PRSCODE).Select(x => new { REGNO = x.REGNO, PRSNAME = x.CTRYCODE + " | " + x.REGTYPE + " | " + x.REGNO }), "REGNO", "PRSNAME", cSCOSECCHG.REGID1);
            ViewBag.REGID2 = new SelectList(db.CSPRSREGs.Where(x => x.PRSCODE == cSCOSECCHG.PRSCODE).Select(x => new { REGNO = x.REGNO, PRSNAME = x.CTRYCODE + " | " + x.REGTYPE + " | " + x.REGNO }), "REGNO", "PRSNAME", cSCOSECCHG.REGID2);
            ViewBag.REGID3 = new SelectList(db.CSPRSREGs.Where(x => x.PRSCODE == cSCOSECCHG.PRSCODE).Select(x => new { REGNO = x.REGNO, PRSNAME = x.CTRYCODE + " | " + x.REGTYPE + " | " + x.REGNO }), "REGNO", "PRSNAME", cSCOSECCHG.REGID3);
            return View(cSCOSECCHG);
        }

        // GET: CSCOSECCHGs/Edit/5
        public ActionResult Edit(string id, string person, DateTime adate, int row)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSCOSECCHG cSCOSECCHG = db.CSCOSECCHGs.Find(MyHtmlHelpers.ConvertByteStrToId(id), person, adate, row);
            if (cSCOSECCHG == null)
            {
                return HttpNotFound();
            }
            ViewBag.NATION = new SelectList(db.HKNATIONs, "NATION", "NATION", cSCOSECCHG.NATION);
            ViewBag.RACE = new SelectList(db.HKRACEs, "RACE", "RACE", cSCOSECCHG.RACE);
            ViewBag.CHGREM = new SelectList(db.CSCHGREMs.OrderBy(x => x.CHGREM), "CHGREM", "CHGREM", cSCOSECCHG.CHGREM);
            ViewBag.ADDRID = new SelectList(db.CSPRSADDRs.Where(x => x.PRSCODE == cSCOSECCHG.PRSCODE).OrderBy(y => y.ADDRID).Select(z => new { ADDRID = z.ADDRID, MAILDET = z.ADDRID + " | " + z.MAILADDR + " | " + z.ADDRTYPE + " | " + z.ADDR1 + z.ADDR2 + z.ADDR3 }), "ADDRID", "MAILDET", cSCOSECCHG.ADDRID);
            ViewBag.REGID1 = new SelectList(db.CSPRSREGs.Where(x => x.PRSCODE == cSCOSECCHG.PRSCODE).Select(x => new { REGNO = x.REGNO, PRSNAME = x.CTRYCODE + " | " + x.REGTYPE + " | " + x.REGNO }), "REGNO", "PRSNAME", cSCOSECCHG.REGID1);
            ViewBag.REGID2 = new SelectList(db.CSPRSREGs.Where(x => x.PRSCODE == cSCOSECCHG.PRSCODE).Select(x => new { REGNO = x.REGNO, PRSNAME = x.CTRYCODE + " | " + x.REGTYPE + " | " + x.REGNO }), "REGNO", "PRSNAME", cSCOSECCHG.REGID2);
            ViewBag.REGID3 = new SelectList(db.CSPRSREGs.Where(x => x.PRSCODE == cSCOSECCHG.PRSCODE).Select(x => new { REGNO = x.REGNO, PRSNAME = x.CTRYCODE + " | " + x.REGTYPE + " | " + x.REGNO }), "REGNO", "PRSNAME", cSCOSECCHG.REGID3);
            return View(cSCOSECCHG);
        }

        // POST: CSCOSECCHGs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CONO,PRSCODE,ADATE,CHGEFFDATE,CHGREM,PRSNAME,NATION,RACE,ADDRID,OCCUPATION,REGCTRY1,REGTYPE1,REGID1,REGCTRY2,REGTYPE2,REGID2,REGCTRY3,REGTYPE3,REGID3,REM,ROWNO,STAMP")] CSCOSECCHG cSCOSECCHG)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    cSCOSECCHG.STAMP = cSCOSECCHG.STAMP + 1;
                    db.Entry(cSCOSECCHG).State = EntityState.Modified;
                    db.SaveChanges();
                    return new RedirectResult(Url.Action("Edit", "CSCOSECs", new { id = MyHtmlHelpers.ConvertIdToByteStr(cSCOSECCHG.CONO), person = cSCOSECCHG.PRSCODE, adate = cSCOSECCHG.ADATE }));
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
            ViewBag.NATION = new SelectList(db.HKNATIONs, "NATION", "NATION", cSCOSECCHG.NATION);
            ViewBag.RACE = new SelectList(db.HKRACEs, "RACE", "RACE", cSCOSECCHG.RACE);
            ViewBag.CHGREM = new SelectList(db.CSCHGREMs.OrderBy(x => x.CHGREM), "CHGREM", "CHGREM", cSCOSECCHG.CHGREM);
            ViewBag.ADDRID = new SelectList(db.CSPRSADDRs.Where(x => x.PRSCODE == cSCOSECCHG.PRSCODE).OrderBy(y => y.ADDRID).Select(z => new { ADDRID = z.ADDRID, MAILDET = z.ADDRID + " | " + z.MAILADDR + " | " + z.ADDRTYPE + " | " + z.ADDR1 + z.ADDR2 + z.ADDR3 }), "ADDRID", "MAILDET", cSCOSECCHG.ADDRID);
            ViewBag.REGID1 = new SelectList(db.CSPRSREGs.Where(x => x.PRSCODE == cSCOSECCHG.PRSCODE).Select(x => new { REGNO = x.REGNO, PRSNAME = x.CTRYCODE + " | " + x.REGTYPE + " | " + x.REGNO }), "REGNO", "PRSNAME", cSCOSECCHG.REGID1);
            ViewBag.REGID2 = new SelectList(db.CSPRSREGs.Where(x => x.PRSCODE == cSCOSECCHG.PRSCODE).Select(x => new { REGNO = x.REGNO, PRSNAME = x.CTRYCODE + " | " + x.REGTYPE + " | " + x.REGNO }), "REGNO", "PRSNAME", cSCOSECCHG.REGID2);
            ViewBag.REGID3 = new SelectList(db.CSPRSREGs.Where(x => x.PRSCODE == cSCOSECCHG.PRSCODE).Select(x => new { REGNO = x.REGNO, PRSNAME = x.CTRYCODE + " | " + x.REGTYPE + " | " + x.REGNO }), "REGNO", "PRSNAME", cSCOSECCHG.REGID3);
            return View(cSCOSECCHG);
        }

        // GET: CSCOSECCHGs/Delete/5
        public ActionResult Delete(string id, string person, DateTime adate, int row)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSCOSECCHG cSCOSECCHG = db.CSCOSECCHGs.Find(MyHtmlHelpers.ConvertByteStrToId(id), person, adate, row);
            if (cSCOSECCHG == null)
            {
                return HttpNotFound();
            }
            return View(cSCOSECCHG);
        }

        // POST: CSCOSECCHGs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id, string person, DateTime adate, int row)
        {
            CSCOSECCHG cSCOSECCHG = db.CSCOSECCHGs.Find(MyHtmlHelpers.ConvertByteStrToId(id), person, adate, row);
            try
            {
                db.CSCOSECCHGs.Remove(cSCOSECCHG);
                db.SaveChanges();
                return new RedirectResult(Url.Action("Edit", "CSCOSECs", new { id = MyHtmlHelpers.ConvertIdToByteStr(cSCOSECCHG.CONO), person = cSCOSECCHG.PRSCODE, adate = cSCOSECCHG.ADATE }));
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
            return View(cSCOSECCHG);
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
