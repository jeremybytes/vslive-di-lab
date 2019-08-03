using OrderTaker.Models;
using OrderTaker.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderTaker.Repository.ParameterizedSQL
{
    public class OrderRepository : IRepository<Order>
    {
        private IRepository<Customer> customerRepo;
        private IRepository<Address> addressRepo;
        private IRepository<OrderItem> orderItemRepo;

        public OrderRepository(IRepository<Customer> customerRepo,
            IRepository<Address> addressRepo, IRepository<OrderItem> orderItemRepo)
        {
            this.customerRepo = customerRepo;
            this.addressRepo = addressRepo;
            this.orderItemRepo = orderItemRepo;
        }

        private Customer GetCustomer(int customerId)
        {
            return customerRepo.GetItem(customerId);
        }

        private Address GetShippingAddress(int shippingAddressId)
        {
            return addressRepo.GetItem(shippingAddressId);
        }

        private List<OrderItem> GetOrderItems(int orderId)
        {
            return orderItemRepo.GetItems(orderId).ToList();
        }

        public IEnumerable<Order> GetItems()
        {
            var queryString =
                "select " +
                    "Id, OrderDate, CustomerId, ShippingAddressId, Discount, ShippingFee, Tax, OrderTotal, ShippingDate " +
                "from " +
                    "Orders ";

            return GetOrders(queryString, null);
        }

        public IEnumerable<Order> GetItems(int filterKey)
        {
            var queryString =
                "select " +
                    "Id, OrderDate, CustomerId, ShippingAddressId, Discount, ShippingFee, Tax, OrderTotal, ShippingDate " +
                "from " +
                    "Orders " +
                "where " +
                    "CustomerId = @customerId ";

            var queryParam = new SqlParameter("customerId", DbType.Int32);
            queryParam.Value = filterKey;

            return GetOrders(queryString, new SqlParameter[] { queryParam });
        }

        public IEnumerable<Order> SearchByName(string searchString)
        {
            var queryString =
                "select " +
                    "o.Id, OrderDate, CustomerId, ShippingAddressId, Discount, ShippingFee, Tax, OrderTotal, ShippingDate " +
                "from " +
                    "Orders o " +
                "join Customers c " +
                    "on a.CustomerId = c.Id " +
                "where " +
                    "c.FirstName like '% + @customerName + %' " +
                "or " +
                    "c.LastName like '% + @customerName + %' ";

            var queryParam = new SqlParameter("customerName", DbType.Int32);
            queryParam.Value = searchString;

            return GetOrders(queryString, new SqlParameter[] { queryParam });
        }

        public Order GetItem(int key)
        {
            var queryString =
                "select " +
                    "Id, OrderDate, CustomerId, ShippingAddressId, Discount, ShippingFee, Tax, OrderTotal, ShippingDate " +
                "from " +
                    "Orders " +
                "where " +
                    "Id = @orderId ";

            var queryParam = new SqlParameter("orderId", DbType.Int32);
            queryParam.Value = key;

            return GetOrders(queryString, new SqlParameter[] { queryParam }).FirstOrDefault();
        }

        private IEnumerable<Order> GetOrders(string queryString, params SqlParameter[] queryParams)
        {
            var orders = new List<Order>();

            using (SqlConnection connection = new SqlConnection(Database.OrderTakerConnection))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(queryString, connection))
                {
                    if (queryParams != null)
                        foreach (var queryParam in queryParams)
                            command.Parameters.Add(queryParam);

                    using (IDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var order = new Order();
                            order.Id = reader.GetInt32(0);
                            order.OrderDate = reader.GetDateTime(1);
                            order.CustomerId = reader.GetInt32(2);
                            order.ShippingAddressId = reader.GetInt32(3);
                            order.Discount = reader.GetInt32(4);
                            order.ShippingFee = reader.GetDecimal(5);
                            order.Tax = reader.GetDecimal(6);
                            order.OrderTotal = reader.GetDecimal(7);
                            if (!reader.IsDBNull(8))
                                order.ShippingDate = reader.GetDateTime(8);

                            order.Customer = GetCustomer(order.CustomerId);
                            order.ShippingAddress = GetShippingAddress(order.ShippingAddressId);
                            order.OrderItems = GetOrderItems(order.Id);

                            orders.Add(order);
                        }
                    }
                }
            }

            return orders;
        }

        public void AddItem(Order newItem)
        {
            throw new NotImplementedException();
        }

        public void UpdateItem(int key, Order updatedItem)
        {
            throw new NotImplementedException();
        }

        public void DeleteItem(int key)
        {
            throw new NotImplementedException();
        }
    }
}
