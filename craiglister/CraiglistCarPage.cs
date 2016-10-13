using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace craiglister
{
    public class CraiglistCarPage
    {
        static Dictionary<string, int> years = new Dictionary<string, int>();                        

        static CraiglistCarPage()
        {
            for (int year = 1990; year < DateTime.Now.Year; year++)
            {
                var shortYear = year < 2000 ? year - 1900 : year - 2000;
                years.Add(shortYear.ToString("00"), year);
                years.Add(year.ToString(), year);
            }
        }

        public static Car Parse(string plainHtml)
        {
            var car = new Car();

            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(plainHtml);

            car.MakeModel = GetMakeModel(htmlDoc);
            car.Price = GetPriceFromHtml(htmlDoc);            
            
            ParseAttributes(htmlDoc, car);

            UpdateAll(car, car.MakeModel);
            UpdateAll(car, GetPostingText(htmlDoc));            

            return car;
        }

        public static void UpdateAll(Car car, string text)
        {
            text = UpdateTitleStatus(car, text);
            text = UpdatePrice(car, text);
            text = UpdateMileage(car, text);
            text = UpdateYear(car, text);                       
        }

        private static string UpdateTitleStatus(Car car, string text)
        {
            if (text.ToLower().Contains("rebuilt"))
                car.TitleStatus = "rebuilt";

            if (text.ToLower().Contains("title clean") || text.ToLower().Contains("clean title"))
                car.TitleStatus = "clean";

            return text;
        }


        static Regex regexYear = new Regex(@"(\d{4,4}|\d{2,2})");
        public static string UpdateYear(Car car, string text)
        {
            if (car.Year > 0)
                return text;
           
            var matches = regexYear.Matches(text);
            foreach (Match match in matches)
            {
                if (match.Success)
                {
                    var v = match.Value.Trim();
                    if (years.ContainsKey(v))
                    {
                        car.Year = years[v];
                        return text.Remove(match.Index, match.Length);
                    }         
                }
            }

            return text;
        }

        static Regex regexMileage = new Regex(@"[0-9\.,]{2,6}k{0,1}");
        public static string UpdateMileage(Car car, string text)
        {
            if (car.Mileage > 0)
                return text;
            
            var matches = regexMileage.Matches(text);
            foreach (Match match in matches)
            {
                if (match.Success)
                {
                    var v = match.Value.Trim();
                    if (!years.ContainsKey(v))
                    {                        
                        int miles = ParseOdometer(v);
                        if (10000 <= miles && miles <= 300000)
                        {
                            car.Mileage = miles;
                            return text.Remove(match.Index, match.Length);
                        }
                    }
                }
            }

            return text;
        }

        private static string GetMakeModel(HtmlDocument htmlDoc)
        {
            var node = htmlDoc.GetElementbyId("titletextonly");
            return node.InnerText;
        }

        static void ParseAttributes(HtmlDocument htmlDoc, Car car)
        {
            var nodes = htmlDoc.DocumentNode.SelectNodes("//div[@class='mapAndAttrs']//p[@class='attrgroup']/span");
            foreach (var node in nodes)
            {
                var text = node.InnerText;
                if (text.Contains("fuel:"))
                    car.Fuel = text.Substring("fuel: ".Length);
                else if (text.Contains("title status:"))
                    car.TitleStatus = text.Substring("title status: ".Length);
                else if (text.Contains("transmission:"))
                    car.Transimission = text.Substring("transmission: ".Length);
                else if (text.Contains("odometer:"))
                    car.Mileage = ParseOdometer(text.Substring("odometer: ".Length));
            }
        }

        static string GetPostingText(HtmlDocument htmlDoc)
        {
            return htmlDoc.GetElementbyId("postingbody").InnerText;
        }

        static int GetPriceFromHtml(HtmlDocument htmlDoc)
        {
            var nodes = htmlDoc.DocumentNode.SelectNodes("//span[@class='price']");
            if (nodes == null)
                return 0;
                        
            return ParsePrice(nodes[0].InnerText);
        }

        static Regex regexPrice = new Regex(@"\${0,1}\s{0,1}\d{3,5}\s{0,1}\${0,1}");
        public static string UpdatePrice(Car car, string text)
        {
            if (car.Price > 0)
                return text;
           
            var matches = regexPrice.Matches(text);
            foreach (Match match in matches)
            {
                if (match.Success && match.Value.Contains("$"))
                {                    
                    car.Price = ParsePrice(match.Value);
                    return text.Remove(match.Index, match.Length);
                }
            }

            return text;
        }

        static int ParsePrice(string text)
        {
            return int.Parse(text.Trim().Replace("$", ""));
        }

        private static string Normalize(string text)
        {
            return " " + text
                .Replace('.', ' ')
                .Replace(',', ' ')
                .Replace('!', ' ')
                .Replace('"', ' ')
                .Replace('*', ' ')
                .Replace('\r', ' ')
                .Replace('\n', ' ') 
                + " ";
        }

        public static int ParseOdometer(string odometer)
        {
            odometer = odometer.Replace("k", "000").Replace(" ", "").Replace("miles", "").Trim();
            double mileage;
            if (!double.TryParse(odometer, out mileage))
                return 0;
            if (mileage < 1000)
                mileage *= 1000;
            return (int)mileage;
        }
    }    
}
