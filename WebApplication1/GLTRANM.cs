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
    
    public partial class GLTRANM
    {
        public string BATCHCODE { get; set; }
        public string BATCHNO { get; set; }
        public System.DateTime BATCHDATE { get; set; }
        public string JOURCODE { get; set; }
        public string REM { get; set; }
        public string SYSGEN { get; set; }
        public string FINYEAR { get; set; }
        public string FINMONTH { get; set; }
        public string POST { get; set; }
        public int STAMP { get; set; }
    
        public virtual GLBATCH GLBATCH { get; set; }
        public virtual GLJOUR GLJOUR { get; set; }
    }
}
