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
    
    public partial class HKCONST
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public HKCONST()
        {
            this.CICLs = new HashSet<CICL>();
            this.CSCOMSTRs = new HashSet<CSCOMSTR>();
            this.CSPRS = new HashSet<CSPR>();
            this.SFPARAMs = new HashSet<SFPARAM>();
            this.SFSGNAMEAPPs = new HashSet<SFSGNAMEAPP>();
        }
    
        public string CONSTCODE { get; set; }
        public string CONSTDESC { get; set; }
        public string CONSTTYPE { get; set; }
        public string AGM { get; set; }
        public int AGM1RULE1 { get; set; }
        public int AGM1RULE2 { get; set; }
        public int AGMRULE1 { get; set; }
        public int AGMRULE2 { get; set; }
        public string R1 { get; set; }
        public int R1MTHB4AGM { get; set; }
        public string R1TEMCODE { get; set; }
        public string R2 { get; set; }
        public int R2MTHB4AGM { get; set; }
        public string R2TEMCODE { get; set; }
        public string R3 { get; set; }
        public int R3MTHB4AGM { get; set; }
        public string R3TEMCODE { get; set; }
        public string RSTEMCODE { get; set; }
        public int STAMP { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CICL> CICLs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CSCOMSTR> CSCOMSTRs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CSPR> CSPRS { get; set; }
        public virtual SATEM SATEM { get; set; }
        public virtual SATEM SATEM1 { get; set; }
        public virtual SATEM SATEM2 { get; set; }
        public virtual SATEM SATEM3 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SFPARAM> SFPARAMs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SFSGNAMEAPP> SFSGNAMEAPPs { get; set; }
    }
}
