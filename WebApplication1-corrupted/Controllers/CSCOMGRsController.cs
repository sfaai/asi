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
    public class CSCOMGRsController : Controller
    {
        private ASIDBConnection db = new ASIDBConnection();

        // GET: CSCOMGRs
        public ActionResult Index()
        {
            var cSCOMGRs = db.CSCOMGRs.Include(c => c.CSPR);
            return View(cSCOMGRs.ToList());
        }

        // GET: CSCOMGRs/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSCOMGR cSCOMGR = db.CSCOMGRs.Find(id);
            if (cSCOMGR == null)
            {
                return HttpNotFound();
            }
            return View(cSCOMGR);
        }

        // GET: CSCOMGRs/Create
        public ActionResult Create()
        {
            ViewBag.PRSCODE = new SelectList(db.CSPRS, "PRSCODE", "CONSTCODE");
            return View();
        }

        // POST: CSCOMGRs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CONO,PRSCODE,ADATE,RDATE,ENDDATE,REM,ROWNO,STAMP")] CSCOMGR cSCOMGR)
        {
            if (ModelState.IsValid)
            {
                db.CSCOMGRs.Add(cSCOMGR);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PRSCODE = new SelectList(db.CSPRS, "PRSCODE", "CONSTCODE", cSCOMGR.PRSCODE);
            return View(cSCOMGR);
        }

        // GET: CSCOMGRs/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSCOMGR cSCOMGR = db.CSCOMGRs.Find(id);
            if (cSCOMGR == null)
            {
                return HttpNotFound();
            }
            ViewBag.PRSCODE = new SelectList(db.CSPRS, "PRSCODE", "CONSTCODE", cSCOMGR.PRSCODE);
            return View(cSCOMGR);
        }

        // POST: CSCOMGRs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CONO,PRSCODE,ADATE,RDATE,ENDDATE,REM,ROWNO,STAMP")] CSCOMGR cSCOMGR)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cSCOMGR).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PRSCODE = new SelectList(db.CSPRS, "PRSCODE", "CONSTCODE", cSCOMGR.PRSCODE);
            return View(cSCOMGR);
        }

        // GET: CSCOMGRs/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSCOMGR cSCOMGR = db.CSCOMGRs.Find(id);
            if (cSCOMGR == null)
            {
                return HttpNotFound();
            }
            return View(cSCOMGR);
        }

        // POST: CSCOMGRs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            CSCOMGR cSCOMGR = db.CSCOMGRs.Find(id);
            db.CSCOMGRs.Remove(cSCOMGR);
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
