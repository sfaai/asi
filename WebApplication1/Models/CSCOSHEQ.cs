using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1
{
    public class CSCOSHEQSPLITDENOM
    {
        private int _denomCnt = 0;
        private int _denomUnit = 0;

        [Display(Name = "Quantity", Order = 110)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N0}")]
        public int DenomCnt { get { return _denomCnt; } set { _denomCnt = value; } }

        [Display(Name = "Denomination", Order = 110)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N0}")]
        public int DenomUnit { get { return _denomUnit; } set { _denomUnit = value; } }

        [Display(Name = "Total", Order = 110)]
        [DisplayFormat( DataFormatString = "{0:N0}")]
        public int DenomTotal { get { return _denomUnit * _denomCnt; }  }
    }


    [MetadataType(typeof(CSCOSHEQMetadata))]

    public partial class CSCOSHEQ
    {
        private ASIDBConnection db = new ASIDBConnection();

        List<CSCOSHEQSPLITDENOM> _splitdenom = new List<CSCOSHEQSPLITDENOM>();
        Nullable<int> _folionoto;
        string _prscodeto;
        decimal _totalSplit;

        public CSCOSHEQ()
        {
            for (int i = 0; i<5; i++)
            {
                CSCOSHEQSPLITDENOM item = new CSCOSHEQSPLITDENOM();
                _splitdenom.Add(item);
            }
        }

        [Display(Name = "Transferee", Order = 12)]
        [MaxLength(10)]
        public string PRSCODETO { get { return _prscodeto; } set { _prscodeto = value; } }

        [Display(Name = "Folio To", Order = 12)]
        public Nullable<int> FOLIONOTO { get { return _folionoto; } set { _folionoto = value; } }


        [Display(Name = "Complete", Order = 12)]
        public bool COMPLETEBool { get { return (this.COMPLETE == "Y"); } set { this.COMPLETE = value ? "Y" : "N"; } }

        [Display(Name = "Script", Order = 12)]
        public bool SCRIPTBool { get { return (this.SCRIPT == "Y"); } set { this.SCRIPT = value ? "Y" : "N"; } }

        public string EQID {
            get
            {
                return this.EQCODE + " | " + this.SCRIPT + " | " + this.CERTNO;
            }
        }

        public IEnumerable<CSCOSHEQ> Details
        {
            get
            {
                return db.CSCOSHEQs.Where(x => x.MVNO == this.MVNO && x.CONO == this.CONO && x.MVID > 0);
            }
        }

        public List<CSCOSHEQSPLITDENOM> Split_Denom
        {
            get { return _splitdenom; }
            set { _splitdenom = value;  }
        }

        public decimal TotalSplit { get { return _totalSplit; } set { _totalSplit = value; } }

        public int MRefCnt
        {

            get {
                if (this.MVID == 0) {
                    try
                    {
                        return this.Details.Sum(x => x.REFCNT) + this.REFCNT;
                    } catch { return 0; }
                }
                else
                {
                    return this.REFCNT;
                }
            }
        }
    }

    public class CSCOSHEQMetadata
    {
        [Key]
        [Display(Name = "Company", Order = 12)]
        public string CONO { get; set; }


        [Display(Name = "Shareholder", Order = 12)]
        [MaxLength(10)]
        public string PRSCODE { get; set; }

        [Display(Name = "Tran #", Order = 12)]
        public string MVNO { get; set; }

        [Display(Name = "Id", Order = 12)]
        public int MVID { get; set; }

        [Display(Name = "Movement", Order = 12)]
        [MaxLength(10)]
        public string MVTYPE { get; set; }

        [Display(Name = "Remarks", Order = 12)]
        [MaxLength(60)]
        public string REM { get; set; }

        [Display(Name = "Folio Source", Order = 12)]
        public Nullable<int> FOLIONOSRC { get; set; }

        [Display(Name = "Complete", Order = 12)]
        [MaxLength(1)]
        public string COMPLETE { get; set; }

        [Display(Name = "Completed Date", Order = 1)]
        [DataType(DataType.Date)]
        public DateTime COMPLETED { get; set; }

        [Display(Name = "Changed", Order = 12)]
        public int STAMP { get; set; }

        [Display(Name = "Used", Order = 12)]
        public int REFCNT { get; set; }



        [Display(Name = "Tran Date", Order = 1)]
        [DataType(DataType.Date)]
        public System.DateTime MVDATE { get; set; }

        [Display(Name = "In/Out", Order = 12)]
        [MaxLength(5)]
        public string MVSIGN { get; set; }

        [Display(Name = "Equity", Order = 12)]
        [MaxLength(10)]
        public string EQCODE { get; set; }

        [Display(Name = "Script", Order = 12)]
        [MaxLength(1)]
        public string SCRIPT { get; set; }

        [Display(Name = "Cert No", Order = 12)]
        [MaxLength(10)]
        public string CERTNO { get; set; }

        [Display(Name = "No of Shares", Order = 110)]
        [DisplayFormat( DataFormatString = "{0:N0}")]
        public decimal SHAREAMT { get; set; }

        [Display(Name = "No of Shares", Order = 110)]
        [DisplayFormat( DataFormatString = "{0:N0}")]
        public decimal SSHAREAMT { get; set; }

        [Display(Name = "Shares Bal", Order = 110)]
        [DisplayFormat( DataFormatString = "{0:N0}")]
        public decimal SHAREOS { get; set; }

        [Display(Name = "Consideration", Order = 110)]
        [DisplayFormat( DataFormatString = "{0:N2}")]
        public decimal AMT { get; set; }

        [Display(Name = "Consideration", Order = 110)]
        [DisplayFormat( DataFormatString = "{0:N2}")]
        public decimal SAMT { get; set; }




    }
}