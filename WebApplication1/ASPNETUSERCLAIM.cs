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
    
    public partial class ASPNETUSERCLAIM
    {
        public int ID { get; set; }
        public string USERID { get; set; }
        public string CLAIMTYPE { get; set; }
        public string CLAIMVALUE { get; set; }
    
        public virtual ASPNETUSER ASPNETUSER { get; set; }
    }
}
