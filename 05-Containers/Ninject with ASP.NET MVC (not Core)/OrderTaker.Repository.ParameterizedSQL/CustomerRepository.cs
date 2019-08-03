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
    public class CustomerRepository : IRepository<Customer>
    {
        public IEnumerable<Customer> GetItems()
        {
            var queryString =
                "select " +
                    "Id, FirstName, LastName, StartDate, Rating, LoginName " +
                "from " +
                    "Customers " + 
                "order by " +
                    "LastName ";

            return GetCustomers(queryString, null);
        }

        public IEnumerable<Customer> GetItems(int filterKey)
        {
            return GetItems();
        }

        public IEnumerable<Customer> SearchByName(string searchString)
        {
            var queryString =
                "select " +
                    "Id, FirstName, LastName, StartDate, Rating, LoginName " +
                "from " +
                    "Customers " +
                "where " +
                    "c.FirstName like '% + @customerName + %' " +
                "or " +
                    "c.LastName like '% + @customerName + %' ";

            var queryParam = new SqlParameter("customerName", DbType.Int32);
            queryParam.Value = searchString;

            return GetCustomers(queryString, new SqlParameter[] { queryParam });
        }

        public Customer GetItem(int key)
        {
            var queryString =
                "select " +
                    "Id, FirstName, LastName, StartDate, Rating, LoginName " +
                "from " +
                    "Customers " +
                "where " +
                    "Id = @customerId ";

            var queryParam = new SqlParameter("customerId", DbType.Int32);
            queryParam.Value = key;

            return GetCustomers(queryString, new SqlParameter[] { queryParam }).FirstOrDefault();
        }

        private static IEnumerable<Customer> GetCustomers(string queryString, params SqlParameter[] queryParams)
        {
            var customers = new List<Customer>();

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
                            var customer = new Customer();
                            customer.Id = reader.GetInt32(0);
                            customer.FirstName = reader.GetString(1);
                            customer.LastName = reader.GetString(2);
                            customer.StartDate = reader.GetDateTime(3);
                            customer.Rating = reader.GetInt32(4);
                            customer.LoginName = reader.GetString(5);
                            customers.Add(customer);
                        }
                    }
                }
            }

            return customers;
        }

        public void AddItem(Customer newItem)
        {
            throw new NotImplementedException();
        }

        public void UpdateItem(int key, Customer updatedItem)
        {
            throw new NotImplementedException();
        }

        public void DeleteItem(int key)
        {
            throw new NotImplementedException();
        }
    }
}
