using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1
{
    [MetadataType(typeof(CSPRFMetadata))]
    public partial class CSPRF
    {
        [Display(Name = "Billing Address")]
        [DataType(DataType.MultilineText)]
        public string COADDR
        {
            get
            {
                string result = this.CONO ?? "";
                result += "-" + this.COADDRID?.ToString();
                return (result == "-" ? null : result);
            }
            set
            {
                if ((this.CONO == null) && ((value != null) || (value.Length > 0)))
                {
                    string[] words = value.Split('-');
                    if (words.Length > 0)
                    {
                        this.CONO = words[0];
                        if (words.Length > 1)
                        {
                            if (words[1] != "")
                            {
                                this.COADDRID = (short)int.Parse(words[1]);
                            }
                            else this.COADDRID = null;
                        }
                        else { this.CONO = null; this.COADDRID = null; }
                    }
                }
            }
        }
    }

    public class CSPRFMetadata
    {
        [Display(Name ="PB Date")]
        [DataType(DataType.Date)]
        public System.DateTime VDATE { get; set; }

        [Display(Name = "Due Date")]
        [DataType(DataType.Date)]
        public System.DateTime DUEDATE { get; set; }
    }
}