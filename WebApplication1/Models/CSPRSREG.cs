using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1
{
    [MetadataType(typeof(CSPRSREGMetadata))]
    public partial class CSPRSREG
    {
    }

    public class CSPRSREGMetadata
    {
        [Key]
        [Display(Name = "Entity", Order = 12)]
        [MaxLength(10)]
        public string PRSCODE { get; set; }

        [Display(Name = "Country", Order = 12)]
        [MaxLength(5)]
        public string CTRYCODE { get; set; }

        [Display(Name = "Registration Type", Order = 12)]
        [MaxLength(20)]
        public string REGTYPE { get; set; }

        [Display(Name = "Registration #", Order = 12)]
        [MaxLength(20)]
        public string REGNO { get; set; }

        [Display(Name = "Remarks", Order = 12)]
        [MaxLength(60)]
        public string REM { get; set; }   

        [Display(Name = "Changed", Order = 12)]
        public int STAMP { get; set; }
    }
}