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
    }

    public class CSCOFEEMetadata
    {
        [Display(Name = "Change Date")]
        [DataType(DataType.Date)]
        public System.DateTime LASTTOUCH { get; set; }

        [Display(Name = "Company")]
        public string CONO { get; set; }

        [Display(Name = "Fee")]
        public string FEECODE { get; set; }

        [Display(Name = "Amount")]
        [DataType(DataType.Currency)]
        public string FEEAMT { get; set; }

        [Display(Name = "Type")]
        public string FEETYPE { get; set; }

        [Display(Name = "Duration(Months)")]
        public int FEEMTH { get; set; }
    }
}