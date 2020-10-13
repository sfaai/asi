using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1
{
    [MetadataType(typeof(HKRCISSLOCMetadata))]
    public partial class HKRCISSLOC
    {
    }

    public class HKRCISSLOCMetadata
    {
        [Display(Name = "Location")]
        public string ISSLOC { get; set; }


    }
}