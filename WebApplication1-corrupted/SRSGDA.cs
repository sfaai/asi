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
    
    public partial class SRSGDA
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SRSGDA()
        {
            this.SRSGIPODBFs = new HashSet<SRSGIPODBF>();
            this.SRSGIPODPFs = new HashSet<SRSGIPODPF>();
            this.SRSGIPODWFs = new HashSet<SRSGIPODWF>();
            this.SRSGPPDs = new HashSet<SRSGPPD>();
        }
    
        public string DACODE { get; set; }
        public string DANAME { get; set; }
        public int STAMP { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SRSGIPODBF> SRSGIPODBFs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SRSGIPODPF> SRSGIPODPFs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SRSGIPODWF> SRSGIPODWFs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SRSGPPD> SRSGPPDs { get; set; }
    }
}