using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderTaker.Repository.ParameterizedSQL
{
    public static class Database
    {
        public static string OrderTakerConnection = 
            ConfigurationManager.ConnectionStrings["OrderTakerDb"].ConnectionString;
    }
}
