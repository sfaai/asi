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
    
    public partial class CSITEM
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CSITEM()
        {
            this.CSBILLs = new HashSet<CSBILL>();
            this.CSCASEFEEs = new HashSet<CSCASEFEE>();
            this.CSTRANMs = new HashSet<CSTRANM>();
        }
    
        public string ITEMTYPE { get; set; }
        public string ITEMDESC { get; set; }
        public string GSTCODE { get; set; }
        public Nullable<decimal> GSTRATE { get; set; }
        public Nullable<int> STAMP { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CSBILL> CSBILLs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CSCASEFEE> CSCASEFEEs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CSTRANM> CSTRANMs { get; set; }
    }
}
