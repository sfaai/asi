using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using DCSoft.RTF;

namespace WebApplication1
{
    [MetadataType(typeof(CSCOMSTRMetadata))]
    public partial class CSCOMSTR
    {
        bool _filter = false;

        public bool filter {  get { return _filter; } set { _filter = value; } }

        public DateTime INCDATESTR { get { return this.INCDATE; } }

        public DateTime COSTATDSTR { get { return this.COSTATD; } }

        [Display(Name = "FS Reminder")]
        public Boolean SPECIALREBool { get { return this.SPECIALRE == "Y"; } set { this.SPECIALRE = value ? "Y" : "N"; } }

        [Display(Name = "AR Reminder")]
        public Boolean ARREBool { get { return this.ARRE == "Y"; } set { this.ARRE = value ? "Y" : "N"; } }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Primary Objective")]
        public string PRINOBJStr
        {
            get
            {

                string result = "";
                if (PRINOBJ != null)
                {
                    for (int i = 0; i < (PRINOBJ.Length); i++)
                    {

                        result = result + (char)PRINOBJ[i];
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
                    PRINOBJ = new byte[value.Length];
                    for (int i = 0; i < value.Length; i++)
                    {
                        PRINOBJ[i] = (byte)value[i];
                    }
                }
            }
        }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Comment")]
        public string CMMTStr
        {
            get
            {

                string result = "";
                if (CMMT != null)
                {
                    for (int i = 0; i < (CMMT.Length); i++)
                    {

                        result = result + (char)CMMT[i];
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
                    CMMT = new byte[value.Length];
                    for (int i = 0; i < value.Length; i++)
                    {
                        CMMT[i] = (byte)value[i];
                    }
                }
            }
        }
    }


    public class CSCOMSTRMetadata
    {
        [Display(Name = "Incorp Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public System.DateTime INCDATE { get; set; }

        [Display(Name = "Primary Objective")]
        public byte[] PRINOBJ { get; set; }

        [Display(Name = "Company #")]
        [MaxLength(10)]
        public string CONO { get; set; }

        [Display(Name = "Company Reg #")]
        [MaxLength(50)]
        public string COREGNO { get; set; }

        [Display(Name = "Country")]
        public string INCCTRY { get; set; }

        [Display(Name = "Constitution")]
        public string CONSTCODE { get; set; }

        [Display(Name = "In Type")]
        public string INTYPE { get; set; }

        [Display(Name = "Company Name")]
        [MaxLength(60)]
        public string CONAME { get; set; }

        [Display(Name = "Primary Industry")]
        [MaxLength(10,ErrorMessage = "Primary Industry Code must not exceed 10 characters")]
        public string PINDCODE { get; set; }

        [Display(Name = "Secondary Industry")]
        [MaxLength(10, ErrorMessage = "Secondary Industry Code must not exceed 10 characters")]
        public string SINDCODE { get; set; }

        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public string WEB { get; set; }

        [Display(Name = "Status")]
        public string COSTAT { get; set; }

        [Display(Name = "Status Since")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public System.DateTime COSTATD { get; set; }

        [Display(Name = "File Type")]
        public string FILETYPE { get; set; }


        [Display(Name = "File Location")]
        public string FILELOC { get; set; }


        [Display(Name = "Seal No.")]
        public string SEALLOC { get; set; }


        [Display(Name = "Staff")]
        public string STAFFCODE { get; set; }

        [Required(ErrorMessage = "Special Reminder FS is required")]
        [DataType("SelectYN")]
        [MaxLength(1)]
        [Display(Name = "FS Reminder")]
        public string SPECIALRE { get; set; }

        [Required(ErrorMessage = "AR Reminder is required")]
        [MaxLength(1)]
        [DataType("SelectYN")]
        [Display(Name = "AR Reminder")]
        public string ARRE { get; set; }

        [Display(Name = "Comment")]
        public byte[] CMMT { get; set; }


        [Display(Name = "Remarks")]
        public string REM { get; set; }


        [Display(Name = "SXCHG Code")]
        public string SXCODE { get; set; }

        [Display(Name = "SXCHG Name")]
        public string SXNAME { get; set; }


        [Display(Name = "Ref. Code")]
        public string REFCODE { get; set; }

        [Display(Name = "Seq No", Order = 12)]
        public int SEQNO { get; set; }

        [Display(Name = "Changed", Order = 12)]
        public int STAMP { get; set; }

    }
}