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
    public class CSDNMsController : Controller
    {
        private ASIDBConnection db = new ASIDBConnection();

        public PartialViewResult Search()
        {
            CSDNM searchRec = null;
            //searchRec.CONO = pSearchCode;
            //searchRec.CONAME = pSearchName;
            if (Session["SearchDNMRec"] != null)
            {
                searchRec = (CSDNM)Session["SearchDNMRec"];

            }
            else
            {
                searchRec = new CSDNM();
                searchRec.VDATE = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                searchRec.DUEDATE = searchRec.VDATE.AddMonths(1);
                searchRec.DUEDATE = searchRec.DUEDATE.AddDays(-1);
            }
            if (Session["SearchDNMSort"] == null)
            {
                Session["SearchDNMSort"] = "TRNO";
            };

            List<SelectListItem> listItems = new List<SelectListItem>();
            listItems.Add(new SelectListItem
            {
                Text = "Company Name",
                Value = "CONAME",
                Selected = (string)Session["SearchDNMSort"] == "CONAME"
            });

            listItems.Add(new SelectListItem
            {
                Text = "Ref #",
                Value = "TRNO",
                Selected = (string)Session["SearchDNMSort"] == "TRNO"
            });

            listItems.Add(new SelectListItem
            {
                Text = "Ref # Latest",
                Value = "TRNOLAST",
                Selected = (string)Session["SearchDNMSort"] == "TRNOLAST"
            });

            listItems.Add(new SelectListItem
            {
                Text = "Date",
                Value = "VDATE",
                Selected = (string)Session["SearchDNMSort"] == "VDATE"
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
                Selected = (string)Session["SearchDNMSort"] == "ARCHIVE"
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
        public ActionResult SearchPost(CSDNM cSDNM)
        {

            Session["SearchDNMRec"] = cSDNM;
            Session["SearchDNMSort"] = Request.Params["SORTBY"] ?? "VDATE";
            return Redirect("?page=1");
            //return Index(1);
        }
        // GET: CSDNMs
        public ActionResult Index(int? page)
        {
            ViewBag.page = page ?? 1;
            return View("Index", CurrentSelection().ToList().ToPagedList(page ?? 1, 30));
        }

        public IQueryable<CSDNM> CurrentSelection()
        {
            string pSearchCode = "";
            string pSearchName = "";
            string pSearchDNM = "";
            DateTime pSearchVdate = DateTime.Parse("01/01/0001");
            DateTime pSearchDdate = DateTime.Parse("01/01/0001");

            if (Session["SearchDNMRec"] != null)
            {
                CSDNM searchRec = (CSDNM)(Session["SearchDNMRec"]);
                pSearchCode = searchRec.CSCOMSTR.COREGNO ?? "";
                pSearchName = searchRec.CSCOMSTR.CONAME ?? "";
                pSearchVdate = searchRec.VDATE;
                pSearchDdate = searchRec.DUEDATE;
                pSearchDNM = searchRec.TRNO ?? "";

            }
            else
            { // start with current month
                pSearchVdate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                pSearchDdate = pSearchVdate.AddMonths(1);
                pSearchDdate = pSearchDdate.AddDays(-1);
            }

            IQueryable<CSDNM> cSDNMs = db.CSDNMs;
            if ((string)Session["SearchDNMSort"] != "ARCHIVE") { cSDNMs = db.CSDNMs.Where(x => x.POST == "N"); }
            else { cSDNMs = db.CSDNMs.Where(x => x.POST == "Y"); }

            if (pSearchCode != "") { cSDNMs = cSDNMs.Where(x => x.CSCOMSTR.COREGNO.Contains(pSearchCode.ToUpper())); };
            if (pSearchName != "") { cSDNMs = cSDNMs.Where(x => x.CSCOMSTR.CONAME.Contains(pSearchName.ToUpper())); };
            if (pSearchVdate != DateTime.Parse("01/01/0001")) { cSDNMs = cSDNMs.Where(x => x.VDATE >= pSearchVdate); };
            if (pSearchDdate != DateTime.Parse("01/01/0001")) { cSDNMs = cSDNMs.Where(x => x.VDATE <= pSearchDdate); };
            if (pSearchDNM != "")
            {
                if (pSearchDNM.Length > 8)
                {
                    cSDNMs = cSDNMs.Where(x => x.TRNO == pSearchDNM);
                }
                else
                {
                    cSDNMs = cSDNMs.Where(x => x.TRNO.Contains(pSearchDNM));
                }

            };
            if (Session["RPT_START"] == null) { Session["RPT_START"] = pSearchVdate; }
            if (Session["RPT_END"] == null) { Session["RPT_END"] = pSearchDdate; }

            DateTime rptStart;
            DateTime.TryParse(Session["RPT_START"].ToString(), out rptStart);

            DateTime rptEnd;
            DateTime.TryParse(Session["RPT_END"].ToString(), out rptEnd);
            ViewBag.RPT_START = rptStart;
            ViewBag.RPT_END = rptEnd;

            cSDNMs = cSDNMs.Include(e => e.CSCOMSTR).Include(f => f.CSCOADDR).Include(g => g.CSDNDs);

            if ((string)Session["SearchDNMSort"] == "CONAME")
            {
                cSDNMs = cSDNMs.OrderBy(n => n.CSCOMSTR.CONAME);
            }
            else if ((string)Session["SearchDNMSort"] == "VDATE")
            {
                cSDNMs = cSDNMs.OrderBy(n => n.VDATE);

            }
            else if ((string)Session["SearchDNMSort"] == "VDATELAST")
            {
                cSDNMs = cSDNMs.OrderByDescending(n => n.VDATE);

            }
            else if ((string)Session["SearchDNMSort"] == "TRNOLAST")
            {
                cSDNMs = cSDNMs.OrderByDescending(n => n.TRNO);

            }
            else
            {
                cSDNMs = cSDNMs.OrderBy(n => n.TRNO);
            }
            return cSDNMs;
        }

        // GET: CSDNMs/Details/5
        public ActionResult Details(string id, int? page)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSDNM cSDNM = db.CSDNMs.Find(MyHtmlHelpers.ConvertByteStrToId(id));
            if (cSDNM == null)
            {
                return HttpNotFound();
            }
            ViewBag.page = page ?? 1;
            Session["CSDNMPage"] = ViewBag.page;

            ViewBag.CSDND = db.CSDNDs.Where(x => x.TRNO == cSDNM.TRNO);
            ViewBag.CONAME = db.CSCOMSTRs.Where(x => x.CONO == cSDNM.CONO).Select(y => y.CONAME).FirstOrDefault();
            ViewBag.CONO = new SelectList(db.CSCOMSTRs.Select(x => new { CONO = x.CONO, CONAME = x.CONAME + "  (" + x.CONO + ")" }).OrderBy(y => y.CONAME), "CONO", "CONAME", cSDNM.CONO);
            ViewBag.ATTNDESC = new SelectList(db.CSCOPICs.Where(x => x.CONO == cSDNM.CONO).Select(x => new { PRSNAME = x.CSPR.PRSNAME, PRSDESC = x.CSPR.PRSNAME }).OrderBy(y => y.PRSDESC), "PRSNAME", "PRSDESC", cSDNM.ATTN);
            ViewBag.COADDR = new SelectList(db.CSCOADDRs.Where(x => x.CONO == cSDNM.CONO).Select(x => new { COADDR = x.CONO + "|" + x.ADDRID, COADDRDESC = x.ADDRTYPE + " | " + x.MAILADDR + " | " + x.ADDR1 + " " + x.ADDR2 + " " + x.ADDR3 }), "COADDR", "COADDRDESC", cSDNM.COADDR);

            ViewBag.Title = "View Debit Note";

            return View("Edit", cSDNM);
        }

        // GET: CSCNMs/Create
        public ActionResult Create()
        {
            CSDNM cSDNM = new CSDNM();
            cSDNM.VDATE = DateTime.Today;
            cSDNM.POST = "N";
            cSDNM.CONO = "750059-M"; // hack to show first company jobs


            ViewBag.CSDND = db.CSDNDs.Where(x => x.TRNO == cSDNM.TRNO);
            ViewBag.CONAME = db.CSCOMSTRs.Where(x => x.CONO == cSDNM.CONO).Select(y => y.CONAME).FirstOrDefault();
            ViewBag.CONO = new SelectList(db.CSCOMSTRs.Select(x => new { CONO = x.CONO, CONAME = x.CONAME + "  (" + x.CONO + ")" }).OrderBy(y => y.CONAME), "CONO", "CONAME", cSDNM.CONO);
            ViewBag.ATTNDESC = new SelectList(db.CSCOPICs.Where(x => x.CONO == cSDNM.CONO).Select(x => new { PRSNAME = x.CSPR.PRSNAME, PRSDESC = x.CSPR.PRSNAME }).OrderBy(y => y.PRSDESC), "PRSNAME", "PRSDESC", cSDNM.ATTN);
            ViewBag.COADDR = new SelectList(db.CSCOADDRs.Where(x => x.CONO == cSDNM.CONO).Select(x => new { COADDR = x.CONO + "|" + x.ADDRID, COADDRDESC = x.ADDRTYPE + " | " + x.MAILADDR + " | " + x.ADDR1 + " " + x.ADDR2 + " " + x.ADDR3 }), "COADDR", "COADDRDESC", cSDNM.COADDR);

            ViewBag.Title = "Create Debit Note";
            return View("Edit", cSDNM);
        }

        // POST: CSDNMs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TRNO,VDATE,CONO,COADDRID,ATTN,REM,SEQNO,POSTBool,STAMP")] CSDNM cSDNM)
        {
            if (ModelState.IsValid)
            {

                SALASTNO serialTbl = db.SALASTNOes.Find("CSDN");
                if (serialTbl != null)
                {

                    CSCOMSTR cSCOMSTR = db.CSCOMSTRs.Find(cSDNM.CONO);
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
                                cSDNM.TRNO = serialTbl.LASTNO.ToString("D10");
                                serialTbl.STAMP = serialTbl.STAMP + 1;
                                db.Entry(serialTbl).State = EntityState.Modified;
                            }
                            cSDNM.STAMP = 1;

                            // increment company seqno count before using it in transaction
                            cSCOMSTR.SEQNO = cSCOMSTR.SEQNO + 1;
                            cSCOMSTR.STAMP = cSCOMSTR.STAMP + 1;

                            cSDNM.SEQNO = cSCOMSTR.SEQNO;
                            db.Entry(cSCOMSTR).State = EntityState.Modified;

                            db.CSDNMs.Add(cSDNM);
                            db.SaveChanges();

                            //return Edit(MyHtmlHelpers.ConvertIdToByteStr(cSDNM.TRNO), 1);
                            return RedirectToAction("Edit", new { id = MyHtmlHelpers.ConvertIdToByteStr(cSDNM.TRNO), page = 1 });
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
                            db.CSDNMs.Remove(cSDNM);
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


            ViewBag.CSDND = db.CSDNDs.Where(x => x.TRNO == cSDNM.TRNO);
            ViewBag.CONAME = db.CSCOMSTRs.Where(x => x.CONO == cSDNM.CONO).Select(y => y.CONAME).FirstOrDefault();
            ViewBag.CONO = new SelectList(db.CSCOMSTRs.Select(x => new { CONO = x.CONO, CONAME = x.CONAME + "  (" + x.CONO + ")" }).OrderBy(y => y.CONAME), "CONO", "CONAME", cSDNM.CONO);
            ViewBag.ATTNDESC = new SelectList(db.CSCOPICs.Where(x => x.CONO == cSDNM.CONO).Select(x => new { PRSNAME = x.CSPR.PRSNAME, PRSDESC = x.CSPR.PRSNAME }).OrderBy(y => y.PRSDESC), "PRSNAME", "PRSDESC", cSDNM.ATTN);
            ViewBag.COADDR = new SelectList(db.CSCOADDRs.Where(x => x.CONO == cSDNM.CONO).Select(x => new { COADDR = x.CONO + "|" + x.ADDRID, COADDRDESC = x.ADDRTYPE + " | " + x.MAILADDR + " | " + x.ADDR1 + " " + x.ADDR2 + " " + x.ADDR3 }), "COADDR", "COADDRDESC", cSDNM.COADDR);

            ViewBag.Title = "Create Debit Note";
            return View("Edit", cSDNM);
        }

        // GET: CSDNMs/Edit/5
        public ActionResult Edit(string id, int? page)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSDNM cSDNM = db.CSDNMs.Find(MyHtmlHelpers.ConvertByteStrToId(id));
            if (cSDNM == null)
            {
                return HttpNotFound();
            }
            ViewBag.page = page ?? 1;
            Session["CSDNMPage"] = ViewBag.page;

            ViewBag.CSDND = db.CSDNDs.Where(x => x.TRNO == cSDNM.TRNO);
            ViewBag.CONAME = db.CSCOMSTRs.Where(x => x.CONO == cSDNM.CONO).Select(y => y.CONAME).FirstOrDefault();
            ViewBag.CONO = new SelectList(db.CSCOMSTRs.Select(x => new { CONO = x.CONO, CONAME = x.CONAME + "  (" + x.CONO + ")" }).OrderBy(y => y.CONAME), "CONO", "CONAME", cSDNM.CONO);
            ViewBag.ATTNDESC = new SelectList(db.CSCOPICs.Where(x => x.CONO == cSDNM.CONO).Select(x => new { PRSNAME = x.CSPR.PRSNAME, PRSDESC = x.CSPR.PRSNAME }).OrderBy(y => y.PRSDESC), "PRSNAME", "PRSDESC", cSDNM.ATTN);
            ViewBag.COADDR = new SelectList(db.CSCOADDRs.Where(x => x.CONO == cSDNM.CONO).Select(x => new { COADDR = x.CONO + "|" + x.ADDRID, COADDRDESC = x.ADDRTYPE + " | " + x.MAILADDR + " | " + x.ADDR1 + " " + x.ADDR2 + " " + x.ADDR3 }), "COADDR", "COADDRDESC", cSDNM.COADDR);

            ViewBag.Title = "Edit Debit Note";
            return View("Edit", cSDNM);
        }

        public ActionResult EditCompany(CSDNM cSDNM)
        {

            //ModelState.Clear(); //apparent this has side-effects and using Remove is preferrable
            ModelState.Remove("CONO");
            ModelState.Remove("ATTN");
            ModelState.Remove("COADDR");

            cSDNM.COADDR = null;
            cSDNM.ATTN = null;

            ViewBag.CSDND = db.CSDNDs.Where(x => x.TRNO == cSDNM.TRNO);
            ViewBag.CONAME = db.CSCOMSTRs.Where(x => x.CONO == cSDNM.CONO).Select(y => y.CONAME).FirstOrDefault();
            ViewBag.ATTNDESC = new SelectList(db.CSCOPICs.Where(x => x.CONO == cSDNM.CONO).Select(x => new { PRSNAME = x.CSPR.PRSNAME, PRSDESC = x.CSPR.PRSNAME }).OrderBy(y => y.PRSDESC), "PRSNAME", "PRSDESC", cSDNM.ATTN);
            ViewBag.COADDR = new SelectList(db.CSCOADDRs.Where(x => x.CONO == cSDNM.CONO).Select(x => new { COADDR = x.CONO + "|" + x.ADDRID, COADDRDESC = x.ADDRTYPE + " | " + x.MAILADDR + " | " + x.ADDR1 + " " + x.ADDR2 + " " + x.ADDR3 }), "COADDR", "COADDRDESC", cSDNM.COADDR);
            return PartialView("Partial/EditCompany", cSDNM);


        }

        // POST: CSCNMs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TRNO,VDATE,CONO,COADDRID,ATTN,REM,SEQNO,POSTBool,STAMP")] CSDNM cSDNM)
        {
            if (ModelState.IsValid)
            {
                ASIDBConnection newdb = new ASIDBConnection();
                db.Entry(cSDNM).State = EntityState.Modified;
                try
                {

                    CSDNM curRec = newdb.CSDNMs.Find(cSDNM.TRNO);
                    if (curRec.STAMP == cSDNM.STAMP)
                    {
                        cSDNM.STAMP = cSDNM.STAMP + 1;
                        db.SaveChanges();

                        int page = (int)Session["CSDNMPage"];
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
            ViewBag.CSDND = db.CSDNDs.Where(x => x.TRNO == cSDNM.TRNO);
            ViewBag.CONAME = db.CSCOMSTRs.Where(x => x.CONO == cSDNM.CONO).Select(y => y.CONAME).FirstOrDefault();
            ViewBag.CONO = new SelectList(db.CSCOMSTRs.Select(x => new { CONO = x.CONO, CONAME = x.CONAME + "  (" + x.CONO + ")" }).OrderBy(y => y.CONAME), "CONO", "CONAME", cSDNM.CONO);
            ViewBag.ATTNDESC = new SelectList(db.CSCOPICs.Where(x => x.CONO == cSDNM.CONO).Select(x => new { PRSNAME = x.CSPR.PRSNAME, PRSDESC = x.CSPR.PRSNAME }).OrderBy(y => y.PRSDESC), "PRSNAME", "PRSDESC", cSDNM.ATTN);
            ViewBag.COADDR = new SelectList(db.CSCOADDRs.Where(x => x.CONO == cSDNM.CONO).Select(x => new { COADDR = x.CONO + "|" + x.ADDRID, COADDRDESC = x.ADDRTYPE + " | " + x.MAILADDR + " | " + x.ADDR1 + " " + x.ADDR2 + " " + x.ADDR3 }), "COADDR", "COADDRDESC", cSDNM.COADDR);

            ViewBag.Title = "Edit Debit Note";
            return View("Edit", cSDNM);
        }

        // GET: CSDNMs/Delete/5
        public ActionResult Delete(string id, int? page)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSDNM cSDNM = db.CSDNMs.Find(MyHtmlHelpers.ConvertByteStrToId(id));
            if (cSDNM == null)
            {
                return HttpNotFound();
            }
            int refcnt = cSDNM.CSDNDs.Sum(x => x.refcnt);
            if (refcnt > 0)
            {
                ModelState.AddModelError(string.Empty, refcnt.ToString() + " Details has been touched. Cannot Delete record");

            }

            ViewBag.page = page ?? 1;
            Session["CSDNMPage"] = ViewBag.page;

            ViewBag.CSDND = db.CSDNDs.Where(x => x.TRNO == cSDNM.TRNO);
            ViewBag.CONAME = db.CSCOMSTRs.Where(x => x.CONO == cSDNM.CONO).Select(y => y.CONAME).FirstOrDefault();
            ViewBag.CONO = new SelectList(db.CSCOMSTRs.Select(x => new { CONO = x.CONO, CONAME = x.CONAME + "  (" + x.CONO + ")" }).OrderBy(y => y.CONAME), "CONO", "CONAME", cSDNM.CONO);
            ViewBag.ATTNDESC = new SelectList(db.CSCOPICs.Where(x => x.CONO == cSDNM.CONO).Select(x => new { PRSNAME = x.CSPR.PRSNAME, PRSDESC = x.CSPR.PRSNAME }).OrderBy(y => y.PRSDESC), "PRSNAME", "PRSDESC", cSDNM.ATTN);
            ViewBag.COADDR = new SelectList(db.CSCOADDRs.Where(x => x.CONO == cSDNM.CONO).Select(x => new { COADDR = x.CONO + "|" + x.ADDRID, COADDRDESC = x.ADDRTYPE + " | " + x.MAILADDR + " | " + x.ADDR1 + " " + x.ADDR2 + " " + x.ADDR3 }), "COADDR", "COADDRDESC", cSDNM.COADDR);

            ViewBag.Title = "Delete Debit Note";
            return View("Edit", cSDNM);
        }

        // POST: CSDNMs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            CSDNM cSDNM = db.CSDNMs.Find(MyHtmlHelpers.ConvertByteStrToId(id));

            if (cSDNM != null)
            {
                List<CSDND> cSDNDList = cSDNM.CSDNDs.ToList();

                int refcnt = cSDNM.CSDNDs.Sum(x => x.refcnt);
                if (refcnt > 0)
                {
                    ModelState.AddModelError(string.Empty, refcnt.ToString() + " Details has been touched. Cannot Delete record");

                }
                else
                {

                    CSDNDsController detc = new CSDNDsController();
                    try
                    {
                        foreach (CSDND rec in cSDNDList)
                        {
                            detc.DeleteRow(db, id, rec.TRID);
                        }
                        db.CSDNMs.Remove(cSDNM);
                        db.SaveChanges();

                        int page = (int)Session["CSDNMPage"];
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

            
            ViewBag.page = Session["CSDNMPage"];

            ViewBag.CSDND = db.CSDNDs.Where(x => x.TRNO == cSDNM.TRNO);
            ViewBag.CONAME = db.CSCOMSTRs.Where(x => x.CONO == cSDNM.CONO).Select(y => y.CONAME).FirstOrDefault();
            ViewBag.CONO = new SelectList(db.CSCOMSTRs.Select(x => new { CONO = x.CONO, CONAME = x.CONAME + "  (" + x.CONO + ")" }).OrderBy(y => y.CONAME), "CONO", "CONAME", cSDNM.CONO);
            ViewBag.ATTNDESC = new SelectList(db.CSCOPICs.Where(x => x.CONO == cSDNM.CONO).Select(x => new { PRSNAME = x.CSPR.PRSNAME, PRSDESC = x.CSPR.PRSNAME }).OrderBy(y => y.PRSDESC), "PRSNAME", "PRSDESC", cSDNM.ATTN);
            ViewBag.COADDR = new SelectList(db.CSCOADDRs.Where(x => x.CONO == cSDNM.CONO).Select(x => new { COADDR = x.CONO + "|" + x.ADDRID, COADDRDESC = x.ADDRTYPE + " | " + x.MAILADDR + " | " + x.ADDR1 + " " + x.ADDR2 + " " + x.ADDR3 }), "COADDR", "COADDRDESC", cSDNM.COADDR);

            ViewBag.Title = "Delete Debit Note";
            return View("Edit", cSDNM);

        }

        public ActionResult DNotelist(int? page)
        {
            if (page > 0)
            {
                int prevPage = (page ?? 1) - 1;
                return View(CurrentSelection().Skip(prevPage * 30).Take(30).ToList());
            }
            return View(CurrentSelection().ToList());
        }

        public ActionResult DNote(string id)
        {
            string sid = MyHtmlHelpers.ConvertByteStrToId(id);
            return View(getDNote(sid));
        }

        public PartialViewResult DNoteM(string id)
        {
            string sid = MyHtmlHelpers.ConvertByteStrToId(id);
            return PartialView("DNote", getDNote(sid));
        }

        public CSDNM getDNote(string sid)
        {
            CSDNM cSDNM = db.CSDNMs.Find(sid);

            SACOMP profileRec = db.SACOMPs.FirstOrDefault();
            ViewBag.Company = profileRec.CONAME.ToUpper() + " (" + profileRec.COREGNO + ") ";
            ViewBag.Address = profileRec.COADDR1 + " " + profileRec.COADDR2 + " " + profileRec.COADDR3 + " " + profileRec.COADDR4;
            ViewBag.Contact = "Tel: " + profileRec.COPHONE1 + " Fax: " + profileRec.COFAX1 + " E-Mail: " + profileRec.COWEB;

            ViewBag.Addr1 = "";
            ViewBag.Addr2 = "";
            if ((cSDNM.CSCOADDR != null) && (cSDNM.CSCOADDR.HKCITY != null))
            {
                ViewBag.Addr1 = cSDNM.CSCOADDR.ADDR1;
                ViewBag.Addr2 = cSDNM.CSCOADDR.ADDR2;
                ViewBag.Addr3 = cSDNM.CSCOADDR.ADDR3 + " " + cSDNM.CSCOADDR.HKCITY.CITYDESC + " " + cSDNM.CSCOADDR.POSTAL;
            }
            else
            {
                if (cSDNM.CSCOADDR != null)
                {
                    ViewBag.Addr1 = cSDNM.CSCOADDR.ADDR1;
                    ViewBag.Addr2 = cSDNM.CSCOADDR.ADDR2;
                    ViewBag.Addr3 = cSDNM.CSCOADDR.ADDR3 + " " + cSDNM.CSCOADDR.POSTAL;
                }
                else
                {
                    ViewBag.Addr3 = "";
                }
            }
            if ((cSDNM.ATTN == null) || (cSDNM.ATTN == string.Empty)) { cSDNM.ATTN = "The Board of Directors"; }

            ViewBag.StaffName = cSDNM.CSCOMSTR.HKSTAFF.STAFFDESC;
            return cSDNM;
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
