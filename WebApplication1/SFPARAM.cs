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
    
    public partial class SFPARAM
    {
        public string PARAMCODE { get; set; }
        public string PARAMDESC { get; set; }
        public string ADDR1 { get; set; }
        public string ADDR2 { get; set; }
        public string ADDR3 { get; set; }
        public string POSTAL { get; set; }
        public string CITYCODE { get; set; }
        public string STATECODE { get; set; }
        public string CTRYCODE { get; set; }
        public string DCLCITY { get; set; }
        public string DCLSTATE { get; set; }
        public string CONSTCODE { get; set; }
        public string INCCTRY { get; set; }
        public int STAMP { get; set; }
    
        public virtual HKCITY HKCITY { get; set; }
        public virtual HKCONST HKCONST { get; set; }
        public virtual HKCTRY HKCTRY { get; set; }
        public virtual HKCTRY HKCTRY1 { get; set; }
        public virtual HKSTATE HKSTATE { get; set; }
    }
}
