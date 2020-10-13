using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1
{
    [MetadataType(typeof(CSJOBMMetadata))]
    public partial class CSJOBM
    {
        DateTime _DUEDATE;

        [Display(Name = "posted", Order = 10)]
        bool JOBPOSTBool { get { return this.JOBPOST == "Y"; }  set { this.JOBPOST = value ? "Y" : "N"; } }
        [Display(Name = "completed", Order = 10)]
        bool COMPLETEBool { get { return this.COMPLETE == "Y"; } set { this.COMPLETE = value ? "Y" : "N"; } }

        [Display(Name = "End Date", Order = 20)]
        [DataType(DataType.Date)]
        public DateTime DUEDATE { get { return _DUEDATE;} set { _DUEDATE = value; } }

        [Display(Name = "Job MEMO", Order = 10)]
        [DataType(DataType.MultilineText)]
        public string JobMEMO
        {
            
            get
            {
                string JobDescStr = "";
                if (this.CSJOBDs != null)
                {
                    foreach (CSJOBD item in this.CSJOBDs)
                    {
                        if (!string.IsNullOrEmpty(JobDescStr)) {
                            JobDescStr = JobDescStr + "\n";
                        }
                        JobDescStr = JobDescStr + item.CASEMEMO;
                    }
                }
                return JobDescStr;
            }
        }

        [Display(Name = "Job Remarks", Order = 10)]
        [DataType(DataType.MultilineText)]
        public string JobREM
        {

            get
            {
                string JobDescStr = "";
                if (this.CSJOBDs != null)
                {
                    foreach (CSJOBD item in this.CSJOBDs)
                    {
                        if (!string.IsNullOrEmpty(JobDescStr))
                        {
                            JobDescStr = JobDescStr + "\n";
                        }
                        JobDescStr = JobDescStr + item.CASEREM;
                    }
                }
                return JobDescStr;
            }
        }

        [Display(Name = "Job CASE", Order = 10)]
        [DataType(DataType.MultilineText)]
        public string JobCASE
        {

            get
            {
                string JobDescStr = "";
                if (this.CSJOBDs != null)
                {
                    foreach (CSJOBD item in this.CSJOBDs)
                    {
                        if (!string.IsNullOrEmpty(JobDescStr))
                        {
                            JobDescStr = JobDescStr + ", ";
                        }
                        JobDescStr = JobDescStr + item.CASECODE;
                    }
                }
                return JobDescStr;
            }
        }
    }

    public class CSJOBMMetadata
    {
        [Display(Name = "Job #", Order = 10)]
        [MaxLength(10)]
        public string JOBNO { get; set; }

        [Display(Name = "Date", Order = 20)]
        [DataType(DataType.Date)]
        public System.DateTime VDATE { get; set; }

        [Display(Name = "Company #", Order = 10)]
        [MaxLength(10)]
        public string CONO { get; set; }

        [Display(Name = "Remarks", Order = 10)]
        [MaxLength(60)]
        public string REM { get; set; }

        [Display(Name = "Staff Code", Order = 10)]
        [MaxLength(5)]
        public string JOBSTAFF { get; set; }

        [Display(Name = "posted", Order = 10)]
        [MaxLength(1)]
        public string JOBPOST { get; set; }

        [Display(Name = "cases", Order = 10)]
        public int CASECNT { get; set; }

        [Display(Name = "dones", Order = 10)]
        public int OKCNT { get; set; }

        [Display(Name = "completed", Order = 10)]
        [MaxLength(1)]
        public string COMPLETE { get; set; }

    }
}