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
    public class CSCOCMsController : Controller
    {
        private ASIDBConnection db = new ASIDBConnection();

        // GET: CSCOCMs
        public ActionResult Index()
        {
            var cSCOCMs = db.CSCOCMs.Include(c => c.CSPR).Include(c => c.CSPRSADDR);
            return View(cSCOCMs.ToList());
        }

        // GET: CSCOCMs/Details/5
        public ActionResult Details(string id, string refno)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSCOCM cSCOCM = db.CSCOCMs.Find(MyHtmlHelpers.ConvertByteStrToId(id), refno);
            if (cSCOCM == null)
            {
                return HttpNotFound();
            }
            ViewBag.PRSCODE = new SelectList(db.CSPRS.OrderBy(x => x.PRSNAME), "PRSCODE", "PRSNAME", cSCOCM.PRSCODE);
            ViewBag.ADDRID = new SelectList(db.CSPRSADDRs.Where(x => x.PRSCODE == cSCOCM.PRSCODE).OrderBy(y => y.ADDRID).Select(z => new { ADDRID = z.ADDRID, MAILDET = z.ADDRID + " | " + z.MAILADDR + " | " + z.ADDRTYPE + " | " + z.ADDR1 + z.ADDR2 + z.ADDR3 }), "ADDRID", "MAILDET", cSCOCM.ADDRID);

            return View(cSCOCM);
        }

        // GET: CSCOCMs/Create
        public ActionResult Create(string id)
        {
            CSCOCM cSCOCM = new CSCOCM();
            cSCOCM.CONO = MyHtmlHelpers.ConvertByteStrToId(id);
            cSCOCM.REFDATE = DateTime.Today;
            cSCOCM.CMSDATE = DateTime.Today;
           
            ViewBag.PRSCODE = new SelectList(db.CSPRS.OrderBy(x => x.PRSNAME), "PRSCODE", "PRSNAME");
            ViewBag.ADDRID = new SelectList(db.CSPRSADDRs.Where(x => x.PRSCODE == cSCOCM.PRSCODE).OrderBy(y => y.ADDRID).Select(z => new { ADDRID = z.ADDRID, MAILDET = z.ADDRID + " | " + z.ADDRTYPE + " | " + z.ADDR1 + z.ADDR2 + z.ADDR3 }), "ADDRID", "MAILDET", cSCOCM.ADDRID);
            cSCOCM.CSCOMSTR = db.CSCOMSTRs.Find(cSCOCM.CONO);
            return View(cSCOCM);
        }

        // POST: CSCOCMs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CONO,REFNO,REFDATE,CMSDATE,CMEDATE,PRSCODE,ADDRID,CMNATURE,LS,CMINFO,CMINFOStr,REM,STAMP")] CSCOCM cSCOCM)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    cSCOCM.STAMP = 0;
                    db.CSCOCMs.Add(cSCOCM);
                    db.SaveChanges();
                    return new RedirectResult(Url.Action("Edit", "CSCOMSTRs", new { id = MyHtmlHelpers.ConvertIdToByteStr(cSCOCM.CONO) }) + "#Charge");
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

            ViewBag.PRSCODE = new SelectList(db.CSPRS.OrderBy(x => x.PRSNAME), "PRSCODE", "PRSNAME", cSCOCM.PRSCODE);
            ViewBag.ADDRID = new SelectList(db.CSPRSADDRs.Where( x => x.PRSCODE == cSCOCM.PRSCODE).OrderBy( y => y.ADDRID).Select( z => new {ADDRID = z.ADDRID, MAILDET = z.ADDRID + " | " + z.MAILADDR + " | " + z.ADDRTYPE + " | " + z.ADDR1 + z.ADDR2 + z.ADDR3  } ), "ADDRID", "MAILDET", cSCOCM.ADDRID);
            cSCOCM.CSCOMSTR = db.CSCOMSTRs.Find(cSCOCM.CONO);
            return View(cSCOCM);
        }

        // GET: CSCOCMs/Edit/5
        public ActionResult Edit(string id, string refno)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSCOCM cSCOCM = db.CSCOCMs.Find(MyHtmlHelpers.ConvertByteStrToId(id), refno);
            if (cSCOCM == null)
            {
                return HttpNotFound();
            }
            Session["CSCOCM_ORIG"] = cSCOCM;
            ViewBag.PRSCODE = new SelectList(db.CSPRS.OrderBy(x => x.PRSNAME), "PRSCODE", "PRSNAME", cSCOCM.PRSCODE);
            ViewBag.ADDRID = new SelectList(db.CSPRSADDRs.Where(x => x.PRSCODE == cSCOCM.PRSCODE).OrderBy(y => y.ADDRID).Select(z => new { ADDRID = z.ADDRID, MAILDET = z.ADDRID + " | " + z.MAILADDR + " | " + z.ADDRTYPE + " | " + z.ADDR1 + z.ADDR2 + z.ADDR3 }), "ADDRID", "MAILDET", cSCOCM.ADDRID);
            return View(cSCOCM);
        }


        public ActionResult EditPRSCODE(CSCOCM cSCOCM)
        {


            ModelState.Remove("ADDRID");

  
         

            ViewBag.ADDRID = new SelectList(db.CSPRSADDRs.Where(x => x.PRSCODE == cSCOCM.PRSCODE).OrderBy(y => y.ADDRID).Select(z => new { ADDRID = z.ADDRID, MAILDET = z.ADDRID + " | " + z.MAILADDR + " | " + z.ADDRTYPE + " | " + z.ADDR1 + z.ADDR2 + z.ADDR3 }), "ADDRID", "MAILDET", cSCOCM.ADDRID);
            return PartialView("Partial/EditPRSCODE", cSCOCM);


        }

        // POST: CSCOCMs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CONO,REFNO,REFDATE,CMSDATE,CMEDATE,PRSCODE,ADDRID,CMNATURE,LS,CMINFO,CMINFOStr,REM,STAMP")] CSCOCM cSCOCM)
        {
            if (ModelState.IsValid)
            {try
                {
                    cSCOCM.STAMP = cSCOCM.STAMP + 1;
                    bool changed = false;
                    CSCOCM csOrig = (CSCOCM)Session["CSCOCM_ORIG"];
                    if (csOrig.REFNO != cSCOCM.REFNO) { changed = true; }

                    if (changed)
                    {
                        CSCOCM csDel = db.CSCOCMs.Find(csOrig.CONO, csOrig.REFNO);
                        db.CSCOCMs.Remove(csDel);
                        db.CSCOCMs.Add(cSCOCM);
                    }
                    else
                    {
                        db.Entry(cSCOCM).State = EntityState.Modified;
                    }
                    db.SaveChanges();
                    return new RedirectResult(Url.Action("Edit", "CSCOMSTRs", new { id = MyHtmlHelpers.ConvertIdToByteStr(cSCOCM.CONO) }) + "#Charge");
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
            ViewBag.PRSCODE = new SelectList(db.CSPRS, "PRSCODE", "PRSNAME", cSCOCM.PRSCODE);
            ViewBag.ADDRID = new SelectList(db.CSPRSADDRs.Where(x => x.PRSCODE == cSCOCM.PRSCODE).OrderBy(y => y.ADDRID).Select(z => new { ADDRID = z.ADDRID, MAILDET = z.ADDRID + " | " + z.ADDRTYPE + " | " + z.ADDR1 + z.ADDR2 + z.ADDR3 }), "ADDRID", "MAILDET", cSCOCM.PRSCODE);
            cSCOCM.CSCOMSTR = db.CSCOMSTRs.Find(cSCOCM.CONO);
            return View(cSCOCM);
        }

        // GET: CSCOCMs/Delete/5
        public ActionResult Delete(string id, string refno)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSCOCM cSCOCM = db.CSCOCMs.Find(MyHtmlHelpers.ConvertByteStrToId(id), refno);
            if (cSCOCM == null)
            {
                return HttpNotFound();
            }
            return View(cSCOCM);
        }

        // POST: CSCOCMs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id, string refno)
        {
            CSCOCM cSCOCM = db.CSCOCMs.Find(MyHtmlHelpers.ConvertByteStrToId(id), refno);
            try
            {
                db.CSCOCMs.Remove(cSCOCM);
                db.SaveChanges();
                return new RedirectResult(Url.Action("Edit", "CSCOMSTRs", new { id = MyHtmlHelpers.ConvertIdToByteStr(cSCOCM.CONO) }) + "#Charge");
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
            return View(cSCOCM);
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
