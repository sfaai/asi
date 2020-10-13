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
    public class CSCOAKsController : Controller
    {
        private ASIDBConnection db = new ASIDBConnection();

        // GET: CSCOAKs
        public ActionResult Index()
        {
            return View(db.CSCOAKs.ToList());
        }

        // GET: CSCOAKs/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSCOAK cSCOAK = db.CSCOAKs.Find(id);
            if (cSCOAK == null)
            {
                return HttpNotFound();
            }
            return View(cSCOAK);
        }

        // GET: CSCOAKs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CSCOAKs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CONO,EFFDATE,AK,REM,ENDDATE,ROWNO,STAMP")] CSCOAK cSCOAK)
        {
            if (ModelState.IsValid)
            {
                db.CSCOAKs.Add(cSCOAK);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(cSCOAK);
        }

        // GET: CSCOAKs/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSCOAK cSCOAK = db.CSCOAKs.Find(id);
            if (cSCOAK == null)
            {
                return HttpNotFound();
            }
            return View(cSCOAK);
        }

        // POST: CSCOAKs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CONO,EFFDATE,AK,REM,ENDDATE,ROWNO,STAMP")] CSCOAK cSCOAK)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cSCOAK).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cSCOAK);
        }

        // GET: CSCOAKs/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSCOAK cSCOAK = db.CSCOAKs.Find(id);
            if (cSCOAK == null)
            {
                return HttpNotFound();
            }
            return View(cSCOAK);
        }

        // POST: CSCOAKs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            CSCOAK cSCOAK = db.CSCOAKs.Find(id);
            db.CSCOAKs.Remove(cSCOAK);
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
