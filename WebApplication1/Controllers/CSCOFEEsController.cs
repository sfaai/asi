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
    public class CSCOFEEsController : Controller
    {
        private ASIDBConnection db = new ASIDBConnection();

        // GET: CSCOFEEs

        public JsonResult SetProcParam(string ProcDateParam)
        {
            DateTime procdate = DateTime.Today;
            DateTime.TryParse(ProcDateParam, out procdate);

            Session["CSCOFEE_procdate"] = procdate;

            return Json("OK", JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Administrator,CS-A/C")]
        public ActionResult Index()
        {
            if (Session["CSCOFEE_procdate"] == null)
            {
                Session["CSCOFEE_procdate"] = DateTime.Today;
            }

            DateTime ProcDate = (DateTime)Session["CSCOFEE_procdate"];
            ViewBag.PROCDATE = ProcDate.ToString("yyyy-MM-dd");
            var cSCOFEEs = db.CSCOFEEs.Include(c => c.CSCASE);
            cSCOFEEs = cSCOFEEs.Include(x => x.CSCOMSTR).Where(y => y.CSCOMSTR.COSTAT == "Active" && y.LASTTOUCH <= ProcDate);
            cSCOFEEs = cSCOFEEs.OrderBy(x => x.LASTTOUCH);
            return View(cSCOFEEs.ToList());
        }

        [HttpPost]
        public ActionResult Post(string[] cono)
        {

            CSPRFsController CSPRFControl = new CSPRFsController();
            try
            {
                SALASTNO serialTbl0 = db.SALASTNOes.Find("CSBILL");
                if ((serialTbl0 != null))
                {
                    foreach (string item in cono)
                    {
                        var conofee = item.Split('|');
                        var myCono = conofee[0];
                        var myFee = conofee[1];
                        string lastCono = "";
                        CSCOMSTR cSCOMSTR = null;
                        myCono = myCono.Trim();
                        myFee = myFee.Trim();



                        var csFees = db.CSCOFEEs.Where(x => x.CONO == myCono && x.FEETYPE == myFee);
                        decimal taxrate = 0;
                        string taxcode = "SSTN01";
                        CSPRF cSPRF = null;
                        int prfId = 0;
                        foreach (var feerec in csFees)
                        {
                            CSITEM cSITEM = db.CSITEMs.Find(feerec.FEETYPE);
                            if (cSITEM != null)
                            {
                                taxrate = cSITEM.GSTRATE ?? 0;
                            }

                            if (lastCono != feerec.CONO)
                            {
                                cSCOMSTR = db.CSCOMSTRs.Find(feerec.CONO);
                                cSCOMSTR.SEQNO = cSCOMSTR.SEQNO + 1;
                                cSCOMSTR.STAMP = cSCOMSTR.STAMP;
                                db.Entry(cSCOMSTR).State = EntityState.Modified;
                            }

                            var feedate = feerec.LASTTOUCH;
                        

                            feerec.LASTTOUCH = feedate.AddMonths(feerec.FEEMTH);
                            feerec.STAMP = feerec.STAMP + 1;
                            db.Entry(feerec).State = EntityState.Modified;

                          
                            SALASTNO serialTbl = db.SALASTNOes.Find("CSPRF");
                            if ((serialTbl != null) && (lastCono != feerec.CONO))
                            {
                                string prefix = serialTbl.LASTPFIX;
                                int MaxNo = serialTbl.LASTNOMAX;
                                bool AutoGen = serialTbl.AUTOGEN == "Y";
                                serialTbl.LASTNO = serialTbl.LASTNO + 1;


                                cSPRF = new CSPRF();
                                cSPRF.STAMP = 0;
                                cSPRF.VDATE = feedate;
                                cSPRF.DUEDATE = feedate;
                                cSPRF.DUEDAYS = 0;
                                cSPRF.SEQNO = cSCOMSTR.SEQNO;
                                cSPRF.CONO = feerec.CONO;
                                short? coid = db.CSCOADDRs.Where(x => x.CONO == feerec.CONO && x.MAILADDR == "Y").Select(y => y.ADDRID).FirstOrDefault();
                                if (coid == 0) { coid = db.CSCOADDRs.Where(x => x.CONO == feerec.CONO && x.MAILADDR == "N").Select(y => y.ADDRID).FirstOrDefault(); }
                                if (coid != 0) { cSPRF.COADDRID = coid; }

                                cSPRF.ATTN = "The Board of Directors";
                                cSPRF.PRFNO = serialTbl.LASTNO.ToString("D10");
                                cSPRF.INVALLOC = "N";

                                serialTbl.STAMP = serialTbl.STAMP + 1;
                                db.Entry(serialTbl).State = EntityState.Modified;
                                db.CSPRFs.Add(cSPRF);
                                prfId = 0;
                            }

                            prfId++;
                            serialTbl0.LASTNO = serialTbl0.LASTNO + 1;
                            string prefix0 = serialTbl0.LASTPFIX;
                            int MaxNo0 = serialTbl0.LASTNOMAX;
                            bool AutoGen0 = serialTbl0.AUTOGEN == "Y";
                            CSBILL cSBILL = new WebApplication1.CSBILL();
                            cSBILL.STAMP = 0;
                            cSBILL.CONO = feerec.CONO;
                            cSBILL.PRFALLOC = "Y";
                            cSBILL.SYSGEN = "Y";
                            cSBILL.PRFNO = cSPRF.PRFNO;
                            cSBILL.PRFID = prfId;
                            cSBILL.BILLNO = serialTbl0.LASTNO.ToString("D10"); ;
                            cSBILL.ENTDATE = feedate;
                            cSBILL.CASECODE = feerec.FEECODE;
                            cSBILL.ITEMTYPE = feerec.FEETYPE;
                            cSBILL.ITEMDESC = feerec.CSCASE.CASEDESC;
                            cSBILL.ITEMSPEC = "- From " + feedate.ToString("dd/MM/yyyy") + " To " + feedate.AddMonths(feerec.FEEMTH).ToString("dd/MM/yyyy");

                            cSBILL.ITEMAMT = feerec.FEEAMT;
                            cSBILL.TAXAMT = taxrate * feerec.FEEAMT / 100;
                            cSBILL.NETAMT = cSBILL.ITEMAMT + cSBILL.TAXAMT;

                            cSBILL.ITEMAMT1 = feerec.FEEAMT;
                            cSBILL.TAXAMT1 = taxrate * feerec.FEEAMT / 100;
                            cSBILL.NETAMT1 = cSBILL.ITEMAMT1 + cSBILL.TAXAMT1;

                            cSBILL.ITEMAMT2 = 0;
                            cSBILL.TAXAMT2 = 0;
                            cSBILL.NETAMT2 = 0;
                            cSBILL.TAXRATE = taxrate;

                            taxcode = db.CSTAXTYPEs.Where(x => x.TAXRATE == taxrate && x.EFFECTIVE_START <= cSBILL.ENTDATE && x.EFFECTIVE_END >= cSBILL.ENTDATE).Select(y => y.TAXCODE).FirstOrDefault() ?? taxcode;
                            cSBILL.TAXCODE = taxcode;
                            db.CSBILLs.Add(cSBILL);


                            CSTRANM cSTRANM = new CSTRANM();
                            cSTRANM.STAMP = 0;
                            CSPRFControl.BD_UpdateCSTRANM(cSPRF, cSBILL, cSTRANM);

                            db.CSTRANMs.Add(cSTRANM);

                            lastCono = feerec.CONO;
                        }
                        //db.Entry(serialTbl0).State = EntityState.Modified;
                        //db.SaveChanges();
                    }
                    db.Entry(serialTbl0).State = EntityState.Modified;
                    db.SaveChanges();
                }


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
            finally
            {
                CSPRFControl.Dispose();
            }

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        // GET: CSCOFEEs/Details/5
        public ActionResult Details(string id, string fee)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSCOFEE cSCOFEE = db.CSCOFEEs.Find(MyHtmlHelpers.ConvertByteStrToId(id), fee);
            if (cSCOFEE == null)
            {
                return HttpNotFound();
            }
            ViewBag.FEETYPE = new SelectList(db.CSITEMs, "ITEMTYPE", "ITEMDESC", cSCOFEE.FEETYPE);
            ViewBag.FEECODE = new SelectList(db.CSCASEs, "CASECODE", "CASEDESC", cSCOFEE.FEECODE);
            return View(cSCOFEE);
        }

        // GET: CSCOFEEs/Create
        public ActionResult Create(string id)
        {
            ViewBag.Parent = MyHtmlHelpers.ConvertByteStrToId(id);
            CSCOFEE curRec = new CSCOFEE();
            curRec.CONO = ViewBag.Parent;
           
            curRec.CSCOMSTR = db.CSCOMSTRs.Find(curRec.CONO);
            ViewBag.FEETYPE = new SelectList(db.CSITEMs, "ITEMTYPE", "ITEMDESC");
            ViewBag.FEECODE = new SelectList(db.CSCASEs, "CASECODE", "CASEDESC");
            return View(curRec);
        }

        // POST: CSCOFEEs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CONO,FEECODE,FEEAMT,FEEMTH,FEETYPE,LASTTOUCH,STAMP")] CSCOFEE cSCOFEE)
        {
            if (ModelState.IsValid)
            {try
                {
                    cSCOFEE.STAMP = 0;
                    db.CSCOFEEs.Add(cSCOFEE);
                    db.SaveChanges();
                    return new RedirectResult(Url.Action("Edit", "CSCOMSTRs", new { id = MyHtmlHelpers.ConvertIdToByteStr(cSCOFEE.CONO) }) + "#Fee");
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
            ViewBag.FEETYPE = new SelectList(db.CSITEMs, "ITEMTYPE", "ITEMDESC", cSCOFEE.FEETYPE);
            ViewBag.FEECODE = new SelectList(db.CSCASEs, "CASECODE", "CASEDESC", cSCOFEE.FEECODE);
            cSCOFEE.CSCOMSTR = db.CSCOMSTRs.Find(cSCOFEE.CONO);
            return View(cSCOFEE);
        }

        // GET: CSCOFEEs/Edit/5
        public ActionResult Edit(string id, string fee)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSCOFEE cSCOFEE = db.CSCOFEEs.Find(MyHtmlHelpers.ConvertByteStrToId(id), fee);
            if (cSCOFEE == null)
            {
                return HttpNotFound();
            }

            Session["CSCOFEE_ORIG"] = cSCOFEE;
            ViewBag.FEETYPE = new SelectList(db.CSITEMs, "ITEMTYPE", "ITEMDESC", cSCOFEE.FEETYPE);
            ViewBag.FEECODE = new SelectList(db.CSCASEs, "CASECODE", "CASEDESC", cSCOFEE.FEECODE);
            return View(cSCOFEE);
        }

        // POST: CSCOFEEs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CONO,FEECODE,FEEAMT,FEEMTH,FEETYPE,LASTTOUCH,STAMP")] CSCOFEE cSCOFEE)
        {
            if (ModelState.IsValid)

            {
                try
                {
                    bool changed = false;
                    CSCOFEE csOrig = (CSCOFEE)Session["CSCOFEE_ORIG"];
                    if (csOrig.FEECODE != cSCOFEE.FEECODE) { changed = true; }

                    cSCOFEE.STAMP = cSCOFEE.STAMP + 1;
                    if (changed)
                    {
                        CSCOFEE csDel = db.CSCOFEEs.Find(csOrig.CONO, csOrig.FEECODE);
                        db.CSCOFEEs.Remove(csDel);
                        db.CSCOFEEs.Add(cSCOFEE);
                    }
                    else
                    {
                        db.Entry(cSCOFEE).State = EntityState.Modified;
                    }
                    db.SaveChanges();
                    return new RedirectResult(Url.Action("Edit", "CSCOMSTRs", new { id = MyHtmlHelpers.ConvertIdToByteStr(cSCOFEE.CONO) }) + "#Fee");
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
            ViewBag.FEETYPE = new SelectList(db.CSITEMs, "ITEMTYPE", "ITEMDESC", cSCOFEE.FEETYPE);
            ViewBag.FEECODE = new SelectList(db.CSCASEs, "CASECODE", "CASEDESC", cSCOFEE.FEECODE);
            cSCOFEE.CSCOMSTR = db.CSCOMSTRs.Find(cSCOFEE.CONO);
            return View(cSCOFEE);
        }

        // GET: CSCOFEEs/Delete/5
        public ActionResult Delete(string id, string fee)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSCOFEE cSCOFEE = db.CSCOFEEs.Find(MyHtmlHelpers.ConvertByteStrToId(id), fee);
            if (cSCOFEE == null)
            {
                return HttpNotFound();
            }
            return View(cSCOFEE);
        }

        // POST: CSCOFEEs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id, string fee)
        {
            CSCOFEE cSCOFEE = db.CSCOFEEs.Find(MyHtmlHelpers.ConvertByteStrToId(id), fee);
            try
            {
                db.CSCOFEEs.Remove(cSCOFEE);
                db.SaveChanges();
                return new RedirectResult(Url.Action("Edit", "CSCOMSTRs", new { id = MyHtmlHelpers.ConvertIdToByteStr(cSCOFEE.CONO) }) + "#Fee");
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
            cSCOFEE.CSCOMSTR = db.CSCOMSTRs.Find(cSCOFEE.CONO);
            return View(cSCOFEE);
        }

        public ActionResult AutogenFee(DateTime ProcDate)
        {
            var FeeList = db.CSCOFEEs.Include(x => x.CSCOMSTR).Where(y => y.CSCOMSTR.COSTAT == "Active" && y.LASTTOUCH < ProcDate);
            return View();
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



