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
    
    public partial class SFSGADDRCHG
    {
        public string TRNO { get; set; }
        public System.DateTime VDATE { get; set; }
        public string CONO { get; set; }
        public Nullable<short> OADDRID { get; set; }
        public string ADDR1 { get; set; }
        public string ADDR2 { get; set; }
        public string ADDR3 { get; set; }
        public string POSTAL { get; set; }
        public string CITYCODE { get; set; }
        public string STATECODE { get; set; }
        public string CTRYCODE { get; set; }
        public byte[] OPRHRS { get; set; }
        public string PRSCODE { get; set; }
        public Nullable<System.DateTime> PRSADATE { get; set; }
        public string STAFFCODE { get; set; }
        public string REM { get; set; }
        public Nullable<System.DateTime> EFFDATE { get; set; }
        public string POST { get; set; }
        public int STAMP { get; set; }
    
        public virtual CSCOADDR CSCOADDR { get; set; }
        public virtual CSCODR CSCODR { get; set; }
        public virtual CSCOMSTR CSCOMSTR { get; set; }
        public virtual HKCITY HKCITY { get; set; }
        public virtual HKCTRY HKCTRY { get; set; }
        public virtual HKSTAFF HKSTAFF { get; set; }
        public virtual HKSTATE HKSTATE { get; set; }
    }
}
