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
    
    public partial class CSCOPIC
    {
        public string CONO { get; set; }
        public string PRSCODE { get; set; }
        public string DESIG { get; set; }
        public string REM { get; set; }
        public int STAMP { get; set; }
    
        public virtual CSPR CSPR { get; set; }
    }
}
