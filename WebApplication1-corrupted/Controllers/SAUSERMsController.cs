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
    public class SAUSERMsController : Controller
    {
        private ASIDBConnection db = new ASIDBConnection();

        // GET: SAUSERMs
        public ActionResult Index()
        {
            return View(db.SAUSERMs.ToList());
        }

        // GET: SAUSERMs/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SAUSERM sAUSERM = db.SAUSERMs.Find(id);
            if (sAUSERM == null)
            {
                return HttpNotFound();
            }
            return View(sAUSERM);
        }

        // GET: SAUSERMs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SAUSERMs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "USERID,USERNAME,PW,CRDATE,EXPDAYS,LUDATE,EXUSER,MAXSESSION,USERTYPE,PWMINCHAR,PWRECYCLE,PWFAILLOCK,USERLOCK,STAMP")] SAUSERM sAUSERM)
        {
            if (ModelState.IsValid)
            {
                db.SAUSERMs.Add(sAUSERM);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(sAUSERM);
        }

        // GET: SAUSERMs/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SAUSERM sAUSERM = db.SAUSERMs.Find(id);
            if (sAUSERM == null)
            {
                return HttpNotFound();
            }
            return View(sAUSERM);
        }

        // POST: SAUSERMs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "USERID,USERNAME,PW,CRDATE,EXPDAYS,LUDATE,EXUSER,MAXSESSION,USERTYPE,PWMINCHAR,PWRECYCLE,PWFAILLOCK,USERLOCK,STAMP")] SAUSERM sAUSERM)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sAUSERM).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(sAUSERM);
        }

        // GET: SAUSERMs/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SAUSERM sAUSERM = db.SAUSERMs.Find(id);
            if (sAUSERM == null)
            {
                return HttpNotFound();
            }
            return View(sAUSERM);
        }

        // POST: SAUSERMs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            SAUSERM sAUSERM = db.SAUSERMs.Find(id);
            db.SAUSERMs.Remove(sAUSERM);
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
