﻿using System;
using System.Net;

namespace Sunset.Library
{
    public class SolarService
    {
        WebClient client = new WebClient();
        string baseUri = "http://localhost:8973/";


        public string GetServiceData(DateTime date)
        {
            var address = $"{baseUri}api/SolarCalculator/48.5090/-122.2801/{date:yyyy-MM-dd}";
            string reply = client.DownloadString(address);
            return reply;
        }
    }
}
