using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using DCSoft.RTF;

namespace WebApplication1
{
    [MetadataType(typeof(CSCOPUKMetadata))]
    public partial class CSCOPUK
    {
        private List<SelectListItem> _consideration = new List<SelectListItem>();

        private void makelist()
        {

            _consideration.Add(new SelectListItem
            {
                Text = "Cash",
                Value = "Cash",
                Selected = true
            });

            _consideration.Add(new SelectListItem
            {
                Text = "Non-Cash",
                Value = "Non-Cash",
                Selected = false
            });

  
        }

        public List<SelectListItem> consideration
        {
            get
            {
                makelist();
                return this._consideration;
            }
        }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Consideration Detail")]
        public string NCDETStr
        {
            get
            {

                string result = "";
                if (NCDET != null)
                {
                    for (int i = 0; i < (NCDET.Length); i++)
                    {

                        result = result + (char)NCDET[i];
                    }
                }
                RTFDomDocument doc = new RTFDomDocument();
                doc.LoadRTFText(result);
                if (string.IsNullOrEmpty(doc.InnerText))
                {
                    return result;
                } 
                return doc.InnerText;
            }

            set
            {
                if (value != null)
                {
                    NCDET = new byte[value.Length];
                    for (int i = 0; i < value.Length; i++)
                    {
                       NCDET[i] = (byte)value[i];
                    }
                }
            }
        }
    }

    public class CSCOPUKMetadata
    {
        [Key]
        [Display(Name = "Company", Order = 12)]
        [MaxLength(10)]
        public string CONO { get; set; }

        [Display(Name = "Row", Order = 12)]
        public int ROWNO { get; set; }

        [Display(Name = "Movement", Order = 1)]
        [DataType(DataType.Date)]
        public System.DateTime EFFDATE { get; set; }


        [Display(Name = "Equity Code", Order = 12)]
        [MaxLength(10)]
        public string EQCODE { get; set; }

        [Display(Name = "Consideration", Order = 12)]
        [MaxLength(10)]
        public string EQCONSIDER { get; set; }

        [Display(Name = "No of Shares", Order = 12)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal NOOFSHARES { get; set; }

        [Display(Name = "Nominal Value", Order = 12)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal NOMINAL { get; set; }

        [Display(Name = "Paid Amt p/s", Order = 12)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal PAIDAMT { get; set; }

        [Display(Name = "Due/Payable Amt p/s", Order = 12)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal DUEAMT { get; set; }

        [Display(Name = "Premium Amt p/s", Order = 12)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal PREMIUM { get; set; }

        [Display(Name = "Consideration Detail", Order = 12)]
        public byte[] NCDET { get; set; }

        [Display(Name = "End Date", Order = 1)]
        [DataType(DataType.Date)]
        public System.DateTime ENDDATE { get; set; }

        [Display(Name = "Changed", Order = 12)]
        public int STAMP { get; set; }
    }
}



