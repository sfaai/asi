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
    
    public partial class CQREG
    {
        public string TRNO { get; set; }
        public System.DateTime VDATE { get; set; }
        public string BANKCODE { get; set; }
        public string BANKACNO { get; set; }
        public string SNO { get; set; }
        public string ENO { get; set; }
        public string POST { get; set; }
        public int STAMP { get; set; }
    
        public virtual CQBANK CQBANK { get; set; }
    }
}