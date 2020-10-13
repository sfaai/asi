using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using DCSoft.RTF;

namespace WebApplication1
{
    [MetadataType(typeof(CSCOCMMetadata))]
    public partial class CSCOCM
    {
       
        [DataType(DataType.MultilineText)]
        [Display(Name = "Charge/Mortgage Particulars")]
        public string CMINFOStr
        {
            get
            {

                string result = "";
                if (CMINFO != null)
                {
                    for (int i = 0; i < (CMINFO.Length); i++)
                    {

                        result = result + (char)CMINFO[i];
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
                    CMINFO = new byte[value.Length];
                    for (int i = 0; i < value.Length; i++)
                    {
                        CMINFO[i] = (byte)value[i];
                    }
                }
            }
        }
    }

    public class CSCOCMMetadata
    {
        [Key]
        [Display(Name = "Company", Order = 12)]
        public string CONO { get; set; }

        [Key]
        [Display(Name = "Reference", Order = 12)]
        [MaxLength(20)]
        public string REFNO { get; set; }

        [Display(Name = "Chargee Entity", Order = 12)]
        [MaxLength(10)]
        public string PRSCODE { get; set; }

        [Display(Name = "Addr Id", Order = 12)]
        public short ADDRID { get; set; }

        [Display(Name = "Ref Date", Order = 1)]
        [DataType(DataType.Date)]
        public System.DateTime REFDATE { get; set; }

        [Display(Name = "Created Date", Order = 1)]
        [DataType(DataType.Date)]
        public System.DateTime CMSDATE { get; set; }

        [Display(Name = "Satisfaction Date", Order = 1)]
        [DataType(DataType.Date)]
        public System.DateTime CMEDATE { get; set; }

        [Display(Name = "Nature of Charge/Mortgage", Order = 12)]
        [MaxLength(60)]
        public string CMNATURE { get; set; }

        [Display(Name = "Liability Secured", Order = 12)]
        [MaxLength(60)]
        public string LS { get; set; }

        [Display(Name = "Remarks", Order = 12)]
        [MaxLength(60)]
        public string REM { get; set; }

        [Display(Name = "Changed", Order = 12)]
        public int STAMP { get; set; }
    }
}




