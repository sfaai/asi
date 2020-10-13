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
    public class CSCOSECsController : Controller
    {
        private ASIDBConnection db = new ASIDBConnection();

        // GET: CSCOSECs
        public ActionResult Index()
        {
            var cSCOSECs = db.CSCOSECs.Include(c => c.CICL).Include(c => c.CSPR);
            return View(cSCOSECs.ToList());
        }

        // GET: CSCOSECs/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSCOSEC cSCOSEC = db.CSCOSECs.Find(id);
            if (cSCOSEC == null)
            {
                return HttpNotFound();
            }
            return View(cSCOSEC);
        }

        // GET: CSCOSECs/Create
        public ActionResult Create()
        {
            ViewBag.CONOSEC = new SelectList(db.CICLs, "CLCODE", "CONSTCODE");
            ViewBag.PRSCODE = new SelectList(db.CSPRS, "PRSCODE", "CONSTCODE");
            return View();
        }

        // POST: CSCOSECs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CONO,PRSCODE,CONOSEC,ADATE,RDATE,ENDDATE,REM,ROWNO,STAMP")] CSCOSEC cSCOSEC)
        {
            if (ModelState.IsValid)
            {
                db.CSCOSECs.Add(cSCOSEC);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CONOSEC = new SelectList(db.CICLs, "CLCODE", "CONSTCODE", cSCOSEC.CONOSEC);
            ViewBag.PRSCODE = new SelectList(db.CSPRS, "PRSCODE", "CONSTCODE", cSCOSEC.PRSCODE);
            return View(cSCOSEC);
        }

        // GET: CSCOSECs/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSCOSEC cSCOSEC = db.CSCOSECs.Find(id);
            if (cSCOSEC == null)
            {
                return HttpNotFound();
            }
            ViewBag.CONOSEC = new SelectList(db.CICLs, "CLCODE", "CONSTCODE", cSCOSEC.CONOSEC);
            ViewBag.PRSCODE = new SelectList(db.CSPRS, "PRSCODE", "CONSTCODE", cSCOSEC.PRSCODE);
            return View(cSCOSEC);
        }

        // POST: CSCOSECs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CONO,PRSCODE,CONOSEC,ADATE,RDATE,ENDDATE,REM,ROWNO,STAMP")] CSCOSEC cSCOSEC)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cSCOSEC).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CONOSEC = new SelectList(db.CICLs, "CLCODE", "CONSTCODE", cSCOSEC.CONOSEC);
            ViewBag.PRSCODE = new SelectList(db.CSPRS, "PRSCODE", "CONSTCODE", cSCOSEC.PRSCODE);
            return View(cSCOSEC);
        }

        // GET: CSCOSECs/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSCOSEC cSCOSEC = db.CSCOSECs.Find(id);
            if (cSCOSEC == null)
            {
                return HttpNotFound();
            }
            return View(cSCOSEC);
        }

        // POST: CSCOSECs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            CSCOSEC cSCOSEC = db.CSCOSECs.Find(id);
            db.CSCOSECs.Remove(cSCOSEC);
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
