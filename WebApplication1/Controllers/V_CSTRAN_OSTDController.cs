using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication1;
using PagedList;

namespace WebApplication1.Controllers
{
    public class V_CSTRAN_OSTDController : Controller
    {
        private ASIDBConnection db = new ASIDBConnection();

        // GET: V_CSTRAN_OSTD
        public ActionResult pIndex(int? page)
        {
            return View( db.V_CSTRAN_OSTD.ToList().ToPagedList(page ?? 1, 30));
        }

        // GET: V_CSTRAN_OSTD/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            V_CSTRAN_OSTD v_CSTRAN_OSTD = db.V_CSTRAN_OSTD.Find(id);
            if (v_CSTRAN_OSTD == null)
            {
                return HttpNotFound();
            }
            return View(v_CSTRAN_OSTD);
        }

        // GET: V_CSTRAN_OSTD/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: V_CSTRAN_OSTD/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CONAME,CONO,COREGNO,ENTDATE,PARTICULARS,PARTICULARS1,PARTICULARS2,SOURCE,SOURCENO,TRAMT,TROS,MINAMT,MAXAMT")] V_CSTRAN_OSTD v_CSTRAN_OSTD)
        {
            if (ModelState.IsValid)
            {
                db.V_CSTRAN_OSTD.Add(v_CSTRAN_OSTD);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(v_CSTRAN_OSTD);
        }

        // GET: V_CSTRAN_OSTD/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            V_CSTRAN_OSTD v_CSTRAN_OSTD = db.V_CSTRAN_OSTD.Find(id);
            if (v_CSTRAN_OSTD == null)
            {
                return HttpNotFound();
            }
            return View(v_CSTRAN_OSTD);
        }

        // POST: V_CSTRAN_OSTD/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CONAME,CONO,COREGNO,ENTDATE,PARTICULARS,PARTICULARS1,PARTICULARS2,SOURCE,SOURCENO,TRAMT,TROS,MINAMT,MAXAMT")] V_CSTRAN_OSTD v_CSTRAN_OSTD)
        {
            if (ModelState.IsValid)
            {
                db.Entry(v_CSTRAN_OSTD).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(v_CSTRAN_OSTD);
        }

        // GET: V_CSTRAN_OSTD/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            V_CSTRAN_OSTD v_CSTRAN_OSTD = db.V_CSTRAN_OSTD.Find(id);
            if (v_CSTRAN_OSTD == null)
            {
                return HttpNotFound();
            }
            return View(v_CSTRAN_OSTD);
        }

        // POST: V_CSTRAN_OSTD/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            V_CSTRAN_OSTD v_CSTRAN_OSTD = db.V_CSTRAN_OSTD.Find(id);
            db.V_CSTRAN_OSTD.Remove(v_CSTRAN_OSTD);
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
