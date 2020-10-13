using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1
{
    [MetadataType(typeof(HKMAPMetadata))]
    public partial class HKMAP
    {
    }

    public class HKMAPMetadata
    {
        [Display(Name = "Mode Map")]
        public string MAPCODE { get; set; }

        [Display(Name = "Mode Map Name")]
        public string MAPDESC { get; set; }

    }
}