using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1
{
    [MetadataType(typeof(CSCOSTATMetadata))]
    public partial class CSCOSTAT
    {
    }

    public class CSCOSTATMetadata
    {
        [Display(Name = "Effective Date", Order = 1)]
        [DataType(DataType.Date)]
        public System.DateTime SDATE { get; set; }

        [Display(Name = "End Date")]
        [ScaffoldColumn(false)]
        //[HiddenInput(DisplayValue = false)]
        [DataType(DataType.Date)]
        public System.DateTime EDATE { get; set; }

        [Key]
        [Display(Name = "Company", Order = 12)]
        public string CONO { get; set; }

        [Display(Name = "Status", Order = 23)]
        public string COSTAT { get; set; }

        [Display(Name = "File Type", Order = 34)]
        public string FILETYPE { get; set; }

        [Display(Name = "File No.", Order = 45)]
        public string FILELOC { get; set; }

        [Display(Name = "Seal No.", Order = 56)]
        public string SEALLOC { get; set; }

        [ScaffoldColumn(false)]
        [Display(AutoGenerateField =false, Order = 99)]
        //[HiddenInput(DisplayValue = false)]
        public int STAMP { get; set; }
    }
}