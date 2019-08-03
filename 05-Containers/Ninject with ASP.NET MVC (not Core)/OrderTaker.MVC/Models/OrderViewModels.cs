using OrderTaker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrderTaker.MVC.Models
{
    public class CreateOrderViewModel
    {
        public Order Order { get; set; }
        public List<Address> AvailableAddresses { get; set; }
    }
}