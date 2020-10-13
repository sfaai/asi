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
    public class CSDNDsController : Controller
    {
        private ASIDBConnection db = new ASIDBConnection();

        // GET: CSCNDs
        public ActionResult Index()
        {
            var cSDNDs = db.CSDNDs.Include(c => c.CSCASE);
            return View(cSDNDs.ToList());
        }

        // GET: CSDNDs/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSDND cSDND = db.CSDNDs.Find(MyHtmlHelpers.ConvertByteStrToId(id));
            if (cSDND == null)
            {
                return HttpNotFound();
            }
            return View(cSDND);
        }

        // GET: CSDNDs/Create
        public ActionResult Create(string id)
        {
            CSDND cSDND = new WebApplication1.CSDND();
            cSDND.STAMP = 0;
            cSDND.TAXRATE = 0;
            cSDND.TRNO = MyHtmlHelpers.ConvertByteStrToId(id);

            ViewBag.CASECODE = new SelectList(db.CSCASEs, "CASECODE", "CASEDESC");
            ViewBag.ITEMTYPE = new SelectList(db.CSITEMs, "ITEMTYPE", "ITEMDESC", cSDND.ITEMTYPE);
            return View("Edit", cSDND);
        }

        // POST: CSDNDs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TRNO,TRID,CASECODE,ITEMTYPE,ITEMDESC,ITEMSPEC,TAXRATE,ITEMAMT1,TAXAMT1,NETAMT1,ITEMAMT2,TAXAMT2,NETAMT2,ITEMAMT,TAXAMT,NETAMT,STAMP")] CSDND cSDND)
        {
            if (ModelState.IsValid)
            {
                CSDND lastRec = db.CSDNDs.Where(m => m.TRNO == cSDND.TRNO).OrderByDescending(n => n.TRID).FirstOrDefault();
                if (lastRec == null)
                {
                    cSDND.TRID = 1;
                }
                else
                {
                    cSDND.TRID = lastRec.TRID + 1;
                }

                db.CSDNDs.Add(cSDND);

                CSDNM parent = db.CSDNMs.Find(cSDND.TRNO);
                if (parent != null)
                {
                    try
                    {
                        CSTRANM cSTRANM = db.CSTRANMs.Find("CSDN", cSDND.TRNO, cSDND.TRID);
                        if (cSTRANM == null)
                        {
                            cSTRANM = new CSTRANM();
                            cSTRANM.STAMP = 0;
                            UpdateCSTRANM(parent, cSDND, cSTRANM);
                            db.CSTRANMs.Add(cSTRANM);
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "CSTRANM record already Existed");
                            throw new Exception("CSTRANM record already Existed");
                            //UpdateCSTRANM(parent, cSDND, cSTRANM);
                            //db.Entry(cSTRANM).State = EntityState.Modified;
                        }
                        db.SaveChanges();
                        return RedirectToAction("Edit", "CSDNMs", new { id = MyHtmlHelpers.ConvertIdToByteStr(cSDND.TRNO) });
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
            ViewBag.CASECODE = new SelectList(db.CSCASEs, "CASECODE", "CASEDESC", cSDND.CASECODE);
            ViewBag.ITEMTYPE = new SelectList(db.CSITEMs, "ITEMTYPE", "ITEMDESC", cSDND.ITEMTYPE);
            return View("Edit", cSDND);
        }

        // GET: CSDNDs/Edit/5
        public ActionResult Edit(string id, int row)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSDND cSDND = db.CSDNDs.Find(MyHtmlHelpers.ConvertByteStrToId(id), row);
            if (cSDND == null)
            {
                return HttpNotFound();
            }
            ViewBag.CASECODE = new SelectList(db.CSCASEs, "CASECODE", "CASEDESC", cSDND.CASECODE);
            ViewBag.ITEMTYPE = new SelectList(db.CSITEMs, "ITEMTYPE", "ITEMDESC", cSDND.ITEMTYPE);
            ViewBag.Title = "Edit Debit Note Details";

            return View("Edit", cSDND);
        }

        // POST: CSDNDs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TRNO,TRID,CASECODE,ITEMTYPE,ITEMDESC,ITEMSPEC,TAXRATE,ITEMAMT1,TAXAMT1,NETAMT1,ITEMAMT2,TAXAMT2,NETAMT2,ITEMAMT,TAXAMT,NETAMT,STAMP")] CSDND cSDND)
        {
            if (ModelState.IsValid)
            {
                ASIDBConnection newdb = new ASIDBConnection();
                db.Entry(cSDND).State = EntityState.Modified;
                try
                {
                    CSDNM parent = newdb.CSDNMs.Find(cSDND.TRNO);
                    if (parent != null)
                    {
                        CSDND curRec = newdb.CSDNDs.Find(cSDND.TRNO, cSDND.TRID);
                        if (curRec.STAMP == cSDND.STAMP)
                        {
                            cSDND.STAMP = cSDND.STAMP + 1;

                            CSTRANM cSTRANM = db.CSTRANMs.Find("CSDN", cSDND.TRNO, cSDND.TRID);
                            if (cSTRANM == null)
                            {
                                cSTRANM = new CSTRANM();
                                cSTRANM.STAMP = 0;
                                UpdateCSTRANM(parent, cSDND, cSTRANM);
                                db.CSTRANMs.Add(cSTRANM);
                            }
                            else
                            {
                                UpdateCSTRANM(parent, cSDND, cSTRANM);
                                db.Entry(cSTRANM).State = EntityState.Modified;
                            }

                            db.SaveChanges();

                            return RedirectToAction("Edit", "CSDNMs", new { id = MyHtmlHelpers.ConvertIdToByteStr(cSDND.TRNO) });
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
            ViewBag.CASECODE = new SelectList(db.CSCASEs, "CASECODE", "CASEDESC", cSDND.CASECODE);
            ViewBag.ITEMTYPE = new SelectList(db.CSITEMs, "ITEMTYPE", "ITEMDESC", cSDND.ITEMTYPE);
            return View(cSDND);
        }

        // GET: CSDNDs/Delete/5
        public ActionResult Delete(string id, int row)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSDND cSDND = db.CSDNDs.Find(MyHtmlHelpers.ConvertByteStrToId(id), row);
            if (cSDND == null)
            {
                return HttpNotFound();
            }
            ViewBag.CASECODE = new SelectList(db.CSCASEs, "CASECODE", "CASEDESC", cSDND.CASECODE);
            ViewBag.ITEMTYPE = new SelectList(db.CSITEMs, "ITEMTYPE", "ITEMDESC", cSDND.ITEMTYPE);
            return View(cSDND);
        }

        // POST: CSDNDs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id, int row)
        {
            CSDND cSDND = db.CSDNDs.Find(MyHtmlHelpers.ConvertByteStrToId(id), row);
            try
            {
                string trno = DeleteRow(db, id, row);
                db.SaveChanges();
                return RedirectToAction("Edit", "CSDNMs", new { id = MyHtmlHelpers.ConvertIdToByteStr(trno) });
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

            ViewBag.CASECODE = new SelectList(db.CSCASEs, "CASECODE", "CASEDESC", cSDND.CASECODE);
            ViewBag.ITEMTYPE = new SelectList(db.CSITEMs, "ITEMTYPE", "ITEMDESC", cSDND.ITEMTYPE);
            return View(cSDND);

        }

        public string DeleteRow(ASIDBConnection mydb, string id, int row)
        {
            CSDND cSDND = mydb.CSDNDs.Find(MyHtmlHelpers.ConvertByteStrToId(id), row);
            CSTRANM cSTRANM = mydb.CSTRANMs.Find("CSDN", cSDND.TRNO, cSDND.TRID);
            if (cSTRANM.REFCNT > 0) { throw new Exception("cannot delete record that has been applied");  }
            mydb.CSTRANMs.Remove(cSTRANM);
            mydb.CSDNDs.Remove(cSDND);
            return cSDND.TRNO;
        }

        protected void UpdateCSTRANM(CSDNM cSDNM, CSDND cSDND, CSTRANM cSTRANM)
        {
            cSTRANM.SOURCE = "CSDN";
            cSTRANM.SOURCENO = cSDNM.TRNO;
            cSTRANM.SOURCEID = cSDND.TRID;
            cSTRANM.CONO = cSDNM.CONO;
            cSTRANM.JOBNO = null;
            cSTRANM.CASENO = null;
            cSTRANM.CASECODE = cSDND.CASECODE;
            cSTRANM.ENTDATE = cSDNM.VDATE;
            cSTRANM.TRTYPE = cSDND.ITEMTYPE;
            cSTRANM.TRDESC = cSDND.ITEMDESC;

            cSTRANM.TRITEM1 = cSDND.ITEMAMT1;
            cSTRANM.TRITEM2 = cSDND.ITEMAMT2;
            cSTRANM.TRITEM = cSDND.ITEMAMT1 + cSDND.ITEMAMT2;

            cSTRANM.TRTAX1 = cSDND.TAXAMT1;
            cSTRANM.TRTAX2 = cSDND.TAXAMT2;
            cSTRANM.TRTAX = cSDND.TAXAMT1 + cSDND.TAXAMT2;

            cSTRANM.TRAMT1 = cSDND.NETAMT1;
            cSTRANM.TRAMT2 = cSDND.NETAMT2;
            cSTRANM.TRAMT = cSDND.NETAMT1 + cSDND.NETAMT2;


            cSTRANM.TRSIGN = "DB";

            cSTRANM.TRSITEM1 = cSDND.ITEMAMT1;
            cSTRANM.TRSITEM2 = cSDND.ITEMAMT2;
            cSTRANM.TRSITEM = cSDND.ITEMAMT1 + cSDND.ITEMAMT2;

            cSTRANM.TRSTAX1 = cSDND.TAXAMT1;
            cSTRANM.TRSTAX2 = cSDND.TAXAMT2;
            cSTRANM.TRSTAX = cSDND.TAXAMT1 + cSDND.TAXAMT2;

            cSTRANM.TRSAMT1 = cSDND.NETAMT1;
            cSTRANM.TRSAMT2 = cSDND.NETAMT2;
            cSTRANM.TRSAMT = cSDND.NETAMT1 + cSDND.NETAMT2;

            cSTRANM.TRITEMOS1 = cSDND.ITEMAMT1;
            cSTRANM.TRITEMOS2 = cSDND.ITEMAMT2;
            cSTRANM.TRITEMOS = cSDND.ITEMAMT1 + cSDND.ITEMAMT2;


            cSTRANM.TRTAXOS1 = cSDND.TAXAMT1;
            cSTRANM.TRTAXOS2 = cSDND.TAXAMT2;
            cSTRANM.TRTAXOS = cSDND.TAXAMT1 + cSDND.TAXAMT2;


            cSTRANM.TROS1 = cSDND.NETAMT1;
            cSTRANM.TROS2 = cSDND.ITEMAMT2;
            cSTRANM.TROS = cSDND.NETAMT1 + cSDND.NETAMT2;


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
            cSTRANM.SEQNO = cSDNM.SEQNO;
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
