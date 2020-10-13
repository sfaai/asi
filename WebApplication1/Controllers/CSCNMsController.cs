using PagedList;
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
    [Authorize(Roles = "Administrator,CS-A/C,CS-SEC,CS-AS")]
    public class CSCNMsController : Controller
    {
        private ASIDBConnection db = new ASIDBConnection();

        public PartialViewResult Search()
        {
            CSCNM searchRec = null;
            //searchRec.CONO = pSearchCode;
            //searchRec.CONAME = pSearchName;
            if (Session["SearchCNMRec"] != null)
            {
                searchRec = (CSCNM)Session["SearchCNMRec"];

            }
            else
            {
                searchRec = new CSCNM();
                searchRec.VDATE = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                searchRec.DUEDATE = searchRec.VDATE.AddMonths(1);
                searchRec.DUEDATE = searchRec.DUEDATE.AddDays(-1);
            }
            if (Session["SearchCNMSort"] == null)
            {
                Session["SearchCNMSort"] = "TRNO";
            };

            List<SelectListItem> listItems = new List<SelectListItem>();
            listItems.Add(new SelectListItem
            {
                Text = "Company Name",
                Value = "CONAME",
                Selected = (string)Session["SearchCNMSort"] == "CONAME"
            });

            listItems.Add(new SelectListItem
            {
                Text = "Ref #",
                Value = "TRNO",
                Selected = (string)Session["SearchCNMSort"] == "TRNO"
            });

            listItems.Add(new SelectListItem
            {
                Text = "Ref # Latest",
                Value = "TRNOLAST",
                Selected = (string)Session["SearchCNMSort"] == "TRNOLAST"
            });

            listItems.Add(new SelectListItem
            {
                Text = "Date",
                Value = "VDATE",
                Selected = (string)Session["SearchCNMSort"] == "VDATE"
            });

            listItems.Add(new SelectListItem
            {
                Text = "Date Latest",
                Value = "VDATELAST",
                Selected = (string)Session["SearchDNMSort"] == "VDATELAST"
            });

            listItems.Add(new SelectListItem
            {
                Text = "Archive",
                Value = "ARCHIVE",
                Selected = (string)Session["SearchCNMSort"] == "ARCHIVE"
            });
            ViewBag.SORTBY = listItems;
            return PartialView("Partial/Search", searchRec);
        }


        [HttpGet]
        public ActionResult SearchPost()
        {
            return Index(1);
        }

        [HttpPost]
        public ActionResult SearchPost(CSCNM cSCNM)
        {

            Session["SearchCNMRec"] = cSCNM;
            Session["SearchCNMSort"] = Request.Params["SORTBY"] ?? "VDATE";
            return Redirect("?page=1");
            //return Index(1);
        }
        // GET: CSCNMs
        public ActionResult Index(int? page)
        {
            ViewBag.page = page ?? 1;
            return View("Index", CurrentSelection().ToList().ToPagedList(page ?? 1, 30));
        }

        public IQueryable<CSCNM> CurrentSelection()
        {
            string pSearchCode = "";
            string pSearchName = "";
            string pSearchCNM = "";
            DateTime pSearchVdate = DateTime.Parse("01/01/0001");
            DateTime pSearchDdate = DateTime.Parse("01/01/0001");

            if (Session["SearchCNMRec"] != null)
            {
                CSCNM searchRec = (CSCNM)(Session["SearchCNMRec"]);
                pSearchCode = searchRec.CSCOMSTR.COREGNO ?? "";
                pSearchName = searchRec.CSCOMSTR.CONAME ?? "";
                pSearchVdate = searchRec.VDATE;
                pSearchDdate = searchRec.DUEDATE;
                pSearchCNM = searchRec.TRNO ?? "";

            }
            else
            { // start with current month
                pSearchVdate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                pSearchDdate = pSearchVdate.AddMonths(1);
                pSearchDdate = pSearchDdate.AddDays(-1);
            }

            IQueryable<CSCNM> cSCNMs = db.CSCNMs;
            if ((string)Session["SearchCNMSort"] != "ARCHIVE") { cSCNMs = db.CSCNMs.Where(x => x.POST == "N"); }
            else { cSCNMs = db.CSCNMs.Where(x => x.POST == "Y"); }

            if (pSearchCode != "") { cSCNMs = cSCNMs.Where(x => x.CSCOMSTR.COREGNO.Contains(pSearchCode.ToUpper())); };
            if (pSearchName != "") { cSCNMs = cSCNMs.Where(x => x.CSCOMSTR.CONAME.Contains(pSearchName.ToUpper())); };
            if (pSearchVdate != DateTime.Parse("01/01/0001")) { cSCNMs = cSCNMs.Where(x => x.VDATE >= pSearchVdate); };
            if (pSearchDdate != DateTime.Parse("01/01/0001")) { cSCNMs = cSCNMs.Where(x => x.VDATE <= pSearchDdate); };
            if (pSearchCNM != "")
            {
                if (pSearchCNM.Length > 8)
                {
                    cSCNMs = cSCNMs.Where(x => x.TRNO == pSearchCNM);
                }
                else
                {
                    cSCNMs = cSCNMs.Where(x => x.TRNO.Contains(pSearchCNM));
                }
            };

            if (Session["RPT_START"] == null) { Session["RPT_START"] = pSearchVdate; }
            if (Session["RPT_END"] == null) { Session["RPT_END"] = pSearchDdate; }

            ViewBag.RPT_START = Session["RPT_START"];
            ViewBag.RPT_END = Session["RPT_END"];

            cSCNMs = cSCNMs.Include(e => e.CSCOMSTR).Include(f => f.CSCOADDR).Include(g => g.CSCNDs);

            if ((string)Session["SearchCNMSort"] == "CONAME")
            {
                cSCNMs = cSCNMs.OrderBy(n => n.CSCOMSTR.CONAME);
            }
            else if ((string)Session["SearchCNMSort"] == "VDATE")
            {
                cSCNMs = cSCNMs.OrderBy(n => n.VDATE);

            }
            else if ((string)Session["SearchDNMSort"] == "VDATELAST")
            {
                cSCNMs = cSCNMs.OrderByDescending(n => n.VDATE);

            }
            else if ((string)Session["SearchCNMSort"] == "TRNOLAST")
            {
                cSCNMs = cSCNMs.OrderByDescending(n => n.TRNO);

            }
            else
            {
                cSCNMs = cSCNMs.OrderBy(n => n.TRNO);
            }
            return cSCNMs;
        }

        // GET: CSCNMs/Details/5
        public ActionResult Details(string id, int? page)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSCNM cSCNM = db.CSCNMs.Find(MyHtmlHelpers.ConvertByteStrToId(id));
            if (cSCNM == null)
            {
                return HttpNotFound();
            }

            ViewBag.page = page ?? 1;
            Session["CSCNMPage"] = ViewBag.page;

            ViewBag.CSCND = db.CSCNDs.Where(x => x.TRNO == cSCNM.TRNO);
            ViewBag.CONAME = db.CSCOMSTRs.Where(x => x.CONO == cSCNM.CONO).Select(y => y.CONAME).FirstOrDefault();
            ViewBag.CONO = new SelectList(db.CSCOMSTRs.Select(x => new { CONO = x.CONO, CONAME = x.CONAME + "  (" + x.CONO + ")" }).OrderBy(y => y.CONAME), "CONO", "CONAME", cSCNM.CONO);
            ViewBag.ATTNDESC = new SelectList(db.CSCOPICs.Where(x => x.CONO == cSCNM.CONO).Select(x => new { PRSNAME = x.CSPR.PRSNAME, PRSDESC = x.CSPR.PRSNAME }).OrderBy(y => y.PRSDESC), "PRSNAME", "PRSDESC", cSCNM.ATTN);
            ViewBag.COADDR = new SelectList(db.CSCOADDRs.Where(x => x.CONO == cSCNM.CONO).Select(x => new { COADDR = x.CONO + "|" + x.ADDRID, COADDRDESC = x.ADDRTYPE + " | " + x.MAILADDR + " | " + x.ADDR1 + " " + x.ADDR2 + " " + x.ADDR3 }), "COADDR", "COADDRDESC", cSCNM.COADDR);

            ViewBag.Title = "View Discounted Bill";

            return View("Edit", cSCNM);
        }

        // GET: CSCNMs/Create
        public ActionResult Create()
        {
            CSCNM cSCNM = new CSCNM();
            cSCNM.VDATE = DateTime.Today;
            cSCNM.POST = "N";
            cSCNM.CONO = "750059-M"; // hack to show first company jobs


            ViewBag.CSCND = db.CSCNDs.Where(x => x.TRNO == cSCNM.TRNO);
            ViewBag.CONAME = db.CSCOMSTRs.Where(x => x.CONO == cSCNM.CONO).Select(y => y.CONAME).FirstOrDefault();
            ViewBag.CONO = new SelectList(db.CSCOMSTRs.Select(x => new { CONO = x.CONO, CONAME = x.CONAME + "  (" + x.CONO + ")" }).OrderBy(y => y.CONAME), "CONO", "CONAME", cSCNM.CONO);
            ViewBag.ATTNDESC = new SelectList(db.CSCOPICs.Where(x => x.CONO == cSCNM.CONO).Select(x => new { PRSNAME = x.CSPR.PRSNAME, PRSDESC = x.CSPR.PRSNAME }).OrderBy(y => y.PRSDESC), "PRSNAME", "PRSDESC", cSCNM.ATTN);
            ViewBag.COADDR = new SelectList(db.CSCOADDRs.Where(x => x.CONO == cSCNM.CONO).Select(x => new { COADDR = x.CONO + "|" + x.ADDRID, COADDRDESC = x.ADDRTYPE + " | " + x.MAILADDR + " | " + x.ADDR1 + " " + x.ADDR2 + " " + x.ADDR3 }), "COADDR", "COADDRDESC", cSCNM.COADDR);

            ViewBag.Title = "Create Discounted Bill";
            return View("Edit", cSCNM);
        }

        // POST: CSCNMs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TRNO,VDATE,CONO,COADDRID,ATTN,REM,SEQNO,POSTBool,STAMP")] CSCNM cSCNM)
        {
            if (ModelState.IsValid)
            {

                SALASTNO serialTbl = db.SALASTNOes.Find("CSCN");
                if (serialTbl != null)
                {

                    CSCOMSTR cSCOMSTR = db.CSCOMSTRs.Find(cSCNM.CONO);
                    if (cSCOMSTR != null)
                    {
                        try
                        {
                            string prefix = serialTbl.LASTPFIX;
                            int MaxNo = serialTbl.LASTNOMAX;
                            bool AutoGen = serialTbl.AUTOGEN == "Y";
                            if (AutoGen)
                            {
                                serialTbl.LASTNO = serialTbl.LASTNO + 1;
                                cSCNM.TRNO = serialTbl.LASTNO.ToString("D10");
                                serialTbl.STAMP = serialTbl.STAMP + 1;
                                db.Entry(serialTbl).State = EntityState.Modified;
                            }
                            cSCNM.STAMP = 1;

                            // increment company seqno count before using it in transaction
                            cSCOMSTR.SEQNO = cSCOMSTR.SEQNO + 1;
                            cSCOMSTR.STAMP = cSCOMSTR.STAMP + 1;

                            cSCNM.SEQNO = cSCOMSTR.SEQNO;
                            db.Entry(cSCOMSTR).State = EntityState.Modified;

                            db.CSCNMs.Add(cSCNM);
                            db.SaveChanges();

                            //return Edit(MyHtmlHelpers.ConvertIdToByteStr(cSCNM.TRNO), 1);
                            return RedirectToAction("Edit", new { id = MyHtmlHelpers.ConvertIdToByteStr(cSCNM.TRNO), page = 1 });
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
                            db.CSCNMs.Remove(cSCNM);
                            ModelState.AddModelError(string.Empty, e.Message);
                        }
                        finally
                        {

                        }
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Unable to find company #");
                    }

                }
            }


            ViewBag.CSCND = db.CSCNDs.Where(x => x.TRNO == cSCNM.TRNO);
            ViewBag.CONAME = db.CSCOMSTRs.Where(x => x.CONO == cSCNM.CONO).Select(y => y.CONAME).FirstOrDefault();
            ViewBag.CONO = new SelectList(db.CSCOMSTRs.Select(x => new { CONO = x.CONO, CONAME = x.CONAME + "  (" + x.CONO + ")" }).OrderBy(y => y.CONAME), "CONO", "CONAME", cSCNM.CONO);
            ViewBag.ATTNDESC = new SelectList(db.CSCOPICs.Where(x => x.CONO == cSCNM.CONO).Select(x => new { PRSNAME = x.CSPR.PRSNAME, PRSDESC = x.CSPR.PRSNAME }).OrderBy(y => y.PRSDESC), "PRSNAME", "PRSDESC", cSCNM.ATTN);
            ViewBag.COADDR = new SelectList(db.CSCOADDRs.Where(x => x.CONO == cSCNM.CONO).Select(x => new { COADDR = x.CONO + "|" + x.ADDRID, COADDRDESC = x.ADDRTYPE + " | " + x.MAILADDR + " | " + x.ADDR1 + " " + x.ADDR2 + " " + x.ADDR3 }), "COADDR", "COADDRDESC", cSCNM.COADDR);

            ViewBag.Title = "Create Discounted Bill";
            return View("Edit", cSCNM);
        }

        // GET: CSCNMs/Edit/5
        public ActionResult Edit(string id, int? page)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSCNM cSCNM = db.CSCNMs.Find(MyHtmlHelpers.ConvertByteStrToId(id));
            if (cSCNM == null)
            {
                return HttpNotFound();
            }
            ViewBag.page = page ?? 1;
            Session["CSCNMPage"] = ViewBag.page;

            ViewBag.CSCND = db.CSCNDs.Where(x => x.TRNO == cSCNM.TRNO);
            ViewBag.CONAME = db.CSCOMSTRs.Where(x => x.CONO == cSCNM.CONO).Select(y => y.CONAME).FirstOrDefault();
            ViewBag.CONO = new SelectList(db.CSCOMSTRs.Select(x => new { CONO = x.CONO, CONAME = x.CONAME + "  (" + x.CONO + ")" }).OrderBy(y => y.CONAME), "CONO", "CONAME", cSCNM.CONO);
            ViewBag.ATTNDESC = new SelectList(db.CSCOPICs.Where(x => x.CONO == cSCNM.CONO).Select(x => new { PRSNAME = x.CSPR.PRSNAME, PRSDESC = x.CSPR.PRSNAME }).OrderBy(y => y.PRSDESC), "PRSNAME", "PRSDESC", cSCNM.ATTN);
            ViewBag.COADDR = new SelectList(db.CSCOADDRs.Where(x => x.CONO == cSCNM.CONO).Select(x => new { COADDR = x.CONO + "|" + x.ADDRID, COADDRDESC = x.ADDRTYPE + " | " + x.MAILADDR + " | " + x.ADDR1 + " " + x.ADDR2 + " " + x.ADDR3 }), "COADDR", "COADDRDESC", cSCNM.COADDR);

            ViewBag.Title = "Edit Discounted Bill";
            return View("Edit", cSCNM);
        }

        public ActionResult EditCompany(CSCNM cSCNM)
        {

            //ModelState.Clear(); //apparent this has side-effects and using Remove is preferrable
            ModelState.Remove("CONO");
            ModelState.Remove("ATTN");
            ModelState.Remove("COADDR");

            cSCNM.COADDR = null;
            cSCNM.ATTN = null;

            ViewBag.CSCND = db.CSCNDs.Where(x => x.TRNO == cSCNM.TRNO);
            ViewBag.CONAME = db.CSCOMSTRs.Where(x => x.CONO == cSCNM.CONO).Select(y => y.CONAME).FirstOrDefault();
            ViewBag.ATTNDESC = new SelectList(db.CSCOPICs.Where(x => x.CONO == cSCNM.CONO).Select(x => new { PRSNAME = x.CSPR.PRSNAME, PRSDESC = x.CSPR.PRSNAME }).OrderBy(y => y.PRSDESC), "PRSNAME", "PRSDESC", cSCNM.ATTN);
            ViewBag.COADDR = new SelectList(db.CSCOADDRs.Where(x => x.CONO == cSCNM.CONO).Select(x => new { COADDR = x.CONO + "|" + x.ADDRID, COADDRDESC = x.ADDRTYPE + " | " + x.MAILADDR + " | " + x.ADDR1 + " " + x.ADDR2 + " " + x.ADDR3 }), "COADDR", "COADDRDESC", cSCNM.COADDR);
            return PartialView("Partial/EditCompany", cSCNM);


        }

        // POST: CSCNMs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TRNO,VDATE,CONO,COADDRID,ATTN,REM,SEQNO,POSTBool,STAMP")] CSCNM cSCNM)
        {
            if (ModelState.IsValid)
            {
                ASIDBConnection newdb = new ASIDBConnection();

                try
                {

                    CSCNM curRec = newdb.CSCNMs.Find(cSCNM.TRNO);
                    if (curRec.STAMP == cSCNM.STAMP)
                    {
                        cSCNM.STAMP = cSCNM.STAMP + 1;
                        db.Entry(cSCNM).State = EntityState.Modified;

                        db.SaveChanges();

                        int page = (int)Session["CSCNMPage"];
                        return RedirectToAction("Index", new { page = page });
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

            ViewBag.CSCND = db.CSCNDs.Where(x => x.TRNO == cSCNM.TRNO);
            ViewBag.CONAME = db.CSCOMSTRs.Where(x => x.CONO == cSCNM.CONO).Select(y => y.CONAME).FirstOrDefault();
            ViewBag.CONO = new SelectList(db.CSCOMSTRs.Select(x => new { CONO = x.CONO, CONAME = x.CONAME + "  (" + x.CONO + ")" }).OrderBy(y => y.CONAME), "CONO", "CONAME", cSCNM.CONO);
            ViewBag.ATTNDESC = new SelectList(db.CSCOPICs.Where(x => x.CONO == cSCNM.CONO).Select(x => new { PRSNAME = x.CSPR.PRSNAME, PRSDESC = x.CSPR.PRSNAME }).OrderBy(y => y.PRSDESC), "PRSNAME", "PRSDESC", cSCNM.ATTN);
            ViewBag.COADDR = new SelectList(db.CSCOADDRs.Where(x => x.CONO == cSCNM.CONO).Select(x => new { COADDR = x.CONO + "|" + x.ADDRID, COADDRDESC = x.ADDRTYPE + " | " + x.MAILADDR + " | " + x.ADDR1 + " " + x.ADDR2 + " " + x.ADDR3 }), "COADDR", "COADDRDESC", cSCNM.COADDR);

            ViewBag.Title = "Edit Discounted Bill";
            return View("Edit", cSCNM);
        }

        // GET: CSCNMs/Delete/5
        public ActionResult Delete(string id, int? page)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSCNM cSCNM = db.CSCNMs.Find(MyHtmlHelpers.ConvertByteStrToId(id));
            if (cSCNM == null)
            {
                return HttpNotFound();
            }
            int refcnt = cSCNM.CSCNDs.Sum(x => x.refcnt);
            if (refcnt > 0)
            {
                ModelState.AddModelError(string.Empty, refcnt.ToString() + " Details has been touched. Cannot Delete record");

            }
            ViewBag.page = page ?? 1;
            Session["CSCNMPage"] = ViewBag.page;

            ViewBag.CSCND = db.CSCNDs.Where(x => x.TRNO == cSCNM.TRNO);
            ViewBag.CONAME = db.CSCOMSTRs.Where(x => x.CONO == cSCNM.CONO).Select(y => y.CONAME).FirstOrDefault();
            ViewBag.CONO = new SelectList(db.CSCOMSTRs.Select(x => new { CONO = x.CONO, CONAME = x.CONAME + "  (" + x.CONO + ")" }).OrderBy(y => y.CONAME), "CONO", "CONAME", cSCNM.CONO);
            ViewBag.ATTNDESC = new SelectList(db.CSCOPICs.Where(x => x.CONO == cSCNM.CONO).Select(x => new { PRSNAME = x.CSPR.PRSNAME, PRSDESC = x.CSPR.PRSNAME }).OrderBy(y => y.PRSDESC), "PRSNAME", "PRSDESC", cSCNM.ATTN);
            ViewBag.COADDR = new SelectList(db.CSCOADDRs.Where(x => x.CONO == cSCNM.CONO).Select(x => new { COADDR = x.CONO + "|" + x.ADDRID, COADDRDESC = x.ADDRTYPE + " | " + x.MAILADDR + " | " + x.ADDR1 + " " + x.ADDR2 + " " + x.ADDR3 }), "COADDR", "COADDRDESC", cSCNM.COADDR);

            ViewBag.Title = "Delete Discounted Bill";
            return View("Edit", cSCNM);
        }

        // POST: CSCNMs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            CSCNM cSCNM = db.CSCNMs.Find(MyHtmlHelpers.ConvertByteStrToId(id));

            if (cSCNM != null)
            {
                List<CSCND> cSCNDList = cSCNM.CSCNDs.ToList();

                int refcnt = cSCNM.CSCNDs.Sum(x => x.refcnt);
                if (refcnt > 0)
                {
                    ModelState.AddModelError(string.Empty, refcnt.ToString() + " Details has been touched. Cannot Delete record");

                }
                else
                {

                    CSCNDsController detc = new CSCNDsController();


                    try
                    {
                        foreach (CSCND rec in cSCNDList)
                        {
                            detc.DeleteRow(db, id, rec.TRID);
                        }
                        db.CSCNMs.Remove(cSCNM);
                        db.SaveChanges();

                        int page = (int)Session["CSCNMPage"];
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
                    finally
                    {
                        detc.Dispose();
                    }
                }

            }

            ViewBag.page = Session["CSCNMPage"];

            ViewBag.CSCND = db.CSCNDs.Where(x => x.TRNO == cSCNM.TRNO);
            ViewBag.CONAME = db.CSCOMSTRs.Where(x => x.CONO == cSCNM.CONO).Select(y => y.CONAME).FirstOrDefault();
            ViewBag.CONO = new SelectList(db.CSCOMSTRs.Select(x => new { CONO = x.CONO, CONAME = x.CONAME + "  (" + x.CONO + ")" }).OrderBy(y => y.CONAME), "CONO", "CONAME", cSCNM.CONO);
            ViewBag.ATTNDESC = new SelectList(db.CSCOPICs.Where(x => x.CONO == cSCNM.CONO).Select(x => new { PRSNAME = x.CSPR.PRSNAME, PRSDESC = x.CSPR.PRSNAME }).OrderBy(y => y.PRSDESC), "PRSNAME", "PRSDESC", cSCNM.ATTN);
            ViewBag.COADDR = new SelectList(db.CSCOADDRs.Where(x => x.CONO == cSCNM.CONO).Select(x => new { COADDR = x.CONO + "|" + x.ADDRID, COADDRDESC = x.ADDRTYPE + " | " + x.MAILADDR + " | " + x.ADDR1 + " " + x.ADDR2 + " " + x.ADDR3 }), "COADDR", "COADDRDESC", cSCNM.COADDR);

            ViewBag.Title = "Delete Discounted Bill";
            return View("Edit", cSCNM);
        }

        public ActionResult CNotelist(int? page)
        {
            if (page > 0)
            {
                int prevPage = (page ?? 1) - 1;
                return View(CurrentSelection().Skip(prevPage * 30).Take(30).ToList());
            }
            return View(CurrentSelection().ToList());
        }

        public ActionResult CNote(string id)
        {
            string sid = MyHtmlHelpers.ConvertByteStrToId(id);
            return View(getCNote(sid));
        }

        public PartialViewResult CNoteM(string id)
        {
            string sid = MyHtmlHelpers.ConvertByteStrToId(id);
            return PartialView("CNote", getCNote(sid));
        }

        public CSCNM getCNote(string sid)
        {
            CSCNM cSCNM = db.CSCNMs.Find(sid);

            SACOMP profileRec = db.SACOMPs.FirstOrDefault();
            ViewBag.Company = profileRec.CONAME.ToUpper() + " (" + profileRec.COREGNO + ") ";
            ViewBag.Address = profileRec.COADDR1 + " " + profileRec.COADDR2 + " " + profileRec.COADDR3 + " " + profileRec.COADDR4;
            ViewBag.Contact = "Tel: " + profileRec.COPHONE1 + " Fax: " + profileRec.COFAX1 + " E-Mail: " + profileRec.COWEB;

            ViewBag.Addr1 = "";
            ViewBag.Addr2 = "";
            if ((cSCNM.CSCOADDR != null) && (cSCNM.CSCOADDR.HKCITY != null))
            {
                ViewBag.Addr1 = cSCNM.CSCOADDR.ADDR1;
                ViewBag.Addr2 = cSCNM.CSCOADDR.ADDR2;
                ViewBag.Addr3 = cSCNM.CSCOADDR.ADDR3 + " " + cSCNM.CSCOADDR.HKCITY.CITYDESC + " " + cSCNM.CSCOADDR.POSTAL;
            }
            else
            {
                if (cSCNM.CSCOADDR != null)
                {
                    ViewBag.Addr1 = cSCNM.CSCOADDR.ADDR1;
                    ViewBag.Addr2 = cSCNM.CSCOADDR.ADDR2;
                    ViewBag.Addr3 = cSCNM.CSCOADDR.ADDR3 + " " + cSCNM.CSCOADDR.POSTAL;
                }
                else
                {
                    ViewBag.Addr3 = "";
                }
            }
            if ((cSCNM.ATTN == null) || (cSCNM.ATTN == string.Empty)) { cSCNM.ATTN = "The Board of Directors"; }

            ViewBag.StaffName = cSCNM.CSCOMSTR.HKSTAFF.STAFFDESC;
            return cSCNM;
        }


        public ActionResult Listing()
        {

            DateTime rptStart;
            DateTime.TryParse(Session["RPT_START"].ToString(), out rptStart);

            DateTime rptEnd;
            DateTime.TryParse(Session["RPT_END"].ToString(), out rptEnd);
            ViewBag.RPT_START = rptStart.ToString("dd/MM/yyyy");
            ViewBag.RPT_END = rptEnd.ToString("dd/MM/yyyy");

            SACOMP profileRec = db.SACOMPs.FirstOrDefault();
            ViewBag.Company = profileRec.CONAME.ToUpper() + " (" + profileRec.COREGNO + ") ";

            return View(CurrentSelection().Where(x => x.VDATE >= rptStart && x.VDATE <= rptEnd));

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
