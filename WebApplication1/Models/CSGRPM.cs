using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1
{
    [MetadataType(typeof(CSGRPMMetadata))]
    public partial class CSGRPM
    {
    }

    public class CSGRPMMetadata
    {
        [Key]
        [Display(Name = "Group Code", Order = 12)]
        [MaxLength(10)]
        public string GRPCODE { get; set; }

        [Display(Name = "Group Description", Order = 12)]
        [MaxLength(60)]
        public string GRPDESC { get; set; }

        [Display(Name = "Changed", Order = 12)]
        public int STAMP { get; set; }
    }
}