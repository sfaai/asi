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
    public class CSCODRCHGsController : Controller
    {
        private ASIDBConnection db = new ASIDBConnection();

        // GET: CSCODRCHGs
        public ActionResult Index()
        {
            var cSCODRCHGs = db.CSCODRCHGs.Include(c => c.HKNATION).Include(c => c.HKRACE).Include(c => c.CSPRSADDR).Include(c => c.CSPRSREG).Include(c => c.CSPRSREG1);
            return View(cSCODRCHGs.ToList());
        }

        // GET: CSCODRCHGs/Details/5
        public ActionResult Details(string id, string person, DateTime adate, int row)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSCODRCHG cSCODRCHG = db.CSCODRCHGs.Find(MyHtmlHelpers.ConvertByteStrToId(id), person, adate, row);
            if (cSCODRCHG == null)
            {
                return HttpNotFound();
            }
            return View(cSCODRCHG);
        }

        // GET: CSCODRCHGs/Create
        public ActionResult Create(string id, string person, DateTime adate)
        {
            CSCODRCHG cSCODRCHG = new CSCODRCHG();

            cSCODRCHG.CONO = MyHtmlHelpers.ConvertByteStrToId(id);
            cSCODRCHG.PRSCODE = person;
            cSCODRCHG.ADATE = adate;
            cSCODRCHG.CHGEFFDATE = DateTime.Today;
            cSCODRCHG.PRSNAME = db.CSPRS.Where(x => x.PRSCODE == person).Select(x => x.PRSNAME).FirstOrDefault();

            ViewBag.NATION = new SelectList(db.HKNATIONs, "NATION", "NATION", cSCODRCHG.NATION);
            ViewBag.RACE = new SelectList(db.HKRACEs, "RACE", "RACE", cSCODRCHG.RACE);
            ViewBag.CHGREM = new SelectList(db.CSCHGREMs.OrderBy(x => x.CHGREM), "CHGREM", "CHGREM", cSCODRCHG.CHGREM);
            ViewBag.ADDRID = new SelectList(db.CSPRSADDRs.Where(x => x.PRSCODE == cSCODRCHG.PRSCODE).OrderBy(y => y.ADDRID).Select(z => new { ADDRID = z.ADDRID, MAILDET = z.ADDRID + " | " + z.MAILADDR + " | " + z.ADDRTYPE + " | " + z.ADDR1 + z.ADDR2 + z.ADDR3 }), "ADDRID", "MAILDET", cSCODRCHG.ADDRID);
            ViewBag.REGID1 = new SelectList(db.CSPRSREGs.Where(x => x.PRSCODE == cSCODRCHG.PRSCODE).Select(x => new { REGNO = x.REGNO, PRSNAME = x.CTRYCODE + " | " + x.REGTYPE + " | " + x.REGNO }), "REGNO", "PRSNAME", cSCODRCHG.REGID1);
            ViewBag.REGID2 = new SelectList(db.CSPRSREGs.Where(x => x.PRSCODE == cSCODRCHG.PRSCODE).Select(x => new { REGNO = x.REGNO, PRSNAME = x.CTRYCODE + " | " + x.REGTYPE + " | " + x.REGNO }), "REGNO", "PRSNAME", cSCODRCHG.REGID2);
            return View(cSCODRCHG);
        }

        // POST: CSCODRCHGs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CONO,PRSCODE,ADATE,CHGEFFDATE,CHGREM,PRSNAME,NATION,RACE,ADDRID,OCCUPATION,ODRSHIP,REGCTRY1,REGTYPE1,REGID1,REGCTRY2,REGTYPE2,REGID2,REM,ROWNO,STAMP")] CSCODRCHG cSCODRCHG)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    int lastRowNo = 0;
                    try
                    {
                        lastRowNo = db.CSCODRCHGs.Where(m => m.CONO == cSCODRCHG.CONO && m.PRSCODE == cSCODRCHG.PRSCODE && m.ADATE == cSCODRCHG.ADATE).Max(n => n.ROWNO);
                    }
                    catch (Exception e) { lastRowNo = 0; }
                    finally { };

                    cSCODRCHG.STAMP = 0;
                    cSCODRCHG.ROWNO = lastRowNo + 1;
                    db.CSCODRCHGs.Add(cSCODRCHG);
                    db.SaveChanges();
                    return new RedirectResult(Url.Action("Edit", "CSCODRs", new { id = MyHtmlHelpers.ConvertIdToByteStr(cSCODRCHG.CONO), person = cSCODRCHG.PRSCODE, adate = cSCODRCHG.ADATE }));
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

            ViewBag.NATION = new SelectList(db.HKNATIONs, "NATION", "NATION", cSCODRCHG.NATION);
            ViewBag.RACE = new SelectList(db.HKRACEs, "RACE", "RACE", cSCODRCHG.RACE);
            ViewBag.CHGREM = new SelectList(db.CSCHGREMs.OrderBy(x => x.CHGREM), "CHGREM", "CHGREM", cSCODRCHG.CHGREM);
            ViewBag.ADDRID = new SelectList(db.CSPRSADDRs.Where(x => x.PRSCODE == cSCODRCHG.PRSCODE).OrderBy(y => y.ADDRID).Select(z => new { ADDRID = z.ADDRID, MAILDET = z.ADDRID + " | " + z.MAILADDR + " | " + z.ADDRTYPE + " | " + z.ADDR1 + z.ADDR2 + z.ADDR3 }), "ADDRID", "MAILDET", cSCODRCHG.ADDRID);
            ViewBag.REGID1 = new SelectList(db.CSPRSREGs.Where(x => x.PRSCODE == cSCODRCHG.PRSCODE).Select(x => new { REGNO = x.REGNO, PRSNAME = x.CTRYCODE + " | " + x.REGTYPE + " | " + x.REGNO }), "REGNO", "PRSNAME", cSCODRCHG.REGID1);
            ViewBag.REGID2 = new SelectList(db.CSPRSREGs.Where(x => x.PRSCODE == cSCODRCHG.PRSCODE).Select(x => new { REGNO = x.REGNO, PRSNAME = x.CTRYCODE + " | " + x.REGTYPE + " | " + x.REGNO }), "REGNO", "PRSNAME", cSCODRCHG.REGID2);

            return View(cSCODRCHG);
        }

        // GET: CSCODRCHGs/Edit/5
        public ActionResult Edit(string id, string person, DateTime adate, int row)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSCODRCHG cSCODRCHG = db.CSCODRCHGs.Find(MyHtmlHelpers.ConvertByteStrToId(id), person, adate, row);
            if (cSCODRCHG == null)
            {
                return HttpNotFound();
            }
            ViewBag.NATION = new SelectList(db.HKNATIONs, "NATION", "NATION", cSCODRCHG.NATION);
            ViewBag.RACE = new SelectList(db.HKRACEs, "RACE", "RACE", cSCODRCHG.RACE);
            ViewBag.CHGREM = new SelectList(db.CSCHGREMs.OrderBy(x => x.CHGREM), "CHGREM", "CHGREM", cSCODRCHG.CHGREM);
            ViewBag.ADDRID = new SelectList(db.CSPRSADDRs.Where(x => x.PRSCODE == cSCODRCHG.PRSCODE).OrderBy(y => y.ADDRID).Select(z => new { ADDRID = z.ADDRID, MAILDET = z.ADDRID + " | " + z.MAILADDR + " | " + z.ADDRTYPE + " | " + z.ADDR1 + z.ADDR2 + z.ADDR3 }), "ADDRID", "MAILDET", cSCODRCHG.ADDRID);
            ViewBag.REGID1 = new SelectList(db.CSPRSREGs.Where(x => x.PRSCODE == cSCODRCHG.PRSCODE).Select(x => new { REGNO = x.REGNO, PRSNAME = x.CTRYCODE + " | " + x.REGTYPE + " | " + x.REGNO }), "REGNO", "PRSNAME", cSCODRCHG.REGID1);
            ViewBag.REGID2 = new SelectList(db.CSPRSREGs.Where(x => x.PRSCODE == cSCODRCHG.PRSCODE).Select(x => new { REGNO = x.REGNO, PRSNAME = x.CTRYCODE + " | " + x.REGTYPE + " | " + x.REGNO }), "REGNO", "PRSNAME", cSCODRCHG.REGID2);

            return View(cSCODRCHG);
        }

        // POST: CSCODRCHGs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CONO,PRSCODE,ADATE,CHGEFFDATE,CHGREM,PRSNAME,NATION,RACE,ADDRID,OCCUPATION,ODRSHIP,REGCTRY1,REGTYPE1,REGID1,REGCTRY2,REGTYPE2,REGID2,REM,ROWNO,STAMP")] CSCODRCHG cSCODRCHG)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    cSCODRCHG.STAMP = cSCODRCHG.STAMP + 1;
                    db.Entry(cSCODRCHG).State = EntityState.Modified;
                    db.SaveChanges();
                    return new RedirectResult(Url.Action("Edit", "CSCODRs", new { id = MyHtmlHelpers.ConvertIdToByteStr(cSCODRCHG.CONO), person = cSCODRCHG.PRSCODE, adate = cSCODRCHG.ADATE }));
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
            ViewBag.NATION = new SelectList(db.HKNATIONs, "NATION", "NATION", cSCODRCHG.NATION);
            ViewBag.RACE = new SelectList(db.HKRACEs, "RACE", "RACE", cSCODRCHG.RACE);
            ViewBag.CHGREM = new SelectList(db.CSCHGREMs.OrderBy(x => x.CHGREM), "CHGREM", "CHGREM", cSCODRCHG.CHGREM);
            ViewBag.ADDRID = new SelectList(db.CSPRSADDRs.Where(x => x.PRSCODE == cSCODRCHG.PRSCODE).OrderBy(y => y.ADDRID).Select(z => new { ADDRID = z.ADDRID, MAILDET = z.ADDRID + " | " + z.MAILADDR + " | " + z.ADDRTYPE + " | " + z.ADDR1 + z.ADDR2 + z.ADDR3 }), "ADDRID", "MAILDET", cSCODRCHG.ADDRID);
            ViewBag.REGID1 = new SelectList(db.CSPRSREGs.Where(x => x.PRSCODE == cSCODRCHG.PRSCODE).Select(x => new { REGNO = x.REGNO, PRSNAME = x.CTRYCODE + " | " + x.REGTYPE + " | " + x.REGNO }), "REGNO", "PRSNAME", cSCODRCHG.REGID1);
            ViewBag.REGID2 = new SelectList(db.CSPRSREGs.Where(x => x.PRSCODE == cSCODRCHG.PRSCODE).Select(x => new { REGNO = x.REGNO, PRSNAME = x.CTRYCODE + " | " + x.REGTYPE + " | " + x.REGNO }), "REGNO", "PRSNAME", cSCODRCHG.REGID2);

            return View(cSCODRCHG);
        }

        // GET: CSCODRCHGs/Delete/5
        public ActionResult Delete(string id, string person, DateTime adate, int row)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSCODRCHG cSCODRCHG = db.CSCODRCHGs.Find(MyHtmlHelpers.ConvertByteStrToId(id), person, adate, row);
            if (cSCODRCHG == null)
            {
                return HttpNotFound();
            }
            return View(cSCODRCHG);
        }

        // POST: CSCODRCHGs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id, string person, DateTime adate, int row)
        {
            CSCODRCHG cSCODRCHG = db.CSCODRCHGs.Find(MyHtmlHelpers.ConvertByteStrToId(id), person, adate, row);
            try
            {
                db.CSCODRCHGs.Remove(cSCODRCHG);
                db.SaveChanges();
                return new RedirectResult(Url.Action("Edit", "CSCODRs", new { id = MyHtmlHelpers.ConvertIdToByteStr(cSCODRCHG.CONO), person = cSCODRCHG.PRSCODE, adate = cSCODRCHG.ADATE }));
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
            return View(cSCODRCHG);
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
