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
    public class CSCOSHCHGsController : Controller
    {
        private ASIDBConnection db = new ASIDBConnection();

        // GET: CSCOSHCHGs
        public ActionResult Index()
        {
            var cSCOSHCHGs = db.CSCOSHCHGs.Include(c => c.HKNATION).Include(c => c.HKRACE).Include(c => c.CSPRSADDR).Include(c => c.CSPRSREG).Include(c => c.CSPRSREG1).Include(c => c.CSCOSH);
            return View(cSCOSHCHGs.ToList());
        }

        // GET: CSCOSHCHGs/Details/5
        public ActionResult Details(string id, string person, int row)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSCOSHCHG cSCOSHCHG = db.CSCOSHCHGs.Find(MyHtmlHelpers.ConvertByteStrToId(id), person, row);
            if (cSCOSHCHG == null)
            {
                return HttpNotFound();
            }
            return View(cSCOSHCHG);
        }

        // GET: CSCOSHCHGs/Create
        public ActionResult Create(string id, string person)
        {
            CSCOSHCHG cSCOSHCHG = new CSCOSHCHG();

            cSCOSHCHG.CONO = MyHtmlHelpers.ConvertByteStrToId(id);
            cSCOSHCHG.PRSCODE = person;         
            cSCOSHCHG.CHGEFFDATE = DateTime.Today;
            cSCOSHCHG.PRSNAME = db.CSPRS.Where(x => x.PRSCODE == person).Select(x => x.PRSNAME).FirstOrDefault();

            ViewBag.NATION = new SelectList(db.HKNATIONs, "NATION", "NATION", cSCOSHCHG.NATION);
            ViewBag.RACE = new SelectList(db.HKRACEs, "RACE", "RACE", cSCOSHCHG.RACE);
            ViewBag.CHGREM = new SelectList(db.CSCHGREMs.OrderBy(x => x.CHGREM), "CHGREM", "CHGREM", cSCOSHCHG.CHGREM);
            ViewBag.ADDRID = new SelectList(db.CSPRSADDRs.Where(x => x.PRSCODE == cSCOSHCHG.PRSCODE).OrderBy(y => y.ADDRID).Select(z => new { ADDRID = z.ADDRID, MAILDET = z.ADDRID + " | " + z.MAILADDR + " | " + z.ADDRTYPE + " | " + z.ADDR1 + z.ADDR2 + z.ADDR3 }), "ADDRID", "MAILDET", cSCOSHCHG.ADDRID);
            ViewBag.REGID1 = new SelectList(db.CSPRSREGs.Where(x => x.PRSCODE == cSCOSHCHG.PRSCODE).Select(x => new { REGNO = x.REGNO, PRSNAME = x.CTRYCODE + " | " + x.REGTYPE + " | " + x.REGNO }), "REGNO", "PRSNAME", cSCOSHCHG.REGID1);
            ViewBag.REGID2 = new SelectList(db.CSPRSREGs.Where(x => x.PRSCODE == cSCOSHCHG.PRSCODE).Select(x => new { REGNO = x.REGNO, PRSNAME = x.CTRYCODE + " | " + x.REGTYPE + " | " + x.REGNO }), "REGNO", "PRSNAME", cSCOSHCHG.REGID2);
            return View(cSCOSHCHG);
        }

        // POST: CSCOSHCHGs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CONO,PRSCODE,CHGEFFDATE,CHGREM,PRSNAME,NATION,RACE,ADDRID,OCCUPATION,REGCTRY1,REGTYPE1,REGID1,REGCTRY2,REGTYPE2,REGID2,REM,ROWNO,STAMP")] CSCOSHCHG cSCOSHCHG)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    int lastRowNo = 0;
                    try
                    {
                        lastRowNo = db.CSCOSHCHGs.Where(m => m.CONO == cSCOSHCHG.CONO && m.PRSCODE == cSCOSHCHG.PRSCODE).Max(n => n.ROWNO);
                    }
                    catch (Exception e) { lastRowNo = 0; }
                    finally { };


                    cSCOSHCHG.ROWNO = lastRowNo + 1;

                    cSCOSHCHG.STAMP = 0;
                    db.CSCOSHCHGs.Add(cSCOSHCHG);
                    db.SaveChanges();
                    return new RedirectResult(Url.Action("Edit", "CSCOSHes", new { id = MyHtmlHelpers.ConvertIdToByteStr(cSCOSHCHG.CONO), person = cSCOSHCHG.PRSCODE }));
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

            ViewBag.NATION = new SelectList(db.HKNATIONs, "NATION", "NATION", cSCOSHCHG.NATION);
            ViewBag.RACE = new SelectList(db.HKRACEs, "RACE", "RACE", cSCOSHCHG.RACE);
            ViewBag.CHGREM = new SelectList(db.CSCHGREMs.OrderBy(x => x.CHGREM), "CHGREM", "CHGREM", cSCOSHCHG.CHGREM);
            ViewBag.ADDRID = new SelectList(db.CSPRSADDRs.Where(x => x.PRSCODE == cSCOSHCHG.PRSCODE).OrderBy(y => y.ADDRID).Select(z => new { ADDRID = z.ADDRID, MAILDET = z.ADDRID + " | " + z.MAILADDR + " | " + z.ADDRTYPE + " | " + z.ADDR1 + z.ADDR2 + z.ADDR3 }), "ADDRID", "MAILDET", cSCOSHCHG.ADDRID);
            ViewBag.REGID1 = new SelectList(db.CSPRSREGs.Where(x => x.PRSCODE == cSCOSHCHG.PRSCODE).Select(x => new { REGNO = x.REGNO, PRSNAME = x.CTRYCODE + " | " + x.REGTYPE + " | " + x.REGNO }), "REGNO", "PRSNAME", cSCOSHCHG.REGID1);
            ViewBag.REGID2 = new SelectList(db.CSPRSREGs.Where(x => x.PRSCODE == cSCOSHCHG.PRSCODE).Select(x => new { REGNO = x.REGNO, PRSNAME = x.CTRYCODE + " | " + x.REGTYPE + " | " + x.REGNO }), "REGNO", "PRSNAME", cSCOSHCHG.REGID2);
            return View(cSCOSHCHG);
        }

        // GET: CSCOSHCHGs/Edit/5
        public ActionResult Edit(string id, string person, int row)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSCOSHCHG cSCOSHCHG = db.CSCOSHCHGs.Find(MyHtmlHelpers.ConvertByteStrToId(id), person, row);
            if (cSCOSHCHG == null)
            {
                return HttpNotFound();
            }
            ViewBag.NATION = new SelectList(db.HKNATIONs, "NATION", "NATION", cSCOSHCHG.NATION);
            ViewBag.RACE = new SelectList(db.HKRACEs, "RACE", "RACE", cSCOSHCHG.RACE);
            ViewBag.CHGREM = new SelectList(db.CSCHGREMs.OrderBy(x => x.CHGREM), "CHGREM", "CHGREM", cSCOSHCHG.CHGREM);
            ViewBag.ADDRID = new SelectList(db.CSPRSADDRs.Where(x => x.PRSCODE == cSCOSHCHG.PRSCODE).OrderBy(y => y.ADDRID).Select(z => new { ADDRID = z.ADDRID, MAILDET = z.ADDRID + " | " + z.MAILADDR + " | " + z.ADDRTYPE + " | " + z.ADDR1 + z.ADDR2 + z.ADDR3 }), "ADDRID", "MAILDET", cSCOSHCHG.ADDRID);
            ViewBag.REGID1 = new SelectList(db.CSPRSREGs.Where(x => x.PRSCODE == cSCOSHCHG.PRSCODE).Select(x => new { REGNO = x.REGNO, PRSNAME = x.CTRYCODE + " | " + x.REGTYPE + " | " + x.REGNO }), "REGNO", "PRSNAME", cSCOSHCHG.REGID1);
            ViewBag.REGID2 = new SelectList(db.CSPRSREGs.Where(x => x.PRSCODE == cSCOSHCHG.PRSCODE).Select(x => new { REGNO = x.REGNO, PRSNAME = x.CTRYCODE + " | " + x.REGTYPE + " | " + x.REGNO }), "REGNO", "PRSNAME", cSCOSHCHG.REGID2);
            return View(cSCOSHCHG);
        }

        // POST: CSCOSHCHGs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CONO,PRSCODE,CHGEFFDATE,CHGREM,PRSNAME,NATION,RACE,ADDRID,OCCUPATION,REGCTRY1,REGTYPE1,REGID1,REGCTRY2,REGTYPE2,REGID2,REM,ROWNO,STAMP")] CSCOSHCHG cSCOSHCHG)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    cSCOSHCHG.STAMP = cSCOSHCHG.STAMP + 1;
                    db.Entry(cSCOSHCHG).State = EntityState.Modified;
                    db.SaveChanges();
                    return new RedirectResult(Url.Action("Edit", "CSCOSHes", new { id = MyHtmlHelpers.ConvertIdToByteStr(cSCOSHCHG.CONO), person = cSCOSHCHG.PRSCODE }));
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
            ViewBag.NATION = new SelectList(db.HKNATIONs, "NATION", "NATION", cSCOSHCHG.NATION);
            ViewBag.RACE = new SelectList(db.HKRACEs, "RACE", "RACE", cSCOSHCHG.RACE);
            ViewBag.CHGREM = new SelectList(db.CSCHGREMs.OrderBy(x => x.CHGREM), "CHGREM", "CHGREM", cSCOSHCHG.CHGREM);
            ViewBag.ADDRID = new SelectList(db.CSPRSADDRs.Where(x => x.PRSCODE == cSCOSHCHG.PRSCODE).OrderBy(y => y.ADDRID).Select(z => new { ADDRID = z.ADDRID, MAILDET = z.ADDRID + " | " + z.MAILADDR + " | " + z.ADDRTYPE + " | " + z.ADDR1 + z.ADDR2 + z.ADDR3 }), "ADDRID", "MAILDET", cSCOSHCHG.ADDRID);
            ViewBag.REGID1 = new SelectList(db.CSPRSREGs.Where(x => x.PRSCODE == cSCOSHCHG.PRSCODE).Select(x => new { REGNO = x.REGNO, PRSNAME = x.CTRYCODE + " | " + x.REGTYPE + " | " + x.REGNO }), "REGNO", "PRSNAME", cSCOSHCHG.REGID1);
            ViewBag.REGID2 = new SelectList(db.CSPRSREGs.Where(x => x.PRSCODE == cSCOSHCHG.PRSCODE).Select(x => new { REGNO = x.REGNO, PRSNAME = x.CTRYCODE + " | " + x.REGTYPE + " | " + x.REGNO }), "REGNO", "PRSNAME", cSCOSHCHG.REGID2);
            return View(cSCOSHCHG);
        }

        // GET: CSCOSHCHGs/Delete/5
        public ActionResult Delete(string id, string person, int row)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSCOSHCHG cSCOSHCHG = db.CSCOSHCHGs.Find(MyHtmlHelpers.ConvertByteStrToId(id), person, row);
            if (cSCOSHCHG == null)
            {
                return HttpNotFound();
            }
            return View(cSCOSHCHG);
        }

        // POST: CSCOSHCHGs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id, string person, int row)
        {
            CSCOSHCHG cSCOSHCHG = db.CSCOSHCHGs.Find(MyHtmlHelpers.ConvertByteStrToId(id), person,  row);
            try
            {
                db.CSCOSHCHGs.Remove(cSCOSHCHG);
                db.SaveChanges();
                return new RedirectResult(Url.Action("Edit", "CSCOSHes", new { id = MyHtmlHelpers.ConvertIdToByteStr(cSCOSHCHG.CONO), person = cSCOSHCHG.PRSCODE }));
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
            return View(cSCOSHCHG);
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
