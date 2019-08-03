using NUnit.Framework;
using Sunset.Interface;
using System;

namespace Sunset.Library.Tests
{
    public class SunsetCalculatorTests
    {
        [Test]
        public void SunsetCalculator_ImplementsISunsetCalculator()
        {
            var calculator = new SunsetCalculator();
            Assert.IsInstanceOf(typeof(ISunsetCalculator), calculator);
        }

        //[Test]
        //public void ConvertToLocalTime_OnValidTimeString_ReturnsDateTime()
        //{
        //    string inputTime = "8:25:51 PM";
        //    DateTime date = new DateTime(2019, 08, 13);
        //    DateTime expected = new DateTime(2019, 08, 13, 20, 25, 51);

        //    DateTime result = SunsetCalculator.ToLocalTime(inputTime, date);

        //    Assert.AreEqual(expected, result);
        //}

        //string goodResult = "{\"results\":{\"sunrise\":\"6:01:04 AM\",\"sunset\":\"8:25:51 PM\",\"solar_noon\":\"1:13:28 PM\",\"day_length\":\"14:24:46.7200000\"},\"status\":\"OK\"}";

        //[Test]
        //public void ParseSunsetTime_OnValidData_ReturnsExpectedString()
        //{
        //    string expected = "8:25:51 PM";

        //    string result = SunsetCalculator.ParseSunsetTime(goodResult);

        //    Assert.AreEqual(expected, result);
        //}

        //[Test]
        //public void GetSunset_WithValidDate_ReturnsExpectedDateTime()
        //{
        //    DateTime inputDate = new DateTime(2019, 08, 13);
        //    DateTime expected = new DateTime(2019, 08, 13, 20, 25, 51);

        //    var calculator = new SunsetCalculator();

        //    DateTime result = calculator.GetSunset(inputDate);

        //    Assert.AreEqual(expected, result);
        //}

        //[Test]
        //public void ParseSunriseTime_OnValidData_ReturnsExpectedString()
        //{
        //    string expected = "6:01:04 AM";

        //    string result = SunsetCalculator.ParseSunriseTime(goodResult);

        //    Assert.AreEqual(expected, result);
        //}

        //[Test]
        //public void GetSunrise_WithValidDate_ReturnsExpectedDateTime()
        //{
        //    DateTime inputDate = new DateTime(2019, 08, 13);
        //    DateTime expected = new DateTime(2019, 08, 13, 06, 01, 04);

        //    var calculator = new SunsetCalculator();
        //    calculator.Service = new GoodService();

        //    DateTime result = calculator.GetSunrise(inputDate);

        //    Assert.AreEqual(expected, result);
        //}
    }
}