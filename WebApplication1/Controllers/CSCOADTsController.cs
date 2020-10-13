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
    public class CSCOADTsController : Controller
    {
        private ASIDBConnection db = new ASIDBConnection();

        // GET: CSCOADTs
        public ActionResult Index()
        {
            var cSCOADTs = db.CSCOADTs.Include(c => c.CSADT).Include(c => c.CSPR);
            return View(cSCOADTs.ToList());
        }

        // GET: CSCOADTs/Details/5
        public ActionResult Details(string id, int row)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSCOADT cSCOADT = db.CSCOADTs.Find(MyHtmlHelpers.ConvertByteStrToId(id), row);
            if (cSCOADT == null)
            {
                return HttpNotFound();
            }


            ViewBag.ADTCODE = new SelectList(db.CSADTs.OrderBy(x => x.ADTDESC), "ADTCODE", "ADTDESC", cSCOADT.ADTCODE);
            ViewBag.PRSCODE = new SelectList(db.CSPRS.Where(x => x.HKCONST.CONSTTYPE == "Individual").OrderBy(x => x.PRSNAME), "PRSCODE", "PRSNAME", cSCOADT.PRSCODE);
            return View(cSCOADT);
        }

        // GET: CSCOADTs/Create
        public ActionResult Create(string id)
        {

            ViewBag.Parent = MyHtmlHelpers.ConvertByteStrToId(id);
            string sid = ViewBag.Parent;

            CSCOADT curRec = new CSCOADT();
            curRec.CONO = sid;
            curRec.ADATE = DateTime.Today;
            curRec.ENDDATE = new DateTime(3000, 1, 1);
            curRec.CSCOMSTR = db.CSCOMSTRs.Find(curRec.CONO);
            ViewBag.ADTCODE = new SelectList(db.CSADTs.OrderBy(x => x.ADTDESC), "ADTCODE", "ADTDESC");
            ViewBag.PRSCODE = new SelectList(db.CSPRS.Where(x => x.HKCONST.CONSTTYPE == "Individual").OrderBy(x => x.PRSNAME), "PRSCODE", "PRSNAME", curRec.PRSCODE);
            return View(curRec);
  
        }

        // POST: CSCOADTs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CONO,ADTCODE,PRSCODE,ADATE,RDATE,ENDDATE,REM,ROWNO,STAMP")] CSCOADT cSCOADT)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    int lastRowNo = 0;
                    try
                    {
                        lastRowNo = db.CSCOADTs.Where(m => m.CONO == cSCOADT.CONO).Max(n => n.ROWNO);
                    }
                    catch (Exception e) { lastRowNo = 0; }
                    finally { };

                    cSCOADT.STAMP = 0;
                    cSCOADT.ROWNO = lastRowNo + 1;

                    db.CSCOADTs.Add(cSCOADT);
                    db.SaveChanges();
                    return new RedirectResult(Url.Action("Edit", "CSCOMSTRs", new { id = MyHtmlHelpers.ConvertIdToByteStr(cSCOADT.CONO) }) + "#Auditor");
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

            cSCOADT.CSCOMSTR = db.CSCOMSTRs.Find(cSCOADT.CONO);
            ViewBag.ADTCODE = new SelectList(db.CSADTs.OrderBy(x => x.ADTDESC), "ADTCODE", "ADTDESC", cSCOADT.ADTCODE);
            ViewBag.PRSCODE = new SelectList(db.CSPRS.Where(x => x.HKCONST.CONSTTYPE == "Individual").OrderBy(x => x.PRSNAME), "PRSCODE", "PRSNAME", cSCOADT.PRSCODE);
            return View(cSCOADT);
        }

        // GET: CSCOADTs/Edit/5
        public ActionResult Edit(string id, int row)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSCOADT cSCOADT = db.CSCOADTs.Find(MyHtmlHelpers.ConvertByteStrToId(id), row);
            if (cSCOADT == null)
            {
                return HttpNotFound();
            }

            ViewBag.ADTCODE = new SelectList(db.CSADTs.OrderBy(x => x.ADTDESC), "ADTCODE", "ADTDESC", cSCOADT.ADTCODE);
            ViewBag.PRSCODE = new SelectList(db.CSPRS.Where(x => x.HKCONST.CONSTTYPE == "Individual").OrderBy(x => x.PRSNAME), "PRSCODE", "PRSNAME", cSCOADT.PRSCODE);

            return View(cSCOADT);
        }

        // POST: CSCOADTs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CONO,ADTCODE,PRSCODE,ADATE,RDATE,ENDDATE,REM,ROWNO,STAMP")] CSCOADT cSCOADT)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    cSCOADT.STAMP = cSCOADT.STAMP + 1;
                    db.Entry(cSCOADT).State = EntityState.Modified;
                    db.SaveChanges();
                    return new RedirectResult(Url.Action("Edit", "CSCOMSTRs", new { id = MyHtmlHelpers.ConvertIdToByteStr(cSCOADT.CONO) }) + "#Auditor");
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
            cSCOADT.CSCOMSTR = db.CSCOMSTRs.Find(cSCOADT.CONO);
            ViewBag.ADTCODE = new SelectList(db.CSADTs.OrderBy(x => x.ADTDESC), "ADTCODE", "ADTDESC", cSCOADT.ADTCODE);
            ViewBag.PRSCODE = new SelectList(db.CSPRS.Where(x => x.HKCONST.CONSTTYPE == "Individual").OrderBy(x => x.PRSNAME), "PRSCODE", "PRSNAME", cSCOADT.PRSCODE);

            return View(cSCOADT);
        }

        // GET: CSCOADTs/Delete/5
        public ActionResult Delete(string id, int row)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSCOADT cSCOADT = db.CSCOADTs.Find(MyHtmlHelpers.ConvertByteStrToId(id), row);
            if (cSCOADT == null)
            {
                return HttpNotFound();
            }
            return View(cSCOADT);
        }

        // POST: CSCOADTs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id, int row)
        {
            CSCOADT cSCOADT = db.CSCOADTs.Find(MyHtmlHelpers.ConvertByteStrToId(id), row);
            try
            {
                db.CSCOADTs.Remove(cSCOADT);
                db.SaveChanges();
                return new RedirectResult(Url.Action("Edit", "CSCOMSTRs", new { id = MyHtmlHelpers.ConvertIdToByteStr(cSCOADT.CONO) }) + "#Auditor");
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
            cSCOADT.CSCOMSTR = db.CSCOMSTRs.Find(cSCOADT.CONO);
            return View(cSCOADT);
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
