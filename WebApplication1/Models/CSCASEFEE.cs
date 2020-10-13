using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1
{
    [MetadataType(typeof(CSCASEFEEMetadata))]
    public partial class CSCASEFEE
    {
    }

    public class CSCASEFEEMetadata
    {
        [Display(Name ="Case Code", Order = 10)]
        public string CASECODE { get; set; }

        [Display(Name = "Row", Order = 20)]
        public int FEEID { get; set; }

        [Display(Name = "Item Type", Order = 30)]
        public string ITEMTYPE { get; set; }


        [Display(Name = "Item Description", Order = 40)]
        public string ITEMDESC { get; set; }

        [Display(Name = "Item Specification", Order = 50)]
        public string ITEMSPEC { get; set; }

        [Display(Name = "Tax Rate %", Order = 60)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal TAXRATE { get; set; }

        [Display(Name = "Own Amount", Order = 110)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal ITEMAMT1 { get; set; }

        [Display(Name = "Own Tax", Order = 120)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal TAXAMT1 { get; set; }

        [Display(Name = "Own Net", Order = 130)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal NETAMT1 { get; set; }

        [Display(Name = "3rd Party Amount", Order = 210)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal ITEMAMT2 { get; set; }

        [Display(Name = "3rd Party Tax", Order = 220)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal TAXAMT2 { get; set; }

        [Display(Name = "3rd Party Net", Order = 230)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal NETAMT2 { get; set; }

        [Display(Name = "Total Amount", Order = 310)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal ITEMAMT { get; set; }

        [Display(Name = "Total Tax", Order = 320)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal TAXAMT { get; set; }

        [Display(Name = "Total Net", Order = 330)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal NETAMT { get; set; }

        [Display(Name = "Changed", Order = 12)]
        public int STAMP { get; set; }
    }
}