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
    
    public partial class APCNM
    {
        public string TRNO { get; set; }
        public System.DateTime VDATE { get; set; }
        public string TRTYPE { get; set; }
        public string CLCODE { get; set; }
        public Nullable<short> CLADDRID { get; set; }
        public string CNNO { get; set; }
        public System.DateTime CNDATE { get; set; }
        public string CURRCODE { get; set; }
        public decimal CNAMT { get; set; }
        public string REM { get; set; }
        public int SEQNO { get; set; }
        public string POST { get; set; }
        public int STAMP { get; set; }
    
        public virtual CICL CICL { get; set; }
        public virtual CICLADDR CICLADDR { get; set; }
        public virtual GLCURR GLCURR { get; set; }
    }
}