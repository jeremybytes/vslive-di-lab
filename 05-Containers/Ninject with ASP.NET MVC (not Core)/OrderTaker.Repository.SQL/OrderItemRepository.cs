﻿using OrderTaker.Models;
using OrderTaker.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderTaker.Repository.SQL
{
    public class OrderItemRepository : IRepository<OrderItem>
    {
        private IRepository<Product> productRepo;

        public OrderItemRepository(IRepository<Product> productRepo)
        {
            this.productRepo = productRepo;
        }

        private Product GetProduct(int productId)
        {
            return productRepo.GetItem(productId);
        }

        public IEnumerable<OrderItem> GetItems()
        {
            var queryString =
                "select " +
                    "Id, OrderId, ProductId, Quantity, TotalPrice " +
                "from " +
                    "OrderItems ";

            return GetOrderItems(queryString);
        }

        public IEnumerable<OrderItem> GetItems(int filterKey)
        {
            var queryString =
                "select " +
                    "Id, OrderId, ProductId, Quantity, TotalPrice " +
                "from " +
                    "OrderItems " +
                "where " +
                    "OrderId = " + filterKey;

            return GetOrderItems(queryString);
        }

        public IEnumerable<OrderItem> SearchByName(string searchString)
        {
            var queryString =
                "select " +
                    "oi.Id, OrderId, ProductId, Quantity, TotalPrice " +
                "from " +
                    "OrderItems oi " +
                "join Products p " +
                    "on oi.ProductId = p.Id " +
                "where " +
                    "p.ProductName like '%" + searchString + "%'";

            return GetOrderItems(queryString);
        }

        public OrderItem GetItem(int key)
        {
            var queryString =
                "select " +
                    "Id, OrderId, ProductId, Quantity, TotalPrice " +
                "from " +
                    "OrderItems " +
                "where " +
                    "Id = " + key;

            return GetOrderItems(queryString).FirstOrDefault();
        }

        private IEnumerable<OrderItem> GetOrderItems(string queryString)
        {
            var orderItems = new List<OrderItem>();

            using (SqlConnection connection = new SqlConnection(Database.OrderTakerConnection))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(queryString, connection))
                {
                    using (IDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var orderItem = new OrderItem();
                            orderItem.Id = reader.GetInt32(0);
                            orderItem.OrderId = reader.GetInt32(1);
                            orderItem.ProductId = reader.GetInt32(2);
                            orderItem.Quantity = reader.GetInt32(3);
                            orderItem.TotalPrice = reader.GetDecimal(4);

                            orderItem.Product = GetProduct(orderItem.ProductId);

                            orderItems.Add(orderItem);
                        }
                    }
                }
            }

            return orderItems;
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
