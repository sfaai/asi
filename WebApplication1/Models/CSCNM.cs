using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1
{
    [MetadataType(typeof(CSCNMMetadata))]
    public partial class CSCNM
    {
        DateTime _DUEDATE;

        [Display(Name = "End Date", Order = 20)]
        [DataType(DataType.Date)]
        public DateTime DUEDATE { get { return _DUEDATE; } set { _DUEDATE = value; } }

        public bool POSTBool { get { return (this.POST == "Y"); } set { this.POST = value ? "Y" : "N"; } }

        [Display(Name = "Archive", Order = 10)]
        public bool archived { get { return this.POST == "Y"; } }

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

        public string address {
            get {
                if (this.CSCOADDR == null) { return (""); }
                else
                {
                    return (this.CSCOADDR.ADDR1 + " " + this.CSCOADDR.ADDR2 + " " + this.CSCOADDR.ADDR3);
                }
            }
           
        }

        [Display(Name = "Net Amount", Order = 10)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal NETAMT
        {
            get
            {               
                return this.CSCNDs.Sum(x => x.NETAMT);
            }
        }

        [Display(Name = "Details Used", Order = 10)]
        public int refcnt
        {
            get
            {
                return this.CSCNDs.Sum(x => x.refcnt);
            }
        }


        [Display(Name = "Total Details", Order = 10)]
        public int detcnt
        {
            get
            {
                return this.CSCNDs.Count();
            }
        }
    }

    public class CSCNMMetadata
    {
        [Display(Name = "Ref", Order = 10)]
        [MaxLength(10)]
        public string TRNO { get; set; }

        [Display(Name = "Date", Order = 20)]
        [DataType(DataType.Date)]
        public DateTime VDATE { get; set; }

        [Display(Name = "Company", Order = 30)]
        [MaxLength(10)]
        public string CONO { get; set; }

        [Display(Name = "Address", Order = 40)]
        public short? COADDRID { get; set; }

        [Display(Name = "Attention")]
        public string ATTN { get; set; }

        [Display(Name = "Remark")]
        public string REM { get; set; }

        [Display(Name = "Posted")]
        public bool POST { get; set; }

        [Display(Name = "Posted")]
        public bool POSTBool { get; set; }

    }
    }