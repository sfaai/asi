using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1
{
    [MetadataType(typeof(HKLOGINMAPMetadata))]
    public partial class HKLOGINMAP
    {
    }

    public class HKLOGINMAPMetadata
    {
        [Display(Name = "User Id")]
        [MaxLength(10)]
        public string USERID { get; set; }

        [Display(Name = "Staff")]
        [MaxLength(5)]
        public string STAFFCODE { get; set; }

        [Display(Name = "Changed")]
        public int STAMP { get; set; }
    }
}