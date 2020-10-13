using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity.Core;
using System.Data.Entity.Validation;
using System.Data.Entity.Infrastructure;
using WebApplication1.Utility;
using FirebirdSql.Data.Client;
using FirebirdSql.Data.FirebirdClient;

namespace WebApplication1.Controllers
{
    [Authorize(Roles = "Administrator,CS-A/C,CS-SEC,CS-AS")]
    public class CSJOBSTsController : Controller
    {
        private ASIDBConnection db = new ASIDBConnection();

        // GET: CSJOBSTs
        public ActionResult Index()
        {
            return View(db.CSJOBSTs.ToList());
        }

        // GET: CSJOBSTs/Details/5
        public ActionResult Details(string id, int row, DateTime indate, string intime)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSJOBST cSJOBST = db.CSJOBSTs.Find(MyHtmlHelpers.ConvertByteStrToId(id), row, indate, intime);
            if (cSJOBST == null)
            {
                return HttpNotFound();
            }
            ViewBag.SENDMODE = GetSendModeList();
            ViewBag.STAGETO = new SelectList(db.CSSTGs.OrderBy(x => x.STAGESEQ), "STAGE", "STAGE");
            ViewBag.Title = "View Job Case Status ";
            return View("Edit", cSJOBST);
        }

        public List<SelectListItem> GetSendModeList()
        {
            List<SelectListItem> listItems = new List<SelectListItem>();
            listItems.Add(new SelectListItem
            {
                Text = "By E-Mail",
                Value = "By E-Mail"
        });

            listItems.Add(new SelectListItem
            {
                Text = "By Fax",
                Value = "By Fax"
           
            });

            listItems.Add(new SelectListItem
            {
                Text = "By Hand",
                Value = "By Hand"
                
            });

            listItems.Add(new SelectListItem
            {
                Text = "By Mail",
                Value = "By Mail"

            });
            listItems.Add(new SelectListItem
            {
                Text = "Courier",
                Value = "Courier"

            });
            listItems.Add(new SelectListItem
            {
                Text = "Office Boy",
                Value = "Office Boy"

            });
            listItems.Add(new SelectListItem
            {
                Text = "Own Collection",
                Value = "Own Collection"

            });

            return listItems;
        }

        // GET: CSJOBSTs/Create
        public ActionResult Create(string id, int row)
        {
            string sid = MyHtmlHelpers.ConvertByteStrToId(id);

            CSJOBST lastRec = db.CSJOBSTs.Where(x => x.JOBNO == sid && x.CASENO == row).OrderByDescending(x => x.INIDX).FirstOrDefault();

            CSJOBST cSJOBST = new CSJOBST();
            cSJOBST.JOBNO = sid;
            cSJOBST.CASENO = row;
            if (lastRec == null)
            {
                cSJOBST.STAGEFR = "New";
              
            } else
            {
                cSJOBST.STAGEFR = lastRec.STAGETO;
            }
            cSJOBST.INDATE = DateTime.Today;
            cSJOBST.INTIME = DateTime.Now.ToString("HH:mm:ss");
            cSJOBST.STAMP = 0;

            ViewBag.SENDMODE = GetSendModeList();
            ViewBag.STAGETO = new SelectList(db.CSSTGs.OrderBy(x => x.STAGESEQ), "STAGE", "STAGE");
            ViewBag.Title = "Create Job Case Status ";
            return View("Edit", cSJOBST);
        }

        // POST: CSJOBSTs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "JOBNO,CASENO,STAGEFR,STAGETO,INDATE,INTIME,OUTDATE,SENDMODE,REM,INIDX,STAMP")] CSJOBST cSJOBST)
        {
            if (ModelState.IsValid)
            {
                CSJOBD cSJOBD = db.CSJOBDs.Find(cSJOBST.JOBNO, cSJOBST.CASENO);
                cSJOBD.STAGE = cSJOBST.STAGETO;
                cSJOBD.STAGEDATE = cSJOBST.INDATE;
                cSJOBD.STAGETIME = cSJOBST.INTIME;
                cSJOBD.STAMP = cSJOBD.STAMP + 1;

                CSJOBST lastRec = db.CSJOBSTs.Where(x => x.JOBNO == cSJOBST.JOBNO && x.CASENO == cSJOBST.CASENO).OrderByDescending(x => x.INIDX).FirstOrDefault();
                if (lastRec == null)
                {
                    cSJOBST.STAGEFR = "New";
                } else
                {
                    cSJOBST.STAGEFR = lastRec.STAGETO;
                    lastRec.OUTDATE = cSJOBST.INDATE;
                }
              
                cSJOBST.STAMP = 0;
                cSJOBST.INIDX = cSJOBST.INDATE.ToString("yyyy/MM/dd") + " " + cSJOBST.INTIME;

                try
                {
                    db.CSJOBSTs.Add(cSJOBST);
                    db.Entry(cSJOBD).State = EntityState.Modified;
                    if (lastRec != null)
                    {
                        db.Entry(lastRec).State = EntityState.Modified;
                    }

                    db.SaveChanges();
                    return RedirectToAction("Edit", "CSJOBDs", new { id = MyHtmlHelpers.ConvertIdToByteStr(cSJOBST.JOBNO), row = cSJOBST.CASENO, rel = 1 });
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
            ViewBag.STAGETO = new SelectList(db.CSSTGs.OrderBy(x => x.STAGESEQ), "STAGE", "STAGE");
            ViewBag.Title = "Edit Job Case Status ";
            return View("Edit", cSJOBST);
        }

        // GET: CSJOBSTs/Edit/5
        public ActionResult Edit(string id, int row, DateTime indate, string intime)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSJOBST cSJOBST = db.CSJOBSTs.Find(MyHtmlHelpers.ConvertByteStrToId(id), row, indate, intime);
            if (cSJOBST == null)
            {
                return HttpNotFound();
            }
            ViewBag.SENDMODE = GetSendModeList();
            ViewBag.STAGETO = new SelectList(db.CSSTGs.OrderBy(x => x.STAGESEQ), "STAGE", "STAGE");
            ViewBag.Title = "Edit Job Case Status ";
            return View("Edit", cSJOBST);
        }

        // POST: CSJOBSTs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "JOBNO,CASENO,STAGEFR,STAGETO,INDATE,INTIME,OUTDATE,SENDMODE,REM,INIDX,STAMP")] CSJOBST cSJOBST)
        {
            CSJOBST lastRec = db.CSJOBSTs.Where(x => x.JOBNO == cSJOBST.JOBNO && x.CASENO == cSJOBST.CASENO).OrderByDescending(x => x.INIDX).FirstOrDefault();
            if (lastRec.INIDX != cSJOBST.INIDX) { ModelState.AddModelError(String.Empty, "Only the latest record can be modified"); }
            if (ModelState.IsValid)
            {

                try
                {
                    //Remove original record --> the way the index is designed make it necessary to remove the record and recreate
                    CSJOBST recDel = db.CSJOBSTs.Find(cSJOBST.JOBNO, cSJOBST.CASENO, cSJOBST.INDATE, cSJOBST.INTIME);
                    db.CSJOBSTs.Remove(recDel);

                    cSJOBST.STAMP = cSJOBST.STAMP + 1;
                    db.CSJOBSTs.Add(cSJOBST);

                    CSJOBD cSJOBD = db.CSJOBDs.Find(cSJOBST.JOBNO, cSJOBST.CASENO);
                    cSJOBD.STAGE = cSJOBST.STAGETO;
                    cSJOBD.STAGEDATE = cSJOBST.INDATE;
                    cSJOBD.STAGETIME = cSJOBST.INTIME;
                    cSJOBD.STAMP = cSJOBD.STAMP + 1;

                    db.Entry(cSJOBD).State = EntityState.Modified;
                 
                    db.SaveChanges();
                    return RedirectToAction("Edit", "CSJOBDs", new { id = MyHtmlHelpers.ConvertIdToByteStr(cSJOBST.JOBNO), row = cSJOBST.CASENO , rel=1});
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
                catch (Exception e) { ModelState.AddModelError(string.Empty, e.Message); }
            }
            ViewBag.SENDMODE = GetSendModeList();
            ViewBag.STAGETO = new SelectList(db.CSSTGs.OrderBy(x => x.STAGESEQ), "STAGE", "STAGE");
            ViewBag.Title = "Edit Job Case Status ";
            return View("Edit", cSJOBST);
        }

        // GET: CSJOBSTs/Delete/5
        public ActionResult Delete(string id, int row, DateTime indate, string intime)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSJOBST cSJOBST = db.CSJOBSTs.Find(MyHtmlHelpers.ConvertByteStrToId(id), row, indate, intime);
            if (cSJOBST == null)
            {
                return HttpNotFound();
            }
            ViewBag.SENDMODE = GetSendModeList();
            ViewBag.STAGETO = new SelectList(db.CSSTGs.OrderBy(x => x.STAGESEQ), "STAGE", "STAGE");
            ViewBag.Title = "Delete Job Case Status ";
            return View("Edit", cSJOBST);
        }

        // POST: CSJOBSTs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id, int row, DateTime indate, string intime)
        {
            CSJOBST cSJOBST = db.CSJOBSTs.Find(MyHtmlHelpers.ConvertByteStrToId(id), row, indate, intime);
            var histRec = db.CSJOBSTs.Where(x => x.JOBNO == cSJOBST.JOBNO && x.CASENO == cSJOBST.CASENO).OrderByDescending(x => x.INIDX);

            if (histRec.Count() == 1) { ModelState.AddModelError(String.Empty, "Cannot Remove system generated record"); }


            CSJOBST lastRec = db.CSJOBSTs.Where(x => x.JOBNO == cSJOBST.JOBNO && x.CASENO == cSJOBST.CASENO).OrderByDescending(x => x.INIDX).FirstOrDefault();
            if (lastRec.INIDX != cSJOBST.INIDX) { ModelState.AddModelError(String.Empty, "Only the latest record can be modified"); }
            if (ModelState.IsValid)
            {try
                {
                    db.CSJOBSTs.Remove(cSJOBST);

                    // Change CSJOBD to previous state before delete

                    foreach (CSJOBST item in histRec)
                    {
                        if (item.INIDX != lastRec.INIDX) { lastRec = item; break; } // get the previous record before last record
                    }
                    CSJOBD cSJOBD = db.CSJOBDs.Find(cSJOBST.JOBNO, cSJOBST.CASENO);
                    cSJOBD.STAGE = lastRec.STAGETO;
                    cSJOBD.STAGEDATE = lastRec.INDATE;
                    cSJOBD.STAGETIME = lastRec.INTIME;
                    cSJOBD.STAMP = cSJOBD.STAMP + 1;

                    db.SaveChanges();
                    return RedirectToAction("Edit", "CSJOBDs", new { id = MyHtmlHelpers.ConvertIdToByteStr(cSJOBST.JOBNO), row = cSJOBST.CASENO, rel = 1 });
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

            ViewBag.SENDMODE = GetSendModeList();
            ViewBag.STAGETO = new SelectList(db.CSSTGs.OrderBy(x => x.STAGESEQ), "STAGE", "STAGE");
            ViewBag.Title = "Delete Job Case Status ";
            return View("Edit", cSJOBST);
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
