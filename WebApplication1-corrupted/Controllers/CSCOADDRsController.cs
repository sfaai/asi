using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication1;

namespace WebApplication1.Controllers
{
    public class CSCOADDRsController : Controller
    {
        private ASIDBConnection db = new ASIDBConnection();

        // GET: CSCOADDRs
        public ActionResult Index()
        {
            var cSCOADDRs = db.CSCOADDRs.Include(c => c.HKCITY).Include(c => c.HKSTATE).Include(c => c.HKCTRY);
            return View(cSCOADDRs.ToList());
        }

        // GET: CSCOADDRs/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSCOADDR cSCOADDR = db.CSCOADDRs.Find(id);
            if (cSCOADDR == null)
            {
                return HttpNotFound();
            }
            return View(cSCOADDR);
        }

        // GET: CSCOADDRs/Create
        public ActionResult Create()
        {
            ViewBag.CITYCODE = new SelectList(db.HKCITies, "CITYCODE", "CITYDESC");
            ViewBag.STATECODE = new SelectList(db.HKSTATEs, "STATECODE", "STATEDESC");
            ViewBag.CTRYCODE = new SelectList(db.HKCTRies, "CTRYCODE", "CTRYDESC");
            return View();
        }

        // POST: CSCOADDRs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CONO,ADDRID,MAILADDR,ADDRTYPE,ADDR1,ADDR2,ADDR3,POSTAL,CITYCODE,STATECODE,CTRYCODE,PHONE1,PHONE2,FAX1,FAX2,OPRHRS,REM,SDATE,EDATE,ENDDATE,STAMP")] CSCOADDR cSCOADDR)
        {
            if (ModelState.IsValid)
            {
                db.CSCOADDRs.Add(cSCOADDR);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CITYCODE = new SelectList(db.HKCITies, "CITYCODE", "CITYDESC", cSCOADDR.CITYCODE);
            ViewBag.STATECODE = new SelectList(db.HKSTATEs, "STATECODE", "STATEDESC", cSCOADDR.STATECODE);
            ViewBag.CTRYCODE = new SelectList(db.HKCTRies, "CTRYCODE", "CTRYDESC", cSCOADDR.CTRYCODE);
            return View(cSCOADDR);
        }

        // GET: CSCOADDRs/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSCOADDR cSCOADDR = db.CSCOADDRs.Find(id);
            if (cSCOADDR == null)
            {
                return HttpNotFound();
            }
            ViewBag.CITYCODE = new SelectList(db.HKCITies, "CITYCODE", "CITYDESC", cSCOADDR.CITYCODE);
            ViewBag.STATECODE = new SelectList(db.HKSTATEs, "STATECODE", "STATEDESC", cSCOADDR.STATECODE);
            ViewBag.CTRYCODE = new SelectList(db.HKCTRies, "CTRYCODE", "CTRYDESC", cSCOADDR.CTRYCODE);
            return View(cSCOADDR);
        }

        // POST: CSCOADDRs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CONO,ADDRID,MAILADDR,ADDRTYPE,ADDR1,ADDR2,ADDR3,POSTAL,CITYCODE,STATECODE,CTRYCODE,PHONE1,PHONE2,FAX1,FAX2,OPRHRS,REM,SDATE,EDATE,ENDDATE,STAMP")] CSCOADDR cSCOADDR)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cSCOADDR).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CITYCODE = new SelectList(db.HKCITies, "CITYCODE", "CITYDESC", cSCOADDR.CITYCODE);
            ViewBag.STATECODE = new SelectList(db.HKSTATEs, "STATECODE", "STATEDESC", cSCOADDR.STATECODE);
            ViewBag.CTRYCODE = new SelectList(db.HKCTRies, "CTRYCODE", "CTRYDESC", cSCOADDR.CTRYCODE);
            return View(cSCOADDR);
        }

        // GET: CSCOADDRs/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSCOADDR cSCOADDR = db.CSCOADDRs.Find(id);
            if (cSCOADDR == null)
            {
                return HttpNotFound();
            }
            return View(cSCOADDR);
        }

        // POST: CSCOADDRs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            CSCOADDR cSCOADDR = db.CSCOADDRs.Find(id);
            db.CSCOADDRs.Remove(cSCOADDR);
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
