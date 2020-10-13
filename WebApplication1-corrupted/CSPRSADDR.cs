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
    
    public partial class CSPRSADDR
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CSPRSADDR()
        {
            this.CSCOAGTCHGs = new HashSet<CSCOAGTCHG>();
            this.CSCOCMs = new HashSet<CSCOCM>();
            this.CSCODRCHGs = new HashSet<CSCODRCHG>();
            this.CSCOMGRCHGs = new HashSet<CSCOMGRCHG>();
            this.CSCOSECCHGs = new HashSet<CSCOSECCHG>();
            this.CSCOSHCHGs = new HashSet<CSCOSHCHG>();
            this.SFMYNEWDRs = new HashSet<SFMYNEWDR>();
            this.SFMYNEWMGs = new HashSet<SFMYNEWMG>();
            this.SFMYNEWSCs = new HashSet<SFMYNEWSC>();
            this.SFMYNEWSHes = new HashSet<SFMYNEWSH>();
            this.SFMYSCMVD1 = new HashSet<SFMYSCMVD1>();
            this.SFSGDRMVD1 = new HashSet<SFSGDRMVD1>();
            this.SFSGNEWDRs = new HashSet<SFSGNEWDR>();
            this.SFSGNEWMGs = new HashSet<SFSGNEWMG>();
            this.SFSGNEWSCs = new HashSet<SFSGNEWSC>();
            this.SFSGNEWSHes = new HashSet<SFSGNEWSH>();
            this.SFSGSCMVD1 = new HashSet<SFSGSCMVD1>();
        }
    
        public string PRSCODE { get; set; }
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
        public virtual ICollection<CSCOAGTCHG> CSCOAGTCHGs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CSCOCM> CSCOCMs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CSCODRCHG> CSCODRCHGs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CSCOMGRCHG> CSCOMGRCHGs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CSCOSECCHG> CSCOSECCHGs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CSCOSHCHG> CSCOSHCHGs { get; set; }
        public virtual HKCITY HKCITY { get; set; }
        public virtual HKSTATE HKSTATE { get; set; }
        public virtual HKCTRY HKCTRY { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SFMYNEWDR> SFMYNEWDRs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SFMYNEWMG> SFMYNEWMGs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SFMYNEWSC> SFMYNEWSCs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SFMYNEWSH> SFMYNEWSHes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SFMYSCMVD1> SFMYSCMVD1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SFSGDRMVD1> SFSGDRMVD1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SFSGNEWDR> SFSGNEWDRs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SFSGNEWMG> SFSGNEWMGs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SFSGNEWSC> SFSGNEWSCs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SFSGNEWSH> SFSGNEWSHes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SFSGSCMVD1> SFSGSCMVD1 { get; set; }
    }
}