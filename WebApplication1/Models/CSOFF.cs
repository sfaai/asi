using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using WebApplication1.Utility;

namespace WebApplication1
{
    [MetadataType(typeof(CSOFFMetadata))]
    public partial class CSOFF
    {
        private ASIDBConnection db = new ASIDBConnection();

        private CSTRANM _CSTRANM;
        private IQueryable<CSTRAND> _CSTRAND;
        private DateTime _DUEDATE;

        public Boolean archived
        {
            get { return this.POST == "Y"; }
        }

        [Display(Name = "Debit Item", Order = 10)]
        public string DBKey
        {
            get { return this.DBTYPE + "|" + this.DBNO + "|" + this.DBID; }
            set
            {
                if (value == null)
                {
                    this.DBTYPE = "";
                    this.DBNO = "";
                    this.DBID = -1;
                }
                else
                {
                    var KeyPart = value.Split('|');
                    if (KeyPart.Length == 3)
                    {
                        this.DBTYPE = KeyPart[0];
                        this.DBNO = KeyPart[1];
                        this.DBID = int.Parse(KeyPart[2]);
                    }
                }
            }
        }

        [Display(Name = "Credit Item", Order = 10)]
        public string CRKey
        {
            get { return this.CRTYPE + "|" + this.CRNO + "|" + this.CRID; }
            set
            {
                if (value == null)
                {
                    this.CRTYPE = "";
                    this.CRNO = "";
                    this.CRID = -1;
                }
                else
                {
                    var KeyPart = value.Split('|');
                    if (KeyPart.Length == 3)
                    {
                        this.CRTYPE = KeyPart[0];
                        this.CRNO = KeyPart[1];
                        this.CRID = int.Parse(KeyPart[2]);
                    }
                }
            }
        }

        [Display(Name = "End Date", Order = 10)]
        [DataType(DataType.Date)]
        public DateTime DUEDATE //used as temporary column holder for search
        {
            get { return _DUEDATE; }
            set { _DUEDATE = value; }
        }

        [Display(Name = "Used", Order = 10)]
        public int refcnt
        {
            get
            {
                if ((this.TRNO != null) && (this.TRNO != string.Empty))
                {
                    _CSTRANM = db.CSTRANMs.Find(this.DBTYPE, this.DBNO, this.DBID);
                    if (_CSTRANM != null) { return _CSTRANM.REFCNT; }
                }
                return 0;
            }
        }

        [DataType(DataType.Url)]
        public Uri Link
        {
            get
            {
                if ((this.TRNO != null) && (this.TRNO != string.Empty))
                {
                    string host = "localhost:49829";

                    _CSTRAND = db.CSTRANDs.Where(x => x.DBTYPE == this.DBTYPE && x.DBNO == this.DBNO && x.DBID == this.DBID);
                    if (_CSTRAND != null)
                    {
                        //if (_CSTRAND.SOURCE == "CSRCP")
                        //{
                        //    string controller = "CSRCPs";
                        //    return new Uri("http://" + host + "/" + controller + "/Edit/" + MyHtmlHelpers.ConvertIdToByteStr(_CSTRAND.SOURCENO) );
                        //    //return new Uri("http://www.msn.com");
                        //}
                    }
                }
                return null;
            }
        }

        [Display(Name = "Link", Order = 10)]
        public string LinkRef
        {
            get
            {

                if ((this.TRNO != null) && (this.TRNO != string.Empty))
                {
                    _CSTRAND = db.CSTRANDs.Where(x => x.DBTYPE == this.DBTYPE && x.DBNO == this.DBNO && x.DBID == this.DBID);
                    if (_CSTRAND != null)
                    {
                        string line = "";
                        string separator = "";
                        foreach (CSTRAND item in _CSTRAND)
                        {
                            if (line.Length > 0) { separator = " , "; }
                            line += separator + item.SOURCE + "|" + item.SOURCENO + "|" + item.SOURCEID;
                        }
                        return line;
                    }
                }
                return "";
            }
        }


    }

    public class CSOFFMetadata
    {
        [Display(Name = "Item #", Order = 10)]
        public string TRNO { get; set; }

        [Display(Name = "Date", Order = 10)]
        [DataType(DataType.Date)]
        public DateTime VDATE { get; set; }

        [Display(Name = "Company")]
        [MaxLength(10)]
        public string CONO { get; set; }

        [Display(Name = "DB Type", Order = 70)]
        [MaxLength(10)]
        public string DBTYPE { get; set; }

        [Display(Name = "DB No", Order = 70)]
        [MaxLength(10)]
        public string DBNO { get; set; }

        [Display(Name = "DB Id", Order = 70)]
        public int DBID { get; set; }

        [Display(Name = "CR Type", Order = 70)]
        [MaxLength(10)]
        public string CRTYPE { get; set; }

        [Display(Name = "CR No", Order = 70)]
        [MaxLength(10)]
        public string CRNO { get; set; }

        [Display(Name = "CR Id", Order = 70)]
        public int CRID { get; set; }


        [Display(Name = "DB Own Item", Order = 110)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal APPDBITEM1 { get; set; }

        [Display(Name = "DB 3P Item", Order = 110)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal APPDBITEM2 { get; set; }

        [Display(Name = "DB Item Total", Order = 110)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal APPDBITEM { get; set; }

        [Display(Name = "DB Own Tax", Order = 110)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal APPDBTAX1 { get; set; }

        [Display(Name = "DB 3P Tax", Order = 110)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal APPDBTAX2 { get; set; }

        [Display(Name = "DB Total Tax", Order = 110)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal APPDBTAX { get; set; }

        [Display(Name = "DB Own Total", Order = 110)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal APPDBAMT1 { get; set; }

        [Display(Name = "DB 3P Total", Order = 110)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal APPDBAMT2 { get; set; }

        [Display(Name = "DB Total Amount", Order = 110)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal APPDBAMT { get; set; }


        [Display(Name = "CR Own Item", Order = 110)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal APPCRITEM1 { get; set; }

        [Display(Name = "CR 3P Item", Order = 110)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal APPCRITEM2 { get; set; }

        [Display(Name = "CRItem Total", Order = 110)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal APPCRITEM { get; set; }

        [Display(Name = "CR Own Tax", Order = 110)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal APPCRTAX1 { get; set; }

        [Display(Name = "CR 3P Tax", Order = 110)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal APPCRTAX2 { get; set; }

        [Display(Name = "CR Total Tax", Order = 110)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal APPCRTAX { get; set; }

        [Display(Name = "CR Own Total", Order = 110)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal APPCRAMT1 { get; set; }

        [Display(Name = "CR 3P Total", Order = 110)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal APPCRAMT2 { get; set; }

        [Display(Name = "CR Total Amount", Order = 110)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal APPCRAMT { get; set; }

        [Display(Name = "Remark")]
        public string REM { get; set; }
        public int SEQNO { get; set; }

        [Display(Name = "Posted")]
        public string POST { get; set; }

        public int STAMP { get; set; }



    }
}