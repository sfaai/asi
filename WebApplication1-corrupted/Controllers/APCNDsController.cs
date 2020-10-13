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
    public class APCNDsController : Controller
    {
        private ASIDBConnection db = new ASIDBConnection();

        // GET: APCNDs
        public ActionResult Index()
        {
            return View(db.APCNDs.ToList());
        }

        // GET: APCNDs/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            APCND aPCND = db.APCNDs.Find(id);
            if (aPCND == null)
            {
                return HttpNotFound();
            }
            return View(aPCND);
        }

        // GET: APCNDs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: APCNDs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TRNO,TRID,VDATE,CLCODE,CNNO,CNDATE,ITEMDESC,CRMAPCODE,BRANCHCODE,UNITCODE,PRJCODE,STAFFCODE,CURRCODE,ITEMAMT,STAMP")] APCND aPCND)
        {
            if (ModelState.IsValid)
            {
                db.APCNDs.Add(aPCND);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(aPCND);
        }

        // GET: APCNDs/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            APCND aPCND = db.APCNDs.Find(id);
            if (aPCND == null)
            {
                return HttpNotFound();
            }
            return View(aPCND);
        }

        // POST: APCNDs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TRNO,TRID,VDATE,CLCODE,CNNO,CNDATE,ITEMDESC,CRMAPCODE,BRANCHCODE,UNITCODE,PRJCODE,STAFFCODE,CURRCODE,ITEMAMT,STAMP")] APCND aPCND)
        {
            if (ModelState.IsValid)
            {
                db.Entry(aPCND).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(aPCND);
        }

        // GET: APCNDs/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            APCND aPCND = db.APCNDs.Find(id);
            if (aPCND == null)
            {
                return HttpNotFound();
            }
            return View(aPCND);
        }

        // POST: APCNDs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            APCND aPCND = db.APCNDs.Find(id);
            db.APCNDs.Remove(aPCND);
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
