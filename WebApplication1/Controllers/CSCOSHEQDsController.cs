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
    public class CSCOSHEQDsController : Controller
    {
        private ASIDBConnection db = new ASIDBConnection();

        // GET: CSCOSHEQDs
        public ActionResult Index()
        {
            var cSCOSHEQDs = db.CSCOSHEQDs.Include(c => c.CSCOSH);
            return View(cSCOSHEQDs.ToList());
        }

        // GET: CSCOSHEQDs/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSCOSHEQD cSCOSHEQD = db.CSCOSHEQDs.Find(id);
            if (cSCOSHEQD == null)
            {
                return HttpNotFound();
            }
            return View(cSCOSHEQD);
        }

        // GET: CSCOSHEQDs/Create
        public ActionResult Create()
        {
            ViewBag.CONO = new SelectList(db.CSCOSHes, "CONO", "REM");
            return View();
        }

        // POST: CSCOSHEQDs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CONO,PRSCODE,MVTYPE,MVNO,MVID,INMVTYPE,INMVNO,INMVID,OUTMVTYPE,OUTMVNO,OUTMVID,MVDATE,SHAREAMT,STAMP")] CSCOSHEQD cSCOSHEQD)
        {
            if (ModelState.IsValid)
            {
                db.CSCOSHEQDs.Add(cSCOSHEQD);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CONO = new SelectList(db.CSCOSHes, "CONO", "REM", cSCOSHEQD.CONO);
            return View(cSCOSHEQD);
        }

        // GET: CSCOSHEQDs/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSCOSHEQD cSCOSHEQD = db.CSCOSHEQDs.Find(id);
            if (cSCOSHEQD == null)
            {
                return HttpNotFound();
            }
            ViewBag.CONO = new SelectList(db.CSCOSHes, "CONO", "REM", cSCOSHEQD.CONO);
            return View(cSCOSHEQD);
        }

        // POST: CSCOSHEQDs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CONO,PRSCODE,MVTYPE,MVNO,MVID,INMVTYPE,INMVNO,INMVID,OUTMVTYPE,OUTMVNO,OUTMVID,MVDATE,SHAREAMT,STAMP")] CSCOSHEQD cSCOSHEQD)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cSCOSHEQD).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CONO = new SelectList(db.CSCOSHes, "CONO", "REM", cSCOSHEQD.CONO);
            return View(cSCOSHEQD);
        }

        // GET: CSCOSHEQDs/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSCOSHEQD cSCOSHEQD = db.CSCOSHEQDs.Find(id);
            if (cSCOSHEQD == null)
            {
                return HttpNotFound();
            }
            return View(cSCOSHEQD);
        }

        // POST: CSCOSHEQDs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            CSCOSHEQD cSCOSHEQD = db.CSCOSHEQDs.Find(id);
            db.CSCOSHEQDs.Remove(cSCOSHEQD);
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
