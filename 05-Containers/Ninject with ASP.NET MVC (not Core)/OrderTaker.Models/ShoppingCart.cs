using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderTaker.Models
{
    public class ShoppingCart
    {
        private List<CartItem> cartItems = new List<CartItem>();

        public IEnumerable<CartItem> CartItems
        {
            get { return cartItems; }
        }

        public void AddItem(Product product, int quantity)
        {
            CartItem item = cartItems
                .FirstOrDefault(i => i.Product.Id == product.Id);

            if (item == null)
            {
                cartItems.Add(new CartItem
                    {
                        Product = product,
                        Quantity = quantity,
                    });
            }
            else
            {
                item.Quantity += quantity;
            }
        }

        public void RemoveItem(Product product)
        {
            cartItems.RemoveAll(i => i.Product.Id == product.Id);
        }

        public decimal GetTotal()
        {
            return cartItems.Sum(i => i.Product.UnitPrice * i.Quantity);
        }

        public void Clear()
        {
            cartItems.Clear();
        }
    }

    public class CartItem
    {
        public Product Product { get; set; }
        public int Quantity { get; set; }
    }
}
