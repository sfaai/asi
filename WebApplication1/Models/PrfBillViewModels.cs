using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data.SqlClient;
using FirebirdSql.Data.FirebirdClient;
using System.Data.Entity;
using System.Text.RegularExpressions;

namespace WebApplication1
{
    public class PrfBillAmountType
    {
        List<SelectListItem> listItems = new List<SelectListItem>();
        private string _amountType;

        List<SelectListItem> listYear = new List<SelectListItem>();
        private string _year;

        List<SelectListItem> listMonth = new List<SelectListItem>();
        private string _month;

        List<SelectListItem> listType = new List<SelectListItem>();
        private string _type;

        List<SelectListItem> listCategory = new List<SelectListItem>();
        private string _category;

        public PrfBillAmountType()
        {
            listCategory.Add(new SelectListItem
            {
                Text = "by Case Code",
                Value = "CASECODE",
                Selected = false
            });

            listCategory.Add(new SelectListItem
            {
                Text = "by Item Type",
                Value = "ITEMTYPE",
                Selected = true
            });


            listType.Add(new SelectListItem
            {
                Text = "by Period",
                Value = "MONTH",
                Selected = true
            });

            listType.Add(new SelectListItem
            {
                Text = "by Staff Job",
                Value = "STAFF",
                Selected = false
            });

            listType.Add(new SelectListItem
            {
                Text = "by Staff Portfolio",
                Value = "PORTFOLIO",
                Selected = false
            });

            listType.Add(new SelectListItem
            {
                Text = "by Staff Combined",
                Value = "COMBO",
                Selected = false
            });

            listMonth.Add(new SelectListItem
            {
                Text = "January",
                Value = "1",
                Selected = false
            });

            listMonth.Add(new SelectListItem
            {
                Text = "February",
                Value = "2",
                Selected = false
            });
            listMonth.Add(new SelectListItem
            {
                Text = "March",
                Value = "3",
                Selected = false
            });

            listMonth.Add(new SelectListItem
            {
                Text = "April",
                Value = "4",
                Selected = false
            });

            listMonth.Add(new SelectListItem
            {
                Text = "May",
                Value = "5",
                Selected = false
            });

            listMonth.Add(new SelectListItem
            {
                Text = "June",
                Value = "6",
                Selected = false
            });


            listMonth.Add(new SelectListItem
            {
                Text = "July",
                Value = "7",
                Selected = false
            });

            listMonth.Add(new SelectListItem
            {
                Text = "August",
                Value = "8",
                Selected = false
            });

            listMonth.Add(new SelectListItem
            {
                Text = "September",
                Value = "9",
                Selected = false
            });

             listMonth.Add(new SelectListItem
            {
                Text = "October",
                Value = "10",
                Selected = false
            });

            listMonth.Add(new SelectListItem
            {
                Text = "November",
                Value = "11",
                Selected = false
            });
            listItems.Add(new SelectListItem
            {
                Text = "Amount",
                Value = "AMOUNT",
                Selected = true
            });

            listMonth.Add(new SelectListItem
            {
                Text = "December",
                Value = "12",
                Selected = false
            });

            listItems.Add(new SelectListItem
            {
                Text = "Tax",
                Value = "TAX",
                Selected = false
            });

            listItems.Add(new SelectListItem
            {
                Text = "Net Amount",
                Value = "NET",
                Selected = false
            });

            listItems.Add(new SelectListItem
            {
                Text = "3rd Party Total",
                Value = "OTHER",
                Selected = false
            });

            listItems.Add(new SelectListItem
            {
                Text = "Total",
                Value = "TOTAL",
                Selected = false
            });

            listYear.Add(new SelectListItem
            {
                Text = "2001",
                Value = "2001",
                Selected = false
            });

            listYear.Add(new SelectListItem
            {
                Text = "2002",
                Value = "2002",
                Selected = false
            });

            listYear.Add(new SelectListItem
            {
                Text = "2003",
                Value = "2003",
                Selected = false
            });

            listYear.Add(new SelectListItem
            {
                Text = "2004",
                Value = "2004",
                Selected = false
            });

            listYear.Add(new SelectListItem
            {
                Text = "2005",
                Value = "2005",
                Selected = false
            });

            listYear.Add(new SelectListItem
            {
                Text = "2006",
                Value = "2006",
                Selected = false
            });
            listYear.Add(new SelectListItem
            {
                Text = "2007",
                Value = "2007",
                Selected = false
            });

            listYear.Add(new SelectListItem
            {
                Text = "2008",
                Value = "2008",
                Selected = false
            });

            listYear.Add(new SelectListItem
            {
                Text = "2009",
                Value = "2009",
                Selected = false
            });

            listYear.Add(new SelectListItem
            {
                Text = "2010",
                Value = "2010",
                Selected = false
            });

            listYear.Add(new SelectListItem
            {
                Text = "2011",
                Value = "2011",
                Selected = false
            });

            listYear.Add(new SelectListItem
            {
                Text = "2012",
                Value = "2012",
                Selected = false
            });

            listYear.Add(new SelectListItem
            {
                Text = "2013",
                Value = "2013",
                Selected = false
            });

            listYear.Add(new SelectListItem
            {
                Text = "2014",
                Value = "2014",
                Selected = false
            });

            listYear.Add(new SelectListItem
            {
                Text = "2015",
                Value = "2015",
                Selected = false
            });

            listYear.Add(new SelectListItem
            {
                Text = "2016",
                Value = "2016",
                Selected = false
            });
            listYear.Add(new SelectListItem
            {
                Text = "2017",
                Value = "2017",
                Selected = false
            });

            listYear.Add(new SelectListItem
            {
                Text = "2018",
                Value = "2018",
                Selected = false
            });

            listYear.Add(new SelectListItem
            {
                Text = "2019",
                Value = "2019",
                Selected = false
            });

            listYear.Add(new SelectListItem
            {
                Text = "2020",
                Value = "2020",
                Selected = false
            });
            listYear.Add(new SelectListItem
            {
                Text = "2021",
                Value = "2021",
                Selected = false
            });

            listYear.Add(new SelectListItem
            {
                Text = "2022",
                Value = "2022",
                Selected = false
            });

            listYear.Add(new SelectListItem
            {
                Text = "2023",
                Value = "2023",
                Selected = false
            });

            listYear.Add(new SelectListItem
            {
                Text = "2024",
                Value = "2024",
                Selected = false
            });

            listYear.Add(new SelectListItem
            {
                Text = "2025",
                Value = "2025",
                Selected = false
            });

            listYear.Add(new SelectListItem
            {
                Text = "2026",
                Value = "2026",
                Selected = false
            });
            listYear.Add(new SelectListItem
            {
                Text = "2027",
                Value = "2027",
                Selected = false
            });

            listYear.Add(new SelectListItem
            {
                Text = "2028",
                Value = "2028",
                Selected = false
            });

            listYear.Add(new SelectListItem
            {
                Text = "2029",
                Value = "2029",
                Selected = false
            });

            listYear.Add(new SelectListItem
            {
                Text = "2030",
                Value = "2030",
                Selected = false
            });
        }

        [Display(Name = "Amount Type", Order = 20)]
        public string AmountType { get { return _amountType; } set { _amountType = value; } }

        [Display(Name = "Year", Order = 20)]
        public string year { get { return _year; } set { _year = value; } }

        [Display(Name = "Type", Order = 20)]
        public string type { get { return _type; } set { _type = value; } }

        [Display(Name = "Month", Order = 20)]
        public string month { get { return _month; } set { _month = value; } }

        [Display(Name = "Category", Order = 20)]
        public string category { get { return _category; } set { _category = value; } }


        public IList<SelectListItem> SelectionYear { get { return listYear; } }
        public IList<SelectListItem> Selection { get { return listItems; } }
        public IList<SelectListItem> SelectionType { get { return listType; } }
        public IList<SelectListItem> SelectionMonth { get { return listMonth; } }
        public IList<SelectListItem> SelectionCategory { get { return listCategory; } }
    }

    public class PrfBillRecord
    {
        private int _yearno;
        private int _mthno;
        private string _casecode;
        private string _itemtype;
        private string _staff;
        private decimal _amt;
        private decimal _tax;
        private int _cnt;
        private decimal _net;
        private decimal _rate;
        DateTime _DUEDATE;


        [Display(Name = "End Date", Order = 20)]
        [DataType(DataType.Date)]
        public DateTime DUEDATE { get { return _DUEDATE; } set { _DUEDATE = value; } }

        [Display(Name = "Case Code", Order = 20)]
        public string casecode { get { return _casecode; } set { _casecode = value; } }

        [Display(Name = "Item Type", Order = 20)]
        public string itemtype { get { return _itemtype; } set { _itemtype = value; } }

        [Display(Name = "Staff", Order = 20)]
        public string staff { get { return _staff; } set { _staff = value; } }

        [Display(Name = "Year", Order = 20)]
        public int yearno { get { return _yearno; } set { _yearno = value; } }


        [Display(Name = "Month", Order = 20)]
        public int mthno { get { return _mthno; } set { _mthno = value; } }
        public int cnt { get { return _cnt; } set { _cnt = value; } }

        [Display(Name = "Amount", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal amt { get { return _amt; } set { _amt = value; } }

        [Display(Name = "Tax", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal tax { get { return _tax; } set { _tax = value; } }

        [Display(Name = "Net", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal net { get { return _net; } set { _net = value; } }

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

    public class PrfBillMthRecord
    {
        private int _yearno;
        private string _casecode;
        private string _casedesc;

        private decimal _jan= 0;
        private decimal _feb= 0;
        private decimal _mar= 0;
        private decimal _apr= 0;
        private decimal _may= 0;
        private decimal _jun= 0;
        private decimal _jul= 0;
        private decimal _aug= 0;
        private decimal _sep= 0;
        private decimal _oct= 0;
        private decimal _nov= 0;
        private decimal _dec= 0;


        [Display(Name = "Case Code", Order = 20)]
        public string casecode { get { return _casecode; } set { _casecode = value; } }

        [Display(Name = "Description", Order = 20)]
        public string casedesc { get { return _casedesc; } set { _casedesc = value; } }



        [Display(Name = "Year", Order = 20)]
        public int yearno { get { return _yearno; } set { _yearno = value; } }


        [Display(Name = "Jan", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal jan { get { return _jan; } set { _jan = value; } }

        [Display(Name = "Feb", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal feb { get { return _feb; } set { _feb = value; } }


        [Display(Name = "Mar", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal mar { get { return _mar; } set { _mar = value; } }

        [Display(Name = "Apr", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal apr { get { return _apr; } set { _apr = value; } }

        [Display(Name = "May", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal may { get { return _may; } set { _may = value; } }

        [Display(Name = "Jun", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal jun { get { return _jun; } set { _jun = value; } }


        [Display(Name = "Jul", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal jul { get { return _jul; } set { _jul = value; } }

        [Display(Name = "Aug", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal aug { get { return _aug; } set { _aug = value; } }

        [Display(Name = "Sep", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal sep { get { return _sep; } set { _sep = value; } }

        [Display(Name = "Oct", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal oct { get { return _oct; } set { _oct = value; } }


        [Display(Name = "Nov", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal nov { get { return _nov; } set { _nov = value; } }

        [Display(Name = "Dec", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal dec { get { return _dec; } set { _dec = value; } }

        [Display(Name = "Total", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal total { get { return _jan  + _feb + _mar + _apr + _may + _jun + _jul + _aug + _sep + _oct + _nov + _dec; } }

    }

    public class PrfBillYearRecord
    {

        private string _casecode;
        private string _casedesc;

        private decimal _2001= 0;
        private decimal _2002= 0;
        private decimal _2003= 0;
        private decimal _2004= 0;
        private decimal _2005= 0;
        private decimal _2006= 0;
        private decimal _2007= 0;
        private decimal _2008= 0;
        private decimal _2009= 0;
        private decimal _2010= 0;
        private decimal _2011= 0;
        private decimal _2012= 0;
        private decimal _2013= 0;
        private decimal _2014= 0;
        private decimal _2015= 0;
        private decimal _2016= 0;
        private decimal _2017= 0;
        private decimal _2018= 0;
        private decimal _2019= 0;
        private decimal _2020= 0;
        private decimal _2021= 0;
        private decimal _2022= 0;
        private decimal _2023= 0;
        private decimal _2024= 0;
        private decimal _2025= 0;
        private decimal _2026= 0;
        private decimal _2027= 0;
        private decimal _2028= 0;
        private decimal _2029= 0;
        private decimal _2030= 0;
        private decimal _2031= 0;
        private decimal _2032= 0;
        private decimal _2033= 0;
        private decimal _2034= 0;
        private decimal _2035= 0;
        private decimal _2036= 0;

        [Display(Name = "Case Code", Order = 20)]
        public string casecode { get { return _casecode; } set { _casecode = value; } }

        [Display(Name = "Description", Order = 20)]
        public string casedesc { get { return _casedesc; } set { _casedesc = value; } }


        [Display(Name = "2001", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal y2001 { get { return _2001; } set { _2001 = value; } }

        [Display(Name = "2002", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal y2002 { get { return _2002; } set { _2002 = value; } }


        [Display(Name = "2003", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal y2003 { get { return _2003; } set { _2003 = value; } }

        [Display(Name = "2004", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal y2004 { get { return _2004; } set { _2004 = value; } }

        [Display(Name = "2005", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal y2005 { get { return _2005; } set { _2005 = value; } }

        [Display(Name = "2006", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal y2006 { get { return _2006; } set { _2006 = value; } }


        [Display(Name = "2007", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal y2007 { get { return _2007; } set { _2007 = value; } }

        [Display(Name = "2008", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal y2008 { get { return _2008; } set { _2008 = value; } }

        [Display(Name = "2009", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal y2009 { get { return _2009; } set { _2009 = value; } }

        [Display(Name = "2010", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal y2010 { get { return _2010; } set { _2010 = value; } }


        [Display(Name = "2011", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal y2011 { get { return _2011; } set { _2011 = value; } }

        [Display(Name = "2012", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal y2012 { get { return _2012; } set { _2012 = value; } }


        [Display(Name = "2013", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal y2013 { get { return _2013; } set { _2013 = value; } }

        [Display(Name = "2014", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal y2014 { get { return _2014; } set { _2014 = value; } }

        [Display(Name = "2015", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal y2015 { get { return _2015; } set { _2015 = value; } }

        [Display(Name = "2016", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal y2016 { get { return _2016; } set { _2016 = value; } }


        [Display(Name = "2017", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal y2017 { get { return _2017; } set { _2017 = value; } }

        [Display(Name = "2018", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal y2018 { get { return _2018; } set { _2018 = value; } }

        [Display(Name = "2019", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal y2019 { get { return _2019; } set { _2019 = value; } }

        [Display(Name = "2020", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal y2020 { get { return _2020; } set { _2020 = value; } }

        [Display(Name = "2021", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal y2021 { get { return _2021; } set { _2021 = value; } }

        [Display(Name = "2022", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal y2022 { get { return _2022; } set { _2022 = value; } }


        [Display(Name = "2023", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal y2023 { get { return _2023; } set { _2023 = value; } }

        [Display(Name = "2024", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal y2024 { get { return _2024; } set { _2024 = value; } }

        [Display(Name = "2025", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal y2025 { get { return _2025; } set { _2025 = value; } }

        [Display(Name = "2026", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal y2026 { get { return _2026; } set { _2026 = value; } }


        [Display(Name = "2027", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal y2027 { get { return _2027; } set { _2027 = value; } }

        [Display(Name = "2028", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal y2028 { get { return _2028; } set { _2028 = value; } }

        [Display(Name = "2029", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal y2029 { get { return _2029; } set { _2029 = value; } }

        [Display(Name = "2030", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal y2030 { get { return _2030; } set { _2030 = value; } }

        [Display(Name = "2031", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal y2031 { get { return _2031; } set { _2031 = value; } }

        [Display(Name = "2032", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal y2032 { get { return _2032; } set { _2032 = value; } }


        [Display(Name = "2033", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal y2033 { get { return _2033; } set { _2033 = value; } }

        [Display(Name = "2034", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal y2034 { get { return _2034; } set { _2034 = value; } }

        [Display(Name = "2035", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal y2035 { get { return _2035; } set { _2035 = value; } }

        [Display(Name = "2036", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal y2036 { get { return _2036; } set { _2036 = value; } }

    }


    public class PrfBillStaffRecord
    {
        private int _yearno;
        private string _casecode;
        private string _casedesc;

        private decimal _c01= 0;
        private decimal _c02= 0;
        private decimal _c03= 0;
        private decimal _c04= 0;
        private decimal _c05= 0;
        private decimal _c06= 0;
        private decimal _c07= 0;
        private decimal _c08= 0;
        private decimal _c09= 0;
        private decimal _c10= 0;
        private decimal _c11= 0;
        private decimal _c12= 0;
        private decimal _c13= 0;
        private decimal _c14= 0;
        private decimal _c15= 0;
        private decimal _c16= 0;
        private decimal _c17= 0;
        private decimal _c18= 0;

        [Display(Name = "Case Code", Order = 20)]
        public string casecode { get { return _casecode; } set { _casecode = value; } }

        [Display(Name = "Description", Order = 20)]
        public string casedesc { get { return _casedesc; } set { _casedesc = value; } }



        [Display(Name = "Year", Order = 20)]
        public int yearno { get { return _yearno; } set { _yearno = value; } }


        [Display(Name = "base 1", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal c01 { get { return _c01; } set { _c01 = value; } }

        [Display(Name = "base 2", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal c02 { get { return _c02; } set { _c02 = value; } }


        [Display(Name = "base 3", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal c03 { get { return _c03; } set { _c03 = value; } }

        [Display(Name = "base 4", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal c04 { get { return _c04; } set { _c04 = value; } }

        [Display(Name = "base 5", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal c05 { get { return _c05; } set { _c05 = value; } }

        [Display(Name = "base 6", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal c06 { get { return _c06; } set { _c06 = value; } }


        [Display(Name = "base 7", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal c07 { get { return _c07; } set { _c07 = value; } }

        [Display(Name = "base 8", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal c08 { get { return _c08; } set { _c08 = value; } }

        [Display(Name = "base 9", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal c09 { get { return _c09; } set { _c09 = value; } }

        [Display(Name = "base 10", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal c10 { get { return _c10; } set { _c10 = value; } }


        [Display(Name = "base 11", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal c11 { get { return _c11; } set { _c11 = value; } }

        [Display(Name = "base 12", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal c12 { get { return _c12; } set { _c12 = value; } }

        [Display(Name = "base 13", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal c13 { get { return _c13; } set { _c13 = value; } }

        [Display(Name = "base 14", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal c14 { get { return _c14; } set { _c14 = value; } }

        [Display(Name = "base 15", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal c15 { get { return _c15; } set { _c15 = value; } }

        [Display(Name = "base 16", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal c16 { get { return _c16; } set { _c16 = value; } }


        [Display(Name = "base 17", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal c17 { get { return _c17; } set { _c17 = value; } }

        [Display(Name = "Unassigned", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal c18 { get { return _c18; } set { _c18 = value; } }

        [Display(Name = "Total", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal total { get { return _c01 + _c02 + _c03 + _c04 + _c05 + _c06 + _c07 + _c08 + _c09 + _c10 + _c11 + _c12 + _c13 + _c14 + _c15 + _c16 + _c17 + _c18; } }

    }

    public class PrfBillDayRecord
    {

        private string _casecode;
        private string _casedesc;
        private int _yearno;
        private int _monthno;

        private decimal _01 = 0;
        private decimal _02= 0;
        private decimal _03= 0;
        private decimal _04= 0;
        private decimal _05= 0;
        private decimal _06= 0;
        private decimal _07= 0;
        private decimal _08= 0;
        private decimal _09= 0;
        private decimal _10= 0;
        private decimal _11= 0;
        private decimal _12= 0;
        private decimal _13= 0;
        private decimal _14= 0;
        private decimal _15= 0;
        private decimal _16= 0;
        private decimal _17= 0;
        private decimal _18= 0;
        private decimal _19= 0;
        private decimal _20= 0;
        private decimal _21= 0;
        private decimal _22= 0;
        private decimal _23= 0;
        private decimal _24= 0;
        private decimal _25= 0;
        private decimal _26= 0;
        private decimal _27= 0;
        private decimal _28= 0;
        private decimal _29= 0;
        private decimal _30= 0;
        private decimal _31= 0;
        private decimal _32= 0;
        private decimal _33= 0;
        private decimal _34= 0;
        private decimal _35= 0;
        private decimal _36= 0;

        [Display(Name = "Case Code", Order = 20)]
        public string casecode { get { return _casecode; } set { _casecode = value; } }

        [Display(Name = "Description", Order = 20)]
        public string casedesc { get { return _casedesc; } set { _casedesc = value; } }

        [Display(Name = "Year", Order = 20)]
        public int yearno { get { return _yearno; } set { _yearno = value; } }

        [Display(Name = "Month", Order = 20)]
        public int monthno { get { return _monthno; } set { _monthno = value; } }

        [Display(Name = "D01", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal D01 { get { return _01; } set { _01 = value; } }

        [Display(Name = "D02", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal D02 { get { return _02; } set { _02 = value; } }


        [Display(Name = "D03", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal D03 { get { return _03; } set { _03 = value; } }

        [Display(Name = "D04", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal D04 { get { return _04; } set { _04 = value; } }

        [Display(Name = "D05", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal D05 { get { return _05; } set { _05 = value; } }

        [Display(Name = "D06", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal D06 { get { return _06; } set { _06 = value; } }


        [Display(Name = "D07", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal D07 { get { return _07; } set { _07 = value; } }

        [Display(Name = "D08", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal D08 { get { return _08; } set { _08 = value; } }

        [Display(Name = "D09", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal D09 { get { return _09; } set { _09 = value; } }

        [Display(Name = "D10", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal D10 { get { return _10; } set { _10 = value; } }


        [Display(Name = "D11", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal D11 { get { return _11; } set { _11 = value; } }

        [Display(Name = "D12", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal D12 { get { return _12; } set { _12 = value; } }


        [Display(Name = "D13", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal D13 { get { return _13; } set { _13 = value; } }

        [Display(Name = "D14", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal D14 { get { return _14; } set { _14 = value; } }

        [Display(Name = "D15", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal D15 { get { return _15; } set { _15 = value; } }

        [Display(Name = "D16", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal D16 { get { return _16; } set { _16 = value; } }


        [Display(Name = "D17", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal D17 { get { return _17; } set { _17 = value; } }

        [Display(Name = "D18", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal D18 { get { return _18; } set { _18 = value; } }

        [Display(Name = "D19", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal D19 { get { return _19; } set { _19 = value; } }

        [Display(Name = "D20", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal D20 { get { return _20; } set { _20 = value; } }

        [Display(Name = "D21", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal D21 { get { return _21; } set { _21 = value; } }

        [Display(Name = "D22", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal D22 { get { return _22; } set { _22 = value; } }


        [Display(Name = "D23", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal D23 { get { return _23; } set { _23 = value; } }

        [Display(Name = "D24", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal D24 { get { return _24; } set { _24 = value; } }

        [Display(Name = "D25", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal D25 { get { return _25; } set { _25 = value; } }

        [Display(Name = "D26", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal D26 { get { return _26; } set { _26 = value; } }


        [Display(Name = "D27", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal D27 { get { return _27; } set { _27 = value; } }

        [Display(Name = "D28", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal D28 { get { return _28; } set { _28 = value; } }

        [Display(Name = "D29", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal D29 { get { return _29; } set { _29 = value; } }

        [Display(Name = "D30", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal D30 { get { return _30; } set { _30 = value; } }

        [Display(Name = "D31", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal D31 { get { return _31; } set { _31 = value; } }

        [Display(Name = "D32", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal D32 { get { return _32; } set { _32 = value; } }


        [Display(Name = "D33", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal D33 { get { return _33; } set { _33 = value; } }

        [Display(Name = "D34", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal D34 { get { return _34; } set { _34 = value; } }

        [Display(Name = "D35", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal D35 { get { return _35; } set { _35 = value; } }

        [Display(Name = "D36", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal D36 { get { return _36; } set { _36 = value; } }

        [Display(Name = "Total", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal total { get
            { return D01 + D02 + D03 + D04 + D05 + D06 + D07 + D08 + D09 + D10 + D11 + D12 + D13 + D14 + D15 + D16 + D17 + D18
                    + D19 + D20 + D21 + D22 + D23 + D24 + D25 + D26 + D27 + D28 + D29 + D30 + D31 + D32 + D33 + D34 + D35 + D36
                    ; } }

    }

}
