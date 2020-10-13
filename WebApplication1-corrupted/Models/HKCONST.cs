using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1
{
    [MetadataType(typeof(HKCONSTMetadata))]
    public partial class HKCONST
    {



    }


    public class HKCONSTMetadata
    {
        [Display(Name = "Constitution")]
        public string CONSTCODE { get; set; }

        [Display(Name = "Const. Desc")]
        public string CONSTDESC { get; set; }

        [Display(Name = "Constitution Type")]
        public string CONSTTYPE { get; set; }

        public string AGM { get; set; }
        public int AGM1RULE1 { get; set; }
        public int AGM1RULE2 { get; set; }
        public int AGMRULE1 { get; set; }
        public int AGMRULE2 { get; set; }
        public string R1 { get; set; }
        public int R1MTHB4AGM { get; set; }
        public string R1TEMCODE { get; set; }
        public string R2 { get; set; }
        public int R2MTHB4AGM { get; set; }
        public string R2TEMCODE { get; set; }
        public string R3 { get; set; }
        public int R3MTHB4AGM { get; set; }
        public string R3TEMCODE { get; set; }
        public string RSTEMCODE { get; set; }
        public int STAMP { get; set; }
    }
}