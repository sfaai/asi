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
    public class CostatStuff
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

        public CostatStuff()
        {
        }

    }

    public class CostatRecord
    {
        private int _yearno;
        private int _mthno;
        private string _costat;
        private int _cnt;


        [Display(Name = "Status", Order = 20)]
        public string costat { get { return _costat; } set { _costat = value; } }

        [Display(Name = "Year", Order = 20)]
        public int yearno { get { return _yearno; } set { _yearno = value; } }


        [Display(Name = "Month", Order = 20)]
        public int mthno { get { return _mthno; } set { _mthno = value; } }


        [Display(Name = "Total", Order = 100)]
        public int cnt { get { return _cnt; } set { _cnt = value; } }


    }

    public class CostatMthRecord
    {
        private int _yearno;
        private string _costat;

        private int _jan= 0;
        private int _feb= 0;
        private int _mar= 0;
        private int _apr= 0;
        private int _may= 0;
        private int _jun= 0;
        private int _jul= 0;
        private int _aug= 0;
        private int _sep= 0;
        private int _oct= 0;
        private int _nov= 0;
        private int _dec= 0;


        [Display(Name = "Status", Order = 20)]
        public string costat { get { return _costat; } set { _costat = value; } }


        [Display(Name = "Year", Order = 20)]
        public int yearno { get { return _yearno; } set { _yearno = value; } }


        [Display(Name = "Jan", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int jan { get { return _jan; } set { _jan = value; } }

        [Display(Name = "Feb", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int feb { get { return _feb; } set { _feb = value; } }


        [Display(Name = "Mar", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int mar { get { return _mar; } set { _mar = value; } }

        [Display(Name = "Apr", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int apr { get { return _apr; } set { _apr = value; } }

        [Display(Name = "May", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int may { get { return _may; } set { _may = value; } }

        [Display(Name = "Jun", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int jun { get { return _jun; } set { _jun = value; } }


        [Display(Name = "Jul", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int jul { get { return _jul; } set { _jul = value; } }

        [Display(Name = "Aug", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int aug { get { return _aug; } set { _aug = value; } }

        [Display(Name = "Sep", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int sep { get { return _sep; } set { _sep = value; } }

        [Display(Name = "Oct", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int oct { get { return _oct; } set { _oct = value; } }


        [Display(Name = "Nov", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int nov { get { return _nov; } set { _nov = value; } }

        [Display(Name = "Dec", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int dec { get { return _dec; } set { _dec = value; } }

        [Display(Name = "Total", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int total { get { return _jan  + _feb + _mar + _apr + _may + _jun + _jul + _aug + _sep + _oct + _nov + _dec; } }

    }

    public class CostatYearRecord
    {

        private string _costat;

        private int _2001= 0;
        private int _2002= 0;
        private int _2003= 0;
        private int _2004= 0;
        private int _2005= 0;
        private int _2006= 0;
        private int _2007= 0;
        private int _2008= 0;
        private int _2009= 0;
        private int _2010= 0;
        private int _2011= 0;
        private int _2012= 0;
        private int _2013= 0;
        private int _2014= 0;
        private int _2015= 0;
        private int _2016= 0;
        private int _2017= 0;
        private int _2018= 0;
        private int _2019= 0;
        private int _2020= 0;
        private int _2021= 0;
        private int _2022= 0;
        private int _2023= 0;
        private int _2024= 0;
        private int _2025= 0;
        private int _2026= 0;
        private int _2027= 0;
        private int _2028= 0;
        private int _2029= 0;
        private int _2030= 0;
        private int _2031= 0;
        private int _2032= 0;
        private int _2033= 0;
        private int _2034= 0;
        private int _2035= 0;
        private int _2036= 0;

        [Display(Name = "Status", Order = 20)]
        public string costat { get { return _costat; } set { _costat = value; } }


        [Display(Name = "2001", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int y2001 { get { return _2001; } set { _2001 = value; } }

        [Display(Name = "2002", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int y2002 { get { return _2002; } set { _2002 = value; } }


        [Display(Name = "2003", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int y2003 { get { return _2003; } set { _2003 = value; } }

        [Display(Name = "2004", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int y2004 { get { return _2004; } set { _2004 = value; } }

        [Display(Name = "2005", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int y2005 { get { return _2005; } set { _2005 = value; } }

        [Display(Name = "2006", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int y2006 { get { return _2006; } set { _2006 = value; } }


        [Display(Name = "2007", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int y2007 { get { return _2007; } set { _2007 = value; } }

        [Display(Name = "2008", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int y2008 { get { return _2008; } set { _2008 = value; } }

        [Display(Name = "2009", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int y2009 { get { return _2009; } set { _2009 = value; } }

        [Display(Name = "2010", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int y2010 { get { return _2010; } set { _2010 = value; } }


        [Display(Name = "2011", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int y2011 { get { return _2011; } set { _2011 = value; } }

        [Display(Name = "2012", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int y2012 { get { return _2012; } set { _2012 = value; } }


        [Display(Name = "2013", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int y2013 { get { return _2013; } set { _2013 = value; } }

        [Display(Name = "2014", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int y2014 { get { return _2014; } set { _2014 = value; } }

        [Display(Name = "2015", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int y2015 { get { return _2015; } set { _2015 = value; } }

        [Display(Name = "2016", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int y2016 { get { return _2016; } set { _2016 = value; } }


        [Display(Name = "2017", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int y2017 { get { return _2017; } set { _2017 = value; } }

        [Display(Name = "2018", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int y2018 { get { return _2018; } set { _2018 = value; } }

        [Display(Name = "2019", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int y2019 { get { return _2019; } set { _2019 = value; } }

        [Display(Name = "2020", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int y2020 { get { return _2020; } set { _2020 = value; } }

        [Display(Name = "2021", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int y2021 { get { return _2021; } set { _2021 = value; } }

        [Display(Name = "2022", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int y2022 { get { return _2022; } set { _2022 = value; } }


        [Display(Name = "2023", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int y2023 { get { return _2023; } set { _2023 = value; } }

        [Display(Name = "2024", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int y2024 { get { return _2024; } set { _2024 = value; } }

        [Display(Name = "2025", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int y2025 { get { return _2025; } set { _2025 = value; } }

        [Display(Name = "2026", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int y2026 { get { return _2026; } set { _2026 = value; } }


        [Display(Name = "2027", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int y2027 { get { return _2027; } set { _2027 = value; } }

        [Display(Name = "2028", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int y2028 { get { return _2028; } set { _2028 = value; } }

        [Display(Name = "2029", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int y2029 { get { return _2029; } set { _2029 = value; } }

        [Display(Name = "2030", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int y2030 { get { return _2030; } set { _2030 = value; } }

        [Display(Name = "2031", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int y2031 { get { return _2031; } set { _2031 = value; } }

        [Display(Name = "2032", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int y2032 { get { return _2032; } set { _2032 = value; } }


        [Display(Name = "2033", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int y2033 { get { return _2033; } set { _2033 = value; } }

        [Display(Name = "2034", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int y2034 { get { return _2034; } set { _2034 = value; } }

        [Display(Name = "2035", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int y2035 { get { return _2035; } set { _2035 = value; } }

        [Display(Name = "2036", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int y2036 { get { return _2036; } set { _2036 = value; } }

    }


    public class CostatDayRecord
    {

        private string _costat;
        private int _yearno;
        private int _monthno;

        private int _01 = 0;
        private int _02= 0;
        private int _03= 0;
        private int _04= 0;
        private int _05= 0;
        private int _06= 0;
        private int _07= 0;
        private int _08= 0;
        private int _09= 0;
        private int _10= 0;
        private int _11= 0;
        private int _12= 0;
        private int _13= 0;
        private int _14= 0;
        private int _15= 0;
        private int _16= 0;
        private int _17= 0;
        private int _18= 0;
        private int _19= 0;
        private int _20= 0;
        private int _21= 0;
        private int _22= 0;
        private int _23= 0;
        private int _24= 0;
        private int _25= 0;
        private int _26= 0;
        private int _27= 0;
        private int _28= 0;
        private int _29= 0;
        private int _30= 0;
        private int _31= 0;
        private int _32= 0;
        private int _33= 0;
        private int _34= 0;
        private int _35= 0;
        private int _36= 0;

        [Display(Name = "Status", Order = 20)]
        public string costat { get { return _costat; } set { _costat = value; } }


        [Display(Name = "Year", Order = 20)]
        public int yearno { get { return _yearno; } set { _yearno = value; } }

        [Display(Name = "Month", Order = 20)]
        public int monthno { get { return _monthno; } set { _monthno = value; } }

        [Display(Name = "D01", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int D01 { get { return _01; } set { _01 = value; } }

        [Display(Name = "D02", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int D02 { get { return _02; } set { _02 = value; } }


        [Display(Name = "D03", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int D03 { get { return _03; } set { _03 = value; } }

        [Display(Name = "D04", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int D04 { get { return _04; } set { _04 = value; } }

        [Display(Name = "D05", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int D05 { get { return _05; } set { _05 = value; } }

        [Display(Name = "D06", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int D06 { get { return _06; } set { _06 = value; } }


        [Display(Name = "D07", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int D07 { get { return _07; } set { _07 = value; } }

        [Display(Name = "D08", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int D08 { get { return _08; } set { _08 = value; } }

        [Display(Name = "D09", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int D09 { get { return _09; } set { _09 = value; } }

        [Display(Name = "D10", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int D10 { get { return _10; } set { _10 = value; } }


        [Display(Name = "D11", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int D11 { get { return _11; } set { _11 = value; } }

        [Display(Name = "D12", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int D12 { get { return _12; } set { _12 = value; } }


        [Display(Name = "D13", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int D13 { get { return _13; } set { _13 = value; } }

        [Display(Name = "D14", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int D14 { get { return _14; } set { _14 = value; } }

        [Display(Name = "D15", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int D15 { get { return _15; } set { _15 = value; } }

        [Display(Name = "D16", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int D16 { get { return _16; } set { _16 = value; } }


        [Display(Name = "D17", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int D17 { get { return _17; } set { _17 = value; } }

        [Display(Name = "D18", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int D18 { get { return _18; } set { _18 = value; } }

        [Display(Name = "D19", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int D19 { get { return _19; } set { _19 = value; } }

        [Display(Name = "D20", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int D20 { get { return _20; } set { _20 = value; } }

        [Display(Name = "D21", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int D21 { get { return _21; } set { _21 = value; } }

        [Display(Name = "D22", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int D22 { get { return _22; } set { _22 = value; } }


        [Display(Name = "D23", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int D23 { get { return _23; } set { _23 = value; } }

        [Display(Name = "D24", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int D24 { get { return _24; } set { _24 = value; } }

        [Display(Name = "D25", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int D25 { get { return _25; } set { _25 = value; } }

        [Display(Name = "D26", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int D26 { get { return _26; } set { _26 = value; } }


        [Display(Name = "D27", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int D27 { get { return _27; } set { _27 = value; } }

        [Display(Name = "D28", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int D28 { get { return _28; } set { _28 = value; } }

        [Display(Name = "D29", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int D29 { get { return _29; } set { _29 = value; } }

        [Display(Name = "D30", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int D30 { get { return _30; } set { _30 = value; } }

        [Display(Name = "D31", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int D31 { get { return _31; } set { _31 = value; } }

        [Display(Name = "D32", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int D32 { get { return _32; } set { _32 = value; } }


        [Display(Name = "D33", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int D33 { get { return _33; } set { _33 = value; } }

        [Display(Name = "D34", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int D34 { get { return _34; } set { _34 = value; } }

        [Display(Name = "D35", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int D35 { get { return _35; } set { _35 = value; } }

        [Display(Name = "D36", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int D36 { get { return _36; } set { _36 = value; } }

        [Display(Name = "Total", Order = 100)]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int total { get
            { return D01 + D02 + D03 + D04 + D05 + D06 + D07 + D08 + D09 + D10 + D11 + D12 + D13 + D14 + D15 + D16 + D17 + D18
                    + D19 + D20 + D21 + D22 + D23 + D24 + D25 + D26 + D27 + D28 + D29 + D30 + D31 + D32 + D33 + D34 + D35 + D36
                    ; } }

    }

}
