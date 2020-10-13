using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1
{
    [MetadataType(typeof(CSSTATMetadata))]
    public partial class CSSTAT
    {
        private List<SelectListItem> _blanklist = new List<SelectListItem>();

        public CSSTAT()
        {
            _blanklist.Add(new SelectListItem
            {
                Text = "Yes",
                Value = "Y",
                Selected = true
            });

            _blanklist.Add(new SelectListItem
            {
                Text = "No",
                Value = "N",
                Selected = false
            });

            _blanklist.Add(new SelectListItem
            {
                Text = "Ignore",
                Value = "I",
                Selected = false
            });
        }

        public List<SelectListItem> blanklist { get { return this._blanklist; } }
    }

    public class CSSTATMetadata
    {
        [Display(Name = "Status", Order = 12)]
        [MaxLength(10)]
        public string COSTAT { get; set; }

        [Display(Name = "Blank File", Order = 12)]
        public string BLANKFILE { get; set; }

        [Display(Name = "File Prefix", Order = 12)]
        public string FILENOPFIX { get; set; }

        [Display(Name = "File From", Order = 12)]
        public int FILENOFR { get; set; }

        [Display(Name = "File To", Order = 12)]
        public int FILENOTO { get; set; }

        [Display(Name = "Blank Seal", Order = 12)]
        public string BLANKSEAL { get; set; }

        [Display(Name = "Seal From", Order = 12)]
        public int SEALNOFR { get; set; }

        [Display(Name = "Seal To", Order = 12)]
        public int SEALNOTO { get; set; }

        [Display(Name = "Changed", Order = 12)]
        public int STAMP { get; set; }
    }
}