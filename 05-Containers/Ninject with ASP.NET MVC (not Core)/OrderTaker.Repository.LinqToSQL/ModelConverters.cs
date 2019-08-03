using OrderTaker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderTaker.Repository.LinqToSQL
{
    public static class ModelConverters
    {
        public static Address GetAddressFromDataAddress(DataAddress dataAddress)
        {
            return new Address()
            {
                Id = dataAddress.Id,
                CustomerId = dataAddress.CustomerId,
                StreetAddress1 = dataAddress.StreetAddress1,
                StreetAddress2 = dataAddress.StreetAddress2,
                City = dataAddress.City,
                State = dataAddress.State,
                PostCode = dataAddress.PostCode,
                Country = dataAddress.Country,
            };
        }

        public static Customer GetCustomerFromDataCustomer(DataCustomer dataCustomer)
        {
            return new Customer()
            {
                Id = dataCustomer.Id,
                FirstName = dataCustomer.FirstName,
                LastName = dataCustomer.LastName,
                StartDate = dataCustomer.StartDate,
                Rating = dataCustomer.Rating,
                LoginName = dataCustomer.LoginName,
            };
        }

        public static OrderItem GetOrderItemFromDataOrderItem(DataOrderItem dataOrderItem)
        {
            return new OrderItem()
            {
                Id = dataOrderItem.Id,
                OrderId = dataOrderItem.OrderId,
                ProductId = dataOrderItem.ProductId,
                Quantity = dataOrderItem.Quantity,
                TotalPrice = dataOrderItem.TotalPrice,
            };
        }

        public static Order GetOrderFromDataOrder(DataOrder dataOrder)
        {
            var order = new Order()
            {
                Id = dataOrder.Id,
                OrderDate = dataOrder.OrderDate,
                CustomerId = dataOrder.CustomerId,
                ShippingAddressId = dataOrder.ShippingAddressId,
                Discount = dataOrder.Discount,
                ShippingFee = dataOrder.ShippingFee,
                Tax = dataOrder.Tax,
                OrderTotal = dataOrder.OrderTotal,
                ShippingDate = dataOrder.ShippingDate,
            };

            order.OrderItems = new List<OrderItem>();
            foreach(var dataOrderItem in dataOrder.DataOrderItems)
            {
                var orderItem = GetOrderItemFromDataOrderItem(dataOrderItem);
                order.OrderItems.Add(orderItem);
            }

            return order;
        }

        public static Product GetProductFromDataProduct(DataProduct dataProduct)
        {
            return new Product()
            {
                Id = dataProduct.Id,
                ProductName = dataProduct.ProductName,
                UnitPrice = dataProduct.UnitPrice,
                IsActive = dataProduct.Active,
                Description = dataProduct.Description,
                ImageUrl = null,
            };
        }

        public static DataAddress GetDataAddressFromAddress(Address address)
        {
            return new DataAddress()
            {
                Id = address.Id,
                CustomerId = address.CustomerId,
                StreetAddress1 = address.StreetAddress1,
                StreetAddress2 = address.StreetAddress2,
                City = address.City,
                State = address.State,
                PostCode = address.PostCode,
                Country = address.Country,
            };
        }

        public static DataCustomer GetDataCustomerFromCustomer(Customer customer)
        {
            return new DataCustomer()
            {
                Id = customer.Id,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                StartDate = customer.StartDate,
                Rating = customer.Rating,
                LoginName = customer.LoginName,
            };
        }

        public static DataOrderItem GetDataOrderItemFromOrderItem(OrderItem orderItem)
        {
            return new DataOrderItem()
            {
                Id = orderItem.Id,
                OrderId = orderItem.OrderId,
                ProductId = orderItem.ProductId,
                Quantity = orderItem.Quantity,
                TotalPrice = orderItem.TotalPrice,
            };
        }

        public static DataOrder GetDataOrderFromOrder(Order order)
        {
            var dataOrder = new DataOrder()
            {
                Id = order.Id,
                OrderDate = order.OrderDate,
                CustomerId = order.CustomerId,
                ShippingAddressId = order.ShippingAddressId,
                Discount = order.Discount,
                ShippingFee = order.ShippingFee,
                Tax = order.Tax,
                OrderTotal = order.OrderTotal,
                ShippingDate = order.ShippingDate,
            };

            if (dataOrder.CustomerId != 0)
            {
                dataOrder.DataCustomer = GetDataCustomerFromCustomer(order.Customer);
            }

            if (dataOrder.ShippingAddressId != 0)
            {
                dataOrder.DataAddress = GetDataAddressFromAddress(order.ShippingAddress);
            }

            foreach (var orderItem in order.OrderItems)
            {
                var dataOrderItem = GetDataOrderItemFromOrderItem(orderItem);
                dataOrder.DataOrderItems.Add(dataOrderItem);
            }

            return dataOrder;
        }

        public static DataProduct GetDataProductFromProduct(Product product)
        {
            return new DataProduct()
            {
                Id = product.Id,
                ProductName = product.ProductName,
                UnitPrice = product.UnitPrice,
                Active = product.IsActive,
                Description = product.Description,
                Image = null,
            };
        }
    }
}
