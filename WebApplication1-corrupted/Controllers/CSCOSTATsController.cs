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
    public class CSCOSTATsController : Controller
    {
        private ASIDBConnection db = new ASIDBConnection();


        // GET: CSCOSTATs
        public ActionResult Index()
        {
            return View(db.CSCOSTATs.OrderByDescending(x => x.ROWNO).ToList());
        }

        // GET: CSCOSTATs/Details/5
        public ActionResult Details(string id, int row)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSCOSTAT cSCOSTAT = db.CSCOSTATs.Find(MyHtmlHelpers.ConvertByteStrToId(id), row);
            if (cSCOSTAT == null)
            {
                return HttpNotFound();
            }
            return View(cSCOSTAT);
        }

        // GET: CSCOSTATs/Create
        public ActionResult Create(string id)
        {
            ViewBag.Parent = MyHtmlHelpers.ConvertByteStrToId(id);
            CSCOSTAT curRec = new CSCOSTAT();
            curRec.CONO = ViewBag.Parent;
            curRec.SDATE = DateTime.Now;

            ViewBag.COSTAT = new SelectList(db.CSSTATs, "COSTAT", "COSTAT", curRec.COSTAT);
            return View(curRec);
        }

        // POST: CSCOSTATs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CONO,SDATE,COSTAT,FILETYPE,FILELOC,SEALLOC,EDATE,ROWNO,STAMP")] CSCOSTAT cSCOSTAT)
        {
            if (ModelState.IsValid)
            {
                int lastRowNo = 0;
                try
                {
                    lastRowNo = db.CSCOSTATs.Where(m => m.CONO == cSCOSTAT.CONO).Max(n => n.ROWNO);
                }
                catch (Exception e) { lastRowNo = -1; }
                finally { };

                cSCOSTAT.STAMP = 1;
                cSCOSTAT.EDATE = cSCOSTAT.SDATE.AddDays(30000);
                cSCOSTAT.ROWNO = lastRowNo + 1;

                db.CSCOSTATs.Add(cSCOSTAT);

                UpdatePreviousRow(cSCOSTAT);

                db.SaveChanges();
                //return RedirectToAction("Details","CSCOMSTRs", new { id = MyHtmlHelpers.ConvertIdToByteStr(cSCOSTAT.CONO) });
                return new RedirectResult(Url.Action("Details", "CSCOMSTRs", new { id = MyHtmlHelpers.ConvertIdToByteStr(cSCOSTAT.CONO) }) + "#Status");
            }

            return View(cSCOSTAT);
        }

        // GET: CSCOSTATs/Edit/5
        public ActionResult Edit(string id, int row)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSCOSTAT cSCOSTAT = db.CSCOSTATs.Find(MyHtmlHelpers.ConvertByteStrToId(id), row);
            if (cSCOSTAT == null)
            {
                return HttpNotFound();
            };


            ViewBag.COSTAT = new SelectList(db.CSSTATs, "COSTAT", "COSTAT", cSCOSTAT.COSTAT);
            return View(cSCOSTAT);
        }


        public ActionResult EditFileLoc(CSCOSTAT cSCOSTAT)
        {

            CSSTAT csStat = db.CSSTATs.Find(cSCOSTAT.COSTAT);
            //ModelState.Clear(); //apparent this has side-effects and using Remove is preferrable
            ModelState.Remove("FILETYPE");
            ModelState.Remove("FILELOC");
            ModelState.Remove("SEALLOC");

            cSCOSTAT.FILETYPE = csStat.FILENOPFIX;
            if ((csStat.FILENOFR == 0) && (csStat.FILENOTO == 0) && (csStat.SEALNOFR == 0) && (csStat.SEALNOTO == 0)) { cSCOSTAT.FILELOC = ""; return PartialView("Partial/EditNoFileSeal", cSCOSTAT); };
            if ((csStat.FILENOFR == 0) && (csStat.FILENOTO == 0)) { cSCOSTAT.FILELOC = ""; return PartialView("Partial/EditSealOnly", cSCOSTAT); };
            if ((csStat.SEALNOFR == 0) && (csStat.SEALNOTO == 0)) { cSCOSTAT.SEALLOC = ""; return PartialView("Partial/EditFileOnly", cSCOSTAT); };

            //db.CSCOSTATs.Attach(cSCOSTAT);
            //EntityState st = db.Entry(cSCOSTAT).State;
            //db.Entry(cSCOSTAT).State = EntityState.Modified;

            return PartialView("Partial/EditFileLoc", cSCOSTAT);


        }

        // POST: CSCOSTATs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CONO,SDATE,COSTAT,FILETYPE,FILELOC,SEALLOC,EDATE,ROWNO,STAMP")] CSCOSTAT cSCOSTAT)
        {
            if (ModelState.IsValid)
            {
                ASIDBConnection newdb = new ASIDBConnection();
                try
                {
                    CSCOSTAT curRec = newdb.CSCOSTATs.Find(cSCOSTAT.CONO, cSCOSTAT.ROWNO);
                    if (curRec.STAMP == cSCOSTAT.STAMP)
                    {
                        cSCOSTAT.STAMP = cSCOSTAT.STAMP + 1;



                        UpdatePreviousRow(cSCOSTAT);

                        db.Entry(cSCOSTAT).State = EntityState.Modified;
                        db.SaveChanges();
                        // return RedirectToAction("Details", "CSCOMSTRs", new { id = MyHtmlHelpers.ConvertIdToByteStr(cSCOSTAT.CONO) });
                        return new RedirectResult(Url.Action("Details", "CSCOMSTRs", new { id = MyHtmlHelpers.ConvertIdToByteStr(cSCOSTAT.CONO) }) + "#Status");
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
            ViewBag.COSTAT = new SelectList(db.CSSTATs, "COSTAT", "COSTAT", cSCOSTAT.COSTAT);
            return View(cSCOSTAT);
        }

        private void UpdatePreviousRow(CSCOSTAT cSCOSTAT)
        {
            CSCOSTAT curRec = db.CSCOSTATs.Where(m => m.CONO == cSCOSTAT.CONO && m.ROWNO < cSCOSTAT.ROWNO).OrderByDescending(n => n.ROWNO).FirstOrDefault();
            if (curRec != null)
            {
                System.DateTime lastDate = (cSCOSTAT.SDATE).AddDays(-1);

                curRec.EDATE = lastDate;
                curRec.STAMP = curRec.STAMP + 1;
                db.Entry(curRec).State = EntityState.Modified;
            }
        }

        // GET: CSCOSTATs/Delete/5
        public ActionResult Delete(string id, int row)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSCOSTAT cSCOSTAT = db.CSCOSTATs.Find(MyHtmlHelpers.ConvertByteStrToId(id), row);
            if (cSCOSTAT == null)
            {
                return HttpNotFound();
            }
            return View(cSCOSTAT);
        }

        // POST: CSCOSTATs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id, int row)
        {
            CSCOSTAT cSCOSTAT = db.CSCOSTATs.Find(MyHtmlHelpers.ConvertByteStrToId(id), row);
            db.CSCOSTATs.Remove(cSCOSTAT);
            db.SaveChanges();
            // return RedirectToAction("Details", "CSCOMSTRs", new { id = MyHtmlHelpers.ConvertIdToByteStr(cSCOSTAT.CONO) });
            return new RedirectResult(Url.Action("Details", "CSCOMSTRs", new { id = MyHtmlHelpers.ConvertIdToByteStr(cSCOSTAT.CONO) }) + "#Status");
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
