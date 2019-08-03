using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseControl.Sunset
{
    public interface ISunsetProvider
    {
        DateTime GetSunset(DateTime date);
        DateTime GetSunrise(DateTime date);
    }
}
