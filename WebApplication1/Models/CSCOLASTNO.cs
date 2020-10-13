using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1
{
    [MetadataType(typeof(CSCOLASTNOMetadata))]
    public partial class CSCOLASTNO
    {
        public bool AUTOGENBool { get { return this.AUTOGEN == "Y"; } set { this.AUTOGEN = value ? "Y" : "N"; } }

    }

    public class CSCOLASTNOMetadata
    {
        [Key]
        [Display(Name = "Company", Order = 12)]
        [MaxLength(10)]
        public string CONO { get; set; }

        [Display(Name = "Code", Order = 12)]
        [MaxLength(10)]
        public string LASTCODE { get; set; }

        [Display(Name = "Description", Order = 12)]
        [MaxLength(60)]
        public string LASTDESC { get; set; }

        [Display(Name = "Prefix", Order = 12)]
        [MaxLength(2)]
        public string LASTPFIX { get; set; }

        [Display(Name = "Last Number", Order = 12)]
        public int LASTNO { get; set; }

        [Display(Name = "Width", Order = 12)]
        public int LASTWD { get; set; }

        [Display(Name = "Autogen", Order = 12)]
        [MaxLength(1)]
        public string AUTOGEN { get; set; }
  

        [Display(Name = "Changed", Order = 12)]
        public int STAMP { get; set; }
    }
}