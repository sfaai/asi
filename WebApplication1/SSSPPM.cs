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
    
    public partial class SSSPPM
    {
        public string LOGINID { get; set; }
        public string STAFFCODE { get; set; }
        public string RSTAFFCODE { get; set; }
        public string ASTAFFCODE { get; set; }
        public string SMTPSMTP { get; set; }
        public Nullable<int> SMTPPORT { get; set; }
        public string SMTPTYPE { get; set; }
        public string SMTPACC { get; set; }
        public string SMTPPW { get; set; }
        public int STAMP { get; set; }
    
        public virtual SISTFST SISTFST { get; set; }
        public virtual SISTFST SISTFST1 { get; set; }
        public virtual SISTFST SISTFST2 { get; set; }
    }
}
