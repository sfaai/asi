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
    
    public partial class CICLREG
    {
        public string CLCODE { get; set; }
        public string CTRYCODE { get; set; }
        public string REGTYPE { get; set; }
        public string REGNO { get; set; }
        public string REM { get; set; }
        public int STAMP { get; set; }
    
        public virtual HKCTRY HKCTRY { get; set; }
    }
}
