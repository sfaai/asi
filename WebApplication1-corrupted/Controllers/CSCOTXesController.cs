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
    public class CSCOTXesController : Controller
    {
        private ASIDBConnection db = new ASIDBConnection();

        // GET: CSCOTXes
        public ActionResult Index()
        {
            var cSCOTXes = db.CSCOTXes.Include(c => c.CSTX);
            return View(cSCOTXes.ToList());
        }

        // GET: CSCOTXes/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSCOTX cSCOTX = db.CSCOTXes.Find(id);
            if (cSCOTX == null)
            {
                return HttpNotFound();
            }
            return View(cSCOTX);
        }

        // GET: CSCOTXes/Create
        public ActionResult Create()
        {
            ViewBag.TXCODE = new SelectList(db.CSTXes, "TXCODE", "TXDESC");
            return View();
        }

        // POST: CSCOTXes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CONO,TXCODE,ADATE,RDATE,ENDDATE,REM,ROWNO,STAMP")] CSCOTX cSCOTX)
        {
            if (ModelState.IsValid)
            {
                db.CSCOTXes.Add(cSCOTX);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.TXCODE = new SelectList(db.CSTXes, "TXCODE", "TXDESC", cSCOTX.TXCODE);
            return View(cSCOTX);
        }

        // GET: CSCOTXes/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSCOTX cSCOTX = db.CSCOTXes.Find(id);
            if (cSCOTX == null)
            {
                return HttpNotFound();
            }
            ViewBag.TXCODE = new SelectList(db.CSTXes, "TXCODE", "TXDESC", cSCOTX.TXCODE);
            return View(cSCOTX);
        }

        // POST: CSCOTXes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CONO,TXCODE,ADATE,RDATE,ENDDATE,REM,ROWNO,STAMP")] CSCOTX cSCOTX)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cSCOTX).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TXCODE = new SelectList(db.CSTXes, "TXCODE", "TXDESC", cSCOTX.TXCODE);
            return View(cSCOTX);
        }

        // GET: CSCOTXes/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSCOTX cSCOTX = db.CSCOTXes.Find(id);
            if (cSCOTX == null)
            {
                return HttpNotFound();
            }
            return View(cSCOTX);
        }

        // POST: CSCOTXes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            CSCOTX cSCOTX = db.CSCOTXes.Find(id);
            db.CSCOTXes.Remove(cSCOTX);
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
