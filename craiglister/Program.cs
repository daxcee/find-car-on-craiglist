using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using HtmlAgilityPack;

namespace craiglister
{
    class Program
    {
        static void Main(string[] args)
        {
            var names = new[] { "civic", "accord", "fit","corolla", "camry", "mazda+3", "mazda+6", "fusion", "focus" };
            foreach (var name in names)
            {
                var readHtml = new ReadWaiter(new ReadHtml());

                var searchPageHtml = readHtml.Read(CraiglistUrls.SearchCarByOwnerInNashville(name));
                var cars = new List<Car>();
                while (true)
                {
                    var searchPage = CraiglistSearchPage.Parse(searchPageHtml);
                    foreach (var url in searchPage.Urls)
                    {
                        var fullCarUrl = CraiglistUrls.CraigListUrlInNashville(url);
                        var carHtml = readHtml.Read(fullCarUrl);
                        if (carHtml != null)
                        {
                            var car = CraiglistCarPage.Parse(carHtml);
                            car.Url = fullCarUrl;
                            cars.Add(car);
                            Console.WriteLine($"{name} - {cars.Count}");
                            CsvWriter.Write(car, $@"d:\sergiy\projects\craiglister\output\{name}.csv");
                        }
                    }

                    if (searchPage.NextUrl == null)
                        break;

                    var nextUrl = CraiglistUrls.CraigListUrlInNashville(searchPage.NextUrl);
                    searchPageHtml = readHtml.Read(nextUrl);
                }                
            }
        }
    }
}
