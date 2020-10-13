using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using WebApplication1.Utility;

namespace WebApplication1
{
    [MetadataType(typeof(CSJOBDMetadata))]
    public partial class CSJOBD
    {
        DateTime _DUEDATE;

        [Display(Name = "Stages", Order = 10)]
        public int Stages
        {
            get { return this.CSJOBSTs.Count(); }
        }

        [Display(Name = "Completed", Order = 10)]
        public bool COMPLETEBool
        {
            get { return this.COMPLETE == "Y"; }
            set { this.COMPLETE = value ? "Y" : "N"; }


        }

        [Display(Name = "End Date", Order = 20)]
        [DataType(DataType.Date)]
        public DateTime DUEDATE { get { return _DUEDATE; } set { _DUEDATE = value; } }

        [Display(Name = "Duration", Order = 20)]
        public int StageAge
        {
            get {
                if ((this.STAGE == "Complete") || (this.STAGE == "Cancel"))
                {
                    return 0;
                }
                else {
                    TimeSpan daydiff = DateTime.Today - this.STAGEDATE;
                    return daydiff.Days;
                }
            }
        } 
    }

    public class CSJOBDMetadata
    {
        [Display(Name = "JOB #", Order = 10)]
        public string JOBNO { get; set; }

        [Display(Name = "Case #", Order = 10)]
        public int CASENO { get; set; }

        [Display(Name = "Case Code", Order = 60)]
        public string CASECODE { get; set; }

        [Display(Name = "Particulars", Order = 90)]
        [DataType(DataType.MultilineText)]
        [MaxLength(2048)]
        public string CASEMEMO { get; set; }

        [Display(Name = "Remarks", Order = 90)]
        [DataType(DataType.MultilineText)]
        [MaxLength(60)]
        public string CASEREM { get; set; }

        [Display(Name = "Stage", Order = 70)]
        [MaxLength(30)]
        public string STAGE { get; set; }

        [Display(Name = "Stage Date", Order = 20)]
        [DataType(DataType.Date)]
        public System.DateTime STAGEDATE { get; set; }

        [Display(Name = "Stage Time", Order = 70)]
        [MaxLength(8)]
        public string STAGETIME { get; set; }

        [Display(Name = "completed", Order = 10)]
        [MaxLength(1)]
        public string COMPLETE { get; set; }

        [Display(Name = "Done Date", Order = 20)]
        [DataType(DataType.Date)]
        public System.DateTime COMPLETED { get; set; }
    }
}