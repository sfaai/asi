using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using FirebirdSql.Data.FirebirdClient;
using System.Data.Entity;
using System.Text.RegularExpressions;
using PagedList;

namespace WebApplication1.Controllers
{
    [Authorize(Roles ="Administrator,CS-A/C")]
    public class ReportController : Controller
    {
        private ASIDBConnection db = new ASIDBConnection();

        private string[] prefixcol = {
            "row",
            "company_no",
            "company_name",
            "Staff",
            "costat",
            "Bill",
            "Amount",
            "Outstanding"
        };

        private string[] prefixcol1 = {
            "row",
            "JOBSTAFF",
            "JOBDESC",
            "NEW",
            "OUT",
            "AGM",
            "STATUTORY",
            "RETURN",
            "CANCEL",
            "COMPLETE",
            "WIP",
            "TOTAL"
        };

        private string[] prefixcol2 = {
            "row", "Case Code", "Case Description",
            "2001", "2002", "2003", "2004","2005", "2006", "2007", "2008","2009", "2010",
            "2011", "2012", "2013", "2014","2015", "2016", "2017", "2018","2019", "2020",
            "2021", "2022", "2023", "2024","2025", "2026", "2027", "2028","2029", "2030",
            "2031", "2032", "2033", "2034","2035", "2036"
        };

        private string[] prefixcol3 = {
            "row", "Case Code", "Case Description",
            "January", "February", "March", "April","May", "June", "July", "August","September", "October",
            "November", "December"
        };

        private string[] prefixcol4 = {
            "row", "Receipt", "Deposit",
            "Fees", "3rd Fees", "Own Tax", "3rd Tax", "Own Work","3rd Work", "Own Disbursement", "3rd Disbursement", "Own Reimburse","3rd Reimburse", "Advance"
        };

        private string[] prefixcol5 = {
            "Rpt Type",
            "January", "February", "March", "April","May", "June", "July", "August","September", "October",
            "November", "December"
        };

        private string[] prefixcol6 = {
            "Company", "Name", "Receipt", "Deposit",
            "Fees", "3rd Fees", "Own Tax", "3rd Tax", "Own Work","3rd Work", "Own Disbursement", "3rd Disbursement", "Own Reimburse","3rd Reimburse", "Advance"
        };

        private string[] prefixcol7 = {
            "Company", "Name", "Receipt", "Deposit",
            "Fees", "3rd Fees", "Own Tax", "3rd Tax", "Own Work","3rd Work", "Own Disbursement", "3rd Disbursement", "Own Reimburse","3rd Reimburse", "Advance",
            "Staff"
        };


        public PartialViewResult Search()
        {
            PrfBillAmountType searchRec = null;
            //searchRec.CONO = pSearchCode;
            //searchRec.CONAME = pSearchName;
            if (Session["SearchPrfBillAmountType"] != null)
            {
                searchRec = (PrfBillAmountType)Session["SearchPrfBillAmountType"];

            }
            else
            {
                searchRec = new PrfBillAmountType();
                searchRec.AmountType = "AMOUNT";
                searchRec.type = "MONTH";
                searchRec.category = "ITEMTYPE";
            }
            ViewBag.AmountType = searchRec.Selection;
            ViewBag.year = searchRec.SelectionYear;
            ViewBag.type = searchRec.SelectionType;
            ViewBag.month = searchRec.SelectionMonth;
            ViewBag.category = searchRec.SelectionCategory;

            return PartialView("Partial/Search", searchRec);
        }

        [HttpPost]
        public ActionResult SearchPost(PrfBillAmountType searchRec)
        {

            Session["SearchPrfBillAmountType"] = searchRec;
            if (string.IsNullOrEmpty(searchRec.year))
            {
                return PrfBillYear(searchRec.AmountType, searchRec.category);
            } else
            {
                if ((string.IsNullOrEmpty(searchRec.type)) || (searchRec.type == "MONTH"))
                {
                    if ((string.IsNullOrEmpty(searchRec.month)))
                    {
                        return PrfBillMonth(searchRec.year, searchRec.AmountType, searchRec.category);
                    }
                    else
                    {
                        return PrfBillDay(searchRec.year, searchRec.month, searchRec.AmountType, searchRec.category);
                    }
                  
                } else
                {
                    if ((string.IsNullOrEmpty(searchRec.month)))
                    {
                        return PrfBillStaff(searchRec.year, searchRec.AmountType, searchRec.type, searchRec.category);
                    } else
                    {
                        return PrfBillStaffMonth(searchRec.year,searchRec.month, searchRec.AmountType, searchRec.type, searchRec.category);
                    }
                }
            }
            //return Index(1);
        }

        public ActionResult SummaryCostatOstd()
        {
            return View();
        }
        public ActionResult SummaryStaffOstd()
        {
            return View();
        }

        public ActionResult SummaryBill()
        {
            return View();
        }

        public ActionResult SummaryLdg()
        {
            return View();
        }

        public ActionResult SummaryBillMth(string year)
        {
            ViewBag.year = year;
            return View();
        }

        public ActionResult SummaryLdgMth(string year)
        {
            ViewBag.year = year;
            return View();
        }

        public ActionResult SummaryLdgDetail(string year, string month)
        {
            ViewBag.year = year;
            ViewBag.month = month;
            return View();
        }

        public ActionResult SummaryLdgDetail1(string year, string month)
        {
            ViewBag.year = year;
            ViewBag.month = month;
            return View();
        }

        public ActionResult SummaryLdgType(string rptType)
        {
            ViewBag.rptType = rptType;
            return View();
        }

        public ActionResult Index()
        {
            return StaffOstd();
        }
        public ActionResult SalesTax(string DateFr, string DateTo)
        {
            DateTime pDateFr = DateTime.MinValue; 
            DateTime pDateTo = DateTime.MinValue;

            DateTime.TryParseExact(DateFr, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out pDateFr);
            DateTime.TryParseExact(DateTo, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out pDateTo);

            FbContext fbcon = FbContext.Create();

            IEnumerable<TaxRecord> TaxRecs = fbcon.PopulateTaxesRcp( pDateFr, pDateTo,"");

            SACOMP profileRec = db.SACOMPs.FirstOrDefault();
            ViewBag.Company = profileRec.CONAME.ToUpper() + " (" + profileRec.COREGNO + ") ";
            ViewBag.RPT_START = DateFr;
            ViewBag.RPT_END = DateTo;
            ViewBag.Title = "Sales Tax Report";
            return View(TaxRecs);
        }

        public ActionResult SalesTaxChecking(string DateFr, string DateTo)
        {
            DateTime pDateFr = DateTime.MinValue;
            DateTime pDateTo = DateTime.MinValue;

            DateTime.TryParseExact(DateFr, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out pDateFr);
            DateTime.TryParseExact(DateTo, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out pDateTo);


            FbContext fbcon = FbContext.Create();

            IEnumerable<TaxRecord> TaxRecs = fbcon.PopulateTaxes(pDateFr, pDateTo);

            SACOMP profileRec = db.SACOMPs.FirstOrDefault();
            ViewBag.Company = profileRec.CONAME.ToUpper() + " (" + profileRec.COREGNO + ") ";
            ViewBag.RPT_START = DateFr;
            ViewBag.RPT_END = DateTo;
            ViewBag.Title = "Sales Tax Checking";
            return View(TaxRecs);
        }

        public ActionResult CostatYear()
        {

            FbContext fbcon = FbContext.Create();
            IEnumerable<CostatYearRecord> CostatYearRecords = fbcon.PopulateCostatYear();
            ViewBag.Title = "Company Status Summary";

            SACOMP profileRec = db.SACOMPs.FirstOrDefault();
            ViewBag.Company = profileRec.CONAME.ToUpper() + " (" + profileRec.COREGNO + ") ";
            return View("CostatYear", CostatYearRecords);
        }

        public ActionResult CostatMonth(string year)
        {

            int yearno = Int32.Parse(year);

            FbContext fbcon = FbContext.Create();
            IEnumerable<CostatMthRecord> CostatMthRecords = fbcon.PopulateCostatMonth( yearno);
            ViewBag.Title = "Company Status Monthly Summary for year " + year;

            SACOMP profileRec = db.SACOMPs.FirstOrDefault();
            ViewBag.year = year;
            ViewBag.Company = profileRec.CONAME.ToUpper() + " (" + profileRec.COREGNO + ") ";
            return View("CostatMonth", CostatMthRecords);
        }

        public ActionResult CostatDay(string year, string month)
        {

            int yearno = Int32.Parse(year);
            int monthno = Int32.Parse(month);

            FbContext fbcon = FbContext.Create();
            IEnumerable<CostatDayRecord> CostatDayRecords = fbcon.PopulateCostatDay(yearno, monthno);
            ViewBag.Title = "Company Status Daily Summary for year " + year + "/" + month;

            SACOMP profileRec = db.SACOMPs.FirstOrDefault();
            ViewBag.year = year;
            ViewBag.month = month;
            ViewBag.Company = profileRec.CONAME.ToUpper() + " (" + profileRec.COREGNO + ") ";
            return View("CostatDay", CostatDayRecords);
        }
        // GET: Report
        public ActionResult PrfBillYear(string AmountType, string Category)
        {
            //DateTime rptStart = pSearchVdate;
            //DateTime rptEnd = pSearchDdate;

            //ViewBag.RPT_START = rptStart.ToString("dd/MM/yyyy");
            //ViewBag.RPT_END = rptEnd.ToString("dd/MM/yyyy");

            if (String.IsNullOrEmpty(AmountType))
            {
                AmountType = "AMOUNT";
            }

            FbContext fbcon = FbContext.Create();
            IEnumerable<PrfBillYearRecord> PrfBillYearRecords = fbcon.PopulatePrfBillYear(AmountType, Category);
            ViewBag.Title = "Proforma Billing Summary for " + AmountType;

            SACOMP profileRec = db.SACOMPs.FirstOrDefault();
            ViewBag.Company = profileRec.CONAME.ToUpper() + " (" + profileRec.COREGNO + ") ";
            return View("PrfBillYear", PrfBillYearRecords);
        }

        public ActionResult PrfBillMonth(string Year, string AmountType, string Category)
        {
            //DateTime rptStart = pSearchVdate;
            //DateTime rptEnd = pSearchDdate;

            //ViewBag.RPT_START = rptStart.ToString("dd/MM/yyyy");
            //ViewBag.RPT_END = rptEnd.ToString("dd/MM/yyyy");

            if (String.IsNullOrEmpty(AmountType))
            {
                AmountType = "AMOUNT";
            }
            int yearno = Int32.Parse(Year);
            FbContext fbcon = FbContext.Create();
            IEnumerable<PrfBillMthRecord> PrfBillMthRecords = fbcon.PopulatePrfBillMonth(yearno, AmountType, Category);
            ViewBag.Title = "Proforma Billing Monthly Summary for " + Year + " " +  AmountType;

            SACOMP profileRec = db.SACOMPs.FirstOrDefault();
            ViewBag.Company = profileRec.CONAME.ToUpper() + " (" + profileRec.COREGNO + ") ";
            return View("PrfBillMonth", PrfBillMthRecords);
        }

        public ActionResult PrfBillDay(string Year,string Month, string AmountType, string Category)
        {

            if (String.IsNullOrEmpty(AmountType))
            {
                AmountType = "AMOUNT";
            }
            int yearno = Int32.Parse(Year);
            int monthno = Int32.Parse(Month);
            FbContext fbcon = FbContext.Create();
            IEnumerable<PrfBillDayRecord> PrfBillDayRecords = fbcon.PopulatePrfBillDay(yearno, monthno, AmountType, Category);
            ViewBag.Title = "Proforma Billing Daily Summary for " + Year + "/" + Month + " " + AmountType;

            SACOMP profileRec = db.SACOMPs.FirstOrDefault();
            ViewBag.Company = profileRec.CONAME.ToUpper() + " (" + profileRec.COREGNO + ") ";
            return View("PrfBillDay", PrfBillDayRecords);
        }

        public ActionResult PrfBillStaffMonth(string Year, string Month, string AmountType, string Type, string Category)
        {
            //DateTime rptStart = pSearchVdate;
            //DateTime rptEnd = pSearchDdate;

            //ViewBag.RPT_START = rptStart.ToString("dd/MM/yyyy");
            //ViewBag.RPT_END = rptEnd.ToString("dd/MM/yyyy");

            if (String.IsNullOrEmpty(AmountType))
            {
                AmountType = "AMOUNT";
            }
            int yearno = Int32.Parse(Year);
            int monthno = Int32.Parse(Month);
            FbContext fbcon = FbContext.Create();

            IEnumerable<PrfBillStaffRecord> PrfBillStaffRecords = null;
            if (Type == "STAFF")
            {
                PrfBillStaffRecords = fbcon.PopulatePrfBillStaffMonth(yearno, monthno, AmountType, Category);
                ViewBag.Title = "Proforma Billing Staff Job Summary for " + Year + "/" + Month + " " + AmountType;
            }  else if (Type == "PORTFOLIO")
            {
                PrfBillStaffRecords = fbcon.PopulatePrfBillStaffMonthPortfolio(yearno, monthno, AmountType, Category);
                ViewBag.Title = "Proforma Billing Staff Portfolio Summary for " + Year + "/" + Month + " " + AmountType;
            }
            else
            {
                PrfBillStaffRecords = fbcon.PopulatePrfBillStaffMonthCombo(yearno, monthno, AmountType, Category);
                ViewBag.Title = "Proforma Billing Staff Combined Summary for " + Year + "/" + Month + " " + AmountType;
            }
            SACOMP profileRec = db.SACOMPs.FirstOrDefault();
            ViewBag.Company = profileRec.CONAME.ToUpper() + " (" + profileRec.COREGNO + ") ";
            return View("PrfBillStaff", PrfBillStaffRecords);
        }

        public ActionResult PrfBillStaff(string Year, string AmountType, string Type, string Category)
        {
            //DateTime rptStart = pSearchVdate;
            //DateTime rptEnd = pSearchDdate;

            //ViewBag.RPT_START = rptStart.ToString("dd/MM/yyyy");
            //ViewBag.RPT_END = rptEnd.ToString("dd/MM/yyyy");

            if (String.IsNullOrEmpty(AmountType))
            {
                AmountType = "AMOUNT";
            }
            int yearno = Int32.Parse(Year);
            FbContext fbcon = FbContext.Create();
            IEnumerable<PrfBillStaffRecord> PrfBillStaffRecords = null;
            if (Type == "STAFF")
            {
                PrfBillStaffRecords = fbcon.PopulatePrfBillStaff(yearno, AmountType, Category);
                ViewBag.Title = "Proforma Billing Staff Job Summary for " + Year + " " + AmountType;
            } else if (Type == "PORTFOLIO")
            {
                PrfBillStaffRecords = fbcon.PopulatePrfBillStaffPortfolio(yearno, AmountType, Category);
                ViewBag.Title = "Proforma Billing Staff Portfolio Summary for " + Year + " " + AmountType;
            } else
            {
                PrfBillStaffRecords = fbcon.PopulatePrfBillStaffCombo(yearno, AmountType, Category);
                ViewBag.Title = "Proforma Billing Staff Combined Summary for " + Year + " " + AmountType;
            }

            SACOMP profileRec = db.SACOMPs.FirstOrDefault();
            ViewBag.Company = profileRec.CONAME.ToUpper() + " (" + profileRec.COREGNO + ") ";
            return View("PrfBillStaff", PrfBillStaffRecords);
        }

        public ActionResult StaffOstd()
        {
            string pSearchCode = "";
            string pSearchName = "";
            string pSearchStaff = "";
            string pSearchStatus = "";
            if (Session["SearchRec"] != null)
            {
                CSCOMSTR searchRec = (CSCOMSTR)(Session["SearchRec"]);
                pSearchCode = searchRec.CONO ?? "";
                pSearchName = searchRec.CONAME ?? "";
                pSearchStaff = searchRec.STAFFCODE ?? "";
                pSearchStatus = searchRec.COSTAT ?? "";
            }

            string conString = ConfigurationManager.ConnectionStrings["SirisCS"].ConnectionString;
            DataTable dt = new DataTable();
            DataRow curRow = null;

            List<string> myCol = new List<string>();
            foreach (string item in prefixcol)
            {
                myCol.Add(item);
            }

            using (FbConnection con = new FbConnection(conString))
            {
                con.Open();

                int lastno = 0;
                int cpos = 0;
                int diff;
                int j = 0;

                FbCommand myCommand = new FbCommand();
                myCommand.Connection = con;

                if (true)
                {

                    myCommand.CommandText =
            @"select a.*,(select b.staffdesc from hkstaff b where b.staffcode = a.staffcode) STAFFDESC from staff_ivtab a where cpos <> 0 order by cpos";
                    using (FbDataReader reader = myCommand.ExecuteReader())
                    {

                        string myGrp = "";
                        string myStaff = "";


                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                //myGrp = reader.GetString(reader.GetOrdinal("STAFFDESC"));
                                myStaff = reader.GetString(reader.GetOrdinal("STAFFCODE"));
                                myGrp = reader.GetString(reader.GetOrdinal("GROUPCODE"));
                                if ((pSearchStaff == myStaff) || (string.IsNullOrEmpty(pSearchStaff)))
                                {
                                    cpos = reader.GetInt32(reader.GetOrdinal("CPOS"));
                                }

                                if (lastno < cpos)
                                {
                                    diff = cpos - lastno;
                                    if (diff == 1)
                                    {
                                        myCol.Add(myGrp);
                                    }
                                    else
                                    {

                                        for (int i = 1; i < diff; i++)
                                        {
                                            j++;
                                            lastno++;
                                            myCol.Add("None" + j.ToString());
                                        }
                                        myCol.Add(myGrp);
                                    }
                                }
                                lastno = cpos;
                            }
                        }


                    }
                    for (int i = cpos; i < 18; i++)
                    {
                        j++;

                        myCol.Add("None" + j.ToString());
                    }

                }



                string[] myArray = new string[myCol.Count];
                for (int c = 0; c < myCol.Count; c++)
                {
                    myArray[c] = myCol[c];
                }
                addColumns(dt, myArray);

                int rowcnt = 0;

                myCommand.CommandText =
            @"select a.*, (select coregno from cscomstr b where b.cono = a.cono) COREGNO from v_costat_ostdivtab1 a";
                myCommand.Connection = con;
                using (FbDataReader reader = myCommand.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            rowcnt++;
                            curRow = dt.Rows.Add();
                            dt.Rows[dt.Rows.Count - 1][0] = rowcnt.ToString("D4");
                            dt.Rows[dt.Rows.Count - 1][1] = reader.GetString(reader.GetOrdinal("COREGNO"));
                            dt.Rows[dt.Rows.Count - 1][2] = reader.GetString(reader.GetOrdinal("CONAME"));
                            dt.Rows[dt.Rows.Count - 1][3] = reader.GetString(reader.GetOrdinal("STAFFCODE"));
                            dt.Rows[dt.Rows.Count - 1][4] = reader.GetString(reader.GetOrdinal("COSTAT"));
                            dt.Rows[dt.Rows.Count - 1][5] = reader.GetInt32(reader.GetOrdinal("BILLS"));
                            dt.Rows[dt.Rows.Count - 1][6] = reader.GetDecimal(reader.GetOrdinal("CSTRANM"));
                            dt.Rows[dt.Rows.Count - 1][7] = reader.GetDecimal(reader.GetOrdinal("OSTD"));
                            dt.Rows[dt.Rows.Count - 1][8] = reader.GetDecimal(reader.GetOrdinal("C01"));
                            dt.Rows[dt.Rows.Count - 1][9] = reader.GetDecimal(reader.GetOrdinal("C02"));
                            dt.Rows[dt.Rows.Count - 1][10] = reader.GetDecimal(reader.GetOrdinal("C03"));
                            dt.Rows[dt.Rows.Count - 1][11] = reader.GetDecimal(reader.GetOrdinal("C04"));
                            dt.Rows[dt.Rows.Count - 1][12] = reader.GetDecimal(reader.GetOrdinal("C05"));
                            dt.Rows[dt.Rows.Count - 1][13] = reader.GetDecimal(reader.GetOrdinal("C06"));
                            dt.Rows[dt.Rows.Count - 1][14] = reader.GetDecimal(reader.GetOrdinal("C07"));
                            dt.Rows[dt.Rows.Count - 1][15] = reader.GetDecimal(reader.GetOrdinal("C08"));
                            dt.Rows[dt.Rows.Count - 1][16] = reader.GetDecimal(reader.GetOrdinal("C09"));
                            dt.Rows[dt.Rows.Count - 1][17] = reader.GetDecimal(reader.GetOrdinal("C10"));
                            dt.Rows[dt.Rows.Count - 1][18] = reader.GetDecimal(reader.GetOrdinal("C11"));
                            dt.Rows[dt.Rows.Count - 1][19] = reader.GetDecimal(reader.GetOrdinal("C12"));
                            dt.Rows[dt.Rows.Count - 1][20] = reader.GetDecimal(reader.GetOrdinal("C13"));
                            dt.Rows[dt.Rows.Count - 1][21] = reader.GetDecimal(reader.GetOrdinal("C14"));
                            dt.Rows[dt.Rows.Count - 1][22] = reader.GetDecimal(reader.GetOrdinal("C15"));
                            dt.Rows[dt.Rows.Count - 1][23] = reader.GetDecimal(reader.GetOrdinal("C16"));
                            dt.Rows[dt.Rows.Count - 1][24] = reader.GetDecimal(reader.GetOrdinal("C17"));
                            dt.Rows[dt.Rows.Count - 1][25] = reader.GetDecimal(reader.GetOrdinal("C18"));

                        }
                    }
                }

            };

            var datarow = dt.AsEnumerable();
            if (pSearchCode != "")
            {
                if (pSearchCode.Length > 9)
                {
                    pSearchCode = pSearchCode.Substring(0, 10);
                    datarow = datarow.Where(x => x.Field<string>("company_no") == pSearchCode);
                }
                else
                {
                    datarow = datarow.Where(x => x.Field<string>("company_no").Contains(pSearchCode));
                }

            };
            if (pSearchName != "") { datarow = datarow.Where(x => x.Field<string>("company_name").Contains(pSearchName.ToUpper())); };
            if (pSearchStaff != "") { datarow = datarow.Where(x => x.Field<string>("staff") == pSearchStaff); ; };
            if (pSearchStatus != "") { datarow = datarow.Where(x => x.Field<string>("costat") == pSearchStatus); ; };

            DataTable ndt;
            if (datarow.Count() == 0)
            {
                ndt = new DataTable();
                ndt.Rows.Add();
            }
            else
            {
                ndt = datarow.CopyToDataTable<DataRow>();
            }
            SACOMP profileRec = db.SACOMPs.FirstOrDefault();
            ViewBag.Company = profileRec.CONAME.ToUpper() + " (" + profileRec.COREGNO + ") ";

            ViewBag.Title = "Client Outstanding Report by staff";
            return View("CustOstd", ndt);

        }

        public ActionResult CoStatOstd()
        {
            string pSearchCode = "";
            string pSearchName = "";
            string pSearchStaff = "";
            string pSearchStatus = "";
            if (Session["SearchRec"] != null)
            {
                CSCOMSTR searchRec = (CSCOMSTR)(Session["SearchRec"]);
                pSearchCode = searchRec.CONO ?? "";
                pSearchName = searchRec.CONAME ?? "";
                pSearchStaff = searchRec.STAFFCODE ?? "";
                pSearchStatus = searchRec.COSTAT ?? "";
            }

            string conString = ConfigurationManager.ConnectionStrings["SirisCS"].ConnectionString;
            DataTable dt = new DataTable();
            DataRow curRow = null;

            List<string> myCol = new List<string>();
            foreach (string item in prefixcol)
            {
                myCol.Add(item);
            }

            using (FbConnection con = new FbConnection(conString))
            {
                con.Open();

                int lastno = 0;
                int cpos = 0;
                int diff;
                int j = 0;

                FbCommand myCommand = new FbCommand();
                myCommand.Connection = con;

                if (true)
                {

                    myCommand.CommandText =
            @"select * from costat_ivtab where cpos <> 0 order by cpos";
                    using (FbDataReader reader = myCommand.ExecuteReader())
                    {

                        string myGrp = "";
                        string myCostat = "";


                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                myGrp = reader.GetString(reader.GetOrdinal("GROUPCODE"));
                                myCostat = reader.GetString(reader.GetOrdinal("COSTAT"));

                                if ((pSearchStatus == myCostat) || (string.IsNullOrEmpty(pSearchStatus)))
                                {
                                    cpos = reader.GetInt32(reader.GetOrdinal("CPOS"));
                                }

                                if (lastno < cpos)
                                {
                                    diff = cpos - lastno;
                                    if (diff == 1)
                                    {
                                        myCol.Add(myGrp);
                                    }
                                    else
                                    {

                                        for (int i = 1; i < diff; i++)
                                        {
                                            j++;
                                            lastno++;
                                            myCol.Add("None" + j.ToString());
                                        }
                                        myCol.Add(myGrp);
                                    }
                                }
                                lastno = cpos;
                            }
                        }


                    }
                    for (int i = cpos; i < 18; i++)
                    {
                        j++;

                        myCol.Add("None" + j.ToString());
                    }

                }



                string[] myArray = new string[myCol.Count];
                for (int c = 0; c < myCol.Count; c++)
                {
                    myArray[c] = myCol[c];
                }
                addColumns(dt, myArray);

                int rowcnt = 0;

                myCommand.CommandText =
             @"select a.*, (select coregno from cscomstr b where b.cono = a.cono) COREGNO from v_costat_ostdivtab1 a";
                myCommand.Connection = con;
                using (FbDataReader reader = myCommand.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            rowcnt++;
                            curRow = dt.Rows.Add();
                            dt.Rows[dt.Rows.Count - 1][0] = rowcnt.ToString("D4");
                            dt.Rows[dt.Rows.Count - 1][1] = reader.GetString(reader.GetOrdinal("COREGNO"));
                            dt.Rows[dt.Rows.Count - 1][2] = reader.GetString(reader.GetOrdinal("CONAME"));
                            dt.Rows[dt.Rows.Count - 1][3] = reader.GetString(reader.GetOrdinal("STAFFCODE"));
                            dt.Rows[dt.Rows.Count - 1][4] = reader.GetString(reader.GetOrdinal("COSTAT"));
                            dt.Rows[dt.Rows.Count - 1][5] = reader.GetInt32(reader.GetOrdinal("BILLS"));
                            dt.Rows[dt.Rows.Count - 1][6] = reader.GetDecimal(reader.GetOrdinal("CSTRANM"));
                            dt.Rows[dt.Rows.Count - 1][7] = reader.GetDecimal(reader.GetOrdinal("OSTD"));
                            dt.Rows[dt.Rows.Count - 1][8] = reader.GetDecimal(reader.GetOrdinal("C01"));
                            dt.Rows[dt.Rows.Count - 1][9] = reader.GetDecimal(reader.GetOrdinal("C02"));
                            dt.Rows[dt.Rows.Count - 1][10] = reader.GetDecimal(reader.GetOrdinal("C03"));
                            dt.Rows[dt.Rows.Count - 1][11] = reader.GetDecimal(reader.GetOrdinal("C04"));
                            dt.Rows[dt.Rows.Count - 1][12] = reader.GetDecimal(reader.GetOrdinal("C05"));
                            dt.Rows[dt.Rows.Count - 1][13] = reader.GetDecimal(reader.GetOrdinal("C06"));
                            dt.Rows[dt.Rows.Count - 1][14] = reader.GetDecimal(reader.GetOrdinal("C07"));
                            dt.Rows[dt.Rows.Count - 1][15] = reader.GetDecimal(reader.GetOrdinal("C08"));
                            dt.Rows[dt.Rows.Count - 1][16] = reader.GetDecimal(reader.GetOrdinal("C09"));
                            dt.Rows[dt.Rows.Count - 1][17] = reader.GetDecimal(reader.GetOrdinal("C10"));
                            dt.Rows[dt.Rows.Count - 1][18] = reader.GetDecimal(reader.GetOrdinal("C11"));
                            dt.Rows[dt.Rows.Count - 1][19] = reader.GetDecimal(reader.GetOrdinal("C12"));
                            dt.Rows[dt.Rows.Count - 1][20] = reader.GetDecimal(reader.GetOrdinal("C13"));
                            dt.Rows[dt.Rows.Count - 1][21] = reader.GetDecimal(reader.GetOrdinal("C14"));
                            dt.Rows[dt.Rows.Count - 1][22] = reader.GetDecimal(reader.GetOrdinal("C15"));
                            dt.Rows[dt.Rows.Count - 1][23] = reader.GetDecimal(reader.GetOrdinal("C16"));
                            dt.Rows[dt.Rows.Count - 1][24] = reader.GetDecimal(reader.GetOrdinal("C17"));
                            dt.Rows[dt.Rows.Count - 1][25] = reader.GetDecimal(reader.GetOrdinal("C18"));
                        }
                    }
                }

            };

            var datarow = dt.AsEnumerable();
            if (pSearchCode != "")
            {
                if (pSearchCode.Length > 9)
                {
                    pSearchCode = pSearchCode.Substring(0, 10);
                    datarow = datarow.Where(x => x.Field<string>("company_no") == pSearchCode);
                }
                else
                {
                    datarow = datarow.Where(x => x.Field<string>("company_no").Contains(pSearchCode));
                }

            };
            if (pSearchName != "") { datarow = datarow.Where(x => x.Field<string>("company_name").Contains(pSearchName.ToUpper())); };
            if (pSearchStaff != "") { datarow = datarow.Where(x => x.Field<string>("staff") == pSearchStaff); ; };
            if (pSearchStatus != "") { datarow = datarow.Where(x => x.Field<string>("costat") == pSearchStatus); ; };

            DataTable ndt;
            if (datarow.Count() == 0)
            {
                ndt = new DataTable();
                ndt.Rows.Add();
            }
            else
            {
                ndt = datarow.CopyToDataTable<DataRow>();
            }
            SACOMP profileRec = db.SACOMPs.FirstOrDefault();
            ViewBag.Company = profileRec.CONAME.ToUpper() + " (" + profileRec.COREGNO + ") ";

            ViewBag.Title = "Client Outstanding Report by status";
            return View("CustOstd", ndt);

        }

        [AllowAnonymous]
        public ActionResult JobStat()
        {


            string conString = ConfigurationManager.ConnectionStrings["SirisCS"].ConnectionString;
            DataTable dt = new DataTable();
            DataRow curRow = null;

            List<string> myCol = new List<string>();
            foreach (string item in prefixcol1)
            {
                myCol.Add(item);
            }

            using (FbConnection con = new FbConnection(conString))
            {
                con.Open();

                int lastno = 0;
                int cpos = 0;
                int diff;
                int j = 0;

                FbCommand myCommand = new FbCommand();
                myCommand.Connection = con;

                string[] myArray = new string[myCol.Count];
                addColumns1(dt, myArray);

                int rowcnt = 0;

                myCommand.CommandText =
             @"select * from v_jobstat_ivtab where 1 = 1 ";

                if (Session["HKSTAFFLIST"] != null)
                {
                    myCommand.CommandText = myCommand.CommandText + " and jobstaff in (" + Session["HKSTAFFLIST"] + ") ";
                } else 
                if (Session["HKSTAFF"] != null)
                {
                    myCommand.CommandText = myCommand.CommandText + " and jobstaff = '" + Session["HKSTAFF"] + "' ";
                }

                myCommand.Connection = con;
                using (FbDataReader reader = myCommand.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            rowcnt++;
                            curRow = dt.Rows.Add();
                            dt.Rows[dt.Rows.Count - 1][0] = rowcnt.ToString("D4");
                            dt.Rows[dt.Rows.Count - 1][1] = reader.GetString(reader.GetOrdinal("JOBSTAFF"));
                            dt.Rows[dt.Rows.Count - 1][2] = reader.GetString(reader.GetOrdinal("STAFFDESC"));
                            dt.Rows[dt.Rows.Count - 1][3] = reader.GetInt32(reader.GetOrdinal("NEW"));
                            dt.Rows[dt.Rows.Count - 1][4] = reader.GetInt32(reader.GetOrdinal("OUT"));
                            dt.Rows[dt.Rows.Count - 1][5] = reader.GetInt32(reader.GetOrdinal("AGM"));
                            dt.Rows[dt.Rows.Count - 1][6] = reader.GetInt32(reader.GetOrdinal("STATUTORY"));
                            dt.Rows[dt.Rows.Count - 1][7] = reader.GetInt32(reader.GetOrdinal("RETURN"));
                            dt.Rows[dt.Rows.Count - 1][8] = reader.GetInt32(reader.GetOrdinal("CANCEL"));
                            dt.Rows[dt.Rows.Count - 1][9] = reader.GetInt32(reader.GetOrdinal("COMPLETE"));
                            dt.Rows[dt.Rows.Count - 1][10] = reader.GetInt32(reader.GetOrdinal("WIP"));
                            dt.Rows[dt.Rows.Count - 1][11] = reader.GetInt32(reader.GetOrdinal("TOTAL"));

                        }
                    }
                }

            };

            var datarow = dt.AsEnumerable();


            DataTable ndt;
            if (datarow.Count() == 0)
            {
                ndt = new DataTable();
                ndt.Rows.Add();
            }
            else
            {
                ndt = datarow.CopyToDataTable<DataRow>();
            }
            SACOMP profileRec = db.SACOMPs.FirstOrDefault();
            ViewBag.Company = profileRec.CONAME.ToUpper() + " (" + profileRec.COREGNO + ") ";

            ViewBag.Title = "Job Status Summary";
            return View("JobStat", ndt);

        }

        public ActionResult BillYear()
        {


            string conString = ConfigurationManager.ConnectionStrings["SirisCS"].ConnectionString;
            DataTable dt = new DataTable();
            DataRow curRow = null;

            List<string> myCol = new List<string>();
            foreach (string item in prefixcol2)
            {
                myCol.Add(item);
            }

            using (FbConnection con = new FbConnection(conString))
            {
                con.Open();

                int lastno = 0;
                int cpos = 0;
                int diff;
                int j = 0;

                FbCommand myCommand = new FbCommand();
                myCommand.Connection = con;

                string[] myArray = new string[myCol.Count];
                addColumns2(dt, myArray);

                int rowcnt = 0;

                myCommand.CommandText =
             @"select * from v_csbill_sumivtab";
                myCommand.Connection = con;
                using (FbDataReader reader = myCommand.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            rowcnt++;
                            curRow = dt.Rows.Add();
                            dt.Rows[dt.Rows.Count - 1][0] = rowcnt.ToString("D4");
                            dt.Rows[dt.Rows.Count - 1][1] = reader.GetString(reader.GetOrdinal("CASECODE"));
                            dt.Rows[dt.Rows.Count - 1][2] = reader.GetString(reader.GetOrdinal("CASEDESC"));
                            dt.Rows[dt.Rows.Count - 1][3] = reader.GetDecimal(reader.GetOrdinal("C01"));
                            dt.Rows[dt.Rows.Count - 1][4] = reader.GetDecimal(reader.GetOrdinal("C02"));
                            dt.Rows[dt.Rows.Count - 1][5] = reader.GetDecimal(reader.GetOrdinal("C03"));
                            dt.Rows[dt.Rows.Count - 1][6] = reader.GetDecimal(reader.GetOrdinal("C04"));
                            dt.Rows[dt.Rows.Count - 1][7] = reader.GetDecimal(reader.GetOrdinal("C05"));
                            dt.Rows[dt.Rows.Count - 1][8] = reader.GetDecimal(reader.GetOrdinal("C06"));
                            dt.Rows[dt.Rows.Count - 1][9] = reader.GetDecimal(reader.GetOrdinal("C07"));
                            dt.Rows[dt.Rows.Count - 1][10] = reader.GetDecimal(reader.GetOrdinal("C08"));
                            dt.Rows[dt.Rows.Count - 1][11] = reader.GetDecimal(reader.GetOrdinal("C09"));
                            dt.Rows[dt.Rows.Count - 1][12] = reader.GetDecimal(reader.GetOrdinal("C10"));
                            dt.Rows[dt.Rows.Count - 1][13] = reader.GetDecimal(reader.GetOrdinal("C11"));
                            dt.Rows[dt.Rows.Count - 1][14] = reader.GetDecimal(reader.GetOrdinal("C12"));
                            dt.Rows[dt.Rows.Count - 1][15] = reader.GetDecimal(reader.GetOrdinal("C13"));
                            dt.Rows[dt.Rows.Count - 1][16] = reader.GetDecimal(reader.GetOrdinal("C14"));
                            dt.Rows[dt.Rows.Count - 1][17] = reader.GetDecimal(reader.GetOrdinal("C15"));
                            dt.Rows[dt.Rows.Count - 1][18] = reader.GetDecimal(reader.GetOrdinal("C16"));
                            dt.Rows[dt.Rows.Count - 1][19] = reader.GetDecimal(reader.GetOrdinal("C17"));
                            dt.Rows[dt.Rows.Count - 1][20] = reader.GetDecimal(reader.GetOrdinal("C18"));
                            dt.Rows[dt.Rows.Count - 1][21] = reader.GetDecimal(reader.GetOrdinal("C19"));
                            dt.Rows[dt.Rows.Count - 1][22] = reader.GetDecimal(reader.GetOrdinal("C20"));
                            dt.Rows[dt.Rows.Count - 1][23] = reader.GetDecimal(reader.GetOrdinal("C21"));
                            dt.Rows[dt.Rows.Count - 1][24] = reader.GetDecimal(reader.GetOrdinal("C22"));
                            dt.Rows[dt.Rows.Count - 1][25] = reader.GetDecimal(reader.GetOrdinal("C23"));
                            dt.Rows[dt.Rows.Count - 1][26] = reader.GetDecimal(reader.GetOrdinal("C24"));
                            dt.Rows[dt.Rows.Count - 1][27] = reader.GetDecimal(reader.GetOrdinal("C25"));
                            dt.Rows[dt.Rows.Count - 1][28] = reader.GetDecimal(reader.GetOrdinal("C26"));
                            dt.Rows[dt.Rows.Count - 1][29] = reader.GetDecimal(reader.GetOrdinal("C27"));
                            dt.Rows[dt.Rows.Count - 1][30] = reader.GetDecimal(reader.GetOrdinal("C28"));
                            dt.Rows[dt.Rows.Count - 1][31] = reader.GetDecimal(reader.GetOrdinal("C29"));
                            dt.Rows[dt.Rows.Count - 1][32] = reader.GetDecimal(reader.GetOrdinal("C30"));
                            dt.Rows[dt.Rows.Count - 1][33] = reader.GetDecimal(reader.GetOrdinal("C31"));
                            dt.Rows[dt.Rows.Count - 1][34] = reader.GetDecimal(reader.GetOrdinal("C32"));
                            dt.Rows[dt.Rows.Count - 1][35] = reader.GetDecimal(reader.GetOrdinal("C33"));
                            dt.Rows[dt.Rows.Count - 1][36] = reader.GetDecimal(reader.GetOrdinal("C34"));
                            dt.Rows[dt.Rows.Count - 1][37] = reader.GetDecimal(reader.GetOrdinal("C35"));
                            dt.Rows[dt.Rows.Count - 1][38] = reader.GetDecimal(reader.GetOrdinal("C36"));

                        }
                    }
                }

            };

            var datarow = dt.AsEnumerable();


            DataTable ndt;
            if (datarow.Count() == 0)
            {
                ndt = new DataTable();
                ndt.Rows.Add();
            }
            else
            {
                ndt = datarow.CopyToDataTable<DataRow>();
            }
            SACOMP profileRec = db.SACOMPs.FirstOrDefault();
            ViewBag.Company = profileRec.CONAME.ToUpper() + " (" + profileRec.COREGNO + ") ";

            ViewBag.Title = "Bill Item Summary";
            return View("BillYear", ndt);

        }

        public ActionResult BillMonth(string year)
        {


            string conString = ConfigurationManager.ConnectionStrings["SirisCS"].ConnectionString;
            DataTable dt = new DataTable();
            DataRow curRow = null;

            List<string> myCol = new List<string>();
            foreach (string item in prefixcol3)
            {
                myCol.Add(item);
            }

            using (FbConnection con = new FbConnection(conString))
            {
                con.Open();

                int lastno = 0;
                int cpos = 0;
                int diff;
                int j = 0;

                FbCommand myCommand = new FbCommand();
                myCommand.Connection = con;

                string[] myArray = new string[myCol.Count];
                addColumns2(dt, myArray);

                int rowcnt = 0;

                myCommand.CommandText =
             @"select * from v_csbill_sumivtab_mth where yearno = " + year;
                myCommand.Connection = con;
                using (FbDataReader reader = myCommand.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            rowcnt++;
                            curRow = dt.Rows.Add();
                            dt.Rows[dt.Rows.Count - 1][0] = rowcnt.ToString("D4");
                            dt.Rows[dt.Rows.Count - 1][1] = reader.GetString(reader.GetOrdinal("CASECODE"));
                            dt.Rows[dt.Rows.Count - 1][2] = reader.GetString(reader.GetOrdinal("CASEDESC"));
                            dt.Rows[dt.Rows.Count - 1][3] = reader.GetDecimal(reader.GetOrdinal("C01"));
                            dt.Rows[dt.Rows.Count - 1][4] = reader.GetDecimal(reader.GetOrdinal("C02"));
                            dt.Rows[dt.Rows.Count - 1][5] = reader.GetDecimal(reader.GetOrdinal("C03"));
                            dt.Rows[dt.Rows.Count - 1][6] = reader.GetDecimal(reader.GetOrdinal("C04"));
                            dt.Rows[dt.Rows.Count - 1][7] = reader.GetDecimal(reader.GetOrdinal("C05"));
                            dt.Rows[dt.Rows.Count - 1][8] = reader.GetDecimal(reader.GetOrdinal("C06"));
                            dt.Rows[dt.Rows.Count - 1][9] = reader.GetDecimal(reader.GetOrdinal("C07"));
                            dt.Rows[dt.Rows.Count - 1][10] = reader.GetDecimal(reader.GetOrdinal("C08"));
                            dt.Rows[dt.Rows.Count - 1][11] = reader.GetDecimal(reader.GetOrdinal("C09"));
                            dt.Rows[dt.Rows.Count - 1][12] = reader.GetDecimal(reader.GetOrdinal("C10"));
                            dt.Rows[dt.Rows.Count - 1][13] = reader.GetDecimal(reader.GetOrdinal("C11"));
                            dt.Rows[dt.Rows.Count - 1][14] = reader.GetDecimal(reader.GetOrdinal("C12"));


                        }
                    }
                }

            };

            var datarow = dt.AsEnumerable();


            DataTable ndt;
            if (datarow.Count() == 0)
            {
                ndt = new DataTable();
                ndt.Rows.Add();
            }
            else
            {
                ndt = datarow.CopyToDataTable<DataRow>();
            }
            SACOMP profileRec = db.SACOMPs.FirstOrDefault();
            ViewBag.Company = profileRec.CONAME.ToUpper() + " (" + profileRec.COREGNO + ") ";

            ViewBag.Title = "Bill Item Monthly Summary for Year of " + year;
            return View("BillMonth", ndt);

        }

        public ActionResult LdgYear()
        {

            string conString = ConfigurationManager.ConnectionStrings["SirisCS"].ConnectionString;
            DataTable dt = new DataTable();
            DataRow curRow = null;

            List<string> myCol = new List<string>();
            foreach (string item in prefixcol4)
            {
                myCol.Add(item);
            }

            using (FbConnection con = new FbConnection(conString))
            {
                con.Open();

                int lastno = 0;
                int cpos = 0;
                int diff;
                int j = 0;

                FbCommand myCommand = new FbCommand();
                myCommand.Connection = con;

                string[] myArray = new string[myCol.Count];
                addColumns3(dt, myArray);

                int rowcnt = 0;

                myCommand.CommandText =
             @"select * from v_csldg_sum_year";
                myCommand.Connection = con;
                using (FbDataReader reader = myCommand.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {

                            rowcnt++;
                            curRow = dt.Rows.Add();
                            dt.Rows[dt.Rows.Count - 1][0] = reader.GetString(reader.GetOrdinal("YEARNO"));
                            dt.Rows[dt.Rows.Count - 1][1] = reader.GetDecimal(reader.GetOrdinal("RECEIPT"));
                            dt.Rows[dt.Rows.Count - 1][2] = reader.GetDecimal(reader.GetOrdinal("DEP"));
                            dt.Rows[dt.Rows.Count - 1][3] = reader.GetDecimal(reader.GetOrdinal("FEE1"));
                            dt.Rows[dt.Rows.Count - 1][4] = reader.GetDecimal(reader.GetOrdinal("FEE2"));
                            dt.Rows[dt.Rows.Count - 1][5] = reader.GetDecimal(reader.GetOrdinal("TAX1"));
                            dt.Rows[dt.Rows.Count - 1][6] = reader.GetDecimal(reader.GetOrdinal("TAX2"));
                            dt.Rows[dt.Rows.Count - 1][7] = reader.GetDecimal(reader.GetOrdinal("WORK1"));
                            dt.Rows[dt.Rows.Count - 1][8] = reader.GetDecimal(reader.GetOrdinal("WORK2"));
                            dt.Rows[dt.Rows.Count - 1][9] = reader.GetDecimal(reader.GetOrdinal("DISB1"));
                            dt.Rows[dt.Rows.Count - 1][10] = reader.GetDecimal(reader.GetOrdinal("DISB2"));
                            dt.Rows[dt.Rows.Count - 1][11] = reader.GetDecimal(reader.GetOrdinal("REIMB1"));
                            dt.Rows[dt.Rows.Count - 1][12] = reader.GetDecimal(reader.GetOrdinal("REIMB2"));
                            dt.Rows[dt.Rows.Count - 1][13] = reader.GetDecimal(reader.GetOrdinal("Advance"));



                        }
                    }
                }

            };

            var datarow = dt.AsEnumerable();


            DataTable ndt;
            if (datarow.Count() == 0)
            {
                ndt = new DataTable();
                ndt.Rows.Add();
            }
            else
            {
                ndt = datarow.CopyToDataTable<DataRow>();
            }
            SACOMP profileRec = db.SACOMPs.FirstOrDefault();
            ViewBag.Company = profileRec.CONAME.ToUpper() + " (" + profileRec.COREGNO + ") ";

            ViewBag.Title = "Client Ledger Summary";
            return View("LdgYear", ndt);


        }

        public ActionResult LdgMonth(string year)
        {

            string conString = ConfigurationManager.ConnectionStrings["SirisCS"].ConnectionString;
            DataTable dt = new DataTable();
            DataRow curRow = null;

            List<string> myCol = new List<string>();
            foreach (string item in prefixcol4)
            {
                myCol.Add(item);
            }

            using (FbConnection con = new FbConnection(conString))
            {
                con.Open();

                int lastno = 0;
                int cpos = 0;
                int diff;
                int j = 0;

                FbCommand myCommand = new FbCommand();
                myCommand.Connection = con;

                string[] myArray = new string[myCol.Count];
                addColumns3(dt, myArray);

                int rowcnt = 0;

                myCommand.CommandText =
             @"select * from v_csldg_sum_year_mth where yearno = '" + year + "' ";



                myCommand.Connection = con;
                using (FbDataReader reader = myCommand.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {

                            rowcnt++;
                            curRow = dt.Rows.Add();
                            dt.Rows[dt.Rows.Count - 1][0] = reader.GetString(reader.GetOrdinal("MONTHNO"));
                            dt.Rows[dt.Rows.Count - 1][1] = reader.GetDecimal(reader.GetOrdinal("RECEIPT"));
                            dt.Rows[dt.Rows.Count - 1][2] = reader.GetDecimal(reader.GetOrdinal("DEP"));
                            dt.Rows[dt.Rows.Count - 1][3] = reader.GetDecimal(reader.GetOrdinal("FEE1"));
                            dt.Rows[dt.Rows.Count - 1][4] = reader.GetDecimal(reader.GetOrdinal("FEE2"));
                            dt.Rows[dt.Rows.Count - 1][5] = reader.GetDecimal(reader.GetOrdinal("TAX1"));
                            dt.Rows[dt.Rows.Count - 1][6] = reader.GetDecimal(reader.GetOrdinal("TAX2"));
                            dt.Rows[dt.Rows.Count - 1][7] = reader.GetDecimal(reader.GetOrdinal("WORK1"));
                            dt.Rows[dt.Rows.Count - 1][8] = reader.GetDecimal(reader.GetOrdinal("WORK2"));
                            dt.Rows[dt.Rows.Count - 1][9] = reader.GetDecimal(reader.GetOrdinal("DISB1"));
                            dt.Rows[dt.Rows.Count - 1][10] = reader.GetDecimal(reader.GetOrdinal("DISB2"));
                            dt.Rows[dt.Rows.Count - 1][11] = reader.GetDecimal(reader.GetOrdinal("REIMB1"));
                            dt.Rows[dt.Rows.Count - 1][12] = reader.GetDecimal(reader.GetOrdinal("REIMB2"));
                            dt.Rows[dt.Rows.Count - 1][13] = reader.GetDecimal(reader.GetOrdinal("Advance"));



                        }
                    }
                }

            };

            var datarow = dt.AsEnumerable();


            DataTable ndt;
            if (datarow.Count() == 0)
            {
                ndt = new DataTable();
                ndt.Rows.Add();
            }
            else
            {
                ndt = datarow.CopyToDataTable<DataRow>();
            }
            SACOMP profileRec = db.SACOMPs.FirstOrDefault();
            ViewBag.Company = profileRec.CONAME.ToUpper() + " (" + profileRec.COREGNO + ") ";
            ViewBag.Year = year;
            ViewBag.Title = "Client Ledger Monthly Summary for " + year;
            return View("LdgMonth", ndt);


        }

        public ActionResult LdgDetail(string year, string month)
        {

            string conString = ConfigurationManager.ConnectionStrings["SirisCS"].ConnectionString;
            DataTable dt = new DataTable();
            DataRow curRow = null;

            List<string> myCol = new List<string>();
            foreach (string item in prefixcol6)
            {
                myCol.Add(item);
            }

            using (FbConnection con = new FbConnection(conString))
            {
                con.Open();

                int lastno = 0;
                int cpos = 0;
                int diff;
                int j = 0;

                FbCommand myCommand = new FbCommand();
                myCommand.Connection = con;

                string[] myArray = new string[myCol.Count];
                addColumns4(dt, myArray);

                int rowcnt = 0;
                int yearno = Int32.Parse(year);
                int monthno = Int32.Parse(month);

                myCommand.CommandText =
             @"select (select coname from cscomstr b where b.cono = a.cono) coname ,
                     (select coregno from cscomstr b where b.cono = a.cono) coregno,
                a.* from v_csldg_sum_cono_year_mth a
                where yearno = @yearno and monthno = @monthno 
                order by 1";
                myCommand.Parameters.Add("@yearno", yearno);
                myCommand.Parameters.Add("@monthno", monthno);


                myCommand.Connection = con;
                using (FbDataReader reader = myCommand.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {

                            rowcnt++;
                            curRow = dt.Rows.Add();
                            dt.Rows[dt.Rows.Count - 1][0] = reader.GetString(reader.GetOrdinal("COREGNO"));
                            dt.Rows[dt.Rows.Count - 1][1] = reader.GetString(reader.GetOrdinal("CONAME"));
                            dt.Rows[dt.Rows.Count - 1][2] = reader.GetDecimal(reader.GetOrdinal("RECEIPT"));
                            dt.Rows[dt.Rows.Count - 1][3] = reader.GetDecimal(reader.GetOrdinal("DEP"));
                            dt.Rows[dt.Rows.Count - 1][4] = reader.GetDecimal(reader.GetOrdinal("FEE1"));
                            dt.Rows[dt.Rows.Count - 1][5] = reader.GetDecimal(reader.GetOrdinal("FEE2"));
                            dt.Rows[dt.Rows.Count - 1][6] = reader.GetDecimal(reader.GetOrdinal("TAX1"));
                            dt.Rows[dt.Rows.Count - 1][7] = reader.GetDecimal(reader.GetOrdinal("TAX2"));
                            dt.Rows[dt.Rows.Count - 1][8] = reader.GetDecimal(reader.GetOrdinal("WORK1"));
                            dt.Rows[dt.Rows.Count - 1][9] = reader.GetDecimal(reader.GetOrdinal("WORK2"));
                            dt.Rows[dt.Rows.Count - 1][10] = reader.GetDecimal(reader.GetOrdinal("DISB1"));
                            dt.Rows[dt.Rows.Count - 1][11] = reader.GetDecimal(reader.GetOrdinal("DISB2"));
                            dt.Rows[dt.Rows.Count - 1][12] = reader.GetDecimal(reader.GetOrdinal("REIMB1"));
                            dt.Rows[dt.Rows.Count - 1][13] = reader.GetDecimal(reader.GetOrdinal("REIMB2"));
                            dt.Rows[dt.Rows.Count - 1][14] = reader.GetDecimal(reader.GetOrdinal("Advance"));



                        }
                    }
                }

            };

            var datarow = dt.AsEnumerable();


            DataTable ndt;
            if (datarow.Count() == 0)
            {
                ndt = new DataTable();
                ndt.Rows.Add();
            }
            else
            {
                ndt = datarow.CopyToDataTable<DataRow>();
            }
            SACOMP profileRec = db.SACOMPs.FirstOrDefault();
            ViewBag.Company = profileRec.CONAME.ToUpper() + " (" + profileRec.COREGNO + ") ";

            ViewBag.Title = "Client Ledger Monthly Details for " + year + "/" + month;
            return View("LdgDetail", ndt);


        }

        public ActionResult LdgDetail1(string year, string month)
        {

            string conString = ConfigurationManager.ConnectionStrings["SirisCS"].ConnectionString;
            DataTable dt = new DataTable();
            DataRow curRow = null;

            List<string> myCol = new List<string>();
            foreach (string item in prefixcol7)
            {
                myCol.Add(item);
            }

            using (FbConnection con = new FbConnection(conString))
            {
                con.Open();

                int lastno = 0;
                int cpos = 0;
                int diff;
                int j = 0;

                FbCommand myCommand = new FbCommand();
                myCommand.Connection = con;

                string[] myArray = new string[myCol.Count];
                addColumns5(dt, myArray);

                int rowcnt = 0;
                int yearno = Int32.Parse(year);
                int monthno = Int32.Parse(month);

                myCommand.CommandText =
             @"select (select coname from cscomstr b where b.cono = a.cono) coname,
                    (select coregno from cscomstr b where b.cono = a.cono) coregno,
                a.* from v_csldg_sum_cono_staff_year_mth a
                where yearno = @yearno and monthno = @monthno 
                order by staffcode, 1";
                myCommand.Parameters.Add("@yearno", yearno);
                myCommand.Parameters.Add("@monthno", monthno);


                myCommand.Connection = con;
                using (FbDataReader reader = myCommand.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {

                            rowcnt++;
                            curRow = dt.Rows.Add();
                            dt.Rows[dt.Rows.Count - 1][0] = reader.GetString(reader.GetOrdinal("COREGNO"));
                            dt.Rows[dt.Rows.Count - 1][1] = reader.GetString(reader.GetOrdinal("CONAME"));
                            dt.Rows[dt.Rows.Count - 1][2] = reader.GetDecimal(reader.GetOrdinal("RECEIPT"));
                            dt.Rows[dt.Rows.Count - 1][3] = reader.GetDecimal(reader.GetOrdinal("DEP"));
                            dt.Rows[dt.Rows.Count - 1][4] = reader.GetDecimal(reader.GetOrdinal("FEE1"));
                            dt.Rows[dt.Rows.Count - 1][5] = reader.GetDecimal(reader.GetOrdinal("FEE2"));
                            dt.Rows[dt.Rows.Count - 1][6] = reader.GetDecimal(reader.GetOrdinal("TAX1"));
                            dt.Rows[dt.Rows.Count - 1][7] = reader.GetDecimal(reader.GetOrdinal("TAX2"));
                            dt.Rows[dt.Rows.Count - 1][8] = reader.GetDecimal(reader.GetOrdinal("WORK1"));
                            dt.Rows[dt.Rows.Count - 1][9] = reader.GetDecimal(reader.GetOrdinal("WORK2"));
                            dt.Rows[dt.Rows.Count - 1][10] = reader.GetDecimal(reader.GetOrdinal("DISB1"));
                            dt.Rows[dt.Rows.Count - 1][11] = reader.GetDecimal(reader.GetOrdinal("DISB2"));
                            dt.Rows[dt.Rows.Count - 1][12] = reader.GetDecimal(reader.GetOrdinal("REIMB1"));
                            dt.Rows[dt.Rows.Count - 1][13] = reader.GetDecimal(reader.GetOrdinal("REIMB2"));
                            dt.Rows[dt.Rows.Count - 1][14] = reader.GetDecimal(reader.GetOrdinal("Advance"));
                            dt.Rows[dt.Rows.Count - 1][15] = reader.GetString(reader.GetOrdinal("STAFFCODE"));



                        }
                    }
                }

            };

            var datarow = dt.AsEnumerable();


            DataTable ndt;
            if (datarow.Count() == 0)
            {
                ndt = new DataTable();
                ndt.Rows.Add();
            }
            else
            {
                ndt = datarow.CopyToDataTable<DataRow>();
            }
            SACOMP profileRec = db.SACOMPs.FirstOrDefault();
            ViewBag.Company = profileRec.CONAME.ToUpper() + " (" + profileRec.COREGNO + ") ";

            ViewBag.Title = "Client Ledger Monthly Details By Staff Portfolio for " + year + "/" + month;
            return View("LdgDetail1", ndt);


        }

        public ActionResult LdgType(string rptType)
        {

            string conString = ConfigurationManager.ConnectionStrings["SirisCS"].ConnectionString;
            DataTable dt = new DataTable();
            DataRow curRow = null;

            List<string> myCol = new List<string>();
            foreach (string item in prefixcol5)
            {
                myCol.Add(item);
            }

            using (FbConnection con = new FbConnection(conString))
            {
                con.Open();

                int lastno = 0;
                int cpos = 0;
                int diff;
                int j = 0;

                FbCommand myCommand = new FbCommand();
                myCommand.Connection = con;

                string[] myArray = new string[myCol.Count];
                addColumns3(dt, myArray);

                int rowcnt = 0;

                string sql = @"select yearno, 
sum({0} * c01) JAN,sum({0} * c02) FEB,sum({0} * c03) MAR,sum({0} * c04) APR,
sum({0} * c05) MAY,sum({0} * c06) JUN,sum({0} * c07) JUL,sum({0} * c08) AUG,
sum({0} * c09) SEP,sum({0} * c10) OCT,sum({0} * c11) NOV,sum({0} * c12) DCB
 from v_csldg_sum_year_mth a,MONTH_IVTAB b where a.MONTHNO = b.MTHNO group by yearno";

                sql = string.Format(sql, rptType);

                myCommand.CommandText = sql;

                myCommand.Connection = con;
                using (FbDataReader reader = myCommand.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {

                            rowcnt++;
                            curRow = dt.Rows.Add();
                            dt.Rows[dt.Rows.Count - 1][0] = reader.GetString(reader.GetOrdinal("YEARNO"));
                            dt.Rows[dt.Rows.Count - 1][1] = reader.GetDecimal(reader.GetOrdinal("JAN"));
                            dt.Rows[dt.Rows.Count - 1][2] = reader.GetDecimal(reader.GetOrdinal("FEB"));
                            dt.Rows[dt.Rows.Count - 1][3] = reader.GetDecimal(reader.GetOrdinal("MAR"));
                            dt.Rows[dt.Rows.Count - 1][4] = reader.GetDecimal(reader.GetOrdinal("APR"));
                            dt.Rows[dt.Rows.Count - 1][5] = reader.GetDecimal(reader.GetOrdinal("MAY"));
                            dt.Rows[dt.Rows.Count - 1][6] = reader.GetDecimal(reader.GetOrdinal("JUN"));
                            dt.Rows[dt.Rows.Count - 1][7] = reader.GetDecimal(reader.GetOrdinal("JUL"));
                            dt.Rows[dt.Rows.Count - 1][8] = reader.GetDecimal(reader.GetOrdinal("AUG"));
                            dt.Rows[dt.Rows.Count - 1][9] = reader.GetDecimal(reader.GetOrdinal("SEP"));
                            dt.Rows[dt.Rows.Count - 1][10] = reader.GetDecimal(reader.GetOrdinal("OCT"));
                            dt.Rows[dt.Rows.Count - 1][11] = reader.GetDecimal(reader.GetOrdinal("NOV"));
                            dt.Rows[dt.Rows.Count - 1][12] = reader.GetDecimal(reader.GetOrdinal("DCB"));
                        }
                    }
                }

            };

            var datarow = dt.AsEnumerable();


            DataTable ndt;
            if (datarow.Count() == 0)
            {
                ndt = new DataTable();
                ndt.Rows.Add();
            }
            else
            {
                ndt = datarow.CopyToDataTable<DataRow>();
            }
            SACOMP profileRec = db.SACOMPs.FirstOrDefault();
            ViewBag.Company = profileRec.CONAME.ToUpper() + " (" + profileRec.COREGNO + ") ";

            string rptTypeDesc;
            if (rptType == "RECEIPT") { rptTypeDesc = "Receipts"; }
            else if (rptType == "FEE1") { rptTypeDesc = "Fees Own"; }
            else if (rptType == "FEE2") { rptTypeDesc = "Fee Others"; }
            else if (rptType == "TAX1") { rptTypeDesc = "Tax Own"; }
            else if (rptType == "TAX2") { rptTypeDesc = "Tax Others"; }
            else if (rptType == "WORK1") { rptTypeDesc = "Work Own"; }
            else if (rptType == "WORK2") { rptTypeDesc = "Work Others"; }
            else if (rptType == "DISB1") { rptTypeDesc = "Disbursements Own"; }
            else if (rptType == "DISB2") { rptTypeDesc = "Disbursements Others"; }
            else if (rptType == "REIMB1") { rptTypeDesc = "Reimbursement Own"; }
            else if (rptType == "REIMB2") { rptTypeDesc = "Reimbursement Others"; }
            else if (rptType == "ADVANCE") { rptTypeDesc = "Advance"; }
            else { rptTypeDesc = "Undefined"; }

            ViewBag.Title = "Client Ledger Summary for " + rptTypeDesc;
            ViewBag.rptType = rptTypeDesc;
            return View("LdgType", ndt);


        }
        // for client_ostd
        private void addColumns(DataTable dt, string[] col)
        {
            int j = col.Length;
            for (int i = 0; i < j; i++)
            {
                if (i == 5)
                {
                    dt.Columns.Add(new DataColumn(col[i], typeof(int)));
                }
                else if (i < 6)
                {
                    dt.Columns.Add(new DataColumn(col[i], typeof(string)));
                }
                else
                {
                    dt.Columns.Add(new DataColumn(col[i], typeof(decimal)));
                }
            }
        }

        // for jobstat
        private void addColumns1(DataTable dt, string[] col)
        {
            int j = col.Length;
            for (int i = 0; i < j; i++)
            {
                if (i <= 2)
                {
                    dt.Columns.Add(new DataColumn(col[i], typeof(string)));
                }
                else
                {
                    dt.Columns.Add(new DataColumn(col[i], typeof(int)));
                }
            }
        }


        // for csbill year
        private void addColumns2(DataTable dt, string[] col)
        {
            int j = col.Length;
            for (int i = 0; i < j; i++)
            {
                if ((i <= 2))
                {
                    dt.Columns.Add(new DataColumn(col[i], typeof(string)));
                }
                else
                {
                    dt.Columns.Add(new DataColumn(col[i], typeof(decimal)));
                }
            }
        }

        private void addColumns3(DataTable dt, string[] col)
        {
            int j = col.Length;
            for (int i = 0; i < j; i++)
            {
                if ((i < 1))
                {
                    dt.Columns.Add(new DataColumn(col[i], typeof(string)));
                }
                else
                {
                    dt.Columns.Add(new DataColumn(col[i], typeof(decimal)));
                }
            }
        }

        private void addColumns4(DataTable dt, string[] col)
        {
            int j = col.Length;
            for (int i = 0; i < j; i++)
            {
                if ((i < 2))
                {
                    dt.Columns.Add(new DataColumn(col[i], typeof(string)));
                }
                else
                {
                    dt.Columns.Add(new DataColumn(col[i], typeof(decimal)));
                }
            }
        }

        private void addColumns5(DataTable dt, string[] col)
        {
            int j = col.Length;
            for (int i = 0; i < j; i++)
            {
                if ((i < 2) || (i == j-1))
                {
                    dt.Columns.Add(new DataColumn(col[i], typeof(string)));
                }
                else
                {
                    dt.Columns.Add(new DataColumn(col[i], typeof(decimal)));
                }
            }
        }
    }


}