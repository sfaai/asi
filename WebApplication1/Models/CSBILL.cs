using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1
{
    [MetadataType(typeof(CSBILLMetadata))]
    public partial class CSBILL
    {
        private ASIDBConnection db = new ASIDBConnection();

        private static IEnumerable<dynamic> _taxrate;   

        private CSTRANM _CSTRANM;

        DateTime _DUEDATE;

        [Display(Name = "End Date", Order = 20)]
        [DataType(DataType.Date)]
        public DateTime DUEDATE { get { return _DUEDATE; } set { _DUEDATE = value; } }

        [Display(Name = "Archive", Order = 10)]
        public bool archived { get { return this.PRFALLOC == "Y"; } }

        [Display(Name = "Used", Order = 10)]
        public int refcnt
        {
            get
            {
                if ((this.PRFNO != null) && (this.PRFNO != string.Empty))
                {
                    _CSTRANM = db.CSTRANMs.Find("CSPRF", this.PRFNO, this.PRFID);
                    if (_CSTRANM != null) { return _CSTRANM.REFCNT; }
                }
                return 0;
            }
        }

        public IEnumerable<dynamic> GetTaxRate()
        {
            if (_taxrate == null)
            {
                _taxrate = db.CSTAXTYPEs.ToList();
            }
            return _taxrate;
        }

        public CSBILL()
        {
            if (_taxrate == null)
            {
                _taxrate = db.CSTAXTYPEs.ToList();
            }
        }

        public string JOBCASE {
            get {
                string result = this.JOBNO ?? "";
                result += "-" + this.CASENO?.ToString();
                return (result == "-" ? null : result);
            }
            set {
                if ((this.JOBNO == null) &&( (value != null) || (value.Length > 0)))
                {
                    string[] words = value.Split('-');
                    if (words.Length > 0)
                    {
                        this.JOBNO = words[0];
                        if (words.Length > 1)
                        {
                            if (words[1] != "")
                            {
                                this.CASENO = int.Parse(words[1]);
                            }
                            else this.CASENO = null;
                        }
                        else { this.JOBNO = null; this.CASENO = null; }
                    }
                }
            }
        }
        public bool SYSGENBool { get { return (this.SYSGEN == "Y"); } set { this.SYSGEN = value ? "Y" : "N"; } }
        public bool PRFALLOCBool { get { return (this.PRFALLOC == "Y"); } set { this.PRFALLOC = value ? "Y" : "N"; } }

        [Display(Name = "Tax Type")]
        public string TAXTYPE { get {
                string retval = "";
                if ((this.TAXCODE == null) || (this.TAXCODE == String.Empty))
                {
                    retval = _taxrate.Where(x => x.EFFECTIVE_START <= this.ENTDATE && x.EFFECTIVE_END >= this.ENTDATE && x.TAXRATE == this.TAXRATE).Select(y => y.TAXTYPE).FirstOrDefault();
                } else
                {
                    retval = _taxrate.Where(x => x.TAXCODE == this.TAXCODE).Select(y => y.TAXTYPE).FirstOrDefault();
                }
                return retval;
            }
        }
        [Display(Name = "Tax Ref")]
        public string TAXRCODE { get {
                string retval = "";
                if ((this.TAXCODE == null) || (this.TAXCODE == String.Empty))
                {
                    retval = _taxrate.Where(x => x.EFFECTIVE_START <= this.ENTDATE && x.EFFECTIVE_END >= this.ENTDATE && x.TAXRATE == this.TAXRATE).Select(y => y.TAXRCODE).FirstOrDefault();
                }
                else
                {
                    retval = _taxrate.Where(x => x.TAXCODE == this.TAXCODE).Select(y => y.TAXRCODE).FirstOrDefault();
                }
                return retval;
            }
        }
    }

    public class CSBILLMetadata
    {
        [Display(Name = "Item #", Order = 10)]
        [MaxLength(10)]
        public string BILLNO { get; set; }

        [Display(Name = "Item Date", Order = 20)]
        [DataType(DataType.Date)]
        public DateTime ENTDATE { get; set; }

        [Display(Name = "Generated", Order = 25)]
        public string SYSGEN { get; set; }

        [Display(Name = "Generated", Order = 26)]
        public bool SYSGENBool { get; set; }

        [Display(Name = "Company", Order = 30)]
        [MaxLength(10)]
        public string CONO { get; set; }

        [Display(Name ="Job Details", Order = 35)]
        public string JOBCASE { get; set; }

        [Display(Name = "Job No", Order = 40)]
        public string JOBNO { get; set; }

        [Display(Name = "Case No", Order = 50)]
        public int CASENO { get; set; }

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

        [Display(Name = "Tax Code", Order = 95)]
        public string TAXCODE { get; set; }

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

        [Display(Name = "Allocated", Order = 500)]
        public bool PRFALLOCBool { get; set; }

        [Display(Name = "Allocated", Order = 501)]
        public bool PRFALLOC { get; set; }

        [Display(Name = "Proforma Invoice", Order = 510)]
        public string PRFNO { get; set; }

        [Display(Name = "Inv Item #", Order = 520)]
        public int PRFID { get; set; }
    }
}