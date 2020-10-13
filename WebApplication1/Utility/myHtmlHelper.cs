using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Reflection;
using System.Web;

namespace WebApplication1.Utility
{
    public static class MyHtmlHelpers
    {
        public static string ConvertIdToByteStr( string id) {
            string s = "";
            foreach (byte b in System.Text.Encoding.UTF8.GetBytes(id.ToCharArray()))
            {
                s = s + b.ToString("D3");
            }
            //return MvcHtmlString.Create(s);

            return s;
        }

        public static string ConvertByteStrToId(string id)
        {
            string r = "";
            int j = 0;
            for(int i = 0; i < id.Length / 3; i++)
            {
                j = Int32.Parse(id.Substring(i * 3, 3));
                r = r + (char)j;
             }
            return r;
        }

        public static MvcHtmlString MyTextBoxFor<TModel, TProperty>(
             this HtmlHelper<TModel> helper,
             Expression<Func<TModel, TProperty>> expression)
        {
            return helper.TextBoxFor(expression, new { @class = "txt" });
        }
    }

    public static class HtmlRequestHelper
    {
        public static string Id(this HtmlHelper htmlHelper)
        {
            var routeValues = HttpContext.Current.Request.RequestContext.RouteData.Values;

            if (routeValues.ContainsKey("id"))
                return (string)routeValues["id"];
            else if (HttpContext.Current.Request.QueryString.AllKeys.Contains("id"))
                return HttpContext.Current.Request.QueryString["id"];

            return string.Empty;
        }

        public static string Controller(this HtmlHelper htmlHelper)
        {
            var routeValues = HttpContext.Current.Request.RequestContext.RouteData.Values;

            if (routeValues.ContainsKey("controller"))
                return (string)routeValues["controller"];

            return string.Empty;
        }

        public static string Action(this HtmlHelper htmlHelper)
        {
            var routeValues = HttpContext.Current.Request.RequestContext.RouteData.Values;

            if (routeValues.ContainsKey("action"))
                return (string)routeValues["action"];

            return string.Empty;
        }

        public static string Area(this HtmlHelper htmlHelper)
        {
            var dataTokens = HttpContext.Current.Request.RequestContext.RouteData.DataTokens;

            if (dataTokens.ContainsKey("area"))
                return (string)dataTokens["area"];

            return string.Empty;
        }
    }

}