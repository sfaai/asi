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
    
    public partial class CSEQ
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CSEQ()
        {
            this.CSCOPUKs = new HashSet<CSCOPUK>();
            this.EMTRAs = new HashSet<EMTRA>();
            this.EMTRCDs = new HashSet<EMTRCD>();
            this.SFMYNEWPUKs = new HashSet<SFMYNEWPUK>();
            this.SFMYNEWSHD1 = new HashSet<SFMYNEWSHD1>();
            this.SFMYNEWSHD2 = new HashSet<SFMYNEWSHD2>();
            this.SFSGNEWDRD2 = new HashSet<SFSGNEWDRD2>();
            this.SFSGNEWDRD3 = new HashSet<SFSGNEWDRD3>();
            this.SFSGNEWPUKs = new HashSet<SFSGNEWPUK>();
            this.SFSGNEWSHD1 = new HashSet<SFSGNEWSHD1>();
            this.SFSGNEWSHD2 = new HashSet<SFSGNEWSHD2>();
            this.CSCOSHEQs = new HashSet<CSCOSHEQ>();
        }
    
        public string EQCODE { get; set; }
        public string EQDESC { get; set; }
        public string EQCAT { get; set; }
        public int STAMP { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CSCOPUK> CSCOPUKs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EMTRA> EMTRAs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EMTRCD> EMTRCDs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SFMYNEWPUK> SFMYNEWPUKs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SFMYNEWSHD1> SFMYNEWSHD1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SFMYNEWSHD2> SFMYNEWSHD2 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SFSGNEWDRD2> SFSGNEWDRD2 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SFSGNEWDRD3> SFSGNEWDRD3 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SFSGNEWPUK> SFSGNEWPUKs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SFSGNEWSHD1> SFSGNEWSHD1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SFSGNEWSHD2> SFSGNEWSHD2 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CSCOSHEQ> CSCOSHEQs { get; set; }
    }
}
