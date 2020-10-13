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
    public class CSPYRController : Controller
    {
        private ASIDBConnection db = new ASIDBConnection();

        // GET: CSPYR
        public ActionResult Index()
        {
            return View(db.CSPR1.ToList());
        }

        // GET: CSPYR/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSPR1 cSPR1 = db.CSPR1.Find(id);
            if (cSPR1 == null)
            {
                return HttpNotFound();
            }
            return View(cSPR1);
        }

        // GET: CSPYR/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CSPYR/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PRCODE,PRDESC,STAMP")] CSPR1 cSPR1)
        {
            if (ModelState.IsValid)
            {
                db.CSPR1.Add(cSPR1);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(cSPR1);
        }

        // GET: CSPYR/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSPR1 cSPR1 = db.CSPR1.Find(id);
            if (cSPR1 == null)
            {
                return HttpNotFound();
            }
            return View(cSPR1);
        }

        // POST: CSPYR/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PRCODE,PRDESC,STAMP")] CSPR1 cSPR1)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cSPR1).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cSPR1);
        }

        // GET: CSPYR/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSPR1 cSPR1 = db.CSPR1.Find(id);
            if (cSPR1 == null)
            {
                return HttpNotFound();
            }
            return View(cSPR1);
        }

        // POST: CSPYR/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            CSPR1 cSPR1 = db.CSPR1.Find(id);
            db.CSPR1.Remove(cSPR1);
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
