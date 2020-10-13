using PagedList;
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


    public class CSCOMSTRsController : Controller
    {
        private ASIDBConnection db = new ASIDBConnection();

        public PartialViewResult Search()
        {
            CSCOMSTR searchRec = null;
            //searchRec.CONO = pSearchCode;
            //searchRec.CONAME = pSearchName;
            if (Session["SearchRec"] != null)
            {
                searchRec = (CSCOMSTR)Session["SearchRec"];
            }
            else
            {
                searchRec = new CSCOMSTR();
            }
            ViewBag.STAFFCODE = new SelectList(db.HKSTAFFs, "STAFFCODE", "STAFFDESC");
            return PartialView("Partial/Search",searchRec);
        }

        [HttpPost]
        public ActionResult SearchPost([Bind(Include = "CONO,CONAME,STAFFCODE")] CSCOMSTR cSCOMSTR)
        {

            Session["SearchRec"] = cSCOMSTR;
            return Index(1);
        }

        // GET: CSCOMSTRs
        public ActionResult Index(int? page)
        {
            string pSearchCode = "";
            string pSearchName = "";
            string pSearchStaff = "";
            if (Session["SearchRec"] != null)
            {
                CSCOMSTR searchRec = (CSCOMSTR)(Session["SearchRec"]);
                pSearchCode = searchRec.CONO ?? "";
                pSearchName = searchRec.CONAME ?? "";
                pSearchStaff = searchRec.STAFFCODE ?? "";
            }

            IQueryable<CSCOMSTR> cSCOMSTRs = db.CSCOMSTRs;
            if (pSearchCode != "") { cSCOMSTRs = cSCOMSTRs.Where(x => x.CONO.Contains(pSearchCode)); };
            if (pSearchName != "") { cSCOMSTRs = cSCOMSTRs.Where(x => x.CONAME.Contains(pSearchName)); };
            if (pSearchStaff != "") { cSCOMSTRs = cSCOMSTRs.Where(x => x.STAFFCODE == pSearchStaff); };

            var curRec = cSCOMSTRs.OrderBy(n => n.CONAME).Include(c => c.HKCTRY).Include(c => c.HKCONST).Include(c => c.HKSTAFF);
            return View("Index", curRec.ToList().ToPagedList(page ?? 1, 30));
            //return View("IndexNG2", cSCOMSTRs.ToList());
        }

        // GET: CSCOMSTRs/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSCOMSTR cSCOMSTR = db.CSCOMSTRs.Find(MyHtmlHelpers.ConvertByteStrToId(id));
            if (cSCOMSTR == null)
            {
                return HttpNotFound();
            }
            return View(cSCOMSTR);
        }

        // GET: CSCOMSTRs/Create
        public ActionResult Create()
        {
            CSCOMSTR cSCOMSTR = new CSCOMSTR();
            ViewBag.INCCTRY = new SelectList(db.HKCTRies, "CTRYCODE", "CTRYDESC");
            ViewBag.CONSTCODE = new SelectList(db.HKCONSTs, "CONSTCODE", "CONSTDESC");
            ViewBag.STAFFCODE = new SelectList(db.HKSTAFFs, "STAFFCODE", "STAFFDESC");
            cSCOMSTR.STAMP = 1; // set initial stamp
            cSCOMSTR.SEQNO = 1; // set inital seqno
            return View("Create1", cSCOMSTR);
        }

        // POST: CSCOMSTRs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CONO,INCDATE,INCCTRY,CONSTCODE,INTYPE,PRINOBJ,PRINOBJStr,CONAME,PINDCODE,SINDCODE,WEB,COSTAT,COSTATD,FILETYPE,FILELOC,SEALLOC,STAFFCODE,SPECIALRE,SPECIALREBool,ARRE,ARREBool,CMMT,CMMTStr,REM,SXCODE,SXNAME,REFCODE,SEQNO,STAMP")] CSCOMSTR cSCOMSTR)
        {
            if (ModelState.IsValid)
            {
                cSCOMSTR.STAMP = 1; // set initial stamp
                cSCOMSTR.SEQNO = 1; // set inital seqno
                db.CSCOMSTRs.Add(cSCOMSTR);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.INCCTRY = new SelectList(db.HKCTRies, "CTRYCODE", "CTRYDESC", cSCOMSTR.INCCTRY);
            ViewBag.CONSTCODE = new SelectList(db.HKCONSTs, "CONSTCODE", "CONSTDESC", cSCOMSTR.CONSTCODE);
            ViewBag.STAFFCODE = new SelectList(db.HKSTAFFs, "STAFFCODE", "STAFFDESC", cSCOMSTR.STAFFCODE);
            return View(cSCOMSTR);
        }

        // GET: CSCOMSTRs/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSCOMSTR cSCOMSTR = db.CSCOMSTRs.Find(MyHtmlHelpers.ConvertByteStrToId(id));
            if (cSCOMSTR == null)
            {
                return HttpNotFound();
            }
            ViewBag.INCCTRY = new SelectList(db.HKCTRies, "CTRYCODE", "CTRYDESC", cSCOMSTR.INCCTRY);
            ViewBag.CONSTCODE = new SelectList(db.HKCONSTs, "CONSTCODE", "CONSTDESC", cSCOMSTR.CONSTCODE);
            ViewBag.STAFFCODE = new SelectList(db.HKSTAFFs, "STAFFCODE", "STAFFDESC", cSCOMSTR.STAFFCODE);
            return View("EditNG",cSCOMSTR);
        }

        // POST: CSCOMSTRs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CONO,INCDATE,INCCTRY,CONSTCODE,INTYPE,PRINOBJ,PRINOBJStr,CONAME,PINDCODE,SINDCODE,WEB,COSTAT,COSTATD,FILETYPE,FILELOC,SEALLOC,STAFFCODE,SPECIALRE,SPECIALREBool,ARRE,ARREBool,CMMT,CMMTStr,REM,SXCODE,SXNAME,REFCODE,SEQNO,STAMP")] CSCOMSTR cSCOMSTR)
        {
            if (ModelState.IsValid)
            {
                ASIDBConnection newdb = new ASIDBConnection();
                db.Entry(cSCOMSTR).State = EntityState.Modified;
                try
                {

                    CSCOMSTR curRec = newdb.CSCOMSTRs.Find(cSCOMSTR.CONO);
                    if (curRec.STAMP == cSCOMSTR.STAMP)
                    {
                        cSCOMSTR.STAMP = cSCOMSTR.STAMP + 1;
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
            ViewBag.INCCTRY = new SelectList(db.HKCTRies, "CTRYCODE", "CTRYDESC", cSCOMSTR.INCCTRY);
            ViewBag.CONSTCODE = new SelectList(db.HKCONSTs, "CONSTCODE", "CONSTDESC", cSCOMSTR.CONSTCODE);
            ViewBag.STAFFCODE = new SelectList(db.HKSTAFFs, "STAFFCODE", "STAFFDESC", cSCOMSTR.STAFFCODE);
            return View(cSCOMSTR);
        }

        // GET: CSCOMSTRs/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSCOMSTR cSCOMSTR = db.CSCOMSTRs.Find(MyHtmlHelpers.ConvertByteStrToId(id));
            if (cSCOMSTR == null)
            {
                return HttpNotFound();
            }
            ViewBag.INCCTRY = new SelectList(db.HKCTRies, "CTRYCODE", "CTRYDESC", cSCOMSTR.INCCTRY);
            ViewBag.CONSTCODE = new SelectList(db.HKCONSTs, "CONSTCODE", "CONSTDESC", cSCOMSTR.CONSTCODE);
            ViewBag.STAFFCODE = new SelectList(db.HKSTAFFs, "STAFFCODE", "STAFFDESC", cSCOMSTR.STAFFCODE);
            return View(cSCOMSTR);
        }

        // POST: CSCOMSTRs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            CSCOMSTR cSCOMSTR = db.CSCOMSTRs.Find(MyHtmlHelpers.ConvertByteStrToId(id));
            db.CSCOMSTRs.Remove(cSCOMSTR);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public PartialViewResult PartialCSCOSTATs(string id)
        {
            string sid = MyHtmlHelpers.ConvertByteStrToId(id);
            ViewBag.Parent = sid;
            var csCOSTAT = db.CSCOSTATs.Where(x => x.CONO == sid).OrderByDescending(y => y.ROWNO).ToList();
            return PartialView("Partial/PartialCSCOSTATs", csCOSTAT);
        }

        public PartialViewResult PartialCSCOFEEs(string id)
        {
            string sid = MyHtmlHelpers.ConvertByteStrToId(id);
            ViewBag.Parent = sid;
            var csCOFEE = db.CSCOFEEs.Where(x => x.CONO == sid).OrderByDescending(y => y.LASTTOUCH).ToList();
            return PartialView("Partial/PartialCSCOFEEs", csCOFEE);
        }

        public PartialViewResult PartialCSCOPICs(string id)
        {
            string sid = MyHtmlHelpers.ConvertByteStrToId(id);
            ViewBag.Parent = sid;
            var csCOPIC = db.CSCOPICs.Where(x => x.CONO == sid).OrderBy(y => y.CSPR.PRSNAME).ToList();
            return PartialView("Partial/PartialCSCOPICs", csCOPIC);
        }

        public PartialViewResult PartialCSCOAGMs(string id)
        {
            string sid = MyHtmlHelpers.ConvertByteStrToId(id);
            ViewBag.Parent = sid;
            var csCOAGM = db.CSCOAGMs.Where(x => x.CONO == sid).OrderByDescending(y => y.AGMNO).ToList();
            return PartialView("Partial/PartialCSCOAGMs", csCOAGM);
        }

        public PartialViewResult PartialCSCOARs(string id)
        {
            string sid = MyHtmlHelpers.ConvertByteStrToId(id);
            ViewBag.Parent = sid;
            var csCOAR = db.CSCOARs.Where(x => x.CONO == sid).OrderByDescending(y => y.ARNO).ToList();
            return PartialView("Partial/PartialCSCOARs", csCOAR);
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
