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
    
    public partial class CSCOPUK
    {
        public string CONO { get; set; }
        public System.DateTime EFFDATE { get; set; }
        public string EQCODE { get; set; }
        public string EQCONSIDER { get; set; }
        public decimal NOOFSHARES { get; set; }
        public decimal NOMINAL { get; set; }
        public decimal PAIDAMT { get; set; }
        public decimal DUEAMT { get; set; }
        public decimal PREMIUM { get; set; }
        public byte[] NCDET { get; set; }
        public System.DateTime ENDDATE { get; set; }
        public int ROWNO { get; set; }
        public int STAMP { get; set; }
    
        public virtual CSEQ CSEQ { get; set; }
        public virtual CSCOMSTR CSCOMSTR { get; set; }
    }
}
