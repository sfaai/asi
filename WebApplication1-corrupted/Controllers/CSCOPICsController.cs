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
    public class CSCOPICsController : Controller
    {
        private ASIDBConnection db = new ASIDBConnection();

        // GET: CSCOPICs
        public ActionResult Index()
        {
            var cSCOPICs = db.CSCOPICs.Include(c => c.CSPR);
            return View(cSCOPICs.ToList());
        }

        // GET: CSCOPICs/Details/5
        public ActionResult Details(string id, string person)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSCOPIC cSCOPIC = db.CSCOPICs.Find(id, person);
            if (cSCOPIC == null)
            {
                return HttpNotFound();
            }
            return View(cSCOPIC);
        }

        // GET: CSCOPICs/Create
        public ActionResult Create()
        {
            ViewBag.PRSCODE = new SelectList(db.CSPRS, "PRSCODE", "CONSTCODE");
            return View();
        }

        // POST: CSCOPICs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CONO,PRSCODE,DESIG,REM,STAMP")] CSCOPIC cSCOPIC)
        {
            if (ModelState.IsValid)
            {
                db.CSCOPICs.Add(cSCOPIC);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PRSCODE = new SelectList(db.CSPRS, "PRSCODE", "CONSTCODE", cSCOPIC.PRSCODE);
            return View(cSCOPIC);
        }

        // GET: CSCOPICs/Edit/5
        public ActionResult Edit(string id, string person)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSCOPIC cSCOPIC = db.CSCOPICs.Find(id, person);
            if (cSCOPIC == null)
            {
                return HttpNotFound();
            }
            ViewBag.PRSCODE = new SelectList(db.CSPRS, "PRSCODE", "PRSNAME", cSCOPIC.PRSCODE);
            return View(cSCOPIC);
        }

        // POST: CSCOPICs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CONO,PRSCODE,DESIG,REM,STAMP")] CSCOPIC cSCOPIC)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cSCOPIC).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PRSCODE = new SelectList(db.CSPRS, "PRSCODE", "CONSTCODE", cSCOPIC.PRSCODE);
            return View(cSCOPIC);
        }

        // GET: CSCOPICs/Delete/5
        public ActionResult Delete(string id, string person)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSCOPIC cSCOPIC = db.CSCOPICs.Find(id, person);
            if (cSCOPIC == null)
            {
                return HttpNotFound();
            }
            return View(cSCOPIC);
        }

        // POST: CSCOPICs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id, string person)
        {
            CSCOPIC cSCOPIC = db.CSCOPICs.Find(id, person);
            db.CSCOPICs.Remove(cSCOPIC);
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
