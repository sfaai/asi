using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1
{
    [MetadataType(typeof(CSTXMetadata))]
    public partial class CSTX
    {
    }

    public class CSTXMetadata
    {
        [Key]
        [Display(Name = "Tax Agents", Order = 12)]
        [MaxLength(10)]
        public string TXCODE { get; set; }

        [Display(Name = "Tax Agent Description", Order = 12)]
        [MaxLength(60)]
        public string TXDESC { get; set; }

        [Display(Name = "Changed", Order = 12)]
        public int STAMP { get; set; }
    }
}