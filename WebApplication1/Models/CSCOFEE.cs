using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1
{
    [MetadataType(typeof(CSCOFEEMetadata))]
    public partial class CSCOFEE
    {
        bool _AutogenFlag = true;

        [Display(Name = "Generate")]
        public bool AutogenFlag { get { return _AutogenFlag; } set { _AutogenFlag = value; } }
    }

    public class CSCOFEEMetadata
    {
        [Display(Name = "Bill To")]
        [DataType(DataType.Date)]
        public System.DateTime LASTTOUCH { get; set; }

        [Display(Name = "Company")]
        public string CONO { get; set; }

        [Display(Name = "Fee")]
        public string FEECODE { get; set; }

        [Display(Name = "Amount")]
        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public string FEEAMT { get; set; }

        [Display(Name = "Type")]
        public string FEETYPE { get; set; }

        [Display(Name = "Duration(Months)")]
        public int FEEMTH { get; set; }

        [Display(Name = "Changed", Order = 12)]
        public int STAMP { get; set; }
    }
}