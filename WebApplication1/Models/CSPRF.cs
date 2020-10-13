using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1
{
    [MetadataType(typeof(CSPRFMetadata))]
    public partial class CSPRF
    {

        private ASIDBConnection db = new ASIDBConnection();        
        private IQueryable<CSTRANM> _CSTRANM;

        public bool INVALLOCBool { get { return (this.INVALLOC == "Y"); } set { this.INVALLOC = value ? "Y" : "N"; } }

        [Display(Name = "Archive", Order = 10)]
        public bool archived { get { return this.INVALLOC == "Y"; } }

        [Display(Name = "Billing Address")]
        [DataType(DataType.MultilineText)]
        public string COADDR
        {
            get
            {
                string result = this.CONO ?? "";
                result += "|" + this.COADDRID?.ToString();
                return (result == "|" ? null : result);
            }
            set
            {
                if ((this.CONO == null) && ((value != null) || (value.Length > 0)))
                {
                    string[] words = value.Split('|');
                    if (words.Length > 0)
                    {
                        this.CONO = words[0];
                        if (words.Length > 1)
                        {
                            if (words[1] != "")
                            {
                                this.COADDRID = (short)int.Parse(words[1]);
                            }
                            else this.COADDRID = null;
                        }
                        else { this.CONO = null; this.COADDRID = null; }
                    }
                }
            }
        }

        public string address
        {

            get
            {
                if (this.CSCOADDR == null) { return (""); }
                else return (this.CSCOADDR.ADDR1 + " " + this.CSCOADDR.ADDR2 + " " + this.CSCOADDR.ADDR3); }

        }

        [Display(Name = "Net Amount", Order = 10)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal NETAMT
        {
            get
            {
                return this.CSBILLs.Sum(x => x.NETAMT);
            }
        }

        [Display(Name = "Bills Used", Order = 10)]
        public int refcnt
        {
            get
            {
                return this.CSBILLs.Sum(x => x.refcnt);
            }
        }


        [Display(Name = "Total Bills", Order = 10)]
        public int billcnt
        {
            get
            {
                return this.CSBILLs.Count();
            }
        }

        [Display(Name = "Allocated Amount", Order = 10)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal AllocAmt
        {
            get
            {
                //return 0;
                _CSTRANM = db.CSTRANMs.Where(x => x.SOURCE == "CSPRF" && x.SOURCENO == this.PRFNO);
                if (_CSTRANM.Count() > 0)
                {
                    return _CSTRANM.Sum(y => y.TRAMT) - _CSTRANM.Sum(y => y.TROS);
                }
                return 0;
            }
        }
    }

    public class CSPRFMetadata
    {


        [Display(Name = "Proforma Bill")]
        [MaxLength(10)]
        public string PRFNO { get; set; }

        [Display(Name ="Billing Date")]
        [DataType(DataType.Date)]
        public System.DateTime VDATE { get; set; }

        [Display(Name = "Company")]
        [MaxLength(10)]
        public string CONO { get; set; }

        [Display(Name = "Address", Order = 40)]
        public short? COADDRID { get; set; }

        [Display(Name = "Due Date")]
        [DataType(DataType.Date)]
        public System.DateTime DUEDATE { get; set; }

        [Display(Name = "Invoiced")]
        public bool INVALLOCBool { get; set; }

        [Display(Name = "Due Days")]
        public int DUEDAYS { get; set; }

        [Display(Name = "Attention")]
        public string ATTN { get; set; }

        [Display(Name = "Remark")]
        public string REM { get; set; }
    }
}