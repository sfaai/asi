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
    
    public partial class CSCOADDR
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CSCOADDR()
        {
            this.CSPRFs = new HashSet<CSPRF>();
            this.CSQTs = new HashSet<CSQT>();
            this.SFMYADDRCHGs = new HashSet<SFMYADDRCHG>();
            this.SFSGADDRCHGs = new HashSet<SFSGADDRCHG>();
            this.CSCNMs = new HashSet<CSCNM>();
            this.CSDNM = new HashSet<CSDNM>();
        }
    
        public string CONO { get; set; }
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
        public byte[] OPRHRS { get; set; }
        public string REM { get; set; }
        public System.DateTime SDATE { get; set; }
        public Nullable<System.DateTime> EDATE { get; set; }
        public System.DateTime ENDDATE { get; set; }
        public int STAMP { get; set; }
    
        public virtual HKCITY HKCITY { get; set; }
        public virtual HKSTATE HKSTATE { get; set; }
        public virtual HKCTRY HKCTRY { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CSPRF> CSPRFs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CSQT> CSQTs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SFMYADDRCHG> SFMYADDRCHGs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SFSGADDRCHG> SFSGADDRCHGs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CSCNM> CSCNMs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CSDNM> CSDNM { get; set; }
        public virtual CSCOMSTR CSCOMSTR { get; set; }
    }
}