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

            return GetProducts(queryString, null);
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
                    "ProductName like '%' + @productName + '%' ";

            var queryParam = new SqlParameter("productName", DbType.String);
            queryParam.Value = searchString;

            return GetProducts(queryString, new SqlParameter[] { queryParam });
        }

        public Product GetItem(int key)
        {
            var queryString =
                "select " +
                    "Id, ProductName, UnitPrice, Active, Description, Image " +
                "from " +
                    "Products " +
                "where " +
                    "Id = @productId ";

            var queryParam = new SqlParameter("productId", DbType.Int32);
            queryParam.Value = key;

            return GetProducts(queryString, new SqlParameter[] { queryParam }).FirstOrDefault();
        }

        private static IEnumerable<Product> GetProducts(string queryString, params SqlParameter[] queryParams)
        {
            var products = new List<Product>();

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
            throw new NotImplementedException();
        }

        public void UpdateItem(int key, Product updatedItem)
        {
            throw new NotImplementedException();
        }

        public void DeleteItem(int key)
        {
            throw new NotImplementedException();
        }
    }
}
