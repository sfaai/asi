using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1
{
    [MetadataType(typeof(CSCHGREMMetadata))]
    public partial class CSCHGREM
    {
    }

    public class CSCHGREMMetadata
    {
        [Key]
        [Display(Name = "Change Remarks", Order = 12)]
        [MaxLength(100)]
        public string CHGREM { get; set; }



        [Display(Name = "Changed", Order = 12)]
        public int STAMP { get; set; }
    }
}