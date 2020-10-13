using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1
{
    [MetadataType(typeof(CSTRANMMetadata))]
    public partial class CSTRANM
    {


        [Display(Name = "Link", Order = 10)]
        public string LinkRef
        {
            get
            {

               
                    if (CSTRANDs != null)
                    {
                        string line = "";
                        string separator = "";
                        CSTRAND item = CSTRANDs;
                      
                          
                            line = separator + item.DBTYPE + "|" + item.DBNO + "|" + item.DBID;
                       
                        return line;
                    }
              
                return "";
            }
        }
    }

    public class CSTRANMMetadata
    {
        [Display(Name = "Source", Order = 10)]
        public string SOURCE { get; set; }

        [Display(Name = "Ref", Order = 10)]
        public string SOURCENO { get; set; }


        [Display(Name = "#", Order = 10)]
        public string SOURCEID { get; set; }

        [Display(Name = "Item Date", Order = 20)]
        [DataType(DataType.Date)]
        public DateTime ENTDATE { get; set; }

        [Display(Name = "Due Date", Order = 20)]
        [DataType(DataType.Date)]
        public DateTime DUEDATE { get; set; }

        [Display(Name = "Company", Order = 30)]
        public string CONO { get; set; }

        [Display(Name = "Job No", Order = 40)]
        public string JOBNO { get; set; }

        [Display(Name = "Case No", Order = 50)]
        public int CASENO { get; set; }

        [Display(Name = "Case", Order = 60)]
        public string CASECODE { get; set; }

        [Display(Name = "Item Type", Order = 70)]
        public string TRTYPE { get; set; }

        [Display(Name = "Item Description", Order = 80)]
        [MaxLength(60)]
        public string TRDESC { get; set; }

        [Display(Name = "Remark", Order = 80)]
        [MaxLength(60)]
        public string REM { get; set; }

        [Display(Name = "Used", Order = 80)]
        public int REFCNT { get; set; }

        [Display(Name = "Own Amount", Order = 110)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal TRITEM1 { get; set; }

        [Display(Name = "Own Tax", Order = 120)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal TRTAX1 { get; set; }

        [Display(Name = "Own Net", Order = 130)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal TRAMT1 { get; set; }

        [Display(Name = "3P Amount", Order = 210)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal TRITEM2 { get; set; }

        [Display(Name = "3P Tax", Order = 220)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal TRTAX2 { get; set; }

        [Display(Name = "3P Net", Order = 230)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal TRAMT2 { get; set; }

        [Display(Name = "Total Amount", Order = 310)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal TRITEM { get; set; }

        [Display(Name = "Total Tax", Order = 320)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal TRTAX { get; set; }

        [Display(Name = "Total Net", Order = 330)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal TRAMT { get; set; }


        [Display(Name = "Own Amount", Order = 110)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal TRITEMOS1 { get; set; }

        [Display(Name = "Own Tax", Order = 120)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal TRTAXOS1 { get; set; }

        [Display(Name = "Own Total", Order = 130)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal TROS1 { get; set; }

        [Display(Name = "3P Amount", Order = 210)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal TRITEMOS2 { get; set; }

        [Display(Name = "3P Tax", Order = 220)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal TRTAXOS2 { get; set; }

        [Display(Name = "3P Total", Order = 230)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal TROS2 { get; set; }

        [Display(Name = "Item Amount", Order = 310)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal TRITEMOS { get; set; }

        [Display(Name = "Item Tax", Order = 320)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal TRTAXOS { get; set; }

        [Display(Name = "Item Total", Order = 330)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal TROS { get; set; }

        [Display(Name = "Own Amount", Order = 110)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal APPITEM1 { get; set; }

        [Display(Name = "Own Tax", Order = 120)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal APPTAX1 { get; set; }

        [Display(Name = "Own Total", Order = 130)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal APPAMT1 { get; set; }

        [Display(Name = "3P Amount", Order = 210)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal APPITEM2 { get; set; }

        [Display(Name = "3P Tax", Order = 220)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal APPTAX2 { get; set; }

        [Display(Name = "3P Total", Order = 230)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal APPAMT2 { get; set; }

        [Display(Name = "Item Amount", Order = 310)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal APPITEM { get; set; }

        [Display(Name = "Item Tax", Order = 320)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal APPTAX { get; set; }

        [Display(Name = "Item Total", Order = 330)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal APPAMT { get; set; }

    }
}