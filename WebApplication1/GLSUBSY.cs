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
    
    public partial class GLSUBSY
    {
        public string SUBSYSCODE { get; set; }
        public string SUBSYSDESC { get; set; }
        public string BATCHCODE { get; set; }
        public string JOURCODE { get; set; }
        public string BRANCHCODE { get; set; }
        public string UNITCODE { get; set; }
        public string PRJCODE { get; set; }
        public string STAFFCODE { get; set; }
        public string CURRCODE { get; set; }
        public int STAMP { get; set; }
    
        public virtual GLBATCH GLBATCH { get; set; }
        public virtual GLBRANCH GLBRANCH { get; set; }
        public virtual GLCURR GLCURR { get; set; }
        public virtual GLJOUR GLJOUR { get; set; }
        public virtual GLPRJ GLPRJ { get; set; }
        public virtual GLSTAFF GLSTAFF { get; set; }
        public virtual GLUNIT GLUNIT { get; set; }
    }
}