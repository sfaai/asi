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
    
    public partial class SFSGNAMECHG
    {
        public string TRNO { get; set; }
        public System.DateTime VDATE { get; set; }
        public string CONO { get; set; }
        public string APPTRNO { get; set; }
        public string PRSCODE { get; set; }
        public Nullable<System.DateTime> PRSADATE { get; set; }
        public string EGMADDR1 { get; set; }
        public string EGMADDR2 { get; set; }
        public string EGMADDR3 { get; set; }
        public string EGMPOSTAL { get; set; }
        public string EGMCITYCODE { get; set; }
        public string EGMSTATECODE { get; set; }
        public string EGMCTRYCODE { get; set; }
        public Nullable<System.DateTime> EGMDATE { get; set; }
        public string EGMTIME { get; set; }
        public string STAFFCODE { get; set; }
        public string REM { get; set; }
        public Nullable<System.DateTime> EFFDATE { get; set; }
        public string POST { get; set; }
        public int STAMP { get; set; }
    
        public virtual CSCODR CSCODR { get; set; }
        public virtual CSCOMSTR CSCOMSTR { get; set; }
        public virtual HKCITY HKCITY { get; set; }
        public virtual HKCTRY HKCTRY { get; set; }
        public virtual HKSTAFF HKSTAFF { get; set; }
        public virtual HKSTATE HKSTATE { get; set; }
    }
}