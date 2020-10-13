using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1
{
    [MetadataType(typeof(CSCOAGMMetadata))]
    public partial class CSCOAGM
    {
    }

    public class CSCOAGMMetadata
    {
        [Display(Name = "Last FS Filed", GroupName = "History", Order = 10)]
        [DataType(DataType.Date)]
        public System.DateTime LASTFYE { get; set; }

        [Display(Name = "FYE Due", GroupName = "Proposed", Order = 20)]
        [DataType(DataType.Date)]
        public System.DateTime FYETOFILE { get; set; }



        [Display(Name = "AGM Due", GroupName = "Proposed", Order = 30)]
        [DataType(DataType.Date)]
        public System.DateTime AGMLAST { get; set; }

        [Display(Name = "AGM/FS Extension", GroupName = "Proposed", Order = 40)]
        [DataType(DataType.Date)]
        public System.DateTime AGMEXT { get; set; }

        [Display(Name = "Reminder 1", GroupName = "Proposed", Order = 50)]
        [DataType(DataType.Date)]
        public System.DateTime REMINDER1 { get; set; }


        [Display(Name = "Reminder 2", GroupName = "Proposed", Order = 60)]
        [DataType(DataType.Date)]
        public System.DateTime REMINDER2 { get; set; }


        [Display(Name = "Reminder Final", GroupName = "Proposed", Order = 70)]
        [DataType(DataType.Date)]
        public System.DateTime REMINDER3 { get; set; }

        [Display(Name = "FS Filed", GroupName = "Confirmed", Order = 120)]
        [DataType(DataType.Date)]
        public System.DateTime FILEDFYE { get; set; }

        [Display(Name = "Circulation Date", GroupName = "Confirmed", Order = 130)]
        [DataType(DataType.Date)]
        [CheckAfterDate("FYETOFILE",ErrorMessage = "Must be after FYE due")]
        public System.DateTime CIRCDATE { get; set; }

        [Display(Name = "AGM Held", GroupName = "Confirmed", Order = 140)]
        [DataType(DataType.Date)]
        public System.DateTime AGMDATE { get; set; }

        [Display(Name = "AR Filed", GroupName = "Confirmed", Order = 150)]
        [DataType(DataType.Date)]
        public System.DateTime AGMFILED { get; set; }

       // [CombinedMinLength(20, "REM", ErrorMessage = "The custom validation test properties should be longer than 20")]

        [Display(Name = "Remarks", GroupName = "Summary", Order = 520)]
        public string REM { get; set; }

        [Display(Name = "AGM No", GroupName = "History", Order = 5)]
        public int AGMNO { get; set; }
    }

    public class CheckAfterDateAttribute : ValidationAttribute
    {
        public CheckAfterDateAttribute(params string[] propertyNames)
        {
            this.PropertyNames = propertyNames;
        }

        public string[] PropertyNames { get; private set; }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null) { return null; };

            var properties = this.PropertyNames.Select(validationContext.ObjectType.GetProperty);
            var values = properties.Select(p => p.GetValue(validationContext.ObjectInstance, null)).OfType<DateTime>();
            DateTime useDate = values.Max(x => x);
            if ((DateTime)value < useDate)
            {
                return new ValidationResult(this.FormatErrorMessage(validationContext.DisplayName));
            }
            return null;
        }
    }

    public class CombinedMinLengthAttribute : ValidationAttribute
    {
        public CombinedMinLengthAttribute(int minLength, params string[] propertyNames)
        {
            this.PropertyNames = propertyNames;
            this.MinLength = minLength;
        }

        public string[] PropertyNames { get; private set; }
        public int MinLength { get; private set; }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var properties = this.PropertyNames.Select(validationContext.ObjectType.GetProperty);
            var values = properties.Select(p => p.GetValue(validationContext.ObjectInstance, null)).OfType<string>();
            var totalLength = values.Sum(x => x.Length) + Convert.ToString(value).Length;
            if (totalLength < this.MinLength)
            {
                return new ValidationResult(this.FormatErrorMessage(validationContext.DisplayName));
            }
            return null;
        }
    }
}