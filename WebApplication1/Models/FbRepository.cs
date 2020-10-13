using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data.SqlClient;
using FirebirdSql.Data.FirebirdClient;
using System.Data.Entity;
using System.Text.RegularExpressions;

namespace WebApplication1
{


    public class FbContext
    {
        private ASIDBConnection db = new ASIDBConnection();
        private static string _connectionName;

        public FbContext(string connectionName)

        {
            _connectionName = connectionName;
        }

        public static FbContext Create()
        {
            string conString = ConfigurationManager.ConnectionStrings["SirisCS"].ConnectionString;
            return new FbContext(conString);
        }

        public IEnumerable<TaxRecord> PopulateTaxes(DateTime dateFrom, DateTime dateTo)
        {
            using (FbConnection con = new FbConnection(_connectionName))
            {
                con.Open();
                FbCommand myCommand = new FbCommand();
                myCommand.Connection = con;

                myCommand.CommandText =
 @"select a.*, (select coname from cscomstr b where b.cono = a.cono), (select coregno from cscomstr b where b.cono = a.cono) from v_cono_tax2 a 
    where a.entdate >= @dateFrom and a.entdate <= @dateTo
order by a.entdate, 7 ";
                myCommand.Parameters.Add("@dateFrom", dateFrom);
                myCommand.Parameters.Add("@dateTo", dateTo);

                using (FbDataReader reader = myCommand.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        IList<TaxRecord> RecList = new List<TaxRecord>();
                        while (reader.Read())
                        {
                            TaxRecord newRec = new WebApplication1.TaxRecord();

                            newRec.entdate = reader.GetDateTime(reader.GetOrdinal("ENTDATE"));
                            newRec.cono = reader.GetString(reader.GetOrdinal("CONO"));
                            newRec.coregno = reader.GetString(reader.GetOrdinal("COREGNO"));
                            newRec.coname = reader.GetString(reader.GetOrdinal("CONAME"));
                            newRec.source = reader.GetString(reader.GetOrdinal("SOURCE"));
                            newRec.cnt = reader.GetInt32(reader.GetOrdinal("CNT"));
                            newRec.amt = reader.GetDecimal(reader.GetOrdinal("AMT"));
                            newRec.tax = reader.GetDecimal(reader.GetOrdinal("TAX"));


                            RecList.Add(newRec);
                        }
                        return RecList;
                    }
                }
                return null;
            }
        }


        public IEnumerable<TaxRecord> PopulateTaxesRcp(DateTime dateFrom, DateTime dateTo, string SortBy)
        {
            using (FbConnection con = new FbConnection(_connectionName))
            {
                con.Open();
                FbCommand myCommand = new FbCommand();
                myCommand.Connection = con;

                myCommand.CommandText =
    @"select a.*, (select coname from cscomstr b where b.cono = a.cono), (select coregno from cscomstr b where b.cono = a.cono)  from v_cono_taxrcp a 
    where a.entdate >= @dateFrom and a.entdate <= @dateTo
    order by a.entdate, 6";
                myCommand.Parameters.Add("@dateFrom", dateFrom);
                myCommand.Parameters.Add("@dateTo", dateTo);

                using (FbDataReader reader = myCommand.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        IList<TaxRecord> RecList = new List<TaxRecord>();
                        while (reader.Read())
                        {
                            TaxRecord newRec = new WebApplication1.TaxRecord();

                            newRec.entdate = reader.GetDateTime(reader.GetOrdinal("ENTDATE"));
                            newRec.cono = reader.GetString(reader.GetOrdinal("CONO"));
                            newRec.coregno = reader.GetString(reader.GetOrdinal("COREGNO"));
                            newRec.coname = reader.GetString(reader.GetOrdinal("CONAME"));
                            newRec.cnt = reader.GetInt32(reader.GetOrdinal("CNT"));
                            newRec.amt = -reader.GetDecimal(reader.GetOrdinal("AMT"));
                            newRec.tax = -reader.GetDecimal(reader.GetOrdinal("TAX"));


                            RecList.Add(newRec);
                        }
                        return RecList;
                    }
                }
                return null;
            }
        }



        public IEnumerable<CSCOMSTR> PopulateAuditor(string Auditor)
        {
            var conoList = GetAuditorConolist(Auditor);
            return db.CSCOMSTRs.Where(x => conoList.Contains(x.CONO));
        }

        public IEnumerable<string> GetAuditorConolist(string auditor)
        {
            using (FbConnection con = new FbConnection(_connectionName))
            {
                con.Open();
                FbCommand myCommand = new FbCommand();
                myCommand.Connection = con;

                if (!string.IsNullOrEmpty(auditor))
                {

                    myCommand.CommandText = @"select distinct a.cono from cscoadt a where adtcode = @auditor";


                    myCommand.Parameters.Add("@auditor", auditor);
                } else
                {
                    myCommand.CommandText = @"select distinct a.cono from cscoadt a";
                }
                 

                using (FbDataReader reader = myCommand.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        IList<string> conoList = new List<string>();
                        while (reader.Read())
                        {
                            conoList.Add(reader.GetString(reader.GetOrdinal("cono")));
                        }
                        return conoList;

                    }
                }
                return null;
            }
        }

        public IEnumerable<CSCOMSTR> PopulateTaxAgent(string TaxAgent)
        {
            var conoList = GetTaxAgentConolist(TaxAgent);
            return db.CSCOMSTRs.Where(x => conoList.Contains(x.CONO));
        }

        public IEnumerable<string> GetTaxAgentConolist(string taxagent)
        {
            using (FbConnection con = new FbConnection(_connectionName))
            {
                con.Open();
                FbCommand myCommand = new FbCommand();
                myCommand.Connection = con;

                if (!string.IsNullOrEmpty(taxagent))
                {

                    myCommand.CommandText = @"select distinct a.cono from cscotx a where txcode = @taxagent";


                    myCommand.Parameters.Add("@taxagent", taxagent);
                }
                else
                {
                    myCommand.CommandText = @"select distinct a.cono from cscotx a";
                }


                using (FbDataReader reader = myCommand.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        IList<string> conoList = new List<string>();
                        while (reader.Read())
                        {
                            conoList.Add(reader.GetString(reader.GetOrdinal("cono")));
                        }
                        return conoList;

                    }
                }
                return null;
            }
        }

        public IEnumerable<CSCOMSTR> PopulateCostatColist(int yearno, int monthno, int dayno, string status)
        {
            var conoList = GetCostatConolist(yearno, monthno, dayno, status);
            return db.CSCOMSTRs.Where(x => conoList.Contains(x.CONO));
        }

        public IEnumerable<string> GetCostatConolist(int yearno, int monthno, int dayno, string status)
        {
            using (FbConnection con = new FbConnection(_connectionName))
            {
                con.Open();
                FbCommand myCommand = new FbCommand();
                myCommand.Connection = con;

                if (string.IsNullOrEmpty(status))
                {
                    if (yearno != 0)
                    {
                        if (monthno != 0)
                        {
                            if (dayno != 0)
                            {
                                if (yearno == 2001)
                                {
                                    myCommand.CommandText = @"select a.cono from v_costat_cono a where yearno <= @yearno
                            and monthno =@monthno and dayno = @dayno ";
                                }
                                else
                                {
                                    myCommand.CommandText = @"select a.cono from v_costat_cono a where yearno = @yearno
                            and monthno =@monthno and dayno = @dayno ";
                                }

                                myCommand.Parameters.Add("@dayno", dayno);
                            }
                            else
                            {
                                if (yearno == 2001)
                                {
                                    myCommand.CommandText = @"select a.cono from v_costat_cono a where yearno <= @yearno
                            and monthno =@monthno  ";
                                }
                                else
                                {
                                    myCommand.CommandText = @"select a.cono from v_costat_cono a where yearno = @yearno
                            and monthno =@monthno  ";
                                }
                            }
                            myCommand.Parameters.Add("@monthno", monthno);
                        }
                        else
                        {
                            if (yearno == 2001)
                            {
                                myCommand.CommandText = @"select a.cono from v_costat_cono a where yearno <= @yearno  ";
                            }
                            else
                            {
                                myCommand.CommandText = @"select a.cono from v_costat_cono a where yearno = @yearno  ";
                            }

                        }
                        myCommand.Parameters.Add("@yearno", yearno);

                    }
                }
                else
                {
                    if (yearno != 0)
                    {
                        if (monthno != 0)
                        {
                            if (dayno != 0)
                            {
                                if (yearno == 2001)
                                {
                                    myCommand.CommandText = @"select a.cono from v_costat_cono a where yearno <= @yearno
                            and monthno =@monthno and dayno = @dayno and costat = @costat ";
                                }
                                else
                                {
                                    myCommand.CommandText = @"select a.cono from v_costat_cono a where yearno = @yearno
                            and monthno =@monthno and dayno = @dayno and costat = @costat ";
                                }

                                myCommand.Parameters.Add("@dayno", dayno);
                            }
                            else
                            {
                                if (yearno == 2001)
                                {
                                    myCommand.CommandText = @"select a.cono from v_costat_cono a where yearno <= @yearno
                            and monthno =@monthno and costat = @costat ";
                                }
                                else
                                {
                                    myCommand.CommandText = @"select a.cono from v_costat_cono a where yearno = @yearno
                            and monthno =@monthno and costat = @costat ";
                                }
                            }
                            myCommand.Parameters.Add("@monthno", monthno);
                        }
                        else
                        {
                            if (yearno == 2001)
                            {
                                myCommand.CommandText = @"select a.cono from v_costat_cono a where yearno <= @yearno
                            and costat = @costat ";
                            }
                            else
                            {
                                myCommand.CommandText = @"select a.cono from v_costat_cono a where yearno = @yearno
                            and costat = @costat ";
                            }

                        }
                        myCommand.Parameters.Add("@yearno", yearno);

                    }
                    else
                    {
                        myCommand.CommandText = @"select a.cono from v_costat_cono a where costat = @costat ";

                    }
                    myCommand.Parameters.Add("@costat", status.Trim());
                }

                using (FbDataReader reader = myCommand.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        IList<string> conoList = new List<string>();
                        while (reader.Read())
                        {
                            conoList.Add(reader.GetString(reader.GetOrdinal("cono")));
                        }
                        return conoList;

                    }
                }
                return null;
            }
        }

        public IEnumerable<CostatYearRecord> PopulateCostatYear()
        {
            using (FbConnection con = new FbConnection(_connectionName))
            {
                con.Open();
                FbCommand myCommand = new FbCommand();
                myCommand.Connection = con;


                myCommand.CommandText = @"select a.* from v_costat_sumivtab_year a ";


                using (FbDataReader reader = myCommand.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        IList<CostatYearRecord> RecList = new List<CostatYearRecord>();
                        while (reader.Read())
                        {
                            CostatYearRecord newRec = new CostatYearRecord();



                            newRec.costat = reader.GetString(reader.GetOrdinal("COSTAT"));



                            newRec.y2001 = reader.GetInt32(reader.GetOrdinal("C01"));
                            newRec.y2002 = reader.GetInt32(reader.GetOrdinal("C02"));
                            newRec.y2003 = reader.GetInt32(reader.GetOrdinal("C03"));
                            newRec.y2004 = reader.GetInt32(reader.GetOrdinal("C04"));
                            newRec.y2005 = reader.GetInt32(reader.GetOrdinal("C05"));
                            newRec.y2006 = reader.GetInt32(reader.GetOrdinal("C06"));
                            newRec.y2007 = reader.GetInt32(reader.GetOrdinal("C07"));
                            newRec.y2008 = reader.GetInt32(reader.GetOrdinal("C08"));
                            newRec.y2009 = reader.GetInt32(reader.GetOrdinal("C09"));
                            newRec.y2010 = reader.GetInt32(reader.GetOrdinal("C10"));
                            newRec.y2011 = reader.GetInt32(reader.GetOrdinal("C11"));
                            newRec.y2012 = reader.GetInt32(reader.GetOrdinal("C12"));
                            newRec.y2013 = reader.GetInt32(reader.GetOrdinal("C13"));
                            newRec.y2014 = reader.GetInt32(reader.GetOrdinal("C14"));
                            newRec.y2015 = reader.GetInt32(reader.GetOrdinal("C15"));
                            newRec.y2016 = reader.GetInt32(reader.GetOrdinal("C16"));
                            newRec.y2017 = reader.GetInt32(reader.GetOrdinal("C17"));
                            newRec.y2018 = reader.GetInt32(reader.GetOrdinal("C18"));
                            newRec.y2019 = reader.GetInt32(reader.GetOrdinal("C19"));
                            newRec.y2020 = reader.GetInt32(reader.GetOrdinal("C20"));
                            newRec.y2021 = reader.GetInt32(reader.GetOrdinal("C21"));
                            newRec.y2022 = reader.GetInt32(reader.GetOrdinal("C22"));
                            newRec.y2023 = reader.GetInt32(reader.GetOrdinal("C23"));
                            newRec.y2024 = reader.GetInt32(reader.GetOrdinal("C24"));
                            newRec.y2025 = reader.GetInt32(reader.GetOrdinal("C25"));
                            newRec.y2026 = reader.GetInt32(reader.GetOrdinal("C26"));
                            newRec.y2027 = reader.GetInt32(reader.GetOrdinal("C27"));
                            newRec.y2028 = reader.GetInt32(reader.GetOrdinal("C28"));
                            newRec.y2029 = reader.GetInt32(reader.GetOrdinal("C29"));
                            newRec.y2030 = reader.GetInt32(reader.GetOrdinal("C30"));
                            newRec.y2031 = reader.GetInt32(reader.GetOrdinal("C31"));
                            newRec.y2032 = reader.GetInt32(reader.GetOrdinal("C32"));
                            newRec.y2033 = reader.GetInt32(reader.GetOrdinal("C33"));
                            newRec.y2034 = reader.GetInt32(reader.GetOrdinal("C34"));
                            newRec.y2035 = reader.GetInt32(reader.GetOrdinal("C35"));
                            newRec.y2036 = reader.GetInt32(reader.GetOrdinal("C36"));
                            RecList.Add(newRec);
                        }
                        return RecList;
                    }
                }
                return null;
            }
        }

        public IEnumerable<CostatMthRecord> PopulateCostatMonth(int year)
        {
            using (FbConnection con = new FbConnection(_connectionName))
            {
                con.Open();
                FbCommand myCommand = new FbCommand();
                myCommand.Connection = con;


                myCommand.CommandText = @"select a.* from v_costat_sumivtab_mth a where yearno = @yearno";
                myCommand.Parameters.Add("@yearno", year);


                using (FbDataReader reader = myCommand.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        IList<CostatMthRecord> RecList = new List<CostatMthRecord>();
                        while (reader.Read())
                        {
                            CostatMthRecord newRec = new CostatMthRecord();



                            newRec.costat = reader.GetString(reader.GetOrdinal("COSTAT"));

                            newRec.yearno = reader.GetInt32(reader.GetOrdinal("YEARNO"));

                            newRec.jan = reader.GetInt32(reader.GetOrdinal("C01"));
                            newRec.feb = reader.GetInt32(reader.GetOrdinal("C02"));
                            newRec.mar = reader.GetInt32(reader.GetOrdinal("C03"));
                            newRec.apr = reader.GetInt32(reader.GetOrdinal("C04"));
                            newRec.may = reader.GetInt32(reader.GetOrdinal("C05"));
                            newRec.jun = reader.GetInt32(reader.GetOrdinal("C06"));
                            newRec.jul = reader.GetInt32(reader.GetOrdinal("C07"));
                            newRec.aug = reader.GetInt32(reader.GetOrdinal("C08"));
                            newRec.sep = reader.GetInt32(reader.GetOrdinal("C09"));
                            newRec.oct = reader.GetInt32(reader.GetOrdinal("C10"));
                            newRec.nov = reader.GetInt32(reader.GetOrdinal("C11"));
                            newRec.dec = reader.GetInt32(reader.GetOrdinal("C12"));

                            RecList.Add(newRec);
                        }
                        return RecList;
                    }
                }
                return null;
            }
        }

        public IEnumerable<CostatDayRecord> PopulateCostatDay(int year, int month)
        {
            using (FbConnection con = new FbConnection(_connectionName))
            {
                con.Open();
                FbCommand myCommand = new FbCommand();
                myCommand.Connection = con;


                myCommand.CommandText = @"select a.* from v_costat_sumivtab_day a
                where yearno = @yearno and monthno = @monthno";
                myCommand.Parameters.Add("@yearno", year);
                myCommand.Parameters.Add("@monthno", month);


                using (FbDataReader reader = myCommand.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        IList<CostatDayRecord> RecList = new List<CostatDayRecord>();
                        while (reader.Read())
                        {
                            CostatDayRecord newRec = new CostatDayRecord();



                            newRec.costat = reader.GetString(reader.GetOrdinal("COSTAT"));

                            newRec.yearno = reader.GetInt32(reader.GetOrdinal("YEARNO"));
                            newRec.monthno = reader.GetInt32(reader.GetOrdinal("MONTHNO"));


                            newRec.D01 = reader.GetInt32(reader.GetOrdinal("C01"));
                            newRec.D02 = reader.GetInt32(reader.GetOrdinal("C02"));
                            newRec.D03 = reader.GetInt32(reader.GetOrdinal("C03"));
                            newRec.D04 = reader.GetInt32(reader.GetOrdinal("C04"));
                            newRec.D05 = reader.GetInt32(reader.GetOrdinal("C05"));
                            newRec.D06 = reader.GetInt32(reader.GetOrdinal("C06"));
                            newRec.D07 = reader.GetInt32(reader.GetOrdinal("C07"));
                            newRec.D08 = reader.GetInt32(reader.GetOrdinal("C08"));
                            newRec.D09 = reader.GetInt32(reader.GetOrdinal("C09"));
                            newRec.D10 = reader.GetInt32(reader.GetOrdinal("C10"));
                            newRec.D11 = reader.GetInt32(reader.GetOrdinal("C11"));
                            newRec.D12 = reader.GetInt32(reader.GetOrdinal("C12"));
                            newRec.D13 = reader.GetInt32(reader.GetOrdinal("C13"));
                            newRec.D14 = reader.GetInt32(reader.GetOrdinal("C14"));
                            newRec.D15 = reader.GetInt32(reader.GetOrdinal("C15"));
                            newRec.D16 = reader.GetInt32(reader.GetOrdinal("C16"));
                            newRec.D17 = reader.GetInt32(reader.GetOrdinal("C17"));
                            newRec.D18 = reader.GetInt32(reader.GetOrdinal("C18"));
                            newRec.D19 = reader.GetInt32(reader.GetOrdinal("C19"));
                            newRec.D20 = reader.GetInt32(reader.GetOrdinal("C20"));
                            newRec.D21 = reader.GetInt32(reader.GetOrdinal("C21"));
                            newRec.D22 = reader.GetInt32(reader.GetOrdinal("C22"));
                            newRec.D23 = reader.GetInt32(reader.GetOrdinal("C23"));
                            newRec.D24 = reader.GetInt32(reader.GetOrdinal("C24"));
                            newRec.D25 = reader.GetInt32(reader.GetOrdinal("C25"));
                            newRec.D26 = reader.GetInt32(reader.GetOrdinal("C26"));
                            newRec.D27 = reader.GetInt32(reader.GetOrdinal("C27"));
                            newRec.D28 = reader.GetInt32(reader.GetOrdinal("C28"));
                            newRec.D29 = reader.GetInt32(reader.GetOrdinal("C29"));
                            newRec.D30 = reader.GetInt32(reader.GetOrdinal("C30"));
                            newRec.D31 = reader.GetInt32(reader.GetOrdinal("C31"));
                            newRec.D32 = reader.GetInt32(reader.GetOrdinal("C32"));
                            newRec.D33 = reader.GetInt32(reader.GetOrdinal("C33"));
                            newRec.D34 = reader.GetInt32(reader.GetOrdinal("C34"));
                            newRec.D35 = reader.GetInt32(reader.GetOrdinal("C35"));
                            newRec.D36 = reader.GetInt32(reader.GetOrdinal("C36"));

                            RecList.Add(newRec);
                        }
                        return RecList;
                    }
                }
                return null;
            }
        }

        public IEnumerable<PrfBillRecord> PopulatePrfBill()
        {
            using (FbConnection con = new FbConnection(_connectionName))
            {
                con.Open();
                FbCommand myCommand = new FbCommand();
                myCommand.Connection = con;

                myCommand.CommandText = @"select a.* from v_csbill_sum_jobstaff_prfalloc a ";


                using (FbDataReader reader = myCommand.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        IList<PrfBillRecord> RecList = new List<PrfBillRecord>();
                        while (reader.Read())
                        {
                            PrfBillRecord newRec = new WebApplication1.PrfBillRecord();

                            newRec.yearno = reader.GetInt32(reader.GetOrdinal("YEARNO"));
                            newRec.mthno = reader.GetInt32(reader.GetOrdinal("MTHNO"));
                            newRec.casecode = reader.GetString(reader.GetOrdinal("CASECODE"));
                            newRec.itemtype = reader.GetString(reader.GetOrdinal("ITEMTYPE"));
                            newRec.staff = reader.GetString(reader.GetOrdinal("JOBSTAFF"));
                            newRec.cnt = reader.GetInt32(reader.GetOrdinal("CNT"));
                            newRec.amt = reader.GetDecimal(reader.GetOrdinal("AMT"));
                            newRec.tax = reader.GetDecimal(reader.GetOrdinal("TAX"));
                            newRec.net = reader.GetDecimal(reader.GetOrdinal("NET"));


                            RecList.Add(newRec);
                        }
                        return RecList;
                    }
                }
                return null;
            }
        }

        public IEnumerable<PrfBillYearRecord> PopulatePrfBillYear(string AmountType, string Category)
        {
            using (FbConnection con = new FbConnection(_connectionName))
            {
                con.Open();
                FbCommand myCommand = new FbCommand();
                myCommand.Connection = con;

                if (Category == "CASECODE")
                {
                    if (AmountType == "AMOUNT")
                    {
                        myCommand.CommandText = @"select a.* from v_prfbill_sumivtab_ownamt a ";
                    }
                    else if (AmountType == "NET")
                    {
                        myCommand.CommandText = @"select a.* from v_prfbill_sumivtab a ";
                    }
                    else if (AmountType == "TAX")
                    {
                        myCommand.CommandText = @"select a.* from v_prfbill_sumivtab_tax a ";
                    }
                    else if (AmountType == "OTHER")
                    {
                        myCommand.CommandText = @"select a.* from v_prfbill_sumivtab_other a ";
                    }
                    else if (AmountType == "TOTAL")
                    {
                        myCommand.CommandText = @"select a.* from v_prfbill_sumivtab_total a ";
                    }
                }
                else
                {
                    if (AmountType == "AMOUNT")
                    {
                        myCommand.CommandText = @"select a.* from v_prfbill_sumivtab1_ownamt a ";
                    }
                    else if (AmountType == "NET")
                    {
                        myCommand.CommandText = @"select a.* from v_prfbill_sumivtab1 a ";
                    }
                    else if (AmountType == "TAX")
                    {
                        myCommand.CommandText = @"select a.* from v_prfbill_sumivtab1_tax a ";
                    }
                    if (AmountType == "OTHER")
                    {
                        myCommand.CommandText = @"select a.* from v_prfbill_sumivtab1_other a ";
                    }
                    else if (AmountType == "TOTAL")
                    {
                        myCommand.CommandText = @"select a.* from v_prfbill_sumivtab1_total a ";
                    }
                }


                using (FbDataReader reader = myCommand.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        IList<PrfBillYearRecord> RecList = new List<PrfBillYearRecord>();
                        while (reader.Read())
                        {
                            PrfBillYearRecord newRec = new PrfBillYearRecord();

                            if (Category == "CASECODE")
                            {
                                newRec.casecode = reader.GetString(reader.GetOrdinal("CASECODE"));
                                newRec.casedesc = reader.GetString(reader.GetOrdinal("CASEDESC"));
                            }
                            else
                            {
                                newRec.casecode = reader.GetString(reader.GetOrdinal("ITEMTYPE"));
                                newRec.casedesc = reader.GetString(reader.GetOrdinal("ITEMDESC"));
                            }

                            newRec.y2001 = reader.GetDecimal(reader.GetOrdinal("C01"));
                            newRec.y2002 = reader.GetDecimal(reader.GetOrdinal("C02"));
                            newRec.y2003 = reader.GetDecimal(reader.GetOrdinal("C03"));
                            newRec.y2004 = reader.GetDecimal(reader.GetOrdinal("C04"));
                            newRec.y2005 = reader.GetDecimal(reader.GetOrdinal("C05"));
                            newRec.y2006 = reader.GetDecimal(reader.GetOrdinal("C06"));
                            newRec.y2007 = reader.GetDecimal(reader.GetOrdinal("C07"));
                            newRec.y2008 = reader.GetDecimal(reader.GetOrdinal("C08"));
                            newRec.y2009 = reader.GetDecimal(reader.GetOrdinal("C09"));
                            newRec.y2010 = reader.GetDecimal(reader.GetOrdinal("C10"));
                            newRec.y2011 = reader.GetDecimal(reader.GetOrdinal("C11"));
                            newRec.y2012 = reader.GetDecimal(reader.GetOrdinal("C12"));
                            newRec.y2013 = reader.GetDecimal(reader.GetOrdinal("C13"));
                            newRec.y2014 = reader.GetDecimal(reader.GetOrdinal("C14"));
                            newRec.y2015 = reader.GetDecimal(reader.GetOrdinal("C15"));
                            newRec.y2016 = reader.GetDecimal(reader.GetOrdinal("C16"));
                            newRec.y2017 = reader.GetDecimal(reader.GetOrdinal("C17"));
                            newRec.y2018 = reader.GetDecimal(reader.GetOrdinal("C18"));
                            newRec.y2019 = reader.GetDecimal(reader.GetOrdinal("C19"));
                            newRec.y2020 = reader.GetDecimal(reader.GetOrdinal("C20"));
                            newRec.y2021 = reader.GetDecimal(reader.GetOrdinal("C21"));
                            newRec.y2022 = reader.GetDecimal(reader.GetOrdinal("C22"));
                            newRec.y2023 = reader.GetDecimal(reader.GetOrdinal("C23"));
                            newRec.y2024 = reader.GetDecimal(reader.GetOrdinal("C24"));
                            newRec.y2025 = reader.GetDecimal(reader.GetOrdinal("C25"));
                            newRec.y2026 = reader.GetDecimal(reader.GetOrdinal("C26"));
                            newRec.y2027 = reader.GetDecimal(reader.GetOrdinal("C27"));
                            newRec.y2028 = reader.GetDecimal(reader.GetOrdinal("C28"));
                            newRec.y2029 = reader.GetDecimal(reader.GetOrdinal("C29"));
                            newRec.y2030 = reader.GetDecimal(reader.GetOrdinal("C30"));
                            newRec.y2031 = reader.GetDecimal(reader.GetOrdinal("C31"));
                            newRec.y2032 = reader.GetDecimal(reader.GetOrdinal("C32"));
                            newRec.y2033 = reader.GetDecimal(reader.GetOrdinal("C33"));
                            newRec.y2034 = reader.GetDecimal(reader.GetOrdinal("C34"));
                            newRec.y2035 = reader.GetDecimal(reader.GetOrdinal("C35"));
                            newRec.y2036 = reader.GetDecimal(reader.GetOrdinal("C36"));
                            RecList.Add(newRec);
                        }
                        return RecList;
                    }
                }
                return null;
            }
        }

        public IEnumerable<PrfBillStaffRecord> PopulatePrfBillStaff(int year, string AmountType, string Category)
        {
            using (FbConnection con = new FbConnection(_connectionName))
            {
                con.Open();
                FbCommand myCommand = new FbCommand();
                myCommand.Connection = con;
                if (Category == "CASECODE")
                {
                    if (AmountType == "AMOUNT")
                    {
                        myCommand.CommandText = @"select a.* from v_prfbill_sum_iv_staff_ownamt a where a.yearno = @yearno";
                        myCommand.Parameters.Add("@yearno", year);
                    }
                    else if (AmountType == "NET")
                    {
                        myCommand.CommandText = @"select a.* from v_prfbill_sum_ivtab_staff a  where a.yearno = @yearno";
                        myCommand.Parameters.Add("@yearno", year);
                    }
                    else if (AmountType == "TAX")
                    {
                        myCommand.CommandText = @"select a.* from v_prfbill_sum_ivtab_staff_tax a  where a.yearno = @yearno";
                        myCommand.Parameters.Add("@yearno", year);
                    }
                    else if (AmountType == "OTHER")
                    {
                        myCommand.CommandText = @"select a.* from v_prfbill_sum_ivtab_stf_other a  where a.yearno = @yearno";
                        myCommand.Parameters.Add("@yearno", year);
                    }
                    else if (AmountType == "TOTAL")
                    {
                        myCommand.CommandText = @"select a.* from v_prfbill_sum_ivtab_stf_total a  where a.yearno = @yearno";
                        myCommand.Parameters.Add("@yearno", year);
                    }
                }
                else
                {
                    if (AmountType == "AMOUNT")
                    {
                        myCommand.CommandText = @"select a.* from v_prfbill_sum_iv1_staff_ownamt a where a.yearno = @yearno";
                        myCommand.Parameters.Add("@yearno", year);
                    }
                    else if (AmountType == "NET")
                    {
                        myCommand.CommandText = @"select a.* from v_prfbill_sum_ivtab1_staff a  where a.yearno = @yearno";
                        myCommand.Parameters.Add("@yearno", year);
                    }
                    else if (AmountType == "TAX")
                    {
                        myCommand.CommandText = @"select a.* from v_prfbill_sum_ivtab1_staff_tax a  where a.yearno = @yearno";
                        myCommand.Parameters.Add("@yearno", year);
                    }
                    else if (AmountType == "OTHER")
                    {
                        myCommand.CommandText = @"select a.* from v_prfbill_sum_ivtab1_stf_other a  where a.yearno = @yearno";
                        myCommand.Parameters.Add("@yearno", year);
                    }
                    else if (AmountType == "TOTAL")
                    {
                        myCommand.CommandText = @"select a.* from v_prfbill_sum_ivtab1_stf_total a  where a.yearno = @yearno";
                        myCommand.Parameters.Add("@yearno", year);
                    }
                }


                using (FbDataReader reader = myCommand.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        IList<PrfBillStaffRecord> RecList = new List<PrfBillStaffRecord>();
                        while (reader.Read())
                        {
                            PrfBillStaffRecord newRec = new PrfBillStaffRecord();

                            if (Category == "CASECODE")
                            {
                                newRec.casecode = reader.GetString(reader.GetOrdinal("CASECODE"));
                                newRec.casedesc = reader.GetString(reader.GetOrdinal("CASEDESC"));
                            }
                            else
                            {
                                newRec.casecode = reader.GetString(reader.GetOrdinal("ITEMTYPE"));
                                newRec.casedesc = reader.GetString(reader.GetOrdinal("ITEMDESC"));
                            }
                            newRec.yearno = reader.GetInt32(reader.GetOrdinal("YEARNO"));

                            newRec.c01 = reader.GetDecimal(reader.GetOrdinal("C01"));
                            newRec.c02 = reader.GetDecimal(reader.GetOrdinal("C02"));
                            newRec.c03 = reader.GetDecimal(reader.GetOrdinal("C03"));
                            newRec.c04 = reader.GetDecimal(reader.GetOrdinal("C04"));
                            newRec.c05 = reader.GetDecimal(reader.GetOrdinal("C05"));
                            newRec.c06 = reader.GetDecimal(reader.GetOrdinal("C06"));
                            newRec.c07 = reader.GetDecimal(reader.GetOrdinal("C07"));
                            newRec.c08 = reader.GetDecimal(reader.GetOrdinal("C08"));
                            newRec.c09 = reader.GetDecimal(reader.GetOrdinal("C09"));
                            newRec.c10 = reader.GetDecimal(reader.GetOrdinal("C10"));
                            newRec.c11 = reader.GetDecimal(reader.GetOrdinal("C11"));
                            newRec.c12 = reader.GetDecimal(reader.GetOrdinal("C12"));
                            newRec.c13 = reader.GetDecimal(reader.GetOrdinal("C13"));
                            newRec.c14 = reader.GetDecimal(reader.GetOrdinal("C14"));
                            newRec.c15 = reader.GetDecimal(reader.GetOrdinal("C15"));
                            newRec.c16 = reader.GetDecimal(reader.GetOrdinal("C16"));
                            newRec.c17 = reader.GetDecimal(reader.GetOrdinal("C17"));
                            newRec.c18 = reader.GetDecimal(reader.GetOrdinal("C18"));
                            RecList.Add(newRec);
                        }
                        return RecList;
                    }
                }
                return null;
            }
        }

        public IEnumerable<PrfBillStaffRecord> PopulatePrfBillStaffPortfolio(int year, string AmountType, string Category)
        {
            using (FbConnection con = new FbConnection(_connectionName))
            {
                con.Open();
                FbCommand myCommand = new FbCommand();
                myCommand.Connection = con;
                if (Category == "CASECODE")
                {
                    if (AmountType == "AMOUNT")
                    {
                        myCommand.CommandText = @"select a.* from V_PRFBILL_SUM_IV_STFPF_OWNAMT a where a.yearno = @yearno";
                        myCommand.Parameters.Add("@yearno", year);
                    }
                    else if (AmountType == "NET")
                    {
                        myCommand.CommandText = @"select a.* from V_PRFBILL_SUM_IV_STFP  a  where a.yearno = @yearno";
                        myCommand.Parameters.Add("@yearno", year);
                    }
                    else if (AmountType == "TAX")
                    {
                        myCommand.CommandText = @"select a.* from V_PRFBILL_SUM_IV_STFPF_TAX  a  where a.yearno = @yearno";
                        myCommand.Parameters.Add("@yearno", year);
                    }
                    else if (AmountType == "OTHER")
                    {
                        myCommand.CommandText = @"select a.* from V_PRFBILL_SUM_IV_STFPF_OTHER  a  where a.yearno = @yearno";
                        myCommand.Parameters.Add("@yearno", year);
                    }
                    else if (AmountType == "TOTAL")
                    {
                        myCommand.CommandText = @"select a.* from V_PRFBILL_SUM_IV_STFPF_TOTAL  a  where a.yearno = @yearno";
                        myCommand.Parameters.Add("@yearno", year);
                    }
                }
                else
                {
                    if (AmountType == "AMOUNT")
                    {
                        myCommand.CommandText = @"select a.* from V_PRFBILL_SUM_IV1_STFPF_OWNAMT a where a.yearno = @yearno";
                        myCommand.Parameters.Add("@yearno", year);
                    }
                    else if (AmountType == "NET")
                    {
                        myCommand.CommandText = @"select a.* from V_PRFBILL_SUM_IV1_STFP  a  where a.yearno = @yearno";
                        myCommand.Parameters.Add("@yearno", year);
                    }
                    else if (AmountType == "TAX")
                    {
                        myCommand.CommandText = @"select a.* from V_PRFBILL_SUM_IV1_STFPF_TAX  a  where a.yearno = @yearno";
                        myCommand.Parameters.Add("@yearno", year);
                    }
                    else if (AmountType == "OTHER")
                    {
                        myCommand.CommandText = @"select a.* from V_PRFBILL_SUM_IV1_STFPF_OTHER  a  where a.yearno = @yearno";
                        myCommand.Parameters.Add("@yearno", year);
                    }
                    else if (AmountType == "TOTAL")
                    {
                        myCommand.CommandText = @"select a.* from V_PRFBILL_SUM_IV1_STFPF_TOTAL  a  where a.yearno = @yearno";
                        myCommand.Parameters.Add("@yearno", year);
                    }
                }


                using (FbDataReader reader = myCommand.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        IList<PrfBillStaffRecord> RecList = new List<PrfBillStaffRecord>();
                        while (reader.Read())
                        {
                            PrfBillStaffRecord newRec = new PrfBillStaffRecord();

                            if (Category == "CASECODE")
                            {
                                newRec.casecode = reader.GetString(reader.GetOrdinal("CASECODE"));
                                newRec.casedesc = reader.GetString(reader.GetOrdinal("CASEDESC"));
                            }
                            else
                            {
                                newRec.casecode = reader.GetString(reader.GetOrdinal("ITEMTYPE"));
                                newRec.casedesc = reader.GetString(reader.GetOrdinal("ITEMDESC"));
                            }
                            newRec.yearno = reader.GetInt32(reader.GetOrdinal("YEARNO"));

                            newRec.c01 = reader.GetDecimal(reader.GetOrdinal("C01"));
                            newRec.c02 = reader.GetDecimal(reader.GetOrdinal("C02"));
                            newRec.c03 = reader.GetDecimal(reader.GetOrdinal("C03"));
                            newRec.c04 = reader.GetDecimal(reader.GetOrdinal("C04"));
                            newRec.c05 = reader.GetDecimal(reader.GetOrdinal("C05"));
                            newRec.c06 = reader.GetDecimal(reader.GetOrdinal("C06"));
                            newRec.c07 = reader.GetDecimal(reader.GetOrdinal("C07"));
                            newRec.c08 = reader.GetDecimal(reader.GetOrdinal("C08"));
                            newRec.c09 = reader.GetDecimal(reader.GetOrdinal("C09"));
                            newRec.c10 = reader.GetDecimal(reader.GetOrdinal("C10"));
                            newRec.c11 = reader.GetDecimal(reader.GetOrdinal("C11"));
                            newRec.c12 = reader.GetDecimal(reader.GetOrdinal("C12"));
                            newRec.c13 = reader.GetDecimal(reader.GetOrdinal("C13"));
                            newRec.c14 = reader.GetDecimal(reader.GetOrdinal("C14"));
                            newRec.c15 = reader.GetDecimal(reader.GetOrdinal("C15"));
                            newRec.c16 = reader.GetDecimal(reader.GetOrdinal("C16"));
                            newRec.c17 = reader.GetDecimal(reader.GetOrdinal("C17"));
                            newRec.c18 = reader.GetDecimal(reader.GetOrdinal("C18"));
                            RecList.Add(newRec);
                        }
                        return RecList;
                    }
                }
                return null;
            }
        }

        public IEnumerable<PrfBillStaffRecord> PopulatePrfBillStaffCombo(int year, string AmountType, string Category)
        {
            using (FbConnection con = new FbConnection(_connectionName))
            {
                con.Open();
                FbCommand myCommand = new FbCommand();
                myCommand.Connection = con;
                if (Category == "CASECODE")
                {
                    if (AmountType == "AMOUNT")
                    {
                        myCommand.CommandText = @"select a.* from V_PRFBILL_SUM_IV_STFCB_OWNAMT a where a.yearno = @yearno";
                        myCommand.Parameters.Add("@yearno", year);
                    }
                    else if (AmountType == "NET")
                    {
                        myCommand.CommandText = @"select a.* from V_PRFBILL_SUM_IV_STCB  a  where a.yearno = @yearno";
                        myCommand.Parameters.Add("@yearno", year);
                    }
                    else if (AmountType == "TAX")
                    {
                        myCommand.CommandText = @"select a.* from V_PRFBILL_SUM_IV_STFCB_TAX  a  where a.yearno = @yearno";
                        myCommand.Parameters.Add("@yearno", year);
                    }
                    else if (AmountType == "OTHER")
                    {
                        myCommand.CommandText = @"select a.* from V_PRFBILL_SUM_IV_STFCB_OTHER  a  where a.yearno = @yearno";
                        myCommand.Parameters.Add("@yearno", year);
                    }
                    else if (AmountType == "TOTAL")
                    {
                        myCommand.CommandText = @"select a.* from V_PRFBILL_SUM_IV_STFCB_TOTAL  a  where a.yearno = @yearno";
                        myCommand.Parameters.Add("@yearno", year);
                    }
                }
                else
                {
                    if (AmountType == "AMOUNT")
                    {
                        myCommand.CommandText = @"select a.* from V_PRFBILL_SUM_IV1_STFCB_OWNAMT a where a.yearno = @yearno";
                        myCommand.Parameters.Add("@yearno", year);
                    }
                    else if (AmountType == "NET")
                    {
                        myCommand.CommandText = @"select a.* from V_PRFBILL_SUM_IV1_STCB  a  where a.yearno = @yearno";
                        myCommand.Parameters.Add("@yearno", year);
                    }
                    else if (AmountType == "TAX")
                    {
                        myCommand.CommandText = @"select a.* from V_PRFBILL_SUM_IV1_STFCB_TAX  a  where a.yearno = @yearno";
                        myCommand.Parameters.Add("@yearno", year);
                    }
                    else if (AmountType == "OTHER")
                    {
                        myCommand.CommandText = @"select a.* from V_PRFBILL_SUM_IV1_STFCB_OTHER  a  where a.yearno = @yearno";
                        myCommand.Parameters.Add("@yearno", year);
                    }
                    else if (AmountType == "TOTAL")
                    {
                        myCommand.CommandText = @"select a.* from V_PRFBILL_SUM_IV1_STFCB_TOTAL  a  where a.yearno = @yearno";
                        myCommand.Parameters.Add("@yearno", year);
                    }
                }


                using (FbDataReader reader = myCommand.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        IList<PrfBillStaffRecord> RecList = new List<PrfBillStaffRecord>();
                        while (reader.Read())
                        {
                            PrfBillStaffRecord newRec = new PrfBillStaffRecord();

                            if (Category == "CASECODE")
                            {
                                newRec.casecode = reader.GetString(reader.GetOrdinal("CASECODE"));
                                newRec.casedesc = reader.GetString(reader.GetOrdinal("CASEDESC"));
                            }
                            else
                            {
                                newRec.casecode = reader.GetString(reader.GetOrdinal("ITEMTYPE"));
                                newRec.casedesc = reader.GetString(reader.GetOrdinal("ITEMDESC"));
                            }
                            newRec.yearno = reader.GetInt32(reader.GetOrdinal("YEARNO"));

                            newRec.c01 = reader.GetDecimal(reader.GetOrdinal("C01"));
                            newRec.c02 = reader.GetDecimal(reader.GetOrdinal("C02"));
                            newRec.c03 = reader.GetDecimal(reader.GetOrdinal("C03"));
                            newRec.c04 = reader.GetDecimal(reader.GetOrdinal("C04"));
                            newRec.c05 = reader.GetDecimal(reader.GetOrdinal("C05"));
                            newRec.c06 = reader.GetDecimal(reader.GetOrdinal("C06"));
                            newRec.c07 = reader.GetDecimal(reader.GetOrdinal("C07"));
                            newRec.c08 = reader.GetDecimal(reader.GetOrdinal("C08"));
                            newRec.c09 = reader.GetDecimal(reader.GetOrdinal("C09"));
                            newRec.c10 = reader.GetDecimal(reader.GetOrdinal("C10"));
                            newRec.c11 = reader.GetDecimal(reader.GetOrdinal("C11"));
                            newRec.c12 = reader.GetDecimal(reader.GetOrdinal("C12"));
                            newRec.c13 = reader.GetDecimal(reader.GetOrdinal("C13"));
                            newRec.c14 = reader.GetDecimal(reader.GetOrdinal("C14"));
                            newRec.c15 = reader.GetDecimal(reader.GetOrdinal("C15"));
                            newRec.c16 = reader.GetDecimal(reader.GetOrdinal("C16"));
                            newRec.c17 = reader.GetDecimal(reader.GetOrdinal("C17"));
                            newRec.c18 = reader.GetDecimal(reader.GetOrdinal("C18"));
                            RecList.Add(newRec);
                        }
                        return RecList;
                    }
                }
                return null;
            }
        }

        public IEnumerable<PrfBillStaffRecord> PopulatePrfBillStaffMonth(int year, int month, string AmountType, string Category)
        {
            using (FbConnection con = new FbConnection(_connectionName))
            {
                con.Open();
                FbCommand myCommand = new FbCommand();
                myCommand.Connection = con;
                if (Category == "CASECODE")
                {
                    if (AmountType == "AMOUNT")
                    {
                        myCommand.CommandText = @"select a.* from v_prfbill_sum_iv_stf_mth_ownamt a where a.yearno = @yearno and a.mthno = @mthno";
                        myCommand.Parameters.Add("@yearno", year);
                        myCommand.Parameters.Add("@mthno", month);
                    }
                    else if (AmountType == "NET")
                    {
                        myCommand.CommandText = @"select a.* from v_prfbill_sum_ivtab_staff_mth a  where a.yearno = @yearno and a.mthno = @mthno";
                        myCommand.Parameters.Add("@yearno", year);
                        myCommand.Parameters.Add("@mthno", month);
                    }
                    else if (AmountType == "TAX")
                    {
                        myCommand.CommandText = @"select a.* from v_prfbill_sum_iv_stf_mth_tax a  where a.yearno = @yearno and a.mthno = @mthno";
                        myCommand.Parameters.Add("@yearno", year);
                        myCommand.Parameters.Add("@mthno", month);
                    }
                    else if (AmountType == "OTHER")
                    {
                        myCommand.CommandText = @"select a.* from v_prfbill_sum_iv_stf_mth_other a  where a.yearno = @yearno and a.mthno = @mthno";
                        myCommand.Parameters.Add("@yearno", year);
                        myCommand.Parameters.Add("@mthno", month);
                    }
                    else if (AmountType == "TOTAL")
                    {
                        myCommand.CommandText = @"select a.* from v_prfbill_sum_iv_stf_mth_total a  where a.yearno = @yearno and a.mthno = @mthno";
                        myCommand.Parameters.Add("@yearno", year);
                        myCommand.Parameters.Add("@mthno", month);
                    }
                }
                else
                {
                    if (AmountType == "AMOUNT")
                    {
                        myCommand.CommandText = @"select a.* from v_prfbill_sum_iv1_stf_mth_own a where a.yearno = @yearno and a.mthno = @mthno";
                        myCommand.Parameters.Add("@yearno", year);
                        myCommand.Parameters.Add("@mthno", month);
                    }
                    else if (AmountType == "NET")
                    {
                        myCommand.CommandText = @"select a.* from v_prfbill_sum_ivtab1_staff_mth a  where a.yearno = @yearno and a.mthno = @mthno";
                        myCommand.Parameters.Add("@yearno", year);
                        myCommand.Parameters.Add("@mthno", month);
                    }
                    else if (AmountType == "TAX")
                    {
                        myCommand.CommandText = @"select a.* from v_prfbill_sum_iv_stf1_mth_tax a  where a.yearno = @yearno and a.mthno = @mthno";
                        myCommand.Parameters.Add("@yearno", year);
                        myCommand.Parameters.Add("@mthno", month);
                    }
                    else if (AmountType == "OTHER")
                    {
                        myCommand.CommandText = @"select a.* from v_prfbill_sum_iv1_stf_mth_other a  where a.yearno = @yearno and a.mthno = @mthno";
                        myCommand.Parameters.Add("@yearno", year);
                        myCommand.Parameters.Add("@mthno", month);
                    }
                    else if (AmountType == "TOTAL")
                    {
                        myCommand.CommandText = @"select a.* from v_prfbill_sum_iv1_stf_mth_total a  where a.yearno = @yearno and a.mthno = @mthno";
                        myCommand.Parameters.Add("@yearno", year);
                        myCommand.Parameters.Add("@mthno", month);
                    }
                }


                using (FbDataReader reader = myCommand.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        IList<PrfBillStaffRecord> RecList = new List<PrfBillStaffRecord>();
                        while (reader.Read())
                        {
                            PrfBillStaffRecord newRec = new PrfBillStaffRecord();

                            if (Category == "CASECODE")
                            {
                                newRec.casecode = reader.GetString(reader.GetOrdinal("CASECODE"));
                                newRec.casedesc = reader.GetString(reader.GetOrdinal("CASEDESC"));
                            }
                            else
                            {
                                newRec.casecode = reader.GetString(reader.GetOrdinal("ITEMTYPE"));
                                newRec.casedesc = reader.GetString(reader.GetOrdinal("ITEMDESC"));
                            }
                            newRec.yearno = reader.GetInt32(reader.GetOrdinal("YEARNO"));

                            newRec.c01 = reader.GetDecimal(reader.GetOrdinal("C01"));
                            newRec.c02 = reader.GetDecimal(reader.GetOrdinal("C02"));
                            newRec.c03 = reader.GetDecimal(reader.GetOrdinal("C03"));
                            newRec.c04 = reader.GetDecimal(reader.GetOrdinal("C04"));
                            newRec.c05 = reader.GetDecimal(reader.GetOrdinal("C05"));
                            newRec.c06 = reader.GetDecimal(reader.GetOrdinal("C06"));
                            newRec.c07 = reader.GetDecimal(reader.GetOrdinal("C07"));
                            newRec.c08 = reader.GetDecimal(reader.GetOrdinal("C08"));
                            newRec.c09 = reader.GetDecimal(reader.GetOrdinal("C09"));
                            newRec.c10 = reader.GetDecimal(reader.GetOrdinal("C10"));
                            newRec.c11 = reader.GetDecimal(reader.GetOrdinal("C11"));
                            newRec.c12 = reader.GetDecimal(reader.GetOrdinal("C12"));
                            newRec.c13 = reader.GetDecimal(reader.GetOrdinal("C13"));
                            newRec.c14 = reader.GetDecimal(reader.GetOrdinal("C14"));
                            newRec.c15 = reader.GetDecimal(reader.GetOrdinal("C15"));
                            newRec.c16 = reader.GetDecimal(reader.GetOrdinal("C16"));
                            newRec.c17 = reader.GetDecimal(reader.GetOrdinal("C17"));
                            newRec.c18 = reader.GetDecimal(reader.GetOrdinal("C18"));
                            RecList.Add(newRec);
                        }
                        return RecList;
                    }
                }
                return null;
            }
        }

        public IEnumerable<PrfBillStaffRecord> PopulatePrfBillStaffMonthPortfolio(int year, int month, string AmountType, string Category)
        {
            using (FbConnection con = new FbConnection(_connectionName))
            {
                con.Open();
                FbCommand myCommand = new FbCommand();
                myCommand.Connection = con;
                if (Category == "CASECODE")
                {
                    if (AmountType == "AMOUNT")
                    {
                        myCommand.CommandText = @"select a.* from V_PRFBILL_SUM_IV_STFP_MTH_OWN  a where a.yearno = @yearno and a.mthno = @mthno";
                        myCommand.Parameters.Add("@yearno", year);
                        myCommand.Parameters.Add("@mthno", month);
                    }
                    else if (AmountType == "NET")
                    {
                        myCommand.CommandText = @"select a.* from V_PRFBILL_SUM_IV_STFP_MTH  a  where a.yearno = @yearno and a.mthno = @mthno";
                        myCommand.Parameters.Add("@yearno", year);
                        myCommand.Parameters.Add("@mthno", month);
                    }
                    else if (AmountType == "TAX")
                    {
                        myCommand.CommandText = @"select a.* from V_PRFBILL_SUM_IV_STFP_MTH_TAX  a  where a.yearno = @yearno and a.mthno = @mthno";
                        myCommand.Parameters.Add("@yearno", year);
                        myCommand.Parameters.Add("@mthno", month);
                    }
                    else if (AmountType == "OTHER")
                    {
                        myCommand.CommandText = @"select a.* from V_PRFBILL_SUM_IV_STFP_MTH_OTH  a  where a.yearno = @yearno and a.mthno = @mthno";
                        myCommand.Parameters.Add("@yearno", year);
                        myCommand.Parameters.Add("@mthno", month);
                    }
                    else if (AmountType == "TOTAL")
                    {
                        myCommand.CommandText = @"select a.* from V_PRFBILL_SUM_IV_STFP_MTH_TOT  a  where a.yearno = @yearno and a.mthno = @mthno";
                        myCommand.Parameters.Add("@yearno", year);
                        myCommand.Parameters.Add("@mthno", month);
                    }
                }
                else
                {
                    if (AmountType == "AMOUNT")
                    {
                        myCommand.CommandText = @"select a.* from V_PRFBILL_SUM_IV1_STFP_MTH_OWN  a where a.yearno = @yearno and a.mthno = @mthno";
                        myCommand.Parameters.Add("@yearno", year);
                        myCommand.Parameters.Add("@mthno", month);
                    }
                    else if (AmountType == "NET")
                    {
                        myCommand.CommandText = @"select a.* from V_PRFBILL_SUM_IV1_STFP_MTH  a  where a.yearno = @yearno and a.mthno = @mthno";
                        myCommand.Parameters.Add("@yearno", year);
                        myCommand.Parameters.Add("@mthno", month);
                    }
                    else if (AmountType == "TAX")
                    {
                        myCommand.CommandText = @"select a.* from V_PRFBILL_SUM_IV1_STFP_MTH_TAX  a  where a.yearno = @yearno and a.mthno = @mthno";
                        myCommand.Parameters.Add("@yearno", year);
                        myCommand.Parameters.Add("@mthno", month);
                    }
                    else if (AmountType == "OTHER")
                    {
                        myCommand.CommandText = @"select a.* from V_PRFBILL_SUM_IV1_STFP_MTH_OTH  a  where a.yearno = @yearno and a.mthno = @mthno";
                        myCommand.Parameters.Add("@yearno", year);
                        myCommand.Parameters.Add("@mthno", month);
                    }
                    else if (AmountType == "TOTAL")
                    {
                        myCommand.CommandText = @"select a.* from V_PRFBILL_SUM_IV1_STFP_MTH_TOT  a  where a.yearno = @yearno and a.mthno = @mthno";
                        myCommand.Parameters.Add("@yearno", year);
                        myCommand.Parameters.Add("@mthno", month);
                    }
                }


                using (FbDataReader reader = myCommand.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        IList<PrfBillStaffRecord> RecList = new List<PrfBillStaffRecord>();
                        while (reader.Read())
                        {
                            PrfBillStaffRecord newRec = new PrfBillStaffRecord();

                            if (Category == "CASECODE")
                            {
                                newRec.casecode = reader.GetString(reader.GetOrdinal("CASECODE"));
                                newRec.casedesc = reader.GetString(reader.GetOrdinal("CASEDESC"));
                            }
                            else
                            {
                                newRec.casecode = reader.GetString(reader.GetOrdinal("ITEMTYPE"));
                                newRec.casedesc = reader.GetString(reader.GetOrdinal("ITEMDESC"));
                            }
                            newRec.yearno = reader.GetInt32(reader.GetOrdinal("YEARNO"));

                            newRec.c01 = reader.GetDecimal(reader.GetOrdinal("C01"));
                            newRec.c02 = reader.GetDecimal(reader.GetOrdinal("C02"));
                            newRec.c03 = reader.GetDecimal(reader.GetOrdinal("C03"));
                            newRec.c04 = reader.GetDecimal(reader.GetOrdinal("C04"));
                            newRec.c05 = reader.GetDecimal(reader.GetOrdinal("C05"));
                            newRec.c06 = reader.GetDecimal(reader.GetOrdinal("C06"));
                            newRec.c07 = reader.GetDecimal(reader.GetOrdinal("C07"));
                            newRec.c08 = reader.GetDecimal(reader.GetOrdinal("C08"));
                            newRec.c09 = reader.GetDecimal(reader.GetOrdinal("C09"));
                            newRec.c10 = reader.GetDecimal(reader.GetOrdinal("C10"));
                            newRec.c11 = reader.GetDecimal(reader.GetOrdinal("C11"));
                            newRec.c12 = reader.GetDecimal(reader.GetOrdinal("C12"));
                            newRec.c13 = reader.GetDecimal(reader.GetOrdinal("C13"));
                            newRec.c14 = reader.GetDecimal(reader.GetOrdinal("C14"));
                            newRec.c15 = reader.GetDecimal(reader.GetOrdinal("C15"));
                            newRec.c16 = reader.GetDecimal(reader.GetOrdinal("C16"));
                            newRec.c17 = reader.GetDecimal(reader.GetOrdinal("C17"));
                            newRec.c18 = reader.GetDecimal(reader.GetOrdinal("C18"));
                            RecList.Add(newRec);
                        }
                        return RecList;
                    }
                }
                return null;
            }
        }

        public IEnumerable<PrfBillStaffRecord> PopulatePrfBillStaffMonthCombo(int year, int month, string AmountType, string Category)
        {
            using (FbConnection con = new FbConnection(_connectionName))
            {
                con.Open();
                FbCommand myCommand = new FbCommand();
                myCommand.Connection = con;
                if (Category == "CASECODE")
                {
                    if (AmountType == "AMOUNT")
                    {
                        myCommand.CommandText = @"select a.* from V_PRFBILL_SUM_IV_STCB_MTH_OWN  a where a.yearno = @yearno and a.mthno = @mthno";
                        myCommand.Parameters.Add("@yearno", year);
                        myCommand.Parameters.Add("@mthno", month);
                    }
                    else if (AmountType == "NET")
                    {
                        myCommand.CommandText = @"select a.* from V_PRFBILL_SUM_IV_STCB_MTH  a  where a.yearno = @yearno and a.mthno = @mthno";
                        myCommand.Parameters.Add("@yearno", year);
                        myCommand.Parameters.Add("@mthno", month);
                    }
                    else if (AmountType == "TAX")
                    {
                        myCommand.CommandText = @"select a.* from V_PRFBILL_SUM_IV_STCB_MTH_TAX  a  where a.yearno = @yearno and a.mthno = @mthno";
                        myCommand.Parameters.Add("@yearno", year);
                        myCommand.Parameters.Add("@mthno", month);
                    }
                    else if (AmountType == "OTHER")
                    {
                        myCommand.CommandText = @"select a.* from V_PRFBILL_SUM_IV_STCB_MTH_OTH  a  where a.yearno = @yearno and a.mthno = @mthno";
                        myCommand.Parameters.Add("@yearno", year);
                        myCommand.Parameters.Add("@mthno", month);
                    }
                    else if (AmountType == "TOTAL")
                    {
                        myCommand.CommandText = @"select a.* from V_PRFBILL_SUM_IV_STCB_MTH_TOT a  where a.yearno = @yearno and a.mthno = @mthno";
                        myCommand.Parameters.Add("@yearno", year);
                        myCommand.Parameters.Add("@mthno", month);
                    }
                }
                else
                {
                    if (AmountType == "AMOUNT")
                    {
                        myCommand.CommandText = @"select a.* from V_PRFBILL_SUM_IV1_STCB_MTH_OWN  a where a.yearno = @yearno and a.mthno = @mthno";
                        myCommand.Parameters.Add("@yearno", year);
                        myCommand.Parameters.Add("@mthno", month);
                    }
                    else if (AmountType == "NET")
                    {
                        myCommand.CommandText = @"select a.* from V_PRFBILL_SUM_IV1_STCB_MTH  a  where a.yearno = @yearno and a.mthno = @mthno";
                        myCommand.Parameters.Add("@yearno", year);
                        myCommand.Parameters.Add("@mthno", month);
                    }
                    else if (AmountType == "TAX")
                    {
                        myCommand.CommandText = @"select a.* from V_PRFBILL_SUM_IV1_STCB_MTH_TAX  a  where a.yearno = @yearno and a.mthno = @mthno";
                        myCommand.Parameters.Add("@yearno", year);
                        myCommand.Parameters.Add("@mthno", month);
                    }
                    else if (AmountType == "OTHER")
                    {
                        myCommand.CommandText = @"select a.* from V_PRFBILL_SUM_IV1_STCB_MTH_OTH  a  where a.yearno = @yearno and a.mthno = @mthno";
                        myCommand.Parameters.Add("@yearno", year);
                        myCommand.Parameters.Add("@mthno", month);
                    }
                    else if (AmountType == "TOTAL")
                    {
                        myCommand.CommandText = @"select a.* from V_PRFBILL_SUM_IV1_STCB_MTH_TOT a  where a.yearno = @yearno and a.mthno = @mthno";
                        myCommand.Parameters.Add("@yearno", year);
                        myCommand.Parameters.Add("@mthno", month);
                    }
                }


                using (FbDataReader reader = myCommand.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        IList<PrfBillStaffRecord> RecList = new List<PrfBillStaffRecord>();
                        while (reader.Read())
                        {
                            PrfBillStaffRecord newRec = new PrfBillStaffRecord();
                            if (Category == "CASECODE")
                            {
                                newRec.casecode = reader.GetString(reader.GetOrdinal("CASECODE"));
                                newRec.casedesc = reader.GetString(reader.GetOrdinal("CASEDESC"));
                            }
                            else
                            {
                                newRec.casecode = reader.GetString(reader.GetOrdinal("ITEMTYPE"));
                                newRec.casedesc = reader.GetString(reader.GetOrdinal("ITEMDESC"));
                            }
                            newRec.yearno = reader.GetInt32(reader.GetOrdinal("YEARNO"));

                            newRec.c01 = reader.GetDecimal(reader.GetOrdinal("C01"));
                            newRec.c02 = reader.GetDecimal(reader.GetOrdinal("C02"));
                            newRec.c03 = reader.GetDecimal(reader.GetOrdinal("C03"));
                            newRec.c04 = reader.GetDecimal(reader.GetOrdinal("C04"));
                            newRec.c05 = reader.GetDecimal(reader.GetOrdinal("C05"));
                            newRec.c06 = reader.GetDecimal(reader.GetOrdinal("C06"));
                            newRec.c07 = reader.GetDecimal(reader.GetOrdinal("C07"));
                            newRec.c08 = reader.GetDecimal(reader.GetOrdinal("C08"));
                            newRec.c09 = reader.GetDecimal(reader.GetOrdinal("C09"));
                            newRec.c10 = reader.GetDecimal(reader.GetOrdinal("C10"));
                            newRec.c11 = reader.GetDecimal(reader.GetOrdinal("C11"));
                            newRec.c12 = reader.GetDecimal(reader.GetOrdinal("C12"));
                            newRec.c13 = reader.GetDecimal(reader.GetOrdinal("C13"));
                            newRec.c14 = reader.GetDecimal(reader.GetOrdinal("C14"));
                            newRec.c15 = reader.GetDecimal(reader.GetOrdinal("C15"));
                            newRec.c16 = reader.GetDecimal(reader.GetOrdinal("C16"));
                            newRec.c17 = reader.GetDecimal(reader.GetOrdinal("C17"));
                            newRec.c18 = reader.GetDecimal(reader.GetOrdinal("C18"));
                            RecList.Add(newRec);
                        }
                        return RecList;
                    }
                }
                return null;
            }
        }

        public IEnumerable<PrfBillMthRecord> PopulatePrfBillMonth(int year, string AmountType, string Category)
        {
            using (FbConnection con = new FbConnection(_connectionName))
            {
                con.Open();
                FbCommand myCommand = new FbCommand();
                myCommand.Connection = con;
                if (Category == "CASECODE")
                {
                    if (AmountType == "AMOUNT")
                    {
                        myCommand.CommandText = @"select a.* from v_prfbill_sumivtab_mth_ownamt a where a.yearno = @yearno";
                        myCommand.Parameters.Add("@yearno", year);
                    }
                    else if (AmountType == "NET")
                    {
                        myCommand.CommandText = @"select a.* from v_prfbill_sumivtab_mth a  where a.yearno = @yearno";
                        myCommand.Parameters.Add("@yearno", year);
                    }
                    else if (AmountType == "TAX")
                    {
                        myCommand.CommandText = @"select a.* from v_prfbill_sumivtab_mth_tax a  where a.yearno = @yearno";
                        myCommand.Parameters.Add("@yearno", year);
                    }
                    else if (AmountType == "OTHER")
                    {
                        myCommand.CommandText = @"select a.* from v_prfbill_sumivtab_mth_other a  where a.yearno = @yearno";
                        myCommand.Parameters.Add("@yearno", year);
                    }
                    else if (AmountType == "TOTAL")
                    {
                        myCommand.CommandText = @"select a.* from v_prfbill_sumivtab_mth_total a  where a.yearno = @yearno";
                        myCommand.Parameters.Add("@yearno", year);
                    }
                }
                else
                {
                    if (AmountType == "AMOUNT")
                    {
                        myCommand.CommandText = @"select a.* from v_prfbill_sumivtab1_mth_ownamt a where a.yearno = @yearno";
                        myCommand.Parameters.Add("@yearno", year);
                    }
                    else if (AmountType == "NET")
                    {
                        myCommand.CommandText = @"select a.* from v_prfbill_sumivtab1_mth a  where a.yearno = @yearno";
                        myCommand.Parameters.Add("@yearno", year);
                    }
                    else if (AmountType == "TAX")
                    {
                        myCommand.CommandText = @"select a.* from v_prfbill_sumivtab1_mth_tax a  where a.yearno = @yearno";
                        myCommand.Parameters.Add("@yearno", year);
                    }
                    else if (AmountType == "OTHER")
                    {
                        myCommand.CommandText = @"select a.* from v_prfbill_sumivtab1_mth_other a  where a.yearno = @yearno";
                        myCommand.Parameters.Add("@yearno", year);
                    }
                    else if (AmountType == "TOTAL")
                    {
                        myCommand.CommandText = @"select a.* from v_prfbill_sumivtab1_mth_total a  where a.yearno = @yearno";
                        myCommand.Parameters.Add("@yearno", year);
                    }
                }


                using (FbDataReader reader = myCommand.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        IList<PrfBillMthRecord> RecList = new List<PrfBillMthRecord>();
                        while (reader.Read())
                        {
                            PrfBillMthRecord newRec = new PrfBillMthRecord();

                            if (Category == "CASECODE")
                            {
                                newRec.casecode = reader.GetString(reader.GetOrdinal("CASECODE"));
                                newRec.casedesc = reader.GetString(reader.GetOrdinal("CASEDESC"));
                            }
                            else
                            {
                                newRec.casecode = reader.GetString(reader.GetOrdinal("ITEMTYPE"));
                                newRec.casedesc = reader.GetString(reader.GetOrdinal("ITEMDESC"));
                            }
                            newRec.yearno = reader.GetInt32(reader.GetOrdinal("YEARNO"));

                            newRec.jan = reader.GetDecimal(reader.GetOrdinal("C01"));
                            newRec.feb = reader.GetDecimal(reader.GetOrdinal("C02"));
                            newRec.mar = reader.GetDecimal(reader.GetOrdinal("C03"));
                            newRec.apr = reader.GetDecimal(reader.GetOrdinal("C04"));
                            newRec.may = reader.GetDecimal(reader.GetOrdinal("C05"));
                            newRec.jun = reader.GetDecimal(reader.GetOrdinal("C06"));
                            newRec.jul = reader.GetDecimal(reader.GetOrdinal("C07"));
                            newRec.aug = reader.GetDecimal(reader.GetOrdinal("C08"));
                            newRec.sep = reader.GetDecimal(reader.GetOrdinal("C09"));
                            newRec.oct = reader.GetDecimal(reader.GetOrdinal("C10"));
                            newRec.nov = reader.GetDecimal(reader.GetOrdinal("C11"));
                            newRec.dec = reader.GetDecimal(reader.GetOrdinal("C12"));

                            RecList.Add(newRec);
                        }
                        return RecList;
                    }
                }
                return null;
            }
        }

        public IEnumerable<PrfBillDayRecord> PopulatePrfBillDay(int year, int month, string AmountType, string Category)
        {
            using (FbConnection con = new FbConnection(_connectionName))
            {
                con.Open();
                FbCommand myCommand = new FbCommand();
                myCommand.Connection = con;

                if (Category == "CASECODE")
                {
                    if (AmountType == "AMOUNT")
                    {
                        myCommand.CommandText = @"select a.* from v_prfbill_day_amt a where a.yearno = @yearno and a.mthno = @monthno";
                        myCommand.Parameters.Add("@yearno", year);
                        myCommand.Parameters.Add("@monthno", month);
                    }
                    else if (AmountType == "NET")
                    {
                        myCommand.CommandText = @"select a.* from v_prfbill_day a  where a.yearno = @yearno and a.mthno = @monthno";
                        myCommand.Parameters.Add("@yearno", year);
                        myCommand.Parameters.Add("@monthno", month);
                    }
                    else if (AmountType == "TAX")
                    {
                        myCommand.CommandText = @"select a.* from v_prfbill_day_tax a  where a.yearno = @yearno and a.mthno = @monthno";
                        myCommand.Parameters.Add("@yearno", year);
                        myCommand.Parameters.Add("@monthno", month);
                    }
                    else if (AmountType == "OTHER")
                    {
                        myCommand.CommandText = @"select a.* from v_prfbill_day_other a  where a.yearno = @yearno and a.mthno = @monthno";
                        myCommand.Parameters.Add("@yearno", year);
                        myCommand.Parameters.Add("@monthno", month);
                    }
                    else if (AmountType == "TOTAL")
                    {
                        myCommand.CommandText = @"select a.* from v_prfbill_day_total a  where a.yearno = @yearno and a.mthno = @monthno";
                        myCommand.Parameters.Add("@yearno", year);
                        myCommand.Parameters.Add("@monthno", month);
                    }
                }
                else
                {
                    if (AmountType == "AMOUNT")
                    {
                        myCommand.CommandText = @"select a.* from v_prfbill_day1_amt a where a.yearno = @yearno and a.mthno = @monthno";
                        myCommand.Parameters.Add("@yearno", year);
                        myCommand.Parameters.Add("@monthno", month);
                    }
                    else if (AmountType == "NET")
                    {
                        myCommand.CommandText = @"select a.* from v_prfbill_day1 a  where a.yearno = @yearno and a.mthno = @monthno";
                        myCommand.Parameters.Add("@yearno", year);
                        myCommand.Parameters.Add("@monthno", month);
                    }
                    else if (AmountType == "TAX")
                    {
                        myCommand.CommandText = @"select a.* from v_prfbill_day1_tax a  where a.yearno = @yearno and a.mthno = @monthno";
                        myCommand.Parameters.Add("@yearno", year);
                        myCommand.Parameters.Add("@monthno", month);
                    }
                    else if (AmountType == "OTHER")
                    {
                        myCommand.CommandText = @"select a.* from v_prfbill_day1_other a  where a.yearno = @yearno and a.mthno = @monthno";
                        myCommand.Parameters.Add("@yearno", year);
                        myCommand.Parameters.Add("@monthno", month);
                    }
                    else if (AmountType == "TOTAL")
                    {
                        myCommand.CommandText = @"select a.* from v_prfbill_day1_total a  where a.yearno = @yearno and a.mthno = @monthno";
                        myCommand.Parameters.Add("@yearno", year);
                        myCommand.Parameters.Add("@monthno", month);
                    }
                }


                using (FbDataReader reader = myCommand.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        IList<PrfBillDayRecord> RecList = new List<PrfBillDayRecord>();
                        while (reader.Read())
                        {
                            PrfBillDayRecord newRec = new PrfBillDayRecord();

                            if (Category == "CASECODE")
                            {
                                newRec.casecode = reader.GetString(reader.GetOrdinal("CASECODE"));
                                newRec.casedesc = reader.GetString(reader.GetOrdinal("CASEDESC"));
                            }
                            else
                            {
                                newRec.casecode = reader.GetString(reader.GetOrdinal("ITEMTYPE"));
                                newRec.casedesc = reader.GetString(reader.GetOrdinal("ITEMDESC"));
                            }
                            newRec.yearno = reader.GetInt32(reader.GetOrdinal("YEARNO"));
                            newRec.monthno = reader.GetInt32(reader.GetOrdinal("MTHNO"));

                            newRec.D01 = reader.GetDecimal(reader.GetOrdinal("C01"));
                            newRec.D02 = reader.GetDecimal(reader.GetOrdinal("C02"));
                            newRec.D03 = reader.GetDecimal(reader.GetOrdinal("C03"));
                            newRec.D04 = reader.GetDecimal(reader.GetOrdinal("C04"));
                            newRec.D05 = reader.GetDecimal(reader.GetOrdinal("C05"));
                            newRec.D06 = reader.GetDecimal(reader.GetOrdinal("C06"));
                            newRec.D07 = reader.GetDecimal(reader.GetOrdinal("C07"));
                            newRec.D08 = reader.GetDecimal(reader.GetOrdinal("C08"));
                            newRec.D09 = reader.GetDecimal(reader.GetOrdinal("C09"));
                            newRec.D10 = reader.GetDecimal(reader.GetOrdinal("C10"));
                            newRec.D11 = reader.GetDecimal(reader.GetOrdinal("C11"));
                            newRec.D12 = reader.GetDecimal(reader.GetOrdinal("C12"));
                            newRec.D13 = reader.GetDecimal(reader.GetOrdinal("C13"));
                            newRec.D14 = reader.GetDecimal(reader.GetOrdinal("C14"));
                            newRec.D15 = reader.GetDecimal(reader.GetOrdinal("C15"));
                            newRec.D16 = reader.GetDecimal(reader.GetOrdinal("C16"));
                            newRec.D17 = reader.GetDecimal(reader.GetOrdinal("C17"));
                            newRec.D18 = reader.GetDecimal(reader.GetOrdinal("C18"));
                            newRec.D19 = reader.GetDecimal(reader.GetOrdinal("C19"));
                            newRec.D20 = reader.GetDecimal(reader.GetOrdinal("C20"));
                            newRec.D21 = reader.GetDecimal(reader.GetOrdinal("C21"));
                            newRec.D22 = reader.GetDecimal(reader.GetOrdinal("C22"));
                            newRec.D23 = reader.GetDecimal(reader.GetOrdinal("C23"));
                            newRec.D24 = reader.GetDecimal(reader.GetOrdinal("C24"));
                            newRec.D25 = reader.GetDecimal(reader.GetOrdinal("C25"));
                            newRec.D26 = reader.GetDecimal(reader.GetOrdinal("C26"));
                            newRec.D27 = reader.GetDecimal(reader.GetOrdinal("C27"));
                            newRec.D28 = reader.GetDecimal(reader.GetOrdinal("C28"));
                            newRec.D29 = reader.GetDecimal(reader.GetOrdinal("C29"));
                            newRec.D30 = reader.GetDecimal(reader.GetOrdinal("C30"));
                            newRec.D31 = reader.GetDecimal(reader.GetOrdinal("C31"));
                            newRec.D32 = reader.GetDecimal(reader.GetOrdinal("C32"));
                            newRec.D33 = reader.GetDecimal(reader.GetOrdinal("C33"));
                            newRec.D34 = reader.GetDecimal(reader.GetOrdinal("C34"));
                            newRec.D35 = reader.GetDecimal(reader.GetOrdinal("C35"));
                            newRec.D36 = reader.GetDecimal(reader.GetOrdinal("C36"));

                            RecList.Add(newRec);
                        }
                        return RecList;
                    }
                }
                return null;
            }
        }
    }
}





