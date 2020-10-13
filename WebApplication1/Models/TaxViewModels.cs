using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data.SqlClient;
using FirebirdSql.Data.FirebirdClient;
using System.Data.Entity;
using System.Text.RegularExpressions;

namespace WebApplication1
{
    public class TaxViewModels
    {
    }


    public class TaxRecord
    {
        private string _cono;
        private string _coregno;
        private string _coname;
        private DateTime _entdate;
        private string _source;
        private decimal _amt;
        private decimal _tax;
        private int _cnt;
        private decimal _rate;
        DateTime _DUEDATE;


        [Display(Name = "End Date", Order = 20)]
        [DataType(DataType.Date)]
        public DateTime DUEDATE { get { return _DUEDATE; } set { _DUEDATE = value; } }

        [Display(Name = "Company #", Order = 20)]
        public string cono { get { return _cono; } set { _cono = value; } }

        [Display(Name = "Company #", Order = 20)]
        public string coregno { get { return _coregno; } set { _coregno = value; } }

        [Display(Name = "Company Name", Order = 20)]
        public string coname { get { return _coname; } set { _coname = value; } }

        [Display(Name = "Source", Order = 20)]
        public string source { get { return _source; } set { _source = value; } }

        [Display(Name = "Date", Order = 20)]
        [DataType(DataType.Date)]
        public DateTime entdate { get { return _entdate; } set { _entdate = value; } }


        public int cnt { get { return _cnt; } set { _cnt = value; } }

        [Display(Name = "Amount", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal amt { get { return _amt; } set { _amt = value; } }

        [Display(Name = "Tax", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal tax { get { return _tax; } set { _tax = value; } }

        [Display(Name = "Tax Rate %", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal rate
        {
            get
            {
                if ((this._amt == 0) || (this._tax == 0))
                {
                    _rate = 0;
                }
                else
                {
                    _rate = _tax * 100 / _amt;
                }
                return _rate;
            }
            set { _rate = value; }
        }
    }



}





