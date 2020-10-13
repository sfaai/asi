using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1
{
    [MetadataType(typeof(CSCOPICMetadata))]
    public partial class CSCOPIC
    {
    }

    public class CSCOPICMetadata
    {


        [Display(Name = "Company")]
        public string CONO { get; set; }

        [Display(Name = "Entity No")]
        public string PRSCODE { get; set; }


        [Display(Name = "Designation")]
        public string DESIG { get; set; }

        [Display(Name = "Remark")]
        public string REM { get; set; }

        [Display(Name = "Changed", Order = 12)]
        public int STAMP { get; set; }
    }
}