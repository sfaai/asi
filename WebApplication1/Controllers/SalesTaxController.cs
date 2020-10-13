using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;

namespace WebApplication1.Controllers
{
    [Authorize(Roles ="Administrator,CS-A/C")]
    public class SalesTaxController : Controller
    {
        private ASIDBConnection db = new ASIDBConnection();

        public PartialViewResult Search()
        {
            TaxRecord searchRec = null;
            //searchRec.CONO = pSearchCode;
            //searchRec.CONAME = pSearchName;
            if (Session["SearchTaxRecordRec"] != null)
            {
                searchRec = (TaxRecord)Session["SearchTaxRecordRec"];

            }
            else
            {
                searchRec = new TaxRecord();
                searchRec.entdate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                searchRec.DUEDATE = searchRec.entdate.AddMonths(1).AddDays(-1);

            }
            if (Session["SearchTaxRecordSort"] == null)
            {
                Session["SearchTaxRecordSort"] = "TaxRecord";
            };

            List<SelectListItem> listItems = new List<SelectListItem>();
            listItems.Add(new SelectListItem
            {
                Text = "Company Name",
                Value = "CONAME",
                Selected = (string)Session["SearchTaxRecordSort"] == "CONAME"
            });



            listItems.Add(new SelectListItem
            {
                Text = "Date",
                Value = "ENTDATE",
                Selected = (string)Session["SearchTaxRecordSort"] == "ENTDATE"
            });

            listItems.Add(new SelectListItem
            {
                Text = "Date Latest",
                Value = "ENTDATELAST",
                Selected = (string)Session["SearchTaxRecordSort"] == "ENTDATELAST"
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
        public ActionResult SearchPost(TaxRecord TaxRecord)
        {

            Session["SearchTaxRecordRec"] = TaxRecord;
            Session["SearchTaxRecordSort"] = Request.Params["SORTBY"] ?? "SDATE";
            return Redirect("?page=1");
            //return Index(1);
        }

        public IEnumerable<TaxRecord> CurrentSelection()
        {
            //return db.TaxRecords.Where(x => x.ROWNO == db.TaxRecords.Where(y => y.CONO == x.CONO).Max(z => z.ROWNO)).OrderByDescending(x => x.SDATE);

            string pSearchCode = "";
            string pSearchName = "";


            DateTime pSearchVdate = DateTime.Parse("01/01/0001");
            DateTime pSearchDdate = DateTime.Parse("01/01/0001");

            if (Session["SearchTaxRecordRec"] != null)
            {
                TaxRecord searchRec = (TaxRecord)(Session["SearchTaxRecordRec"]);
                pSearchCode = searchRec.cono ?? "";
                pSearchName = searchRec.coname ?? "";
                pSearchVdate = searchRec.entdate;
                pSearchDdate = searchRec.DUEDATE;
     

            }
            else
            { // start with current month proforma bills instead of entire list
                pSearchVdate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                pSearchDdate = pSearchVdate.AddMonths(1).AddDays(-1);
            }

        
         
            DateTime rptStart = pSearchVdate;
            DateTime rptEnd = pSearchDdate;

            ViewBag.RPT_START = rptStart.ToString("dd/MM/yyyy");
            ViewBag.RPT_END = rptEnd.ToString("dd/MM/yyyy");

            FbContext fbcon = FbContext.Create();
           IEnumerable<TaxRecord> TaxRecords = fbcon.PopulateTaxesRcp(pSearchVdate, pSearchDdate, (string)Session["SearchTaxRecordSort"]);

            return TaxRecords;
        }

        // GET: SalesTax
        public ActionResult Index(int? page)
        {
         

            IEnumerable<TaxRecord> TaxRecs = CurrentSelection();

            SACOMP profileRec = db.SACOMPs.FirstOrDefault();
            ViewBag.Company = profileRec.CONAME.ToUpper() + " (" + profileRec.COREGNO + ") ";

            ViewBag.Title = "Sales Tax";
            return View(TaxRecs.ToPagedList(page ?? 1, 30));
        }

        // GET: SalesTax/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: SalesTax/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SalesTax/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: SalesTax/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: SalesTax/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: SalesTax/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: SalesTax/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
