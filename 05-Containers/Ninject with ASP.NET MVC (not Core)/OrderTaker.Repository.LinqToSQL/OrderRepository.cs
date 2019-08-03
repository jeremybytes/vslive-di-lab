using OrderTaker.Models;
using OrderTaker.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace OrderTaker.Repository.LinqToSQL
{
    public class OrderRepository : IRepository<Order>
    {
        private IRepository<OrderItem> orderItemRepo;

        public OrderRepository(IRepository<OrderItem> orderItemRepo)
        {
            this.orderItemRepo = orderItemRepo;
        }

        public IEnumerable<Order> GetItems()
        {
            var orders = new List<Order>();
            using (var ctx = new OrderTakerDataContext(Database.OrderTakerConnection))
            {
                var dataOrders = from o in ctx.DataOrders
                                 select o;

                foreach (var dataOrder in dataOrders)
                {
                    var order = ModelConverters.GetOrderFromDataOrder(dataOrder);
                    order.Customer = ModelConverters.GetCustomerFromDataCustomer(dataOrder.DataCustomer);
                    order.ShippingAddress = ModelConverters.GetAddressFromDataAddress(dataOrder.DataAddress);
                    order.OrderItems = orderItemRepo.GetItems(order.Id).ToList();
                    orders.Add(order);
                }
            }
            return orders;
        }

        public IEnumerable<Order> GetItems(int filterKey)
        {
            var orders = new List<Order>();
            using (var ctx = new OrderTakerDataContext(Database.OrderTakerConnection))
            {
                var dataOrders = from o in ctx.DataOrders
                                 where o.CustomerId == filterKey
                                 select o;

                foreach (var dataOrder in dataOrders)
                {
                    var order = ModelConverters.GetOrderFromDataOrder(dataOrder);
                    order.Customer = ModelConverters.GetCustomerFromDataCustomer(dataOrder.DataCustomer);
                    order.ShippingAddress = ModelConverters.GetAddressFromDataAddress(dataOrder.DataAddress);
                    order.OrderItems = orderItemRepo.GetItems(order.Id).ToList();
                    orders.Add(order);
                }
            }
            return orders;
        }

        public IEnumerable<Order> SearchByName(string searchString)
        {
            var orders = new List<Order>();
            using (var ctx = new OrderTakerDataContext(Database.OrderTakerConnection))
            {
                var dataOrders = from o in ctx.DataOrders
                                 where o.DataCustomer.FirstName.Contains(searchString) ||
                                       o.DataCustomer.LastName.Contains(searchString)
                                 select o;

                foreach (var dataOrder in dataOrders)
                {
                    var order = ModelConverters.GetOrderFromDataOrder(dataOrder);
                    order.Customer = ModelConverters.GetCustomerFromDataCustomer(dataOrder.DataCustomer);
                    order.ShippingAddress = ModelConverters.GetAddressFromDataAddress(dataOrder.DataAddress);
                    order.OrderItems = orderItemRepo.GetItems(order.Id).ToList();
                    orders.Add(order);
                }
            }
            return orders;
        }

        public Order GetItem(int key)
        {
            Order order = null;
            using (var ctx = new OrderTakerDataContext(Database.OrderTakerConnection))
            {
                var dataOrder = (from o in ctx.DataOrders
                                 select o).FirstOrDefault();

                if (dataOrder != null)
                {
                    order = ModelConverters.GetOrderFromDataOrder(dataOrder);
                    order.Customer = ModelConverters.GetCustomerFromDataCustomer(dataOrder.DataCustomer);
                    order.ShippingAddress = ModelConverters.GetAddressFromDataAddress(dataOrder.DataAddress);
                    order.OrderItems = orderItemRepo.GetItems(order.Id).ToList();
                }
            }
            return order;
        }

        public void AddItem(Order newItem)
        {
            var dataOrder = ModelConverters.GetDataOrderFromOrder(newItem);

            using (var transaction = new TransactionScope())
            {
                try
                {
                    using (var ctx = new OrderTakerDataContext(Database.OrderTakerConnection))
                    {
                        if (dataOrder.DataAddress.Id == 0)
                        {
                            ctx.DataAddresses.InsertOnSubmit(dataOrder.DataAddress);
                        }
                        ctx.DataOrders.InsertOnSubmit(dataOrder);
                        ctx.DataOrderItems.InsertAllOnSubmit(dataOrder.DataOrderItems);
                        ctx.SubmitChanges();
                        transaction.Complete();
                    }
                }
                catch
                {

                }
            }
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
