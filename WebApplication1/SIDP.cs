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
    
    public partial class SIDP
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SIDP()
        {
            this.SISTFSTMVs = new HashSet<SISTFSTMV>();
        }
    
        public string DPCODE { get; set; }
        public string DPDESC { get; set; }
        public int DPLEVEL { get; set; }
        public string DPMSTR { get; set; }
        public string DPUNITCODE { get; set; }
        public int STAMP { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SISTFSTMV> SISTFSTMVs { get; set; }
    }
}