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
    
    public partial class SAUSERDP
    {
        public string USERID { get; set; }
        public string MODULECODE { get; set; }
        public string PRGCODE { get; set; }
        public string CANACC { get; set; }
        public string CANADD { get; set; }
        public string CANMOD { get; set; }
        public string CANDEL { get; set; }
        public string CANPRN { get; set; }
        public int STAMP { get; set; }
    
        public virtual SAPRG SAPRG { get; set; }
    }
}
