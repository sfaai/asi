using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1
{
    [MetadataType(typeof(CSJOBSTMetadata))]
    public partial class CSJOBST
    {
    }

    public class CSJOBSTMetadata
    {
        [Display(Name = "JOB #", Order = 10)]
        public string JOBNO { get; set; }

        [Display(Name = "row", Order = 10)]
        public int CASENO { get; set; }

        [Display(Name = "Stage From", Order = 10)]
        [MaxLength(30)]
        public string STAGEFR { get; set; }

        [Display(Name = "Stage To", Order = 10)]
        [MaxLength(30)]
        public string STAGETO { get; set; }


        [Display(Name = "In Date", Order = 20)]
        [DataType(DataType.Date)]
        public System.DateTime INDATE { get; set; }

        [Display(Name = "In Time", Order = 20)]
        [MaxLength(8)]
        public string INTIME { get; set; }


        [Display(Name = "Out Date", Order = 20)]
        [DataType(DataType.Date)]
        public System.DateTime OUTDATE { get; set; }

        [Display(Name = "Send Mode", Order = 20)]
        public string SENDMODE { get; set; }

        [Display(Name = "Remarks", Order = 90)]
        [DataType(DataType.MultilineText)]
        [MaxLength(256)]
        public string REM { get; set; }

        [Display(Name = "Index", Order = 90)]
        public string INIDX { get; set; }
        public int STAMP { get; set; }
    }
}