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
    public class AddressRepository : IRepository<Address>
    {
        public IEnumerable<Address> GetItems()
        {
            var queryString =
                "select " +
                    "Id, CustomerId, StreetAddress1, StreetAddress2, City, State, PostCode, Country " +
                "from " +
                    "Addresses ";

            return GetAddresses(queryString, null);
        }

        public IEnumerable<Address> GetItems(int filterKey)
        {
            var queryString =
                "select " +
                    "Id, CustomerId, StreetAddress1, StreetAddress2, City, State, PostCode, Country " +
                "from " +
                    "Addresses " +
                "where " +
                    "CustomerId = @customerID ";

            var queryParam = new SqlParameter("customerId", DbType.Int32);
            queryParam.Value = filterKey;

            return GetAddresses(queryString, new SqlParameter[] { queryParam });
        }

        public IEnumerable<Address> SearchByName(string searchString)
        {
            var queryString =
                "select " +
                    "a.Id, CustomerId, StreetAddress1, StreetAddress2, City, State, PostCode, Country " +
                "from " +
                    "Addresses a " +
                "join Customers c " +
                    "on a.CustomerId = c.Id " +
                "where " +
                    "c.FirstName like '% + @customerName + %' " +
                "or " +
                    "c.LastName like '% + @customerName + %' ";

            var queryParam = new SqlParameter("customerName", DbType.Int32);
            queryParam.Value = searchString;

            return GetAddresses(queryString, new SqlParameter[] { queryParam });
        }

        public Address GetItem(int key)
        {
            var queryString =
                "select " +
                    "Id, CustomerId, StreetAddress1, StreetAddress2, City, State, PostCode, Country " +
                "from " +
                    "Addresses " +
                "where " +
                    "Id = @addressId ";

            var queryParam = new SqlParameter("addressId", DbType.Int32);
            queryParam.Value = key;

            return GetAddresses(queryString, new SqlParameter[] { queryParam }).FirstOrDefault();
        }

        private IEnumerable<Address> GetAddresses(string queryString, params SqlParameter[] queryParams)
        {
            var addresses = new List<Address>();

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
                            var address = new Address();
                            address.Id = reader.GetInt32(0);
                            address.CustomerId = reader.GetInt32(1);
                            if (!reader.IsDBNull(2))
                                address.StreetAddress1 = reader.GetString(2);
                            if (!reader.IsDBNull(3))
                                address.StreetAddress2 = reader.GetString(3);
                            if (!reader.IsDBNull(4))
                                address.City = reader.GetString(4);
                            if (!reader.IsDBNull(5))
                                address.State = reader.GetString(5);
                            if (!reader.IsDBNull(6))
                                address.PostCode = reader.GetString(6);
                            if (!reader.IsDBNull(7))
                                address.Country = reader.GetString(7);
                            addresses.Add(address);
                        }
                    }
                }
            }

            return addresses;
        }

        public void AddItem(Address newItem)
        {
            throw new NotImplementedException();
        }

        public void UpdateItem(int key, Address updatedItem)
        {
            throw new NotImplementedException();
        }

        public void DeleteItem(int key)
        {
            throw new NotImplementedException();
        }
    }
}
