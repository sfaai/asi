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
    
    public partial class SSBFTRM
    {
        public string TRNO { get; set; }
        public System.DateTime VDATE { get; set; }
        public string TRTYPE { get; set; }
        public string TRLOGINID { get; set; }
        public string STAFFCODE { get; set; }
        public string BFCODE { get; set; }
        public byte[] PURPOSE { get; set; }
        public System.DateTime BFSDATE { get; set; }
        public System.DateTime BFEDATE { get; set; }
        public decimal BFAMT { get; set; }
        public decimal CLAIMAMT { get; set; }
        public string REM { get; set; }
        public string LOGINID { get; set; }
        public string FINYEAR { get; set; }
        public string POST { get; set; }
        public int SEQNO { get; set; }
        public string OC { get; set; }
        public Nullable<System.DateTime> OCDATE { get; set; }
        public string OCREM { get; set; }
        public string OCPOST { get; set; }
        public string PYMODE { get; set; }
        public string PYMAPCODE { get; set; }
        public Nullable<int> OCSEQNO { get; set; }
        public int STAMP { get; set; }
    
        public virtual SIBF SIBF { get; set; }
        public virtual SISTFST SISTFST { get; set; }
    }
}
