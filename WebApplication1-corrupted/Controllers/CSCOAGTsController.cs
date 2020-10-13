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
    public class CSCOAGTsController : Controller
    {
        private ASIDBConnection db = new ASIDBConnection();

        // GET: CSCOAGTs
        public ActionResult Index()
        {
            var cSCOAGTs = db.CSCOAGTs.Include(c => c.CSPR);
            return View(cSCOAGTs.ToList());
        }

        // GET: CSCOAGTs/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSCOAGT cSCOAGT = db.CSCOAGTs.Find(id);
            if (cSCOAGT == null)
            {
                return HttpNotFound();
            }
            return View(cSCOAGT);
        }

        // GET: CSCOAGTs/Create
        public ActionResult Create()
        {
            ViewBag.PRSCODE = new SelectList(db.CSPRS, "PRSCODE", "CONSTCODE");
            return View();
        }

        // POST: CSCOAGTs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CONO,PRSCODE,ADATE,RDATE,ENDDATE,REM,ROWNO,STAMP")] CSCOAGT cSCOAGT)
        {
            if (ModelState.IsValid)
            {
                db.CSCOAGTs.Add(cSCOAGT);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PRSCODE = new SelectList(db.CSPRS, "PRSCODE", "CONSTCODE", cSCOAGT.PRSCODE);
            return View(cSCOAGT);
        }

        // GET: CSCOAGTs/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSCOAGT cSCOAGT = db.CSCOAGTs.Find(id);
            if (cSCOAGT == null)
            {
                return HttpNotFound();
            }
            ViewBag.PRSCODE = new SelectList(db.CSPRS, "PRSCODE", "CONSTCODE", cSCOAGT.PRSCODE);
            return View(cSCOAGT);
        }

        // POST: CSCOAGTs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CONO,PRSCODE,ADATE,RDATE,ENDDATE,REM,ROWNO,STAMP")] CSCOAGT cSCOAGT)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cSCOAGT).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PRSCODE = new SelectList(db.CSPRS, "PRSCODE", "CONSTCODE", cSCOAGT.PRSCODE);
            return View(cSCOAGT);
        }

        // GET: CSCOAGTs/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSCOAGT cSCOAGT = db.CSCOAGTs.Find(id);
            if (cSCOAGT == null)
            {
                return HttpNotFound();
            }
            return View(cSCOAGT);
        }

        // POST: CSCOAGTs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            CSCOAGT cSCOAGT = db.CSCOAGTs.Find(id);
            db.CSCOAGTs.Remove(cSCOAGT);
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
