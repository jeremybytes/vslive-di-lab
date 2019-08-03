using OrderTaker.Models;
using OrderTaker.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderTaker.Repository.LinqToSQL
{
    public class ProductRepository : IRepository<Product>
    {
        public IEnumerable<Product> GetItems()
        {
            var products = new List<Product>();
            using (var ctx = new OrderTakerDataContext(Database.OrderTakerConnection))
            {
                var dataProducts = from p in ctx.DataProducts
                                   where p.Active
                                   orderby p.ProductName
                                   select p;

                foreach (var dataProduct in dataProducts)
                    products.Add(ModelConverters.GetProductFromDataProduct(dataProduct));
            }
            return products;
        }

        public IEnumerable<Product> GetItems(int filterKey)
        {
            return GetItems();
        }

        public IEnumerable<Product> SearchByName(string searchString)
        {
            var products = new List<Product>();
            using (var ctx = new OrderTakerDataContext(Database.OrderTakerConnection))
            {
                var dataProducts = from p in ctx.DataProducts
                                   where p.ProductName.Contains(searchString) &&
                                         p.Active
                                   select p;

                foreach (var dataProduct in dataProducts)
                    products.Add(ModelConverters.GetProductFromDataProduct(dataProduct));
            }
            return products;
        }

        public Product GetItem(int key)
        {
            Product product = null;
            using (var ctx = new OrderTakerDataContext(Database.OrderTakerConnection))
            {
                var dataProduct = (from p in ctx.DataProducts
                                   where p.Id == key
                                   select p).FirstOrDefault();

                if (dataProduct != null)
                    product = ModelConverters.GetProductFromDataProduct(dataProduct);
            }
            return product;
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
