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

            return GetCustomers(queryString);
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
                    "FirstName like '%" + searchString + "%' " +
                "or " +
                    "LastName like '%" + searchString + "%' ";

            return GetCustomers(queryString);
        }

        private static IEnumerable<Customer> GetCustomers(string queryString)
        {
            var customers = new List<Customer>();

            using (SqlConnection connection = new SqlConnection(Database.OrderTakerConnection))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(queryString, connection))
                {
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

        public Customer GetItem(int key)
        {
            var queryString =
                "select " +
                    "Id, FirstName, LastName, StartDate, Rating, LoginName " +
                "from " +
                    "Customers " +
                "where " +
                    "Id = " + key;

            return GetCustomers(queryString).FirstOrDefault();
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
