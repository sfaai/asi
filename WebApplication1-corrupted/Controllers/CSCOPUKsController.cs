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
    public class CSCOPUKsController : Controller
    {
        private ASIDBConnection db = new ASIDBConnection();

        // GET: CSCOPUKs
        public ActionResult Index()
        {
            var cSCOPUKs = db.CSCOPUKs.Include(c => c.CSEQ);
            return View(cSCOPUKs.ToList());
        }

        // GET: CSCOPUKs/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSCOPUK cSCOPUK = db.CSCOPUKs.Find(id);
            if (cSCOPUK == null)
            {
                return HttpNotFound();
            }
            return View(cSCOPUK);
        }

        // GET: CSCOPUKs/Create
        public ActionResult Create()
        {
            ViewBag.EQCODE = new SelectList(db.CSEQs, "EQCODE", "EQDESC");
            return View();
        }

        // POST: CSCOPUKs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CONO,EFFDATE,EQCODE,EQCONSIDER,NOOFSHARES,NOMINAL,PAIDAMT,DUEAMT,PREMIUM,NCDET,ENDDATE,ROWNO,STAMP")] CSCOPUK cSCOPUK)
        {
            if (ModelState.IsValid)
            {
                db.CSCOPUKs.Add(cSCOPUK);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.EQCODE = new SelectList(db.CSEQs, "EQCODE", "EQDESC", cSCOPUK.EQCODE);
            return View(cSCOPUK);
        }

        // GET: CSCOPUKs/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSCOPUK cSCOPUK = db.CSCOPUKs.Find(id);
            if (cSCOPUK == null)
            {
                return HttpNotFound();
            }
            ViewBag.EQCODE = new SelectList(db.CSEQs, "EQCODE", "EQDESC", cSCOPUK.EQCODE);
            return View(cSCOPUK);
        }

        // POST: CSCOPUKs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CONO,EFFDATE,EQCODE,EQCONSIDER,NOOFSHARES,NOMINAL,PAIDAMT,DUEAMT,PREMIUM,NCDET,ENDDATE,ROWNO,STAMP")] CSCOPUK cSCOPUK)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cSCOPUK).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.EQCODE = new SelectList(db.CSEQs, "EQCODE", "EQDESC", cSCOPUK.EQCODE);
            return View(cSCOPUK);
        }

        // GET: CSCOPUKs/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSCOPUK cSCOPUK = db.CSCOPUKs.Find(id);
            if (cSCOPUK == null)
            {
                return HttpNotFound();
            }
            return View(cSCOPUK);
        }

        // POST: CSCOPUKs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            CSCOPUK cSCOPUK = db.CSCOPUKs.Find(id);
            db.CSCOPUKs.Remove(cSCOPUK);
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
