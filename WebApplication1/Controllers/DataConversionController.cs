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

namespace WebApplication1.Controllers
{
    public class DataConversionController : Controller
    {
        // GET: DataConversion

        private ASIDBConnection db = new ASIDBConnection();
        #region custom lookup definition hardcoded
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

        private string[] scdcol =
        {
            "New",
    "SRC_SYS_ID-3",
    "CGR_IP_ID-50",
    "UNQ_ID_CGR-300",
    "IP_TP_GRP_ID-10",
    "IP_TP_ID-20",
    "PRIM_IDENTN_TP_ID-50",
    "PRIM_IDENTN_NO-50",
    "BNM_ASGN_ID-50",
    "NM-200",
    "NEW_IC-50",
    "OLD_IC-50",
    "PSPT_NO-50",
    "ARMY_IDENTN_NO-50",
    "POLICE_IDENTN_NO-50",
    "BRTH_CTF_NO-50",
    "ORG_BSN_RGST_ID-50",
    "BRTH_DT-50",
    "ESTB_DT-50",
    "GND_ID-20",
    "CTY_CTZN_ID-20",
    "CTY_RSDNC_ID-20",
    "RSDNC_TP_ID-20",
    "SME_F-20",
    "IDY_CL_ID-20",
    "OCP_TP_ID-20",
    "ANUL_SLRY-25",
    "EMPR_NM-256",
    "EMP_TYPE-10",
    "EMP_SECTOR-10",
    "POST_ADR_ID-50",
    "POST_ADR_LINE_1-150",
    "POST_ADR_LINE_2-150",
    "POST_ADR_LINE_3-150",
    "POST_ADR_LINE_4-150",
    "POST_ADR_LINE_5-150",
    "POST_CITY_DSC-100",
    "POST_PSTCD_AREA_ID-20",
    "POST_STE_ID-100",
    "POST_CTY_ID-20",
    "PERM_ADR_ID-50",
    "PERM_ADR_LINE_1-200",
    "PERM_ADR_LINE_2-150",
    "PERM_ADR_LINE_3-150",
    "PERM_ADR_LINE_4-150",
    "PERM_ADR_LINE_5-150",
    "PERM_CITY_DSC-100",
    "PERM_PSTCD_AREA_ID-50",
    "PERM_STE_ID-100",
    "PERM_CTY_ID-20",
    "RSDNT_ADR_ID-50",
    "RSDNT_ADR_LINE_1-200",
    "RSDNT_ADR_LINE_2-150",
    "RSDNT_ADR_LINE_3-150",
    "RSDNT_ADR_LINE_4-150",
    "RSDNT_ADR_LINE_5-150",
    "RSDNT_CITY_DSC-100",
    "RSDNT_PSTCD_AREA_ID-20",
    "RSDNT_STE_ID-100",
    "RSDNT_CTY_ID-20",
    "BIZ_ADR_ID-50",
    "BIZ_ADR_LINE_1-200",
    "BIZ_ADR_LINE_2-150",
    "BIZ_ADR_LINE_3-150",
    "BIZ_ADR_LINE_4-150",
    "BIZ_ADR_LINE_5-150",
    "BIZ_CITY_DSC-100",
    "BIZ_PSTCD_AREA_ID-20",
    "BIZ_STE_ID-100",
    "BIZ_CTY_ID-20",
    "BIZ_CTY_USR-20",
    "OTH_ADR_ID-50",
    "OTH_ADR_LINE_1-200",
    "OTH_ADR_LINE_2-150",
    "OTH_ADR_LINE_3-150",
    "OTH_ADR_LINE_4-150",
    "OTH_ADR_LINE_5-150",
    "OTH_CITY_DSC-100",
    "OTH_PSTCD_AREA_ID-20",
    "OTH_STE_ID-100",
    "OTH_CTY_ID-20",
    "MP_TEL_ID-50",
    "MP_TEL_NO-50",
    "HP_TEL_ID-50",
    "HP_TEL_NO-50",
    "OP_TEL_ID-50",
    "OP_TEL_NO-50",
    "FAX_TEL_ID-50",
    "FAX_TEL_NO-50",
    "OTH_TEL_ID-50",
    "OTH_TEL_NO-50",
    "IP_TP_ID_EFF_DTE-50",
    "IP_TP_ID_RUF-50",
    "IP_TP_ID_JFA-100",
    "IP_TP_ID_DOC-100",
    "IP_TP_ID_USR-20",
    "PRIM_IDENTN_TP_ID_EFF_DTE-50",
    "PRIM_IDENTN_TP_ID_RUF-10",
    "PRIM_IDENTN_TP_ID_JFA-10",
    "PRIM_IDENTN_TP_ID_DOC-100",
    "PRIM_IDENTN_TP_ID_USR-20",
    "PRIM_IDENTN_NO_EFF_DTE-50",
    "PRIM_IDENTN_NO_RUF-10",
    "PRIM_IDENTN_NO_JFA-10",
    "PRIM_IDENTN_NO_DOC-100",
    "PRIM_IDENTN_NO_USR-20",
    "BNM_ASGN_ID_EFF_DTE-50",
    "BNM_ASGN_ID_RUF-10",
    "BNM_ASGN_ID_JFA-10",
    "BNM_ASGN_ID_DOC-100",
    "BNM_ASGN_ID_USR-20",
    "NM_EFF_DTE-50",
    "NM_RUF-10",
    "NM_JFA-20",
    "NM_DOC-100",
    "NM_USR-10",
    "NEW_IC_EFF_DTE-50",
    "NEW_IC_RUF-10",
    "NEW_IC_JFA-10",
    "NEW_IC_DOC-100",
    "NEW_IC_USR-20",
    "OLD_IC_EFF_DTE-50",
    "OLD_IC_RUF-10",
    "OLD_IC_JFA-10",
    "OLD_IC_DOC-100",
    "OLD_IC_USR-20",
    "PSPT_NO_EFF_DTE-50",
    "PSPT_NO_RUF-10",
    "PSPT_NO_JFA-10",
    "PSPT_NO_DOC-100",
    "PSPT_NO_USR-20",
    "ARMY_IDENTN_NO_EFF_DTE-50",
    "ARMY_IDENTN_NO_RUF-10",
    "ARMY_IDENTN_NO_JFA-10",
    "ARMY_IDENTN_NO_DOC-100",
    "ARMY_IDENTN_NO_USR-20",
    "POLICE_IDENTN_NO_EFF_DTE-50",
    "POLICE_IDENTN_NO_RUF-10",
    "POLICE_IDENTN_NO_JFA-10",
    "POLICE_IDENTN_NO_DOC-100",
    "POLICE_IDENTN_NO_USR-20",
    "BRTH_CTF_NO_EFF_DTE-50",
    "BRTH_CTF_NO_RUF-10",
    "BRTH_CTF_NO_JFA-10",
    "BRTH_CTF_NO_DOC-100",
    "BRTH_CTF_NO_USR-20",
    "ORG_BSN_RGST_ID_EFF_DTE-50",
    "ORG_BSN_RGST_ID_RUF-10",
    "ORG_BSN_RGST_ID_JFA-10",
    "ORG_BSN_RGST_ID_DOC-100",
    "ORG_BSN_RGST_ID_USR-20",
    "BRTH_DT_EFF_DTE-50",
    "BRTH_DT_RUF-10",
    "BRTH_DT_JFA-10",
    "BRTH_DT_DOC-100",
    "BRTH_DT_USR-20",
    "ESTB_DT_EFF_DTE-50",
    "ESTB_DT_RUF-10",
    "ESTB_DT_JFA-10",
    "ESTB_DT_DOC-100",
    "ESTB_DT_USR-20",
    "GND_ID_EFF_DTE-50",
    "GND_ID_RUF-10",
    "GND_ID_JFA-10",
    "GND_ID_DOC-100",
    "GND_ID_USR-20",
    "CTY_CTZN_ID_EFF_DTE-50",
    "CTY_CTZN_ID_RUF-10",
    "CTY_CTZN_ID_JFA-10",
    "CTY_CTZN_ID_DOC-100",
    "CTY_CTZN_ID_USR-20",
    "CTY_RSDNC_ID_EFF_DTE-50",
    "CTY_RSDNC_ID_RUF-10",
    "CTY_RSDNC_ID_JFA-10",
    "CTY_RSDNC_ID_DOC-100",
    "CTY_RSDNC_ID_USR-20",
    "RSDNC_TP_ID_EFF_DTE-50",
    "RSDNC_TP_ID_RUF-10",
    "RSDNC_TP_ID_JFA-10",
    "RSDNC_TP_ID_DOC-100",
    "RSDNC_TP_ID_USR-20",
    "SME_F_EFF_DTE-50",
    "SME_F_RUF-10",
    "SME_F_JFA-10",
    "SME_F_DOC-100",
    "SME_F_USR-20",
    "IDY_CL_ID_EFF_DTE-50",
    "IDY_CL_ID_RUF-10",
    "IDY_CL_ID_JFA-10",
    "IDY_CL_ID_DOC-100",
    "IDY_CL_ID_USR-20",
    "OCP_TP_ID_EFF_DTE-50",
    "OCP_TP_ID_RUF-10",
    "OCP_TP_ID_JFA-10",
    "OCP_TP_ID_DOC-100",
    "OCP_TP_ID_USR-20",
    "ANUL_SLRY_EFF_DTE-50",
    "ANUL_SLRY_RUF-10",
    "ANUL_SLRY_JFA-10",
    "ANUL_SLRY_DOC-100",
    "ANUL_SLRY_USR-20",
    "EMPR_NM_EFF_DTE-50",
    "EMPR_NM_RUF-10",
    "EMPR_NM_JFA-10",
    "EMPR_NM_DOC-100",
    "EMPR_NM_USR-20",
    "EMP_TYPE_EFF_DTE-50",
    "EMP_TYPE_RUF-10",
    "EMP_TYPE_JFA-10",
    "EMP_TYPE_DOC-100",
    "EMP_TYPE_USR-20",
    "EMP_SECTOR_EFF_DTE-50",
    "EMP_SECTOR_RUF-10",
    "EMP_SECTOR_JFA-10",
    "EMP_SECTOR_DOC-100",
    "EMP_SECTOR_USR-20",
    "POST_ADR_LN_EFF_DTE-50",
    "POST_ADR_LN_RUF-10",
    "POST_ADR_LN_JFA-10",
    "POST_ADR_LN_DOC-100",
    "POST_ADR_LN_USR-20",
    "POST_PSTCD_EFF_DTE-50",
    "POST_PSTCD_RUF-10",
    "POST_PSTCD_JFA-10",
    "POST_PSTCD_DOC-100",
    "POST_PSTCD_USR-20",
    "POST_STE_EFF_DTE-50",
    "POST_STE_RUF-10",
    "POST_STE_JFA-10",
    "POST_STE_DOC-100",
    "POST_STE_USR-20",
    "POST_CTY_EFF_DTE-50",
    "POST_CTY_RUF-10",
    "POST_CTY_JFA-10",
    "POST_CTY_DOC-100",
    "POST_CTY_USR-20",
    "PERM_ADR_LN_EFF_DTE-50",
    "PERM_ADR_LN_RUF-10",
    "PERM_ADR_LN_JFA-10",
    "PERM_ADR_LN_DOC-100",
    "PERM_ADR_LN_USR-20",
    "PERM_PSTCD_EFF_DTE-50",
    "PERM_PSTCD_RUF-10",
    "PERM_PSTCD_JFA-10",
    "PERM_PSTCD_DOC-100",
    "PERM_PSTCD_USR-20",
    "PERM_STE_EFF_DTE-50",
    "PERM_STE_RUF-10",
    "PERM_STE_JFA-10",
    "PERM_STE_DOC-100",
    "PERM_STE_USR-20",
    "PERM_CTY_EFF_DTE-50",
    "PERM_CTY_RUF-10",
    "PERM_CTY_JFA-10",
    "PERM_CTY_DOC-100",
    "PERM_CTY_USR-20",
    "RSDNT_ADR_LN_EFF_DTE-50",
    "RSDNT_ADR_LN_RUF-10",
    "RSDNT_ADR_LN_JFA-10",
    "RSDNT_ADR_LN_DOC-100",
    "RSDNT_ADR_LN_USR-20",
    "RSDNT_PSTCD_EFF_DTE-50",
    "RSDNT_PSTCD_RUF-10",
    "RSDNT_PSTCD_JFA-10",
    "RSDNT_PSTCD_DOC-100",
    "RSDNT_PSTCD_USR-20",
    "RSDNT_STE_EFF_DTE-50",
    "RSDNT_STE_RUF-10",
    "RSDNT_STE_JFA-10",
    "RSDNT_STE_DOC-100",
    "RSDNT_STE_USR-20",
    "RSDNT_CTY_EFF_DTE-50",
    "RSDNT_CTY_RUF-10",
    "RSDNT_CTY_JFA-10",
    "RSDNT_CTY_DOC-100",
    "RSDNT_CTY_USR-20",
    "BIZ_ADR_LN_EFF_DTE-50",
    "BIZ_ADR_LN_RUF-10",
    "BIZ_ADR_LN_JFA-10",
    "BIZ_ADR_LN_DOC-100",
    "BIZ_ADR_LN_USR-20",
    "BIZ_PSTCD_EFF_DTE-50",
    "BIZ_PSTCD_RUF-10",
    "BIZ_PSTCD_JFA-100",
    "BIZ_PSTCD_DOC-10",
    "BIZ_PSTCD_USR-20",
    "BIZ_STE_EFF_DTE-50",
    "BIZ_STE_RUF-10",
    "BIZ_STE_JFA-10",
    "BIZ_STE_DOC-100",
    "BIZ_STE_USR-20",
    "BIZ_CTY_EFF_DTE-50",
    "BIZ_CTY_RUF-10",
    "BIZ_CTY_JFA-10",
    "BIZ_CTY_DOC-100",
    "OTH_ADR_LN_EFF_DTE-50",
    "OTH_ADR_LN_RUF-10",
    "OTH_ADR_LN_JFA-10",
    "OTH_ADR_LN_DOC-100",
    "OTH_ADR_LN_USR-20",
    "OTH_PSTCD_EFF_DTE-50",
    "OTH_PSTCD_RUF-10",
    "OTH_PSTCD_JFA-10",
    "OTH_PSTCD_DOC-100",
    "OTH_PSTCD_USR-20",
    "OTH_STE_EFF_DTE-50",
    "OTH_STE_RUF-10",
    "OTH_STE_JFA-10",
    "OTH_STE_DOC-100",
    "OTH_STE_USR-20",
    "OTH_CTY_EFF_DTE-50",
    "OTH_CTY_RUF-10",
    "OTH_CTY_JFA-10",
    "OTH_CTY_DOC-100",
    "OTH_CTY_USR-20",
    "MP_EFF_DTE-50",
    "MP_RUF-10",
    "MP_JFA-10",
    "MP_DOC-100",
    "MP_USR-20",
    "HP_EFF_DTE-50",
    "HP_RUF-10",
    "HP_JFA-10",
    "HP_DOC-100",
    "HP_USR-20",
    "OP_EFF_DTE-50",
    "OP_RUF-10",
    "OP_JFA-10",
    "OP_DOC-100",
    "OP_USR-20",
    "FAX_EFF_DTE-50",
    "FAX_RUF-10",
    "FAX_JFA-10",
    "FAX_DOC-100",
    "FAX_USR-20",
    "OTH_EFF_DTE-50",
    "OTH_RUF-10",
    "OTH_JFA-10",
    "OTH_DOC-100",
    "OTH_USR-20",
    "RC_RCRD_CRT_DT-50",
    "UPD_USR-50",
    "EFF_START-50",
    "EFF_END-50",
    "PPN_DTE-50",
    "BTCH_ID-50",
    "EFF_DTE-50",
    "END_DTE-50",
    "IP_ID-50",
    "UNQ_ID_SRC_STM-30",
    "ANUL_SLRY_SURV-3",
    "ARMY_IDENTN_NO_SURV-3",
    "BIZ_ADR_SURV-3",
    "BNM_ASGN_ID_SURV-3",
    "BRTH_CTF_NO_SURV-3",
    "BRTH_DT_SURV-3",
    "BUMI_IND_SURV-3",
    "CTY_CTZN_SURV-3",
    "CTY_RSDNC_SURV-3",
    "CTZN_TP_SURV-3",
    "EMP_SEC_SURV-3",
    "EMP_TP_SURV-3",
    "EMPR_NM_SURV-3",
    "ESTB_DT_SURV-3",
    "FAX_TEL_SURV-3",
    "HP_TEL_SURV-3",
    "IDY_CL_SURV-3",
    "IP_TP_GRP_SURV-3",
    "IP_TP_SURV-3",
    "MAR_ST_SURV-3",
    "MP_TEL_SURV-3",
    "NEW_IC_SURV-3",
    "NM_SURV-3",
    "OCP_TP_SURV-3",
    "OLD_IC_SURV-3",
    "OP_TEL_SURV-3",
    "ORG_BSN_RGST_ID_SURV-3",
    "OTH_ADR_SURV-3",
    "OTH_TEL_SURV-3",
    "PERM_ADR_SURV-3",
    "POLICE_IDENTN_NO_SURV-3",
    "POST_ADR_SURV-3",
    "PRIM_IDENTN_SURV-3",
    "PSPT_NO_SURV-3",
    "REG_SB_SRW_SURV-3",
    "RSDNC_TP_SURV-3",
    "RSDNT_ADR_SURV-3",
    "SALUT_SURV-3",
    "SME_F_SURV-3",
    "STAFF_F_SURV-3",
    "SYSGENCOL"
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
        #endregion
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

        private void addColumns1(DataTable dt, string[] col)
        {
            int j = col.Length;
            for (int i = 0; i < j; i++)
            {
                if (i == 1)
                {
                    dt.Columns.Add(new DataColumn(col[i], typeof(string)));
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
            ViewBag.Title = "Data Conversion";
            return View(dt);

        }

        public ActionResult Index1()

        {
            DataTable dt = new DataTable();
            addColumns(dt, scdcol);
            dt.Rows.Add();
            ViewBag.Title = "Data Conversion";
            return View(dt);

        }

        [HttpPost]

        public ActionResult Index1(HttpPostedFileBase postedFile, string dbTableName, string dbKey, string localKey)

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

                //postedFile.SaveAs(filePath);

                DataTable dt = new DataTable();

                //addColumns(dt, companycol);
                //dt.Columns.AddRange(new DataColumn[3] { new DataColumn("CaseFeeId", typeof(int)),

                //                new DataColumn("CaseFeeCode", typeof(string)),

                //                new DataColumn("CaseFeeDesc",typeof(string)) });





                //Read the contents of CSV file.

                string csvData = System.IO.File.ReadAllText(postedFile.FileName);
                //string pattern = "/";
                //Regex rgx = new Regex(pattern);


                string[] stringSeparators = new string[] { "\r\n" };
                string[] lines = csvData.Split(stringSeparators, StringSplitOptions.None);


                //Execute a loop over the rows.
                bool IsHeader = false;
                addColumns1(dt, scdcol);

                int rowcnt = 0;
                DataRow curRow = null;
                foreach (string row in lines)

                {

                    if ((!IsHeader) && (!string.IsNullOrEmpty(row)))

                    {

                        //if (curRow == null)
                        //{
                            curRow = dt.Rows.Add();
                        //}
                        rowcnt++;
                        int i = 1;
                        int cellsize = 0;
                        int maxsize = 0;
                        string colname = "";


                        //Execute a loop over the columns.
                        dt.Rows[dt.Rows.Count - 1][0] = rowcnt.ToString("D4");
                        foreach (string cell in row.Split('§'))
                        //foreach (string cell in row.Split('�'))
                        {
                            try
                            {
                                dt.Rows[dt.Rows.Count - 1][i] = cell.Replace("\"", "");
                                cellsize = cell.Length;
                                colname = scdcol[i];
                                maxsize = int.Parse(colname.Split('-')[1]);
                                if (cellsize > maxsize)
                                {
                                    setRemark1(dt,rowcnt, "SYSGENCOL", rowcnt.ToString() + " " + colname.Split('-')[0] + " exceed length " + cellsize.ToString() + " > " + maxsize.ToString());
                                    //return View(dt);
                                    //break;
                                }
                                //setRemark1(dt, rowcnt, "SYSGENCOL", colname.Split('-')[0] + " OK");
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(rowcnt.ToString() + " | " + e.Message);
                                setRemark1(dt, rowcnt, "SYSGENCOL", e.Message);
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
                                    myCol.Add(cell.TrimEnd('\r', '\n'));
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine(rowcnt.ToString() + " | " + e.Message);
                                }
                                finally
                                {


                                }

                            }

                            myCol.Add("SYSGENCOL");

                            string[] myArray = new string[myCol.Count];
                            for (int c = 0; c < myCol.Count; c++)
                            {
                                myArray[c] = myCol[c];
                            }
                            addColumns(dt, myArray);
                        }
                    }

                    setRemark1(dt, rowcnt, "SYSGENCOL", "OK");
                }

                var datarow = dt.AsEnumerable();
                ViewBag.Title = postedFile.FileName;
                datarow = datarow.Where(x => x.Field<string>("SYSGENCOL") != "OK" );
                DataTable ndt = datarow.CopyToDataTable<DataRow>();

                ViewBag.Title = postedFile.FileName;
                return View(ndt);
            }
            return View();
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

            string[] stringSeparators = new string[] { "\r\n" };
            string[] lines = csvData.Split(stringSeparators, StringSplitOptions.None);

            foreach (string row in lines)

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
                string usefile1 = "";
                string usefile2 = "";
                string usefile3 = "";
                string usefile4 = "";
                #region specific table references
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
                else if (dbTableName == "CSCONAME")
                {
                    csRecs = db.CSCONAMEs.ToList();
                    usefile = path + "cs_company1.csv";
                }
                else if (dbTableName == "CSCOAR")
                {
                    csRecs = db.CSCOARs.ToList();
                    usefile = path + "cs_company1.csv";
                }
                else if (dbTableName == "CSCOAGM")
                {
                    csRecs = db.CSCOAGMs.ToList();
                    usefile = path + "cs_company1.csv";
                }
                else if (dbTableName == "CSCOFEE")
                {
                    csRecs = db.CSCOFEEs.ToList();
                    usefile = path + "cs_company1.csv";
                }
                else if (dbTableName == "CSCASE")
                {
                    csRecs = db.CSCASEs.ToList();
                }
                else if (dbTableName == "CSCOADDR")
                {
                    csRecs = db.CSCOADDRs.ToList();
                    usefile = path + "cs_company1.csv";
                }
                else if (dbTableName == "CSJOBM")
                {
                    csRecs = db.CSJOBMs.ToList();
                    usefile = path + "cs_company1.csv";
                    usefile1 = path + "g_users1.csv";
                    usefile2 = path + "g_staffmap1.csv";
                    usefile3 = path + "cs_jobcase2.csv";
                    usefile4 = path + "cs_jobcasehist2.csv";
                }
                else if (dbTableName == "CSBILL")
                {
                    csRecs = db.CSBILLs.Where(z => z.STAMP >= 990).Include(x => x.CSITEM).Include(y => y.CSCOMSTR).ToList();
                    usefile = path + "cs_company1.csv";
                    usefile1 = path + "cs_job2.csv";
                    usefile2 = path + "cs_jobcase2.csv";

                }
                else if (dbTableName == "CSPRF")
                {
                    csRecs = db.CSPRFs.Where(z => z.STAMP >= 990).ToList();
                    usefile = path + "cs_company1.csv";
                    usefile1 = path + "g_users1.csv";
                    usefile2 = path + "cs_billingitem2.csv";

                }
                else if (dbTableName == "CSRCP")
                {
                    csRecs = db.CSRCPs.Where(z => z.STAMP >= 990).ToList();
                    usefile = path + "cs_company1.csv";
                    usefile1 = path + "g_users1.csv";
                    usefile2 = path + "cs_billingitem2.csv";
                    usefile3 = path + "cs_paymentitem2.csv";
                    usefile4 = path + "cs_jobcase2.csv";

                }
                else if (dbTableName == "CSCNM")
                {
                    csRecs = db.CSCNMs.Where(z => z.STAMP == 999).ToList();
                    usefile = path + "cs_company1.csv";
                    usefile1 = path + "g_users1.csv";
                    usefile3 = path + "cs_creditnote_billing2.csv";


                }
                #endregion
                //Create a DataTable.
                #region read csv file header and detail
                DataTable dt = new DataTable();

                //addColumns(dt, companycol);
                //dt.Columns.AddRange(new DataColumn[3] { new DataColumn("CaseFeeId", typeof(int)),

                //                new DataColumn("CaseFeeCode", typeof(string)),

                //                new DataColumn("CaseFeeDesc",typeof(string)) });





                //Read the contents of CSV file.

                string csvData = System.IO.File.ReadAllText(filePath);
                //string pattern = "/";
                //Regex rgx = new Regex(pattern);


                string[] stringSeparators = new string[] { "\r\n" };
                string[] lines = csvData.Split(stringSeparators, StringSplitOptions.None);


                //Execute a loop over the rows.
                bool IsHeader = true;

                int rowcnt = 0;
                DataRow curRow = null;
                foreach (string row in lines)

                {

                    if ((!IsHeader) && (!string.IsNullOrEmpty(row)))

                    {

                        curRow = dt.Rows.Add();
                        rowcnt++;
                        int i = 1;



                        //Execute a loop over the columns.
                        dt.Rows[dt.Rows.Count - 1][0] = rowcnt.ToString("D4");
                        //foreach (string cell in row.Split('\t'))
                        foreach (string cell in row.Split('|'))
                        {
                            try
                            {
                                dt.Rows[dt.Rows.Count - 1][i] = cell.Replace("\"", "");
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

                        if (MustAddDate(dbTableName))
                        {

                            string dateCol = "";
                            if (dbTableName == "CSCOSTAT")
                            {
                                dateCol = "status_date";
                            }
                            else if (dbTableName == "CSCOAR")
                            {
                                dateCol = "ar_date";
                            }
                            else if (dbTableName == "CSCOAGM")
                            {
                                dateCol = "fs_date";
                            }
                            else if (dbTableName == "CSJOBM")
                            {
                                dateCol = "job_entrydate";
                            }
                            else if (dbTableName == "CSBILL")
                            {
                                dateCol = "billingitem_entrydate";
                            }
                            else if (dbTableName == "CSPRF")
                            {
                                dateCol = "perfomabill_entrydate";
                            }
                            else if (dbTableName == "CSRCP")
                            {
                                dateCol = "payment_date";
                            }
                            else if (dbTableName == "CSCNM")
                            {
                                dateCol = "creditnote_entrydate";
                            }
                            else if (dbTableName == "CSCONAME")
                            {
                                dateCol = "name_effectivedate";
                            }
                            string myDateStr = (string)curRow[dateCol];

                            if (myDateStr.Length == 10)
                            {
                                string mySortStr = myDateStr.Split('/')[2] + myDateStr.Split('/')[1] + myDateStr.Split('/')[0];
                                curRow["sort_date"] = mySortStr;
                            }
                            else
                            {
                                curRow["sort_date"] = "00000000";
                            }

                        }

                        if ((localKey.Length > 0) && (DBNull.Value != curRow[localKey]))
                        {
                            int keyInt = (int)curRow[localKey];
                            keyId = keyInt.ToString(); 

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
                                    myCol.Add(cell.TrimEnd('\r', '\n'));
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine(rowcnt.ToString() + " | " + e.Message);
                                }
                                finally
                                {


                                }

                            }
                            if (MustAddDate(dbTableName))
                            {
                                myCol.Add("sort_date");
                            }
                            myCol.Add("SYSGENCOL");

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
                #endregion
                #region CSCOAR

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
                                        if (row["ar_submissiondate"] != DBNull.Value) { cSCOAR.SUBMITAR = DateTime.Parse((string)row["ar_submissiondate"]); };
                                        if (row["ar_reminderdate"] != DBNull.Value) { cSCOAR.REMINDER1 = DateTime.Parse((string)row["ar_reminderdate"]); };
                                        if ((row[7] != DBNull.Value) && ((string)row[7] != "")) { cSCOAR.FILEDAR = DateTime.Parse((string)row[7]); };

                                        if (ModelState.IsValid)
                                        {
                                            int? lastRowNo = 0;
                                            try
                                            {
                                                lastRowNo = db.CSCOARs.Where(m => m.CONO == cSCOAR.CONO).Max(n => n.ARNO);
                                            }
                                            catch (Exception e) { lastRowNo = 0; }
                                            finally { };


                                            cSCOAR.ARNO = (short)((lastRowNo ?? 0) + 1);
                                            datarow.ElementAt(idx)[6] = cSCOAR.ARNO.ToString();

                                            int lastARNo = cSCOAR.ARNO;


                                            string sid = cSCOAR.CONO;
                                            CSCOAR lastRec = db.CSCOARs.Where(m => m.CONO == sid && m.ARNO < lastARNo).OrderByDescending(n => n.ARNO).FirstOrDefault();

                                            if ((lastRec != null) && (lastRec.FILEDAR != null)) // edit last year record
                                            {
                                                cSCOAR.LASTAR = lastRec.FILEDAR;
                                            }
                                            db.CSCOARs.Add(cSCOAR);
                                            db.SaveChanges();



                                        }
                                    }
                                    catch (Exception e)
                                    {
                                        datarow.ElementAt(idx)[6] = e.Message + "!!!";
                                        //db.CSCOARs.Remove(cSCOAR);
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
                    datarow = datarow.Where(y => y.Field<string>(7) != "Exist").OrderBy(x => x.Field<string>("New")).ThenBy(y => y.Field<int>("ar_id"));

                }
                #endregion
                #region CSCOAGM
                else if (dbTableName == "CSCOAGM")
                {
                    datarow = datarow.OrderBy(x => x.Field<string>("company_id")).ThenBy(n => n.Field<int>("fs_id")).Select(x => x);

                    refTable = OpenTable(usefile);
                    var csRefs = refTable.AsEnumerable();


                    int idx = 0;
                    int coid = 0;
                    string cono = "";
                    DateTime sdate;
                    CSCOAGM csRec = null;
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

                                sdate = DateTime.Parse((string)(datarow.ElementAt(idx)["fs_date"]));
                                csRec = ((List<CSCOAGM>)csRecs).Where(item => item.CONO == cono && item.FYETOFILE == sdate).FirstOrDefault();
                                if (csRec != null) { datarow.ElementAt(idx)["SYSGENCOL"] = "Exist"; }
                                else
                                {
                                    CSCOAGM cSCOAGM = new CSCOAGM();
                                    try
                                    {
                                        ModelState.Clear();

                                        cSCOAGM.STAMP = 999;
                                        cSCOAGM.CONO = cono;
                                        cSCOAGM.FYETOFILE = sdate;
                                        if ((row["fs_submissiondate"] != DBNull.Value) && ((string)row["fs_submissiondate"] != "")) { cSCOAGM.CIRCDATE = DateTime.Parse((string)row["fs_submissiondate"]); };
                                        if ((row["fs_reminderdate1"] != DBNull.Value) && ((string)row["fs_reminderdate1"] != "")) { cSCOAGM.REMINDER1 = DateTime.Parse((string)row["fs_reminderdate1"]); };
                                        if ((row["fs_reminderdate2"] != DBNull.Value) && ((string)row["fs_reminderdate2"] != "")) { cSCOAGM.REMINDER2 = DateTime.Parse((string)row["fs_reminderdate2"]); };
                                        if ((row["fs_reminderdate3"] != DBNull.Value) && ((string)row["fs_reminderdate3"] != "")) { cSCOAGM.REMINDER3 = DateTime.Parse((string)row["fs_reminderdate3"]); };
                                        if ((row["fs_fileddate"] != DBNull.Value) && ((string)row["fs_fileddate"] != "")) { cSCOAGM.FILEDFYE = DateTime.Parse((string)row["fs_fileddate"]); };
                                        if ((row["fs_deadline"] != DBNull.Value) && ((string)row["fs_deadline"] != "")) { cSCOAGM.AGMLAST = DateTime.Parse((string)row["fs_deadline"]); };
                                        if ((row["fs_agm"] != DBNull.Value) && ((string)row["fs_agm"] != "")) { cSCOAGM.AGMDATE = DateTime.Parse((string)row["fs_agm"]); };

                                        if (ModelState.IsValid)
                                        {
                                            int? lastRowNo = 0;
                                            try
                                            {
                                                lastRowNo = db.CSCOAGMs.Where(m => m.CONO == cSCOAGM.CONO).Max(n => n.AGMNO);
                                            }
                                            catch (Exception e) { lastRowNo = 0; }
                                            finally { };


                                            cSCOAGM.AGMNO = (short)((lastRowNo ?? 0) + 1);
                                            datarow.ElementAt(idx)["SYSGENCOL"] = cSCOAGM.AGMNO.ToString();

                                            int lastARNo = cSCOAGM.AGMNO;


                                            string sid = cSCOAGM.CONO;
                                            CSCOAGM lastRec = db.CSCOAGMs.Where(m => m.CONO == sid && m.AGMNO < lastARNo).OrderByDescending(n => n.AGMNO).FirstOrDefault();

                                            if ((lastRec != null) && (lastRec.FILEDFYE != null)) // edit last year record
                                            {
                                                cSCOAGM.LASTFYE = lastRec.FILEDFYE;
                                            }
                                            db.CSCOAGMs.Add(cSCOAGM);

                                            db.SaveChanges();


                                        }
                                    }
                                    catch (Exception e)
                                    {
                                        datarow.ElementAt(idx)["SYSGENCOL"] = e.Message + "!!!";
                                        // db.CSCOAGMs.Remove(cSCOAGM);
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

                    datarow = datarow.Where(y => y.Field<string>("SYSGENCOL") != "Exist").OrderBy(x => x.Field<string>("New")).ThenBy(y => y.Field<int>("fs_id"));

                }
                #endregion
                #region CSCOFEE
                else if (dbTableName == "CSCOFEE")
                {
                    datarow = datarow.OrderBy(x => x.Field<string>("company_id")).Select(x => x);

                    refTable = OpenTable(usefile);
                    var csRefs = refTable.AsEnumerable();


                    int idx = 0;
                    int coid = 0;
                    string cono = "";
                    string fcode = "";
                    CSCOFEE csRec = null;
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
                                fcode = (string)row["fee_description"];
                                fcode = fcode.Trim();
                                csRec = ((List<CSCOFEE>)csRecs).Where(item => item.CONO == cono && item.FEECODE == fcode).FirstOrDefault();
                                if (csRec != null) { datarow.ElementAt(idx)["SYSGENCOL"] = "Exist"; }
                                else
                                {
                                    CSCOFEE cSCOFEE = new CSCOFEE();
                                    try
                                    {
                                        ModelState.Clear();

                                        cSCOFEE.STAMP = 999;
                                        cSCOFEE.CONO = cono;
                                        cSCOFEE.FEECODE = fcode;
                                        if ((row["fee_amount"] != DBNull.Value) && ((string)row["fee_amount"] != "")) { cSCOFEE.FEEAMT = decimal.Parse(((string)row["fee_amount"])); };
                                        if ((row["fee_billto"] != DBNull.Value) && ((string)row["fee_billto"] != "")) { cSCOFEE.LASTTOUCH = DateTime.Parse((string)row["fee_billto"]); };
                                        if ((row["fee_billduration"] != DBNull.Value) && ((string)row["fee_billduration"] != "")) { cSCOFEE.FEEMTH = int.Parse((string)row["fee_billduration"]); };
                                        if ((row["fee_type"] != DBNull.Value) && ((string)row["fee_type"] != "")) { cSCOFEE.FEETYPE = (string)row["fee_type"]; };
                                        if (ModelState.IsValid)
                                        {


                                            db.CSCOFEEs.Add(cSCOFEE);

                                            db.SaveChanges();


                                        }
                                    }
                                    catch (Exception e)
                                    {
                                        datarow.ElementAt(idx)["SYSGENCOL"] = e.Message + "!!!";
                                        db.CSCOFEEs.Remove(cSCOFEE);
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

                    datarow = datarow.Where(y => y.Field<string>("SYSGENCOL") != "Exist").OrderBy(x => x.Field<string>("New"));

                }
                else if (dbTableName == "CSCASE")
                {
                    datarow = datarow.OrderBy(x => x.Field<string>("casefee_code")).Select(x => x);

                    int idx = 0;
                    int coid = 0;
                    string cono = "";
                    string fcode = "";
                    CSCASE csRec = null;
                    foreach (DataRow row in datarow)
                    {

                        try
                        {

                            coid = (int)row["casefee_id"];
                            if (true)
                            {

                                fcode = (string)row["casefee_code"];
                                fcode = fcode.Trim();
                                datarow.ElementAt(idx)["New"] = fcode.Length.ToString();
                                csRec = ((List<CSCASE>)csRecs).Where(item => item.CASECODE == fcode).FirstOrDefault();
                                if (csRec != null) { datarow.ElementAt(idx)["SYSGENCOL"] = "Exist"; }
                                else
                                {
                                    CSCASE cSCASE = new CSCASE();
                                    try
                                    {
                                        ModelState.Clear();

                                        cSCASE.STAMP = 999;
                                        cSCASE.CASECODE = fcode;
                                        cSCASE.CASEDESC = (string)row["casefee_description"];

                                        if (ModelState.IsValid)
                                        {


                                            db.CSCASEs.Add(cSCASE);
                                            db.SaveChanges();


                                        }
                                    }
                                    catch (Exception e)
                                    {
                                        datarow.ElementAt(idx)["SYSGENCOL"] = e.Message + "!!!";
                                        db.CSCASEs.Remove(cSCASE);
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

                    datarow = datarow.Where(y => y.Field<string>("SYSGENCOL") != "Exist").OrderBy(x => x.Field<string>("New"));

                }
                #endregion
                #region CSCOADDR
                else if (dbTableName == "CSCOADDR")
                {
                    datarow = datarow.OrderBy(x => x.Field<string>("company_id")).Select(x => x);

                    refTable = OpenTable(usefile);
                    var csRefs = refTable.AsEnumerable();


                    int idx = 0;
                    int coid = 0;
                    string cono = "";
                    string fcode = "";
                    string addrCity = "";
                    string addrState = "";

                    CSCOADDR csRec = null;

                    List<HKCITY> hkCity = db.HKCITies.ToList();
                    List<HKSTATE> hkState = db.HKSTATEs.ToList();

                    foreach (DataRow row in datarow)
                    {
                        templine = (string)row["company_id"];
                        coid = int.Parse(templine);
                        cono = null;
                        addrCity = "";
                        addrState = "";
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
                                fcode = (string)row["address_type"];
                                fcode = fcode.Trim();
                                csRec = ((List<CSCOADDR>)csRecs).Where(item => item.CONO == cono && item.ADDRTYPE == fcode).FirstOrDefault();
                                //csRec = null;
                                if (csRec != null) { datarow.ElementAt(idx)["SYSGENCOL"] = "Exist"; }
                                else
                                {
                                    CSCOADDR cSCOADDR = new CSCOADDR();
                                    try
                                    {
                                        ModelState.Clear();

                                        cSCOADDR.STAMP = 999;
                                        cSCOADDR.CONO = cono;
                                        cSCOADDR.ADDRTYPE = fcode;

                                        string addrline = (string)row["address_address"];
                                        addrline = addrline.TrimEnd(); //get rid of trailing newline
                                        addrline = addrline.TrimEnd('\n'); //get rid of trailing newline                                        
                                        addrline = addrline.TrimEnd(); //get rid of trailing newline
                                        addrline = addrline.TrimEnd('.');  // get rid of last dot

                                        string[] addrSeparators = new string[] { "\n" };
                                        string[] addrlines = addrline.Split(addrSeparators, StringSplitOptions.None);
                                        datarow.ElementAt(idx)["SYSGENCOL"] = addrlines.Length.ToString();


                                        string postcode = "";
                                        if (addrlines.Length > 3)
                                        {


                                            postcode = getPostcode(addrlines[addrlines.Length - 1]);
                                            if (postcode == "")
                                            {
                                                postcode = getPostcode(addrlines[addrlines.Length - 2]);

                                                if (postcode == "")
                                                {
                                                    postcode = getPostcode(addrlines[addrlines.Length - 3]);
                                                    if (postcode != "")
                                                    {
                                                        addrlines[addrlines.Length - 3] = addrlines[addrlines.Length - 3].Substring(postcode.Length + 1);
                                                    }
                                                }
                                                else
                                                {
                                                    addrlines[addrlines.Length - 2] = addrlines[addrlines.Length - 2].Substring(postcode.Length + 1);
                                                }
                                            }
                                            else
                                            {
                                                addrlines[addrlines.Length - 1] = addrlines[addrlines.Length - 1].Substring(postcode.Length + 1);
                                            }
                                            addrCity = addrlines[addrlines.Length - 2];
                                            addrState = addrlines[addrlines.Length - 1];

                                            ;


                                        }
                                        else if (addrlines.Length > 2)
                                        {
                                            postcode = getPostcode(addrlines[addrlines.Length - 1]);
                                            if (postcode == "")
                                            {
                                                postcode = getPostcode(addrlines[addrlines.Length - 2]);

                                                if (postcode == "")
                                                {

                                                }
                                                else
                                                {
                                                    addrlines[addrlines.Length - 2] = addrlines[addrlines.Length - 2].Substring(postcode.Length + 1);
                                                }
                                            }
                                            else
                                            {
                                                addrlines[addrlines.Length - 1] = addrlines[addrlines.Length - 1].Substring(postcode.Length + 1);
                                            }
                                            addrCity = addrlines[addrlines.Length - 2];
                                            addrState = addrlines[addrlines.Length - 1];

                                            ;

                                        }

                                        addrCity = addrCity.TrimEnd();
                                        addrState = addrState.TrimEnd();
                                        addrCity = addrCity.TrimEnd(',', '.');
                                        addrState = addrState.TrimEnd(',', '.');

                                        string addrCityCode = hkCity.Where(x => x.CITYDESC.Contains(addrCity)).Select(y => y.CITYCODE).FirstOrDefault();

                                        datarow.ElementAt(idx)["address_city"] = "KL";// addrCity;

                                        if ((addrCityCode == null) || (addrCityCode == ""))
                                        {
                                            string[] statePartList = addrState.Split(' '); //city  might be in state column
                                            foreach (var statePart in statePartList)
                                            {
                                                addrCityCode = hkCity.Where(x => x.CITYDESC.Contains(statePart)).Select(y => y.CITYCODE).FirstOrDefault();
                                                if ((addrCityCode != null) && (addrCityCode != ""))
                                                {
                                                    break;
                                                }
                                            }
                                        }
                                        if ((addrCityCode != null) && (addrCityCode != "")) { datarow.ElementAt(idx)["address_city"] = addrCityCode; };

                                        string addrStateCode = hkState.Where(x => x.STATEDESC.Contains(addrState)).Select(y => y.STATECODE).FirstOrDefault();
                                        if ((addrStateCode == null) || (addrStateCode == ""))
                                        {
                                            string[] statePartList = addrState.Split(' ');
                                            foreach (var statePart in statePartList)
                                            {
                                                addrStateCode = hkState.Where(x => x.STATEDESC.Contains(statePart)).Select(y => y.STATECODE).FirstOrDefault();
                                                if ((addrStateCode != null) && (addrStateCode != ""))
                                                {
                                                    break;
                                                }
                                            }
                                        }

                                        datarow.ElementAt(idx)["address_state"] = "WLP";// addrState;
                                        if ((addrStateCode != null) && (addrStateCode != "")) { datarow.ElementAt(idx)["address_state"] = addrStateCode; }
                                        if (addrStateCode == null)
                                        {
                                            if (addrState == "Kuala Lumpur") { datarow.ElementAt(idx)["address_state"] = "WLP"; };
                                            if (addrState == "Singapore") { datarow.ElementAt(idx)["address_state"] = "SG"; };
                                        };

                                        datarow.ElementAt(idx)["address_postal"] = postcode;
                                        if (addrlines[addrlines.Length - 1] == "Singapore")
                                        {
                                            datarow.ElementAt(idx)["address_country"] = "SG";
                                        }
                                        else
                                        { datarow.ElementAt(idx)["address_country"] = "MY"; };

                                        if (addrlines.Length > 1) { cSCOADDR.ADDR1 = addrlines[0]; };
                                        if (addrlines.Length > 2) { cSCOADDR.ADDR2 = addrlines[1]; };
                                        if (addrlines.Length > 3) { cSCOADDR.ADDR3 = addrlines[2]; };
                                        cSCOADDR.ADDRTYPE = (string)row["address_type"];
                                        cSCOADDR.POSTAL = (string)row["address_postal"];
                                        cSCOADDR.CITYCODE = (string)row["address_city"];
                                        cSCOADDR.STATECODE = (string)row["address_state"];
                                        cSCOADDR.CTRYCODE = (string)row["address_country"];
                                        cSCOADDR.MAILADDR = (string)row["address_mailing"] == "t" ? "Y" : "N";

                                        string contact = (string)row["address_contact"];
                                        string fax = (string)row["address_fax"];

                                        string[] slsSeparators = new string[] { "/" };
                                        string[] nlSeparators = new string[] { "\n" };
                                        if (contact != "")
                                        {

                                            string[] contactlines = contact.Split(nlSeparators, StringSplitOptions.None);
                                            if (contactlines.Length > 1)
                                            {
                                                cSCOADDR.PHONE2 = getNumbersOnly(contactlines[1]);
                                                cSCOADDR.PHONE1 = getNumbersOnly(contactlines[0]);
                                            }
                                            else
                                            {
                                                string[] contactlines1 = contact.Split(slsSeparators, StringSplitOptions.None);
                                                if (contactlines1.Length > 1)
                                                {
                                                    cSCOADDR.PHONE2 = getNumbersOnly(contactlines1[1]);
                                                }
                                                cSCOADDR.PHONE1 = getNumbersOnly(contactlines1[0]);
                                            }

                                        }

                                        if (fax != "")
                                        {

                                            string[] faxlines = fax.Split(nlSeparators, StringSplitOptions.None);
                                            if (faxlines.Length > 1)
                                            {
                                                cSCOADDR.FAX2 = getNumbersOnly(faxlines[1]);
                                                cSCOADDR.FAX1 = getNumbersOnly(faxlines[0]);
                                            }
                                            else
                                            {
                                                string[] faxlines1 = contact.Split(slsSeparators, StringSplitOptions.None);
                                                if (faxlines1.Length > 1)
                                                {
                                                    cSCOADDR.PHONE2 = getNumbersOnly(faxlines1[1]);
                                                }
                                                cSCOADDR.FAX1 = getNumbersOnly(faxlines1[0]);
                                            }

                                        }


                                        //if ((row["fee_amount"] != DBNull.Value) && ((string)row["fee_amount"] != "")) { cSCOFEE.FEEAMT = decimal.Parse(((string)row["fee_amount"])); };
                                        //if ((row["fee_billto"] != DBNull.Value) && ((string)row["fee_billto"] != "")) { cSCOFEE.LASTTOUCH = DateTime.Parse((string)row["fee_billto"]); };
                                        //if ((row["fee_billduration"] != DBNull.Value) && ((string)row["fee_billduration"] != "")) { cSCOFEE.FEEMTH = int.Parse((string)row["fee_billduration"]); };
                                        //if ((row["fee_type"] != DBNull.Value) && ((string)row["fee_type"] != "")) { cSCOFEE.FEETYPE = (string)row["fee_type"]; };
                                        if (ModelState.IsValid)
                                        {
                                            int? lastRowNo = 0;
                                            try
                                            {
                                                lastRowNo = db.CSCOADDRs.Where(m => m.CONO == cSCOADDR.CONO).Max(n => n.ADDRID);
                                            }
                                            catch (Exception e) { lastRowNo = 0; }
                                            finally { };

                                            cSCOADDR.ADDRID = (short)((lastRowNo ?? 0) + 1);
                                            db.CSCOADDRs.Add(cSCOADDR);

                                            db.SaveChanges();


                                        }
                                    }
                                    catch (Exception e)
                                    {
                                        datarow.ElementAt(idx)["SYSGENCOL"] = e.Message + "!!!";
                                        db.CSCOADDRs.Remove(cSCOADDR);
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

                    datarow = datarow.Where(y => y.Field<string>("SYSGENCOL") != "Exist").OrderBy(x => x.Field<string>("New"));

                }
                #endregion
                #region CSJOBM
                else if (dbTableName == "CSJOBM")
                {
                    datarow = datarow.OrderBy(x => x.Field<int>("job_id")).Select(x => x);

                    refTable = OpenTable(usefile);
                    var csRefs = refTable.AsEnumerable();

                    DataTable refTable1 = OpenTable(usefile1);
                    var staffRefs = refTable1.AsEnumerable();

                    DataTable refTable2 = OpenTable(usefile2);
                    var mapRefs = refTable2.AsEnumerable();

                    DataTable detailTable = OpenTable(usefile3);
                    var detailRecs = detailTable.AsEnumerable();

                    DataTable detailHist = OpenTable(usefile4);
                    var historyRecs = detailHist.AsEnumerable();


                    int idx = 0;
                    int coid = 0;
                    int staffno = -999;
                    int mapid = -999;

                    string cono = "";
                    string staffcode = "";
                    string mapcode = "";
                    string mapbase = "";
                    string fcode = "";
                    string jcode = "";

                    CSJOBM csRec = null;
                    DateTime sdate;
                    DateTime fdate = DateTime.Parse("01/01/3000");
                    bool setfdate = false;
                    string caseid = "";

                    List<string> sqlupdate = new List<string>();

                    foreach (DataRow row in datarow)
                    {
                        // skip certain record
                        if ((int)row["job_id"] == 1) {; }
                        else
                        {

                            setfdate = false;
                            templine = (string)row["company_id"];
                            coid = int.Parse(templine);
                            cono = null;
                            staffno = int.Parse((string)row["createdby_userid"]);

                            mapid = int.Parse((string)row["assignto_mappingid"]);

                            try
                            {
                                var corec = csRefs.Where(x => x.Field<int?>("company_id") == coid).FirstOrDefault();
                                if (corec != null)
                                {
                                    cono = (string)corec["company_no"];
                                }
                                else cono = null;

                                var staffrec = staffRefs.Where(x => x.Field<int?>("user_id") == staffno).FirstOrDefault();
                                if (staffrec != null)
                                {
                                    staffcode = (string)staffrec["user_staffid"];
                                }
                                else staffcode = null;

                                var maprec = mapRefs.Where(x => x.Field<int?>("mapping_id") == mapid).FirstOrDefault();
                                if (maprec != null)
                                {

                                    if ((mapid == 13) || (mapid == 14))
                                    {
                                        jcode = ((int)row["job_id"]).ToString();
                                        fcode = ((int)row["job_id"]).ToString("D8");
                                        fcode = "X" + fcode.Trim();

                                        if (mapid == 13) {
                                            sqlupdate.Add("update csjobm set jobstaff = '8/6' where jobno = '" + fcode + "';");
                                                } else
                                        {
                                            sqlupdate.Add("update csjobm set jobstaff = '8/10' where jobno = '" + fcode + "';");
                                        }
                                    }

                                    mapcode = (string)maprec["mapping_node_id"];
                                    mapbase = (string)maprec["mapping_base_id"];
                                    if (mapcode != null)
                                    {
                                        mapid = Convert.ToInt32(mapcode);
                                        staffrec = staffRefs.Where(x => x.Field<int?>("user_id") == mapid).FirstOrDefault();
                                        if (staffrec != null)
                                        {
                                            mapcode = (string)staffrec["user_staffid"];
                                        }
                                        else mapcode = null;
                                    }
                                }
                                else mapcode = null;



                                cono = null; //force to stop further processing
                                // temporary suspend all processing to just get the staff mapping correct

                                if (cono != null)
                                {
                                    datarow.ElementAt(idx)["New"] = cono;

                                    sdate = DateTime.Parse((string)(datarow.ElementAt(idx)["job_entrydate"]));

                                    jcode = ((int)row["job_id"]).ToString();
                                    fcode = ((int)row["job_id"]).ToString("D8");
                                    fcode = "X" + fcode.Trim();
                                    datarow.ElementAt(idx)["New"] = cono;
                                    csRec = ((List<CSJOBM>)csRecs).Where(item => item.JOBNO == fcode).FirstOrDefault();
                                    if (csRec != null) { datarow.ElementAt(idx)["SYSGENCOL"] = "Exist"; }
                                    else
                                    {
                                        CSJOBM cSJOBM = new CSJOBM();
                                        CSJOBSTF cSJOBSTF = new CSJOBSTF();
                                        try
                                        {
                                            ModelState.Clear();
                                            cSJOBM.CONO = cono;
                                            cSJOBM.JOBSTAFF = staffcode;
                                            cSJOBM.CASECNT = 0;
                                            cSJOBM.OKCNT = 0;
                                            cSJOBM.STAMP = 993;
                                            cSJOBM.JOBNO = fcode;
                                            cSJOBM.VDATE = sdate;
                                            cSJOBM.REM = (string)row["job_remark"];
                                            cSJOBM.JOBPOST = (string)row["job_isposted"] == "t" ? "Y" : "N";
                                            cSJOBM.COMPLETE = "N";
                                            cSJOBM.COMPLETED = DateTime.Parse("01/01/3000");
                                            db.CSJOBMs.Add(cSJOBM);
                                            db.SaveChanges(); // must save master record first;

                                            cSJOBSTF.JOBNO = fcode;
                                            cSJOBSTF.STAFFCODE = mapcode;
                                            cSJOBSTF.STAMP = 993;
                                            cSJOBSTF.ROWNO = 1;
                                            cSJOBSTF.SDATE = sdate;
                                            // cSJOBSTF.EDATE = determine when processing histrec


                                            CSJOBD cSJOBD = null;
                                            CSJOBST cSJOBST = null;

                                            if (ModelState.IsValid)
                                            {
                                                var detailrecs = detailRecs.Where(x => x.Field<string>("job_id") == jcode).OrderBy(y => y.Field<string>("case_no"));

                                                if (detailrecs != null)
                                                {
                                                    cSJOBM.CASECNT = detailrecs.Count();

                                                    foreach (var detailrec in detailrecs)
                                                    {
                                                        caseid = ((int)detailrec["case_id"]).ToString();
                                                        string jobid = (string)detailrec["job_id"];
                                                        cSJOBD = new CSJOBD();
                                                        cSJOBD.JOBNO = fcode;
                                                        cSJOBD.CASECODE = (string)detailrec["case_case"];
                                                        cSJOBD.CASENO = Int32.Parse((string)detailrec["case_no"]);
                                                        cSJOBD.CASEMEMO = (string)detailrec["case_particular"];
                                                        cSJOBD.CASEREM = (string)detailrec["case_remark"];
                                                        cSJOBD.COMPLETED = DateTime.Parse("01/01/3000");
                                                        cSJOBD.STAMP = 993;
                                                        cSJOBD.COMPLETE = "N";
                                                        cSJOBD.STAGE = "";
                                                        cSJOBD.STAGETIME = "";

                                                        if (cSJOBD.CASECODE == "") { cSJOBD.CASECODE = "Misc"; } // blank casecode default to misc
                                                        db.CSJOBDs.Add(cSJOBD);
                                                        db.SaveChanges(); // must save detail record first before csjobst;


                                                        var histrecs = historyRecs.Where(x => x.Field<string>("case_id") == caseid).OrderBy(y => y.Field<int>("history_id"));
                                                        if (histrecs != null)
                                                        {
                                                            foreach (var histrec in histrecs)
                                                            {
                                                                cSJOBST = new CSJOBST();
                                                                setfdate = true;
                                                                fdate = DateTime.Parse((string)histrec["history_date"]);
                                                                cSJOBD.STAGEDATE = DateTime.Parse((string)histrec["history_date"]);
                                                                cSJOBD.STAGETIME = (string)histrec["history_time"];
                                                                cSJOBD.COMPLETE = (string)histrec["history_status"] == "Complete" ? "Y" : "N";
                                                                if (cSJOBD.COMPLETE == "Y")
                                                                {
                                                                    cSJOBD.COMPLETED = DateTime.Parse((string)histrec["history_date"]);
                                                                    cSJOBM.OKCNT = cSJOBM.OKCNT + 1;
                                                                }

                                                                var lastrec = db.CSJOBSTs.Where(x => x.JOBNO == fcode && x.CASENO == cSJOBD.CASENO).OrderByDescending(y => y.INDATE).ThenByDescending(z => z.INTIME).FirstOrDefault();
                                                                string prevStatus = "Pending";
                                                                if (lastrec != null)
                                                                {
                                                                    prevStatus = lastrec.STAGETO;
                                                                    lastrec.OUTDATE = fdate.AddDays(-1);
                                                                    db.Entry(lastrec).State = EntityState.Modified;
                                                                }
                                                                cSJOBST.JOBNO = fcode;
                                                                cSJOBST.CASENO = cSJOBD.CASENO;
                                                                cSJOBST.OUTDATE = DateTime.Parse("01/01/3000");
                                                                cSJOBST.STAGEFR = prevStatus;
                                                                cSJOBST.STAGETO = (string)histrec["history_status"];
                                                                cSJOBST.STAMP = 993;
                                                                cSJOBST.REM = (string)histrec["history_remark"];
                                                                cSJOBST.SENDMODE = (string)histrec["history_sendmode"];
                                                                cSJOBST.INDATE = fdate;
                                                                cSJOBST.INTIME = (string)histrec["history_time"];
                                                                cSJOBST.INIDX = fdate.ToString("yyyy/MM/dd") + cSJOBST.INTIME;
                                                                db.CSJOBSTs.Add(cSJOBST);
                                                                try
                                                                { db.SaveChanges(); }
                                                                catch (Exception e)
                                                                {
                                                                    datarow.ElementAt(idx)["SYSGENCOL"] = datarow.ElementAt(idx)["SYSGENCOL"] + e.Message + "!!!";
                                                                    db.CSJOBSTs.Remove(cSJOBST);
                                                                }

                                                            };
                                                        };
                                                        db.Entry(cSJOBD).State = EntityState.Modified;
                                                        try
                                                        { db.SaveChanges(); }
                                                        catch (Exception e)
                                                        {
                                                            datarow.ElementAt(idx)["SYSGENCOL"] = datarow.ElementAt(idx)["SYSGENCOL"] + e.Message + "!!!";
                                                            db.CSJOBDs.Remove(cSJOBD);
                                                        }
                                                    }

                                                }

                                                if ((cSJOBM.CASECNT == cSJOBM.OKCNT) && (cSJOBM.CASECNT > 0))
                                                {
                                                    cSJOBM.COMPLETE = "Y";
                                                    if (setfdate)
                                                    {
                                                        cSJOBM.COMPLETED = fdate;
                                                    }
                                                }

                                                db.Entry(cSJOBM).State = EntityState.Modified;
                                                db.CSJOBSTFs.Add(cSJOBSTF);



                                                db.SaveChanges();


                                            }
                                        }
                                        catch (Exception e)
                                        {
                                            datarow.ElementAt(idx)["SYSGENCOL"] = datarow.ElementAt(idx)["SYSGENCOL"] + e.Message + "!!!";
                                            db.CSJOBMs.Remove(cSJOBM);
                                            db.CSJOBSTFs.Remove(cSJOBSTF);
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
                    }

                    datarow = datarow.Where(y => y.Field<string>("SYSGENCOL") != "Exist").OrderBy(x => x.Field<string>("New"));

                }
                #endregion
                #region CSBILL
                else if (dbTableName == "CSBILL")
                {
                    datarow = datarow.OrderBy(x => x.Field<int?>("billingitem_id")).Select(x => x);

                    refTable = OpenTable(usefile);
                    var csRefs = refTable.AsEnumerable();

                    DataTable refTable1 = OpenTable(usefile1);
                    var jobRefs = refTable1.AsEnumerable();

                    DataTable refTable2 = OpenTable(usefile2);
                    var caseRefs = refTable2.AsEnumerable();



                    int idx = 0;
                    int coid = 0;
                    int staffno = -999;
                    int mapid = -999;
                    int jobcaseid = -1;
                    int jobid = -1;
                    int caseno = -1;

                    string cono = "";
                    string jobcode = "";
                    string mapcode = "";
                    string fcode = "";
                    string jcode = "";
                    string jcode1 = "";

                    string itemAmt = "";
                    string itemTax = "";
                    string itemTotal = "";
                    string taxRate = "";

                    CSBILL csRec = null;
                    DateTime sdate;
                    DateTime fdate = DateTime.Parse("01/01/3000");
                    bool setfdate = false;
                    string caseid = "";

                    foreach (DataRow row in datarow)
                    {
                        // skip certain record
                        if (true)
                        {
                            try
                            {
                                setfdate = false;
                                templine = (string)row["company_id"];
                                coid = int.Parse(templine);
                                cono = null;
                                jobcaseid = int.Parse((string)row["billingitem_caseid"]);


                                var corec = csRefs.Where(x => x.Field<int?>("company_id") == coid).FirstOrDefault();
                                if (corec != null)
                                {
                                    cono = (string)corec["company_no"];
                                }
                                else cono = null;

                                jobcode = "";
                                jcode1 = "";
                                var caserec = caseRefs.Where(x => x.Field<int?>("case_id") == jobcaseid).FirstOrDefault();
                                if (caserec != null)
                                {
                                    jobcode = (string)caserec["job_id"];
                                    jobid = Int32.Parse(jobcode);
                                    jcode1 = jobid.ToString("D8");
                                    jcode1 = "X" + jcode1.Trim();
                                    caseno = Int32.Parse((string)caserec["case_no"]);
                                }
                                else jobcode = null;



                                if (cono != null)
                                {
                                    datarow.ElementAt(idx)["New"] = cono;

                                    sdate = DateTime.Parse((string)(datarow.ElementAt(idx)["billingitem_entrydate"]));

                                    jcode = ((int)row["billingitem_id"]).ToString();
                                    fcode = ((int)row["billingitem_id"]).ToString("D8");
                                    fcode = "B" + fcode.Trim();
                                    datarow.ElementAt(idx)["New"] = cono;
                                    csRec = ((List<CSBILL>)csRecs).Where(item => item.BILLNO == fcode).FirstOrDefault();
                                    if (csRec != null) { setRemark(datarow, idx, "SYSGENCOL", "Exist"); }
                                    else
                                    {
                                        CSBILL cSBILL = new CSBILL();

                                        try
                                        {
                                            ModelState.Clear();
                                            cSBILL.CONO = cono;
                                            cSBILL.JOBNO = jcode1;
                                            cSBILL.STAMP = 993;
                                            cSBILL.BILLNO = fcode;
                                            cSBILL.ENTDATE = sdate;
                                            cSBILL.CASENO = caseno;

                                            var casecodeOrig = (string)row["billingitem_casefeecode"];
                                            if (casecodeOrig == "S78 Non Cash") { casecodeOrig = "S78NonCash"; }
                                            if (casecodeOrig == "CPO/Idaman") { casecodeOrig = "CPO"; }
                                            if (casecodeOrig == "Incorporation of Company") { casecodeOrig = "S15"; }
                                            if (casecodeOrig == "Winding Up") { casecodeOrig = "WP"; }
                                            if ((casecodeOrig.Length >= 2) && (casecodeOrig.Substring(0, 2) == "FS")) { casecodeOrig = "AC"; }
                                            if ((casecodeOrig.Length >= 9) && (casecodeOrig.Substring(0, 9) == "Insurance")) { casecodeOrig = "DCR"; }
                                            if (casecodeOrig == "Adoption of Constitution") { casecodeOrig = "S32"; }
                                            if ((casecodeOrig.Length >= 9) && (casecodeOrig.Substring(0, 9) == "Financial")) { casecodeOrig = "AC"; }
                                            if ((casecodeOrig.Length >= 8) && (casecodeOrig.Substring(0, 8) == "inancial")) { casecodeOrig = "AC"; }
                                            if (casecodeOrig == "Secretarial Fee + Registered Office") { casecodeOrig = "SecFee"; }
                                            if (casecodeOrig == "Sec Fee") { casecodeOrig = "SecFee"; }
                                            //if (casecodeOrig == "Handling, Delivery & Telecommunication") { casecodeOrig = "SecFee"; }

                                            var casecode = "";
                                            if ((casecodeOrig != "") && (casecodeOrig.Length <= 10))
                                            {
                                                casecode = db.CSCASEs.Where(x => x.CASECODE == casecodeOrig).Select(y => y.CASECODE).FirstOrDefault();
                                            }

                                            if ((casecode == null) || (casecode == ""))
                                            {
                                                setRemark(datarow, idx, "SYSGENCOL", casecodeOrig);
                                            }

                                            var feetype = (string)row["billingitem_feetype"];
                                            if (feetype == "Secretarial Work") { feetype = "Work"; }

                                            cSBILL.PRFALLOC = "N";
                                            cSBILL.PRFNO = "";
                                            cSBILL.SYSGEN = (string)row["billingitem_generated"] == "t" ? "Y" : "N";
                                            cSBILL.CASECODE = casecode;
                                            cSBILL.ITEMTYPE = feetype;
                                            cSBILL.ITEMDESC = (string)row["billingitem_billingdesc"];
                                            if (cSBILL.ITEMDESC.Length > 60) { cSBILL.ITEMDESC = cSBILL.ITEMDESC.Substring(0, 60); }
                                            cSBILL.ITEMSPEC = (string)row["billingitem_billingdetails"];

                                            taxRate = (string)row["billingitem_tax"];
                                            itemAmt = (string)row["billingitem_amount"];
                                            itemTax = (string)row["billingitem_calculatedtax"];
                                            itemTotal = (string)row["billingitem_total"];

                                            cSBILL.TAXRATE = decimal.Parse(taxRate);

                                            if (feetype == "Reimbursement")
                                            {
                                                cSBILL.ITEMAMT2 = decimal.Parse(itemAmt);
                                                cSBILL.TAXAMT2 = decimal.Parse(itemTax);
                                                cSBILL.NETAMT2 = decimal.Parse(itemTotal);

                                                cSBILL.ITEMAMT1 = 0;
                                                cSBILL.TAXAMT1 = 0;
                                                cSBILL.NETAMT1 = 0;
                                            }
                                            else
                                            {
                                                cSBILL.ITEMAMT1 = decimal.Parse(itemAmt);
                                                cSBILL.TAXAMT1 = decimal.Parse(itemTax);
                                                cSBILL.NETAMT1 = decimal.Parse(itemTotal);

                                                cSBILL.ITEMAMT2 = 0;
                                                cSBILL.TAXAMT2 = 0;
                                                cSBILL.NETAMT2 = 0;
                                            }

                                            cSBILL.ITEMAMT = decimal.Parse(itemAmt);
                                            cSBILL.TAXAMT = decimal.Parse(itemTax);
                                            cSBILL.NETAMT = decimal.Parse(itemTotal);


                                            CSPRF cSPRF = null;

                                            if (ModelState.IsValid)
                                            {




                                                //db.Entry(cSBILL).State = EntityState.Modified;
                                                //db.CSPRFs.Add(cSPRF);


                                                db.CSBILLs.Add(cSBILL);
                                                db.SaveChanges();


                                            }
                                        }
                                        catch (Exception e)
                                        {
                                            //datarow.ElementAt(idx)["SYSGENCOL"] = datarow.ElementAt(idx)["SYSGENCOL"] +
                                            setRemark(datarow, idx, "SYSGENCOL", e.Message + "!!!");
                                            db.CSBILLs.Remove(cSBILL);
                                            //db.CSPRFs.Remove(cSPRF);
                                        }
                                        finally
                                        {; };

                                    };
                                }
                                else
                                {
                                    setRemark(datarow, idx, "SYSGENCOL", templine + " No company");
                                }

                            }

                            catch (Exception e)
                            {
                                //datarow.ElementAt(idx)["New"] = e.Message;
                                setRemark(datarow, idx, "New", e.Message);
                            }
                            finally
                            {

                                idx++;
                            }
                        }
                    }

                    //datarow = datarow.Where(y => (y.Field<string>("SYSGENCOL") ?? string.Empty) != string.Empty).OrderBy(x => x.Field<string>("New"));
                    datarow = datarow.Where(y => y.Field<string>("SYSGENCOL") != "Exist").OrderBy(x => x.Field<string>("New"));

                }

                #endregion
                #region CSPRF

                else if (dbTableName == "CSPRF")
                {
                    datarow = datarow.OrderBy(x => x.Field<int?>("perfomabill_id")).Select(x => x);

                    refTable = OpenTable(usefile);
                    var csRefs = refTable.AsEnumerable();

                    DataTable refTable1 = OpenTable(usefile1);
                    var staffRefs = refTable1.AsEnumerable();

                    DataTable refTable2 = OpenTable(usefile2);
                    var billRefs = refTable2.AsEnumerable();



                    int idx = 0;
                    int coid = 0;
                    int staffno = -999;
                    int mapid = -999;
                    int invno = -1;
                    int jobid = -1;
                    int caseno = -1;

                    string cono = "";
                    string jobcode = "";
                    string mapcode = "";
                    string fcode = "";
                    string jcode = "";
                    string jcode1 = "";

                    string itemAmt = "";
                    string itemTax = "";
                    string itemTotal = "";
                    string taxRate = "";

                    CSPRF csRec = null;
                    DateTime sdate;
                    DateTime fdate = DateTime.Parse("01/01/3000");
                    bool setfdate = false;
                    string caseid = "";

                    foreach (DataRow row in datarow)
                    {
                        // skip certain record
                        if (true)
                        {
                            try
                            {
                                setfdate = false;
                                templine = (string)row["company_id"];
                                coid = int.Parse(templine);
                                cono = null;

                                staffno = int.Parse((string)row["perfomabill_createdby"]);

                                invno = int.Parse((string)row["invoice_no"]);


                                var corec = csRefs.Where(x => x.Field<int?>("company_id") == coid).FirstOrDefault();
                                if (corec != null)
                                {
                                    cono = (string)corec["company_no"];
                                }
                                else cono = null;

                                if (cono != null)
                                {
                                    datarow.ElementAt(idx)["New"] = cono;

                                    sdate = DateTime.Parse((string)(datarow.ElementAt(idx)["perfomabill_entrydate"]));

                                    jcode = ((int)row["perfomabill_id"]).ToString();
                                    fcode = invno.ToString("D8");
                                    fcode = "P" + fcode.Trim();
                                    datarow.ElementAt(idx)["New"] = cono;
                                    csRec = ((List<CSPRF>)csRecs).Where(item => item.PRFNO == fcode).FirstOrDefault();
                                    if (csRec != null) { setRemark(datarow, idx, "SYSGENCOL", "Exist"); }
                                    else
                                    {
                                        CSPRF cSPRF = new CSPRF();

                                        try
                                        {


                                            ModelState.Clear();
                                            cSPRF.CONO = cono;
                                            cSPRF.PRFNO = fcode;
                                            cSPRF.STAMP = 993;
                                            cSPRF.VDATE = sdate;
                                            cSPRF.DUEDAYS = Int32.Parse((string)row["perfomabill_duration"]);
                                            cSPRF.DUEDATE = DateTime.Parse((string)row["perfomabill_duedate"]);
                                            cSPRF.ATTN = "The Board of Directors";
                                            cSPRF.REM = "";
                                            cSPRF.INVALLOC = "N";
                                            cSPRF.INVNO = "";

                                            //cSPRF.INVNO = "V" + invno.ToString("D8");
                                            //if (cSPRF.INVNO != string.Empty)
                                            //{
                                            //    cSPRF.INVALLOC = "Y";
                                            //    cSPRF.INVID = 1; // the postgres system only uses 1 proforma bill per invoice
                                            //}

                                            CSCOMSTR cSCOMSTR = db.CSCOMSTRs.Find(cono);

                                            cSPRF.SEQNO = cSCOMSTR.SEQNO; // will need to increment company in production
                                            cSCOMSTR.SEQNO = cSCOMSTR.SEQNO + 1;

                                            db.Entry(cSCOMSTR).State = EntityState.Modified;
                                            if (ModelState.IsValid)
                                            {
                                                var prfidx = 1;
                                                var csbills = billRefs.Where(x => x.Field<string>("perfomabill_id") == jcode);
                                                foreach (var cbill in csbills)
                                                {
                                                    var billno = (int)cbill["billingitem_id"];
                                                    var billid = "B" + billno.ToString("D8");
                                                    CSBILL cSBILL = db.CSBILLs.Find(billid);
                                                    cSBILL.PRFNO = cSPRF.PRFNO;
                                                    cSBILL.PRFID = prfidx;
                                                    cSBILL.PRFALLOC = "Y";

                                                    db.Entry(cSBILL).State = EntityState.Modified;

                                                    CSTRANM cSTRANM = new CSTRANM();
                                                    cSTRANM.SOURCE = "CSPRF";
                                                    cSTRANM.SOURCENO = cSPRF.PRFNO;
                                                    cSTRANM.SOURCEID = prfidx;
                                                    cSTRANM.CONO = cSBILL.CONO;
                                                    cSTRANM.JOBNO = cSBILL.JOBNO;
                                                    cSTRANM.CASENO = cSBILL.CASENO;
                                                    cSTRANM.CASECODE = cSBILL.CASECODE;
                                                    cSTRANM.DUEDATE = cSPRF.DUEDATE;
                                                    cSTRANM.ENTDATE = cSPRF.VDATE;
                                                    cSTRANM.TRTYPE = cSBILL.ITEMTYPE;
                                                    cSTRANM.TRDESC = cSBILL.ITEMDESC;

                                                    cSTRANM.TRITEM = cSBILL.ITEMAMT;
                                                    cSTRANM.TRITEM1 = cSBILL.ITEMAMT1;
                                                    cSTRANM.TRITEM2 = cSBILL.ITEMAMT2;
                                                    cSTRANM.TRTAX = cSBILL.TAXAMT;
                                                    cSTRANM.TRTAX1 = cSBILL.TAXAMT1;
                                                    cSTRANM.TRTAX2 = cSBILL.TAXAMT2;
                                                    cSTRANM.TRAMT = cSBILL.NETAMT;
                                                    cSTRANM.TRAMT1 = cSBILL.NETAMT1;
                                                    cSTRANM.TRAMT2 = cSBILL.NETAMT2;

                                                    cSTRANM.TRSIGN = "DB";

                                                    cSTRANM.TRSITEM = cSBILL.ITEMAMT;
                                                    cSTRANM.TRSITEM1 = cSBILL.ITEMAMT1;
                                                    cSTRANM.TRSITEM2 = cSBILL.ITEMAMT2;
                                                    cSTRANM.TRSTAX = cSBILL.TAXAMT;
                                                    cSTRANM.TRSTAX1 = cSBILL.TAXAMT1;
                                                    cSTRANM.TRSTAX2 = cSBILL.TAXAMT2;
                                                    cSTRANM.TRSAMT = cSBILL.NETAMT;
                                                    cSTRANM.TRSAMT1 = cSBILL.NETAMT1;
                                                    cSTRANM.TRSAMT2 = cSBILL.NETAMT2;

                                                    cSTRANM.TRITEMOS = cSBILL.ITEMAMT;
                                                    cSTRANM.TRITEMOS1 = cSBILL.ITEMAMT1;
                                                    cSTRANM.TRITEMOS2 = cSBILL.ITEMAMT2;
                                                    cSTRANM.TRTAXOS = cSBILL.TAXAMT;
                                                    cSTRANM.TRTAXOS1 = cSBILL.TAXAMT1;
                                                    cSTRANM.TRTAXOS2 = cSBILL.TAXAMT2;
                                                    cSTRANM.TROS = cSBILL.NETAMT;
                                                    cSTRANM.TROS1 = cSBILL.NETAMT1;
                                                    cSTRANM.TROS2 = cSBILL.NETAMT2;

                                                    cSTRANM.APPITEM = 0;
                                                    cSTRANM.APPITEM1 = 0;
                                                    cSTRANM.APPITEM2 = 0;
                                                    cSTRANM.APPTAX = 0;
                                                    cSTRANM.APPTAX1 = 0;
                                                    cSTRANM.APPTAX2 = 0;
                                                    cSTRANM.APPAMT = 0;
                                                    cSTRANM.APPAMT1 = 0;
                                                    cSTRANM.APPAMT2 = 0;

                                                    cSTRANM.COMPLETE = "N";
                                                    cSTRANM.COMPLETED = DateTime.Parse("01/01/3000");
                                                    cSTRANM.SEQNO = cSPRF.SEQNO;
                                                    cSTRANM.REFCNT = 0;
                                                    cSTRANM.STAMP = 993;

                                                    db.CSTRANMs.Add(cSTRANM);
                                                    prfidx++;
                                                }

                                                //CSINV cSINV = new CSINV();
                                                //cSINV.STAMP = 999;
                                                //cSINV.VDATE = cSPRF.VDATE;
                                                //cSINV.CONO = cono;
                                                //cSINV.REM = "";
                                                //cSINV.POST = "N";
                                                //cSINV.INVNO = cSPRF.INVNO;
                                                //db.CSINVs.Add(cSINV);






                                                db.CSPRFs.Add(cSPRF);
                                                db.SaveChanges();


                                            }
                                        }
                                        catch (Exception e)
                                        {
                                            //datarow.ElementAt(idx)["SYSGENCOL"] = datarow.ElementAt(idx)["SYSGENCOL"] +
                                            setRemark(datarow, idx, "SYSGENCOL", e.Message + "!!!");
                                            db.CSPRFs.Remove(cSPRF);
                                        }
                                        finally
                                        {; };

                                    };
                                }
                                else
                                {
                                    setRemark(datarow, idx, "SYSGENCOL", templine + " No company");
                                }

                            }

                            catch (Exception e)
                            {
                                //datarow.ElementAt(idx)["New"] = e.Message;
                                setRemark(datarow, idx, "New", e.Message);
                            }
                            finally
                            {

                                idx++;
                            }
                        }
                    }

                    //datarow = datarow.Where(y => (y.Field<string>("SYSGENCOL") ?? string.Empty) != string.Empty).OrderBy(x => x.Field<string>("New"));
                    datarow = datarow.Where(y => y.Field<string>("SYSGENCOL") != "Exist").OrderBy(x => x.Field<string>("New"));

                }
                #endregion
                #region CSRCP
                else if (dbTableName == "CSRCP")
                {
                    datarow = datarow.OrderBy(x => x.Field<int?>("payment_id")).Select(x => x);

                    refTable = OpenTable(usefile);
                    var csRefs = refTable.AsEnumerable();

                    DataTable refTable1 = OpenTable(usefile1);
                    var staffRefs = refTable1.AsEnumerable();

                    DataTable refTable2 = OpenTable(usefile2);
                    var billRefs = refTable2.AsEnumerable();

                    DataTable refTable3 = OpenTable(usefile3);
                    var payItemRefs = refTable3.AsEnumerable();

                    DataTable refTable4 = OpenTable(usefile4);
                    var jobCaseRefs = refTable4.AsEnumerable();

                    int idx = 0;
                    int coid = 0;
                    int staffno = -999;
                    int mapid = -999;
                    int rcpno = -1;
                    int jobid = -1;
                    int caseno = -1;

                    string cono = "";
                    string jobcode = "";
                    string mapcode = "";
                    string fcode = "";
                    string jcode = "";
                    string jcode1 = "";

                    string itemAmt = "";
                    string itemTax = "";
                    string itemTotal = "";
                    string taxRate = "";

                    CSRCP csRec = null;
                    DateTime sdate;
                    DateTime fdate = DateTime.Parse("01/01/3000");
                    bool setfdate = false;
                    string caseid = "";

                    foreach (DataRow row in datarow)
                    {
                        // skip certain record
                        string confirmed = (string)row["payment_confirmed"];
                        if (confirmed == "t")
                        {
                            try
                            {
                                setfdate = false;
                                templine = (string)row["company_id"];
                                coid = int.Parse(templine);
                                cono = null;

                                staffno = int.Parse((string)row["payment_createdby"]);

                                rcpno = int.Parse((string)row["payment_no"]);


                                var corec = csRefs.Where(x => x.Field<int?>("company_id") == coid).FirstOrDefault();
                                if (corec != null)
                                {
                                    cono = (string)corec["company_no"];
                                }
                                else cono = null;

                                if (cono != null)
                                {
                                    datarow.ElementAt(idx)["New"] = cono;

                                    sdate = DateTime.Parse((string)(datarow.ElementAt(idx)["payment_date"]));

                                    jcode = ((int)row["payment_id"]).ToString();
                                    fcode = rcpno.ToString("D8");
                                    fcode = "R" + fcode.Trim();
                                    datarow.ElementAt(idx)["New"] = cono;
                                    csRec = ((List<CSRCP>)csRecs).Where(item => item.TRNO == fcode).FirstOrDefault();
                                    if (csRec != null) { setRemark(datarow, idx, "SYSGENCOL", "Exist"); }
                                    else
                                    {
                                        CSRCP cSRCP = new CSRCP();

                                        try
                                        {


                                            ModelState.Clear();
                                            cSRCP.CONO = cono;
                                            cSRCP.TRNO = fcode;
                                            cSRCP.STAMP = 993;
                                            cSRCP.VDATE = sdate;
                                            cSRCP.RCAMT = decimal.Parse((string)row["payment_amount"]);
                                            cSRCP.RCMODE = (string)row["payment_method"];

                                            string rcmapcode = (string)row["mode_map"];
                                            if (rcmapcode != "")
                                            {
                                                if (rcmapcode == "RHB Bank Berhad") { rcmapcode = "RHB"; }
                                                else if (rcmapcode == "Affin Bank Berhad") { rcmapcode = "AFFIN"; }
                                                else if (rcmapcode == "Malayan Banking Berhad (Maybank)") { rcmapcode = "MBB"; }
                                                else rcmapcode = "Bank";

                                                datarow.ElementAt(idx)["mode_map"] = rcmapcode;
                                            }

                                            cSRCP.RCMAPCODE = rcmapcode;

                                            string issbank = (string)row["issue_bank"];
                                            if (issbank != "")
                                            {
                                                string[] firstword = issbank.Split(' ');
                                                string firstname = firstword[0];
                                                if (firstname == "Malayan") { firstname = "Maybank"; }
                                                else if (firstname == "AmBank") { firstname = "Arab-Malaysian"; }
                                                else if (firstname == "HSBC") { firstname = "The Hong"; }
                                                issbank = db.HKBANKs.Where(x => x.BANKDESC.StartsWith(firstname)).Select(y => y.BANKCODE).FirstOrDefault();
                                                datarow.ElementAt(idx)["issue_bank"] = issbank;
                                                cSRCP.ISSBANK = issbank;
                                            }
                                            else { cSRCP.ISSBANK = null; }


                                            if ((string)row["issue_date"] != "")
                                            {
                                                cSRCP.ISSDATE = DateTime.Parse((string)row["issue_date"]);
                                            }
                                            cSRCP.ISSLOC = (string)row["issue_location"];
                                            cSRCP.ISSREFNO = (string)row["issue_refno"];
                                            cSRCP.REM = (string)row["payment_remark"];
                                            cSRCP.COMAMT = decimal.Parse((string)row["payment_calculatedtax"]);
                                            cSRCP.NETAMT = decimal.Parse((string)row["payment_total"]);
                                            cSRCP.POST = "N";
                                            cSRCP.CFLAG = "N";
                                            cSRCP.CTRNO = "";
                                            cSRCP.CREM = "";
                                            cSRCP.CPOST = "";

                                            CSCOMSTR cSCOMSTR = db.CSCOMSTRs.Find(cono);

                                            cSRCP.SEQNO = cSCOMSTR.SEQNO; // will need to increment company in production
                                            cSCOMSTR.SEQNO = cSCOMSTR.SEQNO + 1;

                                            db.CSRCPs.Add(cSRCP);
                                            db.SaveChanges();

                                            if (ModelState.IsValid)
                                            {
                                                int prfidx = 1;

                                                var cspayitems = payItemRefs.Where(x => x.Field<string>("payment_id") == jcode);
                                                foreach (var citems in cspayitems)
                                                {
                                                    var pbillid = (string)citems["billingitem_id"];
                                                    var pbillno = -1;
                                                    try
                                                    {
                                                        pbillno = int.Parse(pbillid);
                                                    }
                                                    catch (Exception e)
                                                    { pbillno = 0; }
                                                    finally
                                                    { }

                                                    string payamtstr = (string)citems["payment_amount"];
                                                    if (payamtstr == "") { payamtstr = "0"; }

                                                    string taxamtstr = (string)citems["payment_tax"];
                                                    if (taxamtstr == "") { taxamtstr = "0"; }

                                                    string totalamtstr = (string)citems["payment_total"];
                                                    if (totalamtstr == "") { totalamtstr = "0"; }

                                                    decimal payamt = decimal.Parse(payamtstr);
                                                    decimal taxamt = decimal.Parse(taxamtstr);
                                                    decimal totalamt = decimal.Parse(totalamtstr);


                                                    var csbills = billRefs.Where(x => x.Field<int?>("billingitem_id") == pbillno);
                                                    foreach (var cbill in csbills)
                                                    {
                                                        var billno = (int)cbill["billingitem_id"];
                                                        var billid = "B" + billno.ToString("D8");
                                                        CSBILL cSBILL = db.CSBILLs.Find(billid);

                                                        caseid = (string)cbill["billingitem_caseid"];
                                                        caseno = int.Parse(caseid);

                                                        var caserec = jobCaseRefs.Where(x => x.Field<int>("case_id") == caseno).FirstOrDefault();
                                                        if (caserec != null)
                                                        {
                                                            jobid = int.Parse((string)caserec["job_id"]);
                                                            caseno = int.Parse((string)caserec["case_no"]);
                                                        }


                                                        CSTRANM cSTRANM = new CSTRANM();
                                                        cSTRANM.SOURCE = "CSRCP";
                                                        cSTRANM.SOURCENO = cSRCP.TRNO;
                                                        cSTRANM.SOURCEID = prfidx;
                                                        cSTRANM.CONO = cSBILL.CONO;
                                                        cSTRANM.JOBNO = cSBILL.JOBNO;
                                                        cSTRANM.CASENO = cSBILL.CASENO;
                                                        cSTRANM.CASECODE = cSBILL.CASECODE;
                                                        cSTRANM.ENTDATE = cSRCP.VDATE;
                                                        cSTRANM.TRTYPE = cSBILL.ITEMTYPE;
                                                        cSTRANM.TRDESC = cSBILL.ITEMDESC;

                                                        cSTRANM.TRITEM1 = 0;
                                                        cSTRANM.TRITEM2 = 0;
                                                        if (cSBILL.ITEMAMT1 != 0) { cSTRANM.TRITEM1 = payamt; }
                                                        else if (cSBILL.ITEMAMT2 != 0) { cSTRANM.TRITEM2 = payamt; }
                                                        cSTRANM.TRITEM = payamt;

                                                        cSTRANM.TRTAX1 = 0;
                                                        cSTRANM.TRTAX2 = 0;
                                                        if (cSBILL.TAXAMT1 != 0) { cSTRANM.TRTAX1 = taxamt; }
                                                        else if (cSBILL.TAXAMT2 != 0) { cSTRANM.TRTAX2 = taxamt; }
                                                        cSTRANM.TRTAX = taxamt;

                                                        cSTRANM.TRAMT1 = 0;
                                                        cSTRANM.TRAMT2 = 0;
                                                        if (cSBILL.NETAMT1 != 0) { cSTRANM.TRAMT1 = totalamt; }
                                                        else if (cSBILL.NETAMT2 != 0) { cSTRANM.TRAMT2 = totalamt; }
                                                        cSTRANM.TRAMT = totalamt;


                                                        cSTRANM.TRSIGN = "CR";

                                                        cSTRANM.TRSITEM1 = 0;
                                                        cSTRANM.TRSITEM2 = 0;
                                                        if (cSBILL.ITEMAMT1 != 0) { cSTRANM.TRSITEM1 = -payamt; }
                                                        else if (cSBILL.ITEMAMT2 != 0) { cSTRANM.TRSITEM2 = -payamt; }
                                                        cSTRANM.TRSITEM = -payamt;

                                                        cSTRANM.TRSTAX1 = 0;
                                                        cSTRANM.TRSTAX2 = 0;
                                                        if (cSBILL.TAXAMT1 != 0) { cSTRANM.TRSTAX1 = -taxamt; }
                                                        else if (cSBILL.TAXAMT2 != 0) { cSTRANM.TRSTAX2 = -taxamt; }
                                                        cSTRANM.TRSTAX = -taxamt;

                                                        cSTRANM.TRSAMT1 = 0;
                                                        cSTRANM.TRSAMT2 = 0;
                                                        if (cSBILL.NETAMT1 != 0) { cSTRANM.TRSAMT1 = -totalamt; }
                                                        else if (cSBILL.NETAMT2 != 0) { cSTRANM.TRSAMT2 = -totalamt; }
                                                        cSTRANM.TRSAMT = -totalamt;

                                                        cSTRANM.TRITEMOS1 = 0;
                                                        cSTRANM.TRITEMOS2 = 0;
                                                        cSTRANM.TRITEMOS = 0;
                                                        //if (cSBILL.ITEMAMT1 != 0) { cSTRANM.TRITEMOS1 = -payamt; }
                                                        //else if (cSBILL.ITEMAMT2 != 0) { cSTRANM.TRITEMOS2 = -payamt; }
                                                        //cSTRANM.TRITEMOS = -payamt;

                                                        cSTRANM.TRTAXOS1 = 0;
                                                        cSTRANM.TRTAXOS2 = 0;
                                                        cSTRANM.TRTAXOS = 0;
                                                        //if (cSBILL.TAXAMT1 != 0) { cSTRANM.TRTAXOS1 = -taxamt; }
                                                        //else if (cSBILL.TAXAMT2 != 0) { cSTRANM.TRTAXOS2 = -taxamt; }
                                                        //cSTRANM.TRTAXOS = -taxamt;

                                                        cSTRANM.TROS1 = 0;
                                                        cSTRANM.TROS2 = 0;
                                                        cSTRANM.TROS = 0;
                                                        //if (cSBILL.NETAMT1 != 0) { cSTRANM.TROS1 = -totalamt; }
                                                        //else if (cSBILL.NETAMT2 != 0) { cSTRANM.TROS2 = -totalamt; }
                                                        //cSTRANM.TROS = -totalamt;

                                                        cSTRANM.APPITEM = 0;
                                                        cSTRANM.APPITEM1 = 0;
                                                        cSTRANM.APPITEM2 = 0;
                                                        cSTRANM.APPTAX = 0;
                                                        cSTRANM.APPTAX1 = 0;
                                                        cSTRANM.APPTAX2 = 0;
                                                        cSTRANM.APPAMT = 0;
                                                        cSTRANM.APPAMT1 = 0;
                                                        cSTRANM.APPAMT2 = 0;

                                                        cSTRANM.APPTYPE = "CSPRF";
                                                        cSTRANM.APPNO = cSBILL.PRFNO;
                                                        cSTRANM.APPID = cSBILL.PRFID ?? 0;

                                                        cSTRANM.COMPLETE = "N";
                                                        cSTRANM.COMPLETED = DateTime.Parse("01/01/3000");
                                                        cSTRANM.SEQNO = cSRCP.SEQNO;
                                                        cSTRANM.REFCNT = 0;
                                                        cSTRANM.STAMP = 993;

                                                        CSTRAND cSTRAND = new CSTRAND();

                                                        cSTRAND.SOURCE = "CSRCP";
                                                        cSTRAND.SOURCENO = cSRCP.TRNO;
                                                        cSTRAND.SOURCEID = prfidx;
                                                        cSTRAND.DBTYPE = "CSPRF";
                                                        cSTRAND.DBNO = cSBILL.PRFNO;
                                                        cSTRAND.DBID = cSBILL.PRFID ?? 0;
                                                        cSTRAND.CRTYPE = "CSRCP";
                                                        cSTRAND.CRNO = cSRCP.TRNO;
                                                        cSTRAND.CRID = prfidx;
                                                        cSTRAND.APPDATE = cSRCP.VDATE;

                                                        cSTRAND.APPITEM1 = 0;
                                                        cSTRAND.APPITEM2 = 0;
                                                        if (cSBILL.ITEMAMT1 != 0) { cSTRAND.APPITEM1 = payamt; cSTRANM.APPITEM1 = payamt; }
                                                        else if (cSBILL.ITEMAMT2 != 0) { cSTRAND.APPITEM2 = payamt; cSTRANM.APPITEM2 = payamt; }
                                                        cSTRAND.APPITEM = payamt;
                                                        cSTRANM.APPITEM = payamt;

                                                        cSTRAND.APPTAX1 = 0;
                                                        cSTRAND.APPTAX2 = 0;
                                                        if (cSBILL.TAXAMT1 != 0) { cSTRAND.APPTAX1 = taxamt; cSTRANM.APPTAX1 = taxamt; }
                                                        else if (cSBILL.TAXAMT2 != 0) { cSTRAND.APPTAX2 = taxamt; cSTRANM.APPTAX2 = taxamt; }
                                                        cSTRAND.APPTAX = taxamt;
                                                        cSTRANM.APPTAX = taxamt;

                                                        cSTRAND.APPAMT1 = 0;
                                                        cSTRAND.APPAMT2 = 0;
                                                        if (cSBILL.NETAMT1 != 0) { cSTRAND.APPAMT1 = totalamt; cSTRANM.APPAMT1 = totalamt; }
                                                        else if (cSBILL.NETAMT2 != 0) { cSTRAND.APPAMT2 = totalamt; cSTRANM.APPAMT2 = totalamt; }
                                                        cSTRAND.APPAMT = totalamt;
                                                        cSTRANM.APPAMT = totalamt;

                                                        cSTRANM.COMPLETE = "Y";
                                                        cSTRANM.COMPLETED = cSRCP.VDATE;

                                                        cSTRAND.STAMP = 993;

                                                        try
                                                        {
                                                            db.CSTRANMs.Add(cSTRANM);
                                                            db.SaveChanges();
                                                        }
                                                        catch (Exception e)
                                                        {
                                                            db.CSTRANMs.Remove(cSTRANM);
                                                            setRemark(datarow, idx, "SYSGENCOL", e.Message);
                                                        }
                                                        finally { }


                                                        try
                                                        {
                                                            db.CSTRANDs.Add(cSTRAND);
                                                            db.SaveChanges();
                                                        }
                                                        catch (Exception e)
                                                        {
                                                            db.CSTRANDs.Remove(cSTRAND);
                                                            setRemark(datarow, idx, "SYSGENCOL", e.Message);
                                                        }
                                                        finally { }

                                                        // apply to CSTRANM for PRF

                                                        CSTRANM cSTRANUPDM = db.CSTRANMs.Find("CSPRF", cSBILL.PRFNO, cSBILL.PRFID);
                                                        if (cSTRANUPDM != null)
                                                        {

                                                            if (cSBILL.ITEMAMT1 != 0) { cSTRANUPDM.TRITEMOS1 = cSTRANUPDM.TRITEMOS1 - payamt; }
                                                            else if (cSBILL.ITEMAMT2 != 0) { cSTRANUPDM.TRITEMOS2 = cSTRANUPDM.TRITEMOS2 - payamt; }
                                                            cSTRANUPDM.TRITEMOS = cSTRANUPDM.TRITEMOS - payamt;


                                                            if (cSBILL.TAXAMT1 != 0) { cSTRANUPDM.TRTAXOS1 = cSTRANUPDM.TRTAXOS1 - taxamt; }
                                                            else if (cSBILL.TAXAMT2 != 0) { cSTRANUPDM.TRTAXOS2 = -cSTRANUPDM.TRTAXOS2; }
                                                            cSTRANUPDM.TRTAXOS = cSTRANUPDM.TRTAXOS - taxamt;


                                                            if (cSBILL.NETAMT1 != 0) { cSTRANUPDM.TROS1 = cSTRANUPDM.TROS1 - totalamt; }
                                                            else if (cSBILL.NETAMT2 != 0) { cSTRANUPDM.TROS2 = cSTRANUPDM.TROS2 - totalamt; }
                                                            cSTRANUPDM.TROS = cSTRANUPDM.TROS - totalamt;

                                                            if (cSTRANUPDM.TROS == 0)
                                                            {
                                                                cSTRANUPDM.COMPLETE = "Y";
                                                                cSTRANUPDM.COMPLETED = cSRCP.VDATE;
                                                            }

                                                            db.Entry(cSTRANUPDM).State = EntityState.Modified;
                                                        }


                                                        // apply to CSLDG

                                                        CSLDG cSLDG = new CSLDG();
                                                        if (prfidx == 1)
                                                        {


                                                            cSLDG.SOURCE = "CSRCP";
                                                            cSLDG.SOURCEID = 0;
                                                            cSLDG.SOURCENO = cSRCP.TRNO;
                                                            cSLDG.CONO = cSRCP.CONO;
                                                            cSLDG.JOBNO = cSTRANM.JOBNO;
                                                            cSLDG.CASENO = cSTRANM.CASENO;
                                                            cSLDG.CASECODE = cSTRANM.CASECODE;
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
                                                            cSLDG.ADVANCE = 0;
                                                            cSLDG.REIMB1 = 0;
                                                            cSLDG.REIMB2 = 0;
                                                            cSLDG.REIMBREC1 = 0;
                                                            cSLDG.REIMBREC2 = 0;
                                                            cSLDG.SEQNO = cSRCP.SEQNO;
                                                            cSLDG.STAMP = 993;

                                                            if (cSBILL.ITEMTYPE == "Deposit")
                                                            {
                                                                cSLDG.ADVANCE = cSRCP.NETAMT;
                                                            }
                                                            else
                                                            {
                                                                cSLDG.RECEIPT = cSRCP.NETAMT;
                                                            }

                                                            cSLDG.STAMP = 993;
                                                            db.CSLDGs.Add(cSLDG);

                                                            cSLDG = new CSLDG();
                                                        }



                                                        cSLDG.SOURCE = "CSRCP";
                                                        cSLDG.SOURCEID = prfidx;
                                                        cSLDG.SOURCENO = cSRCP.TRNO;
                                                        cSLDG.CONO = cSRCP.CONO;
                                                        cSLDG.JOBNO = cSTRANM.JOBNO;
                                                        cSLDG.CASENO = cSTRANM.CASENO;
                                                        cSLDG.CASECODE = cSTRANM.CASECODE;
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
                                                        cSLDG.ADVANCE = 0;
                                                        cSLDG.REIMB1 = 0;
                                                        cSLDG.REIMB2 = 0;
                                                        cSLDG.REIMBREC1 = 0;
                                                        cSLDG.REIMBREC2 = 0;
                                                        cSLDG.SEQNO = cSRCP.SEQNO;
                                                        cSLDG.STAMP = 993;

                                                        if (cSBILL.ITEMTYPE == "Fee")
                                                        {
                                                            if (cSBILL.ITEMAMT1 != 0)
                                                            {
                                                                cSLDG.FEE1 = cSTRANM.TRAMT1;
                                                                cSLDG.TAX1 = cSTRANM.TRTAX1;
                                                            }
                                                            else
                                                            {
                                                                cSLDG.FEE2 = cSTRANM.TRAMT2;
                                                                cSLDG.TAX2 = cSTRANM.TRTAX2;
                                                            }
                                                        }
                                                        else if (cSBILL.ITEMTYPE == "Work")
                                                        {
                                                            if (cSBILL.ITEMAMT1 != 0)
                                                            {
                                                                cSLDG.WORK1 = cSTRANM.TRAMT1;
                                                                cSLDG.TAX1 = cSTRANM.TRTAX1;
                                                            }
                                                            else
                                                            {
                                                                cSLDG.WORK2 = cSTRANM.TRAMT2;
                                                                cSLDG.TAX2 = cSTRANM.TRTAX2;
                                                            }
                                                        }
                                                        else if (cSBILL.ITEMTYPE == "Disbursement")
                                                        {
                                                            if (cSBILL.ITEMAMT1 != 0)
                                                            {
                                                                cSLDG.DISB1 = cSTRANM.TRAMT1;
                                                                cSLDG.TAX1 = cSTRANM.TRTAX1;
                                                            }
                                                            else
                                                            {
                                                                cSLDG.DISB2 = cSTRANM.TRAMT2;
                                                                cSLDG.TAX2 = cSTRANM.TRTAX2;
                                                            }
                                                        }
                                                        else if (cSBILL.ITEMTYPE == "Reimbursement")
                                                        {
                                                            if (cSBILL.ITEMAMT1 != 0)
                                                            {
                                                                cSLDG.REIMB1 = cSTRANM.TRAMT1;
                                                                cSLDG.TAX1 = cSTRANM.TRTAX1;
                                                            }
                                                            else
                                                            {
                                                                cSLDG.REIMB2 = cSTRANM.TRAMT2;
                                                                cSLDG.TAX2 = cSTRANM.TRTAX2;
                                                            }
                                                        }

                                                        cSLDG.STAMP = 993;
                                                        db.CSLDGs.Add(cSLDG);

                                                        prfidx++;
                                                        //db.Entry(cSBILL).State = EntityState.Modified;
                                                    }

                                                }






                                                db.Entry(cSCOMSTR).State = EntityState.Modified;



                                                //db.CSRCPs.Add(cSRCP);
                                                db.SaveChanges();


                                            }
                                        }
                                        catch (Exception e)
                                        {
                                            //datarow.ElementAt(idx)["SYSGENCOL"] = datarow.ElementAt(idx)["SYSGENCOL"] +
                                            setRemark(datarow, idx, "SYSGENCOL", e.Message + "!!!");
                                            db.CSRCPs.Remove(cSRCP);
                                        }
                                        finally
                                        {; };

                                    };
                                }
                                else
                                {
                                    setRemark(datarow, idx, "SYSGENCOL", templine + " No company");
                                }

                            }

                            catch (Exception e)
                            {
                                //datarow.ElementAt(idx)["New"] = e.Message;
                                setRemark(datarow, idx, "New", e.Message);
                            }
                            finally
                            {


                            }
                        }
                        idx++;
                    }

                    //datarow = datarow.Where(y => (y.Field<string>("SYSGENCOL") ?? string.Empty) != string.Empty).OrderBy(x => x.Field<string>("New"));
                    datarow = datarow.Where(y => y.Field<string>("SYSGENCOL") != "Exist").OrderBy(x => x.Field<string>("New"));

                }
                #endregion
                #region CSCNM
                else if (dbTableName == "CSCNM")
                {
                    datarow = datarow.OrderBy(x => x.Field<int?>("creditnote_id")).Select(x => x);

                    refTable = OpenTable(usefile);
                    var csRefs = refTable.AsEnumerable();

                    DataTable refTable1 = OpenTable(usefile1);
                    var staffRefs = refTable1.AsEnumerable();

                    DataTable refTable3 = OpenTable(usefile3);
                    var payItemRefs = refTable3.AsEnumerable();


                    int idx = 0;
                    int coid = 0;
                    int staffno = -999;
                    int mapid = -999;
                    int rcpno = -1;
                    int jobid = -1;
                    int caseno = -1;

                    string cono = "";
                    string jobcode = "";
                    string mapcode = "";
                    string fcode = "";
                    string jcode = "";
                    string jcode1 = "";

                    string itemAmt = "";
                    string itemTax = "";
                    string itemTotal = "";
                    string taxRate = "";

                    CSCNM csRec = null;
                    DateTime sdate;
                    DateTime fdate = DateTime.Parse("01/01/3000");
                    bool setfdate = false;
                    string caseid = "";

                    foreach (DataRow row in datarow)
                    {
                        // skip certain record
                        string confirmed = (string)row["creditnote_confirmed"];
                        if (confirmed == "t")
                        {
                            try
                            {
                                setfdate = false;
                                templine = (string)row["creditnote_companyid"];
                                coid = int.Parse(templine);
                                cono = null;

                                staffno = int.Parse((string)row["create_by"]);

                                fcode = (string)row["creditnote_no"];


                                var corec = csRefs.Where(x => x.Field<int?>("company_id") == coid).FirstOrDefault();
                                if (corec != null)
                                {
                                    cono = (string)corec["company_no"];
                                }
                                else cono = null;

                                if (cono != null)
                                {
                                    datarow.ElementAt(idx)["New"] = cono;

                                    sdate = DateTime.Parse((string)(datarow.ElementAt(idx)["creditnote_entrydate"]));

                                    jcode = ((int)row["creditnote_id"]).ToString();

                                    datarow.ElementAt(idx)["New"] = cono;
                                    csRec = ((List<CSCNM>)csRecs).Where(item => item.TRNO == fcode).FirstOrDefault();
                                    if (csRec != null) { setRemark(datarow, idx, "SYSGENCOL", "Exist"); }
                                    else
                                    {
                                        CSCNM cSCNM = new CSCNM();

                                        try
                                        {


                                            ModelState.Clear();
                                            cSCNM.CONO = cono;
                                            cSCNM.TRNO = fcode;
                                            cSCNM.STAMP = 993;
                                            cSCNM.VDATE = sdate;

                                            cSCNM.ATTN = "";
                                            cSCNM.POST = "N";
                                            cSCNM.REM = "";

                                            CSCOMSTR cSCOMSTR = db.CSCOMSTRs.Find(cono);

                                            cSCNM.SEQNO = cSCOMSTR.SEQNO; // will need to increment company in production
                                            cSCOMSTR.SEQNO = cSCOMSTR.SEQNO + 1;

                                            db.CSCNMs.Add(cSCNM);
                                            db.SaveChanges();

                                            if (ModelState.IsValid)
                                            {
                                                int prfidx = 1;
                                                var cspayitems = payItemRefs.Where(x => x.Field<string>("creditnote_id") == jcode);
                                                foreach (var citems in cspayitems)
                                                {
                                                    //var pbillid = ((int)citems["billingitem_id"]).ToString();
                                                    string casecode = (string)citems["billingitem_casefeecode"];
                                                    string feetype = (string)citems["billingitem_feetype"];
                                                    string itemdesc = (string)citems["billingitem_billingdesc"];
                                                    if (itemdesc.Length > 60) { itemdesc = itemdesc.Substring(0, 60); }
                                                    if (feetype == "Secretarial Work") { feetype = "Work"; }

                                                    CSCND cSCND = new CSCND();
                                                    cSCND.TRNO = cSCNM.TRNO;
                                                    cSCND.TRID = prfidx;
                                                    cSCND.CASECODE = casecode;
                                                    cSCND.ITEMTYPE = feetype;
                                                    cSCND.ITEMDESC = itemdesc;
                                                    cSCND.ITEMSPEC = (string)citems["billingitem_billingdetails"];
                                                    cSCND.TAXRATE = decimal.Parse((string)citems["billingitem_tax"]);

                                                   

                                                    string payamtstr = (string)citems["billingitem_amount"];
                                                    if (payamtstr == "") { payamtstr = "0"; }

                                                    string taxamtstr = (string)citems["billingitem_calculatedtax"];
                                                    if (taxamtstr == "") { taxamtstr = "0"; }

                                                    string totalamtstr = (string)citems["billingitem_total"];
                                                    if (totalamtstr == "") { totalamtstr = "0"; }

                                                    decimal payamt = decimal.Parse(payamtstr);
                                                    decimal taxamt = decimal.Parse(taxamtstr);
                                                    decimal totalamt = decimal.Parse(totalamtstr);


                                                    if (feetype == "Reimbursement")
                                                    {
                                                        cSCND.ITEMAMT2 = payamt;
                                                        cSCND.TAXAMT2 = taxamt;
                                                        cSCND.NETAMT2 = totalamt;

                                                        cSCND.ITEMAMT1 = 0;
                                                        cSCND.TAXAMT1 = 0;
                                                        cSCND.NETAMT1 = 0;
                                                    }
                                                    else
                                                    {
                                                        cSCND.ITEMAMT1 = payamt;
                                                        cSCND.TAXAMT1 = taxamt;
                                                        cSCND.NETAMT1 = totalamt;

                                                        cSCND.ITEMAMT2 = 0;
                                                        cSCND.TAXAMT2 = 0;
                                                        cSCND.NETAMT2 = 0;
                                                    }
                                                    cSCND.ITEMAMT = payamt;
                                                    cSCND.TAXAMT = taxamt;
                                                    cSCND.NETAMT = totalamt;
                                                    cSCND.STAMP = 993;


                                                    try
                                                    {
                                                        db.CSCNDs.Add(cSCND);
                                                        db.SaveChanges();
                                                    }
                                                    catch (Exception e)
                                                    {
                                                        db.CSCNDs.Remove(cSCND);
                                                        setRemark(datarow, idx, "SYSGENCOL", e.Message);
                                                    }
                                                    finally { }

                                                    CSTRANM cSTRANM = new CSTRANM();
                                                    cSTRANM.SOURCE = "CSCN";
                                                    cSTRANM.SOURCENO = cSCNM.TRNO;
                                                    cSTRANM.SOURCEID = prfidx;
                                                    cSTRANM.CONO = cono;
                                                    cSTRANM.JOBNO = null;
                                                    cSTRANM.CASENO = null;
                                                    cSTRANM.CASECODE = casecode;
                                                    cSTRANM.ENTDATE = cSCNM.VDATE;
                                                    cSTRANM.TRTYPE = cSCND.ITEMTYPE;
                                                    cSTRANM.TRDESC = cSCND.ITEMDESC;

                                                    cSTRANM.TRITEM1 = 0;
                                                    cSTRANM.TRITEM2 = 0;
                                                    if (cSCND.ITEMAMT1 != 0) { cSTRANM.TRITEM1 = payamt; }
                                                    else if (cSCND.ITEMAMT2 != 0) { cSTRANM.TRITEM2 = payamt; }
                                                    cSTRANM.TRITEM = payamt;

                                                    cSTRANM.TRTAX1 = 0;
                                                    cSTRANM.TRTAX2 = 0;
                                                    if (cSCND.TAXAMT1 != 0) { cSTRANM.TRTAX1 = taxamt; }
                                                    else if (cSCND.TAXAMT2 != 0) { cSTRANM.TRTAX2 = taxamt; }
                                                    cSTRANM.TRTAX = taxamt;

                                                    cSTRANM.TRAMT1 = 0;
                                                    cSTRANM.TRAMT2 = 0;
                                                    if (cSCND.NETAMT1 != 0) { cSTRANM.TRAMT1 = totalamt; }
                                                    else if (cSCND.NETAMT2 != 0) { cSTRANM.TRAMT2 = totalamt; }
                                                    cSTRANM.TRAMT = totalamt;


                                                    cSTRANM.TRSIGN = "CR";

                                                    cSTRANM.TRSITEM1 = 0;
                                                    cSTRANM.TRSITEM2 = 0;
                                                    if (cSCND.ITEMAMT1 != 0) { cSTRANM.TRSITEM1 = -payamt; }
                                                    else if (cSCND.ITEMAMT2 != 0) { cSTRANM.TRSITEM2 = -payamt; }
                                                    cSTRANM.TRSITEM = -payamt;

                                                    cSTRANM.TRSTAX1 = 0;
                                                    cSTRANM.TRSTAX2 = 0;
                                                    if (cSCND.TAXAMT1 != 0) { cSTRANM.TRSTAX1 = -taxamt; }
                                                    else if (cSCND.TAXAMT2 != 0) { cSTRANM.TRSTAX2 = -taxamt; }
                                                    cSTRANM.TRSTAX = -taxamt;

                                                    cSTRANM.TRSAMT1 = 0;
                                                    cSTRANM.TRSAMT2 = 0;
                                                    if (cSCND.NETAMT1 != 0) { cSTRANM.TRSAMT1 = -totalamt; }
                                                    else if (cSCND.NETAMT2 != 0) { cSTRANM.TRSAMT2 = -totalamt; }
                                                    cSTRANM.TRSAMT = -totalamt;

                                                    cSTRANM.TRITEMOS1 = 0;
                                                    cSTRANM.TRITEMOS2 = 0;
                                                    cSTRANM.TRITEMOS = 0;
                                                    //if (cSBILL.ITEMAMT1 != 0) { cSTRANM.TRITEMOS1 = -payamt; }
                                                    //else if (cSBILL.ITEMAMT2 != 0) { cSTRANM.TRITEMOS2 = -payamt; }
                                                    //cSTRANM.TRITEMOS = -payamt;

                                                    cSTRANM.TRTAXOS1 = 0;
                                                    cSTRANM.TRTAXOS2 = 0;
                                                    cSTRANM.TRTAXOS = 0;
                                                    //if (cSBILL.TAXAMT1 != 0) { cSTRANM.TRTAXOS1 = -taxamt; }
                                                    //else if (cSBILL.TAXAMT2 != 0) { cSTRANM.TRTAXOS2 = -taxamt; }
                                                    //cSTRANM.TRTAXOS = -taxamt;

                                                    cSTRANM.TROS1 = 0;
                                                    cSTRANM.TROS2 = 0;
                                                    cSTRANM.TROS = 0;
                                                    //if (cSBILL.NETAMT1 != 0) { cSTRANM.TROS1 = -totalamt; }
                                                    //else if (cSBILL.NETAMT2 != 0) { cSTRANM.TROS2 = -totalamt; }
                                                    //cSTRANM.TROS = -totalamt;

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
                                                    cSTRANM.SEQNO = cSCNM.SEQNO;
                                                    cSTRANM.REFCNT = 0;
                                                    cSTRANM.STAMP = 993;


                                                    try
                                                    {
                                                        db.CSTRANMs.Add(cSTRANM);
                                                        db.SaveChanges();
                                                    }
                                                    catch (Exception e)
                                                    {
                                                        db.CSTRANMs.Remove(cSTRANM);
                                                        setRemark(datarow, idx, "SYSGENCOL", e.Message);
                                                    }
                                                    finally { }

                                                    prfidx++;
                                                    //db.Entry(cSBILL).State = EntityState.Modified;


                                                }


                                                db.Entry(cSCOMSTR).State = EntityState.Modified;

                                                db.SaveChanges();


                                            }
                                        }
                                        catch (Exception e)
                                        {
                                            //datarow.ElementAt(idx)["SYSGENCOL"] = datarow.ElementAt(idx)["SYSGENCOL"] +
                                            setRemark(datarow, idx, "SYSGENCOL", e.Message + "!!!");
                                            db.CSCNMs.Remove(cSCNM);
                                        }
                                        finally
                                        {; };

                                    };
                                }
                                else
                                {
                                    setRemark(datarow, idx, "SYSGENCOL", templine + " No company");
                                }

                            }

                            catch (Exception e)
                            {
                                //datarow.ElementAt(idx)["New"] = e.Message;
                                setRemark(datarow, idx, "New", e.Message);
                            }
                            finally
                            {


                            }
                        }
                        idx++;
                    }

                    //datarow = datarow.Where(y => (y.Field<string>("SYSGENCOL") ?? string.Empty) != string.Empty).OrderBy(x => x.Field<string>("New"));
                    datarow = datarow.Where(y => y.Field<string>("SYSGENCOL") != "Exist").OrderBy(x => x.Field<string>("New"));

                }
                #endregion
                #region CSCOMSTR
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
                #endregion
                #region CSCOSTAT
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

                                        cSCOSTAT.STAMP = 995;
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
                #endregion
                #region CSCONAME
                else if (dbTableName == "CSCONAME")
                {
                    datarow = datarow.OrderBy(x => x.Field<string>("company_id")).ThenBy(n => n.Field<string>("sort_date")).Select(x => x);
                    refTable = OpenTable(usefile);
                    var csRefs = refTable.AsEnumerable();


                    int idx = 0;
                    int coid = 0;
                    string cono = "";
                    DateTime sdate;
                    CSCONAME csRec = null;
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

                                sdate = DateTime.Parse((string)(datarow.ElementAt(idx)["name_effectivedate"]));
                                csRec = ((List<CSCONAME>)csRecs).Where(item => item.CONO == cono && item.EFFDATE == sdate).FirstOrDefault();
                                if (csRec != null) { datarow.ElementAt(idx)["SYSGENCOL"] = "Exist"; }
                                else
                                {
                                    CSCONAME cSCONAME = new CSCONAME();
                                    try
                                    {
                                        ModelState.Clear();

                                        cSCONAME.STAMP = 995;
                                        cSCONAME.CONO = cono;
                                        cSCONAME.CONAME = (string) row["name_name"];
                                        cSCONAME.EFFDATE = sdate;
                     

                                        if (ModelState.IsValid)
                                        {
                                            int? lastRowNo = 0;
                                            try
                                            {
                                                lastRowNo = db.CSCONAMEs.Where(m => m.CONO == cSCONAME.CONO).Max(n => n.ROWNO);

                                                CSCONAME lastRec = db.CSCONAMEs.Where(m => m.CONO == cSCONAME.CONO && m.ROWNO == lastRowNo).FirstOrDefault();
                                                if (lastRec != null)
                                                {
                                                    lastRec.ENDDATE = sdate.AddDays(-1);
                                                    lastRec.STAMP = 995;
                                                    db.Entry(lastRec).State = EntityState.Modified;

                                                }
                                            }
                                            catch (Exception e) { lastRowNo = 0; }
                                            finally { };

                                            cSCONAME.ENDDATE = cSCONAME.EFFDATE.AddDays(30000);
                                            cSCONAME.ROWNO = (lastRowNo ?? 0) + 1;
                                            //datarow.ElementAt(idx)[6] = cSCONAME.ROWNO.ToString();
                                            setRemark(datarow, idx, "SYSGENCOL", cSCONAME.ROWNO.ToString());

                                            db.CSCONAMEs.Add(cSCONAME);
                                            db.SaveChanges();
                                        }
                                    }
                                    catch (Exception e)
                                    {
                                        //datarow.ElementAt(idx)[6] = e.Message + "!!!";
                                        setRemark(datarow, idx, "SYSGENCOL", e.Message + "!!!");
                                        db.CSCONAMEs.Remove(cSCONAME);
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
                    datarow = datarow.Where(y => y.Field<string>("SYSGENCOL") != "Exist").OrderBy(x => x.Field<string>("New"));

                }
                #endregion
                #region return view data
                DataTable ndt = datarow.CopyToDataTable<DataRow>();

                ViewBag.Title = postedFile.FileName;
                return View(ndt);
                #endregion
                #region experimenting code
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
                #endregion

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

        private string getPostcode(string addr)
        {
            // check for 5digit or 6digit postal code in state/city and insert to postal column
            string[] postcodes = addr.Split(' ');
            if (postcodes.Length > 1)
            {
                string postcode = postcodes[0];
                if ((postcode.Length == 5) || (postcode.Length == 6))
                {
                    try
                    {
                        int i = int.Parse(postcode);
                        return postcode; //if can convert to integer means it is probably postcode
                    }
                    catch { }
                    finally { };
                }

            }
            return "";
        }

        private string getNumbersOnly(string instr)
        {
            Regex rgx = new Regex("[^0-9]");
            string str = rgx.Replace(instr, "");
            return str;
        }

        private bool MustAddDate(string dbTableName)
        {
            string[] dbTableList = { "CSCOSTAT", "CSCOAR", "CSCOAGM", "CSJOBM", "CSBILL", "CSPRF", "CSRCP", "CSCNM","CSCONAME" };
            return Array.IndexOf(dbTableList, dbTableName) >= 0;
        }

        private void setRemark(EnumerableRowCollection<DataRow> datarow, int idx, string colName, string msg)
        {
            if ((DBNull.Value == datarow.ElementAt(idx)[colName]) || ((string)datarow.ElementAt(idx)[colName] == "")) { datarow.ElementAt(idx)[colName] = msg; }
            else
            { datarow.ElementAt(idx)[colName] = datarow.ElementAt(idx)[colName] + " | " + msg; }
        }
        private void setRemark1(DataTable datarow, int idx, string colName, string msg)
        {
            if ((DBNull.Value == datarow.Rows[idx-1][colName]) || ((string)datarow.Rows[idx-1][colName] == "")) { datarow.Rows[idx-1][colName] = msg; }
            else
            { datarow.Rows[idx-1][colName] = datarow.Rows[idx-1][colName] + " | " + msg; }
        }
    }


}