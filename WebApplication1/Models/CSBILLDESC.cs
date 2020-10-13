using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1
{
    [MetadataType(typeof(CSBILLDESCMetadata))]
    public partial class CSBILLDESC
    {
    }

    public class CSBILLDESCMetadata
    {
        [Key]
        [Display(Name = "Billing Description", Order = 12)]
        [MaxLength(60)]
        public string BILLDESC { get; set; }

        [Display(Name = "Billing Specification", Order = 12)]
        [MaxLength(256)]
        public string BILLSPEC { get; set; }

        [Display(Name = "Changed", Order = 12)]
        public int STAMP { get; set; }
    }
}