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
    [Authorize(Roles = "Administrator,CS-A/C,CS-SEC,CS-AS")]
    public class CSBILLsController : Controller
    {
        private ASIDBConnection db = new ASIDBConnection();

        private int BatchNo = 0;
        private int BatchSize = 10000;


        public PartialViewResult Search()
        {
            CSBILL searchRec = null;
            //searchRec.CONO = pSearchCode;
            //searchRec.CONAME = pSearchName;
            if (Session["SearchBillRec"] != null)
            {
                searchRec = (CSBILL)Session["SearchBillRec"];
            }
            else
            {
                searchRec = new CSBILL();
                searchRec.ENTDATE = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                searchRec.DUEDATE = searchRec.ENTDATE.AddMonths(1);
                searchRec.DUEDATE = searchRec.DUEDATE.AddDays(-1);
            }
            if (Session["SearchBillSort"] == null)
            {
                Session["SearchBillSort"] = "BILLNO";
            };

            List<SelectListItem> listItems = new List<SelectListItem>();
            listItems.Add(new SelectListItem
            {
                Text = "Company Name",
                Value = "CONAME",
                Selected = (string)Session["SearchBillSort"] == "CONAME"
            });

            listItems.Add(new SelectListItem
            {
                Text = "Item #",
                Value = "BILLNO",
                Selected = (string)Session["SearchBillSort"] == "BILLNO"
            });

            listItems.Add(new SelectListItem
            {
                Text = "Item # Latest",
                Value = "BILLNOLAST",
                Selected = (string)Session["SearchBillSort"] == "BILLNOLAST"
            });

            listItems.Add(new SelectListItem
            {
                Text = "Item Date",
                Value = "ENTDATE",
                Selected = (string)Session["SearchBillSort"] == "ENTDATE"
            });

            listItems.Add(new SelectListItem
            {
                Text = "Net Amt",
                Value = "NETAMT",
                Selected = (string)Session["SearchBillSort"] == "NETAMT"
            });


            listItems.Add(new SelectListItem
            {
                Text = "Net Amt High Value First",
                Value = "NETAMTLAST",
                Selected = (string)Session["SearchBillSort"] == "NETAMTLAST"
            });

            listItems.Add(new SelectListItem
            {
                Text = "Archive",
                Value = "ARCHIVE",
                Selected = (string)Session["SearchBillSort"] == "ARCHIVE"
            });

            ViewBag.SORTBY = listItems;
            ViewBag.CASECODE = new SelectList(db.CSCASEs.OrderBy(x => x.CASECODE), "CASECODE", "CASECODE");
            ViewBag.ITEMTYPE = new SelectList(db.CSITEMs, "ITEMTYPE", "ITEMDESC");
            return PartialView("Partial/Search", searchRec);
        }


        [HttpGet]
        public ActionResult SearchPost()
        {
            return Index(1);
        }

        [HttpPost]
        public ActionResult SearchPost(CSBILL cSBILL)
        {

            Session["SearchBillRec"] = cSBILL;
            Session["SearchBillSort"] = Request.Params["SORTBY"] ?? "ENTDATE";
            return Index(1);
        }
        // GET: CSBILLs
        public ActionResult Index(int? page)
        {
            ViewBag.page = page ?? 1;
            return View("Index", CurrentSelection().ToList().ToPagedList(page ?? 1, 30));
        }

        public IQueryable<CSBILL> CurrentSelection()
        {
            string pSearchCode = "";
            string pSearchName = "";
            string pSearchCase = "";
            string pSearchItem = "";
            string pSearchDesc = "";
            string pSearchBill = "";
            string pSearchJob = "";
            DateTime pSearchVdate = DateTime.Parse("01/01/0001");
            DateTime pSearchDdate = DateTime.Parse("01/01/0001");

            if (Session["SearchBillRec"] != null)
            {
                CSBILL searchRec = (CSBILL)(Session["SearchBillRec"]);
                pSearchCode = searchRec.CSCOMSTR.COREGNO ?? "";
                pSearchName = searchRec.CSCOMSTR.CONAME ?? "";
                pSearchCase = searchRec.CASECODE ?? "";
                pSearchItem = searchRec.ITEMTYPE ?? "";
                pSearchDesc = searchRec.ITEMDESC ?? "";
                pSearchBill = searchRec.BILLNO ?? "";
                pSearchJob = searchRec.JOBNO ?? "";
                pSearchVdate = searchRec.ENTDATE;
                pSearchDdate = searchRec.DUEDATE;
            }
            else
            { // start with current month proforma bills instead of entire list
                pSearchVdate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                pSearchDdate = pSearchVdate.AddMonths(1);
                pSearchDdate = pSearchDdate.AddDays(-1);
            }

            IQueryable<CSBILL> cSBILLs = db.CSBILLs;
            if ((string)Session["SearchBillSort"] != "ARCHIVE") { cSBILLs = cSBILLs.Where(x => x.PRFALLOC == "N"); }
            else { cSBILLs = cSBILLs.Where(x => x.PRFALLOC == "Y"); }

            if (pSearchCode != "") { cSBILLs = cSBILLs.Where(x => x.CSCOMSTR.COREGNO.Contains(pSearchCode.ToUpper()));}
            if (pSearchName != "") { cSBILLs = cSBILLs.Where(x => x.CSCOMSTR.CONAME.Contains(pSearchName.ToUpper())); };
            if (pSearchCase != "") { cSBILLs = cSBILLs.Where(x => x.CASECODE == pSearchCase); };
            if (pSearchItem != "") { cSBILLs = cSBILLs.Where(x => x.ITEMTYPE == pSearchItem); };
            if (pSearchDesc != "") { cSBILLs = cSBILLs.Where(x => x.ITEMDESC.Contains(pSearchDesc)); };
            if (pSearchVdate != DateTime.Parse("01/01/0001")) { cSBILLs = cSBILLs.Where(x => x.ENTDATE >= pSearchVdate); };
            if (pSearchDdate != DateTime.Parse("01/01/0001")) { cSBILLs = cSBILLs.Where(x => x.ENTDATE <= pSearchDdate); };

            if (pSearchBill != "")
            {
                if (pSearchBill.Length > 8)
                {
                    cSBILLs = cSBILLs.Where(x => x.BILLNO == pSearchBill);
                }
                else
                {
                    cSBILLs = cSBILLs.Where(x => x.BILLNO.Contains(pSearchBill));
                }
            };
            if (pSearchJob != "")
            {
                if (pSearchJob.Length > 8)
                {
                    cSBILLs = cSBILLs.Where(x => x.JOBNO == pSearchJob);
                }
                else
                {
                    cSBILLs = cSBILLs.Where(x => x.JOBNO.Contains(pSearchJob));
                }
            };

            cSBILLs = cSBILLs.Include(c => c.CSITEM).Include(d => d.CSCOMSTR).Include(e => e.CSPRF).Include(f => f.CSCASEs);

            ViewBag.BATCHLIST = new SelectList(
    new List<SelectListItem> {

    new SelectListItem { Text="First 10000 records", Value = "0"},
    new SelectListItem { Text=" 10001 - 20000", Value = "1"},
    new SelectListItem { Text=" 20001 - 30000", Value = "2"},
    new SelectListItem { Text=" 30001 - 40000", Value = "3"},
    new SelectListItem { Text=" 40001 - 50000", Value = "4"},
    new SelectListItem { Text=" 50001 - 60000", Value = "5"},
    new SelectListItem { Text=" 60001 - 70000", Value = "6"},
    new SelectListItem { Text=" 70001 - 80000", Value = "7"},
    new SelectListItem { Text=" 80001 - 90000", Value = "8"},
    new SelectListItem { Text=" 90001 - 100000", Value = "9"},
    new SelectListItem { Text="100001 - 110000", Value = "10"},
    new SelectListItem { Text="110001 - 120000", Value = "11"}
}, "Value", "Text", BatchNo);   // <<< Add size here


            if (Session["BATCH"] == null) { Session["BATCH"] = 0; }

            Session["RPT_START"] = pSearchVdate;
            Session["RPT_END"] = pSearchDdate;
            ViewBag.RPT_START = pSearchVdate.ToString("dd/MM/yyyy");
            ViewBag.RPT_END = pSearchDdate.ToString("dd/MM/yyyy");
            ViewBag.BATCH = Session["BATCH"];
            try
            {
                BatchNo = (int)Session["BATCH"];
            }
            catch { BatchNo = 0; }

            if ((string)Session["SearchBillSort"] == "CONAME")
            {
                cSBILLs = cSBILLs.OrderBy(n => n.CSCOMSTR.CONAME);
            }
            else if ((string)Session["SearchBillSort"] == "ENTDATE")
            {
                cSBILLs = cSBILLs.OrderBy(n => n.ENTDATE);

            }
            else if ((string)Session["SearchBillSort"] == "BILLNOLAST")
            {
                cSBILLs = cSBILLs.OrderByDescending(n => n.BILLNO);

            }
            else if ((string)Session["SearchBillSort"] == "NETAMT")
            {
                cSBILLs = cSBILLs.OrderBy(n => n.NETAMT);

            }
            else if ((string)Session["SearchBillSort"] == "NETAMTLAST")
            {
                cSBILLs = cSBILLs.OrderByDescending(n => n.NETAMT);

            }
            else
            {
                cSBILLs = cSBILLs.OrderBy(n => n.BILLNO);
            }
            return cSBILLs.Skip(BatchNo * BatchSize).Take(BatchSize);
        }

        // GET: CSBILLs/Details/5
        public ActionResult Details(string id, int? page)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSBILL cSBILL = db.CSBILLs.Find(MyHtmlHelpers.ConvertByteStrToId(id));
            if (cSBILL == null)
            {
                return HttpNotFound();
            }
            ViewBag.page = page ?? 1;
            Session["CSBillPage"] = ViewBag.page;

            ViewBag.Title = "Billing Details";
            return CallView("Edit", cSBILL);
        }

        // GET: CSBILLs/Create
        public ActionResult Create()
        {
            decimal taxrate = 0;
            string taxcode = "SSTN01";

            CSITEM cSITEM = db.CSITEMs.Find("Work");
            if (cSITEM != null)
            {
                taxrate = cSITEM.GSTRATE ?? 0;
            }

            CSBILL cSBILL = new CSBILL();
            cSBILL.ENTDATE = DateTime.Today;
            cSBILL.SYSGENBool = false;
            cSBILL.STAMP = 1;
            cSBILL.SYSGEN = "N";
            cSBILL.PRFALLOC = "N";
            //cSBILL.TAXRATE = db.CSPARAMs.Find("DEFAULT").TAXRATE;
            //cSBILL.CONO = "750059-M"; // hack to show first company jobs
            taxcode = db.CSTAXTYPEs.Where(x => x.TAXRATE == taxrate && x.EFFECTIVE_START <= cSBILL.ENTDATE && x.EFFECTIVE_END >= cSBILL.ENTDATE).Select(y => y.TAXCODE).FirstOrDefault() ?? taxcode;

            cSBILL.TAXRATE = taxrate;
            cSBILL.TAXCODE = taxcode;

            ViewBag.page = 1;
            Session["CSBillPage"] = ViewBag.page;

            ViewBag.Title = "Create Billing Item";
            return CallView("Edit", cSBILL);
        }

        // POST: CSBILLs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ENTDATE,CONO,JOBNO,SYSGENBool,PRFALLOCBool,CASENO,CASECODE,ITEMTYPE,ITEMDESC,ITEMSPEC,TAXCODE,TAXRATE,ITEMAMT1,TAXAMT1,NETAMT1,ITEMAMT2,TAXAMT2,NETAMT2,ITEMAMT,TAXAMT,NETAMT,SYSGEN,PRFALLOC,PRFNO,PRFID,STAMP")] CSBILL cSBILL)
        {
            if (ModelState.IsValid)
            {
                SALASTNO serialTbl = db.SALASTNOes.Find("CSBILL");
                if (serialTbl != null)
                {
                    try
                    {
                        string prefix = serialTbl.LASTPFIX;
                        int MaxNo = serialTbl.LASTNOMAX;
                        bool AutoGen = serialTbl.AUTOGEN == "Y";
                        serialTbl.LASTNO = serialTbl.LASTNO + 1;
                        cSBILL.BILLNO = serialTbl.LASTNO.ToString("D10");


                        serialTbl.STAMP = serialTbl.STAMP + 1;
                        db.Entry(serialTbl).State = EntityState.Modified;
                        db.CSBILLs.Add(cSBILL);
                        db.SaveChanges();

                        int page = (int)Session["CSBillPage"];
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
                    catch (Exception e)
                    {
                        ModelState.AddModelError(string.Empty, e.Message);
                    }
                    finally
                    {
                        View("Edit", cSBILL);
                    }

                }
            }

            return CallView("Edit", cSBILL);
        }


        public ActionResult EditCompanyJob(CSBILL cSBILL)
        {

            //ModelState.Clear(); //apparent this has side-effects and using Remove is preferrable
            ModelState.Remove("CONO");
            ModelState.Remove("JOBCASE");
            ModelState.Remove("JOBNO");
            ModelState.Remove("CASENO");
            ModelState.Remove("CASECODE");
            ModelState.Remove("ITEMDESC");
            ModelState.Remove("ITEMSPEC");
            ModelState.Remove("TAXRATE");
            cSBILL.JOBNO = null;
            cSBILL.CASENO = null;

            ViewBag.BILLDESC = new SelectList(db.CSBILLDESCs.Select(x => new { BILLDESC = x.BILLDESC, BILLSPEC = x.BILLDESC + " | " + x.BILLSPEC }).OrderBy(y => y.BILLDESC), "BILLDESC", "BILLDESC", cSBILL.ITEMDESC);

            ViewBag.JOBCASE = new SelectList(db.CSJOBDs.Include(c => c.CSJOBM).Where(x => (x.CSJOBM.CONO == cSBILL.CONO && x.CSJOBM.JOBPOST == "Y" && x.COMPLETE == "N" && x.CSJOBM.VDATE <= cSBILL.ENTDATE)).
                Select(s => new { JOBCASE = s.JOBNO + "-" + s.CASENO, JOBDESC = s.JOBNO + " | " + s.CASENO + " | " + s.CSJOBM.VDATE.ToString() + " | " + s.CASECODE + " | " + s.CASEREM + " | " + s.CASEMEMO, JOBDATE = s.CSJOBM.VDATE.ToString() }).
                OrderBy(y => y.JOBDATE), "JOBCASE", "JOBDESC", cSBILL.JOBCASE);
            return PartialView("Partial/EditCompanyJob", cSBILL);


        }

        public ActionResult EditJobCase(CSBILL cSBILL)
        {
            string UseView = "Partial/EditJobCase";

            ModelState.Remove("JOBNO");
            ModelState.Remove("CASENO");
            ModelState.Remove("CASECODE");
            ModelState.Remove("JOBCASE");
            ModelState.Remove("ITEMDESC");
            ModelState.Remove("ITEMSPEC");
            ModelState.Remove("ITEMTYPE");
            ModelState.Remove("TAXRATE");
            ModelState.Remove("ITEMAMT1");
            ModelState.Remove("ITEMAMT2");
            ModelState.Remove("TAXAMT1");
            ModelState.Remove("TAXAMT2");

            //ModelState.Clear();

            CSJOBD csRec = db.CSJOBDs.Find(cSBILL.JOBNO, cSBILL.CASENO);
            if (csRec != null)
            {
                cSBILL.CASECODE = csRec.CASECODE;

                CSCASE csCase = db.CSCASEs.Find(csRec.CASECODE);
                if (csCase != null)
                {
                    cSBILL.ITEMDESC = csCase.CASEDESC;
                    cSBILL.ITEMSPEC = csRec.CASEMEMO;
                }
            }
            else
            {
                UseView = "Partial/EditJobCaseNoJOBD";
                ViewBag.CASECODE = new SelectList(db.CSCASEs, "CASECODE", "CASEDESC", cSBILL.CASECODE);
            }


            ViewBag.TAXCODE = new SelectList(db.CSTAXTYPEs.Select(s => new { TAXCODE = s.TAXCODE, TAXDESC = s.TAXTYPE + "|" + s.TAXRCODE + "|" + s.TAXRATE.ToString() + "|" + s.EFFECTIVE_START.ToString() + "|" + s.EFFECTIVE_END.ToString() + "|" + s.TAXDESC }), "TAXCODE", "TAXDESC", cSBILL.TAXCODE);

            ViewBag.ITEMTYPE = new SelectList(db.CSITEMs, "ITEMTYPE", "ITEMDESC", cSBILL.ITEMTYPE);
            return PartialView(UseView, cSBILL);
        }

        // GET: CSBILLs/Edit/5
        public ActionResult Edit(string id, int? page)
        {
            ViewBag.Title = "Edit Billing Item";
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSBILL cSBILL = db.CSBILLs.Find(MyHtmlHelpers.ConvertByteStrToId(id));
            if (cSBILL == null)
            {
                return HttpNotFound();
            }

            //ViewBag.JOBCASE = new SelectList(db.CSJOBDs.Include(c => c.CSJOBM).Where(x => (x.JOBNO == cSBILL.JOBNO && x.CASENO == cSBILL.CASENO) || (x.CSJOBM.CONO == cSBILL.CONO && x.CSJOBM.JOBPOST == "Y" && x.COMPLETE == "N" && x.CSJOBM.VDATE <= cSBILL.ENTDATE)).
            //    Select(s => new { JOBCASE = s.JOBNO + "-" + s.CASENO, JOBDESC = s.JOBNO + " | " + s.CASENO + " | " + s.CSJOBM.VDATE.ToString() + " | " + s.CASECODE + " | " + s.CASEREM + " | " + s.CASEMEMO }).
            //    OrderBy(y => y.JOBCASE), "JOBCASE", "JOBDESC", cSBILL.JOBCASE);
            //ViewBag.ITEMTYPE = new SelectList(db.CSITEMs, "ITEMTYPE", "ITEMDESC", cSBILL.ITEMTYPE);
            //ViewBag.CONO = new SelectList(db.CSCOMSTRs.Select(x => new { CONO = x.CONO, CONAME = x.CONAME + "  (" + x.CONO + ")" }).OrderBy(y => y.CONAME), "CONO", "CONAME", cSBILL.CONO);
            //ViewBag.BILLDESC = new SelectList(db.CSBILLDESCs.Select(x => new { BILLDESC = x.BILLDESC, BILLSPEC = x.BILLDESC + " | " + x.BILLSPEC }).OrderBy(y => y.BILLDESC), "BILLDESC", "BILLDESC", cSBILL.ITEMDESC);
            //ViewBag.CASECODE = new SelectList(db.CSCASEs, "CASECODE", "CASEDESC", cSBILL.CASECODE);

            ViewBag.page = page ?? 1;
            Session["CSBillPage"] = ViewBag.page;
            return CallView(cSBILL);
        }

        // POST: CSBILLs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "BILLNO,ENTDATE,CONO,JOBNO,CASENO,CASECODE,ITEMTYPE,ITEMDESC,ITEMSPEC,TAXCODE,TAXRATE,ITEMAMT1,TAXAMT1,NETAMT1,ITEMAMT2,TAXAMT2,NETAMT2,ITEMAMT,TAXAMT,NETAMT,SYSGEN,PRFALLOC,SYSGENBool,PRFALLOCBool,PRFNO,PRFID,STAMP")] CSBILL cSBILL)
        {
            if (ModelState.IsValid)
            {
                ASIDBConnection newdb = new ASIDBConnection();
                try
                {
                    CSBILL curRec = newdb.CSBILLs.Find(cSBILL.BILLNO);
                    if (curRec.STAMP == cSBILL.STAMP)
                    {
                        cSBILL.STAMP = cSBILL.STAMP + 1;

                        db.Entry(cSBILL).State = EntityState.Modified;
                        db.SaveChanges();

                        int page = (int)Session["CSBillPage"];
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

            return CallView(cSBILL);
        }

        public ActionResult CallView(CSBILL cSBILL)
        {
            return CallView("", cSBILL);
        }

        public ActionResult CallView(string ViewName, CSBILL cSBILL)
        {

            //if (_company == null)
            //{
            var _company = db.CSCOMSTRs.Select(x => new { CONO = x.CONO, CONAME = x.CONAME + "  (" + x.COREGNO + ")" }).OrderBy(y => y.CONAME).ToList();
            //}

            ViewBag.JOBCASE = new SelectList(db.CSJOBDs.Include(c => c.CSJOBM).Where(x => (x.JOBNO == cSBILL.JOBNO && x.CASENO == cSBILL.CASENO) || (x.CSJOBM.CONO == cSBILL.CONO && x.CSJOBM.JOBPOST == "Y" && x.COMPLETE == "N" && x.CSJOBM.VDATE <= cSBILL.ENTDATE)).
                    Select(s => new { JOBCASE = s.JOBNO + "-" + s.CASENO, JOBDESC = s.JOBNO + " | " + s.CASENO + " | " + s.CSJOBM.VDATE.ToString() + " | " + s.CASECODE + " | " + s.CASEREM + " | " + s.CASEMEMO, JOBDATE = s.CSJOBM.VDATE.ToString() }).
                    OrderBy(y => y.JOBDATE), "JOBCASE", "JOBDESC", cSBILL.JOBCASE);
            ViewBag.ITEMTYPE = new SelectList(db.CSITEMs, "ITEMTYPE", "ITEMDESC", cSBILL.ITEMTYPE ?? "Work");
            ViewBag.CONO = new SelectList(_company, "CONO", "CONAME", cSBILL.CONO);
            ViewBag.BILLDESC = new SelectList(db.CSBILLDESCs.Select(x => new { BILLDESC = x.BILLDESC, BILLSPEC = x.BILLDESC + " | " + x.BILLSPEC }).OrderBy(y => y.BILLDESC), "BILLDESC", "BILLDESC", cSBILL.ITEMDESC);
            ViewBag.CASECODE = new SelectList(db.CSCASEs, "CASECODE", "CASEDESC", cSBILL.CASECODE);
            ViewBag.TAXCODE = new SelectList(db.CSTAXTYPEs.Select(s => new { TAXCODE = s.TAXCODE, TAXDESC = s.TAXTYPE + "|" + s.TAXRCODE + "|" + s.TAXRATE.ToString() + "|" + s.EFFECTIVE_START.ToString() + "|" + s.EFFECTIVE_END.ToString() + "|" + s.TAXDESC }), "TAXCODE", "TAXDESC", cSBILL.TAXCODE);

            if (ViewName == "") { return View(cSBILL); }
            else
            { return View(ViewName, cSBILL); };
        }

        // GET: CSBILLs/Delete/5
        public ActionResult Delete(string id, int? page)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSBILL cSBILL = db.CSBILLs.Find(MyHtmlHelpers.ConvertByteStrToId(id));
            if (cSBILL == null)
            {
                return HttpNotFound();
            }
            ViewBag.page = page ?? 1;
            Session["CSBillPage"] = ViewBag.page;

            ViewBag.Title = "Delete Billing Item";
            return CallView("Edit", cSBILL);
        }

        // POST: CSBILLs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            CSBILL cSBILL = db.CSBILLs.Find(MyHtmlHelpers.ConvertByteStrToId(id));
            try
            {
                db.CSBILLs.Remove(cSBILL);
                db.SaveChanges();

                int page = (int)Session["CSBillPage"];
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

            ViewBag.page = Session["CSBillPage"];

            ViewBag.Title = "Delete Billing Item";
            return CallView("Edit", cSBILL);
        }

        public ActionResult Listing()
        {
            SACOMP profileRec = db.SACOMPs.FirstOrDefault();
            ViewBag.Company = profileRec.CONAME.ToUpper() + " (" + profileRec.COREGNO + ") ";

            return View(CurrentSelection());
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
