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
    
    public partial class CSCASE
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CSCASE()
        {
            this.CSCNDs = new HashSet<CSCND>();
            this.CSCOFEEs = new HashSet<CSCOFEE>();
            this.CSDNDs = new HashSet<CSDND>();
            this.CSJOBDs = new HashSet<CSJOBD>();
            this.SFSGNEWFEEs = new HashSet<SFSGNEWFEE>();
            this.CSBILL = new HashSet<CSBILL>();
        }
    
        public string CASECODE { get; set; }
        public string CASEDESC { get; set; }
        public string MAPSFIX { get; set; }
        public int STAMP { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CSCND> CSCNDs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CSCOFEE> CSCOFEEs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CSDND> CSDNDs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CSJOBD> CSJOBDs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SFSGNEWFEE> SFSGNEWFEEs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CSBILL> CSBILL { get; set; }
    }
}
