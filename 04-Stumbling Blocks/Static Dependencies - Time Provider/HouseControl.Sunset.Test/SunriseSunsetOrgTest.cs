using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace HouseControl.Sunset.Test
{
    [TestClass]
    public class SunriseSunsetOrgTest
    {
        string responseContent = "{\"results\":{\"sunrise\":\"2:56:10 PM\",\"sunset\":\"1:06:09 AM\",\"solar_noon\":\"8:01:09 PM\",\"day_length\":\"10:09:59\",\"civil_twilight_begin\":\"2:29:14 PM\",\"civil_twilight_end\":\"1:33:05 AM\",\"nautical_twilight_begin\":\"1:58:37 PM\",\"nautical_twilight_end\":\"2:03:42 AM\",\"astronomical_twilight_begin\":\"1:28:38 PM\",\"astronomical_twilight_end\":\"2:33:41 AM\"},\"status\":\"OK\"}";
        string errorResponseContent = "{\"results\":{},\"status\":\"ERROR\"}";

        [TestMethod]
        public void GetSunsetString_WithValidResponse_ReturnsUTCString()
        {
            var expected = "1:06:09 AM";
            var result = SunriseSunsetOrg.GetSunsetString(responseContent);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void GetSunsetString_WithNullResponse_ReturnsNull()
        {
            var result = SunriseSunsetOrg.GetSunsetString(null);
            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetSunsetString_WithErrorStatusResponse_ReturnsNull()
        {
            var result = SunriseSunsetOrg.GetSunsetString(errorResponseContent);
            Assert.IsNull(result);
        }

        [TestMethod]
        [ExpectedException(typeof(JsonReaderException))]
        public void GetSunsetString_WithInvalidObject_ThrowsException()
        {
            var result = SunriseSunsetOrg.GetSunsetString("Hello");
        }

        [TestMethod]
        public void GetSunriseString_WithValidResponse_ReturnsUTCString()
        {
            var expected = "2:56:10 PM";
            var result = SunriseSunsetOrg.GetSunriseString(responseContent);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void GetSunriseString_WithNullResponse_ReturnsNull()
        {
            var result = SunriseSunsetOrg.GetSunriseString(null);
            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetSunriseString_WithErrorStatusResponse_ReturnsNull()
        {
            var result = SunriseSunsetOrg.GetSunriseString(errorResponseContent);
            Assert.IsNull(result);
        }

        [TestMethod]
        [ExpectedException(typeof(JsonReaderException))]
        public void GetSunriseString_WithInvalidObject_ThrowsException()
        {
            var result = SunriseSunsetOrg.GetSunriseString("Hello");
        }

        [TestMethod]
        public void GetLocalTime_WithTodaysDate_IsToday()
        {
            var timeString = "1:06:09 AM";
            var date = DateTime.Today;

            var result = SunriseSunsetOrg.GetLocalTime(timeString, date);
            Assert.AreEqual(DateTime.Today, result.Date);
        }
    }
}
