using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1
{
    [MetadataType(typeof(CSRCPMetadata))]
    public partial class CSRCP
    {

        private ASIDBConnection db = new ASIDBConnection();
        //private ASIDBConnection db = null;
        private IQueryable<CSTRANM> _CSTRANM;

        public bool POSTBool { get { return (this.POST == "Y"); } set { this.POST = value ? "Y" : "N"; } }

        [Display(Name = "Archive", Order = 10)]
        public bool archived { get { return this.POST == "Y"; } }

        [Display(Name = "Details Used", Order = 10)]
        public int refcnt
        {
            get
            {
              
               
                _CSTRANM = db.CSTRANMs.Where(x => x.SOURCE == "CSRCP" && x.SOURCENO == this.TRNO);
                if (_CSTRANM.Count() == 0 ) { return 0;  }
                return _CSTRANM.Sum(x => x.REFCNT);
            }
        }

        [Display(Name = "CDetails Used", Order = 10)]
        public int refcntC
        {
            get
            {
                if (this.CTRNO == null) { return 0; }
                _CSTRANM = db.CSTRANMs.Where(x => x.SOURCE == "CSRCPC" && x.SOURCENO == this.CTRNO);
                if (_CSTRANM.Count() == 0) { return 0; }
                return _CSTRANM.Sum(x => x.REFCNT);
            }
        }

        [Display(Name = "Total Details", Order = 10)]
        public int detcnt
        {
            get
            {
              
                _CSTRANM = db.CSTRANMs.Where(x => x.SOURCE == "CSRCP" && x.SOURCENO == this.TRNO);
                return _CSTRANM.Count();
            }
        }

        [Display(Name = "Total CDetails", Order = 10)]
        public int detcntC
        {
            get
            {
                if (this.CTRNO == null) { return 0; }
                _CSTRANM = db.CSTRANMs.Where(x => x.SOURCE == "CSRCPC" && x.SOURCENO == this.CTRNO);
                return _CSTRANM.Count();
            }
        }

        [Display(Name = "Allocated Amount", Order = 10)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal AllocAmt
        {
            get
            {
             
                _CSTRANM = db.CSTRANMs.Where(x => x.SOURCE == "CSRCP" && x.SOURCENO == this.TRNO);
                if (_CSTRANM.Count() > 0)
                {
                    return _CSTRANM.Sum(y => y.TRAMT);
                }
                return 0;
            }
        }
    }

    public class CSRCPMetadata
    {
        [Display(Name = "OR #")]      
        [MaxLength(10)] 
        public string TRNO { get; set; }

        [Display(Name = "Date")]
        [DataType(DataType.Date)]
        public System.DateTime VDATE { get; set; }

        [Display(Name = "Company")]
        [MaxLength(10)]
        public string CONO { get; set; }

        [Display(Name = "Amount")]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal RCAMT { get; set; }

        [Display(Name = "Mode")]
        public string RCMODE { get; set; }

        [Display(Name = "Mode Map")]
        public string RCMAPCODE { get; set; }

        [Display(Name = "Issue Bank")]
        public string ISSBANK { get; set; }

        [Display(Name = "Issue Location")]
        public string ISSLOC { get; set; }

        [Display(Name = "Issue Ref")]
        public string ISSREFNO { get; set; }

        [Display(Name = "Issue Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(NullDisplayText = "", DataFormatString = "{0:dd-MM-yyyy}")]
        public Nullable<System.DateTime> ISSDATE { get; set; }

        [Display(Name = "Tax")]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal COMAMT { get; set; }

        [Display(Name = "Net Amount")]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal NETAMT { get; set; }

        [Display(Name = "Remarks")]
        [MaxLength(60)]
        public string REM { get; set; }

        public int SEQNO { get; set; }

        [Display(Name = "Posted")]
        public string POST { get; set; }

        [Display(Name = "CANCEL")]
        public string CFLAG { get; set; }

        [Display(Name = "CRef")]
        [MaxLength(10)]
        public string CTRNO { get; set; }

        [Display(Name = "Cancel Date")]
        [DataType(DataType.Date)]
        public Nullable<System.DateTime> CVDATE { get; set; }

        [Display(Name = "Cancel Remarks")]
        [MaxLength(60)]
        public string CREM { get; set; }

        public Nullable<int> CSEQNO { get; set; }
        public string CPOST { get; set; }
        public int STAMP { get; set; }

    }
}