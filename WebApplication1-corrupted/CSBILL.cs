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
    
    public partial class CSBILL
    {
        public string BILLNO { get; set; }
        public System.DateTime ENTDATE { get; set; }
        public string CONO { get; set; }
        public string JOBNO { get; set; }
        public Nullable<int> CASENO { get; set; }
        public string CASECODE { get; set; }
        public string ITEMTYPE { get; set; }
        public string ITEMDESC { get; set; }
        public string ITEMSPEC { get; set; }
        public decimal TAXRATE { get; set; }
        public decimal ITEMAMT1 { get; set; }
        public decimal TAXAMT1 { get; set; }
        public decimal NETAMT1 { get; set; }
        public decimal ITEMAMT2 { get; set; }
        public decimal TAXAMT2 { get; set; }
        public decimal NETAMT2 { get; set; }
        public decimal ITEMAMT { get; set; }
        public decimal TAXAMT { get; set; }
        public decimal NETAMT { get; set; }
        public string SYSGEN { get; set; }
        public string PRFALLOC { get; set; }
        public string PRFNO { get; set; }
        public Nullable<int> PRFID { get; set; }
        public int STAMP { get; set; }
    
        public virtual CSITEM CSITEM { get; set; }
        public virtual CSCOMSTR CSCOMSTR { get; set; }
    }
}