using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1
{
    [MetadataType(typeof(CSCOSHEQDMetadata))]

    public partial class CSCOSHEQD
    {
    }

    public class CSCOSHEQDMetadata
    {
        [Key]
        [Display(Name = "Company", Order = 12)]
        public string CONO { get; set; }

        [Display(Name = "Shareholder", Order = 12)]
        [MaxLength(10)]
        public string PRSCODE { get; set; }

        [Display(Name = "Movement", Order = 12)]
        [MaxLength(10)]
        public string MVTYPE { get; set; }

        [Display(Name = "Tran #", Order = 12)]
        public string MVNO { get; set; }

        [Display(Name = "Id", Order = 12)]
        public int MVID { get; set; }

        [Display(Name = "Tran Date", Order = 1)]
        [DataType(DataType.Date)]
        public System.DateTime MVDATE { get; set; }


        [Display(Name = "In Tran Type", Order = 12)]
        public string INMVTYPE { get; set; }

        [Display(Name = "In Tran #", Order = 12)]
        public string INMVNO { get; set; }
  
        [Display(Name = "In Tran Id", Order = 12)]
        public int INMVID { get; set; }

        [Display(Name = "Out Tran Type", Order = 12)]
        public string OUTMVTYPE { get; set; }

        [Display(Name = "Out Tran #", Order = 12)]
        public string OUTMVNO { get; set; }

        [Display(Name = "Out Tran Id", Order = 12)]
        public int OUTMVID { get; set; }

        [Display(Name = "Share Amt", Order = 110)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal SHAREAMT { get; set; }

        [Display(Name = "Changed", Order = 12)]
        public int STAMP { get; set; }

    }
}