using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1
{
    [MetadataType(typeof(CSTAXTYPEMetadata))]
    public partial class CSTAXTYPE
    {
    }

    public class CSTAXTYPEMetadata
    {
        [Display(Name = "Start Date", Order = 20)]
        [DataType(DataType.Date)]
        public DateTime EFFECTIVE_START { get; set; }

        [Display(Name = "END Date", Order = 30)]
        [DataType(DataType.Date)]
        public DateTime EFFECTIVE_END { get; set; }
    }
}