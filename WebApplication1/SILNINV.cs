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
    
    public partial class SILNINV
    {
        public string INVTYPE { get; set; }
        public string INVNO { get; set; }
        public System.DateTime VDATE { get; set; }
        public System.DateTime DUEDATE { get; set; }
        public System.DateTime GRAZEDATE { get; set; }
        public string LOANNO { get; set; }
        public string STAFFCODE { get; set; }
        public string CHRGCODE { get; set; }
        public decimal INVAMT { get; set; }
        public decimal APPAMT { get; set; }
        public decimal INVOS { get; set; }
        public string INVDESC { get; set; }
        public string REM { get; set; }
        public string NPLFLAG { get; set; }
        public string PENALTY { get; set; }
        public System.DateTime LASTTOUCH { get; set; }
        public System.DateTime ZODATE { get; set; }
        public string ISOURCE { get; set; }
        public string ISOURCENO { get; set; }
        public string FSOURCE { get; set; }
        public string FSOURCENO { get; set; }
        public Nullable<int> FSOURCEID { get; set; }
        public int SEQNO { get; set; }
        public string POST { get; set; }
        public string SYSGEN { get; set; }
        public int STAMP { get; set; }
    
        public virtual SICHRG SICHRG { get; set; }
        public virtual SILNMSTR SILNMSTR { get; set; }
    }
}
