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
using System.Data.Entity.Core;
using System.Data.Entity.Validation;
using System.Data.Entity.Infrastructure;
using FirebirdSql.Data.Client;
using FirebirdSql.Data.FirebirdClient;
using System.Configuration;
using FirebirdSql.Data.Isql;
using System.Text;
using DCSoft.RTF;


namespace WebApplication1.Controllers
{

    [Authorize(Roles = "Administrator,CS-A/C,CS-SEC,CS-AS,ACC")]
    public class CSCOMSTRsController : Controller
    {
        private ASIDBConnection db = new ASIDBConnection();


        public ActionResult RemoveCompanyReference(string CONO)
        {
            string conString = ConfigurationManager.ConnectionStrings["SirisCS"].ConnectionString;

            string oldId = CONO;
            using (FbConnection con = new FbConnection(conString))
            {

                con.Open();
                FbBatchExecution fbe = new FbBatchExecution(con);
                //loop through your commands here
                {
                    StringBuilder dbStr = new StringBuilder();
                    dbStr.Append("delete from CSCOADDR where CONO = '" + oldId + "'; ");
                    dbStr.Append("delete from CSCOADT where CONO = '" + oldId + "'; ");
                    dbStr.Append("delete from  CSCOAGT where CONO = '" + oldId + "' ;");
                    dbStr.Append("delete from  CSCOAGTCHG  where CONO = '" + oldId + "' ;");
                    dbStr.Append("delete from  CSCOAK  where CONO = '" + oldId + "' ;");
                    dbStr.Append("delete from  CSCOAR  where CONO = '" + oldId + "' ;");
                    dbStr.Append("delete from  CSCOBANK  where CONO = '" + oldId + "'; ");
                    dbStr.Append("delete from  CSCOCM  where CONO = '" + oldId + "'; ");
                    dbStr.Append("delete from  CSCODEB  where CONO = '" + oldId + "' ;");
                    dbStr.Append("delete from  CSCODR  where CONO = '" + oldId + "' ;");
                    dbStr.Append("delete from  CSCODRCHG  where CONO = '" + oldId + "' ;");
                    dbStr.Append("delete from  CSCOFEE  where CONO = '" + oldId + "' ;");
                    dbStr.Append("delete from  CSCOLASTNO  where CONO = '" + oldId + "'; ");
                    dbStr.Append("delete from  CSCOMGR  where CONO = '" + oldId + "'; ");
                    dbStr.Append("delete from  CSCONAME  where CONO = '" + oldId + "'; ");
                    dbStr.Append("delete from  CSCOPARENT  where CONO = '" + oldId + "' ;");
                    dbStr.Append("delete from  CSCOPIC  where CONO = '" + oldId + "'; ");
                    dbStr.Append("delete from  CSCOPUK  where CONO = '" + oldId + "' ;");
                    dbStr.Append("delete from  CSCOSEC  where CONO = '" + oldId + "' ;");
                    dbStr.Append("delete from  CSCOSECCHG  where CONO = '" + oldId + "'; ");
                    dbStr.Append("delete from  CSCOSH  where CONO = '" + oldId + "' ;");
                    dbStr.Append("delete from  CSCOSHCHG  where CONO = '" + oldId + "' ;");
                    dbStr.Append("delete from  CSCOSHEQ  where CONO = '" + oldId + "' ;");
                    dbStr.Append("delete from  CSCOSHEQD  where CONO = '" + oldId + "' ;");
                    dbStr.Append("delete from  CSCOSTAT  where CONO = '" + oldId + "' ;");
                    dbStr.Append("delete from  CSCOTX  where CONO = '" + oldId + "'; ");
                    dbStr.Append("delete from  CSCOAR  where CONO = '" + oldId + "' ;");
                    dbStr.Append("delete from  CSDNM  where CONO = '" + oldId + "' ;");
                    dbStr.Append("delete from  CSGRPD  where CONO = '" + oldId + "' ;");
                    dbStr.Append("delete from  CSINV  where CONO = '" + oldId + "' ;");
                    dbStr.Append("delete from  CSJOBM  where CONO = '" + oldId + "' ;");
                    dbStr.Append("delete from  CSLDG  where CONO = '" + oldId + "' ;");
                    dbStr.Append("delete from  CSOFF  where CONO = '" + oldId + "' ;");
                    dbStr.Append("delete from  CSPRF where CONO = '" + oldId + "' ;");
                    dbStr.Append("delete from  CSPYM  where CONO = '" + oldId + "' ;");
                    dbStr.Append("delete from  CSQT  where CONO = '" + oldId + "' ;");
                    dbStr.Append("delete from  CSRCP  where CONO = '" + oldId + "' ;");
                    dbStr.Append("delete from  CSRF where CONO = '" + oldId + "' ;");
                    dbStr.Append("delete from  CSTRANM  where CONO = '" + oldId + "' ;");
                    dbStr.Append("delete from  EMTRA  where CONO = '" + oldId + "' ;");
                    dbStr.Append("delete from  EMTRCD  where CONO = '" + oldId + "' ;");
                    dbStr.Append("delete from  EMTRCM  where CONO = '" + oldId + "' ;");
                    dbStr.Append("delete from  EMTRSD  where CONO = '" + oldId + "' ;");
                    dbStr.Append("delete from  EMTRSM  where CONO = '" + oldId + "' ;");
                    dbStr.Append("delete from  EMTRTD  where CONO = '" + oldId + "' ;");
                    dbStr.Append("delete from  EMTRTM  where CONO = '" + oldId + "' ;");
                    dbStr.Append("delete from  UTEMAILGD  where CONO = '" + oldId + "' ;");
                    dbStr.Append("delete from  SRSGRIM  where CONO = '" + oldId + "' ;");
                    dbStr.Append("delete from  SRSGPPM  where CONO = '" + oldId + "' ;");
                    dbStr.Append("delete from  SFSGSCMVD2  where CONO = '" + oldId + "' ;");
                    dbStr.Append("delete from  SFSGSCMV  where CONO = '" + oldId + "' ;");
                    dbStr.Append("delete from  SFSGNEW  where CONO = '" + oldId + "' ;");
                    dbStr.Append("delete from  SFSGNAMECHG  where CONO = '" + oldId + "' ;");
                    dbStr.Append("delete from  SFSGDRMVD2 where CONO = '" + oldId + "' ;");
                    dbStr.Append("delete from  SFSGDRMV where CONO = '" + oldId + "' ;");
                    dbStr.Append("delete from  SFSGADDRCHG  where CONO = '" + oldId + "' ;");
                    dbStr.Append("delete from  SFMYSCMVD2  where CONO = '" + oldId + "' ;");
                    dbStr.Append("delete from  SFMYSCMV  where CONO = '" + oldId + "' ;");
                    dbStr.Append("delete from  SFMYNEW  where CONO = '" + oldId + "' ;");
                    dbStr.Append("delete from  SFMYNAMECHG where CONO = '" + oldId + "' ;");
                    dbStr.Append("delete from  SFMYADDRCHG  where CONO = '" + oldId + "' ;");
                    dbStr.Append("delete from  CSCNM  where CONO = '" + oldId + "' ;");
                    dbStr.Append("delete from  CSCANC3P where CONO = '" + oldId + "' ;");
                    dbStr.Append("delete from  CSBILL  where CONO = '" + oldId + "' ;");
                    dbStr.Append("delete from  CSCNM  where CONO = '" + oldId + "' ;");
                    dbStr.Append("delete from  CICL  where CLCODE = '" + oldId + "' ;");
                    dbStr.Append("delete from  CICLADDR  where CLCODE = '" + oldId + "' ;");
                    dbStr.Append("delete from  CICLPIC  where CLCODE = '" + oldId + "' ;");
                    dbStr.Append("delete from  CICLREG  where CLCODE = '" + oldId + "' ;");
                    dbStr.Append("delete from CSCOMSTR where CONO = '" + oldId + "'; ");
                    string dbScript = dbStr.ToString();
                    FbScript cmd = new FbScript(dbScript);
                    cmd.Parse();
                    fbe.AppendSqlStatements(cmd);
                }
                try
                {
                    fbe.Execute();

                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                finally
                {
                    con.Close();
                }
                return Index(1);
            }
        }

        public ActionResult ChangeCompanyReference(string New_Company_No, string CONO)
        {
            string conString = ConfigurationManager.ConnectionStrings["SirisCS"].ConnectionString;

            string oldId = CONO;
            string newId = New_Company_No;

            CSCOMSTR cSCOMSTR = db.CSCOMSTRs.Find(oldId);
            if (cSCOMSTR != null)
            {
                CSCOMSTR cSCOMSTRNew = new CSCOMSTR();

                cSCOMSTRNew.ARRE = cSCOMSTR.ARRE;
                cSCOMSTRNew.CMMT = cSCOMSTR.CMMT;
                cSCOMSTRNew.CONAME = cSCOMSTR.CONAME;
                cSCOMSTRNew.CONSTCODE = cSCOMSTR.CONSTCODE;
                cSCOMSTRNew.COSTAT = cSCOMSTR.COSTAT;
                cSCOMSTRNew.COSTATD = cSCOMSTR.COSTATD;
                cSCOMSTRNew.FILELOC = cSCOMSTR.FILELOC;
                cSCOMSTRNew.FILETYPE = cSCOMSTR.FILETYPE;
                cSCOMSTRNew.INCCTRY = cSCOMSTR.INCCTRY;
                cSCOMSTRNew.INCDATE = cSCOMSTR.INCDATE;
                cSCOMSTRNew.INTYPE = cSCOMSTR.INTYPE;
                cSCOMSTRNew.PINDCODE = cSCOMSTR.PINDCODE;
                cSCOMSTRNew.PRINOBJ = cSCOMSTR.PRINOBJ;
                cSCOMSTRNew.REFCODE = cSCOMSTR.REFCODE;
                cSCOMSTRNew.REM = cSCOMSTR.REM;
                cSCOMSTRNew.SEALLOC = cSCOMSTR.SEALLOC;
                cSCOMSTRNew.SEQNO = cSCOMSTR.SEQNO;
                cSCOMSTRNew.SINDCODE = cSCOMSTR.SINDCODE;
                cSCOMSTRNew.SPECIALRE = cSCOMSTR.SPECIALRE;
                cSCOMSTRNew.STAFFCODE = cSCOMSTR.STAFFCODE;
                cSCOMSTRNew.STAMP = cSCOMSTR.STAMP;
                cSCOMSTRNew.SXCODE = cSCOMSTR.SXCODE;
                cSCOMSTRNew.SXNAME = cSCOMSTR.SXNAME;
                cSCOMSTRNew.WEB = cSCOMSTR.WEB;
                cSCOMSTRNew.CONO = newId;
                db.CSCOMSTRs.Add(cSCOMSTRNew);
                db.SaveChanges();



                using (FbConnection con = new FbConnection(conString))
                {

                    con.Open();
                    FbBatchExecution fbe = new FbBatchExecution(con);
                    //loop through your commands here
                    {
                        StringBuilder dbStr = new StringBuilder();
                        dbStr.Append("insert into CSCOADDR select '" + newId + "', addrid, mailaddr, addrtype, addr1, addr2, addr3, postal, citycode, statecode, ctrycode, phone1, phone2, fax1, fax2, oprhrs, rem, sdate, edate, enddate, stamp from cscoaddr where CONO = '" + oldId + "'; ");
                        dbStr.Append("update CSCOADT set CONO = '" + newId + "' where CONO = '" + oldId + "'; ");
                        dbStr.Append("update CSCOAGT set CONO = '" + newId + "' where CONO = '" + oldId + "' ;");
                        dbStr.Append("update CSCOAGTCHG set CONO = '" + newId + "' where CONO = '" + oldId + "' ;");
                        dbStr.Append("update CSCOAK set CONO = '" + newId + "' where CONO = '" + oldId + "' ;");
                        dbStr.Append("update CSCOAR set CONO = '" + newId + "' where CONO = '" + oldId + "' ;");
                        dbStr.Append("update CSCOBANK set CONO = '" + newId + "' where CONO = '" + oldId + "'; ");
                        dbStr.Append("update CSCOCM set CONO = '" + newId + "' where CONO = '" + oldId + "'; ");
                        dbStr.Append("update CSCODEB set CONO = '" + newId + "' where CONO = '" + oldId + "' ;");
                        dbStr.Append("update CSCODR set CONO = '" + newId + "' where CONO = '" + oldId + "' ;");
                        dbStr.Append("update CSCODRCHG set CONO = '" + newId + "' where CONO = '" + oldId + "' ;");
                        dbStr.Append("update CSCOFEE set CONO = '" + newId + "' where CONO = '" + oldId + "' ;");
                        dbStr.Append("update CSCOLASTNO set CONO = '" + newId + "' where CONO = '" + oldId + "'; ");
                        dbStr.Append("update CSCOMGR set CONO = '" + newId + "' where CONO = '" + oldId + "'; ");
                        dbStr.Append("update CSCONAME set CONO = '" + newId + "' where CONO = '" + oldId + "'; ");
                        dbStr.Append("update CSCOPARENT set CONO = '" + newId + "' where CONO = '" + oldId + "' ;");
                        dbStr.Append("update CSCOPIC set CONO = '" + newId + "' where CONO = '" + oldId + "'; ");
                        dbStr.Append("update CSCOPUK set CONO = '" + newId + "' where CONO = '" + oldId + "' ;");
                        dbStr.Append("update CSCOSEC set CONO = '" + newId + "' where CONO = '" + oldId + "' ;");
                        dbStr.Append("update CSCOSECCHG set CONO = '" + newId + "' where CONO = '" + oldId + "'; ");
                        dbStr.Append("update CSCOSH set CONO = '" + newId + "' where CONO = '" + oldId + "' ;");
                        dbStr.Append("update CSCOSHCHG set CONO = '" + newId + "' where CONO = '" + oldId + "' ;");
                        dbStr.Append("update CSCOSHEQ set CONO = '" + newId + "' where CONO = '" + oldId + "' ;");
                        dbStr.Append("update CSCOSHEQD set CONO = '" + newId + "' where CONO = '" + oldId + "' ;");
                        dbStr.Append("update CSCOSTAT set CONO = '" + newId + "' where CONO = '" + oldId + "' ;");
                        dbStr.Append("update CSCOTX set CONO = '" + newId + "' where CONO = '" + oldId + "'; ");
                        dbStr.Append("update CSCOAR set CONO = '" + newId + "' where CONO = '" + oldId + "' ;");
                        dbStr.Append("update CSDNM set CONO = '" + newId + "' where CONO = '" + oldId + "' ;");
                        dbStr.Append("update CSGRPD set CONO = '" + newId + "' where CONO = '" + oldId + "' ;");
                        dbStr.Append("update CSINV set CONO = '" + newId + "' where CONO = '" + oldId + "' ;");
                        dbStr.Append("update CSJOBM set CONO = '" + newId + "' where CONO = '" + oldId + "' ;");
                        dbStr.Append("update CSLDG set CONO = '" + newId + "' where CONO = '" + oldId + "' ;");
                        dbStr.Append("update CSOFF set CONO = '" + newId + "' where CONO = '" + oldId + "' ;");
                        dbStr.Append("update CSPRF set CONO = '" + newId + "' where CONO = '" + oldId + "' ;");
                        dbStr.Append("update CSPYM set CONO = '" + newId + "' where CONO = '" + oldId + "' ;");
                        dbStr.Append("update CSQT set CONO = '" + newId + "' where CONO = '" + oldId + "' ;");
                        dbStr.Append("update CSRCP set CONO = '" + newId + "' where CONO = '" + oldId + "' ;");
                        dbStr.Append("update CSRF set CONO = '" + newId + "' where CONO = '" + oldId + "' ;");
                        dbStr.Append("update CSTRANM set CONO = '" + newId + "' where CONO = '" + oldId + "' ;");
                        dbStr.Append("update EMTRA set CONO = '" + newId + "' where CONO = '" + oldId + "' ;");
                        dbStr.Append("update EMTRCD set CONO = '" + newId + "' where CONO = '" + oldId + "' ;");
                        dbStr.Append("update EMTRCM set CONO = '" + newId + "' where CONO = '" + oldId + "' ;");
                        dbStr.Append("update EMTRSD set CONO = '" + newId + "' where CONO = '" + oldId + "' ;");
                        dbStr.Append("update EMTRSM set CONO = '" + newId + "' where CONO = '" + oldId + "' ;");
                        dbStr.Append("update EMTRTD set CONO = '" + newId + "' where CONO = '" + oldId + "' ;");
                        dbStr.Append("update EMTRTM set CONO = '" + newId + "' where CONO = '" + oldId + "' ;");
                        dbStr.Append("update UTEMAILGD set CONO = '" + newId + "' where CONO = '" + oldId + "' ;");
                        dbStr.Append("update SRSGRIM set CONO = '" + newId + "' where CONO = '" + oldId + "' ;");
                        dbStr.Append("update SRSGPPM set CONO = '" + newId + "' where CONO = '" + oldId + "' ;");
                        dbStr.Append("update SFSGSCMVD2 set CONO = '" + newId + "' where CONO = '" + oldId + "' ;");
                        dbStr.Append("update SFSGSCMV set CONO = '" + newId + "' where CONO = '" + oldId + "' ;");
                        dbStr.Append("update SFSGNEW set CONO = '" + newId + "' where CONO = '" + oldId + "' ;");
                        dbStr.Append("update SFSGNAMECHG set CONO = '" + newId + "' where CONO = '" + oldId + "' ;");
                        dbStr.Append("update SFSGDRMVD2 set CONO = '" + newId + "' where CONO = '" + oldId + "' ;");
                        dbStr.Append("update SFSGDRMV set CONO = '" + newId + "' where CONO = '" + oldId + "' ;");
                        dbStr.Append("update SFSGADDRCHG set CONO = '" + newId + "' where CONO = '" + oldId + "' ;");
                        dbStr.Append("update SFMYSCMVD2 set CONO = '" + newId + "' where CONO = '" + oldId + "' ;");
                        dbStr.Append("update SFMYSCMV set CONO = '" + newId + "' where CONO = '" + oldId + "' ;");
                        dbStr.Append("update SFMYNEW set CONO = '" + newId + "' where CONO = '" + oldId + "' ;");
                        dbStr.Append("update SFMYNAMECHG set CONO = '" + newId + "' where CONO = '" + oldId + "' ;");
                        dbStr.Append("update SFMYADDRCHG set CONO = '" + newId + "' where CONO = '" + oldId + "' ;");
                        dbStr.Append("update CSCNM set CONO = '" + newId + "' where CONO = '" + oldId + "' ;");
                        dbStr.Append("update CSCANC3P set CONO = '" + newId + "' where CONO = '" + oldId + "' ;");
                        dbStr.Append("update CSBILL set CONO = '" + newId + "' where CONO = '" + oldId + "' ;");
                        dbStr.Append("update CSCNM set CONO = '" + newId + "' where CONO = '" + oldId + "' ;");
                        dbStr.Append("update CICL set CLCODE = '" + newId + "' where CLCODE = '" + oldId + "' ;");
                        dbStr.Append("update CICLADDR set CLCODE = '" + newId + "' where CLCODE = '" + oldId + "' ;");
                        dbStr.Append("update CICLPIC set CLCODE = '" + newId + "' where CLCODE = '" + oldId + "' ;");
                        dbStr.Append("update CICLREG set CLCODE = '" + newId + "' where CLCODE = '" + oldId + "' ;");
                        dbStr.Append("delete from CSCOADDR where CONO = '" + oldId + "'; ");
                        string dbScript = dbStr.ToString();
                        FbScript cmd = new FbScript(dbScript);
                        cmd.Parse();
                        fbe.AppendSqlStatements(cmd);
                    }
                    try
                    {
                        fbe.Execute();
                        cSCOMSTR = db.CSCOMSTRs.Find(oldId);
                        db.CSCOMSTRs.Remove(cSCOMSTR);
                        db.SaveChanges();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                    finally
                    {
                        con.Close();
                    }
                    return View("Details", cSCOMSTRNew);

                };

            }
            return View("Details", cSCOMSTR); // will definitely return error becuase record is null

        }

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
            ViewBag.COSTAT = new SelectList(db.CSSTATs, "COSTAT", "COSTAT");
            return PartialView("Partial/Search", searchRec);
        }

        [HttpGet]
        public ActionResult SearchPost()
        {
            return Index(1);
        }

        [HttpPost]
        public ActionResult SearchPost([Bind(Include = "COREGNO,CONAME,STAFFCODE,COSTAT, filter")] CSCOMSTR cSCOMSTR)
        {

            Session["SearchRec"] = cSCOMSTR;
            return Redirect("?page=1");
            //return Index(1);
        }

        public ActionResult CompanyList(string year, string month, string day, string status)
        {
            FbContext fbcon = FbContext.Create();
            int yearno = 0;
            if (!string.IsNullOrEmpty(year)) { yearno = Int32.Parse(year); }
            int monthno = 0;
            if (!string.IsNullOrEmpty(month)) { monthno = Int32.Parse(month); }
            int dayno = 0;
            if (!string.IsNullOrEmpty(day)) { dayno = Int32.Parse(day); }

            if (Session["RPT_START"] == null) { Session["RPT_START"] = DateTime.Today; }
            if (Session["RPT_END"] == null) { Session["RPT_END"] = DateTime.Today; }

            ViewBag.RPT_START = Session["RPT_START"];
            ViewBag.RPT_END = Session["RPT_END"];
            ViewBag.page = 1;

            CSCOMSTR searchRec = null;
            if (Session["SearchRec"] != null)
            {
                searchRec = (CSCOMSTR)Session["SearchRec"];
            }
            else
            {
                searchRec = new CSCOMSTR();
            }
            searchRec.filter = true;
            Session["SearchRec"] = searchRec;
            Session["CSCOMSTR_CONOLIST"] = fbcon.GetCostatConolist(yearno, monthno, dayno, status);

            //IEnumerable<CSCOMSTR> curRecs = fbcon.PopulateCostatColist(yearno, monthno, dayno, status);
            return Index(1);
        }

        public ActionResult CompanyListTaxAgent(string TaxAgent)
        {
            FbContext fbcon = FbContext.Create();

            if (Session["RPT_START"] == null) { Session["RPT_START"] = DateTime.Today; }
            if (Session["RPT_END"] == null) { Session["RPT_END"] = DateTime.Today; }

            ViewBag.RPT_START = Session["RPT_START"];
            ViewBag.RPT_END = Session["RPT_END"];
            ViewBag.page = 1;

            CSCOMSTR searchRec = null;
            if (Session["SearchRec"] != null)
            {
                searchRec = (CSCOMSTR)Session["SearchRec"];
            }
            else
            {
                searchRec = new CSCOMSTR();
            }
            searchRec.filter = true;
            Session["SearchRec"] = searchRec;
            Session["CSCOMSTR_CONOLIST"] = fbcon.GetTaxAgentConolist(MyHtmlHelpers.ConvertByteStrToId(TaxAgent));

            //IEnumerable<CSCOMSTR> curRecs = fbcon.PopulateCostatColist(yearno, monthno, dayno, status);
            return Index(1);
        }

        public ActionResult CompanyListAuditor(string Auditor)
        {
            FbContext fbcon = FbContext.Create();

            if (Session["RPT_START"] == null) { Session["RPT_START"] = DateTime.Today; }
            if (Session["RPT_END"] == null) { Session["RPT_END"] = DateTime.Today; }

            ViewBag.RPT_START = Session["RPT_START"];
            ViewBag.RPT_END = Session["RPT_END"];
            ViewBag.page = 1;

            CSCOMSTR searchRec = null;
            if (Session["SearchRec"] != null)
            {
                searchRec = (CSCOMSTR)Session["SearchRec"];
            }
            else
            {
                searchRec = new CSCOMSTR();
            }
            searchRec.filter = true;
            Session["SearchRec"] = searchRec;
            Session["CSCOMSTR_CONOLIST"] = fbcon.GetAuditorConolist(MyHtmlHelpers.ConvertByteStrToId(Auditor));

            //IEnumerable<CSCOMSTR> curRecs = fbcon.PopulateCostatColist(yearno, monthno, dayno, status);
            return Index(1);
        }


        public ActionResult IndexList(IEnumerable<CSCOMSTR> csRecs)
        {
            return View("Index", csRecs.ToList().ToPagedList(1, csRecs.Count()));
        }


        public IEnumerable<CSCOMSTR> CurrentSelection()
        {
            string pSearchCode = "";
            string pSearchName = "";
            string pSearchStaff = "";
            string pSearchStatus = "";
            bool pSearchFiltered = false;

            if (Session["SearchRec"] != null)
            {
                CSCOMSTR searchRec = (CSCOMSTR)(Session["SearchRec"]);
                pSearchCode = searchRec.COREGNO ?? "";
                pSearchName = searchRec.CONAME ?? "";
                pSearchStaff = searchRec.STAFFCODE ?? "";
                pSearchStatus = searchRec.COSTAT ?? "";
                pSearchFiltered = searchRec.filter;
            }

            IQueryable<CSCOMSTR> cSCOMSTRs = db.CSCOMSTRs;
            if (pSearchCode != "")
            {
                cSCOMSTRs = cSCOMSTRs.Where(x => x.COREGNO.Contains(pSearchCode));
            };
            if (pSearchName != "") { cSCOMSTRs = cSCOMSTRs.Where(x => x.CONAME.ToUpper().Contains(pSearchName.ToUpper())); };
            if (pSearchStaff != "") { cSCOMSTRs = cSCOMSTRs.Where(x => x.STAFFCODE == pSearchStaff); };
            if (pSearchStatus != "") { cSCOMSTRs = cSCOMSTRs.Where(x => x.COSTAT == pSearchStatus); };

            if (Session["RPT_START"] == null) { Session["RPT_START"] = DateTime.Today; }
            if (Session["RPT_END"] == null) { Session["RPT_END"] = DateTime.Today; }

            ViewBag.RPT_START = Session["RPT_START"];
            ViewBag.RPT_END = Session["RPT_END"];

            if (pSearchFiltered && (Session["CSCOMSTR_CONOLIST"] != null))
            {
                IList<string> conoList = (IList<string>)Session["CSCOMSTR_CONOLIST"];
                cSCOMSTRs = cSCOMSTRs.Where(x => conoList.Contains(x.CONO));
            }

            cSCOMSTRs = cSCOMSTRs.OrderBy(n => n.CONAME).Include(c => c.HKCTRY).Include(c => c.HKCONST).Include(c => c.HKSTAFF);

            return cSCOMSTRs;
        }

        // GET: CSCOMSTRs
        public ActionResult Index(int? page)
        {

            ViewBag.page = page ?? 1;
            var curRec = CurrentSelection();
            return View("Index", curRec.ToList().ToPagedList(page ?? 1, 30));
            //return View("IndexNG2", cSCOMSTRs.ToList());
        }

        public ActionResult CollectionList(int? page)
        {


            Session["collectionStatusTotal"] = 0;
            Session["collectionStatusReimb"] = 0;
            Session["collectionStatusReceipt"] = 0;
            Session["collectionStatusAdvance"] = 0;
            Session["collectionStatusDeposit"] = 0;
            Session["collectionStatusOwnFeeTotal"] = 0;
            Session["collectionStatusOwnWorkTotal"] = 0;
            Session["collectionStatusOwnTaxTotal"] = 0;
            Session["collectionStatusOwnDisbTotal"] = 0;
            Session["collectionStatus3pFeeTotal"] = 0;
            Session["collectionStatus3pWorkTotal"] = 0;
            Session["collectionStatus3pTaxTotal"] = 0;
            Session["collectionStatus3pDisbTotal"] = 0;

            var curRec = CurrentSelection();
            if (page > 0)
            {
                int prevPage = (page ?? 1) - 1;
                curRec = curRec.Skip(prevPage * 30).Take(30);
            }
            return View(curRec.ToList());
        }

        public ActionResult RegisterList(string id, int? page)
        {

            var curRec = CurrentSelection();
            if (page > 0)
            {
                int prevPage = (page ?? 1) - 1;
                curRec = curRec.Skip(prevPage * 30).Take(30);
            }
            ViewBag.RptType = id;
            return View(curRec.ToList());
        }



        // GET: CSCOMSTRs/Details/5
        public ActionResult Register(string id)
        {
            string sid = MyHtmlHelpers.ConvertByteStrToId(id);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSCOMSTR cSCOMSTR = db.CSCOMSTRs.Find(sid);
            DateTime useDate = DateTime.Now;
            if (cSCOMSTR == null)
            {
                return HttpNotFound();
            }

            SACOMP profileRec = db.SACOMPs.FirstOrDefault();
            ViewBag.Company = profileRec.CONAME.ToUpper() + " (" + profileRec.COREGNO + ") ";

            ViewBag.CSCOSTAT = db.CSCOSTATs.Where(x => x.CONO == sid && x.SDATE <= useDate && x.EDATE >= useDate).FirstOrDefault();

            ViewBag.CSCOFEE = db.CSCOFEEs.Where(x => x.CONO == sid).OrderByDescending(y => y.LASTTOUCH);

            ViewBag.CSCOPIC = db.CSCOPICs.Where(x => x.CONO == sid);

            ViewBag.CSCONAME = db.CSCONAMEs.Where(x => x.CONO == sid).OrderByDescending(y => y.EFFDATE); 

            ViewBag.CSCOADDR = db.CSCOADDRs.Where(x => x.CONO == sid);

            ViewBag.CSCOAGM = db.CSCOAGMs.Where(x => x.CONO == sid).OrderByDescending(y => y.AGMNO);

            ViewBag.CSCOAR = db.CSCOARs.Where(x => x.CONO == sid).OrderByDescending(y => y.ARNO);

            ViewBag.CSCOSH = db.CSCOSHes.Where(x => x.CONO == sid).OrderByDescending(y => y.FOLIONO);

            ViewBag.CSCOADT = db.CSCOADTs.Where(x => x.CONO == sid);

            ViewBag.CSCOTX = db.CSCOTXes.Where(x => x.CONO == sid);

            //ViewBag.CSCOAK = db.CSCOAKs.Where(x => x.CONO == sid).OrderBy( x => x.ROWNO);

            //ViewBag.CSTRANM = db.CSTRANMs.Where(x => x.CONO == sid).OrderByDescending(x => x.ENTDATE);
            DateTime rptStart;
            DateTime.TryParse(Session["RPT_START"].ToString(), out rptStart);

            DateTime rptEnd;
            DateTime.TryParse(Session["RPT_END"].ToString(), out rptEnd);
            ViewBag.RPT_START = rptStart.ToString("dd/MM/yyyy");
            ViewBag.RPT_END = rptEnd.ToString("dd/MM/yyyy");
            return View(cSCOMSTR);
        }

        public ActionResult RegisterAll(string id)
        {
            string sid = MyHtmlHelpers.ConvertByteStrToId(id);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSCOMSTR cSCOMSTR = db.CSCOMSTRs.Find(sid);
            DateTime useDate = DateTime.Now;
            if (cSCOMSTR == null)
            {
                return HttpNotFound();
            }

            SACOMP profileRec = db.SACOMPs.FirstOrDefault();
            ViewBag.Company = profileRec.CONAME.ToUpper() + " (" + profileRec.COREGNO + ") ";

            ViewBag.CSCOSTAT = db.CSCOSTATs.Where(x => x.CONO == sid && x.SDATE <= useDate && x.EDATE >= useDate).FirstOrDefault();

            ViewBag.CSCOFEE = db.CSCOFEEs.Where(x => x.CONO == sid).OrderByDescending(y => y.LASTTOUCH);

            ViewBag.CSCOPIC = db.CSCOPICs.Where(x => x.CONO == sid);

            ViewBag.CSCONAME = db.CSCONAMEs.Where(x => x.CONO == sid).OrderByDescending(y => y.EFFDATE);

            ViewBag.CSCOADDR = db.CSCOADDRs.Where(x => x.CONO == sid);

            ViewBag.CSCOAGM = db.CSCOAGMs.Where(x => x.CONO == sid).OrderByDescending(y => y.AGMNO);

            ViewBag.CSCOAR = db.CSCOARs.Where(x => x.CONO == sid).OrderByDescending(y => y.ARNO);

            ViewBag.CSCOSH = db.CSCOSHes.Where(x => x.CONO == sid).OrderByDescending(y => y.FOLIONO);

            ViewBag.CSCOADT = db.CSCOADTs.Where(x => x.CONO == sid);

            ViewBag.CSCOTX = db.CSCOTXes.Where(x => x.CONO == sid);

            ViewBag.CSCOAK = db.CSCOAKs.Where(x => x.CONO == sid).OrderBy(x => x.ROWNO);
            DateTime rptStart;
            DateTime.TryParse(Session["RPT_START"].ToString(), out rptStart);

            DateTime rptEnd;
            DateTime.TryParse(Session["RPT_END"].ToString(), out rptEnd);

            ViewBag.CSTRANM = db.CSTRANMs.Where(x => x.SOURCE != "CSRCP-3P" && x.SOURCE != "CSPYM" && x.CONO == sid && x.TROS < 0 && x.ENTDATE >= rptStart && x.ENTDATE <= rptEnd).OrderByDescending(x => x.ENTDATE);

            ViewBag.V_CSTRAN_OSTD = db.V_CSTRAN_OSTD.Where(x => x.SOURCE != "CSRCP-3P" && x.SOURCE != "CSPYM" && x.CONO == sid && x.TROS < 0 && x.ENTDATE >= rptStart && x.ENTDATE <= rptEnd).OrderBy(x => x.ENTDATE);

            ViewBag.V_CSTRAN_OSTD1 = db.V_CSTRAN_OSTD.Where(x => x.SOURCE != "CSRCP-3P" && x.SOURCE != "CSPYM" && x.CONO == sid && x.TROS > 0 && x.ENTDATE >= rptStart && x.ENTDATE <= rptEnd).OrderBy(x => x.ENTDATE);

            //ViewBag.CSJOB = db.CSJOBMs.Where(x => x.CONO == sid);

            //ViewBag.CSLDG = db.CSLDGs.Where( x => x.CONO == sid).OrderByDescending(x => x.ENTDATE).ThenBy(x => x.SOURCENO).ThenBy(x => x.SOURCEID);


            ViewBag.RPT_START = rptStart.ToString("dd/MM/yyyy");
            ViewBag.RPT_END = rptEnd.ToString("dd/MM/yyyy");
            return View("Register", cSCOMSTR);
        }

        public ActionResult RegisterActivity(string id)
        {
            string sid = MyHtmlHelpers.ConvertByteStrToId(id);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSCOMSTR cSCOMSTR = db.CSCOMSTRs.Find(sid);
            DateTime useDate = DateTime.Now;
            if (cSCOMSTR == null)
            {
                return HttpNotFound();
            }

            SACOMP profileRec = db.SACOMPs.FirstOrDefault();
            ViewBag.Company = profileRec.CONAME.ToUpper() + " (" + profileRec.COREGNO + ") ";
            DateTime rptStart;
            DateTime.TryParse(Session["RPT_START"].ToString(), out rptStart);

            DateTime rptEnd;
            DateTime.TryParse(Session["RPT_END"].ToString(), out rptEnd);

            ViewBag.HEAD = "SIMPLE";
            ViewBag.RPT_START = rptStart.ToString("dd/MM/yyyy");
            ViewBag.RPT_END = rptEnd.ToString("dd/MM/yyyy");
            ViewBag.CSTRANM = db.CSTRANMs.Where(x => x.SOURCE != "CSRCP-3P" && x.SOURCE != "CSPYM" && x.CONO == sid && x.ENTDATE >= rptStart && x.ENTDATE <= rptEnd).OrderByDescending(x => x.ENTDATE);

            return View("Register", cSCOMSTR);
        }

        public ActionResult Outstanding(string id)
        {
            string sid = MyHtmlHelpers.ConvertByteStrToId(id);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSCOMSTR cSCOMSTR = db.CSCOMSTRs.Find(sid);
            DateTime useDate = DateTime.Now;
            if (cSCOMSTR == null)
            {
                return HttpNotFound();
            }

            SACOMP profileRec = db.SACOMPs.FirstOrDefault();
            ViewBag.Company = profileRec.CONAME.ToUpper() + " (" + profileRec.COREGNO + ") ";
            DateTime rptStart;
            DateTime.TryParse(Session["RPT_START"].ToString(), out rptStart);

            DateTime rptEnd;
            DateTime.TryParse(Session["RPT_END"].ToString(), out rptEnd);
            ViewBag.RPT_START = rptStart.ToString("dd/MM/yyyy");
            ViewBag.RPT_END = rptEnd.ToString("dd/MM/yyyy");
            ViewBag.HEAD = "SIMPLE";
           // ViewBag.V_CSTRAN_OSTD = db.V_CSTRAN_OSTD.Where(x => x.SOURCE != "CSRCP-3P" && x.SOURCE != "CSPYM" && x.CONO == sid && x.TROS < 0 && x.ENTDATE >= rptStart && x.ENTDATE <= rptEnd).OrderBy(x => x.ENTDATE);

            ViewBag.V_CSTRAN_OSTD1 = db.V_CSTRAN_OSTD.Where(x => x.SOURCE != "CSRCP-3P" && x.SOURCE != "CSPYM" && x.CONO == sid && x.TROS > 0 && x.ENTDATE >= rptStart && x.ENTDATE <= rptEnd).OrderBy(x => x.ENTDATE);

            return View("Register", cSCOMSTR);
        }

        public ActionResult Outstanding1(string id)
        {
            string sid = MyHtmlHelpers.ConvertByteStrToId(id);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSCOMSTR cSCOMSTR = db.CSCOMSTRs.Find(sid);
            DateTime useDate = DateTime.Now;
            if (cSCOMSTR == null)
            {
                return HttpNotFound();
            }

            SACOMP profileRec = db.SACOMPs.FirstOrDefault();
            ViewBag.Company = profileRec.CONAME.ToUpper() + " (" + profileRec.COREGNO + ") ";
            DateTime rptStart;
            DateTime.TryParse(Session["RPT_START"].ToString(), out rptStart);

            DateTime rptEnd;
            DateTime.TryParse(Session["RPT_END"].ToString(), out rptEnd);
            ViewBag.RPT_START = rptStart.ToString("dd/MM/yyyy");
            ViewBag.RPT_END = rptEnd.ToString("dd/MM/yyyy");
            ViewBag.HEAD = "SIMPLE";
            ViewBag.V_CSTRAN_OSTD = db.V_CSTRAN_OSTD.Where(x => x.SOURCE != "CSRCP-3P" && x.SOURCE != "CSPYM" && x.CONO == sid && x.TROS < 0 && x.ENTDATE >= rptStart && x.ENTDATE <= rptEnd).OrderBy(x => x.ENTDATE);

            //ViewBag.V_CSTRAN_OSTD1 = db.V_CSTRAN_OSTD.Where(x => x.SOURCE != "CSRCP-3P" && x.SOURCE != "CSPYM" && x.CONO == sid && x.TROS > 0 && x.ENTDATE >= rptStart && x.ENTDATE <= rptEnd).OrderBy(x => x.ENTDATE);

            return View("Register", cSCOMSTR);
        }

        public ActionResult CollectionView(string id)
        {
            string sid = MyHtmlHelpers.ConvertByteStrToId(id);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (Session["RPT_START"] == null) { Session["RPT_START"] = DateTime.Today; }
            if (Session["RPT_END"] == null) { Session["RPT_END"] = DateTime.Today; }

            DateTime rptStart;
            DateTime.TryParse(Session["RPT_START"].ToString(), out rptStart);

            DateTime rptEnd;
            DateTime.TryParse(Session["RPT_END"].ToString(), out rptEnd);

            ViewBag.RPT_START = rptStart.ToString("dd/MM/yyyy");
            ViewBag.RPT_END = rptEnd.ToString("dd/MM/yyyy");

            CSCOMSTR profileRec = db.CSCOMSTRs.Find(sid);
            ViewBag.Company = profileRec.CONAME.ToUpper() + " (" + profileRec.COREGNO + ") ";
            IQueryable<CSTRANM> cSTRANM = db.CSTRANMs.Where(x => x.CONO == sid).OrderByDescending(x => x.ENTDATE);
            ViewBag.CSTRANM = cSTRANM;
            ViewBag.CSJOB = db.CSJOBMs.Where(x => x.CONO == sid);

            IQueryable<CSLDG> cSLDG = db.CSLDGs.Where(x => x.CONO == sid && x.ENTDATE >= rptStart && x.ENTDATE <= rptEnd).OrderByDescending(x => x.ENTDATE).ThenBy(x => x.SOURCENO).ThenBy(x => x.SOURCEID);

            return View(cSLDG);
        }

        public ActionResult Collection(string id)
        {
            string sid = MyHtmlHelpers.ConvertByteStrToId(id);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSCOMSTR cSCOMSTR = db.CSCOMSTRs.Find(sid);
            DateTime useDate = DateTime.Now;
            if (cSCOMSTR == null)
            {
                return HttpNotFound();
            }

            DateTime rptStart;
            DateTime.TryParse(Session["RPT_START"].ToString(), out rptStart);

            DateTime rptEnd;
            DateTime.TryParse(Session["RPT_END"].ToString(), out rptEnd);

            ViewBag.RPT_START = rptStart.ToString("dd/MM/yyyy");
            ViewBag.RPT_END = rptEnd.ToString("dd/MM/yyyy");

            SACOMP profileRec = db.SACOMPs.FirstOrDefault();
            ViewBag.Company = profileRec.CONAME.ToUpper() + " (" + profileRec.COREGNO + ") ";

            ViewBag.CSTRANM = db.CSTRANMs.Where(x => x.CONO == sid).OrderByDescending(x => x.ENTDATE);
            ViewBag.CSJOB = db.CSJOBMs.Where(x => x.CONO == sid);

            ViewBag.CSLDG = db.CSLDGs.Where(x => x.CONO == sid && x.ENTDATE >= rptStart && x.ENTDATE <= rptEnd).OrderByDescending(x => x.ENTDATE).ThenBy(x => x.SOURCENO).ThenBy(x => x.SOURCEID);

            return View(cSCOMSTR);
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
            ViewBag.Title = "View Company Information";
            ViewBag.INCCTRY = new SelectList(db.HKCTRies, "CTRYCODE", "CTRYDESC", cSCOMSTR.INCCTRY);
            ViewBag.CONSTCODE = new SelectList(db.HKCONSTs, "CONSTCODE", "CONSTDESC", cSCOMSTR.CONSTCODE);
            ViewBag.STAFFCODE = new SelectList(db.HKSTAFFs, "STAFFCODE", "STAFFDESC", cSCOMSTR.STAFFCODE);
            ViewBag.INTYPE = new SelectList(
new List<SelectListItem>
{
        new SelectListItem { Text = "Own", Value = "Own"},
        new SelectListItem { Text = "TakeOver", Value ="TakeOver" },
}, "Value", "Text");

            return View("EditNG", cSCOMSTR);
        }

        // GET: CSCOMSTRs/Create
        public ActionResult Create()
        {
            CSCOMSTR cSCOMSTR = new CSCOMSTR();
            ViewBag.INCCTRY = new SelectList(db.HKCTRies, "CTRYCODE", "CTRYDESC", "MY");
            ViewBag.CONSTCODE = new SelectList(db.HKCONSTs, "CONSTCODE", "CONSTDESC");
            ViewBag.STAFFCODE = new SelectList(db.HKSTAFFs, "STAFFCODE", "STAFFDESC");
            ViewBag.INTYPE = new SelectList(
    new List<SelectListItem>
    {
        new SelectListItem { Text = "Own", Value = "Own"},
        new SelectListItem { Text = "TakeOver", Value ="TakeOver" },
    }, "Value", "Text");

            cSCOMSTR.INCDATE = DateTime.Today;
            cSCOMSTR.STAMP = 0; // set initial stamp
            cSCOMSTR.SEQNO = 1; // set inital seqno
            cSCOMSTR.ARRE = "N";
            cSCOMSTR.SPECIALRE = "N";
            cSCOMSTR.COSTAT = "Active";
            cSCOMSTR.COSTATD = DateTime.Today;
            cSCOMSTR.FILETYPE = "";
            cSCOMSTR.FILELOC = "";
            cSCOMSTR.SEALLOC = "";

            ViewBag.Title = "Create Company Information";
            return View("EditNG", cSCOMSTR);
        }

        // POST: CSCOMSTRs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CONO,COREGNO,INCDATE,INCCTRY,CONSTCODE,INTYPE,PRINOBJ,PRINOBJStr,CONAME,PINDCODE,SINDCODE,WEB,COSTAT,COSTATD,FILETYPE,FILELOC,SEALLOC,STAFFCODE,SPECIALRE,SPECIALREBool,ARRE,ARREBool,CMMT,CMMTStr,REM,SXCODE,SXNAME,REFCODE,SEQNO,STAMP")] CSCOMSTR cSCOMSTR)
        {
            if (ModelState.IsValid)
            {
                cSCOMSTR.STAMP = 1; // set initial stamp
                cSCOMSTR.SEQNO = 1; // set inital seqno
                db.CSCOMSTRs.Add(cSCOMSTR);


                try
                {

                    if ((cSCOMSTR.CONO == null) && (cSCOMSTR.COREGNO.Length <= 10))
                    {
                        cSCOMSTR.CONO = cSCOMSTR.COREGNO;
                    }
                    else
                    {
                        SALASTNO serialTbl = db.SALASTNOes.Find("SFNAMECHG");
                        if (serialTbl != null)
                        {

                            string prefix = serialTbl.LASTPFIX ?? "!";
                            int MaxNo = serialTbl.LASTNOMAX;
                            bool AutoGen = serialTbl.AUTOGEN == "Y";
                            serialTbl.LASTNO = serialTbl.LASTNO + 1;
                            cSCOMSTR.CONO = prefix + serialTbl.LASTNO.ToString("D9");

                            serialTbl.STAMP = serialTbl.STAMP + 1;
                            db.Entry(serialTbl).State = EntityState.Modified;

                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "Unable to create company no");
                        }
                    }

                    CICL cICL = db.CICLs.Find(cSCOMSTR.CONO);

                    if (cICL == null)
                    {
                        cICL = new CICL();
                        cICL.CLCODE = cSCOMSTR.CONO;
                        cICL.STAMP = -1;
                        db.CICLs.Add(cICL);
                    }
                    else
                    {
                        db.Entry(cICL).State = EntityState.Modified;
                    }

                    cICL.CONSTCODE = cSCOMSTR.CONSTCODE;
                    cICL.CLNAME = cSCOMSTR.CONAME;
                    cICL.CLNAME = cSCOMSTR.CONAME;
                    cICL.BILLNAME = cSCOMSTR.CONAME;
                    cICL.SYSGEN = "N";
                    cICL.STAMP = cICL.STAMP + 1;

                    CreateLastNo(cSCOMSTR);

                    db.SaveChanges();
                    return RedirectToAction("Edit", "CSCOMSTRs", new { id = MyHtmlHelpers.ConvertIdToByteStr(cSCOMSTR.CONO) });
                    //return RedirectToAction("Index");
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
                    cSCOMSTR.STAMP = 0;
                    cSCOMSTR.SEQNO = 0;
                    ModelState.AddModelError(string.Empty, e.Message);
                }
            }

            ViewBag.INCCTRY = new SelectList(db.HKCTRies, "CTRYCODE", "CTRYDESC", cSCOMSTR.INCCTRY);
            ViewBag.CONSTCODE = new SelectList(db.HKCONSTs, "CONSTCODE", "CONSTDESC", cSCOMSTR.CONSTCODE);
            ViewBag.STAFFCODE = new SelectList(db.HKSTAFFs, "STAFFCODE", "STAFFDESC", cSCOMSTR.STAFFCODE);
            ViewBag.INTYPE = new SelectList(
 new List<SelectListItem>
 {
        new SelectListItem { Text = "Own", Value = "Own"},
        new SelectListItem { Text = "TakeOver", Value ="TakeOver" },
 }, "Value", "Text");

            ViewBag.Title = "Create Company Information";
            return View("EditNG", cSCOMSTR);
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
            ViewBag.INTYPE = new SelectList(
new List<SelectListItem>
{
        new SelectListItem { Text = "Own", Value = "Own"},
        new SelectListItem { Text = "TakeOver", Value ="TakeOver" },
}, "Value", "Text");

            ViewBag.Title = "Edit Company Information";
            return View("EditNG", cSCOMSTR);
        }

        // POST: CSCOMSTRs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CONO,COREGNO,INCDATE,INCCTRY,CONSTCODE,INTYPE,PRINOBJ,PRINOBJStr,CONAME,PINDCODE,SINDCODE,WEB,COSTAT,COSTATD,FILETYPE,FILELOC,SEALLOC,STAFFCODE,SPECIALRE,SPECIALREBool,ARRE,ARREBool,CMMT,CMMTStr,REM,SXCODE,SXNAME,REFCODE,SEQNO,STAMP")] CSCOMSTR cSCOMSTR)
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

                        CICL cICL = db.CICLs.Find(cSCOMSTR.CONO);

                        if (cICL == null)
                        {
                            cICL = new CICL();
                            cICL.CLCODE = cSCOMSTR.CONO;
                            cICL.STAMP = -1;
                            db.CICLs.Add(cICL);
                        }
                        else
                        {
                            db.Entry(cICL).State = EntityState.Modified;
                        }

                        cICL.CONSTCODE = cSCOMSTR.CONSTCODE;
                        cICL.CLNAME = cSCOMSTR.CONAME;
                        cICL.CLNAME = cSCOMSTR.CONAME;
                        cICL.BILLNAME = cSCOMSTR.CONAME;
                        cICL.SYSGEN = "N";
                        cICL.STAMP = cICL.STAMP + 1;

                        CreateLastNo(cSCOMSTR);

                        db.SaveChanges();
                        return RedirectToAction("Index");
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
            ViewBag.INCCTRY = new SelectList(db.HKCTRies, "CTRYCODE", "CTRYDESC", cSCOMSTR.INCCTRY);
            ViewBag.CONSTCODE = new SelectList(db.HKCONSTs, "CONSTCODE", "CONSTDESC", cSCOMSTR.CONSTCODE);
            ViewBag.STAFFCODE = new SelectList(db.HKSTAFFs, "STAFFCODE", "STAFFDESC", cSCOMSTR.STAFFCODE);
            ViewBag.INTYPE = new SelectList(
new List<SelectListItem>
{
        new SelectListItem { Text = "Own", Value = "Own"},
        new SelectListItem { Text = "TakeOver", Value ="TakeOver" },
}, "Value", "Text");
            ViewBag.Title = "Edit Company Information";
            return View("EditNG", cSCOMSTR);
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
            ViewBag.INTYPE = new SelectList(
new List<SelectListItem>
{
        new SelectListItem { Text = "Own", Value = "Own"},
        new SelectListItem { Text = "TakeOver", Value ="TakeOver" },
}, "Value", "Text");
            ViewBag.Title = "Delete Company Information";
            return View("EditNG", cSCOMSTR);
        }

        // POST: CSCOMSTRs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            CSCOMSTR cSCOMSTR = db.CSCOMSTRs.Find(MyHtmlHelpers.ConvertByteStrToId(id));
            try
            {
                db.CSCOMSTRs.Remove(cSCOMSTR);
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

            ViewBag.INCCTRY = new SelectList(db.HKCTRies, "CTRYCODE", "CTRYDESC", cSCOMSTR.INCCTRY);
            ViewBag.CONSTCODE = new SelectList(db.HKCONSTs, "CONSTCODE", "CONSTDESC", cSCOMSTR.CONSTCODE);
            ViewBag.STAFFCODE = new SelectList(db.HKSTAFFs, "STAFFCODE", "STAFFDESC", cSCOMSTR.STAFFCODE);
            ViewBag.INTYPE = new SelectList(
new List<SelectListItem>
{
        new SelectListItem { Text = "Own", Value = "Own"},
        new SelectListItem { Text = "TakeOver", Value ="TakeOver" },
}, "Value", "Text");
            ViewBag.Title = "Delete Company Information";
            return View("EditNG", cSCOMSTR);
        }

        protected void CreateLastNo(CSCOMSTR cSCOMSTR)
        {

            CSCOLASTNO cSCOLASTNO1 = db.CSCOLASTNOes.Find(cSCOMSTR.CONO, "CERTNO");
            if (cSCOLASTNO1 == null)
            {
                cSCOLASTNO1 = new WebApplication1.CSCOLASTNO();

                cSCOLASTNO1.CONO = cSCOMSTR.CONO;
                cSCOLASTNO1.LASTCODE = "CERTNO";
                cSCOLASTNO1.LASTDESC = "Share Certificate No.";
                cSCOLASTNO1.LASTPFIX = "";
                cSCOLASTNO1.LASTNO = 0;
                cSCOLASTNO1.LASTWD = 9;
                cSCOLASTNO1.AUTOGEN = "Y";
                cSCOLASTNO1.STAMP = 0;
                db.CSCOLASTNOes.Add(cSCOLASTNO1);
            }

            CSCOLASTNO cSCOLASTNO2 = db.CSCOLASTNOes.Find(cSCOMSTR.CONO, "SHALLOT");
            if (cSCOLASTNO2 == null)
            {
                cSCOLASTNO2 = new WebApplication1.CSCOLASTNO();
                cSCOLASTNO2.CONO = cSCOMSTR.CONO;
                cSCOLASTNO2.LASTCODE = "SHALLOT";
                cSCOLASTNO2.LASTDESC = "Equity Allotment No.";
                cSCOLASTNO2.LASTPFIX = "A";
                cSCOLASTNO2.LASTNO = 0;
                cSCOLASTNO2.LASTWD = 5;
                cSCOLASTNO2.AUTOGEN = "Y";
                cSCOLASTNO2.STAMP = 0;
                db.CSCOLASTNOes.Add(cSCOLASTNO2);
            }

            CSCOLASTNO cSCOLASTNO3 = db.CSCOLASTNOes.Find(cSCOMSTR.CONO, "SHCONV");
            if (cSCOLASTNO3 == null)
            {
                cSCOLASTNO3 = new WebApplication1.CSCOLASTNO();
                cSCOLASTNO3.CONO = cSCOMSTR.CONO;
                cSCOLASTNO3.LASTCODE = "SHCONV";
                cSCOLASTNO3.LASTDESC = "Equity Conversion No.";
                cSCOLASTNO3.LASTPFIX = "C";
                cSCOLASTNO3.LASTNO = 0;
                cSCOLASTNO3.LASTWD = 5;
                cSCOLASTNO3.AUTOGEN = "Y";
                cSCOLASTNO3.STAMP = 0;
                db.CSCOLASTNOes.Add(cSCOLASTNO3);
            }

            CSCOLASTNO cSCOLASTNO4 = db.CSCOLASTNOes.Find(cSCOMSTR.CONO, "SHFOLIO");
            if (cSCOLASTNO4 == null)
            {
                cSCOLASTNO4 = new WebApplication1.CSCOLASTNO();
                cSCOLASTNO4.CONO = cSCOMSTR.CONO;

                cSCOLASTNO4.LASTCODE = "SHFOLIO";
                cSCOLASTNO4.LASTDESC = "Member Folio No.";
                cSCOLASTNO4.LASTPFIX = "";
                cSCOLASTNO4.LASTNO = 0;
                cSCOLASTNO4.LASTWD = 9;
                cSCOLASTNO4.AUTOGEN = "Y";
                cSCOLASTNO4.STAMP = 0;
                db.CSCOLASTNOes.Add(cSCOLASTNO4);
            }

            CSCOLASTNO cSCOLASTNO5 = db.CSCOLASTNOes.Find(cSCOMSTR.CONO, "SHSPLIT");
            if (cSCOLASTNO5 == null)
            {
                cSCOLASTNO5 = new WebApplication1.CSCOLASTNO();
                cSCOLASTNO5.CONO = cSCOMSTR.CONO;
                cSCOLASTNO5.LASTCODE = "SHSPLIT";
                cSCOLASTNO5.LASTDESC = "Equity Split No.";
                cSCOLASTNO5.LASTPFIX = "S";
                cSCOLASTNO5.LASTNO = 0;
                cSCOLASTNO5.LASTWD = 5;
                cSCOLASTNO5.AUTOGEN = "Y";
                cSCOLASTNO5.STAMP = 0;
                db.CSCOLASTNOes.Add(cSCOLASTNO5);
            }

            CSCOLASTNO cSCOLASTNO6 = db.CSCOLASTNOes.Find(cSCOMSTR.CONO, "SHTRF");
            if (cSCOLASTNO6 == null)
            {
                cSCOLASTNO6 = new WebApplication1.CSCOLASTNO();
                cSCOLASTNO6.CONO = cSCOMSTR.CONO;
                cSCOLASTNO6.LASTCODE = "SHTRF";
                cSCOLASTNO6.LASTDESC = "Equity Transfer No.";
                cSCOLASTNO6.LASTPFIX = "T";
                cSCOLASTNO6.LASTNO = 0;
                cSCOLASTNO6.LASTWD = 5;
                cSCOLASTNO6.AUTOGEN = "Y";
                cSCOLASTNO6.STAMP = 0;
                db.CSCOLASTNOes.Add(cSCOLASTNO6);
            }
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

        public PartialViewResult PartialCSCOADDRs(string id)
        {
            string sid = MyHtmlHelpers.ConvertByteStrToId(id);
            ViewBag.Parent = sid;
            var csCOADDRR = db.CSCOADDRs.Where(x => x.CONO == sid).OrderByDescending(y => y.ADDRID).ToList();
            return PartialView("Partial/PartialCSCOADDRs", csCOADDRR);
        }

        public PartialViewResult PartialCSCOADTs(string id)
        {
            string sid = MyHtmlHelpers.ConvertByteStrToId(id);
            ViewBag.Parent = sid;
            var csCOADTR = db.CSCOADTs.Where(x => x.CONO == sid).ToList();
            return PartialView("Partial/PartialCSCOADTs", csCOADTR);
        }

        public PartialViewResult PartialCSCOAGTs(string id)
        {
            string sid = MyHtmlHelpers.ConvertByteStrToId(id);
            ViewBag.Parent = sid;
            var csCOAGTR = db.CSCOAGTs.Where(x => x.CONO == sid).ToList();
            return PartialView("Partial/PartialCSCOAGTs", csCOAGTR);
        }

        public PartialViewResult PartialCSCOAKs(string id)
        {
            string sid = MyHtmlHelpers.ConvertByteStrToId(id);
            ViewBag.Parent = sid;
            var csCOAKR = db.CSCOAKs.Where(x => x.CONO == sid).OrderByDescending(x => x.ROWNO).ToList();
            return PartialView("Partial/PartialCSCOAKs", csCOAKR);
        }


        public PartialViewResult PartialCSCOBANKs(string id)
        {
            string sid = MyHtmlHelpers.ConvertByteStrToId(id);
            ViewBag.Parent = sid;
            var csCOBANKR = db.CSCOBANKs.Where(x => x.CONO == sid).ToList();
            return PartialView("Partial/PartialCSCOBANKs", csCOBANKR);
        }

        public PartialViewResult PartialCSCOCMs(string id)
        {
            string sid = MyHtmlHelpers.ConvertByteStrToId(id);
            ViewBag.Parent = sid;
            var csCOCMR = db.CSCOCMs.Where(x => x.CONO == sid).ToList();
            return PartialView("Partial/PartialCSCOCMs", csCOCMR);
        }


        public PartialViewResult PartialCSCODEBs(string id)
        {
            string sid = MyHtmlHelpers.ConvertByteStrToId(id);
            ViewBag.Parent = sid;
            var csCODEBR = db.CSCODEBs.Where(x => x.CONO == sid).ToList();
            return PartialView("Partial/PartialCSCODEBs", csCODEBR);
        }

        public PartialViewResult PartialCSCODRs(string id)
        {
            string sid = MyHtmlHelpers.ConvertByteStrToId(id);
            ViewBag.Parent = sid;
            var csCODRR = db.CSCODRs.Where(x => x.CONO == sid).ToList();
            return PartialView("Partial/PartialCSCODRs", csCODRR);
        }

        public PartialViewResult PartialCSCOLASTNOs(string id)
        {
            string sid = MyHtmlHelpers.ConvertByteStrToId(id);
            ViewBag.Parent = sid;
            var csCOLASTNO = db.CSCOLASTNOes.Where(x => x.CONO == sid).ToList();
            return PartialView("Partial/PartialCSCOLASTNOs", csCOLASTNO);
        }

        public PartialViewResult PartialCSCOMGRs(string id)
        {
            string sid = MyHtmlHelpers.ConvertByteStrToId(id);
            ViewBag.Parent = sid;
            var csCOMGR = db.CSCOMGRs.Where(x => x.CONO == sid).ToList();
            return PartialView("Partial/PartialCSCOMGRs", csCOMGR);
        }

        public PartialViewResult PartialCSCONAMEs(string id)
        {
            string sid = MyHtmlHelpers.ConvertByteStrToId(id);
            ViewBag.Parent = sid;
            var csCONAME = db.CSCONAMEs.Where(x => x.CONO == sid).OrderByDescending(y => y.ROWNO).ToList();
            return PartialView("Partial/PartialCSCONAMEs", csCONAME);
        }

        public PartialViewResult PartialCSCOPARENTs(string id)
        {
            string sid = MyHtmlHelpers.ConvertByteStrToId(id);
            ViewBag.Parent = sid;
            var csCOPARENT = db.CSCOPARENTs.Where(x => x.CONO == sid).ToList();
            return PartialView("Partial/PartialCSCOPARENTs", csCOPARENT);
        }

        public PartialViewResult PartialCSCOPUKs(string id)
        {
            string sid = MyHtmlHelpers.ConvertByteStrToId(id);
            ViewBag.Parent = sid;
            var csCOPUK = db.CSCOPUKs.Where(x => x.CONO == sid).ToList();
            return PartialView("Partial/PartialCSCOPUKs", csCOPUK);
        }

        public PartialViewResult PartialCSCOSECs(string id)
        {
            string sid = MyHtmlHelpers.ConvertByteStrToId(id);
            ViewBag.Parent = sid;
            var csCOSEC = db.CSCOSECs.Where(x => x.CONO == sid).ToList();
            return PartialView("Partial/PartialCSCOSECs", csCOSEC);
        }

        public PartialViewResult PartialCSCOSHes(string id)
        {
            string sid = MyHtmlHelpers.ConvertByteStrToId(id);
            ViewBag.Parent = sid;
            var csCOSHe = db.CSCOSHes.Where(x => x.CONO == sid).OrderBy(x => x.FOLIONO).ToList();
            return PartialView("Partial/PartialCSCOSHes", csCOSHe);
        }

        public PartialViewResult PartialCSCOTXes(string id)
        {
            string sid = MyHtmlHelpers.ConvertByteStrToId(id);
            ViewBag.Parent = sid;
            var csCOTXe = db.CSCOTXes.Where(x => x.CONO == sid).ToList();
            return PartialView("Partial/PartialCSCOTXes", csCOTXe);
        }

        public ActionResult Listing()
        {

            if (Session["RPT_START"] == null) { Session["RPT_START"] = DateTime.Today; }
            if (Session["RPT_END"] == null) { Session["RPT_END"] = DateTime.Today; }

            ViewBag.RPT_START = Session["RPT_START"];
            ViewBag.RPT_END = Session["RPT_END"];

            SACOMP profileRec = db.SACOMPs.FirstOrDefault();
            ViewBag.Company = profileRec.CONAME.ToUpper() + " (" + profileRec.COREGNO + ") ";

            var curRec = CurrentSelection();
            return View(curRec);
        }

        [AllowAnonymous]
        public ActionResult Publish()
        {
            SACOMP profileRec = db.SACOMPs.FirstOrDefault();
            ViewBag.Company = profileRec.CONAME.ToUpper() + " (" + profileRec.COREGNO + ") ";

            if (Session["RPT_START"] == null) { Session["RPT_START"] = DateTime.Today; }
            if (Session["RPT_END"] == null) { Session["RPT_END"] = DateTime.Today; }

            DateTime rptStart;
            DateTime.TryParse(Session["RPT_START"].ToString(), out rptStart);

            DateTime rptEnd;
            DateTime.TryParse(Session["RPT_END"].ToString(), out rptEnd);

            ViewBag.RPT_START = rptStart.ToString("dd/MM/yyyy");
            ViewBag.RPT_END = rptEnd.ToString("dd/MM/yyyy");

            string[] constList = { "01", "02", "06" };
            var colist = db.CSCOMSTRs.Where(x => x.COSTAT == "Active" && x.INCDATE <= rptStart && constList.Contains(x.CONSTCODE)).OrderBy(x => x.CONAME);
            return View(colist);
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
