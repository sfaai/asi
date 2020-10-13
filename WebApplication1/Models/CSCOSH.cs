using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1
{
    [MetadataType(typeof(CSCOSHMetadata))]
    public partial class CSCOSH
    {
    }

    public class CSCOSHMetadata
    {
        [Key]
        [Display(Name = "Company", Order = 12)]
        public string CONO { get; set; }


        [Display(Name = "Shareholder", Order = 12)]
        [MaxLength(10)]
        public string PRSCODE { get; set; }

        [Display(Name = "Effective Date", Order = 1)]
        [DataType(DataType.Date)]
        public System.DateTime EFFDATE { get; set; }

        [Display(Name = "Termination Date", Order = 1)]
        [DataType(DataType.Date)]
        public Nullable<System.DateTime> TERDATE { get; set; }

        [Display(Name = "End Date", Order = 1)]
        [DataType(DataType.Date)]
        public System.DateTime ENDDATE { get; set; }

        [Display(Name = "Remarks", Order = 12)]
        [MaxLength(60)]
        public string REM { get; set; }

        [Display(Name = "Folio", Order = 12)]
        public int FOLIONO { get; set; }

        [Display(Name = "Changed", Order = 12)]
        public int STAMP { get; set; }
    }
}