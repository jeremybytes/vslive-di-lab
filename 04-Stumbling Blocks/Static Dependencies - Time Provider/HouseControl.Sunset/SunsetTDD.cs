using Newtonsoft.Json;
using System;
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Headers;

namespace HouseControl.Sunset
{
    public class SunsetTDD : ISunsetProvider
    {
        private ISunsetService sunsetService;
        public ISunsetService SunsetService
        {
            get
            {
                if (sunsetService == null)
                    sunsetService = new SunriseSunsetOrgService();
                return sunsetService;
            }
            set
            {
                if (sunsetService == value)
                    return;
                sunsetService = value;
            }
        }

        private DateTime cacheDataDate;
        private string cacheServiceData;

        public DateTime GetSunset(DateTime date)
        {
            string serviceData = GetServiceData(date);
            string utcTimeString = ParseSunset(serviceData);
            DateTime localTime = GetLocalTime(utcTimeString, date);
            return localTime;
        }

        public DateTime GetSunrise(DateTime date)
        {
            string serviceData = GetServiceData(date);
            string utcTimeString = ParseSunrise(serviceData);
            DateTime localTime = GetLocalTime(utcTimeString, date);
            return localTime;
        }

        private string GetServiceData(DateTime date)
        {
            if (cacheDataDate != date)
            {
                cacheDataDate = date;
                cacheServiceData = SunsetService.GetServiceData(date);
            }
            return cacheServiceData;
        }

        public static bool CheckStatus(string sampleData)
        {
            dynamic data = JsonConvert.DeserializeObject(sampleData);
            return data.status == "OK";
        }

        public static string ParseSunset(string serviceData)
        {
            if (!CheckStatus(serviceData))
                return null;
            dynamic data = JsonConvert.DeserializeObject(serviceData);
            return data.results.sunset;
        }

        public static string ParseSunrise(string serviceData)
        {
            if (!CheckStatus(serviceData))
                return null;
            dynamic data = JsonConvert.DeserializeObject(serviceData);
            return data.results.sunrise;
        }

        public static DateTime GetLocalTime(string utcTimeString, DateTime currentDate)
        {
            if (utcTimeString == null)
                return currentDate.Date;
            DateTime sunsetTime = DateTime.Parse(utcTimeString,
                CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);
            DateTime localTime = currentDate.Date + sunsetTime.TimeOfDay;
            return localTime;
        }
    }

    public interface ISunsetService
    {
        string GetServiceData(DateTime date);
    }

    public class SunriseSunsetOrgService : ISunsetService
    {
        public string GetServiceData(DateTime date)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://api.sunrise-sunset.org/");
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
                var apiString = string.Format(
                    "json?lat=33.8361&lng=-117.8897&date={0:yyyy-MM-dd}", date);
                HttpResponseMessage response = client.GetAsync(apiString).Result;

                if (response.IsSuccessStatusCode)
                    return response.Content.ReadAsStringAsync().Result;
                return null;
            }
        }
    }
}
