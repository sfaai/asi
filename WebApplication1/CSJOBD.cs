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
    
    public partial class CSJOBD
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CSJOBD()
        {
            this.CSJOBSTs = new HashSet<CSJOBST>();
        }
    
        public string JOBNO { get; set; }
        public int CASENO { get; set; }
        public string CASECODE { get; set; }
        public string CASEMEMO { get; set; }
        public string CASEREM { get; set; }
        public string STAGE { get; set; }
        public System.DateTime STAGEDATE { get; set; }
        public string STAGETIME { get; set; }
        public string COMPLETE { get; set; }
        public System.DateTime COMPLETED { get; set; }
        public int STAMP { get; set; }
    
        public virtual CSCASE CSCASE { get; set; }
        public virtual CSJOBM CSJOBM { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CSJOBST> CSJOBSTs { get; set; }
    }
}
