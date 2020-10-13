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
    public class CSCODEBsController : Controller
    {
        private ASIDBConnection db = new ASIDBConnection();

        // GET: CSCODEBs
        public ActionResult Index()
        {
            var cSCODEBs = db.CSCODEBs.Include(c => c.CSPR);
            return View(cSCODEBs.ToList());
        }

        // GET: CSCODEBs/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSCODEB cSCODEB = db.CSCODEBs.Find(id);
            if (cSCODEB == null)
            {
                return HttpNotFound();
            }
            return View(cSCODEB);
        }

        // GET: CSCODEBs/Create
        public ActionResult Create()
        {
            ViewBag.PRSCODE = new SelectList(db.CSPRS, "PRSCODE", "CONSTCODE");
            return View();
        }

        // POST: CSCODEBs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CONO,REFNO,REFDATE,DEEDDATE,PRSCODE,SECUREAMT,ISSUEAMT,DEBCOND,DEBINFO,REM,STAMP")] CSCODEB cSCODEB)
        {
            if (ModelState.IsValid)
            {
                db.CSCODEBs.Add(cSCODEB);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PRSCODE = new SelectList(db.CSPRS, "PRSCODE", "CONSTCODE", cSCODEB.PRSCODE);
            return View(cSCODEB);
        }

        // GET: CSCODEBs/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSCODEB cSCODEB = db.CSCODEBs.Find(id);
            if (cSCODEB == null)
            {
                return HttpNotFound();
            }
            ViewBag.PRSCODE = new SelectList(db.CSPRS, "PRSCODE", "CONSTCODE", cSCODEB.PRSCODE);
            return View(cSCODEB);
        }

        // POST: CSCODEBs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CONO,REFNO,REFDATE,DEEDDATE,PRSCODE,SECUREAMT,ISSUEAMT,DEBCOND,DEBINFO,REM,STAMP")] CSCODEB cSCODEB)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cSCODEB).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PRSCODE = new SelectList(db.CSPRS, "PRSCODE", "CONSTCODE", cSCODEB.PRSCODE);
            return View(cSCODEB);
        }

        // GET: CSCODEBs/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSCODEB cSCODEB = db.CSCODEBs.Find(id);
            if (cSCODEB == null)
            {
                return HttpNotFound();
            }
            return View(cSCODEB);
        }

        // POST: CSCODEBs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            CSCODEB cSCODEB = db.CSCODEBs.Find(id);
            db.CSCODEBs.Remove(cSCODEB);
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
