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
    
    public partial class SFSGNEWSHD1
    {
        public string TRNO { get; set; }
        public string PRSCODE { get; set; }
        public string EQCODE { get; set; }
        public string EQCONSIDER { get; set; }
        public decimal NOOFSHARES { get; set; }
        public decimal AMT { get; set; }
        public int STAMP { get; set; }
    
        public virtual CSEQ CSEQ { get; set; }
    }
}
