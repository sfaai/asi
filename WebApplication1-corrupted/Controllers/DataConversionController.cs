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

namespace WebApplication1.Controllers
{
    public class DataConversionController : Controller
    {
        // GET: DataConversion

        private ASIDBConnection db = new ASIDBConnection();

        private string[] companycol = {
            "New",
            "company_id",
            "company_no",
            "company_name",
            "company_intype",
            "company_constitution",
            "company_incorporation",
            "company_objectives",
            "company_industrya",
            "company_industryb",
            "company_website",
            "company_staffid",
            "company_comment",
            "company_remark",
            "company_country",
            "company_gst",
            "company_activate",
            "company_financialyearend"
        };

        private string convertStaff(string id)
        {
            string retval = "";
            if (id == "1") { retval = "1"; }
            else if (id == "2") { retval = "4"; }
            else if (id == "3") { retval = "6"; }
            else if (id == "4") { retval = "10"; }
            else if (id == "5") { retval = "7"; }
            else if (id == "6") { retval = "8"; }
            else if (id == "7") { retval = "3"; }
            else if (id == "8") { retval = "2"; }
            else if (id == "9") { retval = "12"; }
            else if (id == "10") { retval = "5"; }
            else retval = id;

            return retval;
        }

        private string convertConst(string id)
        {
            string retval = "";
            if (id == "Private Limited") { retval = "02"; }
            else if (id == "Limited Liability Partnership") { retval = "06"; }
            else if (id == "Public Limited") { retval = "01"; }
            //else throw new Exception("Error finding constitution")
            else retval = id
            ;

            return retval;
        }

        private string convertCountry(string id)
        {
            string retval = "";
            if (id == "Malaysia") { retval = "MY"; }
            else if (id == "Singapore") { retval = "SG"; }
            //else throw new Exception("Error finding constitution")
            else retval = id
            ;

            return retval;
        }

        private void addColumns(DataTable dt, string[] col)
        {
            int j = col.Length;
            for (int i = 0; i < j; i++)
            {
                if (i == 1)
                {
                    dt.Columns.Add(new DataColumn(col[i], typeof(int)));
                }
                else
                {
                    dt.Columns.Add(new DataColumn(col[i], typeof(string)));
                }
            }
        }

        public ActionResult Index()

        {
            DataTable dt = new DataTable();
            addColumns(dt, companycol);
            dt.Rows.Add();
            return View(dt);

        }

        private DataTable OpenTable(string filename)
        {
            //Read the contents of CSV file.

            string csvData = System.IO.File.ReadAllText(filename);
            DataTable dt = new DataTable();


            //Execute a loop over the rows.
            bool IsHeader = true;
            int rowcnt = 0;
            DataRow curRow = null;
            foreach (string row in csvData.Split('\n'))

            {

                if ((!IsHeader) && (!string.IsNullOrEmpty(row)))

                {

                    curRow = dt.Rows.Add();
                    rowcnt++;
                    int i = 1;



                    //Execute a loop over the columns.

                    //foreach (string cell in row.Split('\t'))
                    foreach (string cell in row.Split('|'))
                    {
                        try
                        {
                            dt.Rows[dt.Rows.Count - 1][i] = cell ?? "";
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(rowcnt.ToString() + " | " + e.Message);
                        }
                        finally
                        {

                            i++;
                        }

                    }

                }
                else
                {
                    if (IsHeader)
                    {
                        IsHeader = false;
                        List<string> myCol = new List<string>();
                        myCol.Add("New");

                        foreach (string cell in row.Split('|'))
                        {
                            try
                            {
                                myCol.Add(cell);
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(rowcnt.ToString() + " | " + e.Message);
                            }
                            finally
                            {


                            }

                        }
                        string[] myArray = new string[myCol.Count];
                        for (int c = 0; c < myCol.Count; c++)
                        {
                            myArray[c] = myCol[c];
                        }
                        addColumns(dt, myArray);
                    }
                }


            }
            return dt;
        }

        [HttpPost]

        public ActionResult Index(HttpPostedFileBase postedFile, string dbTableName, string dbKey, string localKey)

        {

            string filePath = string.Empty;

            if (postedFile != null)

            {

                string path = Server.MapPath("~/Uploads/");

                if (!Directory.Exists(path))

                {

                    Directory.CreateDirectory(path);

                }



                filePath = path + Path.GetFileName(postedFile.FileName);

                string extension = Path.GetExtension(postedFile.FileName);

                string keyId = "";

                string templine = "";

                postedFile.SaveAs(filePath);


                IEnumerable<dynamic> csRecs = null;

                DataTable refTable;
                string usefile = "";

                if (dbTableName == "CSCOMSTR")
                {
                    csRecs = db.CSCOMSTRs.ToList();

                    //whereFunc = item => item.CONO == $"{keyId}";
                    //orderByFunc = item => item.CONAME;
                }
                else if (dbTableName == "CSCOSTAT") 
                {
                    csRecs = db.CSCOSTATs.ToList();
                    usefile = path + "cs_company1.csv";
                }
                else if  (dbTableName == "CSCOAR")
                {
                    csRecs = db.CSCOARs.ToList();
                    usefile = path + "cs_company1.csv";
                }



                //Create a DataTable.

                DataTable dt = new DataTable();

                //addColumns(dt, companycol);
                //dt.Columns.AddRange(new DataColumn[3] { new DataColumn("CaseFeeId", typeof(int)),

                //                new DataColumn("CaseFeeCode", typeof(string)),

                //                new DataColumn("CaseFeeDesc",typeof(string)) });





                //Read the contents of CSV file.

                string csvData = System.IO.File.ReadAllText(filePath);



                //Execute a loop over the rows.
                bool IsHeader = true;
                int rowcnt = 0;
                DataRow curRow = null;
                foreach (string row in csvData.Split('\n'))

                {

                    if ((!IsHeader) && (!string.IsNullOrEmpty(row)))

                    {

                        curRow = dt.Rows.Add();
                        rowcnt++;
                        int i = 1;



                        //Execute a loop over the columns.

                        //foreach (string cell in row.Split('\t'))
                        foreach (string cell in row.Split('|'))
                        {
                            try
                            {
                                dt.Rows[dt.Rows.Count - 1][i] = cell ?? "";
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(rowcnt.ToString() + " | " + e.Message);
                            }
                            finally
                            {

                                i++;
                            }

                        }

                        if ((dbTableName == "CSCOSTAT") || (dbTableName == "CSCOAR"))
                        {
                            string myDateStr = (string)curRow["status_date"];
                            string mySortStr = myDateStr.Split('/')[2] + myDateStr.Split('/')[1] + myDateStr.Split('/')[0];
                            curRow["sort_date"] = mySortStr;

                        }

                        if ((localKey.Length > 0) && (DBNull.Value != curRow[localKey]))
                        {
                            keyId = (string)curRow[localKey] ?? "";

                            if (dbTableName == "CSCOMSTR")
                            {
                                CSCOMSTR csRec = null;
                                Func<CSCOMSTR, bool> whereFunc = item => item.CONO == $"{keyId}";
                                Func<CSCOMSTR, Object> orderByFunc = item => item.CONAME;

                                //csRec = ((List<CSCOMSTR>) csRecs).Where(whereFunc).FirstOrDefault();
                                csRec = ((List<CSCOMSTR>)csRecs).Where(item => item.CONO == keyId).FirstOrDefault();
                                if (csRec == null)
                                {
                                    curRow["New"] = "***";
                                }
                                else if (csRec.CONAME != (string)curRow["company_name"])
                                {
                                    curRow["New"] = "**";
                                }
                                else if (csRec.INTYPE != (string)curRow["company_intype"])
                                {
                                    curRow["New"] = "*";
                                }
                                else if (csRec.CONSTCODE != (string)curRow["company_constitution"])
                                {
                                    curRow["New"] = "*";
                                }

                                if (DBNull.Value != curRow["company_staffid"])
                                {

                                    curRow["company_staffid"] =
                                        convertStaff((string)curRow["company_staffid"]);
                                }


                                if (DBNull.Value != curRow["company_constitution"])
                                {

                                    curRow["company_constitution"] =
                                        convertConst((string)curRow["company_constitution"]);
                                }

                                if (DBNull.Value != curRow["company_country"])
                                {

                                    curRow["company_country"] =
                                        convertCountry((string)curRow["company_country"]);
                                }
                            }

                            //SortOrder sortOrder = SortOrder.Descending;

                            //if (sortOrder == SortOrder.Ascending)
                            //    orderByFunc = item => item.CONO;
                            //else if (sortOrder == SortOrder.Descending)
                            //    orderByFunc = item => item.CONAME;
                            //db.CSCOMSTRs.OrderBy(orderByFunc);
                        }

                    }
                    else
                    {
                        if (IsHeader)
                        {
                            IsHeader = false;
                            List<string> myCol = new List<string>();
                            myCol.Add("New");

                            foreach (string cell in row.Split('|'))
                            {
                                try
                                {
                                    myCol.Add(cell);
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine(rowcnt.ToString() + " | " + e.Message);
                                }
                                finally
                                {


                                }

                            }
                            if ((dbTableName == "CSCOSTAT") || (dbTableName == "CSCOAR"))
                            {
                                myCol.Add("sort_date");
                            }

                            string[] myArray = new string[myCol.Count];
                            for (int c = 0; c < myCol.Count; c++)
                            {
                                myArray[c] = myCol[c];
                            }
                            addColumns(dt, myArray);
                        }
                    }


                }
                var datarow = dt.AsEnumerable();

                if (dbTableName == "CSCOAR")
                {
                    datarow = datarow.OrderBy(x => x.Field<string>("company_id")).ThenBy(n => n.Field<int>("ar_id")).Select(x => x);

                    refTable = OpenTable(usefile);
                    var csRefs = refTable.AsEnumerable();


                    int idx = 0;
                    int coid = 0;
                    string cono = "";
                    DateTime sdate;
                    CSCOAR csRec = null;
                    foreach (DataRow row in datarow)
                    {
                        templine = (string)row["company_id"];
                        coid = int.Parse(templine);
                        cono = null;
                        try
                        {
                            var corec = csRefs.Where(x => x.Field<int?>("company_id") == coid).FirstOrDefault();
                            if (corec != null)
                            {
                                cono = (string)corec["company_no"];
                            }
                            else cono = null;

                            if (cono != null)
                            {
                                datarow.ElementAt(idx)["New"] = cono;

                                sdate = DateTime.Parse((string)(datarow.ElementAt(idx)["ar_date"]));
                                csRec = ((List<CSCOAR>)csRecs).Where(item => item.CONO == cono && item.ARTOFILE == sdate).FirstOrDefault();
                                if (csRec != null) { datarow.ElementAt(idx)[7] = "Exist"; }
                                else
                                {
                                    CSCOAR cSCOAR = new CSCOAR();
                                    try
                                    {
                                        ModelState.Clear();

                                        cSCOAR.STAMP = 999;
                                        cSCOAR.CONO = cono;
                                        cSCOAR.ARTOFILE = sdate;
                                       // cSCOAR.SUBMITDATE = DateTime.Parse((string)row["ar_submissiondate"]);
                                        cSCOAR.REMINDER1 = DateTime.Parse((string)row["ar_reminderdate"]);
                                        cSCOAR.FILEDAR = DateTime.Parse( (string)row["ar_fileddate"]);

                                        if (ModelState.IsValid)
                                        {
                                            int? lastRowNo = 0;
                                            try
                                            {
                                                lastRowNo = db.CSCOARs.Where(m => m.CONO == cSCOAR.CONO).Max(n => n.ARNO);
                                            }
                                            catch (Exception e) { lastRowNo = -1; }
                                            finally { };


                                            cSCOAR.ARNO = (short) ((lastRowNo ?? 0) + 1);
                                            datarow.ElementAt(idx)[6] = cSCOAR.ARNO.ToString();

                                            db.CSCOARs.Add(cSCOAR);
                                            if (cSCOAR.FILEDAR != null) // create next year record if AR is filed
                                            {
                                                string sid = cSCOAR.CONO;
                                                CSCOAR lastRec = db.CSCOARs.Where(m => m.CONO == sid).OrderByDescending(n => n.ARNO).FirstOrDefault();

                                                if (cSCOAR.ARNO == lastRec.ARNO) // add next year record only if editing the last record
                                                {
                                                    CSCOAR csRec1 = createNextYearAR(cSCOAR);
                                                    db.CSCOARs.Add(csRec1);
                                                }
                                            }
                                            db.SaveChanges();
                                        }
                                    }
                                    catch (Exception e)
                                    {
                                        datarow.ElementAt(idx)[6] = e.Message + "!!!";
                                        db.CSCOARs.Remove(cSCOAR);
                                    }
                                    finally
                                    {; };

                                };
                            }

                        }

                        catch (Exception e)
                        {
                            datarow.ElementAt(idx)["New"] = e.Message;
                        }
                        finally
                        {

                            idx++;
                        }
                    }
                    datarow = datarow.Where(y => y.Field<string>(7) != "Exist").OrderBy(x => x.Field<string>("New")).ThenBy(y => y.Field<int>("status_id"));

                }
                else if (dbTableName == "CSCOMSTR")
                {
                    datarow = datarow.Where(x => x.Field<string>("New") == "***" && x.Field<string>("company_no") != "");
                    datarow = datarow.OrderBy(x => x.Field<int?>("company_id")).Select(x => x);

                    int idx = 0;
                    foreach (DataRow row in datarow)
                    {
                        templine = (string)row["company_no"];

                        CSCOMSTR cSCOMSTR = new CSCOMSTR();

                        try
                        {
                            ModelState.Clear();

                            cSCOMSTR.STAMP = 999;
                            cSCOMSTR.ARRE = "N";
                            cSCOMSTR.SPECIALRE = "N";
                            cSCOMSTR.SEQNO = 1; // set inital seqno
                            cSCOMSTR.CONO = (string)row["company_no"];
                            cSCOMSTR.CONAME = (string)row["company_name"];
                            cSCOMSTR.INTYPE = (string)row["company_intype"];
                            cSCOMSTR.CONSTCODE = (string)row["company_constitution"];
                            cSCOMSTR.COSTATD = DateTime.Now;
                            cSCOMSTR.INCDATE = DateTime.Parse((string)(row["company_incorporation"]));
                            cSCOMSTR.PRINOBJStr = (string)row["company_objectives"] ?? "";
                            cSCOMSTR.PINDCODE = (string)row["company_industrya"] ?? "";
                            cSCOMSTR.SINDCODE = (string)row["company_industryb"] ?? "";
                            cSCOMSTR.WEB = (string)row["company_website"] ?? "";
                            cSCOMSTR.STAFFCODE = (string)row["company_staffid"];
                            cSCOMSTR.CMMTStr = (string)row["company_comment"] ?? "";
                            cSCOMSTR.INCCTRY = "MY";
                            cSCOMSTR.COSTAT = "Active";

                            if (DBNull.Value != row["company_remark"]) { cSCOMSTR.REM = (string)row["company_remark"] ?? ""; }
                            if (DBNull.Value != row["company_country"]) { cSCOMSTR.INCCTRY = (string)row["company_country"]; };
                            if (row["company_activate"] != DBNull.Value) { cSCOMSTR.COSTAT = (string)row["company_activate"] == "t" ? "Active" : "NotActive"; };


                            if (ModelState.IsValid)
                            {

                                db.CSCOMSTRs.Add(cSCOMSTR);
                                db.SaveChanges();
                            }
                        }
                        catch (Exception e)
                        {
                            datarow.ElementAt(idx)["company_industryb"] = "!!!";
                            db.CSCOMSTRs.Remove(cSCOMSTR);
                        }
                        finally
                        {
                            // db.CSCOMSTRs.Add(cSCOMSTR);
                            idx++;
                        }







                        // Console.WriteLine( templine);
                    }
                    //db.SaveChanges();
                }
                else if (dbTableName == "CSCOSTAT")
                {
                    datarow = datarow.OrderBy(x => x.Field<string>("company_id")).ThenBy(n => n.Field<string>("sort_date")).Select(x => x);
                    refTable = OpenTable(usefile);
                    var csRefs = refTable.AsEnumerable();


                    int idx = 0;
                    int coid = 0;
                    string cono = "";
                    DateTime sdate;
                    CSCOSTAT csRec = null;
                    foreach (DataRow row in datarow)
                    {
                        templine = (string)row["company_id"];
                        coid = int.Parse(templine);
                        cono = null;
                        try
                        {
                            var corec = csRefs.Where(x => x.Field<int?>("company_id") == coid).FirstOrDefault();
                            if (corec != null)
                            {
                                cono = (string)corec["company_no"];
                            }
                            else cono = null;
                            if (cono != null)
                            {
                                datarow.ElementAt(idx)["New"] = cono;

                                sdate = DateTime.Parse((string)(datarow.ElementAt(idx)["status_date"]));
                                csRec = ((List<CSCOSTAT>)csRecs).Where(item => item.CONO == cono && item.SDATE == sdate).FirstOrDefault();
                                if (csRec != null) { datarow.ElementAt(idx)[7] = "Exist"; }
                                else
                                {
                                    CSCOSTAT cSCOSTAT = new CSCOSTAT();
                                    try
                                    {
                                        ModelState.Clear();

                                        cSCOSTAT.STAMP = 999;
                                        cSCOSTAT.CONO = cono;
                                        cSCOSTAT.SDATE = sdate;
                                        cSCOSTAT.FILETYPE = "A";
                                        cSCOSTAT.COSTAT = (string)row["status_status"];
                                        cSCOSTAT.FILELOC = (string)row["status_fileno"];
                                        cSCOSTAT.SEALLOC = (string)row["status_sealno"];

                                        if (ModelState.IsValid)
                                        {
                                            int? lastRowNo = 0;
                                            try
                                            {
                                                lastRowNo = db.CSCOSTATs.Where(m => m.CONO == cSCOSTAT.CONO).Max(n => n.ROWNO);
                                            }
                                            catch (Exception e) { lastRowNo = -1; }
                                            finally { };

                                            cSCOSTAT.EDATE = cSCOSTAT.SDATE.AddDays(30000);
                                            cSCOSTAT.ROWNO = (lastRowNo ?? 0) + 1;
                                            datarow.ElementAt(idx)[6] = cSCOSTAT.ROWNO.ToString();

                                            db.CSCOSTATs.Add(cSCOSTAT);
                                            UpdatePreviousRow(cSCOSTAT);
                                            db.SaveChanges();
                                        }
                                    }
                                    catch (Exception e)
                                    {
                                        datarow.ElementAt(idx)[6] = e.Message + "!!!";
                                        db.CSCOSTATs.Remove(cSCOSTAT);
                                    }
                                    finally
                                    {

                                    }
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            datarow.ElementAt(idx)["New"] = e.Message;
                        }
                        finally
                        {

                            idx++;
                        }
                    }
                    datarow = datarow.Where(y => y.Field<string>(7) != "Exist").OrderBy(x => x.Field<string>("New")).ThenBy(y => y.Field<int>("status_id"));

                }

                DataTable ndt = datarow.CopyToDataTable<DataRow>();


                return View(ndt);

                string conString = ConfigurationManager.ConnectionStrings["SirisCS"].ConnectionString;



                using (FbConnection con = new FbConnection(conString))
                {

                    //datarow = dt.AsEnumerable();
                    //newrow = datarow.Where(x => x.Field<int>("Id") == 2);

                };

                //using (SqlConnection con = new SqlConnection(conString))

                //{

                //    using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(con))

                //    {

                //        //Set the database table name.

                //        sqlBulkCopy.DestinationTableName = "dbo.Customers";



                //        //[OPTIONAL]: Map the DataTable columns with that of the database table

                //        sqlBulkCopy.ColumnMappings.Add("Id", "CustomerId");

                //        sqlBulkCopy.ColumnMappings.Add("Name", "Name");

                //        sqlBulkCopy.ColumnMappings.Add("Country", "Country");



                //        con.Open();

                //        sqlBulkCopy.WriteToServer(dt);

                //        con.Close();

                //    }

                //        }

            }



            return View();

        }

        private void UpdatePreviousRow(CSCOSTAT cSCOSTAT)
        {
            CSCOSTAT curRec = db.CSCOSTATs.Where(m => m.CONO == cSCOSTAT.CONO && m.ROWNO < cSCOSTAT.ROWNO).OrderByDescending(n => n.ROWNO).FirstOrDefault();
            if (curRec != null)
            {
                System.DateTime lastDate = (cSCOSTAT.SDATE).AddDays(-1);

                curRec.EDATE = lastDate;
                curRec.STAMP = curRec.STAMP + 1;
                db.Entry(curRec).State = EntityState.Modified;
            }
        }

        private CSCOAR createNextYearAR(CSCOAR curRec)
        {
            string sid = curRec.CONO;
            CSCOMSTR cSCOMSTR = db.CSCOMSTRs.Find(sid);

            DateTime indate = cSCOMSTR.INCDATE; // get incorporate date to determine AR due
            int yy = indate.Year;
            int mm = indate.Month;
            int dd = indate.Day;

            CSCOAR newRec = new CSCOAR();
            newRec.ARNO = (short)(curRec.ARNO + 1); // WARNING assuming the next ARNO is available
            newRec.LASTAR = curRec.FILEDAR;
            newRec.CONO = curRec.CONO;
            int lyy = newRec.LASTAR?.Year ?? yy;
            lyy = lyy + 1; // add 1 year to last year File AR Date

            newRec.ARTOFILE = new DateTime(lyy, mm, dd);

            dd = 1; // reminder is set to 1 month prior to AR Due Date and on the 1st
            if (mm == 1) { mm = 12; lyy = lyy - 1; } else { mm = mm - 1; };
            newRec.REMINDER1 = new DateTime(lyy, mm, dd);
            newRec.STAMP = 1;
            return newRec;
        }

    }
}