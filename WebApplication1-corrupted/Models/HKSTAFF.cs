using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1
{
    [MetadataType(typeof(HKSTAFFMetadata))]
    public partial class HKSTAFF
    {

    }

    public class HKSTAFFMetadata
    {
        [Display(Name = "Staff")]
        public string STAFFCODE { get; set; }

        [Display(Name = "Staff Name")]
        public string STAFFDESC { get; set; }

        [Display(Name = "Level")]
        public int STAFFLEVEL { get; set; }

        public string MSTRSTAFF { get; set; }
        public int REFCNT { get; set; }
        public int STAMP { get; set; }

    }
}