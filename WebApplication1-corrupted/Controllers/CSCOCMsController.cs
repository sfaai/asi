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
    public class CSCOCMsController : Controller
    {
        private ASIDBConnection db = new ASIDBConnection();

        // GET: CSCOCMs
        public ActionResult Index()
        {
            var cSCOCMs = db.CSCOCMs.Include(c => c.CSPR).Include(c => c.CSPRSADDR);
            return View(cSCOCMs.ToList());
        }

        // GET: CSCOCMs/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSCOCM cSCOCM = db.CSCOCMs.Find(id);
            if (cSCOCM == null)
            {
                return HttpNotFound();
            }
            return View(cSCOCM);
        }

        // GET: CSCOCMs/Create
        public ActionResult Create()
        {
            ViewBag.PRSCODE = new SelectList(db.CSPRS, "PRSCODE", "CONSTCODE");
            ViewBag.PRSCODE = new SelectList(db.CSPRSADDRs, "PRSCODE", "MAILADDR");
            return View();
        }

        // POST: CSCOCMs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CONO,REFNO,REFDATE,CMSDATE,CMEDATE,PRSCODE,ADDRID,CMNATURE,LS,CMINFO,REM,STAMP")] CSCOCM cSCOCM)
        {
            if (ModelState.IsValid)
            {
                db.CSCOCMs.Add(cSCOCM);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PRSCODE = new SelectList(db.CSPRS, "PRSCODE", "CONSTCODE", cSCOCM.PRSCODE);
            ViewBag.PRSCODE = new SelectList(db.CSPRSADDRs, "PRSCODE", "MAILADDR", cSCOCM.PRSCODE);
            return View(cSCOCM);
        }

        // GET: CSCOCMs/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSCOCM cSCOCM = db.CSCOCMs.Find(id);
            if (cSCOCM == null)
            {
                return HttpNotFound();
            }
            ViewBag.PRSCODE = new SelectList(db.CSPRS, "PRSCODE", "CONSTCODE", cSCOCM.PRSCODE);
            ViewBag.PRSCODE = new SelectList(db.CSPRSADDRs, "PRSCODE", "MAILADDR", cSCOCM.PRSCODE);
            return View(cSCOCM);
        }

        // POST: CSCOCMs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CONO,REFNO,REFDATE,CMSDATE,CMEDATE,PRSCODE,ADDRID,CMNATURE,LS,CMINFO,REM,STAMP")] CSCOCM cSCOCM)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cSCOCM).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PRSCODE = new SelectList(db.CSPRS, "PRSCODE", "CONSTCODE", cSCOCM.PRSCODE);
            ViewBag.PRSCODE = new SelectList(db.CSPRSADDRs, "PRSCODE", "MAILADDR", cSCOCM.PRSCODE);
            return View(cSCOCM);
        }

        // GET: CSCOCMs/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSCOCM cSCOCM = db.CSCOCMs.Find(id);
            if (cSCOCM == null)
            {
                return HttpNotFound();
            }
            return View(cSCOCM);
        }

        // POST: CSCOCMs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            CSCOCM cSCOCM = db.CSCOCMs.Find(id);
            db.CSCOCMs.Remove(cSCOCM);
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
