using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1
{
    [MetadataType(typeof(CSADTMetadata))]
    public partial class CSADT
    {
    }

    public class CSADTMetadata
    {
        [Key]
        [Display(Name = "Auditor", Order = 12)]
        [MaxLength(10)]
        public string ADTCODE { get; set; }

        [Display(Name = "Auditor Description", Order = 12)]
        [MaxLength(60)]
        public string ADTDESC { get; set; }

        [Display(Name = "Changed", Order = 12)]
        public int STAMP { get; set; }
    }
}