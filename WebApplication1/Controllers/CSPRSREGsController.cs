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
    public class CSPRSREGsController : Controller
    {
        private ASIDBConnection db = new ASIDBConnection();

        // GET: CSPRSREGs
        public ActionResult Index()
        {
            var cSPRSREGs = db.CSPRSREGs.Include(c => c.HKCTRY);
            return View(cSPRSREGs.ToList());
        }

        // GET: CSPRSREGs/Details/5
        public ActionResult Details(string id, string ctry, string regtype, string regno)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSPRSREG cSPRSREG = db.CSPRSREGs.Find(MyHtmlHelpers.ConvertByteStrToId(id), ctry, regtype, regno);
            if (cSPRSREG == null)
            {
                return HttpNotFound();
            }
            return View(cSPRSREG);
        }

        // GET: CSPRSREGs/Create
        public ActionResult Create(string id)
        {
            CSPRSREG cSPRSREG = new CSPRSREG();
            cSPRSREG.PRSCODE = MyHtmlHelpers.ConvertByteStrToId(id);
            ViewBag.PRSCODE = cSPRSREG.PRSCODE;
            ViewBag.CTRYCODE = new SelectList(db.HKCTRies.OrderBy(x => x.CTRYCODE), "CTRYCODE", "CTRYDESC");
            ViewBag.REGTYPE = new SelectList(db.HKREGTYPEs.Select( x => new {REGTYPE = x.REGTYPE, REGDESC = x.CTRYOPR + " | " + x.REGTYPE }), "REGTYPE", "REGDESC");
            return View(cSPRSREG);
        }

        // POST: CSPRSREGs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PRSCODE,CTRYCODE,REGTYPE,REGNO,REM,STAMP")] CSPRSREG cSPRSREG)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    cSPRSREG.STAMP = 0;
                    db.CSPRSREGs.Add(cSPRSREG);
                    db.SaveChanges();
                    return RedirectToAction("Edit","CSPRs",new { id = MyHtmlHelpers.ConvertIdToByteStr(cSPRSREG.PRSCODE) } );
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
            ViewBag.PRSCODE = MyHtmlHelpers.ConvertByteStrToId(cSPRSREG.PRSCODE);
            ViewBag.REGTYPE = new SelectList(db.HKREGTYPEs.Select(x => new { REGTYPE = x.REGTYPE, REGDESC = x.CTRYOPR + " | " + x.REGTYPE }), "REGTYPE", "REGDESC");
            ViewBag.CTRYCODE = new SelectList(db.HKCTRies.OrderBy(x => x.CTRYCODE), "CTRYCODE", "CTRYDESC", cSPRSREG.CTRYCODE);
            return View(cSPRSREG);
        }

        // GET: CSPRSREGs/Edit/5
        public ActionResult Edit(string id, string ctry, string regtype, string regno)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSPRSREG cSPRSREG = db.CSPRSREGs.Find(MyHtmlHelpers.ConvertByteStrToId(id), ctry, regtype,regno);
            if (cSPRSREG == null)
            {
                return HttpNotFound();
            }
            Session["CSPRSREG_ORIG"] = cSPRSREG;

            ViewBag.PRSCODE = MyHtmlHelpers.ConvertByteStrToId(id);
            ViewBag.REGTYPE = new SelectList(db.HKREGTYPEs.Select(x => new { REGTYPE = x.REGTYPE, REGDESC = x.CTRYOPR + " | " + x.REGTYPE }), "REGTYPE", "REGDESC");
            ViewBag.CTRYCODE = new SelectList(db.HKCTRies.OrderBy(x => x.CTRYCODE), "CTRYCODE", "CTRYDESC", cSPRSREG.CTRYCODE);
            return View(cSPRSREG);
        }

        // POST: CSPRSREGs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PRSCODE,CTRYCODE,REGTYPE,REGNO,REM,STAMP")] CSPRSREG cSPRSREG)
        {
            if (ModelState.IsValid)
            {
                cSPRSREG.STAMP = cSPRSREG.STAMP + 1;
                try
                {
                    bool changed = false;
                    CSPRSREG csOrig = (CSPRSREG) Session["CSPRSREG_ORIG"];
                    if (csOrig.CTRYCODE != cSPRSREG.CTRYCODE) { changed = true; }
                    if (csOrig.REGTYPE != cSPRSREG.REGTYPE) { changed = true; }
                    if (csOrig.REGNO != cSPRSREG.REGNO) { changed = true; }

                    if (changed)
                    {
                        CSPRSREG csDel = db.CSPRSREGs.Find(csOrig.PRSCODE, csOrig.CTRYCODE, csOrig.REGTYPE, csOrig.REGNO); ;
                        db.CSPRSREGs.Remove(csDel);
                        db.CSPRSREGs.Add(cSPRSREG);
                    } else
                    {
                        db.Entry(cSPRSREG).State = EntityState.Modified;
                    }

             
                    db.SaveChanges();
                    return RedirectToAction("Edit", "CSPRs", new { id = MyHtmlHelpers.ConvertIdToByteStr(cSPRSREG.PRSCODE) });
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
            ViewBag.PRSCODE = MyHtmlHelpers.ConvertByteStrToId(cSPRSREG.PRSCODE);
            ViewBag.REGTYPE = new SelectList(db.HKREGTYPEs.Select(x => new { REGTYPE = x.REGTYPE, REGDESC = x.CTRYOPR + " | " + x.REGTYPE }), "REGTYPE", "REGDESC");
            ViewBag.CTRYCODE = new SelectList(db.HKCTRies.OrderBy(x => x.CTRYCODE), "CTRYCODE", "CTRYDESC", cSPRSREG.CTRYCODE);
            return View(cSPRSREG);
        }

        // GET: CSPRSREGs/Delete/5
        public ActionResult Delete(string id, string ctry, string regtype, string regno)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSPRSREG cSPRSREG = db.CSPRSREGs.Find(MyHtmlHelpers.ConvertByteStrToId(id), ctry, regtype, regno);
            if (cSPRSREG == null)
            {
                return HttpNotFound();
            }
            return View(cSPRSREG);
        }

        // POST: CSPRSREGs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id, string ctry, string regtype, string regno)
        {
            CSPRSREG cSPRSREG = db.CSPRSREGs.Find(MyHtmlHelpers.ConvertByteStrToId(id), ctry, regtype, regno);
            try
            {
            
                db.CSPRSREGs.Remove(cSPRSREG);
                db.SaveChanges();
                return RedirectToAction("Edit", "CSPRs", new { id = id });
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
            ViewBag.PRSCODE = MyHtmlHelpers.ConvertByteStrToId(id);
            return View(cSPRSREG);
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
