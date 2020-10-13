using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1
{
    [MetadataType(typeof(CSITEMMetadata))]
    public partial class CSITEM
    {
    }

    public class CSITEMMetadata
    {
        [Display(Name = "Item", Order = 10)]
        public string ITEMTYPE { get; set; }

        [Display(Name = "Item Type", Order = 20)]
        public string ITEMDESC { get; set; }

        [Display(Name = "Tax", Order = 30)]
        public string GSTCODE { get; set; }

        [Display(Name = "Tax Rate", Order = 40)]
        [DisplayFormat(DataFormatString = "{0:F2}")]
        public decimal GSTRATE { get; set; }

    }
    }