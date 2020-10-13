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
    public class CSPRFsController : Controller
    {
        private ASIDBConnection db = new ASIDBConnection();

        // GET: CSPRFs
        public ActionResult Index(int ? page)
        {
            var cSPRFs = db.CSPRFs.Include(d => d.CSCOADDR).Include( e => e.CSCOMSTR);
            return View(cSPRFs.ToList().ToPagedList(page ?? 1, 30));
        }

        public PartialViewResult BillAllocated( string id)
        {
            string sid = MyHtmlHelpers.ConvertByteStrToId(id);
            var cSBILLs = db.CSBILLs.Where(x => x.PRFNO == sid);
            return PartialView("Partial/BillAllocated",cSBILLs);
        }

        public PartialViewResult BillOpen(string id)
        {
            string sid = MyHtmlHelpers.ConvertByteStrToId(id);
            var cSBILLs = db.CSBILLs.Where(x => x.CONO == sid);
            return PartialView("Partial/BillOpen",cSBILLs);
        }

        // GET: CSPRFs/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSPRF cSPRF = db.CSPRFs.Find(MyHtmlHelpers.ConvertByteStrToId(id));
            if (cSPRF == null)
            {
                return HttpNotFound();
            }
            return View(cSPRF);
        }

        // GET: CSPRFs/Create
        public ActionResult Create()
        {
            ViewBag.CONO = new SelectList(db.CSCOADDRs, "CONO", "MAILADDR");
            return View();
        }

        // POST: CSPRFs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PRFNO,VDATE,DUEDAYS,DUEDATE,CONO,COADDRID,ATTN,REM,SEQNO,INVALLOC,INVNO,INVID,STAMP")] CSPRF cSPRF)
        {
            if (ModelState.IsValid)
            {
                db.CSPRFs.Add(cSPRF);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CONO = new SelectList(db.CSCOADDRs, "CONO", "MAILADDR", cSPRF.CONO);
            return View(cSPRF);
        }

        public ActionResult EditCompany(CSPRF cSPRF)
        {

            //ModelState.Clear(); //apparent this has side-effects and using Remove is preferrable
            ModelState.Remove("CONO");
            ModelState.Remove("ATTN");
            ModelState.Remove("COADDR");

            cSPRF.COADDR = null;
            cSPRF.ATTN = null;

            //ViewBag.ATTNDESC = new SelectList(db.CSCOPICs.Where(x => x.CONO == cSPRF.CONO).Select(x => new { PRSCODE = x.PRSCODE, PRSDESC= x.CSPR.PRSNAME + " | " + x.DESIG }).OrderBy(y => y.PRSDESC), "PRSCODE", "PRSDESC", cSPRF.ATTN);
            ViewBag.ATTNDESC = new SelectList(db.CSCOPICs.Where(x => x.CONO == cSPRF.CONO).Select(x => new { PRSNAME = x.CSPR.PRSNAME, PRSDESC = x.CSPR.PRSNAME }).OrderBy(y => y.PRSDESC), "PRSNAME", "PRSDESC", cSPRF.ATTN);
            ViewBag.COADDR = new SelectList(db.CSCOADDRs.Where(x => x.CONO == cSPRF.CONO).Select(x => new { COADDR = x.CONO + "-" + x.ADDRID, COADDRDESC = x.ADDRTYPE + " | " + x.MAILADDR + " | " + x.ADDR1 + " " + x.ADDR2 + " " + x.ADDR3 }), "COADDR", "COADDRDESC", cSPRF.COADDR);
            return PartialView("Partial/EditCompany", cSPRF);


        }

        // GET: CSPRFs/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSPRF cSPRF = db.CSPRFs.Find(MyHtmlHelpers.ConvertByteStrToId(id));
            if (cSPRF == null)
            {
                return HttpNotFound();
            }
            ViewBag.CONO = new SelectList(db.CSCOMSTRs.Select(x => new { CONO = x.CONO, CONAME = x.CONAME + "  (" + x.CONO + ")" }).OrderBy(y => y.CONAME), "CONO", "CONAME", cSPRF.CONO);
            ViewBag.ATTNDESC = new SelectList(db.CSCOPICs.Where(x => x.CONO == cSPRF.CONO).Select(x => new { PRSNAME = x.CSPR.PRSNAME, PRSDESC = x.CSPR.PRSNAME  }).OrderBy(y => y.PRSDESC), "PRSNAME", "PRSDESC", cSPRF.ATTN);
            ViewBag.COADDR = new SelectList(db.CSCOADDRs.Where(x => x.CONO == cSPRF.CONO).Select(x => new { COADDR = x.CONO + "-" + x.ADDRID, COADDRDESC = x.ADDRTYPE + " | " + x.MAILADDR + " | " + x.ADDR1 + " " + x.ADDR2 + " " + x.ADDR3 }), "COADDR", "COADDRDESC", cSPRF.COADDR);

            ViewBag.Title = "Edit Proforma Bill";
            return View(cSPRF);
        }

        // POST: CSPRFs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PRFNO,VDATE,DUEDAYS,DUEDATE,CONO,COADDRID,ATTN,REM,SEQNO,INVALLOC,INVNO,INVID,STAMP")] CSPRF cSPRF)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cSPRF).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CONO = new SelectList(db.CSCOMSTRs.Select(x => new { CONO = x.CONO, CONAME = x.CONAME + "  (" + x.CONO + ")" }).OrderBy(y => y.CONAME), "CONO", "CONAME", cSPRF.CONO);
            ViewBag.ATTNDESC = new SelectList(db.CSCOPICs.Where(x => x.CONO == cSPRF.CONO).Select(x => new { PRSNAME = x.CSPR.PRSNAME, PRSDESC = x.CSPR.PRSNAME }).OrderBy(y => y.PRSDESC), "PRSNAME", "PRSDESC", cSPRF.ATTN);
            ViewBag.COADDR = new SelectList(db.CSCOADDRs.Where(x => x.CONO == cSPRF.CONO).Select(x => new { COADDR = x.CONO + "-" + x.ADDRID, COADDRDESC = x.ADDRTYPE + " | " + x.MAILADDR + " | " + x.ADDR1 + " " + x.ADDR2 + " " + x.ADDR3 }), "COADDR", "COADDRDESC", cSPRF.COADDR);

            return View(cSPRF);
        }

        // GET: CSPRFs/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSPRF cSPRF = db.CSPRFs.Find(MyHtmlHelpers.ConvertByteStrToId(id));
            if (cSPRF == null)
            {
                return HttpNotFound();
            }
            ViewBag.CONO = new SelectList(db.CSCOMSTRs.Select(x => new { CONO = x.CONO, CONAME = x.CONAME + "  (" + x.CONO + ")" }).OrderBy(y => y.CONAME), "CONO", "CONAME", cSPRF.CONO);
            ViewBag.ATTNDESC = new SelectList(db.CSCOPICs.Where(x => x.CONO == cSPRF.CONO).Select(x => new { PRSNAME = x.CSPR.PRSNAME, PRSDESC = x.CSPR.PRSNAME }).OrderBy(y => y.PRSDESC), "PRSNAME", "PRSDESC", cSPRF.ATTN);
            ViewBag.COADDR = new SelectList(db.CSCOADDRs.Where(x => x.CONO == cSPRF.CONO).Select(x => new { COADDR = x.CONO + "-" + x.ADDRID, COADDRDESC = x.ADDRTYPE + " | " + x.MAILADDR + " | " + x.ADDR1 + " " + x.ADDR2 + " " + x.ADDR3 }), "COADDR", "COADDRDESC", cSPRF.COADDR);

            return View(cSPRF);
        }

        // POST: CSPRFs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            CSPRF cSPRF = db.CSPRFs.Find(MyHtmlHelpers.ConvertByteStrToId(id));
            db.CSPRFs.Remove(cSPRF);
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
