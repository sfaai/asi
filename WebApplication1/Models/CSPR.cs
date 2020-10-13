﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1
{
    [MetadataType(typeof(CSPRMetadata))]
    public partial class CSPR
    {
        private List<SelectListItem> _gender= new List<SelectListItem>();

        private void makelist()
        {

            _gender.Add(new SelectListItem
            {
                Text = "Male",
                Value = "Male",
                Selected = true
            });

            _gender.Add(new SelectListItem
            {
                Text = "Female",
                Value = "Female",
                Selected = false
            });


        }

        public List<SelectListItem> gender
        {
            get
            {
                makelist();
                return this._gender;
            }
        }
    }


    public class CSPRMetadata
    {
        [Display(Name = "Birth Date")]
        [DataType(DataType.Date)]
        public System.DateTime DOB { get; set; }

        [Display(Name = "Entity Code")]
        public string PRSCODE { get; set; }

        [Display(Name = "Constitution")]
        public string CONSTCODE { get; set; }

        [Display(Name = "Name")]
        public string PRSNAME { get; set; }

        [Display(Name = "Title")]
        public string PRSTITLE { get; set; }

        [Display(Name = "Country")]
        public string NATION { get; set; }

        [Display(Name = "Race")]
        public string RACE { get; set; }

        [Display(Name = "Gender")]
        public string SEX { get; set; }

        [Display(Name = "Country of Incorporation")]
        public string CTRYINC { get; set; }

        [Display(Name = "Mobile 1")]
        [DataType(DataType.PhoneNumber)]
        public string MOBILE1 { get; set; }

        [Display(Name = "Mobile 2")]
        [DataType(DataType.PhoneNumber)]
        public string MOBILE2 { get; set; }

        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public string EMAIL { get; set; }

        [Display(Name = "Occupation")]
        public string OCCUPATION { get; set; }

        [Display(Name = "Remark")]
        public string REM { get; set; }

        [Display(Name = "Changed")]
        public string STAMP { get; set; }

    }
}