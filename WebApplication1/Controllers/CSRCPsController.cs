using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PagedList;
using WebApplication1.Utility;
using System.Data.Entity.Core;
using System.Data.Entity.Validation;
using System.Data.Entity.Infrastructure;
using FirebirdSql.Data.Client;
using FirebirdSql.Data.FirebirdClient;

namespace WebApplication1.Controllers
{

    [Authorize(Roles= "Administrator,CS-A/C")]
    public class CSRCPsController : BaseController
    {
        private ASIDBConnection db = new ASIDBConnection();

        public PartialViewResult Search()
        {
            CSRCP searchRec = null;
            //searchRec.CONO = pSearchCode;
            //searchRec.CONAME = pSearchName;
            if (Session["SearchRCPRec"] != null)
            {
                searchRec = (CSRCP)Session["SearchRCPRec"];

            }
            else
            {
                searchRec = new CSRCP();
                searchRec.VDATE = DateTime.Parse("1/" + DateTime.Today.Month.ToString() + "/" + DateTime.Today.Year.ToString());
                if (DateTime.Today.Month < 12)
                {
                    searchRec.ISSDATE = DateTime.Parse("1/" + (DateTime.Today.Month + 1).ToString() + "/" + DateTime.Today.Year.ToString());
                }
                else
                {
                    searchRec.ISSDATE = DateTime.Parse("1/1/" + (DateTime.Today.Year + 1).ToString());
                }
                //searchRec.ISSDATE = searchRec.ISSDATE.AddDays(-1);
            }
            if (Session["SearchRCPSort"] == null)
            {
                Session["SearchRCPSort"] = "TRNO";
            };

            List<SelectListItem> listItems = new List<SelectListItem>();
            listItems.Add(new SelectListItem
            {
                Text = "Company Name",
                Value = "CONAME",
                Selected = (string)Session["SearchRCPSort"] == "CONAME"
            });

            listItems.Add(new SelectListItem
            {
                Text = "OR #",
                Value = "TRNO",
                Selected = (string)Session["SearchRCPSort"] == "TRNO"
            });

            listItems.Add(new SelectListItem
            {
                Text = "OR # Latest",
                Value = "TRNOLAST",
                Selected = (string)Session["SearchRCPSort"] == "TRNOLAST"
            });

            listItems.Add(new SelectListItem
            {
                Text = "Date",
                Value = "VDATE",
                Selected = (string)Session["SearchRCPSort"] == "VDATE"
            });

            listItems.Add(new SelectListItem
            {
                Text = "Archive",
                Value = "ARCHIVE",
                Selected = (string)Session["SearchRCPSort"] == "ARCHIVE"
            });

            //listItems.Add(new SelectListItem
            //{
            //    Text = "Unallocated Receipts Only",
            //    Value = "NOALLOC",
            //    Selected = (string)Session["SearchRCPSort"] == "NOALLOC"
            //});


            ViewBag.SORTBY = listItems;
            return PartialView("Partial/Search", searchRec);
        }

        public PartialViewResult SearchC()
        {
            CSRCP searchRec = null;
            //searchRec.CONO = pSearchCode;
            //searchRec.CONAME = pSearchName;
            if (Session["SearchRCPCRec"] != null)
            {
                searchRec = (CSRCP)Session["SearchRCPCRec"];

            }
            else
            {
                searchRec = new CSRCP();
                searchRec.VDATE = DateTime.Parse("1/" + DateTime.Today.Month.ToString() + "/" + DateTime.Today.Year.ToString());
                if (DateTime.Today.Month < 12)
                {
                    searchRec.ISSDATE = DateTime.Parse("1/" + (DateTime.Today.Month + 1).ToString() + "/" + DateTime.Today.Year.ToString());
                }
                else
                {
                    searchRec.ISSDATE = DateTime.Parse("1/1/" + (DateTime.Today.Year + 1).ToString());
                }
                //searchRec.ISSDATE = searchRec.ISSDATE.AddDays(-1);
            }
            if (Session["SearchCRCPCSort"] == null)
            {
                Session["SearchCRCPCSort"] = "CTRNO";
            };

            List<SelectListItem> listItems = new List<SelectListItem>();
            listItems.Add(new SelectListItem
            {
                Text = "Company Name",
                Value = "CONAME",
                Selected = (string)Session["SearchRCPCSort"] == "CONAME"
            });

            listItems.Add(new SelectListItem
            {
                Text = "CANCEL #",
                Value = "CTRNO",
                Selected = (string)Session["SearchRCPCSort"] == "CTRNO"
            });

            listItems.Add(new SelectListItem
            {
                Text = "CANCEL # Latest",
                Value = "CTRNOLAST",
                Selected = (string)Session["SearchRCPCSort"] == "CTRNOLAST"
            });

            listItems.Add(new SelectListItem
            {
                Text = "Date",
                Value = "CVDATE",
                Selected = (string)Session["SearchRCPCSort"] == "CVDATE"
            });

            listItems.Add(new SelectListItem
            {
                Text = "Archive",
                Value = "ARCHIVE",
                Selected = (string)Session["SearchRCPCSort"] == "ARCHIVE"
            });

            //listItems.Add(new SelectListItem
            //{
            //    Text = "Unallocated Receipts Only",
            //    Value = "NOALLOC",
            //    Selected = (string)Session["SearchRCPSort"] == "NOALLOC"
            //});


            ViewBag.SORTBY = listItems;
            return PartialView("Partial/SearchC", searchRec);
        }


        [HttpGet]
        public ActionResult SearchPost()
        {
            return Index(1);
        }

        //[HttpPost, ActionName("Search")]
        [HttpPost]
        public ActionResult SearchPost(CSRCP cSRCP)
        {

            Session["SearchRCPRec"] = cSRCP;
            Session["SearchRCPSort"] = Request.Params["SORTBY"] ?? "VDATE";
            return Redirect("?page=1");
            //return Index(1);
        }

        [HttpPost]
        public ActionResult SearchCPost(CSRCP cSRCP)
        {

            Session["SearchRCPCRec"] = cSRCP;
            Session["SearchRCPCSort"] = Request.Params["SORTBY"] ?? "CVDATE";
            return Redirect("IndexC?page=1");
            //return Index(1);
        }
        // GET: CSRCPs
        public ActionResult Index(int? page)
        {
            ViewBag.page = page ?? 1;
            return View("Index", CurrentSelection().ToList().ToPagedList(page ?? 1, 30));
        }

      
        public ActionResult IndexC(int? page)
        {
            ViewBag.page = page ?? 1;
            return View("IndexC", CurrentSelectionC().ToList().ToPagedList(page ?? 1, 30));
        }

        public IQueryable<CSRCP> CurrentSelection()
        {
            string pSearchCode = "";
            string pSearchName = "";
            string pSearchRCP = "";
            DateTime pSearchVdate = DateTime.Parse("01/01/0001");
            DateTime pSearchDdate = DateTime.Parse("01/01/0001");

            if (Session["SearchRCPRec"] != null)
            {
                CSRCP searchRec = (CSRCP)(Session["SearchRCPRec"]);
                pSearchCode = searchRec.CSCOMSTR.COREGNO ?? "";
                pSearchName = searchRec.CSCOMSTR.CONAME ?? "";
                pSearchVdate = searchRec.VDATE;
                pSearchDdate = searchRec.ISSDATE ?? DateTime.Parse("01/01/0001");
                pSearchRCP = searchRec.TRNO ?? "";

            }
            else
            { // start with current month proforma bills instead of entire list
                pSearchVdate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);    
                pSearchDdate = pSearchVdate.AddMonths(1).AddDays(-1);
            }

            IQueryable<CSRCP> cSRCPs = db.CSRCPs.Where(x => x.CFLAG == "N");

            if ((string)Session["SearchRCPSort"] != "ARCHIVE") { cSRCPs = cSRCPs.Where(x => x.POST == "N"); }
            else { cSRCPs = cSRCPs.Where(x => x.POST == "Y"); }

            if (pSearchCode != "") { cSRCPs = cSRCPs.Where(x => x.CSCOMSTR.COREGNO.Contains(pSearchCode.ToUpper()));}
            if (pSearchName != "") { cSRCPs = cSRCPs.Where(x => x.CSCOMSTR.CONAME.Contains(pSearchName.ToUpper())); };
            if (pSearchVdate != DateTime.Parse("01/01/0001")) { cSRCPs = cSRCPs.Where(x => x.VDATE >= pSearchVdate); };
            if (pSearchDdate != DateTime.Parse("01/01/0001")) { cSRCPs = cSRCPs.Where(x => x.VDATE <= pSearchDdate); };
            if (pSearchRCP != "")
            {
                if (pSearchRCP.Length > 8)
                {
                    pSearchRCP = pSearchRCP.Substring(0, 9);
                    cSRCPs = cSRCPs.Where(x => x.TRNO == pSearchRCP);
                }
                else
                {
                    cSRCPs = cSRCPs.Where(x => x.TRNO.Contains(pSearchRCP));
                }
            };
            //cSRCPs = cSRCPs.Include(e => e.CSCOMSTR).Include(c => c.HKRCMODE).Include(c => c.HKMAP);
            cSRCPs = cSRCPs.Include(c => c.HKBANK).Include(c => c.HKRCISSLOC);

            if ((string)Session["SearchRCPSort"] == "CONAME")
            {
                cSRCPs = cSRCPs.OrderBy(n => n.CSCOMSTR.CONAME);
            }
            else if ((string)Session["SearchRCPSort"] == "VDATE")
            {
                cSRCPs = cSRCPs.OrderBy(n => n.VDATE);

            }
            else if ((string)Session["SearchRCPSort"] == "TRNOLAST")
            {
                cSRCPs = cSRCPs.OrderByDescending(n => n.TRNO);

            }
            else if ((string)Session["SearchRCPSort"] == "NOALLOC")
            {
                cSRCPs = cSRCPs.Where(x => x.AllocAmt != x.NETAMT);
                cSRCPs = cSRCPs.OrderBy(n => n.TRNO);
            }
            else
            {
                cSRCPs.OrderBy(n => n.TRNO);
            }

            DateTime rptStart = pSearchVdate;
            DateTime rptEnd = pSearchDdate;

            ViewBag.RPT_START = rptStart.ToString("dd/MM/yyyy");
            ViewBag.RPT_END = rptEnd.ToString("dd/MM/yyyy");
            return cSRCPs;
        }

        public IQueryable<CSRCP> CurrentSelectionC()
        {
            string pSearchCode = "";
            string pSearchName = "";
            string pSearchRCP = "";
            DateTime pSearchVdate = DateTime.Parse("01/01/0001");
            DateTime pSearchDdate = DateTime.Parse("01/01/0001");

            if (Session["SearchRCPCRec"] != null)
            {
                CSRCP searchRec = (CSRCP)(Session["SearchRCPCRec"]);
                pSearchCode = searchRec.CSCOMSTR.COREGNO ?? "";
                pSearchName = searchRec.CSCOMSTR.CONAME ?? "";
                pSearchVdate = searchRec.VDATE;
                pSearchDdate = searchRec.ISSDATE ?? DateTime.Parse("01/01/0001");
                pSearchRCP = searchRec.CTRNO ?? "";

            }
            else
            { // start with current month proforma bills instead of entire list
                pSearchVdate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                pSearchDdate = pSearchVdate.AddMonths(1).AddDays(-1);
            }

            IQueryable<CSRCP> cSRCPs = db.CSRCPs.Where(x => x.CFLAG == "Y");

            if ((string)Session["SearchRCPCSort"] != "ARCHIVE") { cSRCPs = cSRCPs.Where(x => x.CPOST == "N"); }
            else { cSRCPs = cSRCPs.Where(x => x.CPOST == "Y"); }

            if (pSearchCode != "") { cSRCPs = cSRCPs.Where(x => x.CSCOMSTR.COREGNO.Contains(pSearchCode.ToUpper())); }
            if (pSearchName != "") { cSRCPs = cSRCPs.Where(x => x.CSCOMSTR.CONAME.Contains(pSearchName.ToUpper())); };
            if (pSearchVdate != DateTime.Parse("01/01/0001")) { cSRCPs = cSRCPs.Where(x => x.CVDATE >= pSearchVdate); };
            if (pSearchDdate != DateTime.Parse("01/01/0001")) { cSRCPs = cSRCPs.Where(x => x.CVDATE <= pSearchDdate); };
            if (pSearchRCP != "")
            {
                if (pSearchRCP.Length > 8)
                {
                    cSRCPs = cSRCPs.Where(x => x.CTRNO == pSearchRCP);
                }
                else
                {
                    cSRCPs = cSRCPs.Where(x => x.CTRNO.Contains(pSearchRCP));
                }
            };

            cSRCPs = cSRCPs.Include(e => e.CSCOMSTR).Include(c => c.HKMAP).Include(c => c.HKBANK).Include(c => c.HKRCMODE).Include(c => c.HKRCISSLOC);

            if ((string)Session["SearchRCPCSort"] == "CONAME")
            {
                cSRCPs = cSRCPs.OrderBy(n => n.CSCOMSTR.CONAME);
            }
            else if ((string)Session["SearchRCPCSort"] == "CVDATE")
            {
                cSRCPs = cSRCPs.OrderBy(n => n.CVDATE);

            }
            else if ((string)Session["SearchRCPCSort"] == "CTRNOLAST")
            {
                cSRCPs = cSRCPs.OrderByDescending(n => n.CTRNO);

            }
            else if ((string)Session["SearchRCPCSort"] == "NOALLOC")
            {
                cSRCPs = cSRCPs.Where(x => x.AllocAmt != x.NETAMT);
                cSRCPs = cSRCPs.OrderBy(n => n.CTRNO);
            }
            else
            {
                cSRCPs.OrderBy(n => n.CTRNO);
            }

            DateTime rptStart = pSearchVdate;
            DateTime rptEnd = pSearchDdate;

            ViewBag.RPT_START = rptStart.ToString("dd/MM/yyyy");
            ViewBag.RPT_END = rptEnd.ToString("dd/MM/yyyy");
            return cSRCPs;
        }

        // GET: CSRCPs/Details/5
        public ActionResult Details(string id, int? page)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            string sid = MyHtmlHelpers.ConvertByteStrToId(id);
            CSRCP cSRCP = db.CSRCPs.Find(sid);
            if (cSRCP == null)
            {
                return HttpNotFound();
            }
            ViewBag.page = page ?? 1;
            Session["CSRCPPage"] = ViewBag.page;

            ViewBag.DETFLAG = db.HKRCMODEs.Where(x => x.RCMODE == cSRCP.RCMODE).Select(y => y.DETFLAG).FirstOrDefault();
            ViewBag.CONAME = db.CSCOMSTRs.Where(x => x.CONO == cSRCP.CONO).Select(y => y.CONAME).FirstOrDefault();
            ViewBag.CSTRANM = db.CSTRANMs.Where(x => x.SOURCE == "CSRCP" && x.SOURCENO == cSRCP.TRNO);
            ViewBag.CONO = new SelectList(db.CSCOMSTRs.Select(x => new { CONO = x.CONO, CONAME = x.CONAME + "  (" + x.CONO + ")" }).OrderBy(y => y.CONAME), "CONO", "CONAME", cSRCP.CONO);
            ViewBag.RCMAPCODE = new SelectList(db.HKMAPs, "MAPCODE", "MAPCODE", cSRCP.RCMAPCODE);
            ViewBag.ISSBANK = new SelectList(db.HKBANKs.OrderBy(x => x.BANKDESC), "BANKCODE", "BANKDESC", cSRCP.ISSBANK);
            ViewBag.RCMODE = new SelectList(db.HKRCMODEs, "RCMODE", "RCMODE", cSRCP.RCMODE);
            ViewBag.ISSLOC = new SelectList(db.HKRCISSLOCs, "ISSLOC", "ISSLOC", cSRCP.ISSLOC);

            ViewBag.Title = "View Receipt";
            return View("Edit", cSRCP);
        }

        // GET: CSRCPs/Create
        public ActionResult Create(int? mode)
        {
            CSRCP cSRCP = new CSRCP();
            cSRCP.ISSLOC = "Local";
            cSRCP.STAMP = 0;
            cSRCP.POST = "N";
            cSRCP.CFLAG = "N";
            cSRCP.VDATE = DateTime.Today;


            ViewBag.DETFLAG = "Y";
            ViewBag.CONAME = db.CSCOMSTRs.Where(x => x.CONO == cSRCP.CONO).Select(y => y.CONAME).FirstOrDefault();
            ViewBag.CSTRANM = db.CSTRANMs.Where(x => x.SOURCE == "CSRCP" && x.SOURCENO == cSRCP.TRNO);
            ViewBag.CONO = new SelectList(db.CSCOMSTRs.Select(x => new { CONO = x.CONO, CONAME = x.CONAME + "  (" + x.CONO + ")" }).OrderBy(y => y.CONAME), "CONO", "CONAME", cSRCP.CONO);
            ViewBag.RCMAPCODE = new SelectList(db.HKMAPs, "MAPCODE", "MAPCODE", cSRCP.RCMAPCODE);
            ViewBag.ISSBANK = new SelectList(db.HKBANKs.OrderBy(x => x.BANKDESC), "BANKCODE", "BANKDESC", cSRCP.ISSBANK);
            ViewBag.RCMODE = new SelectList(db.HKRCMODEs, "RCMODE", "RCMODE", cSRCP.RCMODE);
            ViewBag.ISSLOC = new SelectList(db.HKRCISSLOCs, "ISSLOC", "ISSLOC", cSRCP.ISSLOC);

            ViewBag.Title = "Create New Receipt";

            int viewMode = mode ?? 0;
            if (viewMode == 0)
            {
                return View("Edit", cSRCP);
            } else
            {
                return View("EditRcpNo", cSRCP);
            }
        }

        // POST: CSRCPs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TRNO,VDATE,CONO,RCAMT,RCMODE,RCMAPCODE,ISSBANK,ISSLOC,ISSREFNO,ISSDATE,COMAMT,NETAMT,REM,SEQNO,POSTBool,CFLAG,CTRNO,CVDATE,CREM,CSEQNO,CPOST,STAMP")] CSRCP cSRCP)
        {
            if (string.IsNullOrEmpty(cSRCP.CONO)) { ModelState.AddModelError("CONO", "Company is required"); }
            if (string.IsNullOrEmpty(cSRCP.RCMODE)) { ModelState.AddModelError("RCMODE", "RC Mode is required"); }
            if (string.IsNullOrEmpty(cSRCP.RCMAPCODE)) { ModelState.AddModelError("MODE MAP", "Mode Map is required"); }

            if (ModelState.IsValid)
            {

                SALASTNO serialTbl = db.SALASTNOes.Find("CSRCP");
                if (serialTbl != null)
                {
                    CSCOMSTR cSCOMSTR = db.CSCOMSTRs.Find(cSRCP.CONO);
                    if (cSCOMSTR != null)
                    {
                        try
                        {
                            if (string.IsNullOrEmpty(cSRCP.TRNO))
                            {
                                string prefix = serialTbl.LASTPFIX;
                                int MaxNo = serialTbl.LASTNOMAX;
                                bool AutoGen = serialTbl.AUTOGEN == "Y";
                                serialTbl.LASTNO = serialTbl.LASTNO + 1;
                                cSRCP.TRNO = serialTbl.LASTNO.ToString("D10");

                                serialTbl.STAMP = serialTbl.STAMP + 1;
                                db.Entry(serialTbl).State = EntityState.Modified;
                            }

                            // increment company seqno count before using it in transaction
                            cSCOMSTR.SEQNO = cSCOMSTR.SEQNO + 1;
                            cSCOMSTR.STAMP = cSCOMSTR.STAMP + 1;

                            cSRCP.SEQNO = cSCOMSTR.SEQNO;
                            db.Entry(cSCOMSTR).State = EntityState.Modified;

                            db.CSRCPs.Add(cSRCP);

                            CSLDG cSLDG = db.CSLDGs.Find("CSRCP", cSRCP.TRNO, 0);
                            if (cSLDG == null)
                            {
                                cSLDG = new CSLDG();
                                cSLDG.STAMP = 0;
                                db.CSLDGs.Add(cSLDG);
                            }
                            else
                            {
                                db.Entry(cSLDG).State = EntityState.Modified;
                            }

                            UpdateCSLDG(cSRCP, cSLDG);

                            db.SaveChanges();
                            //return Edit(MyHtmlHelpers.ConvertIdToByteStr(cSRCP.TRNO), 1);
                            return RedirectToAction("Edit", new { id = MyHtmlHelpers.ConvertIdToByteStr(cSRCP.TRNO), page = 1 });
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
            ViewBag.DETFLAG = db.HKRCMODEs.Where(x => x.RCMODE == cSRCP.RCMODE).Select(y => y.DETFLAG).FirstOrDefault();
            ViewBag.CONAME = db.CSCOMSTRs.Where(x => x.CONO == cSRCP.CONO).Select(y => y.CONAME).FirstOrDefault();
            ViewBag.CSTRANM = db.CSTRANMs.Where(x => x.SOURCE == "CSRCP" && x.SOURCENO == cSRCP.TRNO);
            ViewBag.CONO = new SelectList(db.CSCOMSTRs.Select(x => new { CONO = x.CONO, CONAME = x.CONAME + "  (" + x.CONO + ")" }).OrderBy(y => y.CONAME), "CONO", "CONAME", cSRCP.CONO);
            ViewBag.RCMAPCODE = new SelectList(db.HKMAPs, "MAPCODE", "MAPCODE", cSRCP.RCMAPCODE);
            ViewBag.ISSBANK = new SelectList(db.HKBANKs.OrderBy(x => x.BANKDESC), "BANKCODE", "BANKDESC", cSRCP.ISSBANK);
            ViewBag.RCMODE = new SelectList(db.HKRCMODEs, "RCMODE", "RCMODE", cSRCP.RCMODE);
            ViewBag.ISSLOC = new SelectList(db.HKRCISSLOCs, "ISSLOC", "ISSLOC", cSRCP.ISSLOC);
            return View("Edit", cSRCP);
        }

        public ActionResult EditRCMode(CSRCP cSRCP)
        {

            //ModelState.Clear(); //apparent this has side-effects and using Remove is preferrable
            ModelState.Remove("ISSBANK");
            ModelState.Remove("ISSLOC");
            ModelState.Remove("ISSREFNO");
            ModelState.Remove("ISSDATE");

            ViewBag.DETFLAG = db.HKRCMODEs.Where(x => x.RCMODE == cSRCP.RCMODE).Select(y => y.DETFLAG).FirstOrDefault();

            cSRCP.ISSBANK = null;
            cSRCP.ISSLOC = (ViewBag.DETFLAG == "Y") ? "Local" : null;
            cSRCP.ISSREFNO = null;
            cSRCP.ISSDATE = DateTime.Today;

            ViewBag.ISSBANK = new SelectList(db.HKBANKs.OrderBy(x => x.BANKDESC), "BANKCODE", "BANKDESC", cSRCP.ISSBANK);
            ViewBag.ISSLOC = new SelectList(db.HKRCISSLOCs, "ISSLOC", "ISSLOC", cSRCP.ISSLOC);
            return PartialView("Partial/EditRCMode", cSRCP);


        }

        // GET: CSRCPs/Edit/5
        public ActionResult Edit(string id, int? page)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            string sid = MyHtmlHelpers.ConvertByteStrToId(id);
            CSRCP cSRCP = db.CSRCPs.Find(sid);
            if (cSRCP == null)
            {
                return HttpNotFound();
            }


            ViewBag.page = page ?? 1;
            Session["CSRCPPage"] = ViewBag.page;

            ViewBag.DETFLAG = db.HKRCMODEs.Where(x => x.RCMODE == cSRCP.RCMODE).Select(y => y.DETFLAG).FirstOrDefault();
            ViewBag.CONAME = db.CSCOMSTRs.Where(x => x.CONO == cSRCP.CONO).Select(y => y.CONAME).FirstOrDefault();
            ViewBag.CSTRANM = db.CSTRANMs.Where(x => x.SOURCE == "CSRCP" && x.SOURCENO == cSRCP.TRNO);
            ViewBag.CONO = new SelectList(db.CSCOMSTRs.Select(x => new { CONO = x.CONO, CONAME = x.CONAME + "  (" + x.CONO + ")" }).OrderBy(y => y.CONAME), "CONO", "CONAME", cSRCP.CONO);
            ViewBag.RCMAPCODE = new SelectList(db.HKMAPs, "MAPCODE", "MAPCODE", cSRCP.RCMAPCODE);
            ViewBag.ISSBANK = new SelectList(db.HKBANKs.OrderBy(x => x.BANKDESC), "BANKCODE", "BANKDESC", cSRCP.ISSBANK);
            ViewBag.RCMODE = new SelectList(db.HKRCMODEs, "RCMODE", "RCMODE", cSRCP.RCMODE);
            ViewBag.ISSLOC = new SelectList(db.HKRCISSLOCs, "ISSLOC", "ISSLOC", cSRCP.ISSLOC);
            return View("Edit", cSRCP);
        }

        public ActionResult EditC(string id, int? page)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            string sid = MyHtmlHelpers.ConvertByteStrToId(id);
            CSRCP cSRCP = db.CSRCPs.Find(sid);
            if (cSRCP == null)
            {
                return HttpNotFound();
            }


            ViewBag.page = page ?? 1;
            Session["CSRCPCPage"] = ViewBag.page;

            ViewBag.DETFLAG = db.HKRCMODEs.Where(x => x.RCMODE == cSRCP.RCMODE).Select(y => y.DETFLAG).FirstOrDefault();
            ViewBag.CONAME = db.CSCOMSTRs.Where(x => x.CONO == cSRCP.CONO).Select(y => y.CONAME).FirstOrDefault();
            ViewBag.CSTRANM = db.CSTRANMs.Where(x => x.SOURCE == "CSRCP" && x.SOURCENO == cSRCP.TRNO);
            ViewBag.CONO = new SelectList(db.CSCOMSTRs.Select(x => new { CONO = x.CONO, CONAME = x.CONAME + "  (" + x.CONO + ")" }).OrderBy(y => y.CONAME), "CONO", "CONAME", cSRCP.CONO);
            ViewBag.RCMAPCODE = new SelectList(db.HKMAPs, "MAPCODE", "MAPCODE", cSRCP.RCMAPCODE);
            ViewBag.ISSBANK = new SelectList(db.HKBANKs.OrderBy(x => x.BANKDESC), "BANKCODE", "BANKDESC", cSRCP.ISSBANK);
            ViewBag.RCMODE = new SelectList(db.HKRCMODEs, "RCMODE", "RCMODE", cSRCP.RCMODE);
            ViewBag.ISSLOC = new SelectList(db.HKRCISSLOCs, "ISSLOC", "ISSLOC", cSRCP.ISSLOC);
            return View("EditC", cSRCP);
        }

        // POST: CSRCPs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TRNO,VDATE,CONO,RCAMT,RCMODE,RCMAPCODE,ISSBANK,ISSLOC,ISSREFNO,ISSDATE,COMAMT,NETAMT,REM,SEQNO,POSTBool,CFLAG,CTRNO,CVDATE,CREM,CSEQNO,CPOST,STAMP")] CSRCP cSRCP)
        {
            bool detflag = db.HKRCMODEs.Where(x => x.RCMODE == cSRCP.RCMODE).Select(y => y.DETFLAG).FirstOrDefault() == "Y";
            if (detflag)
            {
                if (cSRCP.ISSBANK == null) { ModelState.AddModelError(string.Empty, "Issue Bank is required"); }
                if (cSRCP.ISSREFNO == null) { ModelState.AddModelError(string.Empty, "Issue Reference is required"); }
                if (cSRCP.ISSDATE == null) { ModelState.AddModelError(string.Empty, "Issue Date is required"); }
                if (cSRCP.ISSLOC == null) { ModelState.AddModelError(string.Empty, "Issue Location is required"); }
            }
            if (string.IsNullOrEmpty(cSRCP.CONO)) { ModelState.AddModelError("CONO", "Company is required"); }
            if (string.IsNullOrEmpty(cSRCP.RCMODE)) { ModelState.AddModelError("RCMODE", "RC Mode is required"); }
            if (string.IsNullOrEmpty(cSRCP.RCMAPCODE)) { ModelState.AddModelError("RCMAPCODE", "Mode Map is required"); }

            if (ModelState.IsValid)
            {
                ASIDBConnection newdb = new ASIDBConnection();

                try
                {
                    CSRCP curRec = newdb.CSRCPs.Find(cSRCP.TRNO);
                    if (curRec.STAMP == cSRCP.STAMP)
                    {
                        cSRCP.STAMP = cSRCP.STAMP + 1;
                        db.Entry(cSRCP).State = EntityState.Modified;

                        CSLDG cSLDG = db.CSLDGs.Find("CSRCP", cSRCP.TRNO, 0);
                        if (cSLDG == null)
                        {
                            cSLDG = new CSLDG();
                            cSLDG.STAMP = 0;
                            db.CSLDGs.Add(cSLDG);
                        }
                        else
                        {
                            db.Entry(cSLDG).State = EntityState.Modified;
                        }

                        UpdateCSLDG(cSRCP, cSLDG);

                        //CMRECD cMRECD = db.CMRECDs.Find("CSRCP", cSRCP.TRNO, 0);
                        //if (cMRECD == null)
                        //{
                        //    cMRECD = new CMRECD();
                        //    cMRECD.STAMP = 0;
                        //    cMRECD.TRNO = null;
                        //    db.CMRECDs.Add(cMRECD);
                        //}
                        //else
                        //{
                        //    db.Entry(cMRECD).State = EntityState.Modified;
                        //}

                        //UpdateCMRECD(cSRCP, cMRECD);

                        db.SaveChanges();

                        int page = (int)Session["CSRCPPage"];
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
            ViewBag.DETFLAG = db.HKRCMODEs.Where(x => x.RCMODE == cSRCP.RCMODE).Select(y => y.DETFLAG).FirstOrDefault();
            ViewBag.CONAME = db.CSCOMSTRs.Where(x => x.CONO == cSRCP.CONO).Select(y => y.CONAME).FirstOrDefault();
            ViewBag.CSTRANM = db.CSTRANMs.Where(x => x.SOURCE == "CSRCP" && x.SOURCENO == cSRCP.TRNO);
            ViewBag.CONO = new SelectList(db.CSCOMSTRs.Select(x => new { CONO = x.CONO, CONAME = x.CONAME + "  (" + x.CONO + ")" }).OrderBy(y => y.CONAME), "CONO", "CONAME", cSRCP.CONO);
            ViewBag.RCMAPCODE = new SelectList(db.HKMAPs, "MAPCODE", "MAPCODE", cSRCP.RCMAPCODE);
            ViewBag.ISSBANK = new SelectList(db.HKBANKs.OrderBy(x => x.BANKDESC), "BANKCODE", "BANKDESC", cSRCP.ISSBANK);
            ViewBag.RCMODE = new SelectList(db.HKRCMODEs, "RCMODE", "RCMODE", cSRCP.RCMODE);
            ViewBag.ISSLOC = new SelectList(db.HKRCISSLOCs, "ISSLOC", "ISSLOC", cSRCP.ISSLOC);
            return View("Edit", cSRCP);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditC([Bind(Include = "TRNO,VDATE,CONO,RCAMT,RCMODE,RCMAPCODE,ISSBANK,ISSLOC,ISSREFNO,ISSDATE,COMAMT,NETAMT,REM,SEQNO,POSTBool,CFLAG,CTRNO,CVDATE,CREM,CSEQNO,CPOST,STAMP")] CSRCP cSRCP)
        {

            if (ModelState.IsValid)
            {
                ASIDBConnection newdb = new ASIDBConnection();

                try
                {
                    CSRCP curRec = newdb.CSRCPs.Find(cSRCP.TRNO);
                    if (curRec.STAMP == cSRCP.STAMP)
                    {
                        cSRCP.STAMP = cSRCP.STAMP + 1;
                        db.Entry(cSRCP).State = EntityState.Modified;



                        db.SaveChanges();

                        int page = (int)Session["CSRCPCPage"];
                        return RedirectToAction("IndexC", new { page = page });
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
            ViewBag.DETFLAG = db.HKRCMODEs.Where(x => x.RCMODE == cSRCP.RCMODE).Select(y => y.DETFLAG).FirstOrDefault();
            ViewBag.CONAME = db.CSCOMSTRs.Where(x => x.CONO == cSRCP.CONO).Select(y => y.CONAME).FirstOrDefault();
            ViewBag.CSTRANM = db.CSTRANMs.Where(x => x.SOURCE == "CSRCP" && x.SOURCENO == cSRCP.TRNO);
            ViewBag.CONO = new SelectList(db.CSCOMSTRs.Select(x => new { CONO = x.CONO, CONAME = x.CONAME + "  (" + x.CONO + ")" }).OrderBy(y => y.CONAME), "CONO", "CONAME", cSRCP.CONO);
            ViewBag.RCMAPCODE = new SelectList(db.HKMAPs, "MAPCODE", "MAPCODE", cSRCP.RCMAPCODE);
            ViewBag.ISSBANK = new SelectList(db.HKBANKs.OrderBy(x => x.BANKDESC), "BANKCODE", "BANKDESC", cSRCP.ISSBANK);
            ViewBag.RCMODE = new SelectList(db.HKRCMODEs, "RCMODE", "RCMODE", cSRCP.RCMODE);
            ViewBag.ISSLOC = new SelectList(db.HKRCISSLOCs, "ISSLOC", "ISSLOC", cSRCP.ISSLOC);
            return View("EditC", cSRCP);
        }

        // GET: CSRCPs/Delete/5
        public ActionResult Delete(string id, int? page)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSRCP cSRCP = db.CSRCPs.Find(MyHtmlHelpers.ConvertByteStrToId(id));
            if (cSRCP == null)
            {
                return HttpNotFound();
            }
            else
            {
                if (cSRCP.refcnt > 0)
                {
                    ModelState.AddModelError(string.Empty, cSRCP.refcnt.ToString() + " Details has been touched. Cannot Delete record");

                }
            }
            Session["CSRCPPage"] = page ?? 1;

            ViewBag.Title = "Delete Receipt";
            ViewBag.DETFLAG = db.HKRCMODEs.Where(x => x.RCMODE == cSRCP.RCMODE).Select(y => y.DETFLAG).FirstOrDefault();
            ViewBag.CONAME = db.CSCOMSTRs.Where(x => x.CONO == cSRCP.CONO).Select(y => y.CONAME).FirstOrDefault();
            ViewBag.CSTRANM = db.CSTRANMs.Where(x => x.SOURCE == "CSRCP" && x.SOURCENO == cSRCP.TRNO);
            ViewBag.CONO = new SelectList(db.CSCOMSTRs.Select(x => new { CONO = x.CONO, CONAME = x.CONAME + "  (" + x.CONO + ")" }).OrderBy(y => y.CONAME), "CONO", "CONAME", cSRCP.CONO);
            ViewBag.RCMAPCODE = new SelectList(db.HKMAPs, "MAPCODE", "MAPCODE", cSRCP.RCMAPCODE);
            ViewBag.ISSBANK = new SelectList(db.HKBANKs.OrderBy(x => x.BANKDESC), "BANKCODE", "BANKDESC", cSRCP.ISSBANK);
            ViewBag.RCMODE = new SelectList(db.HKRCMODEs, "RCMODE", "RCMODE", cSRCP.RCMODE);
            ViewBag.ISSLOC = new SelectList(db.HKRCISSLOCs, "ISSLOC", "ISSLOC", cSRCP.ISSLOC);
            return View("Edit", cSRCP);
        }

        public ActionResult DeleteC(string id, int? page)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSRCP cSRCP = db.CSRCPs.Find(MyHtmlHelpers.ConvertByteStrToId(id));
            if (cSRCP == null)
            {
                return HttpNotFound();
            }
            else
            {
                if (cSRCP.refcntC > 0)
                {
                    ModelState.AddModelError(string.Empty, cSRCP.refcntC.ToString() + " Details has been touched. Cannot Delete record");

                }
            }
            Session["CSRCPCPage"] = page ?? 1;

            ViewBag.Title = "Delete Cancellation";
            ViewBag.DETFLAG = db.HKRCMODEs.Where(x => x.RCMODE == cSRCP.RCMODE).Select(y => y.DETFLAG).FirstOrDefault();
            ViewBag.CONAME = db.CSCOMSTRs.Where(x => x.CONO == cSRCP.CONO).Select(y => y.CONAME).FirstOrDefault();
            ViewBag.CSTRANM = db.CSTRANMs.Where(x => x.SOURCE == "CSRCP" && x.SOURCENO == cSRCP.TRNO);
            ViewBag.CONO = new SelectList(db.CSCOMSTRs.Select(x => new { CONO = x.CONO, CONAME = x.CONAME + "  (" + x.CONO + ")" }).OrderBy(y => y.CONAME), "CONO", "CONAME", cSRCP.CONO);
            ViewBag.RCMAPCODE = new SelectList(db.HKMAPs, "MAPCODE", "MAPCODE", cSRCP.RCMAPCODE);
            ViewBag.ISSBANK = new SelectList(db.HKBANKs.OrderBy(x => x.BANKDESC), "BANKCODE", "BANKDESC", cSRCP.ISSBANK);
            ViewBag.RCMODE = new SelectList(db.HKRCMODEs, "RCMODE", "RCMODE", cSRCP.RCMODE);
            ViewBag.ISSLOC = new SelectList(db.HKRCISSLOCs, "ISSLOC", "ISSLOC", cSRCP.ISSLOC);
            return View("EditC", cSRCP);
        }

        // POST: CSRCPs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id, DateTime? CVDATE, string CREM)
        {
            CSRCP cSRCP = db.CSRCPs.Find(MyHtmlHelpers.ConvertByteStrToId(id));

            //db.CSRCPs.Remove(cSRCP); treat as cancellation
            if ((cSRCP != null) && (cSRCP.refcnt == 0))
            {

                SALASTNO serialTbl = db.SALASTNOes.Find("CSRCPC");
                if (serialTbl != null)
                {
                    CSCOMSTR cSCOMSTR = db.CSCOMSTRs.Find(cSRCP.CONO);
                    if (cSCOMSTR != null)
                    {
                        try
                        {
                            string prefix = serialTbl.LASTPFIX;
                            int MaxNo = serialTbl.LASTNOMAX;
                            bool AutoGen = serialTbl.AUTOGEN == "Y";
                            serialTbl.LASTNO = serialTbl.LASTNO + 1;
                            cSRCP.CTRNO = serialTbl.LASTNO.ToString("D10");

                            serialTbl.STAMP = serialTbl.STAMP + 1;
                            db.Entry(serialTbl).State = EntityState.Modified;

                            // increment company seqno count before using it in transaction
                            cSCOMSTR.SEQNO = cSCOMSTR.SEQNO + 1;
                            cSCOMSTR.STAMP = cSCOMSTR.STAMP + 1;

                            cSRCP.CSEQNO = cSCOMSTR.SEQNO;
                            cSRCP.CFLAG = "Y";
                            cSRCP.STAMP = cSRCP.STAMP + 1;
                            cSRCP.CVDATE = CVDATE;
                            cSRCP.CREM = CREM;
                            cSRCP.CPOST = "N";

                            db.Entry(cSCOMSTR).State = EntityState.Modified;

                            // Reverse CSLDG Entries
                            var cSLDGs = db.CSLDGs.Where(x => x.SOURCE == "CSRCP" && x.SOURCENO == cSRCP.TRNO);
                            foreach (CSLDG item in cSLDGs)
                            {
                                CSLDG newItem = new CSLDG();

                                newItem.SOURCE = "CSRCPC";
                                newItem.SOURCENO = cSRCP.CTRNO;
                                newItem.SOURCEID = item.SOURCEID;
                                newItem.CONO = item.CONO;
                                newItem.CASECODE = item.CASECODE;
                                newItem.ENTDATE = cSRCP.CVDATE ?? cSRCP.VDATE;
                                newItem.SEQNO = cSRCP.CSEQNO ?? cSRCP.SEQNO;
                                newItem.STAMP = cSRCP.STAMP;

                                newItem.DEP = -item.DEP;
                                newItem.DEPREC = -item.DEPREC;
                                newItem.FEE1 = -item.FEE1;
                                newItem.FEE2 = -item.FEE2;
                                newItem.FEEREC1 = -item.FEEREC1;
                                newItem.FEEREC2 = -item.FEEREC2;
                                newItem.TAX1 = -item.TAX1;
                                newItem.TAX2 = -item.TAX2;
                                newItem.TAXREC1 = -item.TAXREC1;
                                newItem.TAXREC2 = -item.TAXREC2;
                                newItem.WORK1 = -item.WORK1;
                                newItem.WORK2 = -item.WORK2;
                                newItem.WORKREC1 = -item.WORKREC1;
                                newItem.WORKREC2 = -item.WORKREC2;
                                newItem.DISB1 = -item.DISB1;
                                newItem.DISB2 = -item.DISB2;
                                newItem.DISBREC1 = -item.DISBREC1;
                                newItem.DISBREC2 = -item.DISBREC2;
                                newItem.REIMB1 = -(item.REIMB1 ?? 0);
                                newItem.REIMB2 = -(item.REIMB2 ?? 0);
                                newItem.REIMBREC1 = -(item.REIMBREC1 ?? 0);
                                newItem.REIMBREC2 = -(item.REIMBREC2 ?? 0);
                                newItem.RECEIPT = -item.RECEIPT;
                                newItem.ADVANCE = -item.ADVANCE;

                                db.CSLDGs.Add(newItem);
                            }

                            // Reverse CSTRANM entries
                            var cSTRANMs = db.CSTRANMs.Where(x => x.SOURCE == "CSRCP" && x.SOURCENO == cSRCP.TRNO);
                            foreach (CSTRANM item in cSTRANMs)
                            {
                                CSTRANM cSTRANM = new WebApplication1.CSTRANM();
                                cSTRANM.SOURCE = "CSRCPC";
                                cSTRANM.SOURCENO = cSRCP.CTRNO;
                                cSTRANM.SOURCEID = item.SOURCEID;
                                cSTRANM.SEQNO = cSRCP.CSEQNO ?? cSRCP.SEQNO;
                                cSTRANM.REFCNT = 0;
                                cSTRANM.STAMP = 0;
                                cSTRANM.CONO = item.CONO;
                                cSTRANM.JOBNO = item.JOBNO;
                                cSTRANM.CASENO = item.CASENO;
                                cSTRANM.CASECODE = item.CASECODE;
                                cSTRANM.ENTDATE = cSRCP.CVDATE ?? cSRCP.VDATE;
                                cSTRANM.DUEDATE = cSRCP.CVDATE ?? cSRCP.VDATE;
                                cSTRANM.TRTYPE = item.TRTYPE;
                                cSTRANM.TRDESC = item.TRDESC;
                                cSTRANM.CASECODE = item.CASECODE;

                                cSTRANM.TRITEM = item.TRITEM;
                                cSTRANM.TRITEM1 = item.TRITEM1;
                                cSTRANM.TRITEM2 = item.TRITEM2;
                                cSTRANM.TRTAX = item.TRTAX;
                                cSTRANM.TRTAX1 = item.TRTAX1;
                                cSTRANM.TRTAX2 = item.TRTAX2;
                                cSTRANM.TRAMT = item.TRAMT;
                                cSTRANM.TRAMT1 = item.TRAMT1;
                                cSTRANM.TRAMT2 = item.TRAMT2;

                                cSTRANM.TRSIGN = "DB";

                                cSTRANM.TRSITEM = -item.TRSITEM;
                                cSTRANM.TRSITEM1 = -item.TRSITEM1;
                                cSTRANM.TRSITEM2 = -item.TRSITEM2;
                                cSTRANM.TRSTAX = -item.TRSTAX;
                                cSTRANM.TRSTAX1 = -item.TRSTAX1;
                                cSTRANM.TRSTAX2 = -item.TRSTAX2;
                                cSTRANM.TRSAMT = -item.TRSAMT;
                                cSTRANM.TRSAMT1 = -item.TRSAMT1;
                                cSTRANM.TRSAMT2 = -item.TRSAMT2;

                                cSTRANM.TRITEMOS = item.APPITEM;
                                cSTRANM.TRITEMOS1 = item.APPITEM1;
                                cSTRANM.TRITEMOS2 = item.APPITEM2;
                                cSTRANM.TRTAXOS = item.APPTAX;
                                cSTRANM.TRTAXOS1 = item.APPTAX1;
                                cSTRANM.TRTAXOS2 = item.APPTAX2;
                                cSTRANM.TROS = item.APPAMT;
                                cSTRANM.TROS1 = item.APPAMT1;
                                cSTRANM.TROS2 = item.APPAMT2;

                                cSTRANM.REM = "Cancellation for receipt no " + item.SOURCENO + "/" + item.SOURCEID;
                                cSTRANM.COMPLETE = "N";
                                cSTRANM.COMPLETED = new DateTime(3000, 1, 1);

                                db.CSTRANMs.Add(cSTRANM);
                            }


                            db.Entry(cSRCP).State = EntityState.Modified;


                            db.SaveChanges();
                            int page = (int)Session["CSRCPPage"];
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
            else
            {
                if (cSRCP == null)
                {
                    ModelState.AddModelError(string.Empty, "Receipt has been modified. Cannot Delete record");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, cSRCP.refcnt.ToString() + " Details has been touched. Cannot Delete record");
                }
            }

            ViewBag.page = Session["CSRCPPage"];

            ViewBag.Title = "Delete Receipt";
            ViewBag.DETFLAG = db.HKRCMODEs.Where(x => x.RCMODE == cSRCP.RCMODE).Select(y => y.DETFLAG).FirstOrDefault();
            ViewBag.CONAME = db.CSCOMSTRs.Where(x => x.CONO == cSRCP.CONO).Select(y => y.CONAME).FirstOrDefault();
            ViewBag.CSTRANM = db.CSTRANMs.Where(x => x.SOURCE == "CSRCP" && x.SOURCENO == cSRCP.TRNO);
            ViewBag.CONO = new SelectList(db.CSCOMSTRs.Select(x => new { CONO = x.CONO, CONAME = x.CONAME + "  (" + x.CONO + ")" }).OrderBy(y => y.CONAME), "CONO", "CONAME", cSRCP.CONO);
            ViewBag.RCMAPCODE = new SelectList(db.HKMAPs, "MAPCODE", "MAPCODE", cSRCP.RCMAPCODE);
            ViewBag.ISSBANK = new SelectList(db.HKBANKs.OrderBy(x => x.BANKDESC), "BANKCODE", "BANKDESC", cSRCP.ISSBANK);
            ViewBag.RCMODE = new SelectList(db.HKRCMODEs, "RCMODE", "RCMODE", cSRCP.RCMODE);
            ViewBag.ISSLOC = new SelectList(db.HKRCISSLOCs, "ISSLOC", "ISSLOC", cSRCP.ISSLOC);
            return View("Edit", cSRCP);

        }


        [HttpPost, ActionName("DeleteC")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteCConfirmed(string id)
        {
            CSRCP cSRCP = db.CSRCPs.Find(MyHtmlHelpers.ConvertByteStrToId(id));

            if ((cSRCP != null) && (cSRCP.refcntC == 0))
            {

                //db.CSRCPs.Remove(cSRCP); treat as cancellation

                List<CSTRANM> cSTRANMs = db.CSTRANMs.Where(x => x.SOURCE == "CSRCPC" && x.SOURCENO == cSRCP.CTRNO).ToList();
                int errcnt = cSTRANMs.Sum(x => x.REFCNT);
                if (errcnt == 0)
                {
                    CSCOMSTR cSCOMSTR = db.CSCOMSTRs.Find(cSRCP.CONO);
                    if (cSCOMSTR != null)
                    {
                        try
                        {

                            // increment company seqno count before using it in transaction
                            cSCOMSTR.SEQNO = cSCOMSTR.SEQNO + 1;
                            cSCOMSTR.STAMP = cSCOMSTR.STAMP + 1;


                            db.Entry(cSCOMSTR).State = EntityState.Modified;

                            // Reverse CSLDG Entries
                            List<CSLDG> cSLDGs = db.CSLDGs.Where(x => x.SOURCE == "CSRCPC" && x.SOURCENO == cSRCP.CTRNO).ToList();

                            foreach (CSLDG item in cSLDGs)
                            {
                                CSLDG cSLDG = db.CSLDGs.Find(item.SOURCE, item.SOURCENO, item.SOURCEID);
                                if (cSLDG != null)
                                {
                                    db.CSLDGs.Remove(cSLDG);
                                }
                            }

                            // Reverse CSTRANM entries

                            foreach (CSTRANM item in cSTRANMs)
                            {

                                CSTRANM cSTRANM = db.CSTRANMs.Find(item.SOURCE, item.SOURCENO, item.SOURCEID);
                                if (cSTRANM != null)
                                {
                                    db.CSTRANMs.Remove(cSTRANM);
                                }

                            }

                            cSRCP.CSEQNO = null;
                            cSRCP.CFLAG = "N";
                            cSRCP.STAMP = cSRCP.STAMP + 1;
                            cSRCP.CVDATE = null;
                            cSRCP.CREM = null;
                            cSRCP.CPOST = null;

                            db.Entry(cSRCP).State = EntityState.Modified;


                            db.SaveChanges();

                            int page = (int)Session["CSRCPCPage"];
                            return RedirectToAction("IndexC", new { page = page });

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
                else
                {
                    ModelState.AddModelError(string.Empty, errcnt.ToString() + " records has been used");
                }
            }
            else
            {
                if (cSRCP == null)
                {
                    ModelState.AddModelError(string.Empty, "Receipt Cancellation has been modified. Cannot Delete record");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, cSRCP.refcntC.ToString() + " Details has been touched. Cannot Delete record");
                }
            }

            ViewBag.page = (int)Session["CSRCPCPage"];

            ViewBag.Title = "Delete Cancellation";
            ViewBag.DETFLAG = db.HKRCMODEs.Where(x => x.RCMODE == cSRCP.RCMODE).Select(y => y.DETFLAG).FirstOrDefault();
            ViewBag.CONAME = db.CSCOMSTRs.Where(x => x.CONO == cSRCP.CONO).Select(y => y.CONAME).FirstOrDefault();
            ViewBag.CSTRANM = db.CSTRANMs.Where(x => x.SOURCE == "CSRCP" && x.SOURCENO == cSRCP.TRNO);
            ViewBag.CONO = new SelectList(db.CSCOMSTRs.Select(x => new { CONO = x.CONO, CONAME = x.CONAME + "  (" + x.CONO + ")" }).OrderBy(y => y.CONAME), "CONO", "CONAME", cSRCP.CONO);
            ViewBag.RCMAPCODE = new SelectList(db.HKMAPs, "MAPCODE", "MAPCODE", cSRCP.RCMAPCODE);
            ViewBag.ISSBANK = new SelectList(db.HKBANKs.OrderBy(x => x.BANKDESC), "BANKCODE", "BANKDESC", cSRCP.ISSBANK);
            ViewBag.RCMODE = new SelectList(db.HKRCMODEs, "RCMODE", "RCMODE", cSRCP.RCMODE);
            ViewBag.ISSLOC = new SelectList(db.HKRCISSLOCs, "ISSLOC", "ISSLOC", cSRCP.ISSLOC);
            return View("EditC", cSRCP);
        }

        public ActionResult ReceiptList(int? page)
        {
            if (page > 0)
            {
                int prevPage = (page ?? 1) - 1;
                return View(CurrentSelection().Skip(prevPage * 30).Take(30).ToList());
            }
            return View(CurrentSelection().ToList());
        }
        public ActionResult Receipt(string id)
        {
            string sid = MyHtmlHelpers.ConvertByteStrToId(id);
            return View(getReceipt(sid));
        }

        public PartialViewResult ReceiptM(string id)
        {
            string sid = MyHtmlHelpers.ConvertByteStrToId(id);
            return PartialView("Receipt", getReceipt(sid));
        }

        public CSRCP getReceipt(string sid)
        {
            CSRCP cSRCP = db.CSRCPs.Find(sid);

            SACOMP profileRec = db.SACOMPs.FirstOrDefault();
            ViewBag.Company = profileRec.CONAME.ToUpper() + " (" + profileRec.COREGNO + ") ";
            ViewBag.Address = profileRec.COADDR1 + " " + profileRec.COADDR2 + " " + profileRec.COADDR3 + " " + profileRec.COADDR4;
            ViewBag.Contact = "Tel: " + profileRec.COPHONE1 + " Fax: " + profileRec.COFAX1 + " E-Mail: " + profileRec.COWEB;

            ViewBag.Addr1 = "";
            ViewBag.Addr2 = "";

            var cSTRANMs = db.CSTRANMs.Where(x => x.SOURCE == "CSRCP" && x.SOURCENO == cSRCP.TRNO).OrderBy(y => y.SOURCEID);
            ViewBag.CSTRANM = cSTRANMs;
            ViewBag.StaffName = cSRCP.CSCOMSTR.HKSTAFF.STAFFDESC;
            return cSRCP;
        }

        public void UpdateCSLDG(CSRCP cSRCP, CSLDG cSLDG)
        {
            cSLDG.SOURCE = "CSRCP";
            cSLDG.SOURCENO = cSRCP.TRNO;
            cSLDG.SOURCEID = 0;

            cSLDG.CONO = cSRCP.CONO;
            cSLDG.ENTDATE = cSRCP.VDATE;
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
            cSLDG.RECEIPT = cSRCP.NETAMT;
            cSLDG.ADVANCE = 0;
            cSLDG.SEQNO = cSRCP.SEQNO;
            cSLDG.REIMB1 = 0;
            cSLDG.REIMB2 = 0;
            cSLDG.REIMBREC1 = 0;
            cSLDG.REIMBREC2 = 0;
            cSLDG.STAMP = cSLDG.STAMP + 1;
        }

        public void UpdateCMRECD(CSRCP cSRCP, CMRECD cMRECD)
        {
            cMRECD.SOURCE = "CSRCP";
            cMRECD.SOURCENO = cSRCP.TRNO;
            cMRECD.SOURCEID = 0;

            cMRECD.TRSIGN = "DB";
            cMRECD.RCPYMODE = cSRCP.RCMODE;
            cMRECD.ISSBANK = cSRCP.ISSBANK;
            cMRECD.ISSDATE = cSRCP.ISSDATE;
            cMRECD.ISSREFNO = cSRCP.ISSREFNO;
            cMRECD.ISSLOC = cSRCP.ISSLOC;
            cMRECD.CURRCODE = "MYR";
            cMRECD.AMT = cSRCP.RCAMT;
            cMRECD.COMAMT = cSRCP.COMAMT;
            cMRECD.NETAMT = cSRCP.NETAMT;
            cMRECD.SNETAMT = cSRCP.NETAMT;
            cMRECD.REM = cSRCP.REM;
            cMRECD.ENTDATE = cSRCP.VDATE;
            cMRECD.MAPCODE = cSRCP.RCMAPCODE;
            cMRECD.CLCODE = cSRCP.CONO;
            cMRECD.DONE = "N";
            cMRECD.REFCNT = 0;
            cMRECD.STAMP = cMRECD.STAMP + 1;

            if (cSRCP.ISSBANK == null)
            {
                cMRECD.DONE = "Y";
                cMRECD.TRNO = "";
            }
        }


        public PartialViewResult CSTRANMAllocated(string id)
        {
            string sid = MyHtmlHelpers.ConvertByteStrToId(id);
            return CSTRANMAllocated1(sid);
        }

        public PartialViewResult CSTRANMAllocatedC(string id)
        {
            string sid = MyHtmlHelpers.ConvertByteStrToId(id);
            return CSTRANMAllocatedC1(sid);
        }

        public PartialViewResult CSTRANMAllocated1(string trno)
        {
            var cSTRANMs = db.CSTRANMs.Where(x => x.SOURCENO == trno && x.SOURCE == "CSRCP").OrderBy(y => y.SOURCEID);
            ViewBag.TRNO = trno;
            return PartialView("Partial/CSTRANMAllocated", cSTRANMs);
        }

        public PartialViewResult CSTRANMAllocatedC1(string trno)
        {
            var cSTRANMs = db.CSTRANMs.Where(x => x.SOURCENO == trno && x.SOURCE == "CSRCPC").OrderBy(y => y.SOURCEID);
            ViewBag.TRNO = trno;
            return PartialView("Partial/CSTRANMAllocatedC", cSTRANMs);
        }


        public PartialViewResult CSTRANMOpen(string id, string cono)
        {
            string trno = MyHtmlHelpers.ConvertByteStrToId(id);

            string sid = MyHtmlHelpers.ConvertByteStrToId(cono);

            return CSTRANMOpen1(trno, sid);
        }

        public PartialViewResult CSTRANMOpen1(string trno, string cono)
        {

            ViewBag.TRNO = trno;
            DateTime rdate = db.CSRCPs.Where(x => x.TRNO == trno).Select(y => y.VDATE).FirstOrDefault();

            var cSTRANMs = db.CSTRANMs.Where(x => x.CONO == cono && x.COMPLETE == "N" && x.ENTDATE <= rdate && (x.TRSIGN == "DB")).OrderBy(y => y.SOURCENO);

            return PartialView("Partial/CSTRANMOpen", cSTRANMs);
        }


        public PartialViewResult CSTRANMAdvance(string id)
        {
            string sid = MyHtmlHelpers.ConvertByteStrToId(id);
            CSRCP cSRCP = db.CSRCPs.Find(sid);

            CSTRANM cSTRANM = new CSTRANM();
            if (cSRCP == null)
            {
            }
            else
            {

                cSTRANM.SOURCE = "CSRCP";
                cSTRANM.SOURCENO = sid;
                cSTRANM.SOURCEID = 0;
                cSTRANM.ENTDATE = cSRCP.VDATE;
                cSTRANM.CONO = cSRCP.CONO;
                cSTRANM.TRTYPE = "Advance";
                cSTRANM.TRDESC = "Advance";
                cSTRANM.APPTYPE = "Advance";
                cSTRANM.TRSIGN = "CR";
                cSTRANM.COMPLETE = "N";
                cSTRANM.COMPLETED = new DateTime(3000, 01, 01);
                cSTRANM.SEQNO = cSRCP.SEQNO;
                cSTRANM.REFCNT = 0;
                cSTRANM.STAMP = 0;
            }

            ViewBag.TRNO = sid;

            return PartialView("Partial/CSTRANMAdvance", cSTRANM);
        }


        public PartialViewResult AddItem(string source, string sourceno, int id, string trno)
        {
            //string sid = MyHtmlHelpers.ConvertByteStrToId(sourceno);
            CSRCP cSRCP = db.CSRCPs.Find(trno);
            CSTRANM cSTRANM = db.CSTRANMs.Find(source, sourceno, id);
            //
            //
            // can only apply up to total receipt amount when creating cstranm 
            // 
            //
            if (cSRCP != null)
            {
                if (cSTRANM != null)
                {
                    try
                    {
                        var cSTRANMs = db.CSTRANMs.Where(x => x.SOURCENO == cSRCP.TRNO && x.SOURCE == "CSRCP");

                        decimal prevTRITEM = 0;
                        decimal prevTRITEM1 = 0;
                        decimal prevTRITEM2 = 0;

                        decimal prevTRTAX = 0;
                        decimal prevTRTAX1 = 0;
                        decimal prevTRITAX2 = 0;

                        decimal prevTRAMT = 0;
                        decimal prevTRAMT1 = 0;
                        decimal prevTRAMT2 = 0;

                        int maxId = 0;
                        if (cSTRANMs.Count() != 0)
                        {
                            maxId = cSTRANMs.Max(y => y.SOURCEID);
                            prevTRITEM = cSTRANMs.Sum(y => y.TRITEM);
                            prevTRITEM1 = cSTRANMs.Sum(y => y.TRITEM1);
                            prevTRITEM2 = cSTRANMs.Sum(y => y.TRITEM2);

                            prevTRTAX = cSTRANMs.Sum(y => y.TRTAX);
                            prevTRTAX1 = cSTRANMs.Sum(y => y.TRTAX1);
                            prevTRITAX2 = cSTRANMs.Sum(y => y.TRTAX2);

                            prevTRAMT = cSTRANMs.Sum(y => y.TRAMT);
                            prevTRAMT1 = cSTRANMs.Sum(y => y.TRAMT1);
                            prevTRAMT2 = cSTRANMs.Sum(y => y.TRAMT2);
                        }

                        decimal MaxAllocAmt = cSRCP.NETAMT - prevTRAMT;
                        decimal CurAllocAmt = MaxAllocAmt;
                        decimal diff = 0;

                        if (MaxAllocAmt <= 0)
                        {
                            ModelState.AddModelError(string.Empty, "Receipt is fully allocated");
                            Response.StatusCode = (int)HttpStatusCode.NotAcceptable;
                            Response.StatusDescription = "Receipt is fully allocated";
                        }
                        else
                        {

                            CSTRANM cSTRANMRCP = new CSTRANM();
                            cSTRANMRCP.SOURCE = "CSRCP";
                            cSTRANMRCP.SOURCENO = cSRCP.TRNO;

                            // if applied amount is less than
                            // sign is the same here
                            if (cSTRANM.TROS <= MaxAllocAmt)
                            {
                                cSTRANMRCP.TRITEM = cSTRANM.TRITEMOS;
                                cSTRANMRCP.TRITEM1 = cSTRANM.TRITEMOS1;
                                cSTRANMRCP.TRITEM2 = cSTRANM.TRITEMOS2;
                                cSTRANMRCP.TRTAX = cSTRANM.TRTAXOS;
                                cSTRANMRCP.TRTAX1 = cSTRANM.TRTAXOS1;
                                cSTRANMRCP.TRTAX2 = cSTRANM.TRTAXOS2;
                                cSTRANMRCP.TRAMT = cSTRANM.TROS;
                                cSTRANMRCP.TRAMT1 = cSTRANM.TROS1;
                                cSTRANMRCP.TRAMT2 = cSTRANM.TROS2;

                                cSTRANM.TRITEMOS = 0;
                                cSTRANM.TRITEMOS1 = 0;
                                cSTRANM.TRITEMOS2 = 0;
                                cSTRANM.TRTAXOS = 0;
                                cSTRANM.TRTAXOS1 = 0;
                                cSTRANM.TRTAXOS2 = 0;
                                cSTRANM.TROS = 0;
                                cSTRANM.TROS1 = 0;
                                cSTRANM.TROS2 = 0;
                                cSTRANM.COMPLETE = "Y";
                                cSTRANM.COMPLETED = cSRCP.VDATE;
                                //cSTRANM.SEQNO = cSRCP.SEQNO;

                            }
                            else
                            {

                                cSTRANMRCP.TRITEM1 = 0;
                                cSTRANMRCP.TRITEM2 = 0;

                                cSTRANMRCP.TRTAX1 = 0;
                                cSTRANMRCP.TRTAX2 = 0;

                                cSTRANMRCP.TRAMT1 = 0;
                                cSTRANMRCP.TRAMT2 = 0;

                                if (cSTRANM.TRITEMOS1 >= CurAllocAmt)
                                {
                                    cSTRANMRCP.TRITEM1 = CurAllocAmt;
                                    cSTRANM.TRITEMOS1 -= CurAllocAmt;
                                    diff = 0;
                                    CurAllocAmt = 0;
                                }
                                else
                                {
                                    diff = CurAllocAmt - cSTRANM.TRITEMOS1;
                                    cSTRANMRCP.TRITEM1 = cSTRANM.TRITEMOS1;
                                    cSTRANM.TRITEMOS1 = 0;
                                    CurAllocAmt = diff;

                                    if (cSTRANM.TRITEMOS2 >= CurAllocAmt)
                                    {
                                        cSTRANMRCP.TRITEM2 = CurAllocAmt;
                                        cSTRANM.TRITEMOS2 -= CurAllocAmt;
                                        diff = 0;
                                        CurAllocAmt = 0;
                                    }
                                    else
                                    {
                                        diff = CurAllocAmt - cSTRANM.TRITEMOS2;
                                        cSTRANMRCP.TRITEM2 = cSTRANM.TRITEMOS2;
                                        cSTRANM.TRITEMOS2 = 0;
                                        CurAllocAmt = diff;

                                        if (cSTRANM.TRTAXOS1 >= CurAllocAmt)
                                        {
                                            cSTRANMRCP.TRTAX1 = CurAllocAmt;
                                            cSTRANM.TRTAXOS1 -= CurAllocAmt;
                                            diff = 0;
                                            CurAllocAmt = 0;
                                        }
                                        else
                                        {
                                            diff = CurAllocAmt - cSTRANM.TRTAXOS1;
                                            cSTRANMRCP.TRTAX1 = cSTRANM.TRTAXOS1;
                                            cSTRANM.TRTAXOS1 = 0;
                                            CurAllocAmt = diff;

                                            if (cSTRANM.TRTAXOS2 >= CurAllocAmt)
                                            {
                                                cSTRANMRCP.TRTAX2 = CurAllocAmt;
                                                cSTRANM.TRTAXOS2 -= CurAllocAmt;
                                                diff = 0;
                                                CurAllocAmt = 0;
                                            }
                                            else
                                            {
                                                diff = CurAllocAmt - cSTRANM.TRTAXOS2;
                                                cSTRANMRCP.TRTAX2 = cSTRANM.TRTAXOS2;
                                                cSTRANM.TRTAXOS2 = 0;
                                                CurAllocAmt = diff;
                                            }
                                        }
                                    }
                                }

                                cSTRANMRCP.TRTAX = cSTRANMRCP.TRTAX1 + cSTRANMRCP.TRTAX2;
                                cSTRANMRCP.TRITEM = cSTRANMRCP.TRITEM1 + cSTRANMRCP.TRITEM2;
                                cSTRANMRCP.TRAMT1 = cSTRANMRCP.TRITEM1 + cSTRANMRCP.TRTAX1;
                                cSTRANMRCP.TRAMT2 = cSTRANMRCP.TRITEM2 + cSTRANMRCP.TRTAX2;
                                cSTRANMRCP.TRAMT = cSTRANMRCP.TRAMT1 + cSTRANMRCP.TRAMT2;
                                cSTRANM.TRITEMOS = cSTRANM.TRITEMOS1 + cSTRANM.TRITEMOS2;
                                cSTRANM.TRTAXOS = cSTRANM.TRTAXOS1 + cSTRANM.TRTAXOS2;
                                cSTRANM.TROS1 = cSTRANM.TRITEMOS1 + cSTRANM.TRTAXOS1;
                                cSTRANM.TROS2 = cSTRANM.TRITEMOS2 + cSTRANM.TRTAXOS2;
                                cSTRANM.TROS = cSTRANM.TROS1 + cSTRANM.TROS2;

                            }
                            cSTRANM.REFCNT = cSTRANM.REFCNT + 1;
                            cSTRANM.STAMP = cSTRANM.STAMP + 1;


                            cSTRANMRCP.SOURCEID = maxId + 1;
                            cSTRANMRCP.CONO = cSTRANM.CONO;
                            cSTRANMRCP.JOBNO = cSTRANM.JOBNO;
                            cSTRANMRCP.CASENO = cSTRANM.CASENO;
                            cSTRANMRCP.CASECODE = cSTRANM.CASECODE;
                            cSTRANMRCP.DUEDATE = cSTRANM.DUEDATE;
                            cSTRANMRCP.ENTDATE = cSTRANM.ENTDATE;
                            cSTRANMRCP.TRTYPE = cSTRANM.TRTYPE;
                            cSTRANMRCP.TRDESC = cSTRANM.TRDESC;
                            cSTRANMRCP.CONO = cSTRANM.CONO;


                            cSTRANMRCP.TRSIGN = "CR";

                            cSTRANMRCP.TRSITEM = -cSTRANMRCP.TRITEM;
                            cSTRANMRCP.TRSITEM1 = -cSTRANMRCP.TRITEM1;
                            cSTRANMRCP.TRSITEM2 = -cSTRANMRCP.TRITEM2;
                            cSTRANMRCP.TRSTAX = -cSTRANMRCP.TRTAX;
                            cSTRANMRCP.TRSTAX1 = -cSTRANMRCP.TRTAX1;
                            cSTRANMRCP.TRSTAX2 = -cSTRANMRCP.TRTAX2;
                            cSTRANMRCP.TRSAMT = -cSTRANMRCP.TRAMT;
                            cSTRANMRCP.TRSAMT1 = -cSTRANMRCP.TRAMT1;
                            cSTRANMRCP.TRSAMT2 = -cSTRANMRCP.TRAMT2;

                            cSTRANMRCP.TRITEMOS = 0;
                            cSTRANMRCP.TRITEMOS1 = 0;
                            cSTRANMRCP.TRITEMOS2 = 0;
                            cSTRANMRCP.TRTAXOS = 0;
                            cSTRANMRCP.TRTAXOS1 = 0;
                            cSTRANMRCP.TRTAXOS2 = 0;
                            cSTRANMRCP.TROS = 0;
                            cSTRANMRCP.TROS1 = 0;
                            cSTRANMRCP.TROS2 = 0;

                            cSTRANMRCP.APPTYPE = cSTRANM.SOURCE;
                            cSTRANMRCP.APPNO = cSTRANM.SOURCENO;
                            cSTRANMRCP.APPID = cSTRANM.SOURCEID;
                            cSTRANMRCP.APPITEM = cSTRANMRCP.TRITEM;
                            cSTRANMRCP.APPITEM1 = cSTRANMRCP.TRITEM1;
                            cSTRANMRCP.APPITEM2 = cSTRANMRCP.TRITEM2;
                            cSTRANMRCP.APPTAX = cSTRANMRCP.TRTAX;
                            cSTRANMRCP.APPTAX1 = cSTRANMRCP.TRTAX1;
                            cSTRANMRCP.APPTAX2 = cSTRANMRCP.TRTAX2;
                            cSTRANMRCP.APPAMT = cSTRANMRCP.TRAMT;
                            cSTRANMRCP.APPAMT1 = cSTRANMRCP.TRAMT1;
                            cSTRANMRCP.APPAMT2 = cSTRANMRCP.TRAMT2;

                            cSTRANMRCP.REM = cSTRANM.REM;
                            cSTRANMRCP.COMPLETE = "Y";
                            cSTRANMRCP.COMPLETED = cSRCP.VDATE;
                            cSTRANMRCP.SEQNO = cSRCP.SEQNO;
                            cSTRANMRCP.REFCNT = 0;
                            cSTRANMRCP.STAMP = 0;


                            //cSTRANM.SEQNO = cSRCP.SEQNO;

                            db.Entry(cSTRANM).State = EntityState.Modified;
                            db.CSTRANMs.Add(cSTRANMRCP);

                            CSTRAND cSTRAND = new CSTRAND();
                            cSTRAND.SOURCE = cSTRANMRCP.SOURCE;
                            cSTRAND.SOURCENO = cSTRANMRCP.SOURCENO;
                            cSTRAND.SOURCEID = cSTRANMRCP.SOURCEID;
                            cSTRAND.DBTYPE = cSTRANM.SOURCE;
                            cSTRAND.DBNO = cSTRANM.SOURCENO;
                            cSTRAND.DBID = cSTRANM.SOURCEID;
                            cSTRAND.CRTYPE = cSTRANMRCP.SOURCE;
                            cSTRAND.CRNO = cSTRANMRCP.SOURCENO;
                            cSTRAND.CRID = cSTRANMRCP.SOURCEID;
                            cSTRAND.APPDATE = cSRCP.VDATE;
                            cSTRAND.APPITEM = cSTRANMRCP.TRITEM;
                            cSTRAND.APPITEM1 = cSTRANMRCP.TRITEM1;
                            cSTRAND.APPITEM2 = cSTRANMRCP.TRITEM2;
                            cSTRAND.APPTAX = cSTRANMRCP.TRTAX;
                            cSTRAND.APPTAX1 = cSTRANMRCP.TRTAX1;
                            cSTRAND.APPTAX2 = cSTRANMRCP.TRTAX2;
                            cSTRAND.APPAMT = cSTRANMRCP.TRAMT;
                            cSTRAND.APPAMT1 = cSTRANMRCP.TRAMT1;
                            cSTRAND.APPAMT2 = cSTRANMRCP.TRAMT2;
                            cSTRAND.STAMP = 0;

                            db.CSTRANDs.Add(cSTRAND);

                            CSLDG cSLDG = new CSLDG();
                            cSLDG.SOURCE = cSTRANMRCP.SOURCE;
                            cSLDG.SOURCENO = cSTRANMRCP.SOURCENO;
                            cSLDG.SOURCEID = cSTRANMRCP.SOURCEID;
                            cSLDG.STAMP = 0;

                            cSLDG.CONO = cSTRANMRCP.CONO;
                            //cSLDG.ENTDATE = cSTRANMRCP.ENTDATE;
                            cSLDG.ENTDATE = cSRCP.VDATE;
                            cSLDG.JOBNO = cSTRANM.JOBNO;
                            cSLDG.CASENO = cSTRANM.CASENO;
                            cSLDG.CASECODE = cSTRANM.CASECODE;

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
                            cSLDG.SEQNO = cSTRANMRCP.SEQNO;
                            cSLDG.REIMB1 = 0;
                            cSLDG.REIMB2 = 0;
                            cSLDG.REIMBREC1 = 0;
                            cSLDG.REIMBREC2 = 0;

                            if (cSTRANM.TRTYPE == "Fee")
                            {
                                cSLDG.FEE1 = cSTRANMRCP.TRITEM1;
                                cSLDG.FEE2 = cSTRANMRCP.TRITEM2;
                                cSLDG.TAX1 = cSTRANMRCP.TRTAX1;
                                cSLDG.TAX2 = cSTRANMRCP.TRTAX2;
                            }

                            if (cSTRANM.TRTYPE == "Work")
                            {
                                cSLDG.WORK1 = cSTRANMRCP.TRITEM1;
                                cSLDG.WORK2 = cSTRANMRCP.TRITEM2;
                                cSLDG.TAX1 = cSTRANMRCP.TRTAX1;
                                cSLDG.TAX2 = cSTRANMRCP.TRTAX2;
                            }


                            if (cSTRANM.TRTYPE == "Disbursement")
                            {
                                cSLDG.DISB1 = cSTRANMRCP.TRITEM1;
                                cSLDG.DISB2 = cSTRANMRCP.TRITEM2;
                                cSLDG.TAX1 = cSTRANMRCP.TRTAX1;
                                cSLDG.TAX2 = cSTRANMRCP.TRTAX2;
                            }

                            if (cSTRANM.TRTYPE == "Reimbursement")
                            {
                                cSLDG.REIMB1 = cSTRANMRCP.TRITEM1;
                                cSLDG.REIMB2 = cSTRANMRCP.TRITEM2;
                                cSLDG.TAX1 = cSTRANMRCP.TRTAX1;
                                cSLDG.TAX2 = cSTRANMRCP.TRTAX2;
                            }
                            if (cSTRANM.TRTYPE == "Advance")
                            {
                                cSLDG.ADVANCE = cSTRANMRCP.TRAMT;

                            }
                            if (cSTRANM.TRTYPE == "Deposit")
                            {
                                cSLDG.DEP = cSTRANMRCP.TRAMT;
                            }

                            db.CSLDGs.Add(cSLDG);

                            db.SaveChanges();
                        }
                    }
                    catch (Exception e)
                    {
                        Response.StatusCode = (int)HttpStatusCode.NotAcceptable;
                        Response.StatusDescription = e.Message;
                    }
                }
                else
                {
                    Response.StatusCode = (int)HttpStatusCode.NotAcceptable;
                    Response.StatusDescription = "selected allocation : " + source + "/" + source + "/" + id.ToString() + " is missing";
                }
            }
            else
            {
                Response.StatusCode = (int)HttpStatusCode.NotAcceptable;
                Response.StatusDescription = "Receipt : " + trno + " is missing";
            }



            string sid = MyHtmlHelpers.ConvertIdToByteStr(trno);
            return CSTRANMAllocated(sid);

        }

        public PartialViewResult RemoveItem(string sourceno, int id)
        {
            //string sid = MyHtmlHelpers.ConvertByteStrToId(sourceno);
            CSTRANM cSTRANM = db.CSTRANMs.Find("CSRCP", sourceno, id);

            if (cSTRANM == null)
            {
                Response.StatusCode = (int)HttpStatusCode.NotAcceptable;
                Response.StatusDescription = "Allocation Item is missing";
            }
            else if (cSTRANM.REFCNT > 0)
            {
                Response.StatusCode = (int)HttpStatusCode.NotAcceptable;
                Response.StatusDescription = "Allocation Item is used";
            }
            else
            {
                try
                {
                    if ((cSTRANM.TRTYPE == "Advance") && (cSTRANM.TRDESC == "Advance"))
                    {
                        return PartialView("Partial/CSTRANMAdvance", cSTRANM);
                    }
                    else
                    {


                        CSTRANM cSTRANMApp = db.CSTRANMs.Find(cSTRANM.APPTYPE, cSTRANM.APPNO, cSTRANM.APPID);
                        if (cSTRANMApp != null)
                        {
                            cSTRANMApp.TRITEMOS = cSTRANMApp.TRITEMOS + cSTRANM.TRITEM;
                            cSTRANMApp.TRITEMOS1 = cSTRANMApp.TRITEMOS1 + cSTRANM.TRITEM1;
                            cSTRANMApp.TRITEMOS2 = cSTRANMApp.TRITEMOS2 + cSTRANM.TRITEM2;
                            cSTRANMApp.TRTAXOS = cSTRANMApp.TRTAXOS + cSTRANM.TRTAX;
                            cSTRANMApp.TRTAXOS1 = cSTRANMApp.TRTAXOS1 + cSTRANM.TRTAX1;
                            cSTRANMApp.TRTAXOS2 = cSTRANMApp.TRTAXOS2 + cSTRANM.TRTAX2;
                            cSTRANMApp.TROS = cSTRANMApp.TROS + cSTRANM.TRAMT;
                            cSTRANMApp.TROS1 = cSTRANMApp.TROS1 + cSTRANM.TRAMT1;
                            cSTRANMApp.TROS2 = cSTRANMApp.TROS2 + cSTRANM.TRAMT2;

                            cSTRANMApp.COMPLETE = "N";
                            cSTRANMApp.COMPLETED = new DateTime(3000, 01, 01);
                            cSTRANMApp.REFCNT = cSTRANMApp.REFCNT - 1;

                            db.Entry(cSTRANMApp).State = EntityState.Modified;
                        }
                        return DeleteItem(sourceno, id);
                        //return CSTRANMAllocated(MyHtmlHelpers.ConvertIdToByteStr(sourceno));
                    }
                }
                catch (Exception e)
                {
                    // catch whatever error that is
                    Response.StatusCode = (int)HttpStatusCode.NotAcceptable;
                    Response.StatusDescription = e.Message;
                }
                //return new RedirectResult(Url.Action("Edit", "CSRCPs", new { id = MyHtmlHelpers.ConvertIdToByteStr(cSTRANM.SOURCENO) }) + "#Advance");
            }

            // if everything else fails
            string sid = MyHtmlHelpers.ConvertIdToByteStr(sourceno);
            return CSTRANMAllocated(sid);
        }

        public PartialViewResult DeleteItem(string sourceno, int id)
        {
            //string sid = MyHtmlHelpers.ConvertByteStrToId(sourceno);

            CSTRANM cSTRANM = db.CSTRANMs.Find("CSRCP", sourceno, id);
            CSLDG cSLDG = db.CSLDGs.Find("CSRCP", sourceno, id);
            CSTRAND cSTRAND = db.CSTRANDs.Find("CSRCP", sourceno, id);

            bool isAdvance = false;
            string cono = "";

            if (cSTRANM != null)
            {
                cono = cSTRANM.CONO;
                db.CSTRANMs.Remove(cSTRANM);
            }

            if (cSTRAND != null)
            {
                db.CSTRANDs.Remove(cSTRAND);
            }

            if (cSLDG != null)
            {
                if (cono == string.Empty)
                {
                    cono = cSLDG.CONO;
                }
                db.CSLDGs.Remove(cSLDG);
            }

            if ((cSLDG != null) || (cSTRANM != null) || (cSTRAND != null))
            {
                db.SaveChanges();
            }

            string sid = MyHtmlHelpers.ConvertIdToByteStr(sourceno);
            cono = MyHtmlHelpers.ConvertIdToByteStr(cono);
            if (isAdvance)
            {
                return CSTRANMAllocated(sid);
            }
            else
            {
                return CSTRANMOpen(sid, cono);
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateAdvance([Bind(Include = "SOURCE,SOURCENO,SOURCEID,CONO,JOBNO,CASENO,CASECODE,DUEDATE,ENTDATE,TRTYPE,TRDESC,TRITEM1,TRITEM2,TRITEM,TRTAX1,TRTAX2,TRTAX,TRAMT1,TRAMT2,TRAMT,TRSIGN,TRSITEM1,TRSITEM2,TRSITEM,TRSTAX1,TRSTAX2,TRSTAX,TRSAMT1,TRSAMT2,TRSAMT,TRITEMOS1,TRITEMOS2,TRITEMOS,TRTAXOS1,TRTAXOS2,TRTAXOS,TROS1,TROS2,TROS,APPTYPE,APPNO,APPID,APPITEM1,APPITEM2,APPITEM,APPTAX1,APPTAX2,APPTAX,APPAMT1,APPAMT2,APPAMT,REM,COMPLETE,COMPLETED,SEQNO,REFCNT,STAMP")] CSTRANM cSTRANM)
        {


            if (ModelState.IsValid)
            {
                if (cSTRANM.SOURCEID == 0)
                {
                    var cSTRANMs = db.CSTRANMs.Where(x => x.SOURCENO == cSTRANM.SOURCENO && x.SOURCE == "CSRCP");

                    int maxId = 0;
                    if (cSTRANMs.Count() != 0)
                    {
                        maxId = cSTRANMs.Max(y => y.SOURCEID);
                    }
                    cSTRANM.SOURCEID = maxId + 1;
                    db.CSTRANMs.Add(cSTRANM);
                }
                else
                {
                    db.Entry(cSTRANM).State = EntityState.Modified;
                }

                cSTRANM.TRSITEM = -cSTRANM.TRITEM;
                cSTRANM.TRSITEM1 = -cSTRANM.TRITEM1;
                cSTRANM.TRSITEM2 = -cSTRANM.TRITEM2;
                cSTRANM.TRSTAX = -cSTRANM.TRTAX;
                cSTRANM.TRSTAX1 = -cSTRANM.TRTAX1;
                cSTRANM.TRSTAX2 = -cSTRANM.TRTAX2;
                cSTRANM.TRSAMT = -cSTRANM.TRAMT;
                cSTRANM.TRSAMT1 = -cSTRANM.TRAMT1;
                cSTRANM.TRSAMT2 = -cSTRANM.TRAMT2;

                cSTRANM.TRITEMOS = -cSTRANM.TRITEM;
                cSTRANM.TRITEMOS1 = -cSTRANM.TRITEM1;
                cSTRANM.TRITEMOS2 = -cSTRANM.TRITEM2;
                cSTRANM.TRTAXOS = -cSTRANM.TRTAX;
                cSTRANM.TRTAXOS1 = -cSTRANM.TRTAX1;
                cSTRANM.TRTAXOS2 = -cSTRANM.TRTAX2;
                cSTRANM.TROS = -cSTRANM.TRAMT;
                cSTRANM.TROS1 = -cSTRANM.TRAMT1;
                cSTRANM.TROS2 = -cSTRANM.TRAMT2;

                cSTRANM.STAMP = cSTRANM.STAMP + 1;

                try
                {
                    CSLDG cSLDG = db.CSLDGs.Find("CSRCP", cSTRANM.SOURCENO, cSTRANM.SOURCEID);
                    if (cSLDG == null)
                    {
                        cSLDG = new CSLDG();
                        cSLDG.SOURCE = "CSRCP";
                        cSLDG.SOURCENO = cSTRANM.SOURCENO;
                        cSLDG.SOURCEID = cSTRANM.SOURCEID;
                        cSLDG.STAMP = 0;
                        db.CSLDGs.Add(cSLDG);
                    }
                    else
                    {
                        db.Entry(cSLDG).State = EntityState.Modified;
                    }


                    cSLDG.CONO = cSTRANM.CONO;
                    cSLDG.ENTDATE = cSTRANM.ENTDATE;
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
                    cSLDG.ADVANCE = cSTRANM.TRAMT;
                    cSLDG.SEQNO = cSTRANM.SEQNO;
                    cSLDG.REIMB1 = 0;
                    cSLDG.REIMB2 = 0;
                    cSLDG.REIMBREC1 = 0;
                    cSLDG.REIMBREC2 = 0;
                    cSLDG.STAMP = cSLDG.STAMP + 1;


                    db.SaveChanges();

                    int page = (int)Session["CSRCPPage"];
                    return RedirectToAction("Edit", new { id = MyHtmlHelpers.ConvertIdToByteStr(cSTRANM.SOURCENO), page = page });
                }
                catch (Exception e) { ModelState.AddModelError(string.Empty, e.Message); }
                finally { }
            }
            return PartialView("Partial/CSTRANMAdvance", cSTRANM);
        }


        public ActionResult Listing()
        {
            SACOMP profileRec = db.SACOMPs.FirstOrDefault();
            ViewBag.Company = profileRec.CONAME.ToUpper() + " (" + profileRec.COREGNO + ") ";
            return View(CurrentSelection());

        }

        public ActionResult ListingC()
        {
            SACOMP profileRec = db.SACOMPs.FirstOrDefault();
            ViewBag.Company = profileRec.CONAME.ToUpper() + " (" + profileRec.COREGNO + ") ";
            return View(CurrentSelectionC());

        }

        public ActionResult ReupdateLedger()
        {
            IQueryable<CSRCP> cSRCPs = CurrentSelection();
            foreach (CSRCP cSRCP in cSRCPs)
            {
                IQueryable<CSTRANM> cSTRANMs = db.CSTRANMs.Where(x => x.SOURCE == "CSRCP" && x.SOURCENO == cSRCP.TRNO);

                foreach (CSTRANM cSTRANMRCP in cSTRANMs)
                {

                    CSLDG cSLDG = db.CSLDGs.Find(cSTRANMRCP.SOURCE, cSTRANMRCP.SOURCENO, cSTRANMRCP.SOURCEID);

                    if (cSLDG == null)
                    {
                        cSLDG = new CSLDG();
                        db.CSLDGs.Add(cSLDG);
                    }
                    else
                    {
                        db.Entry(cSLDG).State = EntityState.Modified;
                    }
                    cSLDG.SOURCE = cSTRANMRCP.SOURCE;
                    cSLDG.SOURCENO = cSTRANMRCP.SOURCENO;
                    cSLDG.SOURCEID = cSTRANMRCP.SOURCEID;
                    cSLDG.STAMP = 0;

                    cSLDG.CONO = cSTRANMRCP.CONO;
                    //cSLDG.ENTDATE = cSTRANMRCP.ENTDATE;
                    cSLDG.ENTDATE = cSRCP.VDATE;
                    cSLDG.JOBNO = cSTRANMRCP.JOBNO;
                    cSLDG.CASENO = cSTRANMRCP.CASENO;
                    cSLDG.CASECODE = cSTRANMRCP.CASECODE;

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
                    cSLDG.SEQNO = cSTRANMRCP.SEQNO;
                    cSLDG.REIMB1 = 0;
                    cSLDG.REIMB2 = 0;
                    cSLDG.REIMBREC1 = 0;
                    cSLDG.REIMBREC2 = 0;

                    if (cSTRANMRCP.TRTYPE == "Fee")
                    {
                        cSLDG.FEE1 = cSTRANMRCP.TRITEM1;
                        cSLDG.FEE2 = cSTRANMRCP.TRITEM2;
                        cSLDG.TAX1 = cSTRANMRCP.TRTAX1;
                        cSLDG.TAX2 = cSTRANMRCP.TRTAX2;
                    }

                    if (cSTRANMRCP.TRTYPE == "Work")
                    {
                        cSLDG.WORK1 = cSTRANMRCP.TRITEM1;
                        cSLDG.WORK2 = cSTRANMRCP.TRITEM2;
                        cSLDG.TAX1 = cSTRANMRCP.TRTAX1;
                        cSLDG.TAX2 = cSTRANMRCP.TRTAX2;
                    }


                    if (cSTRANMRCP.TRTYPE == "Disbursement")
                    {
                        cSLDG.DISB1 = cSTRANMRCP.TRITEM1;
                        cSLDG.DISB2 = cSTRANMRCP.TRITEM2;
                        cSLDG.TAX1 = cSTRANMRCP.TRTAX1;
                        cSLDG.TAX2 = cSTRANMRCP.TRTAX2;
                    }

                    if (cSTRANMRCP.TRTYPE == "Reimbursement")
                    {
                        cSLDG.REIMB1 = cSTRANMRCP.TRITEM1;
                        cSLDG.REIMB2 = cSTRANMRCP.TRITEM2;
                        cSLDG.TAX1 = cSTRANMRCP.TRTAX1;
                        cSLDG.TAX2 = cSTRANMRCP.TRTAX2;
                    }
                    if (cSTRANMRCP.TRTYPE == "Advance")
                    {
                        cSLDG.ADVANCE = cSTRANMRCP.TRAMT;

                    }
                    if (cSTRANMRCP.TRTYPE == "Deposit")
                    {
                        cSLDG.DEP = cSTRANMRCP.TRAMT;
                    }



                  
                }
             
            }
            db.SaveChanges();
            return View("Index", cSRCPs.ToList().ToPagedList(1,30));
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
