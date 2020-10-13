using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1
{
    [MetadataType(typeof(SACOMPMetadata))]
    public partial class SACOMP
    {
    }

    public class SACOMPMetadata
    {

        [Display(Name = "Company Name")]
        [MaxLength(60)]
        public string CONAME { get; set; }

        [Display(Name = "Registration No")]
        [MaxLength(10)]
        public string COREGNO { get; set; }

        [Display(Name = "Address")]
        [MaxLength(60)]
        public string COADDR1 { get; set; }

        [Display(Name = "Address")]
        [MaxLength(60)]
        public string COADDR2 { get; set; }

        [Display(Name = "Address")]
        [MaxLength(60)]
        public string COADDR3 { get; set; }

        [Display(Name = "Address")]
        [MaxLength(60)]
        public string COADDR4 { get; set; }

        [Display(Name = "Phone 1")]
        [MaxLength(15)]
        public string COPHONE1 { get; set; }

        [Display(Name = "Phone 2")]
        [MaxLength(15)]
        public string COPHONE2 { get; set; }

        [Display(Name = "Fax 1")]
        [MaxLength(15)]
        public string COFAX1 { get; set; }

        [Display(Name = "Fax 2")]
        [MaxLength(15)]
        public string COFAX2 { get; set; }

        [Display(Name = "Website")]
        [MaxLength(60)]
        public string COWEB { get; set; }

        [Display(Name = "Country")]
        [MaxLength(60)]
        public string CTRYOPR { get; set; }

        [Display(Name = "Acounting Method")]
        [MaxLength(10)]
        public string ACCMETHOD { get; set; }

        [Display(Name = "Changes")]     
        public int STAMP { get; set; }

    }
}