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
    
    public partial class CICLADDR
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CICLADDR()
        {
            this.APCNMs = new HashSet<APCNM>();
            this.APINVMs = new HashSet<APINVM>();
            this.APPOMs = new HashSet<APPOM>();
            this.SILNDDs = new HashSet<SILNDD>();
        }
    
        public string CLCODE { get; set; }
        public short ADDRID { get; set; }
        public string MAILADDR { get; set; }
        public string ADDRTYPE { get; set; }
        public string ADDR1 { get; set; }
        public string ADDR2 { get; set; }
        public string ADDR3 { get; set; }
        public string POSTAL { get; set; }
        public string CITYCODE { get; set; }
        public string STATECODE { get; set; }
        public string CTRYCODE { get; set; }
        public string PHONE1 { get; set; }
        public string PHONE2 { get; set; }
        public string FAX1 { get; set; }
        public string FAX2 { get; set; }
        public string REM { get; set; }
        public int STAMP { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<APCNM> APCNMs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<APINVM> APINVMs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<APPOM> APPOMs { get; set; }
        public virtual HKCITY HKCITY { get; set; }
        public virtual HKSTATE HKSTATE { get; set; }
        public virtual HKCTRY HKCTRY { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SILNDD> SILNDDs { get; set; }
    }
}