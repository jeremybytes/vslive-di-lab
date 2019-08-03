using OrderTaker.Models;
using OrderTaker.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderTaker.Repository.LinqToSQL
{
    public class OrderItemRepository : IRepository<OrderItem>
    {
        public IEnumerable<OrderItem> GetItems()
        {
            var orderItems = new List<OrderItem>();
            using (var ctx = new OrderTakerDataContext(Database.OrderTakerConnection))
            {
                var dataOrderItems = from oi in ctx.DataOrderItems
                                     select oi;

                foreach (var dataOrderItem in dataOrderItems)
                {
                    var orderItem = ModelConverters.GetOrderItemFromDataOrderItem(dataOrderItem);
                    orderItem.Product = ModelConverters.GetProductFromDataProduct(dataOrderItem.DataProduct);
                    orderItems.Add(orderItem);
                }
            }
            return orderItems;
        }

        public IEnumerable<OrderItem> GetItems(int filterKey)
        {
            var orderItems = new List<OrderItem>();
            using (var ctx = new OrderTakerDataContext(Database.OrderTakerConnection))
            {
                var dataOrderItems = from oi in ctx.DataOrderItems
                                     where oi.OrderId == filterKey
                                     select oi;

                foreach (var dataOrderItem in dataOrderItems)
                {
                    var orderItem = ModelConverters.GetOrderItemFromDataOrderItem(dataOrderItem);
                    orderItem.Product = ModelConverters.GetProductFromDataProduct(dataOrderItem.DataProduct);
                    orderItems.Add(orderItem);
                }
            }
            return orderItems;
        }

        public IEnumerable<OrderItem> SearchByName(string searchString)
        {
            var orderItems = new List<OrderItem>();
            using (var ctx = new OrderTakerDataContext(Database.OrderTakerConnection))
            {
                var dataOrderItems = from oi in ctx.DataOrderItems
                                     where oi.DataProduct.ProductName.Contains(searchString)
                                     select oi;

                foreach (var dataOrderItem in dataOrderItems)
                {
                    var orderItem = ModelConverters.GetOrderItemFromDataOrderItem(dataOrderItem);
                    orderItem.Product = ModelConverters.GetProductFromDataProduct(dataOrderItem.DataProduct);
                    orderItems.Add(orderItem);
                }
            }
            return orderItems;
        }

        public OrderItem GetItem(int key)
        {
            OrderItem orderItem = null;
            using (var ctx = new OrderTakerDataContext(Database.OrderTakerConnection))
            {
                var dataOrderItem = (from oi in ctx.DataOrderItems
                                     where oi.Id == key
                                     select oi).FirstOrDefault();

                if (dataOrderItem != null)
                {
                    orderItem = ModelConverters.GetOrderItemFromDataOrderItem(dataOrderItem);
                    orderItem.Product = ModelConverters.GetProductFromDataProduct(dataOrderItem.DataProduct);
                }
            }
            return orderItem;
        }

        public void AddItem(OrderItem newItem)
        {
            throw new NotImplementedException();
        }

        public void UpdateItem(int key, OrderItem updatedItem)
        {
            throw new NotImplementedException();
        }

        public void DeleteItem(int key)
        {
            throw new NotImplementedException();
        }
    }
}
