using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using DCSoft.RTF;

namespace WebApplication1
{
    [MetadataType(typeof(CSCODEBMetadata))]
    public partial class CSCODEB
    {

        [DataType(DataType.MultilineText)]
        [Display(Name = "Debenture Particulars")]
        public string DEBINFOStr
        {
            get
            {

                string result = "";
                if (DEBINFO != null)
                {
                    for (int i = 0; i < (DEBINFO.Length); i++)
                    {

                        result = result + (char)DEBINFO[i];
                    }
                }
                RTFDomDocument doc = new RTFDomDocument();
                doc.LoadRTFText(result);
                if (string.IsNullOrEmpty(doc.InnerText))
                {
                    return result;
                }
                return doc.InnerText;
            }

            set
            {
                if (value != null)
                {
                    DEBINFO = new byte[value.Length];
                    for (int i = 0; i < value.Length; i++)
                    {
                        DEBINFO[i] = (byte)value[i];
                    }
                }
            }
        }
    }

    public class CSCODEBMetadata
    {
        [Key]
        [Display(Name = "Company", Order = 12)]
        public string CONO { get; set; }

        [Display(Name = "Reference", Order = 12)]
        [MaxLength(20)]
        public string REFNO { get; set; }

        [Display(Name = "Trustee", Order = 12)]
        [MaxLength(10)]
        public string PRSCODE { get; set; }

        [Display(Name = "Ref Date", Order = 1)]
        [DataType(DataType.Date)]
        public System.DateTime REFDATE { get; set; }

        [Display(Name = "Deed Date", Order = 1)]
        [DataType(DataType.Date)]
        public System.DateTime DEEDDATE { get; set; }


        [Display(Name = "Secure Amount", Order = 12)]
        [MaxLength(60)]
        public string SECUREAMT { get; set; }

        [Display(Name = "Issue Amount", Order = 12)]
        [MaxLength(60)]
        public string ISSUEAMT { get; set; }

        [Display(Name = "Payout Condition", Order = 12)]
        [MaxLength(60)]
        public string DEBCOND { get; set; }

        [Display(Name = "Remarks", Order = 12)]
        [MaxLength(60)]
        public string REM { get; set; }

        [Display(Name = "Changed", Order = 12)]
        public int STAMP { get; set; }
    }
}







