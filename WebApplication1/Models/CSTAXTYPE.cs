using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1
{
    [MetadataType(typeof(CSTAXTYPEMetadata))]
    public partial class CSTAXTYPE
    {public bool ACTIVE_FLAGbool { get { return this.ACTIVE_FLAG == "Y"; } set { this.ACTIVE_FLAG = value ? "Y" : "N"; } }

    }

    public class CSTAXTYPEMetadata
    {
        [Display(Name = "Start Date", Order = 20)]
        [DataType(DataType.Date)]
        public DateTime EFFECTIVE_START { get; set; }

        [Display(Name = "END Date", Order = 30)]
        [DataType(DataType.Date)]
        public DateTime EFFECTIVE_END { get; set; }

        [Display(Name = "Tax Code", Order = 20)]
        [MaxLength(10)]
        public string TAXCODE { get; set; }

        [Display(Name = "Tax Description", Order = 20)]
        [MaxLength(30)]
        public string TAXDESC { get; set; }

        [Display(Name = "Tax Type", Order = 20)]
        [MaxLength(10)]
        public string TAXTYPE { get; set; }

        [Display(Name = "Tax RCode", Order = 20)]
        [MaxLength(10)]
        public string TAXRCODE { get; set; }


        [Display(Name = "Tax Rate", Order = 20)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal TAXRATE { get; set; }

        [Display(Name = "Active", Order = 20)]
        public string ACTIVE_FLAG { get; set; }

        [Display(Name = "Changed", Order = 12)]
        public int STAMP { get; set; }
    }

}