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
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace WebApplication1.Controllers
{
    [Authorize(Roles = "Administrator,CS-A/C,CS-SEC,CS-AS")]
    public class VCSCOAGMController : BaseController
    {
        private ASIDBConnection db = new ASIDBConnection();

        static string RenderViewToString(ControllerContext context,
                              string viewPath,
                              object model = null,
                              bool partial = false)
        {
            // first find the ViewEngine for this view
            ViewEngineResult viewEngineResult = null;
            if (partial)
                viewEngineResult = ViewEngines.Engines.FindPartialView(context, viewPath);
            else
                viewEngineResult = ViewEngines.Engines.FindView(context, viewPath, null);

            if (viewEngineResult == null)
                throw new FileNotFoundException("View cannot be found.");

            // get the view and attach the model to view data
            var view = viewEngineResult.View;
            context.Controller.ViewData.Model = model;

            string result = null;

            using (var sw = new StringWriter())
            {
                var ctx = new ViewContext(context, view,
                                            context.Controller.ViewData,
                                            context.Controller.TempData,
                                            sw);
                view.Render(ctx, sw);
                result = sw.ToString();
            }

            return result;
        }

        public async Task<ActionResult> Email(string id)
        {

            string sid = MyHtmlHelpers.ConvertByteStrToId(id);
            V_CSCOAGM vCSCOAGM = getReminder(sid);

            int cnt = await SendEmail(vCSCOAGM);
            return RedirectToAction("Sent", "Home", new { cnt = cnt });
        }

        public async Task<int> SendEmail(V_CSCOAGM vCSCOAGM)
        {

            if (string.IsNullOrEmpty(vCSCOAGM.Email)) { return 0; }


            string html = RenderViewToString(ControllerContext, "~/views/VCSCOAGM/Reminder.cshtml", vCSCOAGM, true);

            //string filename = @"c:\temp\email.html";
            //var sw = new System.IO.StreamWriter(filename, true);
            //sw.WriteLine(html);
            //sw.Close();

            EmailFormModel model = new WebApplication1.EmailFormModel();
            model.Message = html;
            model.FromEmail = "reminder@asi.my";
            model.FromName = "ASI Secretary";

            var body = "<p>Email From: {0} ({1})</p><p>Message:</p><p>{2}</p>";
            var message = new MailMessage();
            message.To.Add(new MailAddress(vCSCOAGM.Email)); //replace with valid value
            message.Subject = "Financial Statement Reminder";
            message.Body = string.Format(body, model.FromName, model.FromEmail, model.Message);
            message.IsBodyHtml = true;
            using (var smtp = new SmtpClient())
            {
                await smtp.SendMailAsync(message);
            }
            return 1;
        }

        public async Task<ActionResult> EReminderList(string param)
        {
            var cSCOAGM = CurrentSelection();
            IEnumerable<V_CSCOAGM> recList = cSCOAGM.ToList();


            SACOMP profileRec = db.SACOMPs.FirstOrDefault();
            ViewBag.Company = profileRec.CONAME.ToUpper() + " (" + profileRec.COREGNO + ") ";

            string paramStr = "";
            if (param != "ALL")
            {
                if (param == "FIRST") { paramStr = "First"; }
                else if (param == "SECOND") { paramStr = "Second"; }
                else if (param == "FINAL") { paramStr = "Final"; }
                else if (param == "SPECIAL") { paramStr = "Special"; }

            }

            if (paramStr != string.Empty)
            {
                recList = cSCOAGM.ToList().Where(x => x.REMINDERTYPE == paramStr);
            }

            if ((string)Session["SearchVCSCOAGMSort"] == "TYPE")
            {
                recList = cSCOAGM.ToList().OrderBy(n => n.REMINDERTYPE).ThenBy(y => y.CONAME);

            }
            var cnt = 0;

            foreach (V_CSCOAGM item in recList)
            {
                if (!string.IsNullOrEmpty(item.Email))
                {                   
                    V_CSCOAGM vCSCOAGM = getReminder(item.CONO);
                    cnt += await SendEmail(vCSCOAGM);

                }
            }
            return RedirectToAction("Sent", "Home", new { cnt = cnt });

        }

        public PartialViewResult Search()
        {
            V_CSCOAGM searchRec = null;
            ;
            if (Session["SearchVCSCOAGMRec"] != null)
            {
                searchRec = (V_CSCOAGM)Session["SearchVCSCOAGMRec"];

            }
            else
            {
                searchRec = new V_CSCOAGM();
                searchRec.AGMDATE = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            }
            if (Session["SearchVCSCOAGMSort"] == null)
            {
                Session["SearchVCSCOAGMSort"] = "CONAME";
            };

            List<SelectListItem> listItems = new List<SelectListItem>();
            listItems.Add(new SelectListItem
            {
                Text = "Company Name",
                Value = "CONAME",
                Selected = (string)Session["SearchVCSCOAGMSort"] == "CONAME"
            });

            listItems.Add(new SelectListItem
            {
                Text = "Company #",
                Value = "CONO",
                Selected = (string)Session["SearchVCSCOAGMSort"] == "CONO"
            });

            listItems.Add(new SelectListItem
            {
                Text = "Staff",
                Value = "STAFFCODE",
                Selected = (string)Session["SearchVCSCOAGMSort"] == "STAFFCODE"
            });

            listItems.Add(new SelectListItem
            {
                Text = "Reminder Type",
                Value = "TYPE",
                Selected = (string)Session["SearchVCSCOAGMSort"] == "TYPE"
            });

            ViewBag.STAFFCODE = new SelectList(db.HKSTAFFs, "STAFFCODE", "STAFFDESC");
            ViewBag.SORTBY = listItems;
            return PartialView("Partial/Search", searchRec);
        }


        [HttpGet]
        public ActionResult SearchPost()
        {
            return Index(1);
        }

        [HttpPost]
        public ActionResult SearchPost(V_CSCOAGM cSCOAGM)
        {

            Session["SearchVCSCOAGMRec"] = cSCOAGM;
            Session["SearchVCSCOAGMSort"] = Request.Params["SORTBY"] ?? "CONAME";
            return Index(1);
        }



        public ActionResult Listing()
        {
            return Output("ALL");
        }

        public ActionResult Output(string param)
        {
            var cSCOAGM = CurrentSelection();


            SACOMP profileRec = db.SACOMPs.FirstOrDefault();
            ViewBag.Company = profileRec.CONAME.ToUpper() + " (" + profileRec.COREGNO + ") ";

            string paramStr = "";
            if (param != "ALL")
            {
                if (param == "FIRST") { paramStr = "First"; }
                else if (param == "SECOND") { paramStr = "Second"; }
                else if (param == "FINAL") { paramStr = "Final"; }
                else if (param == "SPECIAL") { paramStr = "Special"; }

            }

            if (paramStr != string.Empty)
            {
                return View(cSCOAGM.ToList().Where(x => x.REMINDERTYPE == paramStr));
            }

            if ((string)Session["SearchVCSCOAGMSort"] == "TYPE")
            {
                return View(cSCOAGM.ToList().OrderBy(n => n.REMINDERTYPE).ThenBy(y => y.CONAME));

            }


            return View(cSCOAGM.ToList());

        }

        public IQueryable<V_CSCOAGM> CurrentSelection()
        {
            string pSearchCode = "";
            string pSearchName = "";
            string pSearchStaff = "";
            DateTime pSearchAgmDate = DateTime.Parse("01/01/0001");
            DateTime pSearchEndDate = DateTime.Parse("01/01/0001");

            if (Session["SearchVCSCOAGMRec"] != null)
            {
                V_CSCOAGM searchRec = (V_CSCOAGM)(Session["SearchVCSCOAGMRec"]);
                pSearchCode = searchRec.COREGNO ?? "";
                pSearchName = searchRec.CONAME ?? "";
                pSearchStaff = searchRec.STAFFCODE ?? "";
                pSearchAgmDate = searchRec.AGMDATE ?? DateTime.Parse("01/01/0001");
                pSearchEndDate = pSearchAgmDate.AddMonths(1).AddDays(-1);
            }
            else
            {
                pSearchAgmDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                pSearchEndDate = pSearchAgmDate.AddMonths(1).AddDays(-1);
            }


            IQueryable<V_CSCOAGM> cSCOAGM = db.V_CSCOAGM;
            int pSearchMonth = pSearchAgmDate.Month;
            if (pSearchCode != "") { cSCOAGM = cSCOAGM.Where(x => x.COREGNO.Contains(pSearchCode.ToUpper())); };
            if (pSearchName != "") { cSCOAGM = cSCOAGM.Where(x => x.CONAME.Contains(pSearchName.ToUpper())); };
            if (pSearchStaff != "") { cSCOAGM = cSCOAGM.Where(x => x.STAFFCODE == pSearchStaff); };
            if (pSearchAgmDate != DateTime.Parse("01/01/0001")) { cSCOAGM = cSCOAGM.Where(x => (x.REMINDER4 == "Y" && (x.REMTH3 == pSearchMonth)) || (x.REMINDER1 >= pSearchAgmDate && x.REMINDER1 <= pSearchEndDate) || (x.REMINDER2 >= pSearchAgmDate && x.REMINDER2 <= pSearchEndDate) || (x.REMINDER3 >= pSearchAgmDate && x.REMINDER3 <= pSearchEndDate)); };

            foreach (V_CSCOAGM rec in cSCOAGM)
            {
                rec.ReminderMonth = pSearchAgmDate.Month;
                rec.ReminderYear = pSearchAgmDate.Year;
                rec.REMINDERTYPE = rec.CReminderType;
            }

            if ((string)Session["SearchVCSCOAGMSort"] == "CONAME")
            {
                return cSCOAGM.OrderBy(n => n.CONAME);
            }
            else if ((string)Session["SearchVCSCOAGMSort"] == "CONO")
            {
                return cSCOAGM.OrderBy(n => n.CONO);

            }
            else if ((string)Session["SearchVCSCOAGMSort"] == "TYPE")
            {
                return cSCOAGM.OrderBy(n => n.REMINDERTYPE).ThenBy(y => y.CONAME);

            }
            else if ((string)Session["SearchVCSCOAGMSort"] == "STAFFCODE")
            {
                return cSCOAGM.OrderBy(n => n.STAFFCODE).ThenBy(y => y.CONAME);

            }

            ViewBag.ReportTitle = "AGM Reminder Listing By " + (string)Session["SearchVCSCOAGMSort"] + " Dated " + pSearchAgmDate.ToString("dd/MM/yyyy");
            return cSCOAGM;
        }

        public ActionResult Index(int? page)
        {
            var cSCOAGM = CurrentSelection();

            if ((string)Session["SearchVCSCOAGMSort"] == "TYPE")
            {
                return View("Index", cSCOAGM.ToList().OrderBy(n => n.REMINDERTYPE).ThenBy(y => y.CONAME).ToPagedList(page ?? 1, 30));

            }

            return View("Index", cSCOAGM.ToList().ToPagedList(page ?? 1, 30));
        }

        public ActionResult ReminderList(string id)
        {
            return Output(id);
        }

        // GET: VCSCOAGM/Details/5
        public ActionResult Reminder(string id)
        {
            string sid = MyHtmlHelpers.ConvertByteStrToId(id);

            return View(getReminder(sid));
        }

        public PartialViewResult ReminderM(string id)
        {
            string sid = MyHtmlHelpers.ConvertByteStrToId(id);
            return PartialView("Reminder", getReminder(sid));
        }

        public V_CSCOAGM getReminder(string sid)
        {
            if (sid == null)
            {
                return null;
            }
            V_CSCOAGM v_CSCOAGM = db.V_CSCOAGM.Find(sid);
            CSCOADDR cSCOADDR = db.CSCOADDRs.Where(x => x.CONO == sid && x.MAILADDR == "Y").FirstOrDefault();
            if (cSCOADDR == null) { cSCOADDR = db.CSCOADDRs.Where(x => x.CONO == sid).FirstOrDefault(); }
            if (cSCOADDR != null)
            {
                ViewBag.Addr1 = cSCOADDR.ADDR1;
                ViewBag.Addr2 = cSCOADDR.ADDR2;
                ViewBag.Addr3 = cSCOADDR.ADDR3;
                ViewBag.Addr4 = cSCOADDR.POSTAL;
                if (cSCOADDR.HKCITY != null)
                {
                    ViewBag.Addr4 = ViewBag.Addr4 + " " + cSCOADDR.HKCITY.CITYDESC;
                }
                else ViewBag.Addr4 = ViewBag.Addr4 + " " + cSCOADDR.CITYCODE;
            }
            else
            {
                ViewBag.Addr1 = "";
                ViewBag.Addr2 = "";
                ViewBag.Addr3 = "";
                ViewBag.Addr4 = "";
            }

            string pSearchCode = "";
            string pSearchName = "";
            string pSearchStaff = "";
            DateTime pSearchAgmDate = DateTime.Parse("01/01/0001");
            DateTime pSearchEndDate = DateTime.Parse("01/01/0001");

            if (Session["SearchVCSCOAGMRec"] != null)
            {
                V_CSCOAGM searchRec = (V_CSCOAGM)(Session["SearchVCSCOAGMRec"]);
                pSearchCode = searchRec.CONO ?? "";
                pSearchName = searchRec.CONAME ?? "";
                pSearchStaff = searchRec.STAFFCODE ?? "";
                pSearchAgmDate = searchRec.AGMDATE ?? DateTime.Parse("01/01/0001");
            }
            else
            {
                pSearchAgmDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);              
            }
            pSearchEndDate = pSearchAgmDate.AddMonths(1).AddDays(-1);

            SACOMP profileRec = db.SACOMPs.FirstOrDefault();
            ViewBag.Company = profileRec.CONAME.ToUpper() + " (" + profileRec.COREGNO + ") ";
            ViewBag.Address = profileRec.COADDR1 + " " + profileRec.COADDR2 + " " + profileRec.COADDR3 + " " + profileRec.COADDR4;
            ViewBag.Contact = "Tel: " + profileRec.COPHONE1 + " Fax: " + profileRec.COFAX1 + " E-Mail: " + profileRec.COWEB;


            v_CSCOAGM.ReminderMonth = pSearchAgmDate.Month;
            v_CSCOAGM.ReminderYear = pSearchAgmDate.Year;
            ViewBag.ReminderDate = v_CSCOAGM.ReminderDate?.ToString("dd MMM yyyy");

            if (v_CSCOAGM.FYETOFILE.HasValue)
            {
                DateTime DeadlineCirculationDate = v_CSCOAGM.FYETOFILE.Value.AddMonths(6);
                if (DeadlineCirculationDate.Month < 12)
                {
                    DeadlineCirculationDate = new DateTime(DeadlineCirculationDate.Year, DeadlineCirculationDate.Month + 1, 1);
                      
                }
                else
                {
                    DeadlineCirculationDate = new DateTime(DeadlineCirculationDate.Year + 1, DeadlineCirculationDate.Month - 12 + 1, 1);
                }
                DeadlineCirculationDate = DeadlineCirculationDate.AddDays(-1);
                DateTime DeadlineSubmissionDate = DeadlineCirculationDate.AddDays(30);
                ViewBag.Deadline1 = DeadlineCirculationDate.ToString("dd MMM yyyy");
                ViewBag.Deadline2 = DeadlineSubmissionDate.ToString("dd MMM yyyy");
                ViewBag.FYE = v_CSCOAGM.FYETOFILE.Value.ToString("dd MMM yyyy");
            }

            int c = 0;
            DateTime lastfye = DateTime.Parse("01/01/0001");
            if (v_CSCOAGM.CReminderType == "Special")
            {
                MvcHtmlString s = MvcHtmlString.Create("");// MvcHtmlString.Create(ViewBag.FYE);
                IQueryable<CSCOAGM> cSCOAGM = db.CSCOAGMs.Where(x => x.CONO == v_CSCOAGM.CONO && x.FILEDFYE == null).OrderBy(y => y.AGMNO);
                foreach (CSCOAGM rec in cSCOAGM)
                {

                    if (rec.FYETOFILE != null)
                    {
                        if (c == 0)
                        {
                            s = MvcHtmlString.Create(s + rec.FYETOFILE?.ToString("dd MMM yyyy"));
                        }
                        else
                        {
                            s = MvcHtmlString.Create(s + " <br /> " + rec.FYETOFILE?.ToString("dd MMM yyyy"));
                        }
                        c++;

                        lastfye = rec.FYETOFILE ?? DateTime.Parse("01/01/0001");
                    }

                }

                if (lastfye.Year != 1) // this is to stop processing if there is no AGM records
                {
                    lastfye = lastfye.AddYears(1);
                    // while (lastfye.Year <= pSearchAgmDate.Year)
                    while (lastfye <= pSearchAgmDate)
                    {
                        if (c == 0)
                        {
                            s = MvcHtmlString.Create(s + lastfye.ToString("dd MMM yyyy"));
                        }
                        else
                        {
                            s = MvcHtmlString.Create(s + " <br /> " + lastfye.ToString("dd MMM yyyy"));
                        }
                        c++;
                        if (true || (lastfye.Year >= pSearchAgmDate.Year) )
                        {
                            DateTime DeadlineCirculationDate = lastfye.AddMonths(6);
                            if (DeadlineCirculationDate.Month < 12)
                            {
                                DeadlineCirculationDate = new DateTime(DeadlineCirculationDate.Year, DeadlineCirculationDate.Month + 1, 1);

                            }
                            else
                            {
                                DeadlineCirculationDate = new DateTime(DeadlineCirculationDate.Year + 1, DeadlineCirculationDate.Month - 12 + 1, 1);
                            }
                            DeadlineCirculationDate = DeadlineCirculationDate.AddDays(-1);
                            DateTime DeadlineSubmissionDate = DeadlineCirculationDate.AddDays(30);
                            ViewBag.Deadline1 = DeadlineCirculationDate.ToString("dd MMM yyyy");
                            ViewBag.Deadline2 = DeadlineSubmissionDate.ToString("dd MMM yyyy");
                            //break;
                        }
                        lastfye = lastfye.AddYears(1);
                    }


                }
                ViewBag.FYE = s;
            }
            return v_CSCOAGM;
        }

        // GET: VCSCOAGM/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            V_CSCOAGM v_CSCOAGM = db.V_CSCOAGM.Find(id);
            if (v_CSCOAGM == null)
            {
                return HttpNotFound();
            }
            return View(v_CSCOAGM);
        }

        // GET: VCSCOAGM/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: VCSCOAGM/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CONAME,CONO,AGMNO,LASTFYE,FYETOFILE,FILEDFYE,AGMLAST,AGMEXT,REMINDER1,REMINDER2,REMINDER3,AGMDATE,AGMFILED,REM,STAMP,CIRCDATE,REMINDER4")] V_CSCOAGM v_CSCOAGM)
        {
            if (ModelState.IsValid)
            {
                db.V_CSCOAGM.Add(v_CSCOAGM);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(v_CSCOAGM);
        }

        // GET: VCSCOAGM/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            V_CSCOAGM v_CSCOAGM = db.V_CSCOAGM.Find(id);
            if (v_CSCOAGM == null)
            {
                return HttpNotFound();
            }
            return View(v_CSCOAGM);
        }

        // POST: VCSCOAGM/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CONAME,CONO,AGMNO,LASTFYE,FYETOFILE,FILEDFYE,AGMLAST,AGMEXT,REMINDER1,REMINDER2,REMINDER3,AGMDATE,AGMFILED,REM,STAMP,CIRCDATE,REMINDER4")] V_CSCOAGM v_CSCOAGM)
        {
            if (ModelState.IsValid)
            {
                db.Entry(v_CSCOAGM).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(v_CSCOAGM);
        }

        // GET: VCSCOAGM/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            V_CSCOAGM v_CSCOAGM = db.V_CSCOAGM.Find(id);
            if (v_CSCOAGM == null)
            {
                return HttpNotFound();
            }
            return View(v_CSCOAGM);
        }

        // POST: VCSCOAGM/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            V_CSCOAGM v_CSCOAGM = db.V_CSCOAGM.Find(id);
            db.V_CSCOAGM.Remove(v_CSCOAGM);
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
