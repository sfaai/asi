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
    public class CSCODRsController : Controller
    {
        private ASIDBConnection db = new ASIDBConnection();

        // GET: CSCODRs
        public ActionResult Index()
        {
            var cSCODRs = db.CSCODRs.Include(c => c.CSPR);
            return View(cSCODRs.ToList());
        }

        // GET: CSCODRs/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSCODR cSCODR = db.CSCODRs.Find(id);
            if (cSCODR == null)
            {
                return HttpNotFound();
            }
            return View(cSCODR);
        }

        // GET: CSCODRs/Create
        public ActionResult Create()
        {
            ViewBag.PRSCODE = new SelectList(db.CSPRS, "PRSCODE", "CONSTCODE");
            return View();
        }

        // POST: CSCODRs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CONO,PRSCODE,ADATE,RDATE,ENDDATE,REM,ROWNO,STAMP")] CSCODR cSCODR)
        {
            if (ModelState.IsValid)
            {
                db.CSCODRs.Add(cSCODR);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PRSCODE = new SelectList(db.CSPRS, "PRSCODE", "CONSTCODE", cSCODR.PRSCODE);
            return View(cSCODR);
        }

        // GET: CSCODRs/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSCODR cSCODR = db.CSCODRs.Find(id);
            if (cSCODR == null)
            {
                return HttpNotFound();
            }
            ViewBag.PRSCODE = new SelectList(db.CSPRS, "PRSCODE", "CONSTCODE", cSCODR.PRSCODE);
            return View(cSCODR);
        }

        // POST: CSCODRs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CONO,PRSCODE,ADATE,RDATE,ENDDATE,REM,ROWNO,STAMP")] CSCODR cSCODR)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cSCODR).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PRSCODE = new SelectList(db.CSPRS, "PRSCODE", "CONSTCODE", cSCODR.PRSCODE);
            return View(cSCODR);
        }

        // GET: CSCODRs/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSCODR cSCODR = db.CSCODRs.Find(id);
            if (cSCODR == null)
            {
                return HttpNotFound();
            }
            return View(cSCODR);
        }

        // POST: CSCODRs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            CSCODR cSCODR = db.CSCODRs.Find(id);
            db.CSCODRs.Remove(cSCODR);
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
