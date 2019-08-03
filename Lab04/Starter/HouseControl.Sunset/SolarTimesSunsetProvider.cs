using Innovative.SolarCalculator;
using System;

namespace HouseControl.Sunset
{
    public class SolarTimesSunsetProvider : ISunsetProvider
    {
        public DateTimeOffset GetSunset(DateTime date)
        {
            var solarTimes = new SolarTimes(date, 33.8361, -117.8897);
            return new DateTimeOffset(solarTimes.Sunset);
        }

        public DateTimeOffset GetSunrise(DateTime date)
        {
            var solarTimes = new SolarTimes(date, 33.8361, -117.8897);
            return new DateTimeOffset(solarTimes.Sunrise);
        }
    }
}
