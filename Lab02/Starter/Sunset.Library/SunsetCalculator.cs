using System;
using Newtonsoft.Json;
using Sunset.Interface;

namespace Sunset.Library
{
    public class SunsetCalculator
    {
        public DateTime GetSunrise(DateTime date)
        {
            throw new NotImplementedException();
        }

        public DateTime GetSunset(DateTime date)
        {
            // Step 3b: Get data from services

            // Step 3a: hardcoded-data (temporary)
            //string goodResult = "{\"results\":{\"sunrise\":\"6:01:04 AM\",\"sunset\":\"8:25:51 PM\",\"solar_noon\":\"1:13:28 PM\",\"day_length\":\"14:24:46.7200000\"},\"status\":\"OK\"}";

            // Step 2: Parse time string from the JSON data

            // Step 1: Convert time string to datetime value

            throw new NotImplementedException();
        }
    }
}
