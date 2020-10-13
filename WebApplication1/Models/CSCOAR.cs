using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1
{
    [MetadataType(typeof(CSCOARMetadata))]
    public partial class CSCOAR
    {
    }
    public class CSCOARMetadata
    {

        [Display(Name = "AR No", Order = 1)]
        public int ARNO { get; set; }

        [Display(Name = "Last AR Filed", Order = 2 )]
        [DataType(DataType.Date)]
        public System.DateTime LASTAR { get; set; }

        [Display(Name = "AR Due", Order = 3)]
        [DataType(DataType.Date)]
        public System.DateTime ARTOFILE{ get; set; }


        [Display(Name = "Reminder", Order = 4)]
        [DataType(DataType.Date)]
        public System.DateTime REMINDER1 { get; set; }

        [Display(Name = "AR Filed", Order = 5)]
        [DataType(DataType.Date)]
        public System.DateTime FILEDAR { get; set; }

        [Display(Name = "Submission Date", Order = 6)]
        [DataType(DataType.Date)]
        public System.DateTime SUBMITAR { get; set; }

        [Display(Name = "Remarks", Order = 7)]
        public string REM { get; set; }

        [Display(Name = "Changed", Order = 12)]
        public int STAMP { get; set; }
    }
}