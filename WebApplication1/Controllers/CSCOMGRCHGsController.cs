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
    public class CSCOMGRCHGsController : Controller
    {
        private ASIDBConnection db = new ASIDBConnection();

        // GET: CSCOMGRCHGs
        public ActionResult Index()
        {
            var cSCOMGRCHGs = db.CSCOMGRCHGs.Include(c => c.HKNATION).Include(c => c.HKRACE).Include(c => c.CSPRSADDR).Include(c => c.CSPRSREG).Include(c => c.CSPRSREG1);
            return View(cSCOMGRCHGs.ToList());
        }

        // GET: CSCOMGRCHGs/Details/5
        public ActionResult Details(string id, string person, DateTime adate, int row)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSCOMGRCHG cSCOMGRCHG = db.CSCOMGRCHGs.Find(MyHtmlHelpers.ConvertByteStrToId(id), person, adate, row);
            if (cSCOMGRCHG == null)
            {
                return HttpNotFound();
            }
            return View(cSCOMGRCHG);
        }

        // GET: CSCOMGRCHGs/Create
        public ActionResult Create(string id, string person, DateTime adate)
        {
            CSCOMGRCHG cSCOMGRCHG = new CSCOMGRCHG();

            cSCOMGRCHG.CONO = MyHtmlHelpers.ConvertByteStrToId(id);
            cSCOMGRCHG.PRSCODE = person;
            cSCOMGRCHG.ADATE = adate;
            cSCOMGRCHG.CHGEFFDATE = DateTime.Today;
            cSCOMGRCHG.PRSNAME = db.CSPRS.Where(x => x.PRSCODE == person).Select(x => x.PRSNAME).FirstOrDefault();

            ViewBag.NATION = new SelectList(db.HKNATIONs, "NATION", "NATION", cSCOMGRCHG.NATION);
            ViewBag.RACE = new SelectList(db.HKRACEs, "RACE", "RACE", cSCOMGRCHG.RACE);
            ViewBag.CHGREM = new SelectList(db.CSCHGREMs.OrderBy(x => x.CHGREM), "CHGREM", "CHGREM", cSCOMGRCHG.CHGREM);
            ViewBag.ADDRID = new SelectList(db.CSPRSADDRs.Where(x => x.PRSCODE == cSCOMGRCHG.PRSCODE).OrderBy(y => y.ADDRID).Select(z => new { ADDRID = z.ADDRID, MAILDET = z.ADDRID + " | " + z.MAILADDR + " | " + z.ADDRTYPE + " | " + z.ADDR1 + z.ADDR2 + z.ADDR3 }), "ADDRID", "MAILDET", cSCOMGRCHG.ADDRID);
            ViewBag.REGID1 = new SelectList(db.CSPRSREGs.Where(x => x.PRSCODE == cSCOMGRCHG.PRSCODE).Select(x => new { REGNO = x.REGNO, PRSNAME = x.CTRYCODE + " | " + x.REGTYPE + " | " + x.REGNO }), "REGNO", "PRSNAME", cSCOMGRCHG.REGID1);
            ViewBag.REGID2 = new SelectList(db.CSPRSREGs.Where(x => x.PRSCODE == cSCOMGRCHG.PRSCODE).Select(x => new { REGNO = x.REGNO, PRSNAME = x.CTRYCODE + " | " + x.REGTYPE + " | " + x.REGNO }), "REGNO", "PRSNAME", cSCOMGRCHG.REGID2);
            return View(cSCOMGRCHG);
        }

        // POST: CSCOMGRCHGs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CONO,PRSCODE,ADATE,CHGEFFDATE,CHGREM,PRSNAME,NATION,RACE,ADDRID,OCCUPATION,REGCTRY1,REGTYPE1,REGID1,REGCTRY2,REGTYPE2,REGID2,REM,ROWNO,STAMP")] CSCOMGRCHG cSCOMGRCHG)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    int lastRowNo = 0;
                    try
                    {
                        lastRowNo = db.CSCOMGRCHGs.Where(m => m.CONO == cSCOMGRCHG.CONO && m.PRSCODE == cSCOMGRCHG.PRSCODE && m.ADATE == cSCOMGRCHG.ADATE).Max(n => n.ROWNO);
                    }
                    catch (Exception e) { lastRowNo = 0; }
                    finally { };


                    cSCOMGRCHG.ROWNO = lastRowNo + 1;

                    cSCOMGRCHG.STAMP = 0;

                    db.CSCOMGRCHGs.Add(cSCOMGRCHG);
                    db.SaveChanges();
                    return new RedirectResult(Url.Action("Edit", "CSCOMGRs", new { id = MyHtmlHelpers.ConvertIdToByteStr(cSCOMGRCHG.CONO), person = cSCOMGRCHG.PRSCODE, adate = cSCOMGRCHG.ADATE }));
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

            ViewBag.NATION = new SelectList(db.HKNATIONs, "NATION", "NATION", cSCOMGRCHG.NATION);
            ViewBag.RACE = new SelectList(db.HKRACEs, "RACE", "RACE", cSCOMGRCHG.RACE);
            ViewBag.CHGREM = new SelectList(db.CSCHGREMs.OrderBy(x => x.CHGREM), "CHGREM", "CHGREM", cSCOMGRCHG.CHGREM);
            ViewBag.ADDRID = new SelectList(db.CSPRSADDRs.Where(x => x.PRSCODE == cSCOMGRCHG.PRSCODE).OrderBy(y => y.ADDRID).Select(z => new { ADDRID = z.ADDRID, MAILDET = z.ADDRID + " | " + z.MAILADDR + " | " + z.ADDRTYPE + " | " + z.ADDR1 + z.ADDR2 + z.ADDR3 }), "ADDRID", "MAILDET", cSCOMGRCHG.ADDRID);
            ViewBag.REGID1 = new SelectList(db.CSPRSREGs.Where(x => x.PRSCODE == cSCOMGRCHG.PRSCODE).Select(x => new { REGNO = x.REGNO, PRSNAME = x.CTRYCODE + " | " + x.REGTYPE + " | " + x.REGNO }), "REGNO", "PRSNAME", cSCOMGRCHG.REGID1);
            ViewBag.REGID2 = new SelectList(db.CSPRSREGs.Where(x => x.PRSCODE == cSCOMGRCHG.PRSCODE).Select(x => new { REGNO = x.REGNO, PRSNAME = x.CTRYCODE + " | " + x.REGTYPE + " | " + x.REGNO }), "REGNO", "PRSNAME", cSCOMGRCHG.REGID2);
            return View(cSCOMGRCHG);
        }

        // GET: CSCOMGRCHGs/Edit/5
        public ActionResult Edit(string id, string person, DateTime adate, int row)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSCOMGRCHG cSCOMGRCHG = db.CSCOMGRCHGs.Find(MyHtmlHelpers.ConvertByteStrToId(id), person, adate, row);
            if (cSCOMGRCHG == null)
            {
                return HttpNotFound();
            }
            ViewBag.NATION = new SelectList(db.HKNATIONs, "NATION", "NATION", cSCOMGRCHG.NATION);
            ViewBag.RACE = new SelectList(db.HKRACEs, "RACE", "RACE", cSCOMGRCHG.RACE);
            ViewBag.CHGREM = new SelectList(db.CSCHGREMs.OrderBy(x => x.CHGREM), "CHGREM", "CHGREM", cSCOMGRCHG.CHGREM);
            ViewBag.ADDRID = new SelectList(db.CSPRSADDRs.Where(x => x.PRSCODE == cSCOMGRCHG.PRSCODE).OrderBy(y => y.ADDRID).Select(z => new { ADDRID = z.ADDRID, MAILDET = z.ADDRID + " | " + z.MAILADDR + " | " + z.ADDRTYPE + " | " + z.ADDR1 + z.ADDR2 + z.ADDR3 }), "ADDRID", "MAILDET", cSCOMGRCHG.ADDRID);
            ViewBag.REGID1 = new SelectList(db.CSPRSREGs.Where(x => x.PRSCODE == cSCOMGRCHG.PRSCODE).Select(x => new { REGNO = x.REGNO, PRSNAME = x.CTRYCODE + " | " + x.REGTYPE + " | " + x.REGNO }), "REGNO", "PRSNAME", cSCOMGRCHG.REGID1);
            ViewBag.REGID2 = new SelectList(db.CSPRSREGs.Where(x => x.PRSCODE == cSCOMGRCHG.PRSCODE).Select(x => new { REGNO = x.REGNO, PRSNAME = x.CTRYCODE + " | " + x.REGTYPE + " | " + x.REGNO }), "REGNO", "PRSNAME", cSCOMGRCHG.REGID2);
            return View(cSCOMGRCHG);
        }

        // POST: CSCOMGRCHGs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CONO,PRSCODE,ADATE,CHGEFFDATE,CHGREM,PRSNAME,NATION,RACE,ADDRID,OCCUPATION,REGCTRY1,REGTYPE1,REGID1,REGCTRY2,REGTYPE2,REGID2,REM,ROWNO,STAMP")] CSCOMGRCHG cSCOMGRCHG)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    cSCOMGRCHG.STAMP = cSCOMGRCHG.STAMP + 1;
                    db.Entry(cSCOMGRCHG).State = EntityState.Modified;
                    db.SaveChanges();
                    return new RedirectResult(Url.Action("Edit", "CSCOMGRs", new { id = MyHtmlHelpers.ConvertIdToByteStr(cSCOMGRCHG.CONO), person = cSCOMGRCHG.PRSCODE, adate = cSCOMGRCHG.ADATE }));
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
            ViewBag.NATION = new SelectList(db.HKNATIONs, "NATION", "NATION", cSCOMGRCHG.NATION);
            ViewBag.RACE = new SelectList(db.HKRACEs, "RACE", "RACE", cSCOMGRCHG.RACE);
            ViewBag.CHGREM = new SelectList(db.CSCHGREMs.OrderBy(x => x.CHGREM), "CHGREM", "CHGREM", cSCOMGRCHG.CHGREM);
            ViewBag.ADDRID = new SelectList(db.CSPRSADDRs.Where(x => x.PRSCODE == cSCOMGRCHG.PRSCODE).OrderBy(y => y.ADDRID).Select(z => new { ADDRID = z.ADDRID, MAILDET = z.ADDRID + " | " + z.MAILADDR + " | " + z.ADDRTYPE + " | " + z.ADDR1 + z.ADDR2 + z.ADDR3 }), "ADDRID", "MAILDET", cSCOMGRCHG.ADDRID);
            ViewBag.REGID1 = new SelectList(db.CSPRSREGs.Where(x => x.PRSCODE == cSCOMGRCHG.PRSCODE).Select(x => new { REGNO = x.REGNO, PRSNAME = x.CTRYCODE + " | " + x.REGTYPE + " | " + x.REGNO }), "REGNO", "PRSNAME", cSCOMGRCHG.REGID1);
            ViewBag.REGID2 = new SelectList(db.CSPRSREGs.Where(x => x.PRSCODE == cSCOMGRCHG.PRSCODE).Select(x => new { REGNO = x.REGNO, PRSNAME = x.CTRYCODE + " | " + x.REGTYPE + " | " + x.REGNO }), "REGNO", "PRSNAME", cSCOMGRCHG.REGID2);
            return View(cSCOMGRCHG);
        }

        // GET: CSCOMGRCHGs/Delete/5
        public ActionResult Delete(string id, string person, DateTime adate, int row)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSCOMGRCHG cSCOMGRCHG = db.CSCOMGRCHGs.Find(MyHtmlHelpers.ConvertByteStrToId(id), person, adate, row);
            if (cSCOMGRCHG == null)
            {
                return HttpNotFound();
            }
            return View(cSCOMGRCHG);
        }

        // POST: CSCOMGRCHGs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id, string person, DateTime adate, int row)
        {

            CSCOMGRCHG cSCOMGRCHG = db.CSCOMGRCHGs.Find(MyHtmlHelpers.ConvertByteStrToId(id), person, adate, row);
            try
            {
                db.CSCOMGRCHGs.Remove(cSCOMGRCHG);
                db.SaveChanges();
                return new RedirectResult(Url.Action("Edit", "CSCOMGRs", new { id = MyHtmlHelpers.ConvertIdToByteStr(cSCOMGRCHG.CONO), person = cSCOMGRCHG.PRSCODE, adate = cSCOMGRCHG.ADATE }));
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
            return View(cSCOMGRCHG);
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
