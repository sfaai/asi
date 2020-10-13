using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1
{
    [MetadataType(typeof(CSCOAKMetadata))]
    public partial class CSCOAK
    {
    }

    public class CSCOAKMetadata
    {
        [Key]
        [Display(Name = "Company", Order = 12)]
        [MaxLength(10)]
        public string CONO { get; set; }

        [Display(Name = "Row", Order = 12)]
        public int ROWNO { get; set; }

        [Display(Name = "Effective Date", Order = 1)]
        [DataType(DataType.Date)]
        public System.DateTime EFFDATE { get; set; }

        [Display(Name = "Authorised Capital", Order = 12)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal AK { get; set; }

        [Display(Name = "Remarks", Order = 12)]
        [MaxLength(60)]
        public string REM { get; set; }


        [Display(Name = "End Date", Order = 1)]
        [DataType(DataType.Date)]
        public System.DateTime ENDDATE { get; set; }

        [Display(Name = "Changed", Order = 12)]
        public int STAMP { get; set; }
    }
}