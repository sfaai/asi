//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebApplication1
{
    using System;
    using System.Collections.Generic;
    
    public partial class SILNCAP
    {
        public string TRNO { get; set; }
        public System.DateTime VDATE { get; set; }
        public string LOANNO { get; set; }
        public string STAFFCODE { get; set; }
        public string INVTYPE { get; set; }
        public string INVNO { get; set; }
        public decimal CAPAMT { get; set; }
        public string REM { get; set; }
        public int SEQNO { get; set; }
        public string POST { get; set; }
        public int STAMP { get; set; }
    
        public virtual SILNMSTR SILNMSTR { get; set; }
    }
}
