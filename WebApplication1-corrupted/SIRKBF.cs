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
    
    public partial class SIRKBF
    {
        public string RKCODE { get; set; }
        public string BFCODE { get; set; }
        public decimal BFLMT { get; set; }
        public int STAMP { get; set; }
    
        public virtual SIBF SIBF { get; set; }
    }
}
