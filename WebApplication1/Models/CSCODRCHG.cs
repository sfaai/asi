using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1
{
    [MetadataType(typeof(CSCODRCHGMetadata))]
    public partial class CSCODRCHG
    {
    }

    public class CSCODRCHGMetadata
    {
        [Key]
        [Display(Name = "Company", Order = 12)]
        [MaxLength(10)]
        public string CONO { get; set; }

        [Key]
        [Display(Name = "Entity", Order = 12)]
        [MaxLength(10)]
        public string PRSCODE { get; set; }

        [Display(Name = "Name", Order = 12)]
        [MaxLength(60)]
        public string PRSNAME { get; set; }

        [Display(Name = "Addr Id", Order = 12)]
        public short ADDRID { get; set; }

        [Display(Name = "Row", Order = 12)]
        public int ROWNO { get; set; }

        [Display(Name = "Remarks", Order = 12)]
        [MaxLength(60)]
        public string REM { get; set; }

        [Display(Name = "Appointed Date", Order = 1)]
        [DataType(DataType.Date)]
        public System.DateTime ADATE { get; set; }

        [Display(Name = "Change Date", Order = 1)]
        [DataType(DataType.Date)]
        public System.DateTime CHGEFFDATE { get; set; }

        [Display(Name = "Change Remarks", Order = 12)]
        [MaxLength(256)]
        public string CHGREM { get; set; }

        [Display(Name = "Nation", Order = 12)]
        [MaxLength(20)]
        public string NATION { get; set; }

        [Display(Name = "Race", Order = 12)]
        [MaxLength(20)]
        public string RACE { get; set; }



        [Display(Name = "Occupation", Order = 12)]
        [MaxLength(60)]
        public string OCCUPATION { get; set; }

        [Display(Name = "Country", Order = 12)]
        [MaxLength(5)]
        public string REGCTRY1 { get; set; }

        [Display(Name = "Type", Order = 12)]
        [MaxLength(20)]
        public string REGTYPE1 { get; set; }

        [Display(Name = "Registration #1", Order = 12)]
        [MaxLength(20)]
        public string REGID1 { get; set; }

        [Display(Name = "Country", Order = 12)]
        [MaxLength(20)]
        public string REGCTRY2 { get; set; }

        [Display(Name = "Type", Order = 12)]
        [MaxLength(20)]
        public string REGTYPE2 { get; set; }


        [Display(Name = "Registration #2", Order = 12)]
        public string REGID2 { get; set; }

        [Display(Name = "Changed", Order = 12)]
        public int STAMP { get; set; }

    }
}