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
    public class CSBKsController : Controller
    {
        private ASIDBConnection db = new ASIDBConnection();

        // GET: CSBKs
        public ActionResult Index()
        {
            return View(db.CSBKs.ToList());
        }

        // GET: CSBKs/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSBK cSBK = db.CSBKs.Find(id);
            if (cSBK == null)
            {
                return HttpNotFound();
            }
            return View(cSBK);
        }

        // GET: CSBKs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CSBKs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "BKCODE,BKDESC,STAMP")] CSBK cSBK)
        {
            if (ModelState.IsValid)
            {
                db.CSBKs.Add(cSBK);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(cSBK);
        }

        // GET: CSBKs/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSBK cSBK = db.CSBKs.Find(id);
            if (cSBK == null)
            {
                return HttpNotFound();
            }
            return View(cSBK);
        }

        // POST: CSBKs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "BKCODE,BKDESC,STAMP")] CSBK cSBK)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cSBK).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cSBK);
        }

        // GET: CSBKs/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSBK cSBK = db.CSBKs.Find(id);
            if (cSBK == null)
            {
                return HttpNotFound();
            }
            return View(cSBK);
        }

        // POST: CSBKs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            CSBK cSBK = db.CSBKs.Find(id);
            db.CSBKs.Remove(cSBK);
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
