using System;

namespace Sunset.Library.Tests
{
    public class GoodService : ISolarService
    {
        public string GetServiceData(DateTime date)
        {
            return "{\"results\":{\"sunrise\":\"6:01:04 AM\",\"sunset\":\"8:25:51 PM\",\"solar_noon\":\"1:13:28 PM\",\"day_length\":\"14:24:46.7200000\"},\"status\":\"OK\"}";
        }
    }
}
