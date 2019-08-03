using OrderTaker.Models;
using OrderTaker.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderTaker.Repository.LinqToSQL
{
    public class AddressRepository : IRepository<Address>
    {
        public IEnumerable<Address> GetItems()
        {
            var addresses = new List<Address>();
            using (var ctx = new OrderTakerDataContext(Database.OrderTakerConnection))
            {
                var dataAddresses = from a in ctx.DataAddresses
                                    select a;

                foreach (var dataAddress in dataAddresses)
                    addresses.Add(ModelConverters.GetAddressFromDataAddress(dataAddress));
            }
            return addresses;
        }

        public IEnumerable<Address> GetItems(int filterKey)
        {
            var addresses = new List<Address>();
            using (var ctx = new OrderTakerDataContext(Database.OrderTakerConnection))
            {
                var dataAddresses = from a in ctx.DataAddresses
                                    where a.CustomerId == filterKey
                                    select a;

                foreach (var dataAddress in dataAddresses)
                    addresses.Add(ModelConverters.GetAddressFromDataAddress(dataAddress));
            }
            return addresses;
        }

        public IEnumerable<Address> SearchByName(string searchString)
        {
            var addresses = new List<Address>();
            using (var ctx = new OrderTakerDataContext(Database.OrderTakerConnection))
            {
                var dataAddresses = from a in ctx.DataAddresses
                                    where a.DataCustomer.FirstName.Contains(searchString) ||
                                          a.DataCustomer.LastName.Contains(searchString)
                                    select a;

                foreach (var dataAddress in dataAddresses)
                    addresses.Add(ModelConverters.GetAddressFromDataAddress(dataAddress));
            }
            return addresses;
        }

        public Address GetItem(int key)
        {
            Address address = null;
            using (var ctx = new OrderTakerDataContext(Database.OrderTakerConnection))
            {
                var dataAddress = (from a in ctx.DataAddresses
                                    where a.Id == key
                                    select a).FirstOrDefault();
                if(dataAddress != null)
                    address = ModelConverters.GetAddressFromDataAddress(dataAddress);
            }
            return address;
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
