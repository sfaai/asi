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
    public class CSCASEFEEsController : Controller
    {
        private ASIDBConnection db = new ASIDBConnection();

        // GET: CSCASEFEEs
        public ActionResult Index()
        {
            var cSCASEFEEs = db.CSCASEFEEs.Include(c => c.CSITEM);
            return View(cSCASEFEEs.ToList());
        }

        // GET: CSCASEFEEs
        public PartialViewResult IndexByCase(string id)
        {
            string sid = MyHtmlHelpers.ConvertByteStrToId(id);
            ViewBag.Parent = sid;
            var cSCASEFEEs = db.CSCASEFEEs.Where(m => m.CASECODE == sid).Include(c => c.CSITEM);
            return PartialView(cSCASEFEEs.ToList());
        }

        // GET: CSCASEFEEs/Details/5
        public ActionResult Details(string id, int seq)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            string sid = MyHtmlHelpers.ConvertByteStrToId(id);
            CSCASEFEE cSCASEFEE = db.CSCASEFEEs.Find(sid, seq);
            if (cSCASEFEE == null)
            {
                return HttpNotFound();

            }
            ViewBag.Parent = sid;
            ViewBag.ITEMTYPE = new SelectList(db.CSITEMs, "ITEMTYPE", "ITEMDESC", cSCASEFEE.ITEMTYPE);
            return View(cSCASEFEE);
        }

        // GET: CSCASEFEEs/Create
        public ActionResult Create(string id)
        {
            ViewBag.Parent = MyHtmlHelpers.ConvertByteStrToId(id);

            string sid = ViewBag.Parent;
            CSCASEFEE lastRec = db.CSCASEFEEs.Where(m => m.CASECODE == sid).OrderByDescending(n => n.FEEID).FirstOrDefault();

            CSCASEFEE curRec = new CSCASEFEE();

            CSPARAM paramRec = db.CSPARAMs.FirstOrDefault();

            if (paramRec != null)
            {
                curRec.TAXRATE = paramRec.TAXRATE;
            }
            if (lastRec != null)
            {
                curRec.CASECODE = sid;
                curRec.FEEID = lastRec.FEEID + 1;
            }
            else
            {
                curRec.CASECODE = sid;
                curRec.FEEID = 1; 
            };
            curRec.STAMP = 1;

            ViewBag.ITEMTYPE = new SelectList(db.CSITEMs, "ITEMTYPE", "ITEMDESC");
            return View(curRec);
        }

        // POST: CSCASEFEEs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CASECODE,FEEID,ITEMTYPE,ITEMDESC,ITEMSPEC,TAXRATE,ITEMAMT1,TAXAMT1,NETAMT1,ITEMAMT2,TAXAMT2,NETAMT2,ITEMAMT,TAXAMT,NETAMT,STAMP")] CSCASEFEE cSCASEFEE)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.CSCASEFEEs.Add(cSCASEFEE);
                    db.SaveChanges();
                    return new RedirectResult(Url.Action("Details", "CSCASEs", new { id = MyHtmlHelpers.ConvertIdToByteStr(cSCASEFEE.CASECODE) }) + "#CASEFEE");
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

            ViewBag.ITEMTYPE = new SelectList(db.CSITEMs, "ITEMTYPE", "ITEMDESC", cSCASEFEE.ITEMTYPE);
            return View(cSCASEFEE);
        }

        // GET: CSCASEFEEs/Edit/5
        public ActionResult Edit(string id, int seq)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.Parent = MyHtmlHelpers.ConvertByteStrToId(id);
            CSCASEFEE cSCASEFEE = db.CSCASEFEEs.Find(ViewBag.Parent, seq);
            if (cSCASEFEE == null)
            {
                return HttpNotFound();
            }
            ViewBag.ITEMTYPE = new SelectList(db.CSITEMs, "ITEMTYPE", "ITEMDESC", cSCASEFEE.ITEMTYPE);
            return View(cSCASEFEE);
        }

        // POST: CSCASEFEEs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CASECODE,FEEID,ITEMTYPE,ITEMDESC,ITEMSPEC,TAXRATE,ITEMAMT1,TAXAMT1,NETAMT1,ITEMAMT2,TAXAMT2,NETAMT2,ITEMAMT,TAXAMT,NETAMT,STAMP")] CSCASEFEE cSCASEFEE)
        {
            if (ModelState.IsValid)
            {
                ASIDBConnection newdb = new ASIDBConnection();
                try
                {
                    CSCASEFEE curRec = newdb.CSCASEFEEs.Find(cSCASEFEE.CASECODE, cSCASEFEE.FEEID);
                    if (curRec.STAMP == cSCASEFEE.STAMP)
                    {
                        cSCASEFEE.STAMP = cSCASEFEE.STAMP + 1;
                        db.Entry(cSCASEFEE).State = EntityState.Modified;
                        db.SaveChanges();
                        //return RedirectToAction("Index");
                        return new RedirectResult(Url.Action("Details", "CSCASEs", new { id = MyHtmlHelpers.ConvertIdToByteStr(cSCASEFEE.CASECODE) }) + "#CASEFEE");
                    }
                    else { ModelState.AddModelError(string.Empty, "Record is modified"); }

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
            ViewBag.ITEMTYPE = new SelectList(db.CSITEMs, "ITEMTYPE", "ITEMDESC", cSCASEFEE.ITEMTYPE);
            return View(cSCASEFEE);
        }


        // GET: CSCASEFEEs/Delete/5
        public ActionResult Delete(string id, int seq)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSCASEFEE cSCASEFEE = db.CSCASEFEEs.Find(MyHtmlHelpers.ConvertByteStrToId(id), seq);
            if (cSCASEFEE == null)
            {
                return HttpNotFound();
            }
            ViewBag.ITEMTYPE = new SelectList(db.CSITEMs, "ITEMTYPE", "ITEMDESC", cSCASEFEE.ITEMTYPE);
            return View(cSCASEFEE);
        }

        // POST: CSCASEFEEs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id, int seq)
        {
            try
            {
                CSCASEFEE cSCASEFEE = db.CSCASEFEEs.Find(MyHtmlHelpers.ConvertByteStrToId(id), seq);
                db.CSCASEFEEs.Remove(cSCASEFEE);
                db.SaveChanges();
                return new RedirectResult(Url.Action("Details", "CSCASEs", new { id = MyHtmlHelpers.ConvertIdToByteStr(cSCASEFEE.CASECODE) }) + "#CASEFEE");
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
