using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1
{
    [MetadataType(typeof(CSCASEMetadata))]
    public partial class CSCASE
    {

    }

    public class CSCASEMetadata
    {
        [Display(Name = "Case", Order = 10)]
        [MaxLength(10)]
        [Required]
        public string CASECODE { get; set; }

        [Display(Name = "Description", Order = 20)]
        [MaxLength(60)]
        [Required]
        public string CASEDESC { get; set; }

        [Display(Name = "GL Map Suffix", Order = 30)]
        [MaxLength(2)]
        public string MAPSFIX { get; set; }
    }
}