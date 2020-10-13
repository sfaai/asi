using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1
{
    [MetadataType(typeof(V_CSCOAGMMetadata))]
    public partial class V_CSCOAGM
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
        public bool REMINDER4Bool { get { return this.REMINDER4 == "Y"; } set { this.REMINDER4 = value ? "Y" : "N"; } } 

        public int ReminderMonth { get { return this.pReminderMonth; } set { this.pReminderMonth = value; } }
        public int ReminderYear { get { return this.pReminderYear; } set { this.pReminderYear = value; } }

        [Display(Name = "Reminder Type", Order = 10)]
        public string CReminderType { get {
                if ((pReminderYear == 0) || (pReminderMonth == 0)) { return ""; }
                DateTime startDate = new DateTime(pReminderYear, ReminderMonth, 1);
                DateTime endDate  = new DateTime(pReminderYear, ReminderMonth, 1).AddMonths(1).AddDays(-1);


                string remType = "";
                if (REMINDER4 == "Y") { remType = "Special"; }
                else if ((REMINDER1 >= startDate) && (REMINDER1 <= endDate)) { remType = "First"; }
                else if ((REMINDER2 >= startDate) && (REMINDER2 <= endDate)) { remType = "Second"; }
                else if ((REMINDER3 >= startDate) && (REMINDER3 <= endDate)) { remType = "Final"; }
                
                return remType;
            } }

        [Display(Name = "Reminder Date", Order = 15)]
        [DataType(DataType.Date)]
        public DateTime? ReminderDate { get {
                if ((pReminderYear == 0) || (pReminderMonth == 0)) { return null; }
                return new DateTime(pReminderYear, pReminderMonth, 1);
                ; } }
    }

    public class V_CSCOAGMMetadata
    {
        [Display(Name = "Company Name", Order = 10)]
        public string CONAME { get; set; }

        [Display(Name = "Company", Order = 20)]
        public string CONO { get; set; }

        [Display(Name = "Company", Order = 20)]
        public string COREGNO { get; set; }

        [Display(Name = "AGM", Order = 30)]
        public short AGMNO { get; set; }

        [Display(Name = "Last FYE", Order = 40)]
        [DataType(DataType.Date)]
        public DateTime LASTFYE { get; set; }


        [Display(Name = "FYE To File", Order = 50)]
        [DataType(DataType.Date)]
        public DateTime FYETOFILE { get; set; }

        [Display(Name = "Filed FYE", Order = 60)]
        [DataType(DataType.Date)]
        public DateTime FILEDFYE { get; set; }

        [Display(Name = "Last AGM", Order = 70)]
        [DataType(DataType.Date)]
        public DateTime AGMLAST { get; set; }

        [Display(Name = "AGM Extension", Order = 80)]
        [DataType(DataType.Date)]
        public DateTime AGMEXT { get; set; }

        [Display(Name = "Reminder 1", Order = 90)]
        [DataType(DataType.Date)]
        public DateTime REMINDER1 { get; set; }


        [Display(Name = "Reminder 2", Order = 100)]
        [DataType(DataType.Date)]
        public DateTime REMINDER2 { get; set; }


        [Display(Name = "Reminder Final", Order = 110)]
        [DataType(DataType.Date)]
        public DateTime REMINDER3 { get; set; }


        [Display(Name = "Special", Order = 120)]
        public String REMINDER4 { get; set; }

        [Display(Name = "AGM Date", Order = 130)]
        [DataType(DataType.Date)]
        public DateTime AGMDATE { get; set; }

        [Display(Name = "AGM Filed", Order = 140)]
        [DataType(DataType.Date)]
        public DateTime AGMFILED { get; set; }

        [Display(Name = "Circulation Date", Order = 150)]
        [DataType(DataType.Date)]
        public DateTime CIRCDATE { get; set; }

        [Display(Name = "Remarks", Order = 160)]
        public String REM { get; set; }

        [Display(Name = "Staff", Order = 170)]
        public String STAFFCODE { get; set; }


        [Display(Name = "Staff Name", Order = 170)]
        public String STAFFDESC { get; set; }
    }
}