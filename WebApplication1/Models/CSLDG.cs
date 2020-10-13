using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using WebApplication1.Utility;

namespace WebApplication1
{
    [MetadataType(typeof(CSLDGMetadata))]
    public partial class CSLDG
    {
        [Display(Name = "Total", Order = 110)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal TotalItems { get {
                return this.FEE1 + this.FEE2 + this.WORK1 + this.WORK2 + this.TAX1 + this.TAX2 + this.DISB1 + this.DISB2
                    + (this.REIMB1 ?? 0) + (this.REIMB2 ?? 0);
            } }

        [Display(Name = "Fees", Order = 110)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal FEE
        {
            get
            {
                return this.FEE1 + this.FEE2;
            }
        }


        [Display(Name = "Work", Order = 110)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal WORK
        {
            get
            {
                return this.WORK1 + this.WORK2;
            }
        }


        [Display(Name = "Disbursement", Order = 110)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal DISB
        {
            get
            {
                return this.DISB1 + this.DISB2;
            }
        }


        [Display(Name = "Tax", Order = 110)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal TAX
        {
            get
            {
                return this.TAX1 + this.TAX2;
            }
        }


        [Display(Name = "Reimbursement", Order = 110)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal REIMB
        {
            get
            {
                return (this.REIMB1 ?? 0) + (this.REIMB2 ?? 0);
            }
        }
    }

    public class CSLDGMetadata
    {
        [Display(Name = "Type", Order = 110)]
        public string SOURCE { get; set; }

        [Display(Name = "Reference", Order = 110)]
        public string SOURCENO { get; set; }

        [Display(Name = "Id", Order = 110)]
        public int SOURCEID { get; set; }

        [Display(Name = "Company #", Order = 110)]
        public string CONO { get; set; }

        [Display(Name = "Job Id", Order = 110)]
        public string JOBNO { get; set; }

        [Display(Name = "Case #", Order = 110)]
        public Nullable<int> CASENO { get; set; }

        [Display(Name = "Case Code", Order = 110)]
        public string CASECODE { get; set; }

        [Display(Name = "Date", Order = 110)]
        [DataType(DataType.Date)]
        public System.DateTime ENTDATE { get; set; }

        [Display(Name = "Deposit", Order = 110)]
        public decimal DEP { get; set; }
        public decimal DEPREC { get; set; }

        [Display(Name = "Fees", Order = 110)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal FEE1 { get; set; }

        [Display(Name = "Fees", Order = 110)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal FEE2 { get; set; }

        public decimal FEEREC1 { get; set; }
        public decimal FEEREC2 { get; set; }

        [Display(Name = "Tax", Order = 110)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal TAX1 { get; set; }

        [Display(Name = "Tax", Order = 110)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal TAX2 { get; set; }


        public decimal TAXREC1 { get; set; }
        public decimal TAXREC2 { get; set; }

        [Display(Name = "Work", Order = 110)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal WORK1 { get; set; }

        [Display(Name = "Work", Order = 110)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal WORK2 { get; set; }

        public decimal WORKREC1 { get; set; }
        public decimal WORKREC2 { get; set; }

        [Display(Name = "Disbursement", Order = 110)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal DISB1 { get; set; }

        [Display(Name = "Disbursement", Order = 110)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal DISB2 { get; set; }
        public decimal DISBREC1 { get; set; }
        public decimal DISBREC2 { get; set; }

        [Display(Name = "Receipt", Order = 110)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal RECEIPT { get; set; }


        [Display(Name = "Advance", Order = 110)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal ADVANCE { get; set; }

        [Display(Name = "Own Reimbursement", Order = 110)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public Nullable<decimal> REIMB1 { get; set; }

        [Display(Name = "3P Reimbursement", Order = 110)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public Nullable<decimal> REIMB2 { get; set; }

        [Display(Name = "Seq", Order = 110)]
        public int SEQNO { get; set; }
    }
}