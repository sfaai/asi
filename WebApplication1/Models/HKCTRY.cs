using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1
{
    [MetadataType(typeof(HKCTRYMetadata))]
    public partial class HKCTRY
    {
    }

    public class HKCTRYMetadata
    {
        [Display(Name = "Country")]
        public string CTRYCODE { get; set; }

        [Display(Name = "Country Name")]
        public string CTRYDESC { get; set; }

    }
}