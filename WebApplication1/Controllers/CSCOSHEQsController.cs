using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Utility;
using System.Data.Entity.Core;
using System.Data.Entity.Validation;
using System.Data.Entity.Infrastructure;
using FirebirdSql.Data.Client;
using FirebirdSql.Data.FirebirdClient;

namespace WebApplication1.Controllers
{
    public class CSCOSHEQsController : Controller
    {
        private ASIDBConnection db = new ASIDBConnection();

        // GET: CSCOSHEQs
        public ActionResult Index()
        {
            var cSCOSHEQs = db.CSCOSHEQs.Include(c => c.CSCOSH);
            return View(cSCOSHEQs.ToList());
        }

        // GET: CSCOSHEQs/Details/5
        public ActionResult Details(string id, string person, string mvno, int mvid, string mvtype)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            string sid = MyHtmlHelpers.ConvertByteStrToId(id);
            CSCOSHEQ cSCOSHEQ = db.CSCOSHEQs.Find(sid, person, mvno, mvid, mvtype);
            if (cSCOSHEQ == null)
            {
                return HttpNotFound();
            }
            string ViewName = "Edit1";

            if (mvtype == "Allotment")
            {
                ViewName = "Edit";
            }
            else if (mvtype == "Transfer")
            {
                if (cSCOSHEQ.MVSIGN == "In") { ViewName = "Edit"; }
                else
                {
                    CSCOSHEQ cSTransferee = db.CSCOSHEQs.Where(x => x.CONO == sid && x.MVNO == mvno && x.MVID == 1 && x.MVTYPE == mvtype).FirstOrDefault();
                    cSCOSHEQ.FOLIONOTO = cSTransferee.CSCOSH.FOLIONO;
                    cSCOSHEQ.PRSCODETO = cSTransferee.PRSCODE;
                    cSCOSHEQ.SHAREOS = cSCOSHEQ.SHAREAMT; // revert back to original value 
                }
            }
            else if (mvtype == "Split")
            {
                if (cSCOSHEQ.MVSIGN == "In") { ViewName = "Edit"; }
                else
                {
                    cSCOSHEQ.SHAREOS = cSCOSHEQ.SHAREAMT; // revert back to original value 
                }
            }
            else if (mvtype == "Conversion")
            {
                if (cSCOSHEQ.MVSIGN == "In") { ViewName = "Edit"; }
            }

            ViewBag.PRSCODE = new SelectList(db.CSCOSHes.Where(x => x.CONO == cSCOSHEQ.CONO).Select(x => new { PRSCODE = x.PRSCODE, PRSNAME = x.CSPR.PRSNAME + " | " + x.FOLIONO }), "PRSCODE", "PRSNAME", cSCOSHEQ.PRSCODE);
            ViewBag.PRSCODETO = new SelectList(db.CSCOSHes.Where(x => x.CONO == cSCOSHEQ.CONO && x.PRSCODE != cSCOSHEQ.PRSCODE).Select(x => new { PRSCODE = x.PRSCODE, PRSNAME = x.CSPR.PRSNAME + " | " + x.FOLIONO }), "PRSCODE", "PRSNAME", cSCOSHEQ.PRSCODETO);
            ViewBag.EQCODE = new SelectList(db.CSEQs.OrderBy(x => x.EQCODE), "EQCODE", "EQDESC", cSCOSHEQ.EQCODE);
            ViewBag.EQID = new SelectList(db.CSCOSHEQs.Where(x => x.CONO == cSCOSHEQ.CONO && x.MVSIGN == "In" && x.PRSCODE == cSCOSHEQ.PRSCODE && (x.COMPLETE == "N" ||
            (x.SCRIPT == cSCOSHEQ.SCRIPT && x.CERTNO == cSCOSHEQ.CERTNO))).Select(x => new
            {
                EQID = x.EQCODE + " | " + x.SCRIPT + " | " + x.CERTNO,
                EQDESC = x.EQCODE + " | " + x.CSEQ.EQDESC + " | " + x.CSEQ.EQCAT + " | " + x.SCRIPT + " | " + x.CERTNO + " | " + (x.COMPLETE == "Y" ? x.SHAREAMT : x.SHAREOS)
            }), "EQID", "EQDESC", cSCOSHEQ.EQID);
            ViewBag.Title = "View Share " + cSCOSHEQ.MVTYPE;
            ViewBag.Parent = cSCOSHEQ.CONO;
            ViewBag.MVTYPE = mvtype;
            ViewBag.MVNO = cSCOSHEQ.MVNO;
            ViewBag.person = cSCOSHEQ.PRSCODE;
            ViewBag.equity = cSCOSHEQ.EQCODE;

            Session["CSCOSHEQEditViewName"] = ViewName;
            return View(ViewName, cSCOSHEQ);
        }

        // GET: CSCOSHEQs/Create
        public ActionResult Create(string id, string mvtype, string mvno, string person, string eqcode)
        {
            string sid = MyHtmlHelpers.ConvertByteStrToId(id);
            CSCOSHEQ cSCOSHEQ = new CSCOSHEQ();
            cSCOSHEQ.CONO = sid;
            cSCOSHEQ.MVTYPE = mvtype;
            cSCOSHEQ.MVDATE = DateTime.Today;
            cSCOSHEQ.MVID = 0;
            cSCOSHEQ.REFCNT = 0;
            cSCOSHEQ.STAMP = 0;
            cSCOSHEQ.SCRIPT = "Y";
            cSCOSHEQ.COMPLETE = "N";
            cSCOSHEQ.COMPLETED = new DateTime(3000, 1, 1);

            string ViewName = "Create1";

            if (mvtype == "Allotment")
            {
                cSCOSHEQ.MVSIGN = "In";
                ViewName = "Create";
            }
            else if (mvtype == "Transfer")
            {
                cSCOSHEQ.MVSIGN = "Out";
            }
            else if (mvtype == "Split")
            {
                if (String.IsNullOrEmpty(mvno))
                {
                    cSCOSHEQ.MVSIGN = "Out";
                }
                else
                {
                    cSCOSHEQ.MVSIGN = "In";
                    cSCOSHEQ.MVNO = mvno;
                    cSCOSHEQ.EQCODE = eqcode;
                    cSCOSHEQ.PRSCODE = person;
                    ViewName = "Create";
                }
            }
            else if (mvtype == "Conversion")
            {
                if (String.IsNullOrEmpty(mvno))
                {
                    cSCOSHEQ.MVSIGN = "In";
                    ViewName = "Create";
                }
                else
                {
                    cSCOSHEQ.MVSIGN = "Out";
                    cSCOSHEQ.MVNO = mvno;
                    cSCOSHEQ.EQCODE = eqcode;
                    cSCOSHEQ.PRSCODE = person;
                    ViewName = "Create1";
                }
            }


            ViewBag.Parent = sid;
            ViewBag.PRSCODE = new SelectList(db.CSCOSHes.Where(x => x.CONO == sid).Select(x => new { PRSCODE = x.PRSCODE, PRSNAME = x.CSPR.PRSNAME + " | " + x.FOLIONO }), "PRSCODE", "PRSNAME", cSCOSHEQ.PRSCODE);
            ViewBag.PRSCODETO = new SelectList(db.CSCOSHes.Where(x => x.CONO == cSCOSHEQ.CONO && x.PRSCODE != cSCOSHEQ.PRSCODE).Select(x => new { PRSCODE = x.PRSCODE, PRSNAME = x.CSPR.PRSNAME + " | " + x.FOLIONO }), "PRSCODE", "PRSNAME", cSCOSHEQ.PRSCODETO);
            ViewBag.EQCODE = new SelectList(db.CSEQs.OrderBy(x => x.EQCODE), "EQCODE", "EQDESC");
            ViewBag.EQID = new SelectList(db.CSCOSHEQs.Where(x => x.CONO == cSCOSHEQ.CONO && x.PRSCODE == cSCOSHEQ.PRSCODE && x.COMPLETE == "N").Select(x => new { EQID = x.EQCODE + " | " + x.SCRIPT + " | " + x.CERTNO, EQDESC = x.EQCODE + " | " + x.CSEQ.EQDESC + " | " + x.CSEQ.EQCAT + " | " + x.SCRIPT + " | " + x.CERTNO + " | " + x.SHAREOS }), "EQID", "EQDESC", cSCOSHEQ.EQID);
            ViewBag.Title = "Create Share " + mvtype;
            ViewBag.MVTYPE = mvtype;
            return View(ViewName, cSCOSHEQ);
        }

        public ActionResult EditPRSCODE(CSCOSHEQ cSCOSHEQ)
        {

            //ModelState.Clear(); //apparent this has side-effects and using Remove is preferrable
            ModelState.Remove("EQCODE");
            ModelState.Remove("SCRIPT");
            ModelState.Remove("CERTNO");
            ModelState.Remove("SHAREAMT");
            ModelState.Remove("PRSCODE");


            cSCOSHEQ.EQCODE = null;
            cSCOSHEQ.SCRIPT = null;
            cSCOSHEQ.CERTNO = null;
            cSCOSHEQ.SHAREAMT = 0;

            ViewBag.PRSCODETO = new SelectList(db.CSCOSHes.Where(x => x.CONO == cSCOSHEQ.CONO && x.PRSCODE != cSCOSHEQ.PRSCODE).Select(x => new { PRSCODE = x.PRSCODE, PRSNAME = x.CSPR.PRSNAME + " | " + x.FOLIONO }), "PRSCODE", "PRSNAME", cSCOSHEQ.PRSCODETO);
            ViewBag.EQID = new SelectList(db.CSCOSHEQs.Where(x => x.CONO == cSCOSHEQ.CONO && x.MVSIGN == "In" && x.PRSCODE == cSCOSHEQ.PRSCODE && x.COMPLETE == "N").Select(x => new { EQID = x.EQCODE + " | " + x.SCRIPT + " | " + x.CERTNO, EQDESC = x.EQCODE + " | " + x.CSEQ.EQDESC + " | " + x.CSEQ.EQCAT + " | " + x.SCRIPT + " | " + x.CERTNO + " | " + x.SHAREOS }), "EQID", "EQDESC", cSCOSHEQ.EQID);

            return PartialView("Partial/EditPRSCODE", cSCOSHEQ);


        }

        // POST: CSCOSHEQs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Split_Denom,CONO,PRSCODE, PRSCODETO,MVNO,MVID,MVTYPE,MVDATE,MVSIGN,FOLIONOSRC, FOLIONOTO,EQCODE,SCRIPT,SCRIPTBool,CERTNO,SHAREAMT,SSHAREAMT,SHAREOS,AMT,SAMT,REM,REFCNT,COMPLETE,COMPLETED,STAMP")] CSCOSHEQ cSCOSHEQ)
        {
            bool lTranCompleted = true;

            if (ModelState.IsValid)
            {
                try
                {
                    CSCOLASTNO csLastNo = null;
                    if (cSCOSHEQ.MVTYPE == "Allotment")
                    {
                        csLastNo = db.CSCOLASTNOes.Find(cSCOSHEQ.CONO, "SHALLOT");
                        cSCOSHEQ.FOLIONOSRC = null;
                        cSCOSHEQ.SSHAREAMT = cSCOSHEQ.SHAREAMT;
                        cSCOSHEQ.SHAREOS = cSCOSHEQ.SHAREAMT;
                        cSCOSHEQ.SAMT = cSCOSHEQ.AMT;
                        cSCOSHEQ.COMPLETE = "N";
                        cSCOSHEQ.COMPLETED = new DateTime(3000, 1, 1);
                        cSCOSHEQ.REFCNT = 0;
                    }
                    else if (cSCOSHEQ.MVTYPE == "Transfer")
                    {
                        csLastNo = db.CSCOLASTNOes.Find(cSCOSHEQ.CONO, "SHTRF");
                        cSCOSHEQ.SSHAREAMT = -cSCOSHEQ.SHAREAMT;
                        cSCOSHEQ.SHAREOS = cSCOSHEQ.SHAREOS - cSCOSHEQ.SHAREAMT; ;
                        cSCOSHEQ.SAMT = -cSCOSHEQ.AMT;
                        cSCOSHEQ.COMPLETE = "Y";
                        cSCOSHEQ.COMPLETED = cSCOSHEQ.MVDATE;
                        cSCOSHEQ.REFCNT = 0;

                    }
                    else if (cSCOSHEQ.MVTYPE == "Split")
                    {
                        csLastNo = db.CSCOLASTNOes.Find(cSCOSHEQ.CONO, "SHSPLIT");
                        cSCOSHEQ.SSHAREAMT = -cSCOSHEQ.SHAREAMT;
                        cSCOSHEQ.SHAREOS = cSCOSHEQ.SHAREOS - cSCOSHEQ.SHAREAMT; ;
                        cSCOSHEQ.FOLIONOSRC = null;
                        cSCOSHEQ.COMPLETE = "Y";
                        cSCOSHEQ.COMPLETED = cSCOSHEQ.MVDATE;
                        cSCOSHEQ.REFCNT = 0;
                        cSCOSHEQ.SAMT = -cSCOSHEQ.AMT;
                    }
                    else if (cSCOSHEQ.MVTYPE == "Conversion")
                    {
                        if (cSCOSHEQ.MVSIGN == "In")
                        {
                            csLastNo = db.CSCOLASTNOes.Find(cSCOSHEQ.CONO, "SHCONV");
                            cSCOSHEQ.FOLIONOSRC = null;
                            cSCOSHEQ.SSHAREAMT = cSCOSHEQ.SHAREAMT;
                            cSCOSHEQ.SHAREOS = cSCOSHEQ.SHAREAMT;
                            cSCOSHEQ.SAMT = cSCOSHEQ.AMT;
                            cSCOSHEQ.COMPLETE = "N";
                            cSCOSHEQ.COMPLETED = new DateTime(3000, 1, 1);
                            cSCOSHEQ.REFCNT = 0;
                        } else
                        {
                            cSCOSHEQ.FOLIONOSRC = null;
                            cSCOSHEQ.SSHAREAMT = -cSCOSHEQ.SHAREAMT;
                            cSCOSHEQ.SHAREOS = cSCOSHEQ.SHAREOS - cSCOSHEQ.SHAREAMT;
                            cSCOSHEQ.SAMT = -cSCOSHEQ.AMT;
                            cSCOSHEQ.COMPLETE = "Y";
                            cSCOSHEQ.COMPLETED = DateTime.Today;
                            cSCOSHEQ.REFCNT = 0;
                        }
                    }

                    if ((cSCOSHEQ.SCRIPT == "Y") && (String.IsNullOrEmpty(cSCOSHEQ.CERTNO)))
                    {
                        CSCOLASTNO csCertNo = db.CSCOLASTNOes.Find(cSCOSHEQ.CONO, "CERTNO");
                        cSCOSHEQ.CERTNO = (csCertNo.LASTNO + 1).ToString("D" + (csCertNo.LASTWD).ToString());
                        csCertNo.LASTNO = csCertNo.LASTNO + 1;
                        db.Entry(csCertNo).State = EntityState.Modified;
                    }

                    if (string.IsNullOrEmpty(cSCOSHEQ.MVNO))
                    {
                        cSCOSHEQ.MVNO = csLastNo.LASTPFIX + (csLastNo.LASTNO + 1).ToString("D" + (csLastNo.LASTWD - 1).ToString());
                        cSCOSHEQ.MVID = 0;
                        csLastNo.LASTNO = csLastNo.LASTNO + 1;
                        db.Entry(csLastNo).State = EntityState.Modified;
                    }

                    cSCOSHEQ.STAMP = 0;
                    db.CSCOSHEQs.Add(cSCOSHEQ);



                    if (cSCOSHEQ.MVTYPE == "Allotment")
                    {
                        EMTRA eMTRA = db.EMTRAs.Find(cSCOSHEQ.CONO, cSCOSHEQ.MVNO);
                        if (eMTRA != null)
                        {
                            db.Entry(eMTRA).State = EntityState.Modified;
                        }
                        else
                        {
                            eMTRA = new EMTRA();
                            eMTRA.CONO = cSCOSHEQ.CONO;
                            eMTRA.TRNO = cSCOSHEQ.MVNO;
                            db.EMTRAs.Add(eMTRA);
                        }
                        eMTRA.TRDATE = cSCOSHEQ.MVDATE;
                        eMTRA.FOLIONO = db.CSCOSHes.Where(x => x.CONO == cSCOSHEQ.CONO && x.PRSCODE == cSCOSHEQ.PRSCODE).Select(y => y.FOLIONO).FirstOrDefault();
                        eMTRA.EQCODE = cSCOSHEQ.EQCODE;
                        eMTRA.PRSCODE = cSCOSHEQ.PRSCODE;
                        eMTRA.SCRIPT = cSCOSHEQ.SCRIPT;
                        eMTRA.CERTNO = cSCOSHEQ.CERTNO;
                        eMTRA.SHAREAMT = cSCOSHEQ.SHAREAMT;
                        eMTRA.AMT = cSCOSHEQ.AMT;
                        eMTRA.REM = cSCOSHEQ.REM;
                        eMTRA.STAMP = cSCOSHEQ.STAMP;
                    }
                    else if (cSCOSHEQ.MVTYPE == "Transfer")
                    {

                        CSCOSHEQ csSource = db.CSCOSHEQs.Where(x => x.CONO == cSCOSHEQ.CONO && x.PRSCODE == cSCOSHEQ.PRSCODE && x.CERTNO == cSCOSHEQ.CERTNO && x.SCRIPT == cSCOSHEQ.SCRIPT).FirstOrDefault();
                        if (csSource != null)
                        {
                            csSource.COMPLETE = "Y";
                            csSource.SHAREOS = cSCOSHEQ.SHAREOS;
                            csSource.COMPLETED = cSCOSHEQ.MVDATE;
                            csSource.REFCNT = csSource.REFCNT + 1;
                            csSource.STAMP = csSource.STAMP + 1;
                            db.Entry(csSource).State = EntityState.Modified;
                        }
                        else
                        {
                            throw new Exception("Unable to find source cert " + cSCOSHEQ.CERTNO);
                        }

                        CSCOSHEQ csDet = new CSCOSHEQ();
                        csDet.CONO = cSCOSHEQ.CONO;
                        csDet.MVNO = cSCOSHEQ.MVNO;
                        csDet.MVID = cSCOSHEQ.MVID + 1;
                        csDet.MVTYPE = cSCOSHEQ.MVTYPE;
                        csDet.PRSCODE = cSCOSHEQ.PRSCODETO;
                        csDet.MVDATE = cSCOSHEQ.MVDATE;
                        csDet.MVSIGN = "In";
                        csDet.FOLIONOSRC = cSCOSHEQ.FOLIONOSRC;
                        csDet.EQCODE = cSCOSHEQ.EQCODE;
                        csDet.SCRIPT = cSCOSHEQ.SCRIPT;
                        csDet.CERTNO = null;

                        cSCOSHEQ.FOLIONOSRC = cSCOSHEQ.FOLIONOTO;

                        if ((csDet.SCRIPT == "Y") && (String.IsNullOrEmpty(csDet.CERTNO)))
                        {
                            CSCOLASTNO csCertNo = db.CSCOLASTNOes.Find(cSCOSHEQ.CONO, "CERTNO");
                            csDet.CERTNO = (csCertNo.LASTNO + 1).ToString("D" + (csCertNo.LASTWD).ToString());
                            csCertNo.LASTNO = csCertNo.LASTNO + 1;
                            db.Entry(csCertNo).State = EntityState.Modified;
                        }
                        csDet.SHAREAMT = cSCOSHEQ.SHAREAMT;
                        csDet.SSHAREAMT = cSCOSHEQ.SHAREAMT;
                        csDet.SHAREOS = cSCOSHEQ.SHAREAMT;
                        csDet.AMT = cSCOSHEQ.AMT;
                        csDet.SAMT = cSCOSHEQ.AMT;
                        csDet.REM = cSCOSHEQ.REM;
                        csDet.COMPLETE = "N";
                        csDet.COMPLETED = new DateTime(3000, 1, 1);
                        csDet.STAMP = 0;
                        csDet.REFCNT = 0;
                        db.CSCOSHEQs.Add(csDet);

                        CSCOSHEQD cSCOSHEQD = db.CSCOSHEQDs.Find(cSCOSHEQ.CONO, cSCOSHEQ.PRSCODE, cSCOSHEQ.MVTYPE, cSCOSHEQ.MVNO, cSCOSHEQ.MVID);
                        if (cSCOSHEQD != null)
                        {
                            cSCOSHEQD.STAMP = cSCOSHEQD.STAMP + 1;
                            db.Entry(cSCOSHEQD).State = EntityState.Modified;
                        }
                        else
                        {
                            cSCOSHEQD = new CSCOSHEQD();
                            cSCOSHEQD.CONO = cSCOSHEQ.CONO;
                            cSCOSHEQD.PRSCODE = cSCOSHEQ.PRSCODE;
                            cSCOSHEQD.MVTYPE = cSCOSHEQ.MVTYPE;
                            cSCOSHEQD.MVNO = cSCOSHEQ.MVNO;
                            cSCOSHEQD.MVID = cSCOSHEQ.MVID;
                            cSCOSHEQD.STAMP = 0;
                            db.CSCOSHEQDs.Add(cSCOSHEQD);

                        }
                        cSCOSHEQD.INMVTYPE = csSource.MVTYPE;
                        cSCOSHEQD.INMVNO = csSource.MVNO;
                        cSCOSHEQD.INMVID = csSource.MVID;
                        cSCOSHEQD.OUTMVTYPE = cSCOSHEQ.MVTYPE;
                        cSCOSHEQD.OUTMVNO = cSCOSHEQ.MVNO;
                        cSCOSHEQD.OUTMVID = cSCOSHEQ.MVID;
                        cSCOSHEQD.MVDATE = cSCOSHEQ.MVDATE;
                        cSCOSHEQD.SHAREAMT = cSCOSHEQ.SHAREAMT;


                        EMTRTM eMTRTM = db.EMTRTMs.Find(cSCOSHEQ.CONO, cSCOSHEQ.MVNO);
                        if (eMTRTM != null)
                        {
                            db.Entry(eMTRTM).State = EntityState.Modified;
                        }
                        else
                        {
                            eMTRTM = new EMTRTM();
                            eMTRTM.CONO = cSCOSHEQ.CONO;
                            eMTRTM.TRNO = cSCOSHEQ.MVNO;
                            db.EMTRTMs.Add(eMTRTM);
                        }
                        eMTRTM.TRDATE = cSCOSHEQ.MVDATE;
                        eMTRTM.FOLIONOFR = db.CSCOSHes.Where(x => x.CONO == cSCOSHEQ.CONO && x.PRSCODE == cSCOSHEQ.PRSCODE).Select(y => y.FOLIONO).FirstOrDefault();
                        eMTRTM.EQCODEFR = cSCOSHEQ.EQCODE;
                        eMTRTM.PRSCODEFR = cSCOSHEQ.PRSCODE;
                        eMTRTM.SCRIPTFR = cSCOSHEQ.SCRIPT;
                        eMTRTM.CERTNOFR = cSCOSHEQ.CERTNO;
                        eMTRTM.SHAREAMT = cSCOSHEQ.SHAREAMT;
                        eMTRTM.AMT = cSCOSHEQ.AMT;
                        eMTRTM.REM = cSCOSHEQ.REM;
                        eMTRTM.PRSCODETO = cSCOSHEQ.PRSCODETO;
                        eMTRTM.FOLIONOTO = cSCOSHEQ.FOLIONOTO ?? 0;
                        eMTRTM.STAMP = cSCOSHEQ.STAMP;

                        EMTRTD eMTRTD = db.EMTRTDs.Find(cSCOSHEQ.CONO, cSCOSHEQ.MVNO, csDet.MVID);
                        if (eMTRTD != null)
                        {
                            db.Entry(eMTRTD).State = EntityState.Modified;
                        }
                        else
                        {
                            eMTRTD = new EMTRTD();
                            eMTRTD.CONO = cSCOSHEQ.CONO;
                            eMTRTD.TRNO = cSCOSHEQ.MVNO;
                            eMTRTD.TRID = csDet.MVID;
                            db.EMTRTDs.Add(eMTRTD);
                        }

                        eMTRTD.EQCODE = csDet.EQCODE;
                        eMTRTD.SCRIPT = csDet.SCRIPT;
                        eMTRTD.CERTNO = csDet.CERTNO;
                        eMTRTD.SHAREAMT = csDet.SHAREAMT;
                        eMTRTD.AMT = csDet.AMT;
                        eMTRTD.STAMP = 0;
                    }
                    else if (cSCOSHEQ.MVTYPE == "Split")
                    {
                        if (cSCOSHEQ.MVSIGN == "In")
                        {
                            lTranCompleted = false;
                            int lastmvno = 0;
                            try
                            {
                                lastmvno = db.CSCOSHEQs.Where(x => x.CONO == cSCOSHEQ.CONO && x.MVNO == cSCOSHEQ.MVNO && x.MVTYPE == cSCOSHEQ.MVTYPE).Max(x => x.MVID);
                            }
                            catch { lastmvno = 0; }

                            cSCOSHEQ.MVID = lastmvno + 1;
                            cSCOSHEQ.SSHAREAMT = cSCOSHEQ.SHAREAMT;
                            cSCOSHEQ.SHAREOS = cSCOSHEQ.SHAREAMT;
                            cSCOSHEQ.SAMT = cSCOSHEQ.AMT;
                            cSCOSHEQ.COMPLETE = "N";
                            cSCOSHEQ.COMPLETED = new DateTime(3000, 1, 1);
                            cSCOSHEQ.STAMP = 0;
                            cSCOSHEQ.REFCNT = 0;

                            EMTRSD eMTRSD = db.EMTRSDs.Find(cSCOSHEQ.CONO, cSCOSHEQ.MVNO, cSCOSHEQ.MVID);
                            if (eMTRSD != null)
                            {
                                db.Entry(eMTRSD).State = EntityState.Modified;
                            }
                            else
                            {
                                eMTRSD = new EMTRSD();
                                eMTRSD.CONO = cSCOSHEQ.CONO;
                                eMTRSD.TRNO = cSCOSHEQ.MVNO;
                                eMTRSD.TRID = cSCOSHEQ.MVID;
                                db.EMTRSDs.Add(eMTRSD);
                            }

                            eMTRSD.EQCODE = cSCOSHEQ.EQCODE;
                            eMTRSD.SCRIPT = cSCOSHEQ.SCRIPT;
                            eMTRSD.CERTNO = cSCOSHEQ.CERTNO;
                            eMTRSD.SHAREAMT = cSCOSHEQ.SHAREAMT;
                            eMTRSD.STAMP = 0;

                        }
                        else
                        {
                            CSCOSHEQ csSource = db.CSCOSHEQs.Where(x => x.CONO == cSCOSHEQ.CONO && x.PRSCODE == cSCOSHEQ.PRSCODE && x.CERTNO == cSCOSHEQ.CERTNO && x.SCRIPT == cSCOSHEQ.SCRIPT).FirstOrDefault();
                            if (csSource != null)
                            {
                                csSource.COMPLETE = "Y";
                                csSource.SHAREOS = cSCOSHEQ.SHAREOS;
                                csSource.COMPLETED = cSCOSHEQ.MVDATE;
                                csSource.REFCNT = csSource.REFCNT + 1;
                                csSource.STAMP = csSource.STAMP + 1;
                                db.Entry(csSource).State = EntityState.Modified;
                            }
                            else
                            {
                                throw new Exception("Unable to find source cert " + cSCOSHEQ.CERTNO);
                            }

                            int nextId = cSCOSHEQ.MVID;
                            lTranCompleted = false;
                            foreach (var item in cSCOSHEQ.Split_Denom)
                            {
                                for (var dcnt = 0; dcnt < item.DenomCnt; dcnt++)
                                {
                                    nextId++;
                                    lTranCompleted = true;
                                    CSCOSHEQ csDet = new CSCOSHEQ();
                                    csDet.CONO = cSCOSHEQ.CONO;
                                    csDet.MVNO = cSCOSHEQ.MVNO;
                                    csDet.MVID = nextId;
                                    csDet.MVTYPE = cSCOSHEQ.MVTYPE;
                                    csDet.PRSCODE = cSCOSHEQ.PRSCODE;
                                    csDet.MVDATE = cSCOSHEQ.MVDATE;
                                    csDet.MVSIGN = "In";
                                    //csDet.FOLIONOSRC = cSCOSHEQ.FOLIONOSRC;
                                    csDet.EQCODE = cSCOSHEQ.EQCODE;
                                    csDet.SCRIPT = cSCOSHEQ.SCRIPT;
                                    csDet.CERTNO = null;

                                    //cSCOSHEQ.FOLIONOSRC = cSCOSHEQ.FOLIONOTO;

                                    if ((csDet.SCRIPT == "Y") && (String.IsNullOrEmpty(csDet.CERTNO)))
                                    {
                                        CSCOLASTNO csCertNo = db.CSCOLASTNOes.Find(cSCOSHEQ.CONO, "CERTNO");
                                        csDet.CERTNO = (csCertNo.LASTNO + 1).ToString("D" + (csCertNo.LASTWD).ToString());
                                        csCertNo.LASTNO = csCertNo.LASTNO + 1;
                                        db.Entry(csCertNo).State = EntityState.Modified;
                                    }
                                    csDet.SHAREAMT = item.DenomUnit;
                                    csDet.SSHAREAMT = item.DenomUnit;
                                    csDet.SHAREOS = item.DenomUnit;
                                    csDet.AMT = 0;
                                    csDet.SAMT = 0;
                                    csDet.REM = cSCOSHEQ.REM;
                                    csDet.COMPLETE = "N";
                                    csDet.COMPLETED = new DateTime(3000, 1, 1);
                                    csDet.STAMP = 0;
                                    csDet.REFCNT = 0;
                                    db.CSCOSHEQs.Add(csDet);

                                    EMTRSD eMTRSD = db.EMTRSDs.Find(cSCOSHEQ.CONO, cSCOSHEQ.MVNO, csDet.MVID);
                                    if (eMTRSD != null)
                                    {
                                        db.Entry(eMTRSD).State = EntityState.Modified;
                                    }
                                    else
                                    {
                                        eMTRSD = new EMTRSD();
                                        eMTRSD.CONO = cSCOSHEQ.CONO;
                                        eMTRSD.TRNO = cSCOSHEQ.MVNO;
                                        eMTRSD.TRID = csDet.MVID;
                                        db.EMTRSDs.Add(eMTRSD);
                                    }

                                    eMTRSD.EQCODE = csDet.EQCODE;
                                    eMTRSD.SCRIPT = csDet.SCRIPT;
                                    eMTRSD.CERTNO = csDet.CERTNO;
                                    eMTRSD.SHAREAMT = csDet.SHAREAMT;
                                    eMTRSD.STAMP = 0;
                                }

                            }

                            CSCOSHEQD cSCOSHEQD = db.CSCOSHEQDs.Find(cSCOSHEQ.CONO, cSCOSHEQ.PRSCODE, cSCOSHEQ.MVTYPE, cSCOSHEQ.MVNO, cSCOSHEQ.MVID);
                            if (cSCOSHEQD != null)
                            {
                                cSCOSHEQD.STAMP = cSCOSHEQD.STAMP + 1;
                                db.Entry(cSCOSHEQD).State = EntityState.Modified;
                            }
                            else
                            {
                                cSCOSHEQD = new CSCOSHEQD();
                                cSCOSHEQD.CONO = cSCOSHEQ.CONO;
                                cSCOSHEQD.PRSCODE = cSCOSHEQ.PRSCODE;
                                cSCOSHEQD.MVTYPE = cSCOSHEQ.MVTYPE;
                                cSCOSHEQD.MVNO = cSCOSHEQ.MVNO;
                                cSCOSHEQD.MVID = cSCOSHEQ.MVID;
                                cSCOSHEQD.STAMP = 0;
                                db.CSCOSHEQDs.Add(cSCOSHEQD);

                            }
                            cSCOSHEQD.INMVTYPE = csSource.MVTYPE;
                            cSCOSHEQD.INMVNO = csSource.MVNO;
                            cSCOSHEQD.INMVID = csSource.MVID;
                            cSCOSHEQD.OUTMVTYPE = cSCOSHEQ.MVTYPE;
                            cSCOSHEQD.OUTMVNO = cSCOSHEQ.MVNO;
                            cSCOSHEQD.OUTMVID = cSCOSHEQ.MVID;
                            cSCOSHEQD.MVDATE = cSCOSHEQ.MVDATE;
                            cSCOSHEQD.SHAREAMT = cSCOSHEQ.SHAREAMT;


                            EMTRSM eMTRSM = db.EMTRSMs.Find(cSCOSHEQ.CONO, cSCOSHEQ.MVNO);
                            if (eMTRSM != null)
                            {
                                db.Entry(eMTRSM).State = EntityState.Modified;
                            }
                            else
                            {
                                eMTRSM = new EMTRSM();
                                eMTRSM.CONO = cSCOSHEQ.CONO;
                                eMTRSM.TRNO = cSCOSHEQ.MVNO;
                                db.EMTRSMs.Add(eMTRSM);
                            }
                            eMTRSM.TRDATE = cSCOSHEQ.MVDATE;
                            eMTRSM.FOLIONOFR = db.CSCOSHes.Where(x => x.CONO == cSCOSHEQ.CONO && x.PRSCODE == cSCOSHEQ.PRSCODE).Select(y => y.FOLIONO).FirstOrDefault();
                            eMTRSM.EQCODEFR = cSCOSHEQ.EQCODE;
                            eMTRSM.PRSCODEFR = cSCOSHEQ.PRSCODE;
                            eMTRSM.SCRIPTFR = cSCOSHEQ.SCRIPT;
                            eMTRSM.CERTNOFR = cSCOSHEQ.CERTNO;
                            eMTRSM.SHAREAMT = cSCOSHEQ.SHAREAMT;
                            eMTRSM.REM = cSCOSHEQ.REM;

                            eMTRSM.STAMP = cSCOSHEQ.STAMP;
                        }
                    } else 
                    if (cSCOSHEQ.MVTYPE == "Conversion")
                    {
                        if (cSCOSHEQ.MVSIGN == "In")
                        {
                            EMTRCM eMTRCM = db.EMTRCMs.Find(cSCOSHEQ.CONO, cSCOSHEQ.MVNO);
                            if (eMTRCM != null)
                            {
                                db.Entry(eMTRCM).State = EntityState.Modified;
                            }
                            else
                            {
                                eMTRCM = new EMTRCM();
                                eMTRCM.CONO = cSCOSHEQ.CONO;
                                eMTRCM.TRNO = cSCOSHEQ.MVNO;
                                db.EMTRCMs.Add(eMTRCM);
                            }
                            eMTRCM.TRDATE = cSCOSHEQ.MVDATE;
                            eMTRCM.FOLIONOFR = db.CSCOSHes.Where(x => x.CONO == cSCOSHEQ.CONO && x.PRSCODE == cSCOSHEQ.PRSCODE).Select(y => y.FOLIONO).FirstOrDefault();
                            eMTRCM.EQCODEFR = cSCOSHEQ.EQCODE;
                            eMTRCM.PRSCODEFR = cSCOSHEQ.PRSCODE;
                            eMTRCM.SCRIPTFR = cSCOSHEQ.SCRIPT;
                            eMTRCM.CERTNOFR = cSCOSHEQ.CERTNO;
                            eMTRCM.SHAREAMT = cSCOSHEQ.SHAREAMT;
                            eMTRCM.AMT = cSCOSHEQ.AMT;
                            eMTRCM.REM = cSCOSHEQ.REM;
                            eMTRCM.STAMP = cSCOSHEQ.STAMP;
                        } else
                        {
                            CSCOSHEQ csSource = db.CSCOSHEQs.Where(x => x.CONO == cSCOSHEQ.CONO && x.PRSCODE == cSCOSHEQ.PRSCODE && x.CERTNO == cSCOSHEQ.CERTNO && x.SCRIPT == cSCOSHEQ.SCRIPT).FirstOrDefault();
                            if (csSource != null)
                            {
                                csSource.COMPLETE = "Y";
                                csSource.SHAREOS = cSCOSHEQ.SHAREOS;
                                csSource.COMPLETED = cSCOSHEQ.MVDATE;
                                csSource.REFCNT = csSource.REFCNT + 1;
                                csSource.STAMP = csSource.STAMP + 1;
                                db.Entry(csSource).State = EntityState.Modified;
                            }
                            else
                            {
                                throw new Exception("Unable to find source cert " + cSCOSHEQ.CERTNO);
                            }

                            int lastmvno = 0;
                            try
                            {
                                lastmvno = db.CSCOSHEQs.Where(x => x.CONO == cSCOSHEQ.CONO && x.MVNO == cSCOSHEQ.MVNO && x.MVTYPE == cSCOSHEQ.MVTYPE).Max(x => x.MVID);
                            }
                            catch { lastmvno = 0; }

                            cSCOSHEQ.MVID = lastmvno + 1;


                            EMTRCD eMTRCD = db.EMTRCDs.Find(cSCOSHEQ.CONO, cSCOSHEQ.MVNO, cSCOSHEQ.MVID);
                            if (eMTRCD != null)
                            {
                                db.Entry(eMTRCD).State = EntityState.Modified;
                            }
                            else
                            {
                                eMTRCD = new EMTRCD();
                                eMTRCD.CONO = cSCOSHEQ.CONO;
                                eMTRCD.TRNO = cSCOSHEQ.MVNO;
                                eMTRCD.TRID = cSCOSHEQ.MVID;
                                db.EMTRCDs.Add(eMTRCD);
                            }
                            eMTRCD.EQCODE = cSCOSHEQ.EQCODE;

                            eMTRCD.SCRIPT = cSCOSHEQ.SCRIPT;
                            eMTRCD.CERTNO= cSCOSHEQ.CERTNO;
                            eMTRCD.SHAREAMT = cSCOSHEQ.SHAREAMT;
                            eMTRCD.AMT = cSCOSHEQ.AMT;
                            eMTRCD.STAMP = 0;
                        }
                        lTranCompleted = false;
                    }


                    db.SaveChanges();
                    if (lTranCompleted)
                    {
                        return RedirectToAction("ShareMove", "CSCOSHes", new { id = MyHtmlHelpers.ConvertIdToByteStr(cSCOSHEQ.CONO) });
                    }
                    else
                    {
                        return RedirectToAction("Edit", "CSCOSHEQs", new { id = MyHtmlHelpers.ConvertIdToByteStr(cSCOSHEQ.CONO), person = cSCOSHEQ.PRSCODE, mvtype = cSCOSHEQ.MVTYPE, mvno = cSCOSHEQ.MVNO, mvid = 0 });
                    }
                }
                catch (DbEntityValidationException dbEx)
                {
                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            //string message = string.Format("{0}:{1}",
                            //    validationErrors.Entry.Entity.ToString(),
                            //   validationError.ErrorMessage);
                            // raise a new exception nesting
                            // the current instance as InnerException
                            ModelState.AddModelError(validationError.PropertyName, validationError.ErrorMessage);
                        }
                    }
                }
                catch (DbUpdateException ex)
                {
                    UpdateException updateException = (UpdateException)ex.InnerException;
                    if (updateException != null)
                    {
                        if (updateException.InnerException != null)
                        {
                            var sqlException = (FirebirdSql.Data.FirebirdClient.FbException)updateException.InnerException;

                            foreach (var error in sqlException.Errors)
                            {
                                if (error.Message != null)
                                {
                                    ModelState.AddModelError(string.Empty, error.Message);
                                }
                            }
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, updateException.Message);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, updateException.Message);
                    }
                }
                catch (Exception e)
                {
                    ModelState.AddModelError(string.Empty, e.Message);
                }
                finally
                {

                }
            }


            ViewBag.PRSCODE = new SelectList(db.CSCOSHes.Where(x => x.CONO == cSCOSHEQ.CONO).Select(x => new { PRSCODE = x.PRSCODE, PRSNAME = x.CSPR.PRSNAME + " | " + x.FOLIONO }), "PRSCODE", "PRSNAME", cSCOSHEQ.PRSCODE);
            ViewBag.PRSCODETO = new SelectList(db.CSCOSHes.Where(x => x.CONO == cSCOSHEQ.CONO && x.PRSCODE != cSCOSHEQ.PRSCODE).Select(x => new { PRSCODE = x.PRSCODE, PRSNAME = x.CSPR.PRSNAME + " | " + x.FOLIONO }), "PRSCODE", "PRSNAME", cSCOSHEQ.PRSCODETO);
            ViewBag.EQCODE = new SelectList(db.CSEQs.OrderBy(x => x.EQCODE), "EQCODE", "EQDESC", cSCOSHEQ.EQCODE);
            ViewBag.EQID = new SelectList(db.CSCOSHEQs.Where(x => x.CONO == cSCOSHEQ.CONO && x.MVSIGN == "In" && x.PRSCODE == cSCOSHEQ.PRSCODE && x.COMPLETE == "N").Select(x => new { EQID = x.EQCODE + " | " + x.SCRIPT + " | " + x.CERTNO, EQDESC = x.EQCODE + " | " + x.CSEQ.EQDESC + " | " + x.CSEQ.EQCAT + " | " + x.SCRIPT + " | " + x.CERTNO + " | " + x.SHAREOS }), "EQID", "EQDESC", cSCOSHEQ.EQID);
            ViewBag.Title = "Create Share " + cSCOSHEQ.MVTYPE;
            ViewBag.MVTYPE = cSCOSHEQ.MVTYPE;
            ViewBag.Parent = cSCOSHEQ.CONO;
            return View(cSCOSHEQ);
        }

        // GET: CSCOSHEQs/Edit/5
        public ActionResult Edit(string id, string person, string mvno, int mvid, string mvtype)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            string sid = MyHtmlHelpers.ConvertByteStrToId(id);
            CSCOSHEQ cSCOSHEQ = db.CSCOSHEQs.Find(sid, person, mvno, mvid, mvtype);
            if (cSCOSHEQ == null)
            {
                return HttpNotFound();
            }
            string ViewName = "Edit1";

            if (mvtype == "Allotment")
            {
                ViewName = "Edit";
            }
            else if (mvtype == "Transfer")
            {
                if (cSCOSHEQ.MVSIGN == "In") { ViewName = "Edit"; }
                else
                {
                    CSCOSHEQ cSTransferee = db.CSCOSHEQs.Where(x => x.CONO == sid && x.MVNO == mvno && x.MVID == 1 && x.MVTYPE == mvtype).FirstOrDefault();
                    cSCOSHEQ.FOLIONOTO = cSTransferee.CSCOSH.FOLIONO;
                    cSCOSHEQ.PRSCODETO = cSTransferee.PRSCODE;
                    cSCOSHEQ.SHAREOS = cSCOSHEQ.SHAREAMT; // revert back to original value 
                }
            }
            else if (mvtype == "Split")
            {
                if (cSCOSHEQ.MVSIGN == "In") { ViewName = "Edit"; }
                else
                {
                    cSCOSHEQ.SHAREOS = cSCOSHEQ.SHAREAMT; // revert back to original value 
                }
            }
            else if (mvtype == "Conversion")
            {
                if (cSCOSHEQ.MVSIGN == "In") { ViewName = "Edit"; }
            }

            ViewBag.PRSCODE = new SelectList(db.CSCOSHes.Where(x => x.CONO == cSCOSHEQ.CONO).Select(x => new { PRSCODE = x.PRSCODE, PRSNAME = x.CSPR.PRSNAME + " | " + x.FOLIONO }), "PRSCODE", "PRSNAME", cSCOSHEQ.PRSCODE);
            ViewBag.PRSCODETO = new SelectList(db.CSCOSHes.Where(x => x.CONO == cSCOSHEQ.CONO && x.PRSCODE != cSCOSHEQ.PRSCODE).Select(x => new { PRSCODE = x.PRSCODE, PRSNAME = x.CSPR.PRSNAME + " | " + x.FOLIONO }), "PRSCODE", "PRSNAME", cSCOSHEQ.PRSCODETO);
            ViewBag.EQCODE = new SelectList(db.CSEQs.OrderBy(x => x.EQCODE), "EQCODE", "EQDESC", cSCOSHEQ.EQCODE);
            ViewBag.EQID = new SelectList(db.CSCOSHEQs.Where(x => x.CONO == cSCOSHEQ.CONO && x.MVSIGN == "In" && x.PRSCODE == cSCOSHEQ.PRSCODE && (x.COMPLETE == "N" ||
            (x.SCRIPT == cSCOSHEQ.SCRIPT && x.CERTNO == cSCOSHEQ.CERTNO))).Select(x => new
            {
                EQID = x.EQCODE + " | " + x.SCRIPT + " | " + x.CERTNO,
                EQDESC = x.EQCODE + " | " + x.CSEQ.EQDESC + " | " + x.CSEQ.EQCAT + " | " + x.SCRIPT + " | " + x.CERTNO + " | " + (x.COMPLETE == "Y" ? x.SHAREAMT : x.SHAREOS)
            }), "EQID", "EQDESC", cSCOSHEQ.EQID);
            ViewBag.Title = "Edit Share " + cSCOSHEQ.MVTYPE;
            ViewBag.Parent = cSCOSHEQ.CONO;
            ViewBag.MVTYPE = mvtype;
            ViewBag.MVNO = cSCOSHEQ.MVNO;
            ViewBag.person = cSCOSHEQ.PRSCODE;
            ViewBag.equity = cSCOSHEQ.EQCODE;
            Session["CSCOSHEQ_ORIG"] = cSCOSHEQ;
            Session["CSCOSHEQEditViewName"] = ViewName;
            return View(ViewName, cSCOSHEQ);
        }

        // POST: CSCOSHEQs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CONO,PRSCODE, PRSCODETO,MVNO,MVID,MVTYPE,MVDATE,MVSIGN,FOLIONOSRC, FOLIONOTO,EQCODE,SCRIPT,CERTNO,SHAREAMT,SSHAREAMT,SHAREOS,AMT,SAMT,REM,REFCNT,COMPLETE,COMPLETED,STAMP")] CSCOSHEQ cSCOSHEQ)
        {
            string ViewName = (string)Session["CSCOSHEQEditViewName"];

            if (cSCOSHEQ.MRefCnt > 0)
            {
                ModelState.AddModelError(string.Empty, "Cannot Change records that is used " + cSCOSHEQ.MRefCnt.ToString());
            }
            else
            if (ModelState.IsValid)
            {
                try
                {
                    cSCOSHEQ.STAMP = cSCOSHEQ.STAMP + 1;

                    bool changed = false;
                    CSCOSHEQ csOrigSave = (CSCOSHEQ)Session["CSCOSHEQ_ORIG"];
                    if (csOrigSave.PRSCODE != cSCOSHEQ.PRSCODE) { changed = true; }

                    if (changed)
                    {
                        if ((cSCOSHEQ.MVTYPE == "Conversion") && (cSCOSHEQ.Details != null) && (cSCOSHEQ.Details.Count() > 0))
                        {
                            throw new Exception("Cannot change member after conversion. Please remove conversion details first");
                        }

                        CSCOSHEQ csDel = db.CSCOSHEQs.Find(csOrigSave.CONO, csOrigSave.PRSCODE, csOrigSave.MVNO, csOrigSave.MVID, csOrigSave.MVTYPE);
                        db.CSCOSHEQs.Remove(csDel);
                        db.CSCOSHEQs.Add(cSCOSHEQ);
                    }
                    else
                    {
                        db.Entry(cSCOSHEQ).State = EntityState.Modified;
                    }


                    if ((cSCOSHEQ.SCRIPT == "Y") && (String.IsNullOrEmpty(cSCOSHEQ.CERTNO)))
                    {
                        CSCOLASTNO csCertNo = db.CSCOLASTNOes.Find(cSCOSHEQ.CONO, "CERTNO");
                        cSCOSHEQ.CERTNO = (csCertNo.LASTNO + 1).ToString("D" + (csCertNo.LASTWD).ToString());
                        csCertNo.LASTNO = csCertNo.LASTNO + 1;
                        db.Entry(csCertNo).State = EntityState.Modified;
                    }


                    if (cSCOSHEQ.MVTYPE == "Allotment")
                    {
                        cSCOSHEQ.SSHAREAMT = cSCOSHEQ.SHAREAMT;
                        cSCOSHEQ.SHAREOS = cSCOSHEQ.SHAREAMT;
                        cSCOSHEQ.SAMT = cSCOSHEQ.AMT;

                        EMTRA eMTRA = db.EMTRAs.Find(cSCOSHEQ.CONO, cSCOSHEQ.MVNO);
                        if (eMTRA != null)
                        {
                            db.Entry(eMTRA).State = EntityState.Modified;
                        }
                        else
                        {
                            eMTRA = new EMTRA();
                            eMTRA.CONO = cSCOSHEQ.CONO;
                            eMTRA.TRNO = cSCOSHEQ.MVNO;
                            db.EMTRAs.Add(eMTRA);
                        }
                        eMTRA.TRDATE = cSCOSHEQ.MVDATE;
                        eMTRA.FOLIONO = db.CSCOSHes.Where(x => x.CONO == cSCOSHEQ.CONO && x.PRSCODE == cSCOSHEQ.PRSCODE).Select(y => y.FOLIONO).FirstOrDefault();
                        eMTRA.EQCODE = cSCOSHEQ.EQCODE;
                        eMTRA.PRSCODE = cSCOSHEQ.PRSCODE;
                        eMTRA.SCRIPT = cSCOSHEQ.SCRIPT;
                        eMTRA.CERTNO = cSCOSHEQ.CERTNO;
                        eMTRA.SHAREAMT = cSCOSHEQ.SHAREAMT;
                        eMTRA.AMT = cSCOSHEQ.AMT;
                        eMTRA.REM = cSCOSHEQ.REM;
                        eMTRA.STAMP = cSCOSHEQ.STAMP;
                    }
                    else if (cSCOSHEQ.MVTYPE == "Transfer")
                    {

                        if (cSCOSHEQ.MVSIGN == "In")
                        {

                            cSCOSHEQ.SSHAREAMT = cSCOSHEQ.SHAREAMT;
                            cSCOSHEQ.SHAREOS = cSCOSHEQ.SHAREAMT;
                            cSCOSHEQ.SAMT = cSCOSHEQ.AMT;
                            cSCOSHEQ.COMPLETE = "N";
                            cSCOSHEQ.COMPLETED = new DateTime(3000, 1, 1);
                            cSCOSHEQ.STAMP = 0;
                            cSCOSHEQ.REFCNT = 0;

                            EMTRTM eMTRTM = db.EMTRTMs.Find(cSCOSHEQ.CONO, cSCOSHEQ.MVNO);
                            if (eMTRTM != null)
                            {
                                db.Entry(eMTRTM).State = EntityState.Modified;
                            }
                            else
                            {
                                CSCOSHEQ csTransferor = db.CSCOSHEQs.Where(x => x.CONO == cSCOSHEQ.CONO && x.MVNO == cSCOSHEQ.MVNO && x.MVID == 0 && x.MVTYPE == cSCOSHEQ.MVTYPE).FirstOrDefault();
                                if (csTransferor == null)
                                {
                                    throw new Exception("Cannot Find Transferor Record");
                                }

                                eMTRTM = new EMTRTM();
                                eMTRTM.CONO = csTransferor.CONO;
                                eMTRTM.TRNO = csTransferor.MVNO;
                                eMTRTM.TRDATE = csTransferor.MVDATE;
                                eMTRTM.FOLIONOFR = db.CSCOSHes.Where(x => x.CONO == csTransferor.CONO && x.PRSCODE == csTransferor.PRSCODE).Select(y => y.FOLIONO).FirstOrDefault();
                                eMTRTM.EQCODEFR = csTransferor.EQCODE;
                                eMTRTM.PRSCODEFR = csTransferor.PRSCODE;
                                eMTRTM.SCRIPTFR = csTransferor.SCRIPT;
                                eMTRTM.CERTNOFR = csTransferor.CERTNO;
                                eMTRTM.SHAREAMT = csTransferor.SHAREAMT;
                                eMTRTM.AMT = csTransferor.AMT;
                                eMTRTM.REM = csTransferor.REM;
                                db.EMTRTMs.Add(eMTRTM);
                            }

                            eMTRTM.PRSCODETO = cSCOSHEQ.PRSCODE;
                            eMTRTM.FOLIONOTO = cSCOSHEQ.FOLIONOSRC ?? 0;
                            eMTRTM.STAMP = eMTRTM.STAMP + 1;

                            EMTRTD eMTRTD = db.EMTRTDs.Find(cSCOSHEQ.CONO, cSCOSHEQ.MVNO, cSCOSHEQ.MVID);
                            if (eMTRTD != null)
                            {
                                eMTRTD.STAMP = eMTRTD.STAMP + 1;
                                db.Entry(eMTRTD).State = EntityState.Modified;
                            }
                            else
                            {
                                eMTRTD = new EMTRTD();
                                eMTRTD.CONO = cSCOSHEQ.CONO;
                                eMTRTD.TRNO = cSCOSHEQ.MVNO;
                                eMTRTD.TRID = cSCOSHEQ.MVID;
                                eMTRTD.STAMP = 0;
                                db.EMTRTDs.Add(eMTRTD);
                            }

                            eMTRTD.EQCODE = cSCOSHEQ.EQCODE;
                            eMTRTD.SCRIPT = cSCOSHEQ.SCRIPT;
                            eMTRTD.CERTNO = cSCOSHEQ.CERTNO;
                            eMTRTD.SHAREAMT = cSCOSHEQ.SHAREAMT;

                        }
                        else
                        {

                            cSCOSHEQ.SSHAREAMT = -cSCOSHEQ.SHAREAMT;
                            cSCOSHEQ.SHAREOS = cSCOSHEQ.SHAREOS - cSCOSHEQ.SHAREAMT;
                            cSCOSHEQ.SAMT = -cSCOSHEQ.AMT;
                            cSCOSHEQ.COMPLETE = "Y";
                            cSCOSHEQ.COMPLETED = cSCOSHEQ.MVDATE;

                            CSCOSHEQ csOrig = db.CSCOSHEQs.Find(cSCOSHEQ.CONO, cSCOSHEQ.PRSCODE, cSCOSHEQ.MVNO, cSCOSHEQ.MVID, cSCOSHEQ.MVTYPE);
                            if (csOrig == null)
                            {
                                throw new Exception("Unable to find original record " + cSCOSHEQ.MVNO + " " + cSCOSHEQ.MVID.ToString());
                            }
                            else
                            {
                                CSCOSHEQ csOrigSource = db.CSCOSHEQs.Where(x => x.CONO == csOrig.CONO && x.PRSCODE == csOrig.PRSCODE && x.CERTNO == csOrig.CERTNO && x.SCRIPT == csOrig.SCRIPT && x.MVNO != cSCOSHEQ.MVNO).FirstOrDefault();
                                if (csOrigSource == null)
                                {
                                    throw new Exception("Unable to find source cert " + csOrig.CERTNO);
                                }
                                else
                                {

                                    CSCOSHEQ csSource = db.CSCOSHEQs.Where(x => x.CONO == cSCOSHEQ.CONO && x.PRSCODE == cSCOSHEQ.PRSCODE && x.CERTNO == cSCOSHEQ.CERTNO && x.SCRIPT == cSCOSHEQ.SCRIPT).FirstOrDefault();
                                    if (csSource != null)
                                    {
                                        if (csOrig.CERTNO != csSource.CERTNO)
                                        {  // if the source cert is changed then reverse the orig source cert status

                                            csOrigSource.COMPLETE = "N";
                                            csOrigSource.SHAREOS = csOrigSource.SHAREAMT;
                                            csOrigSource.COMPLETED = new DateTime(3000, 1, 1);
                                            csOrigSource.REFCNT = csOrigSource.REFCNT - 1;
                                            csOrigSource.STAMP = csOrigSource.STAMP + 1;
                                            db.Entry(csOrigSource).State = EntityState.Modified;
                                            //db.SaveChanges();

                                            csSource.COMPLETE = "Y";
                                            csSource.SHAREOS = cSCOSHEQ.SHAREOS;
                                            csSource.COMPLETED = cSCOSHEQ.MVDATE;
                                            csSource.REFCNT = csSource.REFCNT + 1;
                                            csSource.STAMP = csSource.STAMP + 1;
                                            db.Entry(csSource).State = EntityState.Modified;
                                            //db.SaveChanges();
                                        }
                                        db.Entry(csOrig).State = EntityState.Detached; // no longer needed and would cause current record to fail to modify


                                        CSCOSHEQD cSCOSHEQD = db.CSCOSHEQDs.Find(cSCOSHEQ.CONO, cSCOSHEQ.PRSCODE, cSCOSHEQ.MVTYPE, cSCOSHEQ.MVNO, cSCOSHEQ.MVID);
                                        if (cSCOSHEQD != null)
                                        {
                                            cSCOSHEQD.STAMP = cSCOSHEQD.STAMP + 1;
                                            db.Entry(cSCOSHEQD).State = EntityState.Modified;
                                        }
                                        else
                                        {
                                            cSCOSHEQD = new CSCOSHEQD();
                                            cSCOSHEQD.CONO = cSCOSHEQ.CONO;
                                            cSCOSHEQD.PRSCODE = cSCOSHEQ.PRSCODE;
                                            cSCOSHEQD.MVTYPE = cSCOSHEQ.MVTYPE;
                                            cSCOSHEQD.MVNO = cSCOSHEQ.MVNO;
                                            cSCOSHEQD.MVID = cSCOSHEQ.MVID;
                                            cSCOSHEQD.STAMP = 0;
                                            db.CSCOSHEQDs.Add(cSCOSHEQD);

                                        }
                                        cSCOSHEQD.INMVTYPE = csSource.MVTYPE;
                                        cSCOSHEQD.INMVNO = csSource.MVNO;
                                        cSCOSHEQD.INMVID = csSource.MVID;
                                        cSCOSHEQD.OUTMVTYPE = cSCOSHEQ.MVTYPE;
                                        cSCOSHEQD.OUTMVNO = cSCOSHEQ.MVNO;
                                        cSCOSHEQD.OUTMVID = cSCOSHEQ.MVID;
                                        cSCOSHEQD.MVDATE = cSCOSHEQ.MVDATE;
                                        cSCOSHEQD.SHAREAMT = cSCOSHEQ.SHAREAMT;

                                    }
                                    else
                                    {
                                        throw new Exception("Unable to find source cert " + cSCOSHEQ.CERTNO);
                                    }
                                }
                            }

                            bool detChanged = false;
                            CSCOSHEQ csDetOrig = db.CSCOSHEQs.Where(x => x.CONO == cSCOSHEQ.CONO && x.MVNO == cSCOSHEQ.MVNO && x.MVTYPE == cSCOSHEQ.MVTYPE && x.MVID == 1).FirstOrDefault();
                            if (csDetOrig != null)
                            {
                                detChanged = (csDetOrig.PRSCODE != cSCOSHEQ.PRSCODETO);
                                if (detChanged)
                                {
                                    db.CSCOSHEQs.Remove(csDetOrig);
                                }
                            }

                            CSCOSHEQ csDet = db.CSCOSHEQs.Find(cSCOSHEQ.CONO, cSCOSHEQ.PRSCODETO, cSCOSHEQ.MVNO, 1, cSCOSHEQ.MVTYPE);
                            if (csDet == null)
                            {
                                csDet = new CSCOSHEQ();
                                csDet.CONO = cSCOSHEQ.CONO;
                                csDet.MVNO = cSCOSHEQ.MVNO;
                                csDet.MVID = 1;
                                csDet.MVTYPE = cSCOSHEQ.MVTYPE;
                                csDet.PRSCODE = cSCOSHEQ.PRSCODETO;
                                csDet.MVDATE = cSCOSHEQ.MVDATE;
                                csDet.MVSIGN = "In";
                                csDet.FOLIONOSRC = cSCOSHEQ.FOLIONOSRC;
                                csDet.EQCODE = cSCOSHEQ.EQCODE;
                                csDet.SCRIPT = cSCOSHEQ.SCRIPT;
                                csDet.CERTNO = null;
                                db.CSCOSHEQs.Add(csDet);
                            }
                            else
                            {
                                db.Entry(csDet).State = EntityState.Modified;
                            }

                            cSCOSHEQ.FOLIONOSRC = cSCOSHEQ.FOLIONOTO;

                            if ((csDet.SCRIPT == "Y") && (String.IsNullOrEmpty(csDet.CERTNO)))
                            {
                                CSCOLASTNO csCertNo = db.CSCOLASTNOes.Find(cSCOSHEQ.CONO, "CERTNO");
                                csDet.CERTNO = (csCertNo.LASTNO + 1).ToString("D" + (csCertNo.LASTWD).ToString());
                                csCertNo.LASTNO = csCertNo.LASTNO + 1;
                                db.Entry(csCertNo).State = EntityState.Modified;
                            }
                            csDet.SHAREAMT = cSCOSHEQ.SHAREAMT;
                            csDet.SSHAREAMT = cSCOSHEQ.SHAREAMT;
                            csDet.SHAREOS = cSCOSHEQ.SHAREAMT;
                            csDet.AMT = cSCOSHEQ.AMT;
                            csDet.SAMT = cSCOSHEQ.AMT;
                            csDet.REM = cSCOSHEQ.REM;
                            csDet.COMPLETE = "N";
                            csDet.COMPLETED = new DateTime(3000, 1, 1);
                            csDet.STAMP = 0;
                            csDet.REFCNT = 0;



                            EMTRTM eMTRTM = db.EMTRTMs.Find(cSCOSHEQ.CONO, cSCOSHEQ.MVNO);
                            if (eMTRTM != null)
                            {
                                db.Entry(eMTRTM).State = EntityState.Modified;
                            }
                            else
                            {
                                eMTRTM = new EMTRTM();
                                eMTRTM.CONO = cSCOSHEQ.CONO;
                                eMTRTM.TRNO = cSCOSHEQ.MVNO;
                                db.EMTRTMs.Add(eMTRTM);
                            }
                            eMTRTM.TRDATE = cSCOSHEQ.MVDATE;
                            eMTRTM.FOLIONOFR = db.CSCOSHes.Where(x => x.CONO == cSCOSHEQ.CONO && x.PRSCODE == cSCOSHEQ.PRSCODE).Select(y => y.FOLIONO).FirstOrDefault();
                            eMTRTM.EQCODEFR = cSCOSHEQ.EQCODE;
                            eMTRTM.PRSCODEFR = cSCOSHEQ.PRSCODE;
                            eMTRTM.SCRIPTFR = cSCOSHEQ.SCRIPT;
                            eMTRTM.CERTNOFR = cSCOSHEQ.CERTNO;
                            eMTRTM.SHAREAMT = cSCOSHEQ.SHAREAMT;
                            eMTRTM.AMT = cSCOSHEQ.AMT;
                            eMTRTM.REM = cSCOSHEQ.REM;
                            eMTRTM.PRSCODETO = cSCOSHEQ.PRSCODETO;
                            eMTRTM.FOLIONOTO = cSCOSHEQ.FOLIONOTO ?? 0;
                            eMTRTM.STAMP = cSCOSHEQ.STAMP;

                            EMTRTD eMTRTD = db.EMTRTDs.Find(cSCOSHEQ.CONO, cSCOSHEQ.MVNO, csDet.MVID);
                            if (eMTRTD != null)
                            {
                                db.Entry(eMTRTD).State = EntityState.Modified;
                            }
                            else
                            {
                                eMTRTD = new EMTRTD();
                                eMTRTD.CONO = cSCOSHEQ.CONO;
                                eMTRTD.TRNO = cSCOSHEQ.MVNO;
                                eMTRTD.TRID = csDet.MVID;
                                db.EMTRTDs.Add(eMTRTD);
                            }

                            eMTRTD.EQCODE = csDet.EQCODE;
                            eMTRTD.SCRIPT = csDet.SCRIPT;
                            eMTRTD.CERTNO = csDet.CERTNO;
                            eMTRTD.SHAREAMT = csDet.SHAREAMT;
                            eMTRTD.AMT = csDet.AMT;
                            eMTRTD.STAMP = 0;
                        }
                    }
                    else if (cSCOSHEQ.MVTYPE == "Split")
                    {

                        if (cSCOSHEQ.MVSIGN == "In")
                        {

                            cSCOSHEQ.SSHAREAMT = cSCOSHEQ.SHAREAMT;
                            cSCOSHEQ.SHAREOS = cSCOSHEQ.SHAREAMT;
                            cSCOSHEQ.SAMT = cSCOSHEQ.AMT;
                            cSCOSHEQ.COMPLETE = "N";
                            cSCOSHEQ.COMPLETED = new DateTime(3000, 1, 1);
                            cSCOSHEQ.STAMP = 0;
                            cSCOSHEQ.REFCNT = 0;

                            EMTRSD eMTRSD = db.EMTRSDs.Find(cSCOSHEQ.CONO, cSCOSHEQ.MVNO, cSCOSHEQ.MVID);
                            if (eMTRSD != null)
                            {
                                eMTRSD.STAMP = eMTRSD.STAMP + 1;
                                db.Entry(eMTRSD).State = EntityState.Modified;
                            }
                            else
                            {
                                eMTRSD = new EMTRSD();
                                eMTRSD.CONO = cSCOSHEQ.CONO;
                                eMTRSD.TRNO = cSCOSHEQ.MVNO;
                                eMTRSD.TRID = cSCOSHEQ.MVID;
                                eMTRSD.STAMP = 0;
                                db.EMTRSDs.Add(eMTRSD);
                            }

                            eMTRSD.EQCODE = cSCOSHEQ.EQCODE;
                            eMTRSD.SCRIPT = cSCOSHEQ.SCRIPT;
                            eMTRSD.CERTNO = cSCOSHEQ.CERTNO;
                            eMTRSD.SHAREAMT = cSCOSHEQ.SHAREAMT;

                        }
                        else
                        {
                            cSCOSHEQ.SSHAREAMT = -cSCOSHEQ.SHAREAMT;
                            cSCOSHEQ.SHAREOS = cSCOSHEQ.SHAREOS - cSCOSHEQ.SHAREAMT;
                            cSCOSHEQ.SAMT = -cSCOSHEQ.AMT;
                            cSCOSHEQ.COMPLETE = "Y";
                            cSCOSHEQ.COMPLETED = cSCOSHEQ.MVDATE;


                            CSCOSHEQ csOrig = csOrigSave;
                            if (csOrig == null)
                            {
                                throw new Exception("Unable to find original record " + csOrig.MVNO + " " + csOrig.MVID.ToString());
                            }
                            else
                            {
                                CSCOSHEQ csOrigSource = db.CSCOSHEQs.Where(x => x.CONO == csOrig.CONO && x.PRSCODE == csOrig.PRSCODE && x.CERTNO == csOrig.CERTNO && x.SCRIPT == csOrig.SCRIPT && x.MVNO != cSCOSHEQ.MVNO).FirstOrDefault();
                                if (csOrigSource == null)
                                {
                                    throw new Exception("Unable to find source cert " + csOrig.CERTNO);
                                }
                                else
                                {

                                    CSCOSHEQ csSource = db.CSCOSHEQs.Where(x => x.CONO == cSCOSHEQ.CONO && x.PRSCODE == cSCOSHEQ.PRSCODE && x.CERTNO == cSCOSHEQ.CERTNO && x.SCRIPT == cSCOSHEQ.SCRIPT).FirstOrDefault();
                                    if (csSource != null)
                                    {
                                        if (csOrig.CERTNO != csSource.CERTNO)
                                        {  // if the source cert is changed then reverse the orig source cert status

                                            csOrigSource.COMPLETE = "N";
                                            csOrigSource.SHAREOS = csOrigSource.SHAREAMT;
                                            csOrigSource.COMPLETED = new DateTime(3000, 1, 1);
                                            csOrigSource.REFCNT = csOrigSource.REFCNT - 1;
                                            csOrigSource.STAMP = csOrigSource.STAMP + 1;
                                            db.Entry(csOrigSource).State = EntityState.Modified;
                                            //db.SaveChanges();

                                            csSource.COMPLETE = "Y";
                                            csSource.SHAREOS = cSCOSHEQ.SHAREOS;
                                            csSource.COMPLETED = cSCOSHEQ.MVDATE;
                                            csSource.REFCNT = csSource.REFCNT + 1;
                                            csSource.STAMP = csSource.STAMP + 1;
                                            db.Entry(csSource).State = EntityState.Modified;
                                            //db.SaveChanges();
                                        }
                                        //db.Entry(csOrig).State = EntityState.Detached; // no longer needed and would cause current record to fail to modify


                                        CSCOSHEQD cSCOSHEQD = db.CSCOSHEQDs.Find(cSCOSHEQ.CONO, cSCOSHEQ.PRSCODE, cSCOSHEQ.MVTYPE, cSCOSHEQ.MVNO, cSCOSHEQ.MVID);
                                        if (cSCOSHEQD != null)
                                        {
                                            cSCOSHEQD.STAMP = cSCOSHEQD.STAMP + 1;
                                            db.Entry(cSCOSHEQD).State = EntityState.Modified;
                                        }
                                        else
                                        {
                                            cSCOSHEQD = new CSCOSHEQD();
                                            cSCOSHEQD.CONO = cSCOSHEQ.CONO;
                                            cSCOSHEQD.PRSCODE = cSCOSHEQ.PRSCODE;
                                            cSCOSHEQD.MVTYPE = cSCOSHEQ.MVTYPE;
                                            cSCOSHEQD.MVNO = cSCOSHEQ.MVNO;
                                            cSCOSHEQD.MVID = cSCOSHEQ.MVID;
                                            cSCOSHEQD.STAMP = 0;
                                            db.CSCOSHEQDs.Add(cSCOSHEQD);

                                        }
                                        cSCOSHEQD.INMVTYPE = csSource.MVTYPE;
                                        cSCOSHEQD.INMVNO = csSource.MVNO;
                                        cSCOSHEQD.INMVID = csSource.MVID;
                                        cSCOSHEQD.OUTMVTYPE = cSCOSHEQ.MVTYPE;
                                        cSCOSHEQD.OUTMVNO = cSCOSHEQ.MVNO;
                                        cSCOSHEQD.OUTMVID = cSCOSHEQ.MVID;
                                        cSCOSHEQD.MVDATE = cSCOSHEQ.MVDATE;
                                        cSCOSHEQD.SHAREAMT = cSCOSHEQ.SHAREAMT;

                                        if (changed)
                                        {
                                            foreach (var detItem in csOrig.Details)
                                            {
                                                CSCOSHEQ csDel = db.CSCOSHEQs.Find(detItem.CONO, detItem.PRSCODE, detItem.MVNO, detItem.MVID, detItem.MVTYPE);


                                                CSCOSHEQ csItem = new CSCOSHEQ();
                                                csItem.CONO = csDel.CONO;
                                                csItem.PRSCODE = cSCOSHEQ.PRSCODE;
                                                csItem.MVNO = csDel.MVNO;
                                                csItem.MVID = csDel.MVID;
                                                csItem.MVTYPE = csDel.MVTYPE;
                                                csItem.MVDATE = csDel.MVDATE;
                                                csItem.MVSIGN = csDel.MVSIGN;
                                                csItem.FOLIONOSRC = null;
                                                csItem.EQCODE = csDel.EQCODE;
                                                csItem.SCRIPT = csDel.SCRIPT;
                                                csItem.CERTNO = csDel.CERTNO;
                                                csItem.SHAREAMT = csDel.SHAREAMT;
                                                csItem.SSHAREAMT = csDel.SSHAREAMT;
                                                csItem.SHAREOS = csDel.SHAREOS;
                                                csItem.AMT = csDel.AMT;
                                                csItem.SAMT = csDel.SAMT;
                                                csItem.REM = cSCOSHEQ.REM;
                                                csItem.REFCNT = csDel.REFCNT;
                                                csItem.COMPLETE = csDel.COMPLETE;
                                                csItem.COMPLETED = csDel.COMPLETED;
                                                csItem.STAMP = csDel.STAMP + 1;

                                                db.CSCOSHEQs.Add(csItem);
                                                db.CSCOSHEQs.Remove(csDel);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        throw new Exception("Unable to find source cert " + cSCOSHEQ.CERTNO);
                                    }
                                }
                            }



                            EMTRSM eMTRSM = db.EMTRSMs.Find(cSCOSHEQ.CONO, cSCOSHEQ.MVNO);
                            if (eMTRSM != null)
                            {
                                eMTRSM.STAMP = eMTRSM.STAMP + 1;
                                db.Entry(eMTRSM).State = EntityState.Modified;
                            }
                            else
                            {
                                eMTRSM = new EMTRSM();
                                eMTRSM.CONO = cSCOSHEQ.CONO;
                                eMTRSM.TRNO = cSCOSHEQ.MVNO;
                                eMTRSM.STAMP = 0;
                                db.EMTRSMs.Add(eMTRSM);
                            }
                            eMTRSM.TRDATE = cSCOSHEQ.MVDATE;
                            eMTRSM.FOLIONOFR = db.CSCOSHes.Where(x => x.CONO == cSCOSHEQ.CONO && x.PRSCODE == cSCOSHEQ.PRSCODE).Select(y => y.FOLIONO).FirstOrDefault();
                            eMTRSM.EQCODEFR = cSCOSHEQ.EQCODE;
                            eMTRSM.PRSCODEFR = cSCOSHEQ.PRSCODE;
                            eMTRSM.SCRIPTFR = cSCOSHEQ.SCRIPT;
                            eMTRSM.CERTNOFR = cSCOSHEQ.CERTNO;
                            eMTRSM.SHAREAMT = cSCOSHEQ.SHAREAMT;
                            eMTRSM.REM = cSCOSHEQ.REM;


                        }
                    }


                    if (!changed)
                    {
                        db.Entry(cSCOSHEQ).State = EntityState.Modified;
                    }
                    db.SaveChanges();
                    return RedirectToAction("ShareMove", "CSCOSHes", new { id = MyHtmlHelpers.ConvertIdToByteStr(cSCOSHEQ.CONO) });
                }
                catch (DbEntityValidationException dbEx)
                {
                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            //string message = string.Format("{0}:{1}",
                            //    validationErrors.Entry.Entity.ToString(),
                            //   validationError.ErrorMessage);
                            // raise a new exception nesting
                            // the current instance as InnerException
                            ModelState.AddModelError(validationError.PropertyName, validationError.ErrorMessage);
                        }
                    }
                }
                catch (DbUpdateException ex)
                {
                    UpdateException updateException = (UpdateException)ex.InnerException;
                    if (updateException != null)
                    {
                        if (updateException.InnerException != null)
                        {
                            var sqlException = (FirebirdSql.Data.FirebirdClient.FbException)updateException.InnerException;

                            foreach (var error in sqlException.Errors)
                            {
                                if (error.Message != null)
                                {
                                    ModelState.AddModelError(string.Empty, error.Message);
                                }
                            }
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, updateException.Message);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, updateException.Message);
                    }
                }
                catch (Exception e)
                {
                    ModelState.AddModelError(string.Empty, e.Message);
                }
                finally
                {

                }
            }

            ViewBag.PRSCODE = new SelectList(db.CSCOSHes.Where(x => x.CONO == cSCOSHEQ.CONO).Select(x => new { PRSCODE = x.PRSCODE, PRSNAME = x.CSPR.PRSNAME + " | " + x.FOLIONO }), "PRSCODE", "PRSNAME", cSCOSHEQ.PRSCODE);
            ViewBag.PRSCODETO = new SelectList(db.CSCOSHes.Where(x => x.CONO == cSCOSHEQ.CONO && x.PRSCODE != cSCOSHEQ.PRSCODE).Select(x => new { PRSCODE = x.PRSCODE, PRSNAME = x.CSPR.PRSNAME + " | " + x.FOLIONO }), "PRSCODE", "PRSNAME", cSCOSHEQ.PRSCODETO);
            ViewBag.EQCODE = new SelectList(db.CSEQs.OrderBy(x => x.EQCODE), "EQCODE", "EQDESC", cSCOSHEQ.EQCODE);
            ViewBag.EQID = new SelectList(db.CSCOSHEQs.Where(x => x.CONO == cSCOSHEQ.CONO && x.MVSIGN == "In" && x.PRSCODE == cSCOSHEQ.PRSCODE && (x.COMPLETE == "N" ||
            (x.SCRIPT == cSCOSHEQ.SCRIPT && x.CERTNO == cSCOSHEQ.CERTNO))).Select(x => new
            {
                EQID = x.EQCODE + " | " + x.SCRIPT + " | " + x.CERTNO,
                EQDESC = x.EQCODE + " | " + x.CSEQ.EQDESC + " | " + x.CSEQ.EQCAT + " | " + x.SCRIPT + " | " + x.CERTNO + " | " + (x.COMPLETE == "Y" ? x.SHAREAMT : x.SHAREOS)
            }), "EQID", "EQDESC", cSCOSHEQ.EQID);
            ViewBag.Title = "Edit Share " + cSCOSHEQ.MVTYPE;
            ViewBag.Parent = cSCOSHEQ.CONO;
            ViewBag.MVTYPE = cSCOSHEQ.MVTYPE;
            ViewBag.MVNO = cSCOSHEQ.MVNO;
            ViewBag.person = cSCOSHEQ.PRSCODE;
            ViewBag.equity = cSCOSHEQ.EQCODE;
            cSCOSHEQ.CSCOSH = db.CSCOSHes.Find(cSCOSHEQ.CONO, cSCOSHEQ.PRSCODE);
            return View(ViewName, cSCOSHEQ);
        }

        // GET: CSCOSHEQs/Delete/5
        public ActionResult Delete(string id, string person, string mvno, int mvid, string mvtype)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            string sid = MyHtmlHelpers.ConvertByteStrToId(id);
            CSCOSHEQ cSCOSHEQ = db.CSCOSHEQs.Find(sid, person, mvno, mvid, mvtype);
            if (cSCOSHEQ == null)
            {
                return HttpNotFound();
            }
            string ViewName = "Edit1";

            if (mvtype == "Allotment")
            {
                ViewName = "Edit";
            }
            else if (mvtype == "Transfer")
            {
                if (cSCOSHEQ.MVSIGN == "In") { ViewName = "Edit"; }
                else
                {
                    CSCOSHEQ cSTransferee = db.CSCOSHEQs.Where(x => x.CONO == sid && x.MVNO == mvno && x.MVID == 1 && x.MVTYPE == mvtype).FirstOrDefault();
                    cSCOSHEQ.FOLIONOTO = cSTransferee.CSCOSH.FOLIONO;
                    cSCOSHEQ.PRSCODETO = cSTransferee.PRSCODE;
                    cSCOSHEQ.SHAREOS = cSCOSHEQ.SHAREAMT; // revert back to original value 
                }
            }
            else if (mvtype == "Split")
            {
                if (cSCOSHEQ.MVSIGN == "In") { ViewName = "Edit"; }
                else
                {
                    cSCOSHEQ.SHAREOS = cSCOSHEQ.SHAREAMT; // revert back to original value 
                }
            }
            else if (mvtype == "Conversion")
            {
                if (cSCOSHEQ.MVSIGN == "In") { ViewName = "Edit"; }
            }

            ViewBag.PRSCODE = new SelectList(db.CSCOSHes.Where(x => x.CONO == cSCOSHEQ.CONO).Select(x => new { PRSCODE = x.PRSCODE, PRSNAME = x.CSPR.PRSNAME + " | " + x.FOLIONO }), "PRSCODE", "PRSNAME", cSCOSHEQ.PRSCODE);
            ViewBag.PRSCODETO = new SelectList(db.CSCOSHes.Where(x => x.CONO == cSCOSHEQ.CONO && x.PRSCODE != cSCOSHEQ.PRSCODE).Select(x => new { PRSCODE = x.PRSCODE, PRSNAME = x.CSPR.PRSNAME + " | " + x.FOLIONO }), "PRSCODE", "PRSNAME", cSCOSHEQ.PRSCODETO);
            ViewBag.EQCODE = new SelectList(db.CSEQs.OrderBy(x => x.EQCODE), "EQCODE", "EQDESC", cSCOSHEQ.EQCODE);
            ViewBag.EQID = new SelectList(db.CSCOSHEQs.Where(x => x.CONO == cSCOSHEQ.CONO && x.MVSIGN == "In" && x.PRSCODE == cSCOSHEQ.PRSCODE && (x.COMPLETE == "N" ||
            (x.SCRIPT == cSCOSHEQ.SCRIPT && x.CERTNO == cSCOSHEQ.CERTNO))).Select(x => new
            {
                EQID = x.EQCODE + " | " + x.SCRIPT + " | " + x.CERTNO,
                EQDESC = x.EQCODE + " | " + x.CSEQ.EQDESC + " | " + x.CSEQ.EQCAT + " | " + x.SCRIPT + " | " + x.CERTNO + " | " + (x.COMPLETE == "Y" ? x.SHAREAMT : x.SHAREOS)
            }), "EQID", "EQDESC", cSCOSHEQ.EQID);
            ViewBag.Title = "Delete Share " + cSCOSHEQ.MVTYPE;
            ViewBag.Parent = cSCOSHEQ.CONO;
            ViewBag.MVTYPE = mvtype;
            ViewBag.MVNO = cSCOSHEQ.MVNO;
            ViewBag.person = cSCOSHEQ.PRSCODE;
            ViewBag.equity = cSCOSHEQ.EQCODE;

            Session["CSCOSHEQEditViewName"] = ViewName;
            return View(ViewName, cSCOSHEQ);
        }

        // POST: CSCOSHEQs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id, string person, string mvno, int mvid, string mvtype)
        {
            CSCOSHEQ cSCOSHEQ = db.CSCOSHEQs.Find(MyHtmlHelpers.ConvertByteStrToId(id), person, mvno, mvid, mvtype);

            if (cSCOSHEQ.MRefCnt > 0)
            {
                ModelState.AddModelError(string.Empty, "Cannot Remove records that is used " + cSCOSHEQ.MRefCnt.ToString());
            }
            else
            {
                try
                {
                    if (cSCOSHEQ.MVSIGN == "Out")
                    {
                        CSCOSHEQ csSource = db.CSCOSHEQs.Where(x => x.CONO == cSCOSHEQ.CONO && x.PRSCODE == cSCOSHEQ.PRSCODE && x.CERTNO == cSCOSHEQ.CERTNO && x.SCRIPT == cSCOSHEQ.SCRIPT).FirstOrDefault();
                        if (csSource != null)
                        {


                            csSource.COMPLETE = "N";
                            csSource.SHAREOS = csSource.SHAREOS + cSCOSHEQ.SHAREAMT;
                            csSource.COMPLETED = new DateTime(3000, 1, 1);
                            csSource.REFCNT = csSource.REFCNT - 1;
                            csSource.STAMP = csSource.STAMP + 1;
                            db.Entry(csSource).State = EntityState.Modified;

                            CSCOSHEQD cSCOSHEQD = db.CSCOSHEQDs.Find(cSCOSHEQ.CONO, cSCOSHEQ.PRSCODE, cSCOSHEQ.MVTYPE, cSCOSHEQ.MVNO, cSCOSHEQ.MVID);
                            if (cSCOSHEQD != null)
                            {
                                db.CSCOSHEQDs.Remove(cSCOSHEQD);
                            }
                        
                        }

                        if (cSCOSHEQ.MVTYPE == "Split")
                        {
                            List<CSCOSHEQ> csDets = db.CSCOSHEQs.Where(x => x.CONO == cSCOSHEQ.CONO && x.MVNO == cSCOSHEQ.MVNO).ToList();
                            foreach (var item in csDets)
                            {
                                CSCOSHEQ csDet = db.CSCOSHEQs.Find(item.CONO, item.PRSCODE, item.MVNO, item.MVID, item.MVTYPE);
                                if (csDet != null)
                                {
                                    db.CSCOSHEQs.Remove(csDet);
                                }
                            }

                            List<EMTRSD> emDets = db.EMTRSDs.Where(x => x.CONO == cSCOSHEQ.CONO && x.TRNO == cSCOSHEQ.MVNO).ToList();
                            foreach( var item in emDets)
                            {
                                EMTRSD emDet = db.EMTRSDs.Find(item.CONO, item.TRNO, item.TRID);
                                if (emDet != null)
                                {
                                    db.EMTRSDs.Remove(emDet);
                                }
                            }

                            EMTRSM emRec = db.EMTRSMs.Find(cSCOSHEQ.CONO, cSCOSHEQ.MVNO);
                            if (emRec != null)
                            {
                               db.EMTRSMs.Remove(emRec);
                            }
                        }
                        else if (cSCOSHEQ.MVTYPE == "Transfer")
                        {
                            List<CSCOSHEQ> csDets = db.CSCOSHEQs.Where(x => x.CONO == cSCOSHEQ.CONO && x.MVNO == cSCOSHEQ.MVNO).ToList();
                            foreach (var item in csDets)
                            {
                                CSCOSHEQ csDet = db.CSCOSHEQs.Find(item.CONO, item.PRSCODE, item.MVNO, item.MVID, item.MVTYPE);
                                if (csDet != null)
                                {
                                    db.CSCOSHEQs.Remove(csDet);
                                }
                            }

                            List<EMTRTD> emDets = db.EMTRTDs.Where(x => x.CONO == cSCOSHEQ.CONO && x.TRNO == cSCOSHEQ.MVNO).ToList();
                            foreach (var item in emDets)
                            {
                                EMTRTD emDet = db.EMTRTDs.Find(item.CONO, item.TRNO, item.TRID);
                                if (emDet != null)
                                {
                                    db.EMTRTDs.Remove(emDet);
                                }
                            }

                            EMTRTM emRec = db.EMTRTMs.Find(cSCOSHEQ.CONO, cSCOSHEQ.MVNO);
                            if (emRec != null)
                            {
                                db.EMTRTMs.Remove(emRec);
                            }
                        }
                        else if (cSCOSHEQ.MVTYPE == "Conversion")
                        {

                            EMTRCD emRec = db.EMTRCDs.Find(cSCOSHEQ.CONO, cSCOSHEQ.MVNO, cSCOSHEQ.MVID);
                            if (emRec != null)
                            {
                                db.EMTRCDs.Remove(emRec);
                            }
                        }
                    } else
                    { // mvtype = "In"
                        if (cSCOSHEQ.MVTYPE == "Allotment")
                        {
                            EMTRA emRec = db.EMTRAs.Find(cSCOSHEQ.CONO, cSCOSHEQ.MVNO);
                            if (emRec != null)
                            {
                                db.EMTRAs.Remove(emRec);
                            }
                        }
                        else if (cSCOSHEQ.MVTYPE == "Transfer")
                        {

                            EMTRTD emRec = db.EMTRTDs.Find(cSCOSHEQ.CONO, cSCOSHEQ.MVNO, cSCOSHEQ.MVID);
                            if (emRec != null)
                            {
                                db.EMTRTDs.Remove(emRec);
                            }
                        }
                        else if (cSCOSHEQ.MVTYPE == "Split")
                        {

                            EMTRSD emRec = db.EMTRSDs.Find(cSCOSHEQ.CONO, cSCOSHEQ.MVNO, cSCOSHEQ.MVID);
                            if (emRec != null)
                            {
                                db.EMTRSDs.Remove(emRec);
                            }
                        }
                        else if (cSCOSHEQ.MVTYPE == "Conversion")
                        {

                            List<EMTRCD> emDets = db.EMTRCDs.Where(x => x.CONO == cSCOSHEQ.CONO && x.TRNO == cSCOSHEQ.MVNO).ToList();
                            foreach (var item in emDets)
                            {
                                EMTRCD emDet = db.EMTRCDs.Find(item.CONO, item.TRNO, item.TRID);
                                if (emDet != null)
                                {
                                    db.EMTRCDs.Remove(emDet);
                                }
                            }

                            EMTRCM emRec = db.EMTRCMs.Find(cSCOSHEQ.CONO, cSCOSHEQ.MVNO);
                            if (emRec != null)
                            {
                                db.EMTRCMs.Remove(emRec);
                            }
                        }
                    }


                    db.CSCOSHEQs.Remove(cSCOSHEQ);
                    db.SaveChanges();
                    return RedirectToAction("ShareMove", "CSCOSHes", new { id = MyHtmlHelpers.ConvertIdToByteStr(cSCOSHEQ.CONO) });
                }
                catch (DbEntityValidationException dbEx)
                {
                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            //string message = string.Format("{0}:{1}",
                            //    validationErrors.Entry.Entity.ToString(),
                            //   validationError.ErrorMessage);
                            // raise a new exception nesting
                            // the current instance as InnerException
                            ModelState.AddModelError(validationError.PropertyName, validationError.ErrorMessage);
                        }
                    }
                }
                catch (DbUpdateException ex)
                {
                    UpdateException updateException = (UpdateException)ex.InnerException;
                    if (updateException != null)
                    {
                        if (updateException.InnerException != null)
                        {
                            var sqlException = (FirebirdSql.Data.FirebirdClient.FbException)updateException.InnerException;

                            foreach (var error in sqlException.Errors)
                            {
                                if (error.Message != null)
                                {
                                    ModelState.AddModelError(string.Empty, error.Message);
                                }
                            }
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, updateException.Message);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, updateException.Message);
                    }
                }
                catch (Exception e)
                {
                    ModelState.AddModelError(string.Empty, e.Message);
                }
                finally
                {

                }
            }

            ViewBag.PRSCODE = new SelectList(db.CSCOSHes.Where(x => x.CONO == cSCOSHEQ.CONO).Select(x => new { PRSCODE = x.PRSCODE, PRSNAME = x.CSPR.PRSNAME + " | " + x.FOLIONO }), "PRSCODE", "PRSNAME", cSCOSHEQ.PRSCODE);
            ViewBag.PRSCODETO = new SelectList(db.CSCOSHes.Where(x => x.CONO == cSCOSHEQ.CONO && x.PRSCODE != cSCOSHEQ.PRSCODE).Select(x => new { PRSCODE = x.PRSCODE, PRSNAME = x.CSPR.PRSNAME + " | " + x.FOLIONO }), "PRSCODE", "PRSNAME", cSCOSHEQ.PRSCODETO);
            ViewBag.EQCODE = new SelectList(db.CSEQs.OrderBy(x => x.EQCODE), "EQCODE", "EQDESC", cSCOSHEQ.EQCODE);
            ViewBag.EQID = new SelectList(db.CSCOSHEQs.Where(x => x.CONO == cSCOSHEQ.CONO && x.MVSIGN == "In" && x.PRSCODE == cSCOSHEQ.PRSCODE && (x.COMPLETE == "N" ||
            (x.SCRIPT == cSCOSHEQ.SCRIPT && x.CERTNO == cSCOSHEQ.CERTNO))).Select(x => new
            {
                EQID = x.EQCODE + " | " + x.SCRIPT + " | " + x.CERTNO,
                EQDESC = x.EQCODE + " | " + x.CSEQ.EQDESC + " | " + x.CSEQ.EQCAT + " | " + x.SCRIPT + " | " + x.CERTNO + " | " + (x.COMPLETE == "Y" ? x.SHAREAMT : x.SHAREOS)
            }), "EQID", "EQDESC", cSCOSHEQ.EQID);
            ViewBag.Title = "Delete Share " + cSCOSHEQ.MVTYPE;
            ViewBag.Parent = cSCOSHEQ.CONO;
            ViewBag.MVTYPE = mvtype;
            ViewBag.MVNO = cSCOSHEQ.MVNO;
            ViewBag.person = cSCOSHEQ.PRSCODE;
            ViewBag.equity = cSCOSHEQ.EQCODE;

            string ViewName = (string)Session["CSCOSHEQEditViewName"];
            return View(ViewName, cSCOSHEQ);
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
