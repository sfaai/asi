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
    
    public partial class CQPARAM
    {
        public string PARAMCODE { get; set; }
        public string PARAMDESC { get; set; }
        public string BANKCODE { get; set; }
        public string BANKACNO { get; set; }
        public Nullable<short> MAXITEM { get; set; }
        public int STAMP { get; set; }
    
        public virtual CQBANK CQBANK { get; set; }
    }
}