using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1
{
    [MetadataType(typeof(HKBANKMetadata))]
    public partial class HKBANK
    {
    }

    public class HKBANKMetadata
    {
        [Display(Name = "Bank")]
        public string BANKCODE { get; set; }

        [Display(Name = "Bank Name")]
        public string BANKDESC { get; set; }

    }
}