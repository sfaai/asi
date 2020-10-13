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
    public class SACOMPsController : Controller
    {
        private ASIDBConnection db = new ASIDBConnection();

        // GET: SACOMPs
        public ActionResult Index()
        {
            return View(db.SACOMPs.ToList());
        }

        // GET: SACOMPs/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SACOMP sACOMP = db.SACOMPs.Find(id);
            if (sACOMP == null)
            {
                return HttpNotFound();
            }
            return View(sACOMP);
        }

        // GET: SACOMPs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SACOMPs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CONAME,COREGNO,COADDR1,COADDR2,COADDR3,COADDR4,COPHONE1,COPHONE2,COFAX1,COFAX2,COWEB,CTRYOPR,ACCMETHOD,STAMP")] SACOMP sACOMP)
        {
            if (ModelState.IsValid)
            {
                db.SACOMPs.Add(sACOMP);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(sACOMP);
        }

        // GET: SACOMPs/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SACOMP sACOMP = db.SACOMPs.Find(id);
            if (sACOMP == null)
            {
                return HttpNotFound();
            }
            return View(sACOMP);
        }

        // POST: SACOMPs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CONAME,COREGNO,COADDR1,COADDR2,COADDR3,COADDR4,COPHONE1,COPHONE2,COFAX1,COFAX2,COWEB,CTRYOPR,ACCMETHOD,STAMP")] SACOMP sACOMP)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sACOMP).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(sACOMP);
        }

        // GET: SACOMPs/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SACOMP sACOMP = db.SACOMPs.Find(id);
            if (sACOMP == null)
            {
                return HttpNotFound();
            }
            return View(sACOMP);
        }

        // POST: SACOMPs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            SACOMP sACOMP = db.SACOMPs.Find(id);
            db.SACOMPs.Remove(sACOMP);
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
