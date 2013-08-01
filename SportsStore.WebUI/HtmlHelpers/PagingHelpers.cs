using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using SportsStore.WebUI.Models;

namespace SportsStore.WebUI.HtmlHelpers
{
    public static class PagingHelpers
    {
        public static MvcHtmlString PageLinks(this HtmlHelper helper, PageInfo pageInfo, Func<int, string> pageUrl)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 1; i <= pageInfo.TotalPages; i++)
            {
                TagBuilder tag = new TagBuilder("a");
                tag.MergeAttribute("href", pageUrl(i));
                tag.InnerHtml = i.ToString();
                if (i == pageInfo.CurrentPage)
                    tag.AddCssClass("selected");
                sb.Append(tag.ToString());
            }
            return MvcHtmlString.Create(sb.ToString());
        }
    }
}