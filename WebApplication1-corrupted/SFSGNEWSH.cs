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
    
    public partial class SFSGNEWSH
    {
        public string TRNO { get; set; }
        public string PRSCODE { get; set; }
        public string NATION { get; set; }
        public string RACE { get; set; }
        public short ADDRID { get; set; }
        public string OCCUPATION { get; set; }
        public string REGCTRY1 { get; set; }
        public string REGTYPE1 { get; set; }
        public string REGID1 { get; set; }
        public string REGCTRY2 { get; set; }
        public string REGTYPE2 { get; set; }
        public string REGID2 { get; set; }
        public string CHGREM { get; set; }
        public int FOLIONO { get; set; }
        public int STAMP { get; set; }
    
        public virtual CSPR CSPR { get; set; }
        public virtual CSPRSADDR CSPRSADDR { get; set; }
        public virtual CSPRSREG CSPRSREG { get; set; }
        public virtual CSPRSREG CSPRSREG1 { get; set; }
        public virtual HKNATION HKNATION { get; set; }
        public virtual HKRACE HKRACE { get; set; }
    }
}
