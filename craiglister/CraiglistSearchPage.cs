using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace craiglister
{
    class CraiglistSearchPage
    {
        public List<string> Urls { get; private set; } = new List<string>();
        public string NextUrl { get; private set; }

        public static CraiglistSearchPage Parse(string plainHtml)
        {
            var result = new CraiglistSearchPage();
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(plainHtml);
            var nodes = htmlDoc.DocumentNode.SelectNodes("//div[@class='content']//p/a");
            if (nodes == null)
                return result;

            foreach (var x in nodes)
            {
                var href = x.GetAttributeValue("href", null);
                if (href != null)
                    result.Urls.Add(href);
            }

            nodes = htmlDoc.DocumentNode.SelectNodes("//a[@class='button next']");
            if (nodes != null && nodes.Count > 0)
                result.NextUrl = nodes[0].GetAttributeValue("href", null);
            return result;
        }    

    }
}
