using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PagedList;
using WebApplication1.Utility;
using System.Data.Entity.Core;
using System.Data.Entity.Validation;
using System.Data.Entity.Infrastructure;
using FirebirdSql.Data.Client;
using FirebirdSql.Data.FirebirdClient;

namespace WebApplication1.Controllers
{
    [Authorize(Roles = "Administrator,CS-SEC,CS-A/C,CS-AS")]
    public class CSPRsController : Controller
    {
        private ASIDBConnection db = new ASIDBConnection();

        public PartialViewResult Search()
        {
            CSPR searchRec = null;
            //searchRec.CONO = pSearchCode;
            //searchRec.CONAME = pSearchName;
            if (Session["SearchPRSRec"] != null)
            {
                searchRec = (CSPR)Session["SearchPRSRec"];
            }
            else
            {
                searchRec = new CSPR();
                searchRec.DOB = new DateTime(1, 1, 1);

            }
            if (Session["SearchPRSSort"] == null)
            {
                Session["SearchPRSSort"] = "PRSNAME";
            };

            List<SelectListItem> listItems = new List<SelectListItem>();
            listItems.Add(new SelectListItem
            {
                Text = "Entity Name",
                Value = "PRSNAME",
                Selected = (string)Session["SearchPRSSort"] == "PRSNAME"
            });

            listItems.Add(new SelectListItem
            {
                Text = "Constitution #",
                Value = "CONSTCODE",
                Selected = (string)Session["SearchPRSSort"] == "CONSTCODE"
            });

            listItems.Add(new SelectListItem
            {
                Text = "Nation",
                Value = "NATION",
                Selected = (string)Session["SearchPRSSort"] == "NATION"
            });

            listItems.Add(new SelectListItem
            {
                Text = "Date",
                Value = "DOB",
                Selected = (string)Session["SearchPRSSort"] == "DOB"
            });


            ViewBag.SORTBY = listItems;
            ViewBag.CONSTCODE = new SelectList(db.HKCONSTs.OrderBy(x => x.CONSTCODE).Select(x => new { CONSTCODE = x.CONSTCODE, CONSTDESC = x.CONSTCODE + " | " + x.CONSTDESC + " | " + x.CONSTTYPE }), "CONSTCODE", "CONSTDESC");
            ViewBag.NATION = new SelectList(db.HKNATIONs, "NATION", "NATION");
            return PartialView("Partial/Search", searchRec);
        }


        [HttpGet]
        public ActionResult SearchPost()
        {
            return Index(1);
        }

        [HttpPost]
        public ActionResult SearchPost(CSPR cSPRS)
        {

            Session["SearchPRSRec"] = cSPRS;
            Session["SearchPRSSort"] = Request.Params["SORTBY"] ?? "ENTDATE";
            return Index(1);
        }

        // GET: CSPRs
        public IQueryable<CSPR> CurrentSelection()
        {
            string pSearchConstcode = "";
            string pSearchName = "";
            string pSearchNation = "";

            DateTime pSearchVdate = DateTime.Parse("01/01/0001");


            if (Session["SearchPRSRec"] != null)
            {
                CSPR searchRec = (CSPR)(Session["SearchPRSRec"]);
                pSearchConstcode = searchRec.CONSTCODE ?? "";
                pSearchName = searchRec.PRSNAME ?? "";
                pSearchNation = searchRec.NATION ?? "";
                pSearchVdate = searchRec.DOB ?? new DateTime(1, 1, 1);

            }
            else
            { // start with current month proforma PRSs instead of entire list
                pSearchVdate = new DateTime(1, 1, 1);
                ;
            }

            IQueryable<CSPR> cSPRSs = db.CSPRS;

            if (pSearchName != "") { cSPRSs = cSPRSs.Where(x => x.PRSNAME.ToUpper().Contains(pSearchName.ToUpper())); };
            if (pSearchNation != "") { cSPRSs = cSPRSs.Where(x => x.NATION == pSearchNation); };
            if (pSearchConstcode != "") { cSPRSs = cSPRSs.Where(x => x.CONSTCODE == pSearchConstcode); };
            if (pSearchVdate != DateTime.Parse("01/01/0001")) { cSPRSs = cSPRSs.Where(x => x.DOB >= pSearchVdate); };



            cSPRSs = cSPRSs.Include(c => c.HKCONST).Include(c => c.HKTITLE).Include(c => c.HKNATION).Include(c => c.HKRACE).Include(c => c.HKCTRY);





            if ((string)Session["SearchPRSSort"] == "PRSNAME")
            {
                cSPRSs = cSPRSs.OrderBy(n => n.PRSNAME);
            }
            else if ((string)Session["SearchPRSSort"] == "DOB")
            {
                cSPRSs = cSPRSs.OrderBy(n => n.DOB);

            }
            else if ((string)Session["SearchPRSSort"] == "NATION")
            {
                cSPRSs = cSPRSs.OrderByDescending(n => n.NATION);

            }
            else if ((string)Session["SearchPRSSort"] == "CONSTCODE")
            {
                cSPRSs = cSPRSs.OrderBy(n => n.CONSTCODE);

            }
            else
            {
                cSPRSs = cSPRSs.OrderBy(n => n.PRSNAME);
            }
            return cSPRSs;

        }

        public ActionResult Index(int? page)
        {
            ViewBag.page = page ?? 1;
            return View("Index", CurrentSelection().ToList().ToPagedList(page ?? 1, 30));
        }

        public ActionResult Listing()
        {
            SACOMP profileRec = db.SACOMPs.FirstOrDefault();
            ViewBag.Company = profileRec.CONAME.ToUpper() + " (" + profileRec.COREGNO + ") ";

            return View(CurrentSelection());
        }


        // GET: CSPRs/Details/5
        public ActionResult Details(string id, int? page)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSPR cSPR = db.CSPRS.Find(MyHtmlHelpers.ConvertByteStrToId(id));
            if (cSPR == null)
            {
                return HttpNotFound();
            }
            ViewBag.SEX = cSPR.gender;
            ViewBag.CONSTCODE = new SelectList(db.HKCONSTs.OrderBy(x => x.CONSTCODE).Select(x => new { CONSTCODE = x.CONSTCODE, CONSTDESC = x.CONSTCODE + " | " + x.CONSTDESC + " | " + x.CONSTTYPE }), "CONSTCODE", "CONSTDESC", cSPR.CONSTCODE);
            ViewBag.PRSTITLE = new SelectList(db.HKTITLEs, "TITLE", "TITLE", cSPR.PRSTITLE);
            ViewBag.NATION = new SelectList(db.HKNATIONs, "NATION", "NATION", cSPR.NATION);
            ViewBag.RACE = new SelectList(db.HKRACEs, "RACE", "RACE", cSPR.RACE);
            ViewBag.CTRYINC = new SelectList(db.HKCTRies, "CTRYCODE", "CTRYDESC", cSPR.CTRYINC);
            ViewBag.Title = "View Entity ";
            Session["CSPRSPage"] = page ?? 1;
            ViewBag.page = page ?? 1;
            return View("Edit", cSPR);
        }

        // GET: CSPRs/Create
        public ActionResult Create()
        {
            CSPR cSPR = new CSPR();

            ViewBag.SEX = cSPR.gender;
            ViewBag.CONSTCODE = new SelectList(db.HKCONSTs.OrderBy(x => x.CONSTCODE).Select(x => new { CONSTCODE = x.CONSTCODE, CONSTDESC = x.CONSTCODE + " | " + x.CONSTDESC + " | " + x.CONSTTYPE }), "CONSTCODE", "CONSTDESC");
            ViewBag.PRSTITLE = new SelectList(db.HKTITLEs, "TITLE", "TITLE");
            ViewBag.NATION = new SelectList(db.HKNATIONs, "NATION", "NATION");
            ViewBag.RACE = new SelectList(db.HKRACEs, "RACE", "RACE");
            ViewBag.CTRYINC = new SelectList(db.HKCTRies, "CTRYCODE", "CTRYDESC");
            ViewBag.Title = "Create Entity ";
            Session["CSPRSPage"] = 1;
            ViewBag.page = 1;
            return View("Edit", cSPR);
        }

        // POST: CSPRs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PRSCODE,CONSTCODE,PRSNAME,PRSTITLE,NATION,RACE,SEX,DOB,CTRYINC,MOBILE1,MOBILE2,EMAIL,OCCUPATION,REM,STAMP")] CSPR cSPR)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    cSPR.STAMP = 0;

                    SALASTNO serialTbl = db.SALASTNOes.Find("CSPRS");
                    if (serialTbl != null)
                    {
                        try
                        {
                            string prefix = serialTbl.LASTPFIX;
                            int MaxNo = serialTbl.LASTNOMAX;
                            bool AutoGen = serialTbl.AUTOGEN == "Y";
                            serialTbl.LASTNO = serialTbl.LASTNO + 1;
                            cSPR.PRSCODE = serialTbl.LASTNO.ToString("D10");


                            serialTbl.STAMP = serialTbl.STAMP + 1;
                            db.Entry(serialTbl).State = EntityState.Modified;

                            db.CSPRS.Add(cSPR);
                            db.SaveChanges();
                            int page = (int)Session["CSPRSPage"];
                            return RedirectToAction("Edit", new { id = MyHtmlHelpers.ConvertIdToByteStr(cSPR.PRSCODE), page = page });
                        }
                        catch (Exception e)
                        {
                            ModelState.AddModelError(string.Empty, e.Message);
                        }


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
            }
            ViewBag.SEX = cSPR.gender;
            ViewBag.CONSTCODE = new SelectList(db.HKCONSTs.OrderBy(x => x.CONSTCODE).Select(x => new { CONSTCODE = x.CONSTCODE, CONSTDESC = x.CONSTCODE + " | " + x.CONSTDESC + " | " + x.CONSTTYPE }), "CONSTCODE", "CONSTDESC", cSPR.CONSTCODE);
            ViewBag.PRSTITLE = new SelectList(db.HKTITLEs, "TITLE", "TITLE", cSPR.PRSTITLE);
            ViewBag.NATION = new SelectList(db.HKNATIONs, "NATION", "NATION", cSPR.NATION);
            ViewBag.RACE = new SelectList(db.HKRACEs, "RACE", "RACE", cSPR.RACE);
            ViewBag.CTRYINC = new SelectList(db.HKCTRies, "CTRYCODE", "CTRYDESC", cSPR.CTRYINC);
            ViewBag.Title = "Create Entity ";
            ViewBag.page = (int)Session["CSPRSPage"];
            return View("Edit", cSPR);
        }

        // GET: CSPRs/Edit/5
        public ActionResult Edit(string id, int? page)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSPR cSPR = db.CSPRS.Find(MyHtmlHelpers.ConvertByteStrToId(id));
            if (cSPR == null)
            {
                return HttpNotFound();
            }
            ViewBag.SEX = cSPR.gender;
            ViewBag.CONSTCODE = new SelectList(db.HKCONSTs.OrderBy(x => x.CONSTCODE).Select(x => new { CONSTCODE = x.CONSTCODE, CONSTDESC = x.CONSTCODE + " | " + x.CONSTDESC + " | " + x.CONSTTYPE }), "CONSTCODE", "CONSTDESC", cSPR.CONSTCODE);
            ViewBag.PRSTITLE = new SelectList(db.HKTITLEs, "TITLE", "TITLE", cSPR.PRSTITLE);
            ViewBag.NATION = new SelectList(db.HKNATIONs, "NATION", "NATION", cSPR.NATION);
            ViewBag.RACE = new SelectList(db.HKRACEs, "RACE", "RACE", cSPR.RACE);
            ViewBag.CTRYINC = new SelectList(db.HKCTRies, "CTRYCODE", "CTRYDESC", cSPR.CTRYINC);
            ViewBag.Title = "Edit Entity ";
            Session["CSPRSPage"] = page ?? 1;
            ViewBag.page = page ?? 1;
            return View("Edit", cSPR);
        }

        // POST: CSPRs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PRSCODE,CONSTCODE,PRSNAME,PRSTITLE,NATION,RACE,SEX,DOB,CTRYINC,MOBILE1,MOBILE2,EMAIL,OCCUPATION,REM,STAMP")] CSPR cSPR)
        {
            int page = (int)Session["CSPRSPage"];
            if (ModelState.IsValid)
            {
                try
                {
                    cSPR.STAMP = cSPR.STAMP + 1;
                    db.Entry(cSPR).State = EntityState.Modified;
                    db.SaveChanges();

                    ViewBag.page = page;
                    return RedirectToAction("Index", new { page = page });
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
            ViewBag.SEX = cSPR.gender;
            ViewBag.CONSTCODE = new SelectList(db.HKCONSTs.OrderBy(x => x.CONSTCODE).Select(x => new { CONSTCODE = x.CONSTCODE, CONSTDESC = x.CONSTCODE + " | " + x.CONSTDESC + " | " + x.CONSTTYPE }), "CONSTCODE", "CONSTDESC", cSPR.CONSTCODE);
            ViewBag.PRSTITLE = new SelectList(db.HKTITLEs, "TITLE", "TITLE", cSPR.PRSTITLE);
            ViewBag.NATION = new SelectList(db.HKNATIONs, "NATION", "NATION", cSPR.NATION);
            ViewBag.RACE = new SelectList(db.HKRACEs, "RACE", "RACE", cSPR.RACE);
            ViewBag.CTRYINC = new SelectList(db.HKCTRies, "CTRYCODE", "CTRYDESC", cSPR.CTRYINC);
            ViewBag.Title = "Edit Entity ";
         
            ViewBag.page = page;
            return View("Edit", cSPR);
        }

        // GET: CSPRs/Delete/5
        public ActionResult Delete(string id, int? page)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSPR cSPR = db.CSPRS.Find(MyHtmlHelpers.ConvertByteStrToId(id));
            if (cSPR == null)
            {
                return HttpNotFound();
            }
            ViewBag.SEX = cSPR.gender;
            ViewBag.CONSTCODE = new SelectList(db.HKCONSTs.OrderBy(x => x.CONSTCODE).Select(x => new { CONSTCODE = x.CONSTCODE, CONSTDESC = x.CONSTCODE + " | " + x.CONSTDESC + " | " + x.CONSTTYPE }), "CONSTCODE", "CONSTDESC", cSPR.CONSTCODE);
            ViewBag.PRSTITLE = new SelectList(db.HKTITLEs, "TITLE", "TITLE", cSPR.PRSTITLE);
            ViewBag.NATION = new SelectList(db.HKNATIONs, "NATION", "NATION", cSPR.NATION);
            ViewBag.RACE = new SelectList(db.HKRACEs, "RACE", "RACE", cSPR.RACE);
            ViewBag.CTRYINC = new SelectList(db.HKCTRies, "CTRYCODE", "CTRYDESC", cSPR.CTRYINC);
            Session["CSPRSPage"] = page ?? 1;
            ViewBag.Title = "Delete Entity ";
            ViewBag.page = page ?? 1;
            return View("Edit", cSPR);
        }

        // POST: CSPRs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            int page = (int)Session["CSPRSPage"];
            CSPR cSPR = db.CSPRS.Find(MyHtmlHelpers.ConvertByteStrToId(id));
            try
            {
                db.CSPRS.Remove(cSPR);
                db.SaveChanges();
             
                return RedirectToAction("Index", new { page = page });
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
            ViewBag.SEX = cSPR.gender;
            ViewBag.CONSTCODE = new SelectList(db.HKCONSTs.OrderBy(x => x.CONSTCODE).Select(x => new { CONSTCODE = x.CONSTCODE, CONSTDESC = x.CONSTCODE + " | " + x.CONSTDESC + " | " + x.CONSTTYPE }), "CONSTCODE", "CONSTDESC", cSPR.CONSTCODE);
            ViewBag.PRSTITLE = new SelectList(db.HKTITLEs, "TITLE", "TITLE", cSPR.PRSTITLE);
            ViewBag.NATION = new SelectList(db.HKNATIONs, "NATION", "NATION", cSPR.NATION);
            ViewBag.RACE = new SelectList(db.HKRACEs, "RACE", "RACE", cSPR.RACE);
            ViewBag.CTRYINC = new SelectList(db.HKCTRies, "CTRYCODE", "CTRYDESC", cSPR.CTRYINC);
            ViewBag.Title = "Delete Entity ";
            ViewBag.page = page;
            return View("Edit", cSPR);
        }


        public PartialViewResult CSPRSREG(string id)
        {
            string sid = MyHtmlHelpers.ConvertByteStrToId(id);
            var cSPRSREGs = db.CSPRSREGs.Where(x => x.PRSCODE == sid ).OrderBy(y => y.REGNO);
            ViewBag.PRSCODE = sid;
            return PartialView("Partial/CSPRSREG", cSPRSREGs);
        }

        public PartialViewResult CSPRSADDR(string id)
        {
            string sid = MyHtmlHelpers.ConvertByteStrToId(id);
            var cSPRSADDRs = db.CSPRSADDRs.Where(x => x.PRSCODE == sid).OrderByDescending(y => y.ADDRID);
            ViewBag.PRSCODE = sid;
            return PartialView("Partial/CSPRSADDR", cSPRSADDRs);
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
