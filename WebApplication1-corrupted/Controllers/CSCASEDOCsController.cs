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
    public class CSCASEDOCsController : Controller
    {
        private ASIDBConnection db = new ASIDBConnection();

        // GET: CSCASEDOCs
        public ActionResult Index()
        {
            return View(db.CSCASEDOCs.ToList());
        }

        // GET: CSCASEDOCs/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSCASEDOC cSCASEDOC = db.CSCASEDOCs.Find(id);
            if (cSCASEDOC == null)
            {
                return HttpNotFound();
            }
            return View(cSCASEDOC);
        }

        // GET: CSCASEDOCs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CSCASEDOCs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CASECODE,DOCINVID,DOCINVDESC,DOCINVQTY,STAMP")] CSCASEDOC cSCASEDOC)
        {
            if (ModelState.IsValid)
            {
                db.CSCASEDOCs.Add(cSCASEDOC);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(cSCASEDOC);
        }

        // GET: CSCASEDOCs/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSCASEDOC cSCASEDOC = db.CSCASEDOCs.Find(id);
            if (cSCASEDOC == null)
            {
                return HttpNotFound();
            }
            return View(cSCASEDOC);
        }

        // POST: CSCASEDOCs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CASECODE,DOCINVID,DOCINVDESC,DOCINVQTY,STAMP")] CSCASEDOC cSCASEDOC)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cSCASEDOC).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cSCASEDOC);
        }

        // GET: CSCASEDOCs/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSCASEDOC cSCASEDOC = db.CSCASEDOCs.Find(id);
            if (cSCASEDOC == null)
            {
                return HttpNotFound();
            }
            return View(cSCASEDOC);
        }

        // POST: CSCASEDOCs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            CSCASEDOC cSCASEDOC = db.CSCASEDOCs.Find(id);
            db.CSCASEDOCs.Remove(cSCASEDOC);
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
