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
    public class CSCOADTsController : Controller
    {
        private ASIDBConnection db = new ASIDBConnection();

        // GET: CSCOADTs
        public ActionResult Index()
        {
            var cSCOADTs = db.CSCOADTs.Include(c => c.CSADT).Include(c => c.CSPR);
            return View(cSCOADTs.ToList());
        }

        // GET: CSCOADTs/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSCOADT cSCOADT = db.CSCOADTs.Find(id);
            if (cSCOADT == null)
            {
                return HttpNotFound();
            }
            return View(cSCOADT);
        }

        // GET: CSCOADTs/Create
        public ActionResult Create()
        {
            ViewBag.ADTCODE = new SelectList(db.CSADTs, "ADTCODE", "ADTDESC");
            ViewBag.PRSCODE = new SelectList(db.CSPRS, "PRSCODE", "CONSTCODE");
            return View();
        }

        // POST: CSCOADTs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CONO,ADTCODE,PRSCODE,ADATE,RDATE,ENDDATE,REM,ROWNO,STAMP")] CSCOADT cSCOADT)
        {
            if (ModelState.IsValid)
            {
                db.CSCOADTs.Add(cSCOADT);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ADTCODE = new SelectList(db.CSADTs, "ADTCODE", "ADTDESC", cSCOADT.ADTCODE);
            ViewBag.PRSCODE = new SelectList(db.CSPRS, "PRSCODE", "CONSTCODE", cSCOADT.PRSCODE);
            return View(cSCOADT);
        }

        // GET: CSCOADTs/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSCOADT cSCOADT = db.CSCOADTs.Find(id);
            if (cSCOADT == null)
            {
                return HttpNotFound();
            }
            ViewBag.ADTCODE = new SelectList(db.CSADTs, "ADTCODE", "ADTDESC", cSCOADT.ADTCODE);
            ViewBag.PRSCODE = new SelectList(db.CSPRS, "PRSCODE", "CONSTCODE", cSCOADT.PRSCODE);
            return View(cSCOADT);
        }

        // POST: CSCOADTs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CONO,ADTCODE,PRSCODE,ADATE,RDATE,ENDDATE,REM,ROWNO,STAMP")] CSCOADT cSCOADT)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cSCOADT).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ADTCODE = new SelectList(db.CSADTs, "ADTCODE", "ADTDESC", cSCOADT.ADTCODE);
            ViewBag.PRSCODE = new SelectList(db.CSPRS, "PRSCODE", "CONSTCODE", cSCOADT.PRSCODE);
            return View(cSCOADT);
        }

        // GET: CSCOADTs/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSCOADT cSCOADT = db.CSCOADTs.Find(id);
            if (cSCOADT == null)
            {
                return HttpNotFound();
            }
            return View(cSCOADT);
        }

        // POST: CSCOADTs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            CSCOADT cSCOADT = db.CSCOADTs.Find(id);
            db.CSCOADTs.Remove(cSCOADT);
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
