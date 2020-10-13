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
    public class CSTRANMsController : Controller
    {
        private ASIDBConnection db = new ASIDBConnection();

        // GET: CSTRANMs
        public ActionResult Index()
        {
            return View(db.CSTRANMs.ToList());
        }

        // GET: CSTRANMs/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSTRANM cSTRANM = db.CSTRANMs.Find(id);
            if (cSTRANM == null)
            {
                return HttpNotFound();
            }
            return View(cSTRANM);
        }

        // GET: CSTRANMs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CSTRANMs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SOURCE,SOURCENO,SOURCEID,CONO,JOBNO,CASENO,CASECODE,DUEDATE,ENTDATE,TRTYPE,TRDESC,TRITEM1,TRITEM2,TRITEM,TRTAX1,TRTAX2,TRTAX,TRAMT1,TRAMT2,TRAMT,TRSIGN,TRSITEM1,TRSITEM2,TRSITEM,TRSTAX1,TRSTAX2,TRSTAX,TRSAMT1,TRSAMT2,TRSAMT,TRITEMOS1,TRITEMOS2,TRITEMOS,TRTAXOS1,TRTAXOS2,TRTAXOS,TROS1,TROS2,TROS,APPTYPE,APPNO,APPID,APPITEM1,APPITEM2,APPITEM,APPTAX1,APPTAX2,APPTAX,APPAMT1,APPAMT2,APPAMT,REM,COMPLETE,COMPLETED,SEQNO,REFCNT,STAMP")] CSTRANM cSTRANM)
        {
            if (ModelState.IsValid)
            {
                db.CSTRANMs.Add(cSTRANM);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(cSTRANM);
        }

        // GET: CSTRANMs/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSTRANM cSTRANM = db.CSTRANMs.Find(id);
            if (cSTRANM == null)
            {
                return HttpNotFound();
            }
            return View(cSTRANM);
        }

        // POST: CSTRANMs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SOURCE,SOURCENO,SOURCEID,CONO,JOBNO,CASENO,CASECODE,DUEDATE,ENTDATE,TRTYPE,TRDESC,TRITEM1,TRITEM2,TRITEM,TRTAX1,TRTAX2,TRTAX,TRAMT1,TRAMT2,TRAMT,TRSIGN,TRSITEM1,TRSITEM2,TRSITEM,TRSTAX1,TRSTAX2,TRSTAX,TRSAMT1,TRSAMT2,TRSAMT,TRITEMOS1,TRITEMOS2,TRITEMOS,TRTAXOS1,TRTAXOS2,TRTAXOS,TROS1,TROS2,TROS,APPTYPE,APPNO,APPID,APPITEM1,APPITEM2,APPITEM,APPTAX1,APPTAX2,APPTAX,APPAMT1,APPAMT2,APPAMT,REM,COMPLETE,COMPLETED,SEQNO,REFCNT,STAMP")] CSTRANM cSTRANM)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cSTRANM).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cSTRANM);
        }

        // GET: CSTRANMs/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSTRANM cSTRANM = db.CSTRANMs.Find(id);
            if (cSTRANM == null)
            {
                return HttpNotFound();
            }
            return View(cSTRANM);
        }

        // POST: CSTRANMs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            CSTRANM cSTRANM = db.CSTRANMs.Find(id);
            db.CSTRANMs.Remove(cSTRANM);
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
