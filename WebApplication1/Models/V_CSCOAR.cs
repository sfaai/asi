using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1
{
    [MetadataType(typeof(V_CSCOARMetadata))]
    public partial class V_CSCOAR
    {
        private ASIDBConnection db = new ASIDBConnection();

        private int pReminderMonth = 0;
        private int pReminderYear = 0;


        [Display(Name = "Email")]
        public string Email
        {
            get
            {

                return db.CSCOMSTRs.Where(x => x.CONO == this.CONO).Select(x => x.WEB).FirstOrDefault();
            }
        }

        [Display(Name = "Special", Order = 10)]
        public bool REMINDER2Bool { get { return this.REMINDER2 == "Y"; } set { this.REMINDER2 = value ? "Y" : "N"; } }

        public int ReminderMonth { get { return this.pReminderMonth; } set { this.pReminderMonth = value; } }
        public int ReminderYear { get { return this.pReminderYear; } set { this.pReminderYear = value; } }

        [Display(Name = "Reminder Type", Order = 10)]
        public string CReminderType
        {
            get
            {
                if ((pReminderYear == 0) || (pReminderMonth == 0)) { return ""; }
                DateTime startDate = new DateTime(pReminderYear, ReminderMonth, 1);
                DateTime endDate = new DateTime(pReminderYear, ReminderMonth, 1).AddMonths(1).AddDays(-1);

                string remType = "";
                if (REMINDER2 == "Y") { remType = "Special"; }
                else if ((REMINDER1 >= startDate) && (REMINDER1 <= endDate)) { remType = "Reminder"; }


                return remType;
            }
        }

        [Display(Name = "Reminder Date", Order = 15)]
        [DataType(DataType.Date)]
        public DateTime? ReminderDate
        {
            get
            {
                if ((pReminderYear == 0) || (pReminderMonth == 0)) { return null; }
                return new DateTime(pReminderYear, pReminderMonth, 1);
                ;
            }
        }
    }

    public class V_CSCOARMetadata
    {
        [Display(Name = "Company Name", Order = 10)]
        public string CONAME { get; set; }

        [Display(Name = "Company", Order = 20)]
        public string CONO { get; set; }

        [Display(Name = "Company", Order = 20)]
        public string COREGNO { get; set; }

        [Display(Name = "AR NO", Order = 30)]
        public short ARNO { get; set; }

        [Display(Name = "Last AR", Order = 40)]
        [DataType(DataType.Date)]
        public DateTime LASTAR { get; set; }


        [Display(Name = "AR To File", Order = 50)]
        [DataType(DataType.Date)]
        public DateTime ARTOFILE { get; set; }

        [Display(Name = "Filed AR", Order = 50)]
        [DataType(DataType.Date)]
        public DateTime FILEDAR { get; set; }

        [Display(Name = "Reminder", Order = 60)]
        [DataType(DataType.Date)]
        public DateTime REMINDER1 { get; set; }

        [Display(Name = "Special", Order = 70)]
        public String REMINDER2 { get; set; }

        [Display(Name = "Submit AR", Order = 80)]
        [DataType(DataType.Date)]
        public DateTime SUBMITAR { get; set; }

        [Display(Name = "Remarks", Order = 90)]
        public String REM { get; set; }

        [Display(Name = "Staff", Order = 100)]
        public String STAFFCODE { get; set; }


        [Display(Name = "Staff Name", Order = 110)]
        public String STAFFDESC { get; set; }
    }
}