using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PagedList;
using System.Data.Entity.Core;
using System.Data.Entity.Validation;
using System.Data.Entity.Infrastructure;
using WebApplication1.Utility;
using FirebirdSql.Data.Client;
using FirebirdSql.Data.FirebirdClient;

namespace WebApplication1.Controllers
{
    [Authorize(Roles = "Administrator,CS-A/C")]
    public class CSOFFsController : Controller
    {
        private ASIDBConnection db = new ASIDBConnection();

        public PartialViewResult Search()
        {
            CSOFF searchRec = null;
            //searchRec.CONO = pSearchCode;
            //searchRec.CONAME = pSearchName;
            if (Session["SearchOFFRec"] != null)
            {
                searchRec = (CSOFF)Session["SearchOffRec"];

            }
            else
            {
                searchRec = new CSOFF();
                searchRec.VDATE = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                searchRec.DUEDATE = searchRec.VDATE.AddMonths(1).AddDays(-1);
            }
            if (Session["SearchOFFSort"] == null)
            {
                Session["SearchOFFSort"] = "TRNO";
            };

            List<SelectListItem> listItems = new List<SelectListItem>();
            listItems.Add(new SelectListItem
            {
                Text = "Company Name",
                Value = "CONAME",
                Selected = (string)Session["SearchOFFSort"] == "CONAME"
            });

            listItems.Add(new SelectListItem
            {
                Text = "Item #",
                Value = "TRNO",
                Selected = (string)Session["SearchOFFSort"] == "TRNO"
            });

            listItems.Add(new SelectListItem
            {
                Text = "Item # Latest",
                Value = "TRNOLAST",
                Selected = (string)Session["SearchOFFSort"] == "TRNOLAST"
            });

            listItems.Add(new SelectListItem
            {
                Text = "Date",
                Value = "VDATE",
                Selected = (string)Session["SearchOFFSort"] == "VDATE"
            });

            listItems.Add(new SelectListItem
            {
                Text = "Archive",
                Value = "ARCHIVE",
                Selected = (string)Session["SearchOFFSort"] == "ARCHIVE"
            });
            ViewBag.SORTBY = listItems;
            return PartialView("Partial/Search", searchRec);
        }


        [HttpGet]
        public ActionResult SearchPost()
        {
            return Index(1);
        }

        [HttpPost]
        public ActionResult SearchPost(CSOFF cSOFF)
        {

            Session["SearchOFFRec"] = cSOFF;
            Session["SearchOFFSort"] = Request.Params["SORTBY"] ?? "VDATE";
            return Redirect("?page=1");
            //return Index(1);
        }

        public IQueryable<CSOFF> CurrentSelection()
        {
            string pSearchCode = "";
            string pSearchName = "";
            string pSearchOFF = "";
            DateTime pSearchVdate = DateTime.Parse("01/01/0001");
            DateTime pSearchDdate = DateTime.Parse("01/01/0001");

            if (Session["SearchOFFRec"] != null)
            {
                CSOFF searchRec = (CSOFF)(Session["SearchOFFRec"]);
                pSearchCode = searchRec.CSCOMSTR.COREGNO ?? "";
                pSearchName = searchRec.CSCOMSTR.CONAME ?? "";
                pSearchVdate = searchRec.VDATE;
                pSearchDdate = searchRec.DUEDATE;
                pSearchOFF = searchRec.TRNO ?? "";

            }
            else
            { // start with current month proforma bills instead of entire list
                pSearchVdate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);    
                pSearchDdate = pSearchVdate.AddMonths(1).AddDays(-1);
            }

            IQueryable<CSOFF> cSOFFs = db.CSOFFs;

            if ((string)Session["SearchOFFSort"] != "ARCHIVE") { cSOFFs = db.CSOFFs.Where(x => x.POST == "N"); }
            else { cSOFFs = db.CSOFFs.Where(x => x.POST == "Y"); }

            if (pSearchCode != "") { cSOFFs = cSOFFs.Where(x => x.CSCOMSTR.COREGNO.Contains(pSearchCode.ToUpper())); };
            if (pSearchName != "") { cSOFFs = cSOFFs.Where(x => x.CSCOMSTR.CONAME.Contains(pSearchName.ToUpper())); };
            if (pSearchVdate != DateTime.Parse("01/01/0001")) { cSOFFs = cSOFFs.Where(x => x.VDATE >= pSearchVdate); };
            if (pSearchDdate != DateTime.Parse("01/01/0001")) { cSOFFs = cSOFFs.Where(x => x.VDATE <= pSearchDdate); };
            if (pSearchOFF != "")
            {
                if (pSearchOFF.Length > 8)
                {
                    cSOFFs = cSOFFs.Where(x => x.TRNO == pSearchOFF);
                }
                else
                {
                    cSOFFs = cSOFFs.Where(x => x.TRNO.Contains(pSearchOFF));
                }
            };

            if ((string)Session["SearchOFFSort"] == "CONAME")
            {
                cSOFFs = cSOFFs.OrderBy(n => n.CSCOMSTR.CONAME);
            }
            else if ((string)Session["SearchOFFSort"] == "VDATE")
            {
                cSOFFs = cSOFFs.OrderBy(n => n.VDATE);

            }
            else if ((string)Session["SearchOFFSort"] == "TRNOLAST")
            {
                cSOFFs = cSOFFs.OrderByDescending(n => n.TRNO);

            }
            else
            {
                cSOFFs = cSOFFs.OrderBy(n => n.TRNO);
            }
            DateTime rptStart = pSearchVdate;
            DateTime rptEnd = pSearchDdate;

            ViewBag.RPT_START = rptStart.ToString("dd/MM/yyyy");
            ViewBag.RPT_END = rptEnd.ToString("dd/MM/yyyy");

            return cSOFFs;
        }

        // GET: CSOFFs
        public ActionResult Index(int? page)
        {
            return View(CurrentSelection().ToList().ToPagedList(page ?? 1, 30));
        }

        // GET: CSOFFs/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSOFF cSOFF = db.CSOFFs.Find(MyHtmlHelpers.ConvertByteStrToId(id));
            if (cSOFF == null)
            {
                return HttpNotFound();
            }
            SetDropDownList(cSOFF);
            ViewBag.Title = "View Offset Item ";

            return View("Edit", cSOFF);
        }

        protected void SetDropDownList(CSOFF cSOFF)
        {
            ViewBag.CONO = new SelectList(db.CSCOMSTRs.Select(x => new { CONO = x.CONO, CONAME = x.CONAME + "  (" + x.CONO + ")" }).OrderBy(y => y.CONAME), "CONO", "CONAME", cSOFF.CONO);
            ViewBag.DBKey = new SelectList(db.CSTRANMs.Where(x => (x.SOURCE == cSOFF.DBTYPE && x.SOURCENO == cSOFF.DBNO && x.SOURCEID == cSOFF.DBID) || (x.CONO == cSOFF.CONO && x.TRSIGN == "DB" && x.COMPLETE == "N" && x.SOURCE != "CSRCP-3P" && x.ENTDATE <= cSOFF.VDATE)).Select(x => new
            {
                KEY = x.SOURCE + "|" + x.SOURCENO + "|" + x.SOURCEID,
                DESC = x.ENTDATE + " | " + x.TROS + " | " + x.SOURCE + " | " + x.SOURCENO + " | " + x.SOURCEID + " | " + x.TRTYPE + " | " + x.TRDESC
            }), "KEY", "DESC", cSOFF.DBKey);
            ViewBag.CRKey = new SelectList(db.CSTRANMs.Where(x => (x.SOURCE == cSOFF.CRTYPE && x.SOURCENO == cSOFF.CRNO && x.SOURCEID == cSOFF.CRID) || (x.CONO == cSOFF.CONO && x.TRSIGN == "CR" && x.COMPLETE == "N" && x.SOURCE != "CSRCP-3P" && x.ENTDATE <= cSOFF.VDATE)).Select(x => new
            { KEY = x.SOURCE + "|" + x.SOURCENO + "|" + x.SOURCEID, DESC = x.ENTDATE + " | " + x.TROS + " | " + x.SOURCE + " | " + x.SOURCENO + " | " + x.SOURCEID + " | " + x.TRTYPE + " | " + x.TRDESC }), "KEY", "DESC", cSOFF.CRKey);

        }

        // GET: CSOFFs/Create
        public ActionResult Create()
        {
            CSOFF cSOFF = new CSOFF();
            cSOFF.VDATE = DateTime.Today;
            cSOFF.CONO = "750059-M";
            cSOFF.POST = "N";

            SetDropDownList(cSOFF);
            ViewBag.Title = "Create New Offset Item ";

            return View("Edit", cSOFF);
        }

        // POST: CSOFFs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TRNO,VDATE,CONO,DBKey,CRKey, DBTYPE,DBNO,DBID,CRTYPE,CRNO,CRID,APPDBITEM1,APPDBITEM2,APPDBITEM,APPDBTAX1,APPDBTAX2,APPDBTAX,APPDBAMT1,APPDBAMT2,APPDBAMT,APPCRITEM1,APPCRITEM2,APPCRITEM,APPCRTAX1,APPCRTAX2,APPCRTAX,APPCRAMT1,APPCRAMT2,APPCRAMT,REM,SEQNO,POST,STAMP")] CSOFF cSOFF)
        {
            if (ModelState.IsValid)
            {
                SALASTNO serialTbl = db.SALASTNOes.Find("CSOFF");
                if (serialTbl != null)
                {
                    CSCOMSTR cSCOMSTR = db.CSCOMSTRs.Find(cSOFF.CONO);
                    if (cSCOMSTR != null)
                    {
                        try
                        {
                            string prefix = serialTbl.LASTPFIX;
                            int MaxNo = serialTbl.LASTNOMAX;
                            bool AutoGen = serialTbl.AUTOGEN == "Y";
                            serialTbl.LASTNO = serialTbl.LASTNO + 1;
                            cSOFF.TRNO = serialTbl.LASTNO.ToString("D10");

                            serialTbl.STAMP = serialTbl.STAMP + 1;
                            db.Entry(serialTbl).State = EntityState.Modified;

                            // increment company seqno count before using it in transaction
                            cSCOMSTR.SEQNO = cSCOMSTR.SEQNO + 1;
                            cSCOMSTR.STAMP = cSCOMSTR.STAMP + 1;

                            cSOFF.SEQNO = cSCOMSTR.SEQNO;
                            db.Entry(cSCOMSTR).State = EntityState.Modified;

                            db.CSOFFs.Add(cSOFF);
                            //db.SaveChanges();
                            ApplyOffset(cSOFF);
                            db.SaveChanges();
                            return RedirectToAction("Index");
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
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Unable to find company #");
                    }
                }
            }
            SetDropDownList(cSOFF);
            return View("Edit", cSOFF);
        }

        public ActionResult EditCompany(CSOFF cSOFF)
        {

            //ModelState.Clear(); //apparent this has side-effects and using Remove is preferrable
            ModelState.Remove("CONO");
            ModelState.Remove("DBKey");
            ModelState.Remove("CRKey");

            cSOFF.DBKey = null;
            cSOFF.CRKey = null;

            SetDropDownList(cSOFF);
            return PartialView("Partial/EditCompany", cSOFF);


        }

        public ActionResult EditDBItem(CSOFF cSOFF)
        {

            //ModelState.Clear(); //apparent this has side-effects and using Remove is preferrable
            //ModelState.Remove("CONO");
            //ModelState.Remove("DBKey");
            //ModelState.Remove("CRKey");
            //cSOFF = db.CSOFFs.Find(cSOFF.TRNO);

            CSTRANM cSTRANMDB;
            CSTRANM cSTRANMCR;
            if (ModelState.IsValid)
            {
                cSOFF.CSTRANM = db.CSTRANMs.Find(cSOFF.DBTYPE, cSOFF.DBNO, cSOFF.DBID);

                CSTRANM cSTRANM = db.CSTRANMs.Find("CSOFF", cSOFF.TRNO, 1);
                cSTRANMDB = cSTRANM;
                cSTRANMCR = db.CSTRANMs.Find("CSOFF", cSOFF.TRNO, 2);

                if ((cSTRANM != null) && (cSTRANM.APPTYPE == cSOFF.CSTRANM.SOURCE && cSTRANM.APPNO == cSOFF.CSTRANM.SOURCENO && cSTRANM.APPID == cSOFF.CSTRANM.SOURCEID))
                {
                    // restore the original balance of applied offset
                    cSOFF.CSTRANM.TROS = cSOFF.CSTRANM.TROS + cSTRANM.APPAMT;
                    cSOFF.CSTRANM.TROS1 = cSOFF.CSTRANM.TROS1 + cSTRANM.APPAMT1;
                    cSOFF.CSTRANM.TROS2 = cSOFF.CSTRANM.TROS2 + cSTRANM.APPAMT2;
                    cSOFF.CSTRANM.TRTAXOS = cSOFF.CSTRANM.TRTAXOS + cSTRANM.APPTAX;
                    cSOFF.CSTRANM.TRTAXOS1 = cSOFF.CSTRANM.TRTAXOS1 + cSTRANM.APPTAX1;
                    cSOFF.CSTRANM.TRTAXOS2 = cSOFF.CSTRANM.TRTAXOS2 + cSTRANM.APPTAX2;
                    cSOFF.CSTRANM.TRITEMOS = cSOFF.CSTRANM.TRITEMOS + cSTRANM.APPITEM;
                    cSOFF.CSTRANM.TRITEMOS1 = cSOFF.CSTRANM.TRITEMOS1 + cSTRANM.APPITEM1;
                    cSOFF.CSTRANM.TRITEMOS2 = cSOFF.CSTRANM.TRITEMOS2 + cSTRANM.APPITEM2;
                }
                SetDropDownList(cSOFF);
            }


            return PartialView("Partial/EditDBItem", cSOFF);


        }

        public ActionResult EditCRItem(CSOFF cSOFF)
        {

            //ModelState.Clear(); //apparent this has side-effects and using Remove is preferrable
            //ModelState.Remove("CONO");
            //ModelState.Remove("DBKey");
            //ModelState.Remove("CRKey");
            //cSOFF = db.CSOFFs.Find(cSOFF.TRNO);

            CSTRANM cSTRANMDB;
            CSTRANM cSTRANMCR;
            if (ModelState.IsValid)
            {
                cSOFF.CSTRANM1 = db.CSTRANMs.Find(cSOFF.CRTYPE, cSOFF.CRNO, cSOFF.CRID);
                cSTRANMDB = db.CSTRANMs.Find("CSOFF", cSOFF.TRNO, 1);
                CSTRANM cSTRANM = db.CSTRANMs.Find("CSOFF", cSOFF.TRNO, 2);
                cSTRANMCR = cSTRANM;

                // restore balance to before apply offset
                if ((cSTRANM != null) && (cSTRANM.APPTYPE == cSOFF.CSTRANM1.SOURCE && cSTRANM.APPNO == cSOFF.CSTRANM1.SOURCENO && cSTRANM.APPID == cSOFF.CSTRANM1.SOURCEID))
                {
                    // restore the original balance of applied offset
                    cSOFF.CSTRANM1.TROS = -cSOFF.CSTRANM1.TROS + cSTRANM.APPAMT;
                    cSOFF.CSTRANM1.TROS1 = -cSOFF.CSTRANM1.TROS1 + cSTRANM.APPAMT1;
                    cSOFF.CSTRANM1.TROS2 = -cSOFF.CSTRANM1.TROS2 + cSTRANM.APPAMT2;
                    cSOFF.CSTRANM1.TRTAXOS = -cSOFF.CSTRANM1.TRTAXOS + cSTRANM.APPTAX;
                    cSOFF.CSTRANM1.TRTAXOS1 = -cSOFF.CSTRANM1.TRTAXOS1 + cSTRANM.APPTAX1;
                    cSOFF.CSTRANM1.TRTAXOS2 = -cSOFF.CSTRANM1.TRTAXOS2 + cSTRANM.APPTAX2;
                    cSOFF.CSTRANM1.TRITEMOS = -cSOFF.CSTRANM1.TRITEMOS + cSTRANM.APPITEM;
                    cSOFF.CSTRANM1.TRITEMOS1 = -cSOFF.CSTRANM1.TRITEMOS1 + cSTRANM.APPITEM1;
                    cSOFF.CSTRANM1.TRITEMOS2 = -cSOFF.CSTRANM1.TRITEMOS2 + cSTRANM.APPITEM2;
                }
                else
                {
                    // show CR items as positive
                    cSOFF.CSTRANM1.TROS = -cSOFF.CSTRANM1.TROS;
                    cSOFF.CSTRANM1.TROS1 = -cSOFF.CSTRANM1.TROS1;
                    cSOFF.CSTRANM1.TROS2 = -cSOFF.CSTRANM1.TROS2;
                    cSOFF.CSTRANM1.TRTAXOS = -cSOFF.CSTRANM1.TRTAXOS;
                    cSOFF.CSTRANM1.TRTAXOS1 = -cSOFF.CSTRANM1.TRTAXOS1;
                    cSOFF.CSTRANM1.TRTAXOS2 = -cSOFF.CSTRANM1.TRTAXOS2;
                    cSOFF.CSTRANM1.TRITEMOS = -cSOFF.CSTRANM1.TRITEMOS;
                    cSOFF.CSTRANM1.TRITEMOS1 = -cSOFF.CSTRANM1.TRITEMOS1;
                    cSOFF.CSTRANM1.TRITEMOS2 = -cSOFF.CSTRANM1.TRITEMOS2;
                }

                SetDropDownList(cSOFF);
            }


            return PartialView("Partial/EditCRItem", cSOFF);


        }


        // GET: CSOFFs/Edit/5
        public ActionResult Edit(string id, int? page)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSOFF cSOFF = db.CSOFFs.Find(MyHtmlHelpers.ConvertByteStrToId(id));
            if (cSOFF == null)
            {
                return HttpNotFound();
            }

            //cSOFF.CSTRANM = db.CSTRANMs.Find(cSOFF.DBTYPE, cSOFF.DBNO, cSOFF.DBID);
            CSTRANM cSTRANM = db.CSTRANMs.Find("CSOFF", cSOFF.TRNO, 1);
            if ((cSTRANM != null) && (cSTRANM.APPTYPE == cSOFF.CSTRANM.SOURCE && cSTRANM.APPNO == cSOFF.CSTRANM.SOURCENO && cSTRANM.APPID == cSOFF.CSTRANM.SOURCEID))
            {
                // restore the original balance of applied offset
                cSOFF.CSTRANM.TROS = cSOFF.CSTRANM.TROS + cSTRANM.APPAMT;
                cSOFF.CSTRANM.TROS1 = cSOFF.CSTRANM.TROS1 + cSTRANM.APPAMT1;
                cSOFF.CSTRANM.TROS2 = cSOFF.CSTRANM.TROS2 + cSTRANM.APPAMT2;
                cSOFF.CSTRANM.TRTAXOS = cSOFF.CSTRANM.TRTAXOS + cSTRANM.APPTAX;
                cSOFF.CSTRANM.TRTAXOS1 = cSOFF.CSTRANM.TRTAXOS1 + cSTRANM.APPTAX1;
                cSOFF.CSTRANM.TRTAXOS2 = cSOFF.CSTRANM.TRTAXOS2 + cSTRANM.APPTAX2;
                cSOFF.CSTRANM.TRITEMOS = cSOFF.CSTRANM.TRITEMOS + cSTRANM.APPITEM;
                cSOFF.CSTRANM.TRITEMOS1 = cSOFF.CSTRANM.TRITEMOS1 + cSTRANM.APPITEM1;
                cSOFF.CSTRANM.TRITEMOS2 = cSOFF.CSTRANM.TRITEMOS2 + cSTRANM.APPITEM2;
            }

            //cSOFF.CSTRANM1 = db.CSTRANMs.Find(cSOFF.CRTYPE, cSOFF.CRNO, cSOFF.CRID);
            cSTRANM = db.CSTRANMs.Find("CSOFF", cSOFF.TRNO, 2);
            if ((cSTRANM != null) && (cSTRANM.APPTYPE == cSOFF.CSTRANM1.SOURCE && cSTRANM.APPNO == cSOFF.CSTRANM1.SOURCENO && cSTRANM.APPID == cSOFF.CSTRANM1.SOURCEID)) // && (cSTRANM.APPTYPE == cSOFF.CSTRANM1.SOURCE && cSTRANM.APPNO == cSOFF.CSTRANM1.SOURCENO && cSTRANM.APPID == cSOFF.CSTRANM1.SOURCEID))
            {
                // restore the original balance of applied offset
                cSOFF.CSTRANM1.TROS = -cSOFF.CSTRANM1.TROS + cSTRANM.APPAMT;
                cSOFF.CSTRANM1.TROS1 = -cSOFF.CSTRANM1.TROS1 + cSTRANM.APPAMT1;
                cSOFF.CSTRANM1.TROS2 = -cSOFF.CSTRANM1.TROS2 + cSTRANM.APPAMT2;
                cSOFF.CSTRANM1.TRTAXOS = -cSOFF.CSTRANM1.TRTAXOS + cSTRANM.APPTAX;
                cSOFF.CSTRANM1.TRTAXOS1 = -cSOFF.CSTRANM1.TRTAXOS1 + cSTRANM.APPTAX1;
                cSOFF.CSTRANM1.TRTAXOS2 = -cSOFF.CSTRANM1.TRTAXOS2 + cSTRANM.APPTAX2;
                cSOFF.CSTRANM1.TRITEMOS = -cSOFF.CSTRANM1.TRITEMOS + cSTRANM.APPITEM;
                cSOFF.CSTRANM1.TRITEMOS1 = -cSOFF.CSTRANM1.TRITEMOS1 + cSTRANM.APPITEM1;
                cSOFF.CSTRANM1.TRITEMOS2 = -cSOFF.CSTRANM1.TRITEMOS2 + cSTRANM.APPITEM2;
            }
            Session["CSOFFPage"] = page;
            SetDropDownList(cSOFF);
            ViewBag.Title = "Edit Offset Item ";
            return View(cSOFF);
        }

        // POST: CSOFFs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TRNO,VDATE,CONO,DBKey,CRKey,DBTYPE,DBNO,DBID,CRTYPE,CRNO,CRID,APPDBITEM1,APPDBITEM2,APPDBITEM,APPDBTAX1,APPDBTAX2,APPDBTAX,APPDBAMT1,APPDBAMT2,APPDBAMT,APPCRITEM1,APPCRITEM2,APPCRITEM,APPCRTAX1,APPCRTAX2,APPCRTAX,APPCRAMT1,APPCRAMT2,APPCRAMT,REM,SEQNO,POST,STAMP")] CSOFF cSOFF)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cSOFF).State = EntityState.Modified;


                if (ModelState.IsValid)
                {
                    ASIDBConnection newdb = new ASIDBConnection();
                    db.Entry(cSOFF).State = EntityState.Modified;
                    try
                    {

                        CSOFF curRec = newdb.CSOFFs.Find(cSOFF.TRNO);
                        if (curRec.STAMP == cSOFF.STAMP)
                        {
                            cSOFF.STAMP = cSOFF.STAMP + 1;

                            // Reverse Original Offsets in cstrand
                            // Reverse Original Offsets in cstranm
                            // Reverse Original Offsets in csldg - not necessary because will be reupdated
                            ReverseOffset(cSOFF);
                            ApplyOffset(cSOFF);

                            db.SaveChanges();

                            int page = (int)Session["CSOFFPage"];
                            return RedirectToAction("Index", new { page = page });
                        }
                        else { ModelState.AddModelError(string.Empty, "Record is modified"); }

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
                        newdb.Dispose();
                    }
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.page = (int)Session["CSOFFPage"];
            SetDropDownList(cSOFF);
            return View(cSOFF);
        }


        // GET: CSOFFs/Delete/5
        public ActionResult Delete(string id, int? page)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSOFF cSOFF = db.CSOFFs.Find(MyHtmlHelpers.ConvertByteStrToId(id));
            if (cSOFF == null)
            {
                return HttpNotFound();
            }

            Session["CSOFFPage"] = page ?? 1;

            SetDropDownList(cSOFF);

            ViewBag.Title = "Delete Offset Item ";
            return View("Edit",cSOFF);
        }

        // POST: CSOFFs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            CSOFF cSOFF = db.CSOFFs.Find(MyHtmlHelpers.ConvertByteStrToId(id));
            try
            {
                ReverseOffset(cSOFF);

                db.CSOFFs.Remove(cSOFF);
                db.SaveChanges();

                int page = (int)Session["CSOFFPage"];
                return RedirectToAction("Index", new { page = page });
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


            SetDropDownList(cSOFF);

            ViewBag.Title = "Delete Offset Item ";
            return View("Edit", cSOFF);
        }


        protected void ReverseOffset(CSOFF cSOFF)
        {
            IQueryable<CSTRAND> cSTRANDs = db.CSTRANDs.Where(x => x.SOURCE == "CSOFF" & x.SOURCENO == cSOFF.TRNO);
            List<CSTRAND> cSTRANDList = cSTRANDs.ToList();
            CSTRAND cSTRAND;
            CSTRANM cSTRANMDB;
            CSTRANM cSTRANMCR;
            CSLDG cSLDG;
            CSTRANM cSTRANM;

            foreach (CSTRAND item in cSTRANDList)
            {

                cSTRANMDB = db.CSTRANMs.Find(item.DBTYPE, item.DBNO, item.DBID);
                cSTRANMCR = db.CSTRANMs.Find(item.CRTYPE, item.CRNO, item.CRID);

                // if this is the original offset details that will be deleted so do nothing 
                // otherwise remove the offset application
                if ((cSTRANMDB != null) && (item.SOURCE == cSTRANMDB.SOURCE) && (item.SOURCENO == cSTRANMDB.SOURCENO) && (item.SOURCEID == cSTRANMDB.SOURCEID))
                { }
                else
                {
                    if (cSTRANMDB != null)
                    {
                        cSTRANMDB.TROS = cSTRANMDB.TROS + item.APPAMT;
                        cSTRANMDB.TROS1 = cSTRANMDB.TROS1 + item.APPAMT1;
                        cSTRANMDB.TROS2 = cSTRANMDB.TROS2 + item.APPAMT2;
                        cSTRANMDB.TRTAXOS = cSTRANMDB.TRTAXOS + item.APPTAX;
                        cSTRANMDB.TRTAXOS1 = cSTRANMDB.TRTAXOS1 + item.APPTAX1;
                        cSTRANMDB.TRTAXOS2 = cSTRANMDB.TRTAXOS2 + item.APPTAX2;
                        cSTRANMDB.TRITEMOS = cSTRANMDB.TRITEMOS + item.APPITEM;
                        cSTRANMDB.TRITEMOS1 = cSTRANMDB.TRITEMOS1 + item.APPITEM1;
                        cSTRANMDB.TRITEMOS2 = cSTRANMDB.TRITEMOS2 + item.APPITEM2;
                        cSTRANMDB.COMPLETE = "N";
                        cSTRANMDB.COMPLETED = new DateTime(3000, 1, 1);
                    }
                }

                // if this is the original offset details that will be deleted so do nothing 
                // otherwise remove the offset application
                if ((cSTRANMCR != null) && (item.SOURCE == cSTRANMCR.SOURCE) && (item.SOURCENO == cSTRANMCR.SOURCENO) && (item.SOURCEID == cSTRANMCR.SOURCEID))
                { }
                else
                {
                    if (cSTRANMCR != null)
                    {
                        cSTRANMCR.TROS = cSTRANMCR.TROS - item.APPAMT;
                        cSTRANMCR.TROS1 = cSTRANMCR.TROS1 - item.APPAMT1;
                        cSTRANMCR.TROS2 = cSTRANMCR.TROS2 - item.APPAMT2;
                        cSTRANMCR.TRTAXOS = cSTRANMCR.TRTAXOS - item.APPTAX;
                        cSTRANMCR.TRTAXOS1 = cSTRANMCR.TRTAXOS1 - item.APPTAX1;
                        cSTRANMCR.TRTAXOS2 = cSTRANMCR.TRTAXOS2 - item.APPTAX2;
                        cSTRANMCR.TRITEMOS = cSTRANMCR.TRITEMOS - item.APPITEM;
                        cSTRANMCR.TRITEMOS1 = cSTRANMCR.TRITEMOS1 - item.APPITEM1;
                        cSTRANMCR.TRITEMOS2 = cSTRANMCR.TRITEMOS2 - item.APPITEM2;
                        cSTRANMCR.COMPLETE = "N";
                        cSTRANMCR.COMPLETED = new DateTime(3000, 1, 1);
                    }
                }

                // remove cstrand
                cSTRAND = db.CSTRANDs.Find(item.SOURCE, item.SOURCENO, item.SOURCEID);
                if (cSTRAND != null)
                {
                    db.CSTRANDs.Remove(cSTRAND);
                }

                cSLDG = db.CSLDGs.Find("CSOFF", item.SOURCENO, item.SOURCEID);
                if (cSLDG != null)
                {
                    db.CSLDGs.Remove(cSLDG);
                }

                cSTRANM = db.CSTRANMs.Find("CSOFF", item.SOURCENO, item.SOURCEID);
                if (cSTRANM != null)
                {
                    db.CSTRANMs.Remove(cSTRANM);
                }
            }


        }

        protected void ApplyOffset(CSOFF cSOFF)
        {


            // Apply Offset to CSTRANM
            CSTRANM cSTRANMDB = db.CSTRANMs.Find(cSOFF.DBTYPE, cSOFF.DBNO, cSOFF.DBID);
            CSTRANM cSTRANMCR = db.CSTRANMs.Find(cSOFF.CRTYPE, cSOFF.CRNO, cSOFF.CRID);
            

            if (cSTRANMDB != null)
            {
                cSTRANMDB.TROS = cSTRANMDB.TROS - cSOFF.APPDBAMT;
                cSTRANMDB.TROS1 = cSTRANMDB.TROS1 - cSOFF.APPDBAMT1;
                cSTRANMDB.TROS2 = cSTRANMDB.TROS2 - cSOFF.APPDBAMT2;
                cSTRANMDB.TRTAXOS = cSTRANMDB.TRTAXOS - cSOFF.APPDBTAX;
                cSTRANMDB.TRTAXOS1 = cSTRANMDB.TRTAXOS1 - cSOFF.APPDBTAX1;
                cSTRANMDB.TRTAXOS2 = cSTRANMDB.TRTAXOS2 - cSOFF.APPDBTAX2;
                cSTRANMDB.TRITEMOS = cSTRANMDB.TRITEMOS - cSOFF.APPDBITEM;
                cSTRANMDB.TRITEMOS1 = cSTRANMDB.TRITEMOS1 - cSOFF.APPDBITEM1;
                cSTRANMDB.TRITEMOS2 = cSTRANMDB.TRITEMOS2 - cSOFF.APPDBITEM2;
                if (cSTRANMDB.TROS == 0)
                {
                    cSTRANMDB.COMPLETE = "Y";
                    cSTRANMDB.COMPLETED = cSOFF.VDATE;
                }

                CSTRAND cSTRAND = new CSTRAND();
                cSTRAND.SOURCE = "CSOFF";
                cSTRAND.SOURCENO = cSOFF.TRNO;
                cSTRAND.SOURCEID = 1;
                cSTRAND.DBTYPE = cSOFF.DBTYPE;
                cSTRAND.DBNO = cSOFF.DBNO;
                cSTRAND.DBID = cSOFF.DBID;
                cSTRAND.CRTYPE = "CSOFF";
                cSTRAND.CRNO = cSOFF.TRNO;
                cSTRAND.CRID = 1;
                cSTRAND.APPDATE = cSOFF.VDATE;
                cSTRAND.APPITEM = cSOFF.APPDBITEM;
                cSTRAND.APPITEM1 = cSOFF.APPDBITEM1;
                cSTRAND.APPITEM2 = cSOFF.APPDBITEM2;
                cSTRAND.APPTAX = cSOFF.APPDBTAX;
                cSTRAND.APPTAX1 = cSOFF.APPDBTAX1;
                cSTRAND.APPTAX2 = cSOFF.APPDBTAX2;
                cSTRAND.APPAMT = cSOFF.APPDBAMT;
                cSTRAND.APPAMT1 = cSOFF.APPDBAMT1;
                cSTRAND.APPAMT2 = cSOFF.APPDBAMT2;
                cSTRAND.STAMP = 0;


                CSLDG cSLDG = db.CSLDGs.Find("CSOFF", cSOFF.TRNO, 1);
                if (cSLDG == null)
                {
                    cSLDG = new CSLDG();
                    db.CSLDGs.Add(cSLDG);
                }
                else
                {
                    db.Entry(cSLDG).State = EntityState.Modified;
                }


                cSLDG.SOURCE = "CSOFF";
                cSLDG.SOURCENO = cSOFF.TRNO;
                cSLDG.SOURCEID = 1;
                cSLDG.STAMP = 0;

                cSLDG.CONO = cSTRANMDB.CONO;
                cSLDG.ENTDATE = cSOFF.VDATE;
                cSLDG.JOBNO = cSTRANMDB.JOBNO;
                cSLDG.CASENO = cSTRANMDB.CASENO;
                cSLDG.CASECODE = cSTRANMDB.CASECODE;

                cSLDG.DEP = 0;
                cSLDG.DEPREC = 0;
                cSLDG.FEE1 = 0;
                cSLDG.FEE2 = 0;
                cSLDG.FEEREC1 = 0;
                cSLDG.FEEREC2 = 0;
                cSLDG.TAX1 = 0;
                cSLDG.TAX2 = 0;
                cSLDG.TAXREC1 = 0;
                cSLDG.TAXREC2 = 0;
                cSLDG.WORK1 = 0;
                cSLDG.WORK2 = 0;
                cSLDG.WORKREC1 = 0;
                cSLDG.WORKREC2 = 0;
                cSLDG.DISB1 = 0;
                cSLDG.DISB2 = 0;
                cSLDG.DISBREC1 = 0;
                cSLDG.DISBREC2 = 0;
                cSLDG.RECEIPT = 0;
                cSLDG.ADVANCE = 0;
                cSLDG.SEQNO = cSOFF.SEQNO;
                cSLDG.REIMB1 = 0;
                cSLDG.REIMB2 = 0;
                cSLDG.REIMBREC1 = 0;
                cSLDG.REIMBREC2 = 0;

                if (cSTRANMDB.TRTYPE == "Fee")
                {
                    cSLDG.FEE1 = cSOFF.APPDBITEM1;
                    cSLDG.FEE2 = cSOFF.APPDBITEM2;
                    cSLDG.TAX1 = cSOFF.APPDBTAX1;
                    cSLDG.TAX2 = cSOFF.APPDBTAX2;
                }

                if (cSTRANMDB.TRTYPE == "Work")
                {
                    cSLDG.WORK1 = cSOFF.APPDBITEM1;
                    cSLDG.WORK2 = cSOFF.APPDBITEM2;
                    cSLDG.TAX1 = cSOFF.APPDBTAX1;
                    cSLDG.TAX2 = cSOFF.APPDBTAX2;
                }


                if (cSTRANMDB.TRTYPE == "Disbursement")
                {
                    cSLDG.DISB1 = cSOFF.APPDBITEM1;
                    cSLDG.DISB2 = cSOFF.APPDBITEM2;
                    cSLDG.TAX1 = cSOFF.APPDBTAX1;
                    cSLDG.TAX2 = cSOFF.APPDBTAX2;
                }

                if (cSTRANMDB.TRTYPE == "Reimbursement")
                {
                    cSLDG.REIMB1 = cSOFF.APPDBITEM1;
                    cSLDG.REIMB2 = cSOFF.APPDBITEM2;
                    cSLDG.TAX1 = cSOFF.APPDBTAX1;
                    cSLDG.TAX2 = cSOFF.APPDBTAX2;
                }
                if (cSTRANMDB.TRTYPE == "Advance")
                {
                    cSLDG.ADVANCE = cSOFF.APPDBAMT;

                }
                if (cSTRANMDB.TRTYPE == "Deposit")
                {
                    cSLDG.DEP = cSOFF.APPDBAMT;
                }


                db.Entry(cSTRANMDB).State = EntityState.Modified;
                db.CSTRANDs.Add(cSTRAND);

                CSTRANM cSTRANMDBOff = db.CSTRANMs.Find("CSOFF", cSOFF.TRNO, 1);
                if (cSTRANMDBOff == null)
                {
                    cSTRANMDBOff = new CSTRANM();
                    db.CSTRANMs.Add(cSTRANMDBOff);

                    cSTRANMDBOff.SOURCE = "CSOFF";
                    cSTRANMDBOff.SOURCENO = cSOFF.TRNO;
                    cSTRANMDBOff.SOURCEID = 1;
                    cSTRANMDBOff.STAMP = 0;
                    cSTRANMDBOff.REFCNT = 0;
                    cSTRANMDBOff.TRSIGN = "CR";
                    cSTRANMDBOff.SEQNO = cSOFF.SEQNO;
                    cSTRANMDBOff.REM = cSOFF.REM;
                    cSTRANMDBOff.COMPLETE = "Y";
                    cSTRANMDBOff.COMPLETED = cSOFF.VDATE;
                }
                else
                {
                    db.Entry(cSTRANMDBOff).State = EntityState.Modified;
                }

                cSTRANMDBOff.CONO = cSTRANMDB.CONO;
                cSTRANMDBOff.ENTDATE = cSOFF.VDATE;
                cSTRANMDBOff.DUEDATE = cSOFF.VDATE;
                cSTRANMDBOff.JOBNO = cSTRANMDB.JOBNO;
                cSTRANMDBOff.CASENO = cSTRANMDB.CASENO;
                cSTRANMDBOff.CASECODE = cSTRANMDB.CASECODE;
                cSTRANMDBOff.TRTYPE = cSTRANMDB.TRTYPE;
                cSTRANMDBOff.TRDESC = cSTRANMDB.TRDESC;

                cSTRANMDBOff.TRAMT = cSOFF.APPDBAMT;
                cSTRANMDBOff.TRAMT1 = cSOFF.APPDBAMT1;
                cSTRANMDBOff.TRAMT2 = cSOFF.APPDBAMT2;
                cSTRANMDBOff.TRTAX = cSOFF.APPDBTAX;
                cSTRANMDBOff.TRTAX1 = cSOFF.APPDBTAX1;
                cSTRANMDBOff.TRTAX2 = cSOFF.APPDBTAX2;
                cSTRANMDBOff.TRITEM = cSOFF.APPDBITEM;
                cSTRANMDBOff.TRITEM1 = cSOFF.APPDBITEM1;
                cSTRANMDBOff.TRITEM2 = cSOFF.APPDBITEM2;

                cSTRANMDBOff.TRSAMT = -cSOFF.APPDBAMT;
                cSTRANMDBOff.TRSAMT1 = -cSOFF.APPDBAMT1;
                cSTRANMDBOff.TRSAMT2 = -cSOFF.APPDBAMT2;
                cSTRANMDBOff.TRSTAX = -cSOFF.APPDBTAX;
                cSTRANMDBOff.TRSTAX1 = -cSOFF.APPDBTAX1;
                cSTRANMDBOff.TRSTAX2 = -cSOFF.APPDBTAX2;
                cSTRANMDBOff.TRSITEM = -cSOFF.APPDBITEM;
                cSTRANMDBOff.TRSITEM1 = -cSOFF.APPDBITEM1;
                cSTRANMDBOff.TRSITEM2 = -cSOFF.APPDBITEM2;

                cSTRANMDBOff.TROS = 0;
                cSTRANMDBOff.TROS1 = 0;
                cSTRANMDBOff.TROS2 = 0;
                cSTRANMDBOff.TRTAXOS = 0;
                cSTRANMDBOff.TRTAXOS1 = 0;
                cSTRANMDBOff.TRTAXOS2 = 0;
                cSTRANMDBOff.TRITEMOS = 0;
                cSTRANMDBOff.TRITEMOS1 = 0;
                cSTRANMDBOff.TRITEMOS2 = 0;

                cSTRANMDBOff.APPTYPE = cSOFF.DBTYPE;
                cSTRANMDBOff.APPNO = cSOFF.DBNO;
                cSTRANMDBOff.APPID = cSOFF.DBID;

                cSTRANMDBOff.APPAMT = cSOFF.APPDBAMT;
                cSTRANMDBOff.APPAMT1 = cSOFF.APPDBAMT1;
                cSTRANMDBOff.APPAMT2 = cSOFF.APPDBAMT2;
                cSTRANMDBOff.APPTAX = cSOFF.APPDBTAX;
                cSTRANMDBOff.APPTAX1 = cSOFF.APPDBTAX1;
                cSTRANMDBOff.APPTAX2 = cSOFF.APPDBTAX2;
                cSTRANMDBOff.APPITEM = cSOFF.APPDBITEM;
                cSTRANMDBOff.APPITEM1 = cSOFF.APPDBITEM1;
                cSTRANMDBOff.APPITEM2 = cSOFF.APPDBITEM2;

                //db.SaveChanges();
            }
            else { throw new Exception("Debit Item missing"); }

            if (cSTRANMCR != null)
            {
                cSTRANMCR.TROS = cSTRANMCR.TROS + cSOFF.APPCRAMT;
                cSTRANMCR.TROS1 = cSTRANMCR.TROS1 + cSOFF.APPCRAMT1;
                cSTRANMCR.TROS2 = cSTRANMCR.TROS2 + cSOFF.APPCRAMT2;
                cSTRANMCR.TRTAXOS = cSTRANMCR.TRTAXOS + cSOFF.APPCRTAX;
                cSTRANMCR.TRTAXOS1 = cSTRANMCR.TRTAXOS1 + cSOFF.APPCRTAX1;
                cSTRANMCR.TRTAXOS2 = cSTRANMCR.TRTAXOS2 + cSOFF.APPCRTAX2;
                cSTRANMCR.TRITEMOS = cSTRANMCR.TRITEMOS + cSOFF.APPCRITEM;
                cSTRANMCR.TRITEMOS1 = cSTRANMCR.TRITEMOS1 + cSOFF.APPCRITEM1;
                cSTRANMCR.TRITEMOS2 = cSTRANMCR.TRITEMOS2 + cSOFF.APPCRITEM2;
                if (cSTRANMCR.TROS == 0)
                {
                    cSTRANMCR.COMPLETE = "Y";
                    cSTRANMCR.COMPLETED = cSOFF.VDATE;
                }

                CSTRAND cSTRAND1 = new CSTRAND();
                cSTRAND1.SOURCE = "CSOFF";
                cSTRAND1.SOURCENO = cSOFF.TRNO;
                cSTRAND1.SOURCEID = 2;
                cSTRAND1.DBTYPE = "CSOFF";
                cSTRAND1.DBNO = cSOFF.TRNO;
                cSTRAND1.DBID = 2;
                cSTRAND1.CRTYPE = cSOFF.CRTYPE;
                cSTRAND1.CRNO = cSOFF.CRNO;
                cSTRAND1.CRID = cSOFF.CRID;

                cSTRAND1.APPDATE = cSOFF.VDATE;
                cSTRAND1.APPITEM = cSOFF.APPCRITEM;
                cSTRAND1.APPITEM1 = cSOFF.APPCRITEM1;
                cSTRAND1.APPITEM2 = cSOFF.APPCRITEM2;
                cSTRAND1.APPTAX = cSOFF.APPCRTAX;
                cSTRAND1.APPTAX1 = cSOFF.APPCRTAX1;
                cSTRAND1.APPTAX2 = cSOFF.APPCRTAX2;
                cSTRAND1.APPAMT = cSOFF.APPCRAMT;
                cSTRAND1.APPAMT1 = cSOFF.APPCRAMT1;
                cSTRAND1.APPAMT2 = cSOFF.APPCRAMT2;
                cSTRAND1.STAMP = 0;

                CSLDG cSLDG = db.CSLDGs.Find("CSOFF", cSOFF.TRNO, 2);
                if (cSLDG == null)
                {
                    cSLDG = new CSLDG();
                    db.CSLDGs.Add(cSLDG);
                }
                else
                {
                    db.Entry(cSLDG).State = EntityState.Modified;
                }


                cSLDG.SOURCE = "CSOFF";
                cSLDG.SOURCENO = cSOFF.TRNO;
                cSLDG.SOURCEID = 2;
                cSLDG.STAMP = 0;

                cSLDG.CONO = cSTRANMCR.CONO;
                cSLDG.ENTDATE = cSOFF.VDATE;
                cSLDG.JOBNO = cSTRANMCR.JOBNO;
                cSLDG.CASENO = cSTRANMCR.CASENO;
                cSLDG.CASECODE = cSTRANMCR.CASECODE;

                cSLDG.DEP = 0;
                cSLDG.DEPREC = 0;
                cSLDG.FEE1 = 0;
                cSLDG.FEE2 = 0;
                cSLDG.FEEREC1 = 0;
                cSLDG.FEEREC2 = 0;
                cSLDG.TAX1 = 0;
                cSLDG.TAX2 = 0;
                cSLDG.TAXREC1 = 0;
                cSLDG.TAXREC2 = 0;
                cSLDG.WORK1 = 0;
                cSLDG.WORK2 = 0;
                cSLDG.WORKREC1 = 0;
                cSLDG.WORKREC2 = 0;
                cSLDG.DISB1 = 0;
                cSLDG.DISB2 = 0;
                cSLDG.DISBREC1 = 0;
                cSLDG.DISBREC2 = 0;
                cSLDG.RECEIPT = 0;
                cSLDG.ADVANCE = 0;
                cSLDG.SEQNO = cSOFF.SEQNO;
                cSLDG.REIMB1 = 0;
                cSLDG.REIMB2 = 0;
                cSLDG.REIMBREC1 = 0;
                cSLDG.REIMBREC2 = 0;

                if (cSTRANMCR.TRTYPE == "Fee")
                {
                    cSLDG.FEE1 = -cSOFF.APPCRITEM1;
                    cSLDG.FEE2 = -cSOFF.APPCRITEM2;
                    cSLDG.TAX1 = -cSOFF.APPCRTAX1;
                    cSLDG.TAX2 = -cSOFF.APPCRTAX2;
                }

                if (cSTRANMCR.TRTYPE == "Work")
                {
                    cSLDG.WORK1 = -cSOFF.APPCRITEM1;
                    cSLDG.WORK2 = -cSOFF.APPCRITEM2;
                    cSLDG.TAX1 = -cSOFF.APPCRTAX1;
                    cSLDG.TAX2 = -cSOFF.APPCRTAX2;
                }


                if (cSTRANMCR.TRTYPE == "Disbursement")
                {
                    cSLDG.DISB1 = -cSOFF.APPCRITEM1;
                    cSLDG.DISB2 = -cSOFF.APPCRITEM2;
                    cSLDG.TAX1 = -cSOFF.APPCRTAX1;
                    cSLDG.TAX2 = -cSOFF.APPCRTAX2;
                }

                if (cSTRANMCR.TRTYPE == "Reimbursement")
                {
                    cSLDG.REIMB1 = -cSOFF.APPCRITEM1;
                    cSLDG.REIMB2 = -cSOFF.APPCRITEM2;
                    cSLDG.TAX1 = -cSOFF.APPCRTAX1;
                    cSLDG.TAX2 = -cSOFF.APPCRTAX2;
                }
                if (cSTRANMCR.TRTYPE == "Advance")
                {
                    cSLDG.ADVANCE = -cSOFF.APPCRAMT;

                }
                if (cSTRANMCR.TRTYPE == "Deposit")
                {
                    cSLDG.DEP = -cSOFF.APPCRAMT;
                }


                db.Entry(cSTRANMCR).State = EntityState.Modified;
                db.CSTRANDs.Add(cSTRAND1);

                CSTRANM cSTRANMCROff = db.CSTRANMs.Find("CSOFF", cSOFF.TRNO, 2);
                if (cSTRANMCROff == null)
                {
                    cSTRANMCROff = new CSTRANM();
                    db.CSTRANMs.Add(cSTRANMCROff);

                    cSTRANMCROff.SOURCE = "CSOFF";
                    cSTRANMCROff.SOURCENO = cSOFF.TRNO;
                    cSTRANMCROff.SOURCEID = 2;
                    cSTRANMCROff.STAMP = 0;
                    cSTRANMCROff.REFCNT = 0;
                    cSTRANMCROff.TRSIGN = "DB";
                    cSTRANMCROff.SEQNO = cSOFF.SEQNO;
                    cSTRANMCROff.REM = cSOFF.REM;
                    cSTRANMCROff.COMPLETE = "Y";
                    cSTRANMCROff.COMPLETED = cSOFF.VDATE;
                }
                else
                {
                    db.Entry(cSTRANMCROff).State = EntityState.Modified;
                }

                cSTRANMCROff.CONO = cSTRANMCR.CONO;
                cSTRANMCROff.ENTDATE = cSOFF.VDATE;
                cSTRANMCROff.DUEDATE = cSOFF.VDATE;
                cSTRANMCROff.JOBNO = cSTRANMCR.JOBNO;
                cSTRANMCROff.CASENO = cSTRANMCR.CASENO;
                cSTRANMCROff.CASECODE = cSTRANMCR.CASECODE;
                cSTRANMCROff.TRTYPE = cSTRANMCR.TRTYPE;
                cSTRANMCROff.TRDESC = cSTRANMCR.TRDESC;

                cSTRANMCROff.TRAMT = cSOFF.APPCRAMT;
                cSTRANMCROff.TRAMT1 = cSOFF.APPCRAMT1;
                cSTRANMCROff.TRAMT2 = cSOFF.APPCRAMT2;
                cSTRANMCROff.TRTAX = cSOFF.APPCRTAX;
                cSTRANMCROff.TRTAX1 = cSOFF.APPCRTAX1;
                cSTRANMCROff.TRTAX2 = cSOFF.APPCRTAX2;
                cSTRANMCROff.TRITEM = cSOFF.APPCRITEM;
                cSTRANMCROff.TRITEM1 = cSOFF.APPCRITEM1;
                cSTRANMCROff.TRITEM2 = cSOFF.APPCRITEM2;

                cSTRANMCROff.TRSAMT = cSOFF.APPCRAMT;
                cSTRANMCROff.TRSAMT1 = cSOFF.APPCRAMT1;
                cSTRANMCROff.TRSAMT2 = cSOFF.APPCRAMT2;
                cSTRANMCROff.TRSTAX = cSOFF.APPCRTAX;
                cSTRANMCROff.TRSTAX1 = cSOFF.APPCRTAX1;
                cSTRANMCROff.TRSTAX2 = cSOFF.APPCRTAX2;
                cSTRANMCROff.TRSITEM = cSOFF.APPCRITEM;
                cSTRANMCROff.TRSITEM1 = cSOFF.APPCRITEM1;
                cSTRANMCROff.TRSITEM2 = cSOFF.APPCRITEM2;

                cSTRANMCROff.TROS = 0;
                cSTRANMCROff.TROS1 = 0;
                cSTRANMCROff.TROS2 = 0;
                cSTRANMCROff.TRTAXOS = 0;
                cSTRANMCROff.TRTAXOS1 = 0;
                cSTRANMCROff.TRTAXOS2 = 0;
                cSTRANMCROff.TRITEMOS = 0;
                cSTRANMCROff.TRITEMOS1 = 0;
                cSTRANMCROff.TRITEMOS2 = 0;

                cSTRANMCROff.APPTYPE = cSOFF.CRTYPE;
                cSTRANMCROff.APPNO = cSOFF.CRNO;
                cSTRANMCROff.APPID = cSOFF.CRID;

                cSTRANMCROff.APPAMT = cSOFF.APPCRAMT;
                cSTRANMCROff.APPAMT1 = cSOFF.APPCRAMT1;
                cSTRANMCROff.APPAMT2 = cSOFF.APPCRAMT2;
                cSTRANMCROff.APPTAX = cSOFF.APPCRTAX;
                cSTRANMCROff.APPTAX1 = cSOFF.APPCRTAX1;
                cSTRANMCROff.APPTAX2 = cSOFF.APPCRTAX2;
                cSTRANMCROff.APPITEM = cSOFF.APPCRITEM;
                cSTRANMCROff.APPITEM1 = cSOFF.APPCRITEM1;
                cSTRANMCROff.APPITEM2 = cSOFF.APPCRITEM2;

                //db.SaveChanges();
            }
            else { throw new Exception("Credit Item missing"); }


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
