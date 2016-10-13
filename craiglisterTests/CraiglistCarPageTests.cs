using Microsoft.VisualStudio.TestTools.UnitTesting;
using craiglister;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace craiglister.Tests
{
    [TestClass()]
    public class CraiglistCarPageTests
    {
        Car car;

        [TestMethod()]
        public void UpdatePriceTest()
        {
            car = new Car();
            CraiglistCarPage.UpdatePrice(car, "fjkldsjfl $1800 O.B.O");
            Assert.AreEqual(1800, car.Price);

            car = new Car();
            CraiglistCarPage.UpdatePrice(car, "fjkldsjfl $ 1900 O.B.O");
            Assert.AreEqual(1900, car.Price);

            car = new Car();
            CraiglistCarPage.UpdatePrice(car, "fjkldsjfl 200$ O.B.O");
            Assert.AreEqual(200, car.Price);
        }

        [TestMethod()]
        public void UpdateMileage()
        {
            car = new Car();
            CraiglistCarPage.UpdateMileage(car, "103k");
            Assert.AreEqual(103000, car.Mileage);

            car = new Car();
            CraiglistCarPage.UpdateMileage(car, "110000miles");
            Assert.AreEqual(110000, car.Mileage);

            car = new Car();
            CraiglistCarPage.UpdateMileage(car, "110001");
            Assert.AreEqual(110001, car.Mileage);
        }

        [TestMethod()]
        public void UpdateAllUsingTitle()
        {
            car = new Car();
            CraiglistCarPage.UpdateAll(car, "2003 toyota camry 105k $$$4900$$");
            Assert.AreEqual(105000, car.Mileage, "2003 toyota camry 105k $$$4900$$");
            Assert.AreEqual(2003, car.Year, "2003 toyota camry 105k $$$4900$$");
            Assert.AreEqual(4900, car.Price, "2003 toyota camry 105k $$$4900$$");

            car = new Car();
            CraiglistCarPage.UpdateAll(car, "toyota Camry 2005 LE/X low miles110k with sunroof");
            Assert.AreEqual(110000, car.Mileage);
            Assert.AreEqual(2005, car.Year);
            Assert.AreEqual(0, car.Price);

            car = new Car();
            CraiglistCarPage.UpdateAll(car, "2015 Toyota Camry SE 1 owner 20k miles");
            Assert.AreEqual(20000, car.Mileage);
            Assert.AreEqual(2015, car.Year);
            Assert.AreEqual(0, car.Price);

            car = new Car();
            CraiglistCarPage.UpdateAll(car, "2007***TOYOTA*** CAMRY ***LE");
            Assert.AreEqual(0, car.Mileage);
            Assert.AreEqual(2007, car.Year);
            Assert.AreEqual(0, car.Price);

            car = new Car();
            CraiglistCarPage.UpdateAll(car, "2011 Toyota Camry 80.5k miles");
            Assert.AreEqual(80500, car.Mileage);
            Assert.AreEqual(2011, car.Year);
            Assert.AreEqual(0, car.Price);

            car = new Car();
            CraiglistCarPage.UpdateAll(car, "03 MITSUBISHI LANCER CLEAN TITLE LOW MILES WITH 87K!!");
            Assert.AreEqual(87000, car.Mileage, "03 MITSUBISHI LANCER CLEAN TITLE LOW MILES WITH 87K!!");
            Assert.AreEqual(2003, car.Year, "03 MITSUBISHI LANCER CLEAN TITLE LOW MILES WITH 87K!!");
            Assert.AreEqual(0, car.Price, "03 MITSUBISHI LANCER CLEAN TITLE LOW MILES WITH 87K!!");
        }
    }
}