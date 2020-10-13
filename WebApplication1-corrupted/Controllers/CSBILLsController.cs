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

namespace WebApplication1.Controllers
{
    public class CSBILLsController : Controller
    {
        private ASIDBConnection db = new ASIDBConnection();

        


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


            ViewBag.SORTBY = listItems;
            ViewBag.CASECODE = new SelectList(db.CSCASEs.OrderBy(x => x.CASECODE), "CASECODE", "CASECODE");
            ViewBag.ITEMTYPE = new SelectList(db.CSITEMs, "ITEMTYPE", "ITEMDESC");
            return PartialView("Partial/Search", searchRec);
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
            string pSearchCode = "";
            string pSearchName = "";
            string pSearchCase = "";
            string pSearchItem = "";
            string pSearchDesc = "";
            string pSearchBill = "";
            string pSearchJob = "";
            if (Session["SearchBillRec"] != null)
            {
                CSBILL searchRec = (CSBILL)(Session["SearchBillRec"]);
                pSearchCode = searchRec.CONO ?? "";
                pSearchName = searchRec.CSCOMSTR.CONAME ?? "";
                pSearchCase = searchRec.CASECODE ?? "";
                pSearchItem = searchRec.ITEMTYPE ?? "";
                pSearchDesc = searchRec.ITEMDESC ?? "";
                pSearchBill = searchRec.BILLNO ?? "";
                pSearchJob = searchRec.JOBNO ?? "";
            }

            IQueryable<CSBILL> cSBILLs = db.CSBILLs;
            if (pSearchCode != "") { cSBILLs = cSBILLs.Where(x => x.CONO.Contains(pSearchCode)); };
            if (pSearchName != "") { cSBILLs = cSBILLs.Where(x => x.CSCOMSTR.CONAME.Contains(pSearchName)); };
            if (pSearchCase != "") { cSBILLs = cSBILLs.Where(x => x.CASECODE == pSearchCase); };
            if (pSearchItem != "") { cSBILLs = cSBILLs.Where(x => x.ITEMTYPE == pSearchItem); };
            if (pSearchDesc != "") { cSBILLs = cSBILLs.Where(x => x.ITEMDESC.Contains(pSearchDesc)); };
            if (pSearchBill != "") { cSBILLs = cSBILLs.Where(x => x.BILLNO.Contains(pSearchBill)); };
            if (pSearchJob != "") { cSBILLs = cSBILLs.Where(x => x.JOBNO.Contains(pSearchJob)); };

            cSBILLs = cSBILLs.Where(d => d.PRFALLOC == "N").Include(c => c.CSITEM).Include(d => d.CSCOMSTR);

            if ((string)Session["SearchBillSort"] == "CONAME")
            {
                return View("Index", cSBILLs.OrderBy(n => n.CSCOMSTR.CONAME).ToList().ToPagedList(page ?? 1, 30));
            }
            else if ((string)Session["SearchBillSort"] == "ENTDATE")
            {
                return View("Index", cSBILLs.OrderBy(n => n.ENTDATE).ToList().ToPagedList(page ?? 1, 30));

            }
            else if ((string)Session["SearchBillSort"] == "BILLNOLAST")
            {
                return View("Index", cSBILLs.OrderByDescending(n => n.BILLNO).ToList().ToPagedList(page ?? 1, 30));

            }
            else
            {
                return View("Index", cSBILLs.OrderBy(n => n.BILLNO).ToList().ToPagedList(page ?? 1, 30));
            }

        }

        // GET: CSBILLs/Details/5
        public ActionResult Details(string id)
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
            ViewBag.Title = "Billing Details";
            return CallView("Edit", cSBILL);
        }

        // GET: CSBILLs/Create
        public ActionResult Create()
        {
            CSBILL cSBILL = new CSBILL();
            cSBILL.ENTDATE = DateTime.Now;
            cSBILL.SYSGENBool = false;
            cSBILL.STAMP = 1;
            cSBILL.SYSGEN = "N";
            cSBILL.PRFALLOC = "N";
            cSBILL.TAXRATE = db.CSPARAMs.Find("DEFAULT").TAXRATE;
            cSBILL.CONO = "750059-M"; // hack to show first company jobs


            ViewBag.Title = "Create Billing Item";
            return CallView("Edit", cSBILL);
        }

        // POST: CSBILLs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ENTDATE,CONO,JOBNO,SYSGENBool,PRFALLOCBool,CASENO,CASECODE,ITEMTYPE,ITEMDESC,ITEMSPEC,TAXRATE,ITEMAMT1,TAXAMT1,NETAMT1,ITEMAMT2,TAXAMT2,NETAMT2,ITEMAMT,TAXAMT,NETAMT,SYSGEN,PRFALLOC,PRFNO,PRFID,STAMP")] CSBILL cSBILL)
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
                        return RedirectToAction("Index");
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
                Select(s => new { JOBCASE = s.JOBNO + "-" + s.CASENO, JOBDESC = s.JOBNO + " | " + s.CASENO + " | " + s.CSJOBM.VDATE.ToString() + " | " + s.CASECODE + " | " + s.CASEREM + " | " + s.CASEMEMO }).
                OrderBy(y => y.JOBCASE), "JOBCASE", "JOBDESC", cSBILL.JOBCASE);
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


            ViewBag.ITEMTYPE = new SelectList(db.CSITEMs, "ITEMTYPE", "ITEMDESC", cSBILL.ITEMTYPE);
            return PartialView(UseView, cSBILL);
        }

        // GET: CSBILLs/Edit/5
        public ActionResult Edit(string id)
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
            return CallView(cSBILL);
        }

        // POST: CSBILLs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "BILLNO,ENTDATE,CONO,JOBNO,CASENO,CASECODE,ITEMTYPE,ITEMDESC,ITEMSPEC,TAXRATE,ITEMAMT1,TAXAMT1,NETAMT1,ITEMAMT2,TAXAMT2,NETAMT2,ITEMAMT,TAXAMT,NETAMT,SYSGEN,PRFALLOC,SYSGENBool,PRFALLOCBool,PRFNO,PRFID,STAMP")] CSBILL cSBILL)
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
                        return RedirectToAction("Index");
                    }
                    else { ModelState.AddModelError(string.Empty, "Record is modified"); }
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
            var _company = db.CSCOMSTRs.Select(x => new { CONO = x.CONO, CONAME = x.CONAME + "  (" + x.CONO + ")" }).OrderBy(y => y.CONAME).ToList();
            //}

            ViewBag.JOBCASE = new SelectList(db.CSJOBDs.Include(c => c.CSJOBM).Where(x => (x.JOBNO == cSBILL.JOBNO && x.CASENO == cSBILL.CASENO) || (x.CSJOBM.CONO == cSBILL.CONO && x.CSJOBM.JOBPOST == "Y" && x.COMPLETE == "N" && x.CSJOBM.VDATE <= cSBILL.ENTDATE)).
                    Select(s => new { JOBCASE = s.JOBNO + "-" + s.CASENO, JOBDESC = s.JOBNO + " | " + s.CASENO + " | " + s.CSJOBM.VDATE.ToString() + " | " + s.CASECODE + " | " + s.CASEREM + " | " + s.CASEMEMO }).
                    OrderBy(y => y.JOBCASE), "JOBCASE", "JOBDESC", cSBILL.JOBCASE);
            ViewBag.ITEMTYPE = new SelectList(db.CSITEMs, "ITEMTYPE", "ITEMDESC", cSBILL.ITEMTYPE ?? "Work");
            ViewBag.CONO = new SelectList(_company, "CONO", "CONAME", cSBILL.CONO);
            ViewBag.BILLDESC = new SelectList(db.CSBILLDESCs.Select(x => new { BILLDESC = x.BILLDESC, BILLSPEC = x.BILLDESC + " | " + x.BILLSPEC }).OrderBy(y => y.BILLDESC), "BILLDESC", "BILLDESC", cSBILL.ITEMDESC);
            ViewBag.CASECODE = new SelectList(db.CSCASEs, "CASECODE", "CASEDESC", cSBILL.CASECODE);

            if (ViewName == "") { return View(cSBILL); }
            else
            { return View(ViewName, cSBILL); };
        }

        // GET: CSBILLs/Delete/5
        public ActionResult Delete(string id)
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
            ViewBag.Title = "Delete Billing Item";
            return CallView("Edit", cSBILL);
        }

        // POST: CSBILLs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            CSBILL cSBILL = db.CSBILLs.Find(MyHtmlHelpers.ConvertByteStrToId(id));
            db.CSBILLs.Remove(cSBILL);
            db.SaveChanges();
            return RedirectToAction("Index");
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
