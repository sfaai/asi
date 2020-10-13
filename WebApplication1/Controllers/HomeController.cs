using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        private ASIDBConnection db = new ASIDBConnection();

        public ActionResult samples()
        {
            return View();
        }

        public ActionResult Work()
        {
           return View("Work");
        }

        public ActionResult Index()
        {
            DateTime rptStart = DateTime.Today;
            int mth = rptStart.Month;
            int year = rptStart.Year;
            int batch = 0;
            rptStart = new DateTime(year, mth, 1);

            mth += 1;
            if (mth > 12) { mth = 1; year += 1; }
         
            DateTime rptEnd = new DateTime(year, mth, 1).AddDays(-1);

            if (Session["RPT_START"] == null)
            {
                Session["RPT_START"] = rptStart;
            }

            if (Session["RPT_END"] == null)
            {
                Session["RPT_END"] = rptEnd;
            }

            if (Session["BATCH"] == null)
            {
                Session["BATCH"] = batch;
            }
            //return RedirectToAction(  "Publish", "CSCOMSTRs");

            if (HttpContext.Request.IsAuthenticated) { return Work(); }
            return View();
        }

        public ActionResult main()
        {
            return View();
        }

        public ActionResult main4()
        {
            return View();
        }
        public ActionResult About()
        {
            ViewBag.Message = "About";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Address";
            SACOMP profileRec = db.SACOMPs.FirstOrDefault();
            return View(profileRec);
        }

        public JsonResult SetRptParam(string RPT_START, string RPT_END, int? BATCH)
        {
            DateTime rptStart = new DateTime(1, 1, 1);
            DateTime.TryParse(RPT_START, out rptStart);

            DateTime rptEnd = new DateTime(1, 1, 1); ;
            DateTime.TryParse(RPT_END, out rptEnd);

          
            Session["RPT_START"] = rptStart;
                   
            Session["RPT_END"] = rptEnd;
          

            Session["BATCH"] = BATCH ?? 0;
            return Json("OK",JsonRequestBehavior.AllowGet);
        }

        public ActionResult ContactViaEmail()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ContactViaEmail(EmailFormModel model)
        {
            if (ModelState.IsValid)
            {
                var body = "<p>Email From: {0} ({1})</p><p>Message:</p><p>{2}</p>";
                var message = new MailMessage();
                message.To.Add(new MailAddress("asi@sec.my")); //replace with valid value
                message.Subject = "Contact";
                message.Body = string.Format(body, model.FromName, model.FromEmail, model.Message);
                message.IsBodyHtml = true;
                using (var smtp = new SmtpClient())
                {
                    await smtp.SendMailAsync(message);
                    return RedirectToAction("Sent");
                }
            }
            return View(model);
        }

        public ActionResult Sent(int? cnt)
        {
            ViewBag.cnt = cnt ?? 1;
            return View();
        }
    }
}