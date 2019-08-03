using OrderTaker.Models;
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
    public class ProductRepository : IRepository<Product>
    {
        public IEnumerable<Product> GetItems()
        {
            var queryString =
                "select " +
                    "Id, ProductName, UnitPrice, Active, Description, Image " +
                "from " +
                    "Products " +
                "where " +
                    "Active = 'true' ";

            return GetProducts(queryString);

            /*
                        // Event Logo
                        MemoryStream ms = new MemoryStream();
                        BinaryWriter bw = new BinaryWriter(ms);

                        int bufferSize = 10000;
                        byte[] outbyte = new byte[bufferSize];
                        long retval;
                        long startIndex = 0;

                        retval = dr.GetBytes(5, startIndex, outbyte, 0, bufferSize);

                        while (retval == bufferSize)
                        {
                            bw.Write(outbyte);
                            bw.Flush();

                            startIndex += bufferSize;
                            retval = dr.GetBytes(5, startIndex, outbyte, 0, bufferSize);
                        }
                        if (retval > 0)
                        {
                            bw.Write(outbyte, 0, (int)retval);
                            bw.Flush();
                        }
                        if (ms.Length > 0)
                            _eventLogo = Image.FromStream(ms);
             */
        }

        public IEnumerable<Product> GetItems(int filterKey)
        {
            return GetItems();
        }

        public IEnumerable<Product> SearchByName(string searchString)
        {
            var queryString =
                "select " +
                    "Id, ProductName, UnitPrice, Active, Description, Image " +
                "from " +
                    "Products " +
                "where " +
                    "Active = 'true' " +
                "and " +
                    "ProductName like '%" + searchString + "%'";

            return GetProducts(queryString);
        }

        public Product GetItem(int key)
        {
            var queryString =
                "select " +
                    "Id, ProductName, UnitPrice, Active, Description, Image " +
                "from " +
                    "Products " +
                "where " +
                    "Id = " + key;

            return GetProducts(queryString).FirstOrDefault();
        }

        private static IEnumerable<Product> GetProducts(string queryString)
        {
            var products = new List<Product>();

            using (SqlConnection connection = new SqlConnection(Database.OrderTakerConnection))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(queryString, connection))
                {
                    using (IDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var product = new Product();
                            product.Id = reader.GetInt32(0);
                            product.ProductName = reader.GetString(1);
                            product.UnitPrice = reader.GetDecimal(2);
                            product.IsActive = reader.GetBoolean(3);
                            if (!reader.IsDBNull(4))
                                product.Description = reader.GetString(4);
                            //product.ImageUrl = "http://";
                            products.Add(product);
                        }
                    }
                }
            }

            return products;
        }

        public void AddItem(Product newItem)
        {
            var commandString =
                "insert into Products " +
                    "(ProductName, UnitPrice, Active, Description) " +
                "values " +
                    "('" + newItem.ProductName + "', " +
                    newItem.UnitPrice + ", " +
                    "'" + newItem.IsActive + "', " +
                    "'" + newItem.Description + "') ";

            ExecuteCommand(commandString);
        }

        public void UpdateItem(int key, Product updatedItem)
        {
            var commandString =
                "update Products " +
                    "set ProductName = '" + updatedItem.ProductName + "', " +
                    "UnitPrice = " + updatedItem.UnitPrice + ", " +
                    "Active = '" + updatedItem.IsActive + "', " +
                    "Description = '" + updatedItem.Description + "' " +
                "where " +
                     "ProductId = " + key;

            ExecuteCommand(commandString);
        }

        public void DeleteItem(int key)
        {
            var commandString =
                "delete from Products " +
                "where " +
                    "ProductId = " + key;

            ExecuteCommand(commandString);
        }

        private static void ExecuteCommand(string commandString)
        {
            using (SqlConnection connection = new SqlConnection(Database.OrderTakerConnection))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(commandString, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
