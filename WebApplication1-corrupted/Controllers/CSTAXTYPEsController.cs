using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Utility;

namespace WebApplication1.Controllers
{
    public class CSTAXTYPEsController : Controller
    {
        private ASIDBConnection db = new ASIDBConnection();

        // GET: CSTAXTYPEs
        public ActionResult Index()
        {
            return View(db.CSTAXTYPEs.ToList());
        }

        // GET: CSTAXTYPEs/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSTAXTYPE cSTAXTYPE = db.CSTAXTYPEs.Find(MyHtmlHelpers.ConvertByteStrToId(id));
            if (cSTAXTYPE == null)
            {
                return HttpNotFound();
            }
            return View(cSTAXTYPE);
        }

        // GET: CSTAXTYPEs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CSTAXTYPEs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TAXCODE,TAXDESC,TAXTYPE,TAXRCODE,TAXRATE,EFFECTIVE_START,EFFECTIVE_END,ACTIVE_FLAG,STAMP")] CSTAXTYPE cSTAXTYPE)
        {
            if (ModelState.IsValid)
            {
                db.CSTAXTYPEs.Add(cSTAXTYPE);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(cSTAXTYPE);
        }

        // GET: CSTAXTYPEs/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSTAXTYPE cSTAXTYPE = db.CSTAXTYPEs.Find(MyHtmlHelpers.ConvertByteStrToId(id));
            if (cSTAXTYPE == null)
            {
                return HttpNotFound();
            }
            return View(cSTAXTYPE);
        }

        // POST: CSTAXTYPEs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TAXCODE,TAXDESC,TAXTYPE,TAXRCODE,TAXRATE,EFFECTIVE_START,EFFECTIVE_END,ACTIVE_FLAG,STAMP")] CSTAXTYPE cSTAXTYPE)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cSTAXTYPE).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cSTAXTYPE);
        }

        // GET: CSTAXTYPEs/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSTAXTYPE cSTAXTYPE = db.CSTAXTYPEs.Find(MyHtmlHelpers.ConvertByteStrToId(id));
            if (cSTAXTYPE == null)
            {
                return HttpNotFound();
            }
            return View(cSTAXTYPE);
        }

        // POST: CSTAXTYPEs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            CSTAXTYPE cSTAXTYPE = db.CSTAXTYPEs.Find(MyHtmlHelpers.ConvertByteStrToId(id));
            db.CSTAXTYPEs.Remove(cSTAXTYPE);
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
