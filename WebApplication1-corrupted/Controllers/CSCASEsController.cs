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
    public class CSCASEsController : Controller
    {
        private ASIDBConnection db = new ASIDBConnection();

        // GET: CSCASEs
        public ActionResult Index()
        {
            return View(db.CSCASEs.OrderBy(n => n.CASECODE).ToList());
        }

        // GET: CSCASEs/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSCASE cSCASE = db.CSCASEs.Find(MyHtmlHelpers.ConvertByteStrToId(id));
            if (cSCASE == null)
            {
                return HttpNotFound();
            }
            return View(cSCASE);
        }

        // GET: CSCASEs/Create
        public ActionResult Create()
        {
            CSCASE curRec = new CSCASE();
            curRec.STAMP = 1;
            return View(curRec);
        }

        // POST: CSCASEs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CASECODE,CASEDESC,MAPSFIX,STAMP")] CSCASE cSCASE)
        {
            if (ModelState.IsValid)
            {
                db.CSCASEs.Add(cSCASE);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(cSCASE);
        }

        // GET: CSCASEs/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            string sid = MyHtmlHelpers.ConvertByteStrToId(id);
            CSCASE cSCASE = db.CSCASEs.Find(sid);
            if (cSCASE == null)
            {
                return HttpNotFound();
            }
            return View(cSCASE);
        }

        // POST: CSCASEs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CASECODE,CASEDESC,MAPSFIX,STAMP")] CSCASE cSCASE)
        {
            if (ModelState.IsValid)
            {
                ASIDBConnection newdb = new ASIDBConnection();
                try
                {
                    CSCASE curRec = newdb.CSCASEs.Find(cSCASE.CASECODE);
                    if (curRec.STAMP == cSCASE.STAMP)
                    {
                        cSCASE.STAMP = cSCASE.STAMP + 1;
                        db.Entry(cSCASE).State = EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    else { ModelState.AddModelError(string.Empty, "Record is modified"); }

                }

                catch (Exception e)
                {
                    ModelState.AddModelError(string.Empty, e.Message);
                }
                finally
                {
                    newdb.Dispose();
                }
            }
            return View(cSCASE);
        }

        // GET: CSCASEs/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSCASE cSCASE = db.CSCASEs.Find(MyHtmlHelpers.ConvertByteStrToId(id));
            if (cSCASE == null)
            {
                return HttpNotFound();
            }
            return View(cSCASE);
        }

        // POST: CSCASEs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            CSCASE cSCASE = db.CSCASEs.Find(MyHtmlHelpers.ConvertByteStrToId(id));
            db.CSCASEs.Remove(cSCASE);
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
