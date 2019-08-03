using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderTaker.Models
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public int CustomerId { get; set; }
        public int ShippingAddressId { get; set; }
        public int Discount { get; set; }
        public decimal ShippingFee { get; set; }
        public decimal Tax { get; set; }
        public decimal OrderTotal { get; set; }
        public DateTime? ShippingDate { get; set; }

        public Customer Customer { get; set; }
        public Address ShippingAddress { get; set; }
        public List<OrderItem> OrderItems { get; set; }

        public static Order GetOrderFromCart(ShoppingCart cart,
            Customer customer, Address shippingAddress)
        {
            var order = new Order();

            order.Customer = customer;
            if (customer != null)
                order.CustomerId = order.Customer.Id;
            order.ShippingAddress = shippingAddress;
            if (shippingAddress != null)
                order.ShippingAddressId = shippingAddress.Id;

            order.OrderDate = DateTime.UtcNow;
            order.OrderItems = new List<OrderItem>();
            foreach (var item in cart.CartItems)
            {
                var orderItem = new OrderItem()
                {
                    Product = item.Product,
                    ProductId = item.Product.Id,
                    Quantity = item.Quantity,
                };
                orderItem.TotalPrice = item.Product.UnitPrice * item.Quantity;
                order.OrderItems.Add(orderItem);
            }
            order.Discount = 0;
            order.OrderTotal = order.OrderItems.Sum(i => i.TotalPrice) + order.Tax + order.ShippingFee;

            return order;
        }
    }
}
