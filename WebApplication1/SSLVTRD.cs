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
    
    public partial class SSLVTRD
    {
        public string TRNO { get; set; }
        public string STAFFCODE { get; set; }
        public string COVERTYPE { get; set; }
        public int STAMP { get; set; }
    
        public virtual SISTFST SISTFST { get; set; }
    }
}
