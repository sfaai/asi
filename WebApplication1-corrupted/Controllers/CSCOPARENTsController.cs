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
    public class CSCOPARENTsController : Controller
    {
        private ASIDBConnection db = new ASIDBConnection();

        // GET: CSCOPARENTs
        public ActionResult Index()
        {
            var cSCOPARENTs = db.CSCOPARENTs.Include(c => c.CSCOMSTR);
            return View(cSCOPARENTs.ToList());
        }

        // GET: CSCOPARENTs/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSCOPARENT cSCOPARENT = db.CSCOPARENTs.Find(id);
            if (cSCOPARENT == null)
            {
                return HttpNotFound();
            }
            return View(cSCOPARENT);
        }

        // GET: CSCOPARENTs/Create
        public ActionResult Create()
        {
            ViewBag.CONOPARENT = new SelectList(db.CSCOMSTRs, "CONO", "INCCTRY");
            return View();
        }

        // POST: CSCOPARENTs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CONO,CONOPARENT,ADATE,RDATE,ENDDATE,REM,ROWNO,STAMP")] CSCOPARENT cSCOPARENT)
        {
            if (ModelState.IsValid)
            {
                db.CSCOPARENTs.Add(cSCOPARENT);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CONOPARENT = new SelectList(db.CSCOMSTRs, "CONO", "INCCTRY", cSCOPARENT.CONOPARENT);
            return View(cSCOPARENT);
        }

        // GET: CSCOPARENTs/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSCOPARENT cSCOPARENT = db.CSCOPARENTs.Find(id);
            if (cSCOPARENT == null)
            {
                return HttpNotFound();
            }
            ViewBag.CONOPARENT = new SelectList(db.CSCOMSTRs, "CONO", "INCCTRY", cSCOPARENT.CONOPARENT);
            return View(cSCOPARENT);
        }

        // POST: CSCOPARENTs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CONO,CONOPARENT,ADATE,RDATE,ENDDATE,REM,ROWNO,STAMP")] CSCOPARENT cSCOPARENT)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cSCOPARENT).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CONOPARENT = new SelectList(db.CSCOMSTRs, "CONO", "INCCTRY", cSCOPARENT.CONOPARENT);
            return View(cSCOPARENT);
        }

        // GET: CSCOPARENTs/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSCOPARENT cSCOPARENT = db.CSCOPARENTs.Find(id);
            if (cSCOPARENT == null)
            {
                return HttpNotFound();
            }
            return View(cSCOPARENT);
        }

        // POST: CSCOPARENTs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            CSCOPARENT cSCOPARENT = db.CSCOPARENTs.Find(id);
            db.CSCOPARENTs.Remove(cSCOPARENT);
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
