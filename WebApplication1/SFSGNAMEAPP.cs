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
    
    public partial class SFSGNAMEAPP
    {
        public string TRNO { get; set; }
        public System.DateTime VDATE { get; set; }
        public string APPNAME { get; set; }
        public string APPPURPOSE { get; set; }
        public string APPTYPE { get; set; }
        public string APPCONST { get; set; }
        public string PINDCODE { get; set; }
        public string SINDCODE { get; set; }
        public string OBTAINAPPR { get; set; }
        public string TAKEOVERBU { get; set; }
        public string STAFFCODE { get; set; }
        public string REM { get; set; }
        public string APPST { get; set; }
        public System.DateTime APPSTD { get; set; }
        public Nullable<System.DateTime> APPEXP { get; set; }
        public string APPREFNO { get; set; }
        public string POST { get; set; }
        public int STAMP { get; set; }
    
        public virtual HKCONST HKCONST { get; set; }
        public virtual HKSTAFF HKSTAFF { get; set; }
    }
}
