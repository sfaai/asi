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
    public class CSCOFEEsController : Controller
    {
        private ASIDBConnection db = new ASIDBConnection();

        // GET: CSCOFEEs
        public ActionResult Index()
        {
            var cSCOFEEs = db.CSCOFEEs.Include(c => c.CSCASE);
            return View(cSCOFEEs.ToList());
        }

        // GET: CSCOFEEs/Details/5
        public ActionResult Details(string id, string fee)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSCOFEE cSCOFEE = db.CSCOFEEs.Find(id, fee);
            if (cSCOFEE == null)
            {
                return HttpNotFound();
            }
            return View(cSCOFEE);
        }

        // GET: CSCOFEEs/Create
        public ActionResult Create()
        {
            ViewBag.FEECODE = new SelectList(db.CSCASEs, "CASECODE", "CASEDESC");
            return View();
        }

        // POST: CSCOFEEs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CONO,FEECODE,FEEAMT,FEEMTH,FEETYPE,LASTTOUCH,STAMP")] CSCOFEE cSCOFEE)
        {
            if (ModelState.IsValid)
            {
                db.CSCOFEEs.Add(cSCOFEE);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.FEECODE = new SelectList(db.CSCASEs, "CASECODE", "CASEDESC", cSCOFEE.FEECODE);
            return View(cSCOFEE);
        }

        // GET: CSCOFEEs/Edit/5
        public ActionResult Edit(string id, string fee)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSCOFEE cSCOFEE = db.CSCOFEEs.Find(id, fee);
            if (cSCOFEE == null)
            {
                return HttpNotFound();
            }
            ViewBag.FEECODE = new SelectList(db.CSCASEs, "CASECODE", "CASEDESC", cSCOFEE.FEECODE);
            return View(cSCOFEE);
        }

        // POST: CSCOFEEs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CONO,FEECODE,FEEAMT,FEEMTH,FEETYPE,LASTTOUCH,STAMP")] CSCOFEE cSCOFEE)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cSCOFEE).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.FEECODE = new SelectList(db.CSCASEs, "CASECODE", "CASEDESC", cSCOFEE.FEECODE);
            return View(cSCOFEE);
        }

        // GET: CSCOFEEs/Delete/5
        public ActionResult Delete(string id, string fee)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSCOFEE cSCOFEE = db.CSCOFEEs.Find(id, fee);
            if (cSCOFEE == null)
            {
                return HttpNotFound();
            }
            return View(cSCOFEE);
        }

        // POST: CSCOFEEs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id, string fee)
        {
            CSCOFEE cSCOFEE = db.CSCOFEEs.Find(id, fee);
            db.CSCOFEEs.Remove(cSCOFEE);
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
