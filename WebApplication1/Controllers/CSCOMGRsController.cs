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
    public class CSCOMGRsController : Controller
    {
        private ASIDBConnection db = new ASIDBConnection();

        // GET: CSCOMGRs
        public ActionResult Index()
        {
            var cSCOMGRs = db.CSCOMGRs.Include(c => c.CSPR);
            return View(cSCOMGRs.ToList());
        }

        public PartialViewResult EditRegChg(string id, string person, DateTime adate)
        {

            string sid = MyHtmlHelpers.ConvertByteStrToId(id);
            ViewBag.Parent = sid;
            ViewBag.CONO = sid;
            ViewBag.PRSCODE = person;
            ViewBag.ADATE = adate;

            var cSCOMGRCHG = db.CSCOMGRCHGs.Where(x => x.CONO == sid && x.PRSCODE == person && x.ADATE == adate).ToList();
            return PartialView("Partial/PartialChg", cSCOMGRCHG);
        }

        // GET: CSCOMGRs/Details/5
        public ActionResult Details(string id, string person, DateTime adate)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSCOMGR cSCOMGR = db.CSCOMGRs.Find(MyHtmlHelpers.ConvertByteStrToId(id), person, adate);
            if (cSCOMGR == null)
            {
                return HttpNotFound();
            }
            ViewBag.PRSCODE = new SelectList(db.CSPRS.Where(x => x.HKCONST.CONSTTYPE == "Individual").OrderBy(x => x.PRSNAME), "PRSCODE", "PRSNAME", cSCOMGR.PRSCODE);
            return View(cSCOMGR);
        }

        // GET: CSCOMGRs/Create
        public ActionResult Create(string id)
        {
            ViewBag.Parent = MyHtmlHelpers.ConvertByteStrToId(id);
            string sid = ViewBag.Parent;

            CSCOMGR curRec = new CSCOMGR();
            curRec.CONO = sid;
            curRec.ADATE = DateTime.Today;
            curRec.ENDDATE = new DateTime(3000, 1, 1);
            curRec.CSCOMSTR = db.CSCOMSTRs.Find(curRec.CONO);
            ViewBag.PRSCODE = new SelectList(db.CSPRS.Where(x => x.HKCONST.CONSTTYPE == "Individual").OrderBy(x => x.PRSNAME), "PRSCODE", "PRSNAME", curRec.PRSCODE);
            return View(curRec);

        }

        // POST: CSCOMGRs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CONO,PRSCODE,ADATE,RDATE,ENDDATE,REM,ROWNO,STAMP")] CSCOMGR cSCOMGR)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    int lastRowNo = 0;
                    try
                    {
                        lastRowNo = db.CSCOMGRs.Where(m => m.CONO == cSCOMGR.CONO).Max(n => n.ROWNO);
                    }
                    catch (Exception e) { lastRowNo = 0; }
                    finally { };

                    cSCOMGR.STAMP = 0;
                    cSCOMGR.ROWNO = lastRowNo + 1;
                    db.CSCOMGRs.Add(cSCOMGR);
                    db.SaveChanges();
                    return new RedirectResult(Url.Action("Edit", "CSCOMSTRs", new { id = MyHtmlHelpers.ConvertIdToByteStr(cSCOMGR.CONO) }) + "#Manager");
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

            cSCOMGR.CSCOMSTR = db.CSCOMSTRs.Find(cSCOMGR.CONO);        
            ViewBag.PRSCODE = new SelectList(db.CSPRS.Where(x => x.HKCONST.CONSTTYPE == "Individual").OrderBy(x => x.PRSNAME), "PRSCODE", "PRSNAME", cSCOMGR.PRSCODE);

            return View(cSCOMGR);
        }

        // GET: CSCOMGRs/Edit/5
        public ActionResult Edit(string id, string person, DateTime adate)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSCOMGR cSCOMGR = db.CSCOMGRs.Find(MyHtmlHelpers.ConvertByteStrToId(id), person, adate);
            if (cSCOMGR == null)
            {
                return HttpNotFound();
            }

            Session["CSCOMGR_ORIG"] = cSCOMGR;
            ViewBag.PRSCODE = new SelectList(db.CSPRS.Where(x => x.HKCONST.CONSTTYPE == "Individual").OrderBy(x => x.PRSNAME), "PRSCODE", "PRSNAME", cSCOMGR.PRSCODE);
            return View(cSCOMGR);
        }

        // POST: CSCOMGRs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CONO,PRSCODE,ADATE,RDATE,ENDDATE,REM,ROWNO,STAMP")] CSCOMGR cSCOMGR)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    bool changed = false;
                    CSCOMGR csOrig = (CSCOMGR)Session["CSCOMGR_ORIG"];
                    if (csOrig.PRSCODE != cSCOMGR.PRSCODE) { changed = true; }
                    if (csOrig.ADATE != cSCOMGR.ADATE) { changed = true; }


                    if (changed)
                    {
                        CSCOMGR csDel = db.CSCOMGRs.Find(csOrig.CONO, csOrig.PRSCODE, csOrig.ADATE);
                        db.CSCOMGRs.Remove(csDel);
                        db.CSCOMGRs.Add(cSCOMGR);
                    }
                    else
                    {
                        db.Entry(cSCOMGR).State = EntityState.Modified;
                    }
                    db.SaveChanges();
                    return new RedirectResult(Url.Action("Edit", "CSCOMSTRs", new { id = MyHtmlHelpers.ConvertIdToByteStr(cSCOMGR.CONO) }) + "#Manager");
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
            ViewBag.PRSCODE = new SelectList(db.CSPRS.Where(x => x.HKCONST.CONSTTYPE == "Individual").OrderBy(x => x.PRSNAME), "PRSCODE", "PRSNAME", cSCOMGR.PRSCODE);
            cSCOMGR.CSCOMGRCHGs = db.CSCOMGRCHGs.Where(x => x.CONO == cSCOMGR.CONO && x.PRSCODE == cSCOMGR.PRSCODE && x.ADATE == cSCOMGR.ADATE).ToList();
            cSCOMGR.CSCOMSTR = db.CSCOMSTRs.Find(cSCOMGR.CONO);
            return View(cSCOMGR);
        }

        // GET: CSCOMGRs/Delete/5
        public ActionResult Delete(string id, string person, DateTime adate)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSCOMGR cSCOMGR = db.CSCOMGRs.Find(MyHtmlHelpers.ConvertByteStrToId(id), person, adate);
            if (cSCOMGR == null)
            {
                return HttpNotFound();
            }
            return View(cSCOMGR);
        }

        // POST: CSCOMGRs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id, string person, DateTime adate)
        {
            CSCOMGR cSCOMGR = db.CSCOMGRs.Find(MyHtmlHelpers.ConvertByteStrToId(id), person, adate);
            try
            {
                db.CSCOMGRs.Remove(cSCOMGR);
                db.SaveChanges();
                return new RedirectResult(Url.Action("Edit", "CSCOMSTRs", new { id = MyHtmlHelpers.ConvertIdToByteStr(cSCOMGR.CONO) }) + "#Manager");
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

            cSCOMGR.CSCOMSTR = db.CSCOMSTRs.Find(cSCOMGR.CONO);
            return View(cSCOMGR);
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
