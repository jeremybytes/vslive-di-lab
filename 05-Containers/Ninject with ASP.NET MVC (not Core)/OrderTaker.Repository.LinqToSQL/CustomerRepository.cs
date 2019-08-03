using OrderTaker.Models;
using OrderTaker.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderTaker.Repository.LinqToSQL
{
    public class CustomerRepository : IRepository<Customer>
    {
        public IEnumerable<Customer> GetItems()
        {
            var customers = new List<Customer>();
            using (var ctx = new OrderTakerDataContext(Database.OrderTakerConnection))
            {
                var dataCustomers = from c in ctx.DataCustomers
                                    select c;

                foreach (var dataCustomer in dataCustomers)
                    customers.Add(ModelConverters.GetCustomerFromDataCustomer(dataCustomer));
            }
            return customers;
        }

        public IEnumerable<Customer> GetItems(int filterKey)
        {
            return GetItems();
        }

        public IEnumerable<Customer> SearchByName(string searchString)
        {
            var customers = new List<Customer>();
            using (var ctx = new OrderTakerDataContext(Database.OrderTakerConnection))
            {
                var dataCustomers = from c in ctx.DataCustomers
                                    where c.FirstName.Contains(searchString) ||
                                          c.LastName.Contains(searchString)
                                    select c;

                foreach (var dataCustomer in dataCustomers)
                    customers.Add(ModelConverters.GetCustomerFromDataCustomer(dataCustomer));
            }
            return customers;
        }

        public Customer GetItem(int key)
        {
            Customer customer = null;
            using (var ctx = new OrderTakerDataContext(Database.OrderTakerConnection))
            {
                var dataCustomer = (from c in ctx.DataCustomers
                                    where c.Id == key
                                    select c).FirstOrDefault();

                customer = ModelConverters.GetCustomerFromDataCustomer(dataCustomer);
            }
            return customer;
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
