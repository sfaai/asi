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
    
    public partial class CSCOMSTR
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CSCOMSTR()
        {
            this.CSCOPARENTs = new HashSet<CSCOPARENT>();
            this.CSGRPDs = new HashSet<CSGRPD>();
            this.CSJOBMs = new HashSet<CSJOBM>();
            this.CSQTs = new HashSet<CSQT>();
            this.SFMYADDRCHGs = new HashSet<SFMYADDRCHG>();
            this.SFMYNAMECHGs = new HashSet<SFMYNAMECHG>();
            this.SFMYSCMVs = new HashSet<SFMYSCMV>();
            this.SFSGADDRCHGs = new HashSet<SFSGADDRCHG>();
            this.SFSGDRMVs = new HashSet<SFSGDRMV>();
            this.SFSGNAMECHGs = new HashSet<SFSGNAMECHG>();
            this.SFSGSCMVs = new HashSet<SFSGSCMV>();
            this.SRSGIPOMs = new HashSet<SRSGIPOM>();
            this.SRSGPPMs = new HashSet<SRSGPPM>();
            this.SRSGRIMs = new HashSet<SRSGRIM>();
            this.CSBILLs = new HashSet<CSBILL>();
            this.CSPRFs = new HashSet<CSPRF>();
        }
    
        public string CONO { get; set; }
        public System.DateTime INCDATE { get; set; }
        public string INCCTRY { get; set; }
        public string CONSTCODE { get; set; }
        public string INTYPE { get; set; }
        public byte[] PRINOBJ { get; set; }
        public string CONAME { get; set; }
        public string PINDCODE { get; set; }
        public string SINDCODE { get; set; }
        public string WEB { get; set; }
        public string COSTAT { get; set; }
        public System.DateTime COSTATD { get; set; }
        public string FILETYPE { get; set; }
        public string FILELOC { get; set; }
        public string SEALLOC { get; set; }
        public string STAFFCODE { get; set; }
        public string SPECIALRE { get; set; }
        public byte[] CMMT { get; set; }
        public string REM { get; set; }
        public string SXCODE { get; set; }
        public string SXNAME { get; set; }
        public string REFCODE { get; set; }
        public int SEQNO { get; set; }
        public int STAMP { get; set; }
        public string ARRE { get; set; }
    
        public virtual HKCTRY HKCTRY { get; set; }
        public virtual HKCONST HKCONST { get; set; }
        public virtual HKSTAFF HKSTAFF { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CSCOPARENT> CSCOPARENTs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CSGRPD> CSGRPDs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CSJOBM> CSJOBMs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CSQT> CSQTs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SFMYADDRCHG> SFMYADDRCHGs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SFMYNAMECHG> SFMYNAMECHGs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SFMYSCMV> SFMYSCMVs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SFSGADDRCHG> SFSGADDRCHGs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SFSGDRMV> SFSGDRMVs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SFSGNAMECHG> SFSGNAMECHGs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SFSGSCMV> SFSGSCMVs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SRSGIPOM> SRSGIPOMs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SRSGPPM> SRSGPPMs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SRSGRIM> SRSGRIMs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CSBILL> CSBILLs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CSPRF> CSPRFs { get; set; }
    }
}
