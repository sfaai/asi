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
    
    public partial class SATEM
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SATEM()
        {
            this.CQBANKs = new HashSet<CQBANK>();
            this.CSQTs = new HashSet<CSQT>();
            this.HKCONSTs = new HashSet<HKCONST>();
            this.HKCONSTs1 = new HashSet<HKCONST>();
            this.HKCONSTs2 = new HashSet<HKCONST>();
            this.HKCONSTs3 = new HashSet<HKCONST>();
        }
    
        public string TEMCODE { get; set; }
        public string TEMDESC { get; set; }
        public string TEMPLATE { get; set; }
        public string OUTTYPE { get; set; }
        public string OUTPROTECT { get; set; }
        public string USERDEF { get; set; }
        public int STAMP { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CQBANK> CQBANKs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CSQT> CSQTs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HKCONST> HKCONSTs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HKCONST> HKCONSTs1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HKCONST> HKCONSTs2 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HKCONST> HKCONSTs3 { get; set; }
    }
}