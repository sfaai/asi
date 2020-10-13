using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1
{
    [MetadataType(typeof(CSEQMetadata))]
    public partial class CSEQ
    {
        private List<SelectListItem> _category = new List<SelectListItem>();

        private void makelist() 
        {
           
            _category.Add(new SelectListItem
            {
                Text = "Ordinary",
                Value = "Ordinary",
                Selected = true
            });

            _category.Add(new SelectListItem
            {
                Text = "Preference",
                Value = "Preference",
                Selected = false
            });

            _category.Add(new SelectListItem
            {
                Text = "Other",
                Value = "Other",
                Selected = false
            });
        }

        public List<SelectListItem> category { get {
                makelist();
                return this._category; } }
    }

    public class CSEQMetadata
    {
        [Key]
        [Display(Name = "Equity", Order = 12)]
        [MaxLength(10)]
        public string EQCODE { get; set; }

        [Display(Name = "Equity Description", Order = 12)]
        [MaxLength(60)]
        public string EQDESC { get; set; }

        [Display(Name = "Category", Order = 12)]
        [MaxLength(20)]
        public string EQCAT { get; set; }

        [Display(Name = "Changed", Order = 12)]
        public int STAMP { get; set; }
    }
}