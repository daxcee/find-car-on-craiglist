using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace craiglister
{
    class CraiglistUrls
    {
        public static string SearchCarByOwnerInNashville(string words)
        {
            return "http://nashville.craigslist.org/search/cto?query=" + words;
        }

        public static string CraigListUrlInNashville(string craigListUrl)
        {
            return "http://nashville.craigslist.org" + craigListUrl;
        }
    }
}
