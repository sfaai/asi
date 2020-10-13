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
    public class VCSCOARController : BaseController
    {
        private ASIDBConnection db = new ASIDBConnection();


        public async Task<ActionResult> Email(string id)
        {

            string sid = MyHtmlHelpers.ConvertByteStrToId(id);
            V_CSCOAR vCSCOAR = getReminder(sid);
            int cnt = await SendEmail(vCSCOAR);
            return RedirectToAction("Sent", "Home", new { cnt = cnt });
        }


        public async Task<int> SendEmail(V_CSCOAR vCSCOAR)
        { 
            
            if (string.IsNullOrEmpty(vCSCOAR.Email)) { return 0; }

           
            string html = RenderViewToString(ControllerContext, "~/views/VCSCOAR/Reminder.cshtml", vCSCOAR, true);

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
            message.To.Add(new MailAddress(vCSCOAR.Email)); //replace with valid value
            message.Subject = "Annual Return Reminder";
            message.Body = string.Format(body, model.FromName, model.FromEmail, model.Message);
            message.IsBodyHtml = true;
            using (var smtp = new SmtpClient())
            {
                await smtp.SendMailAsync(message);          
            }
            return 1;
        }

        public async Task<ActionResult> Emails()
        {
            var cSCOARList = CurrentSelection();

            int cnt = 0;
            foreach (V_CSCOAR item in cSCOARList)
            {
                if (!string.IsNullOrEmpty(item.Email))
                {
                    V_CSCOAR vCSCOAR = getReminder(item.CONO);
                    cnt += await SendEmail(vCSCOAR);
        
                }
            }
            return RedirectToAction("Sent", "Home", new { cnt = cnt });
            
        }

        public PartialViewResult Search()
        {
            V_CSCOAR searchRec = null;
            ;
            if (Session["SearchVCSCOARRec"] != null)
            {
                searchRec = (V_CSCOAR)Session["SearchVCSCOARRec"];

            }
            else
            {
                searchRec = new V_CSCOAR();
                searchRec.LASTAR = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            }
            if (Session["SearchVCSCOARSort"] == null)
            {
                Session["SearchVCSCOARSort"] = "CONAME";
            };

            List<SelectListItem> listItems = new List<SelectListItem>();
            listItems.Add(new SelectListItem
            {
                Text = "Company Name",
                Value = "CONAME",
                Selected = (string)Session["SearchVCSCOARSort"] == "CONAME"
            });

            listItems.Add(new SelectListItem
            {
                Text = "Company #",
                Value = "CONO",
                Selected = (string)Session["SearchVCSCOARSort"] == "CONO"
            });

            listItems.Add(new SelectListItem
            {
                Text = "Staff",
                Value = "STAFFCODE",
                Selected = (string)Session["SearchVCSCOARSort"] == "STAFFCODE"
            });

            listItems.Add(new SelectListItem
            {
                Text = "Reminder Type",
                Value = "TYPE",
                Selected = (string)Session["SearchVCSCOARSort"] == "TYPE"
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
        public ActionResult SearchPost(V_CSCOAR cSCOAR)
        {

            Session["SearchVCSCOARRec"] = cSCOAR;
            Session["SearchVCSCOARSort"] = Request.Params["SORTBY"] ?? "CONAME";
            return Index(1);
        }

        public IQueryable<V_CSCOAR> CurrentSelection()
        {
            string pSearchCode = "";
            string pSearchName = "";
            string pSearchStaff = "";
            DateTime pSearchARDate = DateTime.Parse("01/01/0001");
            DateTime pSearchEndDate = DateTime.Parse("01/01/0001");

            if (Session["SearchVCSCOARRec"] != null)
            {
                V_CSCOAR searchRec = (V_CSCOAR)(Session["SearchVCSCOARRec"]);
                pSearchCode = searchRec.COREGNO ?? "";
                pSearchName = searchRec.CONAME ?? "";
                pSearchStaff = searchRec.STAFFCODE ?? "";
                pSearchARDate = searchRec.LASTAR ?? DateTime.Parse("01/01/0001");
                pSearchEndDate = pSearchARDate.AddMonths(1).AddDays(-1);
            }
            else
            {
                pSearchARDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                pSearchEndDate = pSearchARDate.AddMonths(1).AddDays(-1);
            }


            IQueryable<V_CSCOAR> cSCOAR = db.V_CSCOAR;
            int pSearchMonth = pSearchARDate.Month;
            if (pSearchCode != "") { cSCOAR = cSCOAR.Where(x => x.COREGNO.Contains(pSearchCode.ToUpper())); };
            if (pSearchName != "") { cSCOAR = cSCOAR.Where(x => x.CONAME.Contains(pSearchName.ToUpper())); };
            if (pSearchStaff != "") { cSCOAR = cSCOAR.Where(x => x.STAFFCODE == pSearchStaff); };
            if (pSearchARDate != DateTime.Parse("01/01/0001")) { cSCOAR = cSCOAR.Where(x => (x.REMINDER2 == "Y" && (x.REMINDER1.Value.Month == pSearchMonth)) || (x.REMINDER1 >= pSearchARDate && x.REMINDER1 <= pSearchEndDate)); };

            foreach (V_CSCOAR rec in cSCOAR)
            {
                rec.ReminderMonth = pSearchARDate.Month;
                rec.ReminderYear = pSearchARDate.Year;
                rec.REMINDERTYPE = rec.CReminderType;

            }
            ViewBag.ReportTitle = "AR Reminder Listing By " + (string)Session["SearchVCSCOARSort"] + " Dated " + pSearchARDate.ToString("dd/MM/yyyy");
            return cSCOAR;
        }

        public ActionResult Index(int? page)
        {
            var cSCOAR = CurrentSelection();

            if ((string)Session["SearchVCSCOARSort"] == "CONAME")
            {
                return View("Index", cSCOAR.OrderBy(n => n.CONAME).ToList().ToPagedList(page ?? 1, 30));
            }
            else if ((string)Session["SearchVCSCOARSort"] == "CONO")
            {
                return View("Index", cSCOAR.OrderBy(n => n.CONO).ToList().ToPagedList(page ?? 1, 30));

            }
            else if ((string)Session["SearchVCSCOARSort"] == "TYPE")
            {

                return View("Index", cSCOAR.ToList().OrderBy(n => n.REMINDERTYPE).ToPagedList(page ?? 1, 30));

            }
            else if ((string)Session["SearchVCSCOARSort"] == "STAFFCODE")
            {
                return View("Index", cSCOAR.OrderBy(n => n.STAFFCODE).ThenBy(y => y.CONAME).ToList().ToPagedList(page ?? 1, 30));

            }


            return View("Index", cSCOAR.ToList().ToPagedList(page ?? 1, 30));
        }

        public ActionResult ReminderList()
        {
            return OutputList();
        }

        public ActionResult Listing()
        {
            return OutputList();
        }

        public ActionResult OutputList()
        {
            var cSCOAR = CurrentSelection();
           

            SACOMP profileRec = db.SACOMPs.FirstOrDefault();
            ViewBag.Company = profileRec.CONAME.ToUpper() + " (" + profileRec.COREGNO + ") ";
            


            if ((string)Session["SearchVCSCOARSort"] == "CONAME")
            {
                return View( cSCOAR.OrderBy(n => n.CONAME).ToList());
            }
            else if ((string)Session["SearchVCSCOARSort"] == "CONO")
            {
                return View( cSCOAR.OrderBy(n => n.CONO).ToList());

            }
            else if ((string)Session["SearchVCSCOARSort"] == "TYPE")
            {
                return View( cSCOAR.ToList().OrderBy(n => n.REMINDERTYPE).ThenBy(y => y.CONAME));

            }
            else if ((string)Session["SearchVCSCOARSort"] == "STAFFCODE")
            {
                return View(cSCOAR.OrderBy(n => n.STAFFCODE).ThenBy(y => y.CONAME).ToList());
            }


            return View(cSCOAR.ToList());
        }
        // GET: VCSCOAR
        //public ActionResult Index()
        //{
        //    return View(db.V_CSCOAR.ToList());
        //}

        // GET: VCSCOAR/Details/5
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

        public V_CSCOAR getReminder(string sid) // Should use a ViewModel rather than using ViewBag
        {
            if (sid == null)
            {
                return null;
            }
            V_CSCOAR v_CSCOAR = db.V_CSCOAR.Find(sid);
            CSCOADDR cSCOADDR = db.CSCOADDRs.Where(x => x.CONO == sid  && x.MAILADDR == "Y").FirstOrDefault();
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
            DateTime pSearchARDate = DateTime.Parse("01/01/0001");
            DateTime pSearchEndDate = DateTime.Parse("01/01/0001");

            if (Session["SearchVCSCOARRec"] != null)
            {
                V_CSCOAR searchRec = (V_CSCOAR)(Session["SearchVCSCOARRec"]);
                pSearchCode = searchRec.CONO ?? "";
                pSearchName = searchRec.CONAME ?? "";
                pSearchStaff = searchRec.STAFFCODE ?? "";
                pSearchARDate = searchRec.LASTAR ?? DateTime.Parse("01/01/0001");
                pSearchEndDate = pSearchARDate.AddMonths(1).AddDays(-1);
            }
            else
            {
                pSearchARDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                pSearchEndDate = pSearchARDate.AddMonths(1).AddDays(-1);
            }


            SACOMP profileRec = db.SACOMPs.FirstOrDefault();
            ViewBag.Company = profileRec.CONAME.ToUpper() + " (" + profileRec.COREGNO + ") ";
            ViewBag.Address = profileRec.COADDR1 + " " + profileRec.COADDR2 + " " + profileRec.COADDR3 + " " + profileRec.COADDR4;
            ViewBag.Contact = "Tel: " + profileRec.COPHONE1 + " Fax: " + profileRec.COFAX1 + " E-Mail: " + profileRec.COWEB;


            v_CSCOAR.ReminderMonth = pSearchARDate.Month;
            v_CSCOAR.ReminderYear = pSearchARDate.Year;
            ViewBag.ReminderDate = v_CSCOAR.ReminderDate?.ToString("dd MMM yyyy");

            if (v_CSCOAR.ARTOFILE.HasValue)
            {

                ViewBag.AR = v_CSCOAR.ARTOFILE.Value.ToString("dd MMM yyyy");
                DateTime Deadline = v_CSCOAR.ARTOFILE.Value.AddDays(30);
                ViewBag.Deadline = Deadline.ToString("dd MMM yyyy");
            }

            int c = 0;
            DateTime lastar = DateTime.Parse("01/01/0001");
            if (v_CSCOAR.CReminderType == "Special")
            {
                MvcHtmlString s = MvcHtmlString.Create("");// MvcHtmlString.Create(ViewBag.AR);
                MvcHtmlString t = MvcHtmlString.Create("");
                IQueryable<CSCOAR> cSCOAR = db.CSCOARs.Where(x => x.CONO == v_CSCOAR.CONO && x.FILEDAR == null).OrderBy(y => y.ARNO);
                foreach (CSCOAR rec in cSCOAR)
                {

                    if (rec.ARTOFILE != null)
                    {
                        if (c == 0)
                        {
                            s = MvcHtmlString.Create(s + rec.ARTOFILE?.ToString("dd MMM yyyy"));
                            t = MvcHtmlString.Create(t + rec.ARTOFILE.Value.AddDays(30).ToString("dd MMM yyyy"));
                        }
                        else
                        {
                            s = MvcHtmlString.Create(s + " <br /> " + rec.ARTOFILE?.ToString("dd MMM yyyy"));
                            t = MvcHtmlString.Create(t + " <br /> " + rec.ARTOFILE.Value.AddDays(30).ToString("dd MMM yyyy"));
                        }
                        c++;

                        lastar = rec.ARTOFILE ?? DateTime.Parse("01/01/0001");
                    }

                }

                if (lastar.Year != 1) // this is to stop processing if there is no AR records
                {
                    lastar = lastar.AddYears(1);
                    while (lastar.Year <= pSearchARDate.Year)
                    {
                        if (c == 0)
                        {
                            s = MvcHtmlString.Create(s + lastar.ToString("dd MMM yyyy"));
                            t = MvcHtmlString.Create(t + lastar.AddDays(30).ToString("dd MMM yyyy"));
                        }
                        else
                        {
                            s = MvcHtmlString.Create(s + " <br /> " + lastar.ToString("dd MMM yyyy"));
                            t = MvcHtmlString.Create(t + " <br /> " + lastar.AddDays(30).ToString("dd MMM yyyy"));
                        }
                        c++;
                        if (lastar.Year >= pSearchARDate.Year)
                        {

                            break;
                        }
                        lastar = lastar.AddYears(1);
                    }


                }
                ViewBag.AR = s;
                ViewBag.Deadline = t;
            }
            return v_CSCOAR;
        }

        // GET: VCSCOAR/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            V_CSCOAR v_CSCOAR = db.V_CSCOAR.Find(id);
            if (v_CSCOAR == null)
            {
                return HttpNotFound();
            }
            return View(v_CSCOAR);
        }

        // GET: VCSCOAR/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: VCSCOAR/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CONAME,CONO,ARNO,LASTAR,ARTOFILE,FILEDAR,REMINDER1,REM,STAMP,SUBMITAR,REMINDER2")] V_CSCOAR v_CSCOAR)
        {
            if (ModelState.IsValid)
            {
                db.V_CSCOAR.Add(v_CSCOAR);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(v_CSCOAR);
        }

        // GET: VCSCOAR/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            V_CSCOAR v_CSCOAR = db.V_CSCOAR.Find(id);
            if (v_CSCOAR == null)
            {
                return HttpNotFound();
            }
            return View(v_CSCOAR);
        }

        // POST: VCSCOAR/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CONAME,CONO,ARNO,LASTAR,ARTOFILE,FILEDAR,REMINDER1,REM,STAMP,SUBMITAR,REMINDER2")] V_CSCOAR v_CSCOAR)
        {
            if (ModelState.IsValid)
            {
                db.Entry(v_CSCOAR).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(v_CSCOAR);
        }

        // GET: VCSCOAR/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            V_CSCOAR v_CSCOAR = db.V_CSCOAR.Find(id);
            if (v_CSCOAR == null)
            {
                return HttpNotFound();
            }
            return View(v_CSCOAR);
        }

        // POST: VCSCOAR/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            V_CSCOAR v_CSCOAR = db.V_CSCOAR.Find(id);
            db.V_CSCOAR.Remove(v_CSCOAR);
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
