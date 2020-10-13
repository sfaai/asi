using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1
{
    [MetadataType(typeof(CSPRSADDRMetadata))]
    public partial class CSPRSADDR
    {
        private List<SelectListItem> _addrtype = new List<SelectListItem>();

        private void makelist()
        {

            _addrtype.Add(new SelectListItem
            {
                Text = "Business",
                Value = "Business",
                Selected = true
            });

            _addrtype.Add(new SelectListItem
            {
                Text = "Other",
                Value = "Other",
                Selected = false
            });

            _addrtype.Add(new SelectListItem
            {
                Text = "Registered",
                Value = "Registered",
                Selected = false
            });

            _addrtype.Add(new SelectListItem
            {
                Text = "Resident",
                Value = "Resident",
                Selected = false
            });
        }

        public List<SelectListItem> addrtypeList
        {
            get
            {
                makelist();
                return this._addrtype;
            }
        }


        [Display(Name = "Mailing")]
        public Boolean MAILADDRBool { get { return this.MAILADDR == "Y"; } set { this.MAILADDR = value ? "Y" : "N"; } }

        [Display(Name = "Address")]
        public string ADDR { get { return this.ADDR1 + " " + this.ADDR2 + " " + this.ADDR3; }  }

        [Display(Name = "Phone")]
        public string PHONE { get { return this.PHONE1 + " " + this.PHONE2 ; } }

        [Display(Name = "Fax")]
        public string FAX { get { return this.FAX1 + " " + this.FAX2; } }
    }

    public class CSPRSADDRMetadata
    {
        [Display(Name = "Entity", Order = 12)]
        [MaxLength(10)]
        public string PRSCODE { get; set; }

        [Display(Name = "Row", Order = 12)]
        public short ADDRID { get; set; }

        [Display(Name = "Mailing", Order = 12)]
        public string MAILADDR { get; set; }

        [Display(Name = "Address Type", Order = 12)]
        [MaxLength(10)]
        public string ADDRTYPE { get; set; }

        [Display(Name = "Address", Order = 12)]
        [MaxLength(60)]
        public string ADDR1 { get; set; }

        [Display(Name = "Address", Order = 12)]
        [MaxLength(60)]
        public string ADDR2 { get; set; }

        [Display(Name = "Address", Order = 12)]
        [MaxLength(60)]
        public string ADDR3 { get; set; }

        [Display(Name = "Postcode", Order = 12)]
        [MaxLength(10)]
        public string POSTAL { get; set; }

        [Display(Name = "City", Order = 12)]
        [MaxLength(5)]
        public string CITYCODE { get; set; }

        [Display(Name = "State", Order = 12)]
        [MaxLength(5)]
        public string STATECODE { get; set; }

        [Display(Name = "Country", Order = 12)]
        [MaxLength(5)]
        public string CTRYCODE { get; set; }

        [Display(Name = "Phone", Order = 12)]
        [MaxLength(15)]
        public string PHONE1 { get; set; }

        [Display(Name = "Phone", Order = 12)]
        [MaxLength(15)]
        public string PHONE2 { get; set; }

        [Display(Name = "Fax", Order = 12)]
        [MaxLength(15)]
        public string FAX1 { get; set; }

        [Display(Name = "Fax", Order = 12)]
        [MaxLength(15)]
        public string FAX2 { get; set; }

        [Display(Name = "Remarks", Order = 12)]
        [MaxLength(60)]
        public string REM { get; set; }

        [Display(Name = "Changed", Order = 1)]
        public int STAMP { get; set; }
    }
}