using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1
{
    [MetadataType(typeof(CSGRPDMetadata))]
    public partial class CSGRPD
    {
    }

    public class CSGRPDMetadata
    {
        [Key]
        [Display(Name = "Group Code", Order = 12)]
        [MaxLength(10)]
        public string GRPCODE { get; set; }

        [Display(Name = "Company #", Order = 12)]
        [MaxLength(10)]
        public string CONO { get; set; }

        [Display(Name = "Changed", Order = 12)]
        public int STAMP { get; set; }
    }
}