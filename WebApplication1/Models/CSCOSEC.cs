using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1
{
    [MetadataType(typeof(CSCOSECMetadata))]
    public partial class CSCOSEC
    {
    }

    public class CSCOSECMetadata
    {
        [Key]
        [Display(Name = "Company", Order = 12)]
        public string CONO { get; set; }

        [Display(Name = "Firm", Order = 12)]
        [MaxLength(10)]
        public string CONOSEC { get; set; }

        [Display(Name = "Secretary", Order = 12)]
        [MaxLength(10)]
        public string PRSCODE { get; set; }

        [Display(Name = "Appointed Date", Order = 1)]
        [DataType(DataType.Date)]
        public System.DateTime ADATE { get; set; }

        [Display(Name = "Resignation Date", Order = 1)]
        [DataType(DataType.Date)]
        public Nullable<System.DateTime> RDATE { get; set; }

        [Display(Name = "End Date", Order = 1)]
        [DataType(DataType.Date)]
        public System.DateTime ENDDATE { get; set; }

        [Display(Name = "Remarks", Order = 12)]
        [MaxLength(60)]
        public string REM { get; set; }

        [Display(Name = "Row", Order = 12)]
        public int ROWNO { get; set; }

        [Display(Name = "Changed", Order = 12)]
        public int STAMP { get; set; }
    }
}