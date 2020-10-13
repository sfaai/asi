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
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace WebApplication1.Controllers
{
  

   [Authorize(Roles = "Administrator,CS-A/C,CS-SEC,CS-AS")]
    public class CSPRFsController : BaseController
    {
        private ASIDBConnection db = new ASIDBConnection();    

         public async Task<ActionResult> Email(string id)
        {
            string sid = MyHtmlHelpers.ConvertByteStrToId(id);
            CSPRF cSPRF = getProforma(sid);


            string html = RenderViewToString(ControllerContext, "~/views/CSPRFs/Proforma.cshtml", cSPRF, true);

            EmailFormModel model = new WebApplication1.EmailFormModel();
            model.Message = html;
            model.FromEmail = "reminder@asi.my";
            model.FromName = "ASI Secretary";

            var body = "<p>Email From: {0} ({1})</p><p>Message:</p><p>{2}</p>";
            var message = new MailMessage();
            message.To.Add(new MailAddress(cSPRF.CSCOMSTR.WEB)); //replace with valid value
            message.Subject = "Proforma Billing " + cSPRF.PRFNO;
            message.Body = string.Format(body, model.FromName, model.FromEmail, model.Message);
            message.IsBodyHtml = true;
            using (var smtp = new SmtpClient())
            {
                await smtp.SendMailAsync(message);
                return RedirectToAction("Sent", "Home");
            }


            //var credential = new NetworkCredential
            //{
            //    UserName = "user@outlook.com",  // replace with valid value
            //    Password = "password"  // replace with valid value
            //};
            //smtp.Credentials = credential;
            //smtp.Host = "smtp-mail.outlook.com";
            //smtp.Port = 587;
            //smtp.EnableSsl = true;
            //await smtp.SendMailAsync(message);
            //return RedirectToAction("Sent");




            return View("Proforma", cSPRF);
        }

        public PartialViewResult Search()
        {
            CSPRF searchRec = null;
            //searchRec.CONO = pSearchCode;
            //searchRec.CONAME = pSearchName;
            if (Session["SearchPRFRec"] != null)
            {
                searchRec = (CSPRF)Session["SearchPRFRec"];

            }
            else
            {
                searchRec = new CSPRF();            
                searchRec.VDATE = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                searchRec.DUEDATE = searchRec.VDATE.AddMonths(1);
                searchRec.DUEDATE = searchRec.DUEDATE.AddDays(-1);
            }
            if (Session["SearchPRFSort"] == null)
            {
                Session["SearchPRFSort"] = "PRFNO";
            };

            List<SelectListItem> listItems = new List<SelectListItem>();
            listItems.Add(new SelectListItem
            {
                Text = "Company Name",
                Value = "CONAME",
                Selected = (string)Session["SearchPRFSort"] == "CONAME"
            });

            listItems.Add(new SelectListItem
            {
                Text = "proforma #",
                Value = "PRFNO",
                Selected = (string)Session["SearchPRFSort"] == "PRFNO"
            });

            listItems.Add(new SelectListItem
            {
                Text = "proforma # Latest",
                Value = "PRFNOLAST",
                Selected = (string)Session["SearchPRFSort"] == "PRFNOLAST"
            });

            listItems.Add(new SelectListItem
            {
                Text = "PRF Date",
                Value = "VDATE",
                Selected = (string)Session["SearchPRFSort"] == "VDATE"
            });

            listItems.Add(new SelectListItem
            {
                Text = "Archive",
                Value = "ARCHIVE",
                Selected = (string)Session["SearchPRFSort"] == "ARCHIVE"
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
        public ActionResult SearchPost(CSPRF cSPRF)
        {

            Session["SearchPRFRec"] = cSPRF;
            Session["SearchPRFSort"] = Request.Params["SORTBY"] ?? "VDATE";
            return Redirect("?page=1");
            //return Index(1);
        }
        // GET: CSPRFs
        public ActionResult Index(int? page)
        {
            ViewBag.page = page ?? 1;
            return View("Index", CurrentSelection().ToList().ToPagedList(page ?? 1, 30));
        }

        public IQueryable<CSPRF> CurrentSelection()
        {
            string pSearchCode = "";
            string pSearchName = "";
            string pSearchPRF = "";
            DateTime pSearchVdate = DateTime.Parse("01/01/0001");
            DateTime pSearchDdate = DateTime.Parse("01/01/0001");

            if (Session["SearchPRFRec"] != null)
            {
                CSPRF searchRec = (CSPRF)(Session["SearchPRFRec"]);
                pSearchCode = searchRec.CSCOMSTR.COREGNO ?? "";
                pSearchName = searchRec.CSCOMSTR.CONAME ?? "";
                pSearchVdate = searchRec.VDATE;
                pSearchDdate = searchRec.DUEDATE;
                pSearchPRF = searchRec.PRFNO ?? "";

            }
            else
            { // start with current month proforma bills instead of entire list
                pSearchVdate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                pSearchDdate = pSearchVdate.AddMonths(1);
                pSearchDdate = pSearchDdate.AddDays(-1);
            }

            IQueryable<CSPRF> cSPRFs = db.CSPRFs;

            if ((string)Session["SearchPRFSort"] != "ARCHIVE") { cSPRFs = db.CSPRFs.Where(x => x.INVALLOC == "N"); }
            else { cSPRFs = db.CSPRFs.Where(x => x.INVALLOC == "Y"); }

            if (pSearchCode != "") { cSPRFs = cSPRFs.Where(x => x.CSCOMSTR.COREGNO.Contains(pSearchCode.ToUpper())); };
            if (pSearchName != "") { cSPRFs = cSPRFs.Where(x => x.CSCOMSTR.CONAME.Contains(pSearchName.ToUpper())); };
            if (pSearchVdate != DateTime.Parse("01/01/0001")) { cSPRFs = cSPRFs.Where(x => x.VDATE >= pSearchVdate); };
            if (pSearchDdate != DateTime.Parse("01/01/0001")) { cSPRFs = cSPRFs.Where(x => x.VDATE <= pSearchDdate); };
            if (pSearchPRF != "")
            {
                if (pSearchPRF.Length > 8)
                {
                    cSPRFs = cSPRFs.Where(x => x.PRFNO == pSearchPRF);
                }
                else
                {
                    cSPRFs = cSPRFs.Where(x => x.PRFNO.Contains(pSearchPRF));
                }
            };
            cSPRFs = cSPRFs.Include(d => d.CSCOADDR).Include(e => e.CSCOMSTR);

            if ((string)Session["SearchPRFSort"] == "CONAME")
            {
                cSPRFs = cSPRFs.OrderBy(n => n.CSCOMSTR.CONAME);
            }
            else if ((string)Session["SearchPRFSort"] == "VDATE")
            {
                cSPRFs = cSPRFs.OrderBy(n => n.VDATE);

            }
            else if ((string)Session["SearchPRFSort"] == "PRFNOLAST")
            {
                cSPRFs = cSPRFs.OrderByDescending(n => n.PRFNO);

            }
            else
            {
                cSPRFs = cSPRFs.OrderBy(n => n.PRFNO);
            }
            DateTime rptStart = pSearchVdate;
            DateTime rptEnd = pSearchDdate;

            ViewBag.RPT_START = rptStart.ToString("dd/MM/yyyy");
            ViewBag.RPT_END = rptEnd.ToString("dd/MM/yyyy");

            return cSPRFs;
        }

        public PartialViewResult BillAllocated(string id)
        {
            string sid = MyHtmlHelpers.ConvertByteStrToId(id);
            var cSBILLs = db.CSBILLs.Where(x => x.PRFNO == sid).OrderBy(y => y.PRFID);
            ViewBag.PRFNO = sid;
            ViewBag.PRFNOB = id;
            return PartialView("Partial/BillAllocated", cSBILLs);
        }

        public PartialViewResult BillOpen(string id, string prfno)
        {
            string sid = MyHtmlHelpers.ConvertByteStrToId(id);
            var cSBILLs = db.CSBILLs.Where(x => x.CONO == sid && x.PRFNO == null);
            ViewBag.PRFNO = MyHtmlHelpers.ConvertByteStrToId(prfno);
            ViewBag.PRFNOB = prfno;
            return PartialView("Partial/BillOpen", cSBILLs);
        }

        public ActionResult ProformaList(int? page)
        {

            if (page > 0)
            {
                int prevPage = (page ?? 1) - 1;
                return View(CurrentSelection().Skip(prevPage * 30).Take(30).ToList());
            }

            return View(CurrentSelection().ToList());
        }


        public ActionResult Proforma(string id)
        {
            string sid = MyHtmlHelpers.ConvertByteStrToId(id);
            CSPRF cSPRF = getProforma(sid);
            //string html = RenderViewToString(ControllerContext, "~/views/CSPRFs/Proforma.cshtml", cSPRF, true);

            //EmailFormModel model = new WebApplication1.EmailFormModel();
            //model.Message = html;
            //model.FromEmail = "sfaai@sphinxsd.com";
            //model.FromName = "my tester";

            //var body = "<p>Email From: {0} ({1})</p><p>Message:</p><p>{2}</p>";
            //var message = new MailMessage();
            //message.To.Add(new MailAddress("sfaai1969@gmail.com")); //replace with valid value
            //message.Subject = "Your email subject";
            //message.Body = string.Format(body, model.FromName, model.FromEmail, model.Message);
            //message.IsBodyHtml = true;
            //using (var smtp = new SmtpClient())
            //{
            //    smtp.Send(message);
            //   
            //}

            return View(cSPRF);
        }

        public PartialViewResult ProformaM(string id)
        {
            string sid = MyHtmlHelpers.ConvertByteStrToId(id);
            return PartialView("Proforma", getProforma(sid));
        }

        public CSPRF getProforma(string sid)
        {
            CSPRF cSPRF = db.CSPRFs.Find(sid);

            SACOMP profileRec = db.SACOMPs.FirstOrDefault();
            ViewBag.Company = profileRec.CONAME.ToUpper() + " (" + profileRec.COREGNO + ") ";
            ViewBag.Address = profileRec.COADDR1 + " " + profileRec.COADDR2 + " " + profileRec.COADDR3 + " " + profileRec.COADDR4;
            ViewBag.Contact = "Tel: " + profileRec.COPHONE1 + " Fax: " + profileRec.COFAX1 + " E-Mail: " + profileRec.COWEB;

            var cSBILL = db.CSBILLs.Where(x => x.PRFNO == sid && x.JOBNO != "" && x.JOBNO != null).FirstOrDefault();
            if (cSBILL != null)
            {
                var cSJOB = db.CSJOBMs.Where(x => x.JOBNO == cSBILL.JOBNO).FirstOrDefault();
                ViewBag.StaffName = cSJOB.HKSTAFF.STAFFDESC;
            }
            else
            {
                ViewBag.StaffName = cSPRF.CSCOMSTR.HKSTAFF.STAFFDESC; // default to staff assigned to company
                //ViewBag.StaffName = "No Staff assigned";
            }
            ViewBag.Addr1 = "";
            ViewBag.Addr2 = "";
            if ((cSPRF.CSCOADDR != null) && (cSPRF.CSCOADDR.HKCITY != null))
            {
                ViewBag.Addr1 = cSPRF.CSCOADDR.ADDR1;
                ViewBag.Addr2 = cSPRF.CSCOADDR.ADDR2;
                ViewBag.Addr3 = cSPRF.CSCOADDR.ADDR3 + " " + cSPRF.CSCOADDR.HKCITY.CITYDESC + " " + cSPRF.CSCOADDR.POSTAL;
            }
            else
            {
                if (cSPRF.CSCOADDR != null)
                {
                    ViewBag.Addr1 = cSPRF.CSCOADDR.ADDR1;
                    ViewBag.Addr2 = cSPRF.CSCOADDR.ADDR2;
                    ViewBag.Addr3 = cSPRF.CSCOADDR.ADDR3 + " " + cSPRF.CSCOADDR.POSTAL;
                }
                else
                {
                    ViewBag.Addr3 = "";
                }
            }
            if ((cSPRF.ATTN == null) || (cSPRF.ATTN == string.Empty)) { cSPRF.ATTN = "The Board of Directors"; }

            decimal totalItem = 0;
            decimal totalTax = 0;
            decimal reimbItem = 0;
            decimal reimbTax = 0;

            int linecnt = 0;

            ViewBag.TaxCode = "";
            ViewBag.TaxRate = "";
            ViewBag.RTaxCode = "";
            ViewBag.RTaxRate = "";
            ViewBag.CSBILL_fee = db.CSBILLs.Where(x => x.PRFNO == sid && x.ITEMTYPE == "Fee").OrderBy(y => y.PRFID).ToList();
            foreach (CSBILL rec in ViewBag.CSBILL_fee)
            {
                totalItem = totalItem + rec.ITEMAMT;
                totalTax = totalTax + rec.TAXAMT;
                if (rec.TAXTYPE != "") { ViewBag.TaxCode = rec.TAXTYPE; }
                if (rec.TAXRATE != 0) { ViewBag.TaxRate = rec.TAXRATE.ToString("N2") + "%"; }
                linecnt++;
            }

            ViewBag.CSBILL_work = db.CSBILLs.Where(x => x.PRFNO == sid && x.ITEMTYPE == "Work").OrderBy(y => y.PRFID).ToList();
            foreach (CSBILL rec in ViewBag.CSBILL_work)
            {
                totalItem = totalItem + rec.ITEMAMT;
                totalTax = totalTax + rec.TAXAMT;
                if (rec.TAXTYPE != "") { ViewBag.TaxCode = rec.TAXTYPE; }
                if (rec.TAXRATE != 0) { ViewBag.TaxRate = rec.TAXRATE.ToString("N2") + "%"; }
                linecnt++;
            }
            ViewBag.CSBILL_disbursement = db.CSBILLs.Where(x => x.PRFNO == sid && x.ITEMTYPE == "Disbursement").OrderBy(y => y.PRFID).ToList();
            foreach (CSBILL rec in ViewBag.CSBILL_disbursement)
            {
                totalItem = totalItem + rec.ITEMAMT;
                totalTax = totalTax + rec.TAXAMT;
                if (rec.TAXTYPE != "") { ViewBag.TaxCode = rec.TAXTYPE; }
                if (rec.TAXRATE != 0) { ViewBag.TaxRate = rec.TAXRATE.ToString("N2") + "%"; }
                linecnt++;
            }
            ViewBag.CSBILL_reimbursement = db.CSBILLs.Where(x => x.PRFNO == sid && x.ITEMTYPE == "Reimbursement").OrderBy(y => y.PRFID).ToList();
            foreach (CSBILL rec in ViewBag.CSBILL_reimbursement)
            {
                reimbItem = reimbItem + rec.ITEMAMT;
                reimbTax = reimbTax + rec.TAXAMT;
                if (rec.TAXTYPE != "") { ViewBag.RTaxCode = rec.TAXTYPE; }
                if (rec.TAXRATE != 0) { ViewBag.RTaxRate = rec.TAXRATE.ToString("N2") + "%"; }
                linecnt++;
            }
            ViewBag.totalItem = totalItem.ToString("N2");
            ViewBag.totalTax = totalTax.ToString("N2");
            ViewBag.TotalAmt = (totalItem + totalTax).ToString("N2");
            ViewBag.reimbItem = reimbItem.ToString("N2");
            ViewBag.reimbTax = reimbTax.ToString("N2");
            ViewBag.reimbAmt = (reimbItem + reimbTax).ToString("N2");
            ViewBag.payable = (totalItem + totalTax + reimbItem + reimbTax).ToString("N2");
            ViewBag.linecnt = linecnt;
            return cSPRF;
        }



        // GET: CSPRFs/Details/5
        public ActionResult Details(string id, int? page)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSPRF cSPRF = db.CSPRFs.Find(MyHtmlHelpers.ConvertByteStrToId(id));
            if (cSPRF == null)
            {
                return HttpNotFound();
            }
            ViewBag.page = page ?? 1;
            Session["CSPRFPage"] = ViewBag.page;

            ViewBag.CONO = new SelectList(db.CSCOMSTRs.Select(x => new { CONO = x.CONO, CONAME = x.CONAME + "  (" + x.CONO + ")" }).OrderBy(y => y.CONAME), "CONO", "CONAME", cSPRF.CONO);
            ViewBag.ATTNDESC = new SelectList(db.CSCOPICs.Where(x => x.CONO == cSPRF.CONO).Select(x => new { PRSNAME = x.CSPR.PRSNAME, PRSDESC = x.CSPR.PRSNAME }).OrderBy(y => y.PRSDESC), "PRSNAME", "PRSDESC", cSPRF.ATTN);
            ViewBag.COADDR = new SelectList(db.CSCOADDRs.Where(x => x.CONO == cSPRF.CONO).Select(x => new { COADDR = x.CONO + "|" + x.ADDRID, COADDRDESC = x.ADDRTYPE + " | " + x.MAILADDR + " | " + x.ADDR1 + " " + x.ADDR2 + " " + x.ADDR3 }), "COADDR", "COADDRDESC", cSPRF.COADDR);
            ViewBag.DETAILS = db.CSBILLs.Where(x => x.PRFNO == cSPRF.PRFNO);
            ViewBag.CONAME = db.CSCOMSTRs.Where(x => x.CONO == cSPRF.CONO).Select(y => y.CONAME).FirstOrDefault();
            ViewBag.Title = "Edit Proforma Bill " + cSPRF.PRFNO;
            ViewBag.PRFNO = cSPRF.PRFNO;

            ViewBag.Title = "View Proforma Bill " + cSPRF.PRFNO;
            return View("Edit", cSPRF);
        }

        // GET: CSPRFs/Create
        public ActionResult Create()
        {
            CSPRF cSPRF = new CSPRF();
            cSPRF.VDATE = DateTime.Today;
            cSPRF.DUEDAYS = 0;
            cSPRF.INVALLOC = "N";
            cSPRF.DUEDATE = cSPRF.VDATE;
            cSPRF.CONO = "750059-M"; // hack to show first company jobs

            ViewBag.page = 1;
            Session["CSPRFPage"] = ViewBag.page;
            ViewBag.CONO = new SelectList(db.CSCOMSTRs.Select(x => new { CONO = x.CONO, CONAME = x.CONAME + "  (" + x.CONO + ")" }).OrderBy(y => y.CONAME), "CONO", "CONAME", cSPRF.CONO);
            ViewBag.ATTNDESC = new SelectList(db.CSCOPICs.Where(x => x.CONO == cSPRF.CONO).Select(x => new { PRSNAME = x.CSPR.PRSNAME, PRSDESC = x.CSPR.PRSNAME }).OrderBy(y => y.PRSDESC), "PRSNAME", "PRSDESC", cSPRF.ATTN);
            ViewBag.COADDR = new SelectList(db.CSCOADDRs.Where(x => x.CONO == cSPRF.CONO).Select(x => new { COADDR = x.CONO + "|" + x.ADDRID, COADDRDESC = x.ADDRTYPE + " | " + x.MAILADDR + " | " + x.ADDR1 + " " + x.ADDR2 + " " + x.ADDR3 }), "COADDR", "COADDRDESC", cSPRF.COADDR);

            ViewBag.Title = "Create Proforma Bill";
            return View("Edit", cSPRF);
        }

        // POST: CSPRFs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PRFNO,VDATE,DUEDAYS,DUEDATE,CONO,COADDR,COADDRID,ATTN,REM,SEQNO,INVALLOC,INVNO,INVID,STAMP")] CSPRF cSPRF)
        {

            if (ModelState.IsValid)
            {

                SALASTNO serialTbl = db.SALASTNOes.Find("CSPRF");
                if (serialTbl != null)
                {
                    CSCOMSTR cSCOMSTR = db.CSCOMSTRs.Find(cSPRF.CONO);
                    if (cSCOMSTR != null)
                    {
                        try
                        {
                            string prefix = serialTbl.LASTPFIX;
                            int MaxNo = serialTbl.LASTNOMAX;
                            bool AutoGen = serialTbl.AUTOGEN == "Y";
                            serialTbl.LASTNO = serialTbl.LASTNO + 1;
                            cSPRF.PRFNO = serialTbl.LASTNO.ToString("D10");
                            cSPRF.STAMP = 1;

                            // increment company seqno count before using it in transaction
                            cSCOMSTR.SEQNO = cSCOMSTR.SEQNO + 1;
                            cSCOMSTR.STAMP = cSCOMSTR.STAMP + 1;

                            cSPRF.SEQNO = cSCOMSTR.SEQNO;
                            db.Entry(cSCOMSTR).State = EntityState.Modified;

                            serialTbl.STAMP = serialTbl.STAMP + 1;
                            db.Entry(serialTbl).State = EntityState.Modified;
                            db.CSPRFs.Add(cSPRF);
                            db.SaveChanges();

                            //return Edit(MyHtmlHelpers.ConvertIdToByteStr(cSPRF.PRFNO), 1);
                            return RedirectToAction("Edit", new { id = MyHtmlHelpers.ConvertIdToByteStr(cSPRF.PRFNO), page = 1 });
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
            ViewBag.CONO = new SelectList(db.CSCOMSTRs.Select(x => new { CONO = x.CONO, CONAME = x.CONAME + "  (" + x.CONO + ")" }).OrderBy(y => y.CONAME), "CONO", "CONAME", cSPRF.CONO);
            ViewBag.ATTNDESC = new SelectList(db.CSCOPICs.Where(x => x.CONO == cSPRF.CONO).Select(x => new { PRSNAME = x.CSPR.PRSNAME, PRSDESC = x.CSPR.PRSNAME }).OrderBy(y => y.PRSDESC), "PRSNAME", "PRSDESC", cSPRF.ATTN);
            ViewBag.COADDR = new SelectList(db.CSCOADDRs.Where(x => x.CONO == cSPRF.CONO).Select(x => new { COADDR = x.CONO + "|" + x.ADDRID, COADDRDESC = x.ADDRTYPE + " | " + x.MAILADDR + " | " + x.ADDR1 + " " + x.ADDR2 + " " + x.ADDR3 }), "COADDR", "COADDRDESC", cSPRF.COADDR);

            ViewBag.Title = "Create Proforma Bill";
            return View("Edit", cSPRF);
        }

        public ActionResult EditCompany(CSPRF cSPRF)
        {

            //ModelState.Clear(); //apparent this has side-effects and using Remove is preferrable
            ModelState.Remove("CONO");
            ModelState.Remove("ATTN");
            ModelState.Remove("COADDR");

            cSPRF.COADDR = null;
            cSPRF.ATTN = null;

            //ViewBag.ATTNDESC = new SelectList(db.CSCOPICs.Where(x => x.CONO == cSPRF.CONO).Select(x => new { PRSCODE = x.PRSCODE, PRSDESC= x.CSPR.PRSNAME + " | " + x.DESIG }).OrderBy(y => y.PRSDESC), "PRSCODE", "PRSDESC", cSPRF.ATTN);
            ViewBag.ATTNDESC = new SelectList(db.CSCOPICs.Where(x => x.CONO == cSPRF.CONO).Select(x => new { PRSNAME = x.CSPR.PRSNAME, PRSDESC = x.CSPR.PRSNAME }).OrderBy(y => y.PRSDESC), "PRSNAME", "PRSDESC", cSPRF.ATTN);
            ViewBag.COADDR = new SelectList(db.CSCOADDRs.Where(x => x.CONO == cSPRF.CONO).Select(x => new { COADDR = x.CONO + "|" + x.ADDRID, COADDRDESC = x.ADDRTYPE + " | " + x.MAILADDR + " | " + x.ADDR1 + " " + x.ADDR2 + " " + x.ADDR3 }), "COADDR", "COADDRDESC", cSPRF.COADDR);
            return PartialView("Partial/EditCompany", cSPRF);


        }

        // GET: CSPRFs/Edit/5
        public ActionResult Edit(string id, int? page)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSPRF cSPRF = db.CSPRFs.Find(MyHtmlHelpers.ConvertByteStrToId(id));
            if (cSPRF == null)
            {
                return HttpNotFound();
            }

            ViewBag.page = page ?? 1;
            Session["CSPRFPage"] = ViewBag.page;

            ViewBag.CONO = new SelectList(db.CSCOMSTRs.Select(x => new { CONO = x.CONO, CONAME = x.CONAME + "  (" + x.CONO + ")" }).OrderBy(y => y.CONAME), "CONO", "CONAME", cSPRF.CONO);
            ViewBag.ATTNDESC = new SelectList(db.CSCOPICs.Where(x => x.CONO == cSPRF.CONO).Select(x => new { PRSNAME = x.CSPR.PRSNAME, PRSDESC = x.CSPR.PRSNAME }).OrderBy(y => y.PRSDESC), "PRSNAME", "PRSDESC", cSPRF.ATTN);
            ViewBag.COADDR = new SelectList(db.CSCOADDRs.Where(x => x.CONO == cSPRF.CONO).Select(x => new { COADDR = x.CONO + "|" + x.ADDRID, COADDRDESC = x.ADDRTYPE + " | " + x.MAILADDR + " | " + x.ADDR1 + " " + x.ADDR2 + " " + x.ADDR3 }), "COADDR", "COADDRDESC", cSPRF.COADDR);
            ViewBag.DETAILS = db.CSBILLs.Where(x => x.PRFNO == cSPRF.PRFNO);
            ViewBag.CONAME = db.CSCOMSTRs.Where(x => x.CONO == cSPRF.CONO).Select(y => y.CONAME).FirstOrDefault();
            ViewBag.Title = "Edit Proforma Bill " + cSPRF.PRFNO;
            ViewBag.PRFNO = cSPRF.PRFNO;
            return View("Edit", cSPRF);
        }

        // POST: CSPRFs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PRFNO,VDATE,DUEDAYS,DUEDATE,CONO,COADDR,COADDRID,ATTN,REM,SEQNO,INVALLOC,INVNO,INVID,STAMP")] CSPRF cSPRF)
        {
            if (ModelState.IsValid)
            {
                ASIDBConnection newdb = new ASIDBConnection();
                db.Entry(cSPRF).State = EntityState.Modified;
                try
                {

                    CSPRF curRec = newdb.CSPRFs.Find(cSPRF.PRFNO);
                    if (curRec.STAMP == cSPRF.STAMP)
                    {
                        cSPRF.STAMP = cSPRF.STAMP + 1;
                        db.SaveChanges();

                        int page = (int)Session["CSPRFPage"];
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
            ViewBag.CONO = new SelectList(db.CSCOMSTRs.Select(x => new { CONO = x.CONO, CONAME = x.CONAME + "  (" + x.CONO + ")" }).OrderBy(y => y.CONAME), "CONO", "CONAME", cSPRF.CONO);
            ViewBag.ATTNDESC = new SelectList(db.CSCOPICs.Where(x => x.CONO == cSPRF.CONO).Select(x => new { PRSNAME = x.CSPR.PRSNAME, PRSDESC = x.CSPR.PRSNAME }).OrderBy(y => y.PRSDESC), "PRSNAME", "PRSDESC", cSPRF.ATTN);
            ViewBag.COADDR = new SelectList(db.CSCOADDRs.Where(x => x.CONO == cSPRF.CONO).Select(x => new { COADDR = x.CONO + "|" + x.ADDRID, COADDRDESC = x.ADDRTYPE + " | " + x.MAILADDR + " | " + x.ADDR1 + " " + x.ADDR2 + " " + x.ADDR3 }), "COADDR", "COADDRDESC", cSPRF.COADDR);
            ViewBag.DETAILS = db.CSBILLs.Where(x => x.PRFNO == cSPRF.PRFNO);
            ViewBag.CONAME = db.CSCOMSTRs.Where(x => x.CONO == cSPRF.CONO).Select(y => y.CONAME).FirstOrDefault();
            ViewBag.Title = "Edit Proforma Bill " + cSPRF.PRFNO;
            return View(cSPRF);
        }

        // GET: CSPRFs/Delete/5
        public ActionResult Delete(string id, int? page)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSPRF cSPRF = db.CSPRFs.Find(MyHtmlHelpers.ConvertByteStrToId(id));
            if (cSPRF == null)
            {
                return HttpNotFound();
            }

            int refcnt = cSPRF.refcnt;
            if (refcnt > 0)
            {
                ModelState.AddModelError(string.Empty, refcnt.ToString() + " Details has been touched. Cannot Delete record");

            }
            ViewBag.page = page ?? 1;
            Session["CSPRFPage"] = ViewBag.page;

            ViewBag.CONO = new SelectList(db.CSCOMSTRs.Select(x => new { CONO = x.CONO, CONAME = x.CONAME + "  (" + x.CONO + ")" }).OrderBy(y => y.CONAME), "CONO", "CONAME", cSPRF.CONO);
            ViewBag.ATTNDESC = new SelectList(db.CSCOPICs.Where(x => x.CONO == cSPRF.CONO).Select(x => new { PRSNAME = x.CSPR.PRSNAME, PRSDESC = x.CSPR.PRSNAME }).OrderBy(y => y.PRSDESC), "PRSNAME", "PRSDESC", cSPRF.ATTN);
            ViewBag.COADDR = new SelectList(db.CSCOADDRs.Where(x => x.CONO == cSPRF.CONO).Select(x => new { COADDR = x.CONO + "|" + x.ADDRID, COADDRDESC = x.ADDRTYPE + " | " + x.MAILADDR + " | " + x.ADDR1 + " " + x.ADDR2 + " " + x.ADDR3 }), "COADDR", "COADDRDESC", cSPRF.COADDR);
            ViewBag.DETAILS = db.CSBILLs.Where(x => x.PRFNO == cSPRF.PRFNO);
            ViewBag.CONAME = db.CSCOMSTRs.Where(x => x.CONO == cSPRF.CONO).Select(y => y.CONAME).FirstOrDefault();
            ViewBag.Title = "Delete Proforma Bill " + cSPRF.PRFNO;
            return View("Edit", cSPRF);
        }

        // POST: CSPRFs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            CSPRF cSPRF = db.CSPRFs.Find(MyHtmlHelpers.ConvertByteStrToId(id));

            if (cSPRF != null)
            {
                int refcnt = cSPRF.refcnt;
                if (refcnt > 0)
                {
                    ModelState.AddModelError(string.Empty, refcnt.ToString() + " Details has been touched. Cannot Delete record");

                }
                else
                {
                    // Remove CSTRANM 
                    // Modify CSBILL to remove reference to PRFNO
                    try
                    {
                        List<CSBILL> ListBill = cSPRF.CSBILLs.ToList();
                        foreach (CSBILL item in ListBill)
                        {
                            CSTRANM cSTRANM = db.CSTRANMs.Find("CSPRF", item.PRFNO, item.PRFID);
                            db.CSTRANMs.Remove(cSTRANM);

                            CSBILL cSBill = db.CSBILLs.Find(item.BILLNO);
                            cSBill.STAMP = cSBill.STAMP + 1;
                            cSBill.PRFALLOC = "N";
                            cSBill.PRFNO = null;
                            cSBill.PRFID = null;
                            db.Entry(cSBill).State = EntityState.Modified;
                        }


                        db.CSPRFs.Remove(cSPRF);
                        db.SaveChanges();

                        int page = (int)Session["CSPRFPage"];
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
                    catch (Exception e) { ModelState.AddModelError(String.Empty, e.Message); }
                }
            }

            ViewBag.page = Session["CSPRFPage"];

            ViewBag.CONO = new SelectList(db.CSCOMSTRs.Select(x => new { CONO = x.CONO, CONAME = x.CONAME + "  (" + x.CONO + ")" }).OrderBy(y => y.CONAME), "CONO", "CONAME", cSPRF.CONO);
            ViewBag.ATTNDESC = new SelectList(db.CSCOPICs.Where(x => x.CONO == cSPRF.CONO).Select(x => new { PRSNAME = x.CSPR.PRSNAME, PRSDESC = x.CSPR.PRSNAME }).OrderBy(y => y.PRSDESC), "PRSNAME", "PRSDESC", cSPRF.ATTN);
            ViewBag.COADDR = new SelectList(db.CSCOADDRs.Where(x => x.CONO == cSPRF.CONO).Select(x => new { COADDR = x.CONO + "|" + x.ADDRID, COADDRDESC = x.ADDRTYPE + " | " + x.MAILADDR + " | " + x.ADDR1 + " " + x.ADDR2 + " " + x.ADDR3 }), "COADDR", "COADDRDESC", cSPRF.COADDR);
            ViewBag.DETAILS = db.CSBILLs.Where(x => x.PRFNO == cSPRF.PRFNO);
            ViewBag.CONAME = db.CSCOMSTRs.Where(x => x.CONO == cSPRF.CONO).Select(y => y.CONAME).FirstOrDefault();
            ViewBag.Title = "Delete Proforma Bill " + cSPRF.PRFNO;
            return View("Edit", cSPRF);
        }

        public PartialViewResult RemoveItem(string id)
        {

            string sid = MyHtmlHelpers.ConvertByteStrToId(id);
            CSBILL cSBILL = db.CSBILLs.Find(sid);
            string prfno = cSBILL.PRFNO;
            int? prfId = cSBILL.PRFID;
            string prfnoId = MyHtmlHelpers.ConvertIdToByteStr(prfno);

            if (cSBILL == null)
            {
                Response.StatusCode = (int)HttpStatusCode.NotAcceptable;
                Response.StatusDescription = "Bill Item is missing";
            }
            else if (cSBILL.PRFALLOC == "N")
            {
                Response.StatusCode = (int)HttpStatusCode.NotAcceptable;
                Response.StatusDescription = "Bill has been Allocated";
            }
            else if (cSBILL.refcnt > 0)
            {

                Response.StatusCode = (int)HttpStatusCode.NotAcceptable;
                Response.StatusDescription = "Bill has been touched";
                //throw new Exception("Bill has been touched");
            }
            else
            {


                ASIDBConnection newdb = new ASIDBConnection();
                try
                {
                    CSBILL curRec = newdb.CSBILLs.Find(sid);

                    if (curRec.STAMP == cSBILL.STAMP)
                    {
                        cSBILL.STAMP = cSBILL.STAMP + 1;
                        cSBILL.PRFALLOC = "N";
                        cSBILL.PRFNO = null;
                        cSBILL.PRFID = null;

                        db.Entry(cSBILL).State = EntityState.Modified;

                        CSTRANM cSTRANM = db.CSTRANMs.Find("CSPRF", prfno, prfId);
                        db.CSTRANMs.Remove(cSTRANM);
                        db.SaveChanges();

                    }
                    else
                    {
                        Response.StatusCode = (int)HttpStatusCode.NotAcceptable;
                        Response.StatusDescription = "bill was modified";
                        throw new Exception("bill was modified");
                    }
                }
                catch (Exception e)
                {
                    Response.StatusCode = (int)HttpStatusCode.NotAcceptable;
                    Response.StatusDescription = e.Message;

                }
                finally
                {
                    newdb.Dispose();
                }
            }
            //return RedirectToAction("Edit/" + prfnoId);
            return BillAllocated(MyHtmlHelpers.ConvertIdToByteStr(prfno));
        }

        public PartialViewResult AddItem(string id, string prfno)
        {
            string sid = MyHtmlHelpers.ConvertByteStrToId(id);
            CSBILL cSBILL = db.CSBILLs.Find(sid);
            CSPRF cSPRF = db.CSPRFs.Find(prfno);

            if (cSBILL.PRFALLOC == "Y")
            {
                Response.StatusCode = (int)HttpStatusCode.NotAcceptable;
                Response.StatusDescription = "Bill has been Allocated";
            }
            else
            if (cSBILL.refcnt > 0)
            {

                Response.StatusCode = (int)HttpStatusCode.NotAcceptable;
                Response.StatusDescription = "Bill has been touched";
                //throw new Exception("Bill has been touched");
            }
            else
            {
                int prfId = 0;
                string prfnoId = MyHtmlHelpers.ConvertIdToByteStr(prfno);

                ASIDBConnection newdb = new ASIDBConnection();
                try
                {


                    CSBILL curRec = newdb.CSBILLs.Find(sid);

                    var cSTRANMs = db.CSTRANMs.Where(x => x.SOURCE == "CSPRF" && x.SOURCENO == prfno);

                    prfId = 0;
                    if (cSTRANMs.Count() != 0)
                    {
                        prfId = cSTRANMs.Max(y => y.SOURCEID);
                    }
                    prfId++;

                    if (curRec.STAMP == cSBILL.STAMP)
                    {
                        cSBILL.STAMP = cSBILL.STAMP + 1;
                        cSBILL.PRFALLOC = "Y";
                        cSBILL.PRFNO = prfno;
                        cSBILL.PRFID = prfId;

                        db.Entry(cSBILL).State = EntityState.Modified;

                        CSTRANM cSTRANM = new CSTRANM();
                        cSTRANM.STAMP = 0;
                        UpdateCSTRANM(cSPRF, cSBILL, cSTRANM);

                        db.CSTRANMs.Add(cSTRANM);
                        db.SaveChanges();


                    }
                    else
                    {
                        Response.StatusCode = (int)HttpStatusCode.NotAcceptable;
                        Response.StatusDescription = "bill was modified";
                        throw new Exception("bill was modified");
                    }
                }
                catch (Exception e)
                {
                    Response.StatusCode = (int)HttpStatusCode.NotAcceptable;
                    Response.StatusDescription = e.Message;

                }
                finally
                {
                    newdb.Dispose();
                }
            }
            return BillOpen(MyHtmlHelpers.ConvertIdToByteStr(cSPRF.CONO), MyHtmlHelpers.ConvertIdToByteStr(prfno));
            //return RedirectToAction("Edit/" + prfnoId);
        }

        public void BD_UpdateCSTRANM(CSPRF cSPRF, CSBILL cSBILL, CSTRANM cSTRANM)
        {
            UpdateCSTRANM(cSPRF, cSBILL, cSTRANM);
        }

        protected void UpdateCSTRANM(CSPRF cSPRF, CSBILL cSBILL, CSTRANM cSTRANM)
        {
            cSTRANM.SOURCE = "CSPRF";
            cSTRANM.SOURCENO = cSPRF.PRFNO;
            cSTRANM.SOURCEID = cSBILL.PRFID ?? 0;
            cSTRANM.CONO = cSPRF.CONO;
            cSTRANM.DUEDATE = cSPRF.DUEDATE;
            cSTRANM.JOBNO = cSBILL.JOBNO;
            cSTRANM.CASENO = cSBILL.CASENO;
            cSTRANM.CASECODE = cSBILL.CASECODE;
            cSTRANM.ENTDATE = cSBILL.ENTDATE;
            cSTRANM.TRTYPE = cSBILL.ITEMTYPE;
            cSTRANM.TRDESC = cSBILL.ITEMDESC;

            cSTRANM.TRITEM1 = cSBILL.ITEMAMT1;
            cSTRANM.TRITEM2 = cSBILL.ITEMAMT2;
            cSTRANM.TRITEM = cSBILL.ITEMAMT1 + cSBILL.ITEMAMT2;

            cSTRANM.TRTAX1 = cSBILL.TAXAMT1;
            cSTRANM.TRTAX2 = cSBILL.TAXAMT2;
            cSTRANM.TRTAX = cSBILL.TAXAMT1 + cSBILL.TAXAMT2;

            cSTRANM.TRAMT1 = cSBILL.NETAMT1;
            cSTRANM.TRAMT2 = cSBILL.NETAMT2;
            cSTRANM.TRAMT = cSBILL.NETAMT1 + cSBILL.NETAMT2;


            cSTRANM.TRSIGN = "DB";

            cSTRANM.TRSITEM1 = cSBILL.ITEMAMT1;
            cSTRANM.TRSITEM2 = cSBILL.ITEMAMT2;
            cSTRANM.TRSITEM = cSBILL.ITEMAMT1 + cSBILL.ITEMAMT2;

            cSTRANM.TRSTAX1 = cSBILL.TAXAMT1;
            cSTRANM.TRSTAX2 = cSBILL.TAXAMT2;
            cSTRANM.TRSTAX = cSBILL.TAXAMT1 + cSBILL.TAXAMT2;

            cSTRANM.TRSAMT1 = cSBILL.NETAMT1;
            cSTRANM.TRSAMT2 = cSBILL.NETAMT2;
            cSTRANM.TRSAMT = cSBILL.NETAMT1 + cSBILL.NETAMT2;

            cSTRANM.TRITEMOS1 = cSBILL.ITEMAMT1;
            cSTRANM.TRITEMOS2 = cSBILL.ITEMAMT2;
            cSTRANM.TRITEMOS = cSBILL.ITEMAMT1 + cSBILL.ITEMAMT2;


            cSTRANM.TRTAXOS1 = cSBILL.TAXAMT1;
            cSTRANM.TRTAXOS2 = cSBILL.TAXAMT2;
            cSTRANM.TRTAXOS = cSBILL.TAXAMT1 + cSBILL.TAXAMT2;


            cSTRANM.TROS1 = cSBILL.NETAMT1;
            cSTRANM.TROS2 = cSBILL.ITEMAMT2;
            cSTRANM.TROS = cSBILL.NETAMT1 + cSBILL.NETAMT2;


            cSTRANM.APPITEM = 0;
            cSTRANM.APPITEM1 = 0;
            cSTRANM.APPITEM2 = 0;
            cSTRANM.APPTAX = 0;
            cSTRANM.APPTAX1 = 0;
            cSTRANM.APPTAX2 = 0;
            cSTRANM.APPAMT = 0;
            cSTRANM.APPAMT1 = 0;
            cSTRANM.APPAMT2 = 0;

            cSTRANM.APPTYPE = null;
            cSTRANM.APPNO = null;
            cSTRANM.APPID = null;

            cSTRANM.COMPLETE = "N";
            cSTRANM.COMPLETED = DateTime.Parse("01/01/3000");
            cSTRANM.SEQNO = cSPRF.SEQNO;
            cSTRANM.REFCNT = 0;
            cSTRANM.STAMP = cSTRANM.STAMP + 1;
        }

        public ActionResult Listing()
        {
            SACOMP profileRec = db.SACOMPs.FirstOrDefault();
            ViewBag.Company = profileRec.CONAME.ToUpper() + " (" + profileRec.COREGNO + ") ";
            return View(CurrentSelection());
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
