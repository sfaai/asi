using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using DCSoft.RTF;

namespace WebApplication1
{
    [MetadataType(typeof(V_CSTRAN_OSTDMetadata))]
    public partial class V_CSTRAN_OSTD
    {

        //public string CONAME { get; set; }
        //public string CONO { get; set; }
        //public string COREGNO { get; set; }
        //public DateTime ENTDATE { get; set; }
        //public string PARTICULARS { get; set; }
        //public string PARTICULARS1 { get; set; }
        //public string PARTICULARS2 { get; set; }

        //[DataType(DataType.MultilineText)]
        //[Display(Name = "Particulars")]
        //public string PARTICULARS2Str
        //{
        //    get
        //    {

        //        string result = "";
        //        if (PARTICULARS2 != null)
        //        {
        //            for (int i = 0; i < (PARTICULARS2.Length); i++)
        //            {

        //                result = result + (char)PARTICULARS2[i];
        //            }
        //        }
        //        RTFDomDocument doc = new RTFDomDocument();
        //        doc.LoadRTFText(result);
        //        return doc.InnerText;
        //    }

        //    set
        //    {
        //        if (value != null)
        //        {
        //            PARTICULARS2 = new byte[value.Length];
        //            for (int i = 0; i < value.Length; i++)
        //            {
        //                PARTICULARS2[i] = (byte)value[i];
        //            }
        //        }
        //    }
        //}
        //public string SOURCE { get; set; }
        //public string SOURCENO { get; set; }
        //public decimal TRAMT { get; set; }
        //public decimal TROS { get; set; }
        //public decimal MAXAMT { get; set; }
        //public decimal MINAMT { get; set; }


    }

    public class V_CSTRAN_OSTDMetadata
    {
        [Display(Name = "Company Name", Order = 10)]
        public string CONAME { get; set; }

        [Display(Name = "Company", Order = 20)]
        public string CONO { get; set; }

        [Display(Name = "Company", Order = 30)]
        public string COREGNO { get; set; }


        [Display(Name = "Ent Date", Order = 40)]
        [DataType(DataType.Date)]
        public DateTime ENTDATE { get; set; }

        [Display(Name = "Particulars", Order = 50)]
        public string PARTICULARS { get; set; }

        [Display(Name = "Particulars 1", Order = 60)]
        public string PARTICULARS1 { get; set; }

        [Display(Name = "Particulars Full", Order = 70)]
        public string PARTICULARS2 { get; set; }

        [Display(Name = "Source", Order = 80)]
        public string SOURCE { get; set; }

        [Display(Name = "Reference", Order = 90)]
        public string SOURCENO { get; set; }

        [Display(Name = "Item Amount", Order = 120)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal TRAMT { get; set; }

        [Display(Name = "Ostd Amount", Order = 130)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal TROS { get; set; }

        [Display(Name = "Min Amount", Order = 140)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal MINAMT { get; set; }

        [Display(Name = "Max Amount", Order = 150)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal MAXAMT { get; set; }
    }
}