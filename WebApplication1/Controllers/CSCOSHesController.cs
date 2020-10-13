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
    public class CSCOSHesController : Controller
    {
        private ASIDBConnection db = new ASIDBConnection();

        // GET: CSCOSHes
        public ActionResult Index()
        {
            var cSCOSHes = db.CSCOSHes.Include(c => c.CSPR);
            return View(cSCOSHes.ToList());
        }

        public PartialViewResult PartialSum(string id, string person)
        {

            string sid = MyHtmlHelpers.ConvertByteStrToId(id);
            ViewBag.Parent = sid;
            ViewBag.CONO = sid;
            ViewBag.PRSCODE = person;


            var cSCOSHEQ = db.CSCOSHEQs.Where(x => x.CONO == sid && x.PRSCODE == person  && x.COMPLETE == "N").ToList();
            return PartialView("Partial/PartialSum", cSCOSHEQ);
        }

        public PartialViewResult PartialMove(string id, string person)
        {

            string sid = MyHtmlHelpers.ConvertByteStrToId(id);
            ViewBag.Parent = sid;
            ViewBag.CONO = sid;
            ViewBag.PRSCODE = person;


            var cSCOSHEQ = db.CSCOSHEQs.Where(x => x.CONO == sid && x.PRSCODE == person).ToList();
            return PartialView("Partial/PartialMove", cSCOSHEQ);
        }

        public PartialViewResult EditRegChg(string id, string person)
        {

            string sid = MyHtmlHelpers.ConvertByteStrToId(id);
            ViewBag.Parent = sid;
            ViewBag.CONO = sid;
            ViewBag.PRSCODE = person;


            var cSCOSHCHG = db.CSCOSHCHGs.Where(x => x.CONO == sid && x.PRSCODE == person).ToList();
            return PartialView("Partial/PartialChg", cSCOSHCHG);
        }

        public PartialViewResult PartialCOSHAllotment(string id)
        {
            ViewBag.Parent = MyHtmlHelpers.ConvertByteStrToId(id);
            string sid = ViewBag.Parent;

            var curRec = db.CSCOSHEQs.Where(x => x.CONO == sid && x.MVTYPE == "Allotment").OrderBy(x => x.MVNO).ThenBy(x => x.MVID);
       
            return PartialView("Partial/PartialCOSHAllotment", curRec);
        }

        public PartialViewResult PartialCOSHTransfer(string id)
        {
            ViewBag.Parent = MyHtmlHelpers.ConvertByteStrToId(id);
            string sid = ViewBag.Parent;

            var curRec = db.CSCOSHEQs.Where(x => x.CONO == sid && x.MVTYPE == "Transfer").OrderBy(x => x.MVNO).ThenBy(x => x.MVID);

            return PartialView("Partial/PartialCOSHTransfer", curRec);
        }

        public PartialViewResult PartialCOSHSplit(string id)
        {
            ViewBag.Parent = MyHtmlHelpers.ConvertByteStrToId(id);
            string sid = ViewBag.Parent;

            var curRec = db.CSCOSHEQs.Where(x => x.CONO == sid && x.MVTYPE == "Split").OrderBy(x => x.MVNO).ThenBy(x => x.MVID);

            return PartialView("Partial/PartialCOSHSplit", curRec);
        }

        public PartialViewResult PartialCOSHConversion(string id)
        { 
            ViewBag.Parent = MyHtmlHelpers.ConvertByteStrToId(id);
            string sid = ViewBag.Parent;

            var curRec = db.CSCOSHEQs.Where(x => x.CONO == sid && x.MVTYPE == "Conversion").OrderBy(x => x.MVNO).ThenBy(x => x.MVID);

            return PartialView("Partial/PartialCOSHConversion", curRec);
        }

        public ActionResult ShareMove(string id)
        {
            ViewBag.Parent = MyHtmlHelpers.ConvertByteStrToId(id);
            string sid = ViewBag.Parent;

            CSCOSH curRec = new CSCOSH();
            curRec.CONO = sid;
            curRec.EFFDATE = DateTime.Today;
            curRec.ENDDATE = new DateTime(3000, 1, 1);
            curRec.CSCOMSTR = db.CSCOMSTRs.Find(curRec.CONO);
            ViewBag.PRSCODE = new SelectList(db.CSPRS.OrderBy(x => x.PRSNAME), "PRSCODE", "PRSNAME", curRec.PRSCODE);
            return View(curRec);
        }

        // GET: CSCOSHes/Details/5
        public ActionResult Details(string id, string person)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSCOSH cSCOSH = db.CSCOSHes.Find(MyHtmlHelpers.ConvertByteStrToId(id), person);
            if (cSCOSH == null)
            {
                return HttpNotFound();
            }
            ViewBag.PRSCODE = new SelectList(db.CSPRS.OrderBy(x => x.PRSNAME), "PRSCODE", "PRSNAME", cSCOSH.PRSCODE);

            return View(cSCOSH);
        }

        // GET: CSCOSHes/Create
        public ActionResult Create(string id)
        {
            ViewBag.Parent = MyHtmlHelpers.ConvertByteStrToId(id);
            string sid = ViewBag.Parent;

            CSCOSH curRec = new CSCOSH();
            curRec.CONO = sid;
            curRec.EFFDATE = DateTime.Today;
            curRec.ENDDATE = new DateTime(3000, 1, 1);
            curRec.CSCOMSTR = db.CSCOMSTRs.Find(curRec.CONO);
            ViewBag.PRSCODE = new SelectList(db.CSPRS.OrderBy(x => x.PRSNAME), "PRSCODE", "PRSNAME", curRec.PRSCODE);
            return View(curRec);
        }

        // POST: CSCOSHes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CONO,PRSCODE,EFFDATE,TERDATE,ENDDATE,FOLIONO,REM,STAMP")] CSCOSH cSCOSH)
        {
            if (ModelState.IsValid)
            {
                cSCOSH.STAMP = 0;
                try
                {
                    int lastRowNo = 0;
                    try
                    {
                        lastRowNo = db.CSCOSHes.Where(m => m.CONO == cSCOSH.CONO).Max(n => n.FOLIONO);
                    }
                    catch (Exception e) { lastRowNo = 0; }
                    finally { };

                    cSCOSH.FOLIONO = lastRowNo + 1;
                    db.CSCOSHes.Add(cSCOSH);

                    CSCOLASTNO cSCOLASTNO = db.CSCOLASTNOes.Find( cSCOSH.CONO, "SHFOLIO");
                    if (cSCOLASTNO != null)
                    {
                        cSCOLASTNO.LASTNO = cSCOSH.FOLIONO;
                        db.Entry(cSCOLASTNO).State = EntityState.Modified;
                    }
                    db.SaveChanges();
                    return new RedirectResult(Url.Action("Edit", "CSCOMSTRs", new { id = MyHtmlHelpers.ConvertIdToByteStr(cSCOSH.CONO) }) + "#Member");
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

            cSCOSH.CSCOMSTR = db.CSCOMSTRs.Find(cSCOSH.CONO);
            ViewBag.PRSCODE = new SelectList(db.CSPRS.OrderBy(x => x.PRSNAME), "PRSCODE", "PRSNAME", cSCOSH.PRSCODE);
            return View(cSCOSH);
        }

        // GET: CSCOSHes/Edit/5
        public ActionResult Edit(string id, string person)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSCOSH cSCOSH = db.CSCOSHes.Find(MyHtmlHelpers.ConvertByteStrToId(id), person);
            if (cSCOSH == null)
            {
                return HttpNotFound();
            }
            Session["CSCOSH_ORIG"] = cSCOSH;
            ViewBag.PRSCODE = new SelectList(db.CSPRS.OrderBy(x => x.PRSNAME), "PRSCODE", "PRSNAME", cSCOSH.PRSCODE);

            return View(cSCOSH);
        }

        // POST: CSCOSHes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CONO,PRSCODE,EFFDATE,TERDATE,ENDDATE,FOLIONO,REM,STAMP")] CSCOSH cSCOSH)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    bool changed = false;
                    CSCOSH csOrig = (CSCOSH)Session["CSCOSH_ORIG"];
                    if (csOrig.PRSCODE != cSCOSH.PRSCODE) { changed = true; }

                    cSCOSH.STAMP = cSCOSH.STAMP + 1;
                    if (changed)
                    {
                        CSCOSH csDel = db.CSCOSHes.Find(csOrig.CONO, csOrig.PRSCODE);
                        db.CSCOSHes.Remove(csDel);
                        db.CSCOSHes.Add(cSCOSH);
                    }
                    else
                    {
                        db.Entry(cSCOSH).State = EntityState.Modified;
                    }

                    db.SaveChanges();
                    return new RedirectResult(Url.Action("Edit", "CSCOMSTRs", new { id = MyHtmlHelpers.ConvertIdToByteStr(cSCOSH.CONO) }) + "#Member");
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
            cSCOSH.CSCOMSTR = db.CSCOMSTRs.Find(cSCOSH.CONO);
            ViewBag.PRSCODE = new SelectList(db.CSPRS.OrderBy(x => x.PRSNAME), "PRSCODE", "PRSNAME", cSCOSH.PRSCODE);
            cSCOSH.CSCOSHCHGs = db.CSCOSHCHGs.Where(x => x.CONO == cSCOSH.CONO && x.PRSCODE == cSCOSH.PRSCODE ).ToList();
            return View(cSCOSH);
        }

        // GET: CSCOSHes/Delete/5
        public ActionResult Delete(string id, string person)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSCOSH cSCOSH = db.CSCOSHes.Find(MyHtmlHelpers.ConvertByteStrToId(id), person);
            if (cSCOSH == null)
            {
                return HttpNotFound();
            }
            return View(cSCOSH);
        }

        // POST: CSCOSHes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id, string person)
        {
            CSCOSH cSCOSH = db.CSCOSHes.Find(MyHtmlHelpers.ConvertByteStrToId(id), person);
            try
            {
                db.CSCOSHes.Remove(cSCOSH);
                db.SaveChanges();
                return new RedirectResult(Url.Action("Edit", "CSCOMSTRs", new { id = MyHtmlHelpers.ConvertIdToByteStr(cSCOSH.CONO) }) + "#Member");
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
            return View(cSCOSH);
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
