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
    
    public partial class FALOCCHG
    {
        public string SOURCE { get; set; }
        public string SOURCENO { get; set; }
        public int SOURCEID { get; set; }
        public string ASSETNO { get; set; }
        public System.DateTime SDATE { get; set; }
        public System.DateTime EDATE { get; set; }
        public string LOCCODE { get; set; }
        public string REM { get; set; }
        public string POST { get; set; }
        public int STAMP { get; set; }
    
        public virtual FALOC FALOC { get; set; }
    }
}
