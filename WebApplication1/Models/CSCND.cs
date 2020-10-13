using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using WebApplication1.Utility;

namespace WebApplication1
{
    [MetadataType(typeof(CSCNDMetadata))]
    public partial class CSCND
    {
        private ASIDBConnection db = new ASIDBConnection();

        private CSTRANM _CSTRANM;
        private IQueryable<CSTRAND> _CSTRAND;

        [Display(Name = "Used", Order = 10)]
        public int refcnt
        {
            get {
                if ((this.TRNO != null) && (this.TRNO != string.Empty))
                {
                    if (this.TRNO.Length > 10) { return 0; }
                    _CSTRANM = db.CSTRANMs.Find("CSCN", this.TRNO, this.TRID);
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
                    if (this.TRNO.Length > 10) { return null; }
                    string host = "localhost:49829";

                    _CSTRAND = db.CSTRANDs.Where(x => x.CRTYPE == "CSCN" && x.CRNO == this.TRNO && x.CRID == this.TRID);
                    if (_CSTRAND != null)
                    {
                        //if (_CSTRAND.SOURCE == "CSRCP")
                        //{
                        //    string controller = "CSRCPs";
                        //    return new Uri("http://" + host + "/" + controller + "/Edit/" + MyHtmlHelpers.ConvertIdToByteStr(_CSTRAND.SOURCENO));
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
                    if (this.TRNO.Length > 10) { return ""; }
                    _CSTRAND = db.CSTRANDs.Where(x => x.CRTYPE == "CSCN" && x.CRNO == this.TRNO && x.CRID == this.TRID);
                    if (_CSTRAND != null) {
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

    public class CSCNDMetadata
    {
        [Display(Name = "Item #", Order = 10)]
        public string TRNO { get; set; }

        [Display(Name = "row", Order = 10)]
        public int TRID { get; set; }

        [Display(Name = "Case", Order = 60)]
        public string CASECODE { get; set; }

        [Display(Name = "Item Type", Order = 70)]
        public string ITEMTYPE { get; set; }

        [Display(Name = "Item Description", Order = 80)]
        [MaxLength(60)]
        public string ITEMDESC { get; set; }

        [Display(Name = "Item Specification", Order = 90)]
        [DataType(DataType.MultilineText)]
        [MaxLength(256)]
        public string ITEMSPEC { get; set; }

        [Display(Name = "Tax Rate %", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal TAXRATE { get; set; }

        [Display(Name = "Own Amount", Order = 110)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal ITEMAMT1 { get; set; }

        [Display(Name = "Own Tax", Order = 120)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal TAXAMT1 { get; set; }

        [Display(Name = "Own Net", Order = 130)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal NETAMT1 { get; set; }

        [Display(Name = "3rd Party Amount", Order = 210)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal ITEMAMT2 { get; set; }

        [Display(Name = "3rd Party Tax", Order = 220)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal TAXAMT2 { get; set; }

        [Display(Name = "3rd Party Net", Order = 230)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal NETAMT2 { get; set; }

        [Display(Name = "Total Amount", Order = 310)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal ITEMAMT { get; set; }

        [Display(Name = "Total Tax", Order = 320)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal TAXAMT { get; set; }

        [Display(Name = "Total Net", Order = 330)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal NETAMT { get; set; }

    }
}