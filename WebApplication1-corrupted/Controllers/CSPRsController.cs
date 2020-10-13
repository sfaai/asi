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
    public class CSPRsController : Controller
    {
        private ASIDBConnection db = new ASIDBConnection();

        // GET: CSPRs
        public ActionResult Index()
        {
            var cSPRS = db.CSPRS.Include(c => c.HKCONST).Include(c => c.HKTITLE).Include(c => c.HKNATION).Include(c => c.HKRACE).Include(c => c.HKCTRY);
            return View(cSPRS.ToList());
        }

        // GET: CSPRs/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSPR cSPR = db.CSPRS.Find(id);
            if (cSPR == null)
            {
                return HttpNotFound();
            }
            return View(cSPR);
        }

        // GET: CSPRs/Create
        public ActionResult Create()
        {
            ViewBag.CONSTCODE = new SelectList(db.HKCONSTs, "CONSTCODE", "CONSTDESC");
            ViewBag.PRSTITLE = new SelectList(db.HKTITLEs, "TITLE", "TITLE");
            ViewBag.NATION = new SelectList(db.HKNATIONs, "NATION", "NATION");
            ViewBag.RACE = new SelectList(db.HKRACEs, "RACE", "RACE");
            ViewBag.CTRYINC = new SelectList(db.HKCTRies, "CTRYCODE", "CTRYDESC");
            return View();
        }

        // POST: CSPRs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PRSCODE,CONSTCODE,PRSNAME,PRSTITLE,NATION,RACE,SEX,DOB,CTRYINC,MOBILE1,MOBILE2,EMAIL,OCCUPATION,REM,STAMP")] CSPR cSPR)
        {
            if (ModelState.IsValid)
            {
                db.CSPRS.Add(cSPR);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CONSTCODE = new SelectList(db.HKCONSTs, "CONSTCODE", "CONSTDESC", cSPR.CONSTCODE);
            ViewBag.PRSTITLE = new SelectList(db.HKTITLEs, "TITLE", "TITLE", cSPR.PRSTITLE);
            ViewBag.NATION = new SelectList(db.HKNATIONs, "NATION", "NATION", cSPR.NATION);
            ViewBag.RACE = new SelectList(db.HKRACEs, "RACE", "RACE", cSPR.RACE);
            ViewBag.CTRYINC = new SelectList(db.HKCTRies, "CTRYCODE", "CTRYDESC", cSPR.CTRYINC);
            return View(cSPR);
        }

        // GET: CSPRs/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSPR cSPR = db.CSPRS.Find(id);
            if (cSPR == null)
            {
                return HttpNotFound();
            }
            ViewBag.CONSTCODE = new SelectList(db.HKCONSTs, "CONSTCODE", "CONSTDESC", cSPR.CONSTCODE);
            ViewBag.PRSTITLE = new SelectList(db.HKTITLEs, "TITLE", "TITLE", cSPR.PRSTITLE);
            ViewBag.NATION = new SelectList(db.HKNATIONs, "NATION", "NATION", cSPR.NATION);
            ViewBag.RACE = new SelectList(db.HKRACEs, "RACE", "RACE", cSPR.RACE);
            ViewBag.CTRYINC = new SelectList(db.HKCTRies, "CTRYCODE", "CTRYDESC", cSPR.CTRYINC);
            return View(cSPR);
        }

        // POST: CSPRs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PRSCODE,CONSTCODE,PRSNAME,PRSTITLE,NATION,RACE,SEX,DOB,CTRYINC,MOBILE1,MOBILE2,EMAIL,OCCUPATION,REM,STAMP")] CSPR cSPR)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cSPR).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CONSTCODE = new SelectList(db.HKCONSTs, "CONSTCODE", "CONSTDESC", cSPR.CONSTCODE);
            ViewBag.PRSTITLE = new SelectList(db.HKTITLEs, "TITLE", "TITLE", cSPR.PRSTITLE);
            ViewBag.NATION = new SelectList(db.HKNATIONs, "NATION", "NATION", cSPR.NATION);
            ViewBag.RACE = new SelectList(db.HKRACEs, "RACE", "RACE", cSPR.RACE);
            ViewBag.CTRYINC = new SelectList(db.HKCTRies, "CTRYCODE", "CTRYDESC", cSPR.CTRYINC);
            return View(cSPR);
        }

        // GET: CSPRs/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSPR cSPR = db.CSPRS.Find(id);
            if (cSPR == null)
            {
                return HttpNotFound();
            }
            return View(cSPR);
        }

        // POST: CSPRs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            CSPR cSPR = db.CSPRS.Find(id);
            db.CSPRS.Remove(cSPR);
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
