using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1
{
    [MetadataType(typeof(HKRCMODEMetadata))]
    public partial class HKRCMODE
    {
        [Display(Name = "More Details")]
        public bool DETFLAGBool { get { return (this.DETFLAG == "Y"); } set { this.DETFLAG = value ? "Y" : "N"; } }

    }

    public class HKRCMODEMetadata
    {
        [Display(Name = "Mode")]
        public string RCMODE { get; set; }

        [Display(Name = "More Details")]
        public string DETFLAG { get; set; }
    }
}