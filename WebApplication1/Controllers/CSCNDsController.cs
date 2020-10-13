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
    public class CSCNDsController : Controller
    {
        private ASIDBConnection db = new ASIDBConnection();

        // GET: CSCNDs
        public ActionResult Index()
        {
            var cSCNDs = db.CSCNDs.Include(c => c.CSCASE);
            return View(cSCNDs.ToList());
        }

        // GET: CSCNDs/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSCND cSCND = db.CSCNDs.Find(MyHtmlHelpers.ConvertByteStrToId(id));
            if (cSCND == null)
            {
                return HttpNotFound();
            }
            return View(cSCND);
        }

        // GET: CSCNDs/Create
        public ActionResult Create(string id)
        {
            CSCND cSCND = new WebApplication1.CSCND();
            cSCND.STAMP = 0;
            cSCND.TAXRATE = 0;
            cSCND.TRNO = MyHtmlHelpers.ConvertByteStrToId(id);

            ViewBag.CASECODE = new SelectList(db.CSCASEs, "CASECODE", "CASEDESC");
            ViewBag.ITEMTYPE = new SelectList(db.CSITEMs, "ITEMTYPE", "ITEMDESC", cSCND.ITEMTYPE);
            return View("Edit", cSCND);
        }

        // POST: CSCNDs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TRNO,TRID,CASECODE,ITEMTYPE,ITEMDESC,ITEMSPEC,TAXRATE,ITEMAMT1,TAXAMT1,NETAMT1,ITEMAMT2,TAXAMT2,NETAMT2,ITEMAMT,TAXAMT,NETAMT,STAMP")] CSCND cSCND)
        {
            if (ModelState.IsValid)
            {
                CSCND lastRec = db.CSCNDs.Where(m => m.TRNO == cSCND.TRNO).OrderByDescending(n => n.TRID).FirstOrDefault();
                if (lastRec == null)
                {
                    cSCND.TRID = 1;
                }
                else
                {
                    cSCND.TRID = lastRec.TRID + 1;
                }

                db.CSCNDs.Add(cSCND);

                CSCNM parent = db.CSCNMs.Find(cSCND.TRNO);
                if (parent != null)
                {
                    try
                    {
                        CSTRANM cSTRANM = db.CSTRANMs.Find("CSCN", cSCND.TRNO, cSCND.TRID);
                        if (cSTRANM == null)
                        {
                            cSTRANM = new CSTRANM();
                            cSTRANM.STAMP = 0;
                            UpdateCSTRANM(parent, cSCND, cSTRANM);
                            db.CSTRANMs.Add(cSTRANM);
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "CSTRANM record already Existed");
                            throw new Exception("CSTRANM record already Existed");
                            //UpdateCSTRANM(parent, cSCND, cSTRANM);
                            //db.Entry(cSTRANM).State = EntityState.Modified;
                        }
                        db.SaveChanges();
                        return RedirectToAction("Edit", "CSCNMs", new { id = MyHtmlHelpers.ConvertIdToByteStr(cSCND.TRNO) });
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
                else
                {
                    ModelState.AddModelError(string.Empty, "Parent Record is missing");
                }
            }
            ViewBag.CASECODE = new SelectList(db.CSCASEs, "CASECODE", "CASEDESC", cSCND.CASECODE);
            ViewBag.ITEMTYPE = new SelectList(db.CSITEMs, "ITEMTYPE", "ITEMDESC", cSCND.ITEMTYPE);
            return View("Edit", cSCND);
        }

        // GET: CSCNDs/Edit/5
        public ActionResult Edit(string id, int row)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSCND cSCND = db.CSCNDs.Find(MyHtmlHelpers.ConvertByteStrToId(id), row);
            if (cSCND == null)
            {
                return HttpNotFound();
            }
            ViewBag.CASECODE = new SelectList(db.CSCASEs, "CASECODE", "CASEDESC", cSCND.CASECODE);
            ViewBag.ITEMTYPE = new SelectList(db.CSITEMs, "ITEMTYPE", "ITEMDESC", cSCND.ITEMTYPE);
            ViewBag.Title = "Edit Discounted Bills Details";

            return View("Edit", cSCND);
        }

        // POST: CSCNDs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TRNO,TRID,CASECODE,ITEMTYPE,ITEMDESC,ITEMSPEC,TAXRATE,ITEMAMT1,TAXAMT1,NETAMT1,ITEMAMT2,TAXAMT2,NETAMT2,ITEMAMT,TAXAMT,NETAMT,STAMP")] CSCND cSCND)
        {
            if (ModelState.IsValid)
            {
                ASIDBConnection newdb = new ASIDBConnection();
                db.Entry(cSCND).State = EntityState.Modified;
                try
                {
                    CSCNM parent = newdb.CSCNMs.Find(cSCND.TRNO);
                    if (parent != null)
                    {
                        CSCND curRec = newdb.CSCNDs.Find(cSCND.TRNO, cSCND.TRID);
                        if (curRec.STAMP == cSCND.STAMP)
                        {
                            cSCND.STAMP = cSCND.STAMP + 1;

                            CSTRANM cSTRANM = db.CSTRANMs.Find("CSCN", cSCND.TRNO, cSCND.TRID);
                            if (cSTRANM == null)
                            {
                                cSTRANM = new CSTRANM();
                                cSTRANM.STAMP = 0;
                                UpdateCSTRANM(parent, cSCND, cSTRANM);
                                db.CSTRANMs.Add(cSTRANM);
                            }
                            else
                            {
                                UpdateCSTRANM(parent, cSCND, cSTRANM);
                                db.Entry(cSTRANM).State = EntityState.Modified;
                            }

                            db.SaveChanges();

                            return RedirectToAction("Edit", "CSCNMs", new { id = MyHtmlHelpers.ConvertIdToByteStr(cSCND.TRNO) });
                        }
                        else { ModelState.AddModelError(string.Empty, "Record is modified"); }
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Parent Record is missing");
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
                    newdb.Dispose();
                }
            }
            ViewBag.CASECODE = new SelectList(db.CSCASEs, "CASECODE", "CASEDESC", cSCND.CASECODE);
            ViewBag.ITEMTYPE = new SelectList(db.CSITEMs, "ITEMTYPE", "ITEMDESC", cSCND.ITEMTYPE);
            return View(cSCND);
        }

        // GET: CSCNDs/Delete/5
        public ActionResult Delete(string id, int row)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSCND cSCND = db.CSCNDs.Find(MyHtmlHelpers.ConvertByteStrToId(id), row);
            if (cSCND == null)
            {
                return HttpNotFound();
            }
            ViewBag.CASECODE = new SelectList(db.CSCASEs, "CASECODE", "CASEDESC", cSCND.CASECODE);
            ViewBag.ITEMTYPE = new SelectList(db.CSITEMs, "ITEMTYPE", "ITEMDESC", cSCND.ITEMTYPE);
            return View(cSCND);
        }

        // POST: CSCNDs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id, int row)
        {
            string trno = DeleteRow(db, id, row);
            try
            {
                db.SaveChanges();
                return RedirectToAction("Edit", "CSCNMs", new { id = MyHtmlHelpers.ConvertIdToByteStr(trno) });
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
            CSCND cSCND = db.CSCNDs.Find(MyHtmlHelpers.ConvertByteStrToId(id), row);
            ViewBag.CASECODE = new SelectList(db.CSCASEs, "CASECODE", "CASEDESC", cSCND.CASECODE);
            ViewBag.ITEMTYPE = new SelectList(db.CSITEMs, "ITEMTYPE", "ITEMDESC", cSCND.ITEMTYPE);
            return View(cSCND);
        }

        public string DeleteRow(ASIDBConnection mydb, string id, int row)
        {
            CSCND cSCND = mydb.CSCNDs.Find(MyHtmlHelpers.ConvertByteStrToId(id), row);
            CSTRANM cSTRANM = mydb.CSTRANMs.Find("CSCN", cSCND.TRNO, cSCND.TRID);
            if (cSTRANM.REFCNT > 0) { throw new Exception("cannot delete record that has been applied");  }
            mydb.CSTRANMs.Remove(cSTRANM);
            mydb.CSCNDs.Remove(cSCND);
            return cSCND.TRNO;
        }

        protected void UpdateCSTRANM(CSCNM cSCNM, CSCND cSCND, CSTRANM cSTRANM)
        {
            cSTRANM.SOURCE = "CSCN";
            cSTRANM.SOURCENO = cSCNM.TRNO;
            cSTRANM.SOURCEID = cSCND.TRID;
            cSTRANM.CONO = cSCNM.CONO;
            cSTRANM.JOBNO = null;
            cSTRANM.CASENO = null;
            cSTRANM.CASECODE = cSCND.CASECODE;
            cSTRANM.ENTDATE = cSCNM.VDATE;
            cSTRANM.TRTYPE = cSCND.ITEMTYPE;
            cSTRANM.TRDESC = cSCND.ITEMDESC;

            cSTRANM.TRITEM1 = cSCND.ITEMAMT1;
            cSTRANM.TRITEM2 = cSCND.ITEMAMT2;
            cSTRANM.TRITEM = cSCND.ITEMAMT1 + cSCND.ITEMAMT2;

            cSTRANM.TRTAX1 = cSCND.TAXAMT1;
            cSTRANM.TRTAX2 = cSCND.TAXAMT2;
            cSTRANM.TRTAX = cSCND.TAXAMT1 + cSCND.TAXAMT2;

            cSTRANM.TRAMT1 = cSCND.NETAMT1;
            cSTRANM.TRAMT2 = cSCND.NETAMT2;
            cSTRANM.TRAMT = cSCND.NETAMT1 + cSCND.NETAMT2;


            cSTRANM.TRSIGN = "CR";

            cSTRANM.TRSITEM1 = -cSCND.ITEMAMT1;
            cSTRANM.TRSITEM2 = -cSCND.ITEMAMT2;
            cSTRANM.TRSITEM = -cSCND.ITEMAMT1 - cSCND.ITEMAMT2;

            cSTRANM.TRSTAX1 = -cSCND.TAXAMT1;
            cSTRANM.TRSTAX2 = -cSCND.TAXAMT2;
            cSTRANM.TRSTAX = -cSCND.TAXAMT1 - cSCND.TAXAMT2;

            cSTRANM.TRSAMT1 = -cSCND.NETAMT1;
            cSTRANM.TRSAMT2 = -cSCND.NETAMT2;
            cSTRANM.TRSAMT = -cSCND.NETAMT1 - cSCND.NETAMT2;

            cSTRANM.TRITEMOS1 = -cSCND.ITEMAMT1;
            cSTRANM.TRITEMOS2 = -cSCND.ITEMAMT2;
            cSTRANM.TRITEMOS = -cSCND.ITEMAMT1 - cSCND.ITEMAMT2;


            cSTRANM.TRTAXOS1 = -cSCND.TAXAMT1;
            cSTRANM.TRTAXOS2 = -cSCND.TAXAMT2;
            cSTRANM.TRTAXOS = -cSCND.TAXAMT1 - cSCND.TAXAMT2;


            cSTRANM.TROS1 = -cSCND.NETAMT1;
            cSTRANM.TROS2 = -cSCND.ITEMAMT2;
            cSTRANM.TROS = -cSCND.NETAMT1 - cSCND.NETAMT2;


            cSTRANM.APPITEM = 0;
            cSTRANM.APPITEM1 = 0;
            cSTRANM.APPITEM2 = 0;
            cSTRANM.APPTAX = 0;
            cSTRANM.APPTAX1 = 0;
            cSTRANM.APPTAX2 = 0;
            cSTRANM.APPAMT = 0;
            cSTRANM.APPAMT1 = 0;
            cSTRANM.APPAMT2 = 0;

            cSTRANM.APPTYPE = null;
            cSTRANM.APPNO = null;
            cSTRANM.APPID = null;

            cSTRANM.COMPLETE = "N";
            cSTRANM.COMPLETED = DateTime.Parse("01/01/3000");
            cSTRANM.SEQNO = cSCNM.SEQNO;
            cSTRANM.REFCNT = 0;
            cSTRANM.STAMP = cSTRANM.STAMP + 1;
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
