using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace craiglister
{
    class CsvWriter
    {
        public static void Write(List<Car> cars, string fileName)
        {
            using (var sw = new StreamWriter(fileName))
            {
                sw.WriteLine("Make, Price, Mileage, Year, Title Status, Transmission, Url, Fuel");
                foreach (var car in cars)
                {
                    var make = car.MakeModel.Replace(",", " ");
                    sw.WriteLine($"{make}, {car.Price}, {car.Mileage}, {car.Year}, {car.TitleStatus}, {car.Transimission}, {car.Url}, {car.Fuel}");
                }
            }
        }

        public static void Write(Car car, string fileName)
        {
            bool writeHeader = !File.Exists(fileName);
            using (var sw = new StreamWriter(fileName, true))
            {
                if (writeHeader)
                    sw.WriteLine("Make, Price, Mileage, Year, Title Status, Transmission, Url, Fuel");
                
                var make = car.MakeModel.Replace(",", " ");
                sw.WriteLine($"{make}, {car.Price}, {car.Mileage}, {car.Year}, {car.TitleStatus}, {car.Transimission}, {car.Url}, {car.Fuel}");
            }
        }
    }
}
