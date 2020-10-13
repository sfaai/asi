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
    public class CSCOAGMsController : Controller
    {
        private ASIDBConnection db = new ASIDBConnection();

        // GET: CSCOAGMs
        public ActionResult Index()
        {
            return View(db.CSCOAGMs.ToList());
        }

        // GET: CSCOAGMs/Details/5
        public ActionResult Details(string id, int seq)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSCOAGM cSCOAGM = db.CSCOAGMs.Find(MyHtmlHelpers.ConvertByteStrToId(id), seq);
            if (cSCOAGM == null)
            {
                return HttpNotFound();
            }
            return View(cSCOAGM);
        }

        // GET: CSCOAGMs/Create
        public ActionResult Create(string id)
        {
            ViewBag.Parent = MyHtmlHelpers.ConvertByteStrToId(id);
            string sid = ViewBag.Parent;
            CSCOAGM lastRec = db.CSCOAGMs.Where(m => m.CONO == sid).OrderByDescending(n => n.AGMNO).FirstOrDefault();

            CSCOAGM curRec = new CSCOAGM();

            if (lastRec != null)
            {
                curRec = createNextYearRecord(lastRec);
            }
            else
            {
                curRec.CONO = sid;
                curRec.AGMNO = 1;
                curRec.STAMP = 1;
            };


            return View(curRec);
        }

        // POST: CSCOAGMs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CONO,AGMNO,LASTFYE,FYETOFILE,FILEDFYE,AGMLAST,AGMEXT,REMINDER1,REMINDER2,REMINDER3,AGMDATE,AGMFILED,REM,STAMP,CIRCDATE")] CSCOAGM cSCOAGM)
        {
            if (ModelState.IsValid)
            {
                db.CSCOAGMs.Add(cSCOAGM);
                db.SaveChanges();
                //return RedirectToAction("Details", "CSCOMSTRs", new { id = MyHtmlHelpers.ConvertIdToByteStr(cSCOAGM.CONO) });
                return new RedirectResult(Url.Action("Details", "CSCOMSTRs", new { id = MyHtmlHelpers.ConvertIdToByteStr(cSCOAGM.CONO) }) + "#AGM");
            }


            return View(cSCOAGM);
        }

        // GET: CSCOAGMs/Edit/5
        public ActionResult Edit(string id, int seq)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSCOAGM cSCOAGM = db.CSCOAGMs.Find(MyHtmlHelpers.ConvertByteStrToId(id), seq);
            if (cSCOAGM == null)
            {
                return HttpNotFound();
            }
            return View(cSCOAGM);
        }

        private CSCOAGM createNextYearRecord(CSCOAGM curRec)
        {
            string sid = curRec.CONO;

            CSCOAGM newRec = new CSCOAGM();
            newRec.AGMNO = (short)(curRec.AGMNO + 1); // WARNING assuming the next ARNO is available
            newRec.CONO = curRec.CONO;
            newRec.STAMP = 1;
            newRec.LASTFYE = curRec.FILEDFYE;
            newRec.FYETOFILE = curRec.FILEDFYE?.AddYears(1);
            newRec.AGMLAST = newRec.FYETOFILE?.AddMonths(6) < curRec.AGMDATE?.AddMonths(15) ? newRec.FYETOFILE?.AddMonths(6) : curRec.AGMDATE?.AddMonths(15);
            newRec.REMINDER1 = newRec.FYETOFILE?.AddMonths(-1);
            newRec.REMINDER2 = newRec.FYETOFILE?.AddMonths(3);
            newRec.REMINDER3 = newRec.FYETOFILE?.AddMonths(5);


            return newRec;
        }

        // POST: CSCOAGMs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CONO,AGMNO,LASTFYE,FYETOFILE,FILEDFYE,AGMLAST,AGMEXT,REMINDER1,REMINDER2,REMINDER3,AGMDATE,AGMFILED,REM,STAMP,CIRCDATE")] CSCOAGM cSCOAGM)
        {
            if (ModelState.IsValid)
            {
                ASIDBConnection newdb = new ASIDBConnection();
                try
                {
                    CSCOAGM curRec = newdb.CSCOAGMs.Find(cSCOAGM.CONO, cSCOAGM.AGMNO);
                    if (curRec.STAMP == cSCOAGM.STAMP)
                    {
                        cSCOAGM.STAMP = cSCOAGM.STAMP + 1;
                        db.Entry(cSCOAGM).State = EntityState.Modified;


                        if (cSCOAGM.FILEDFYE != null) // create next year record if AR is filed
                        {
                            string sid = cSCOAGM.CONO;
                            CSCOAGM lastRec = db.CSCOAGMs.Where(m => m.CONO == sid).OrderByDescending(n => n.AGMNO).FirstOrDefault();

                            if (cSCOAGM.AGMNO == lastRec.AGMNO) // add next year record only if editing the last record
                            {
                                CSCOAGM csRec = createNextYearRecord(cSCOAGM);
                                db.CSCOAGMs.Add(csRec);
                            }
                        }

                        db.SaveChanges();
                        //return RedirectToAction("Details", "CSCOMSTRs", new { id = MyHtmlHelpers.ConvertIdToByteStr(cSCOAGM.CONO) });
                        return new RedirectResult(Url.Action("Details", "CSCOMSTRs", new { id = MyHtmlHelpers.ConvertIdToByteStr(cSCOAGM.CONO) }) + "#AGM");
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
            return View(cSCOAGM);
        }

        // GET: CSCOAGMs/Delete/5
        public ActionResult Delete(string id, int seq)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSCOAGM cSCOAGM = db.CSCOAGMs.Find(MyHtmlHelpers.ConvertByteStrToId(id), seq);
            if (cSCOAGM == null)
            {
                return HttpNotFound();
            }
            return View(cSCOAGM);
        }

        // POST: CSCOAGMs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id, int seq)
        {
            CSCOAGM cSCOAGM = db.CSCOAGMs.Find(MyHtmlHelpers.ConvertByteStrToId(id), seq);
            db.CSCOAGMs.Remove(cSCOAGM);
            db.SaveChanges();
            //return RedirectToAction("Details", "CSCOMSTRs", new { id = MyHtmlHelpers.ConvertIdToByteStr(cSCOAGM.CONO) });
            return new RedirectResult(Url.Action("Details", "CSCOMSTRs", new { id = MyHtmlHelpers.ConvertIdToByteStr(cSCOAGM.CONO) }) + "#AGM");
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
