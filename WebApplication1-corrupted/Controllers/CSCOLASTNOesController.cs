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
    public class CSCOLASTNOesController : Controller
    {
        private ASIDBConnection db = new ASIDBConnection();

        // GET: CSCOLASTNOes
        public ActionResult Index()
        {
            return View(db.CSCOLASTNOes.ToList());
        }

        // GET: CSCOLASTNOes/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSCOLASTNO cSCOLASTNO = db.CSCOLASTNOes.Find(id);
            if (cSCOLASTNO == null)
            {
                return HttpNotFound();
            }
            return View(cSCOLASTNO);
        }

        // GET: CSCOLASTNOes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CSCOLASTNOes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CONO,LASTCODE,LASTDESC,LASTPFIX,LASTNO,LASTWD,AUTOGEN,STAMP")] CSCOLASTNO cSCOLASTNO)
        {
            if (ModelState.IsValid)
            {
                db.CSCOLASTNOes.Add(cSCOLASTNO);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(cSCOLASTNO);
        }

        // GET: CSCOLASTNOes/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSCOLASTNO cSCOLASTNO = db.CSCOLASTNOes.Find(id);
            if (cSCOLASTNO == null)
            {
                return HttpNotFound();
            }
            return View(cSCOLASTNO);
        }

        // POST: CSCOLASTNOes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CONO,LASTCODE,LASTDESC,LASTPFIX,LASTNO,LASTWD,AUTOGEN,STAMP")] CSCOLASTNO cSCOLASTNO)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cSCOLASTNO).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cSCOLASTNO);
        }

        // GET: CSCOLASTNOes/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSCOLASTNO cSCOLASTNO = db.CSCOLASTNOes.Find(id);
            if (cSCOLASTNO == null)
            {
                return HttpNotFound();
            }
            return View(cSCOLASTNO);
        }

        // POST: CSCOLASTNOes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            CSCOLASTNO cSCOLASTNO = db.CSCOLASTNOes.Find(id);
            db.CSCOLASTNOes.Remove(cSCOLASTNO);
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
