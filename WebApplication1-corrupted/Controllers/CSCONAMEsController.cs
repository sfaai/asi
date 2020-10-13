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
    public class CSCONAMEsController : Controller
    {
        private ASIDBConnection db = new ASIDBConnection();

        // GET: CSCONAMEs
        public ActionResult Index()
        {
            return View(db.CSCONAMEs.ToList());
        }

        // GET: CSCONAMEs/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSCONAME cSCONAME = db.CSCONAMEs.Find(id);
            if (cSCONAME == null)
            {
                return HttpNotFound();
            }
            return View(cSCONAME);
        }

        // GET: CSCONAMEs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CSCONAMEs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CONO,EFFDATE,CONAME,ENDDATE,ROWNO,STAMP")] CSCONAME cSCONAME)
        {
            if (ModelState.IsValid)
            {
                db.CSCONAMEs.Add(cSCONAME);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(cSCONAME);
        }

        // GET: CSCONAMEs/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSCONAME cSCONAME = db.CSCONAMEs.Find(id);
            if (cSCONAME == null)
            {
                return HttpNotFound();
            }
            return View(cSCONAME);
        }

        // POST: CSCONAMEs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CONO,EFFDATE,CONAME,ENDDATE,ROWNO,STAMP")] CSCONAME cSCONAME)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cSCONAME).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cSCONAME);
        }

        // GET: CSCONAMEs/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSCONAME cSCONAME = db.CSCONAMEs.Find(id);
            if (cSCONAME == null)
            {
                return HttpNotFound();
            }
            return View(cSCONAME);
        }

        // POST: CSCONAMEs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            CSCONAME cSCONAME = db.CSCONAMEs.Find(id);
            db.CSCONAMEs.Remove(cSCONAME);
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
