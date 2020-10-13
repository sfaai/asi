using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using WebApplication1.Utility;

namespace WebApplication1
{
    [MetadataType(typeof(CSJOBSTFMetadata))]
    public partial class CSJOBSTF
    {

    }

    public class CSJOBSTFMetadata
    {
        [Display(Name = "JOB #", Order = 10)]
        public string JOBNO { get; set; }

        [Display(Name = "row", Order = 10)]
        public int ROWNO { get; set; }

        [Display(Name = "Remarks", Order = 90)]
        [DataType(DataType.MultilineText)]
        [MaxLength(60)]
        public string REM { get; set; }

        [Display(Name = "Staff", Order = 70)]
        [MaxLength(10)]
        public string STAFFCODE { get; set; }

        [Display(Name = "Start Date", Order = 20)]
        [DataType(DataType.Date)]
        public System.DateTime SDATE { get; set; }

        [Display(Name = "End Date", Order = 20)]
        [DataType(DataType.Date)]
        public System.DateTime EDATE { get; set; }



    }
}