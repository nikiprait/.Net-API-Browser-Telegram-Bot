using AngleSharp.Dom.Html;
using System.Collections.Generic;
using System.Linq;

namespace Tg_NetAPIBrowser.Resources.MSDN
{
    class MSDNParser : IParser<string[]>
    {
        public string[] Parse(IHtmlDocument document)
        {
            var list = new List<string>();
            var items = document.QuerySelectorAll("div").Where(item => item.ClassName != null && item.ClassName.Contains("summary clearFix"));

            foreach (var item in items)
            {
                list.Add(item.TextContent);
            }

            return list.ToArray();
        }
    }
}
