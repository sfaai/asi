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
    
    public partial class SILNRCM
    {
        public string TRNO { get; set; }
        public System.DateTime VDATE { get; set; }
        public string STAFFCODE { get; set; }
        public decimal RCAMT { get; set; }
        public string RCMODE { get; set; }
        public string RCMAPCODE { get; set; }
        public string ISSBANK { get; set; }
        public string ISSLOC { get; set; }
        public string ISSREFNO { get; set; }
        public Nullable<System.DateTime> ISSDATE { get; set; }
        public decimal COMAMT { get; set; }
        public decimal NETAMT { get; set; }
        public string REM { get; set; }
        public int SEQNO { get; set; }
        public string POST { get; set; }
        public string CFLAG { get; set; }
        public string CTRNO { get; set; }
        public Nullable<System.DateTime> CVDATE { get; set; }
        public string CREM { get; set; }
        public Nullable<int> CSEQNO { get; set; }
        public string CPOST { get; set; }
        public int STAMP { get; set; }
    
        public virtual HKBANK HKBANK { get; set; }
        public virtual HKMAP HKMAP { get; set; }
    }
}
