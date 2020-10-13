using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1
{
    [MetadataType(typeof(CSCASEDOCMetadata))]
    public partial class CSCASEDOC
    {
    }

    public class CSCASEDOCMetadata
    {
        [Display(Name = "Case Code", Order = 10)]
        public string CASECODE { get; set; }

        [Display(Name = "Document Id", Order = 20)]
        public int DOCINVID { get; set; }

        [Display(Name = "Document Description", Order = 30)]
        [MaxLength(60)]
        public string DOCINVDESC { get; set; }

        [Display(Name = "Unit", Order = 40)]
        [MaxLength(20)]
        public string DOCINVQTY { get; set; }
    }
}