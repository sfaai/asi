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
    public class CSCOSHesController : Controller
    {
        private ASIDBConnection db = new ASIDBConnection();

        // GET: CSCOSHes
        public ActionResult Index()
        {
            var cSCOSHes = db.CSCOSHes.Include(c => c.CSPR);
            return View(cSCOSHes.ToList());
        }

        // GET: CSCOSHes/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSCOSH cSCOSH = db.CSCOSHes.Find(id);
            if (cSCOSH == null)
            {
                return HttpNotFound();
            }
            return View(cSCOSH);
        }

        // GET: CSCOSHes/Create
        public ActionResult Create()
        {
            ViewBag.PRSCODE = new SelectList(db.CSPRS, "PRSCODE", "CONSTCODE");
            return View();
        }

        // POST: CSCOSHes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CONO,PRSCODE,EFFDATE,TERDATE,ENDDATE,FOLIONO,REM,STAMP")] CSCOSH cSCOSH)
        {
            if (ModelState.IsValid)
            {
                db.CSCOSHes.Add(cSCOSH);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PRSCODE = new SelectList(db.CSPRS, "PRSCODE", "CONSTCODE", cSCOSH.PRSCODE);
            return View(cSCOSH);
        }

        // GET: CSCOSHes/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSCOSH cSCOSH = db.CSCOSHes.Find(id);
            if (cSCOSH == null)
            {
                return HttpNotFound();
            }
            ViewBag.PRSCODE = new SelectList(db.CSPRS, "PRSCODE", "CONSTCODE", cSCOSH.PRSCODE);
            return View(cSCOSH);
        }

        // POST: CSCOSHes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CONO,PRSCODE,EFFDATE,TERDATE,ENDDATE,FOLIONO,REM,STAMP")] CSCOSH cSCOSH)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cSCOSH).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PRSCODE = new SelectList(db.CSPRS, "PRSCODE", "CONSTCODE", cSCOSH.PRSCODE);
            return View(cSCOSH);
        }

        // GET: CSCOSHes/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSCOSH cSCOSH = db.CSCOSHes.Find(id);
            if (cSCOSH == null)
            {
                return HttpNotFound();
            }
            return View(cSCOSH);
        }

        // POST: CSCOSHes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            CSCOSH cSCOSH = db.CSCOSHes.Find(id);
            db.CSCOSHes.Remove(cSCOSH);
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
