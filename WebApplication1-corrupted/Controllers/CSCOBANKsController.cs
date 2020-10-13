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
    public class CSCOBANKsController : Controller
    {
        private ASIDBConnection db = new ASIDBConnection();

        // GET: CSCOBANKs
        public ActionResult Index()
        {
            var cSCOBANKs = db.CSCOBANKs.Include(c => c.HKBANK);
            return View(cSCOBANKs.ToList());
        }

        // GET: CSCOBANKs/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSCOBANK cSCOBANK = db.CSCOBANKs.Find(id);
            if (cSCOBANK == null)
            {
                return HttpNotFound();
            }
            return View(cSCOBANK);
        }

        // GET: CSCOBANKs/Create
        public ActionResult Create()
        {
            ViewBag.BANKCODE = new SelectList(db.HKBANKs, "BANKCODE", "BANKDESC");
            return View();
        }

        // POST: CSCOBANKs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CONO,BANKCODE,ACTYPE,EFFDATE,TERDATE,ENDDATE,REM,ROWNO,STAMP")] CSCOBANK cSCOBANK)
        {
            if (ModelState.IsValid)
            {
                db.CSCOBANKs.Add(cSCOBANK);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.BANKCODE = new SelectList(db.HKBANKs, "BANKCODE", "BANKDESC", cSCOBANK.BANKCODE);
            return View(cSCOBANK);
        }

        // GET: CSCOBANKs/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSCOBANK cSCOBANK = db.CSCOBANKs.Find(id);
            if (cSCOBANK == null)
            {
                return HttpNotFound();
            }
            ViewBag.BANKCODE = new SelectList(db.HKBANKs, "BANKCODE", "BANKDESC", cSCOBANK.BANKCODE);
            return View(cSCOBANK);
        }

        // POST: CSCOBANKs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CONO,BANKCODE,ACTYPE,EFFDATE,TERDATE,ENDDATE,REM,ROWNO,STAMP")] CSCOBANK cSCOBANK)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cSCOBANK).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BANKCODE = new SelectList(db.HKBANKs, "BANKCODE", "BANKDESC", cSCOBANK.BANKCODE);
            return View(cSCOBANK);
        }

        // GET: CSCOBANKs/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSCOBANK cSCOBANK = db.CSCOBANKs.Find(id);
            if (cSCOBANK == null)
            {
                return HttpNotFound();
            }
            return View(cSCOBANK);
        }

        // POST: CSCOBANKs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            CSCOBANK cSCOBANK = db.CSCOBANKs.Find(id);
            db.CSCOBANKs.Remove(cSCOBANK);
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
