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
    
    public partial class SISTFSTMV
    {
        public string STAFFCODE { get; set; }
        public System.DateTime SDATE { get; set; }
        public System.DateTime EDATE { get; set; }
        public string MVTYPE { get; set; }
        public string DPCODE { get; set; }
        public string DESIG { get; set; }
        public string RKCODE { get; set; }
        public string DPHEAD { get; set; }
        public int STAFFLEVEL { get; set; }
        public string MSTRSTAFF { get; set; }
        public string REM { get; set; }
        public string POST { get; set; }
        public int STAMP { get; set; }
    
        public virtual SIDP SIDP { get; set; }
        public virtual SIRK SIRK { get; set; }
        public virtual SISTFST SISTFST { get; set; }
    }
}