using OrderTaker.Models;
using OrderTaker.MVC.Models;
using OrderTaker.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OrderTaker.MVC.Controllers
{
    public class OrderController : Controller
    {
        private IRepository<Order> orderRepo;
        private IRepository<Customer> customerRepo;
        private IRepository<Address> addressRepo;

        public OrderController(IRepository<Order> orderRepo,
            IRepository<Customer> customerRepo, IRepository<Address> addressRepo)
        {
            this.orderRepo = orderRepo;
            this.customerRepo = customerRepo;
            this.addressRepo = addressRepo;
        }

        private Customer GetCustomerByLoginName(string loginName)
        {
            return customerRepo.GetItems().FirstOrDefault(c => c.LoginName == loginName);
        }

        //
        // GET: /Order/
        [Authorize]
        public ActionResult Index()
        {
            var customer = GetCustomerByLoginName(User.Identity.Name);
            ViewBag.Customer = customer;
            if (customer == null)
            {
                Response.Write("Invalid Customer");
                return null;
            }
            var orders = orderRepo.GetItems(customer.Id);
            return View(orders);
        }

        //
        // GET: /Order/Details/5
        [Authorize]
        public ActionResult Details(int id)
        {
            var order = orderRepo.GetItem(id);
            if (order == null)
            {
                Response.Write("Invalid Order");
                return null;
            }
            order.Customer = customerRepo.GetItem(order.CustomerId);

            if (order.Customer.LoginName != User.Identity.Name)
                return RedirectToAction("Index");
                //throw new InvalidOperationException("Order customer does not match logged in user.");

            return View(order);
        }

        //
        // GET: /Order/Create
        [Authorize]
        public ActionResult Create(ShoppingCart cart)
        {
            var customer = GetCustomerByLoginName(User.Identity.Name);

            var order = Order.GetOrderFromCart(cart, customer, null);

            var addresses = addressRepo.GetItems(order.CustomerId).ToList();
            var orderVM = new CreateOrderViewModel()
                            {
                                Order = order,
                                AvailableAddresses = addresses,
                            };

            return View(orderVM);
        }

        //
        // POST: /Order/Create
        [HttpPost]
        public ActionResult Create(ShoppingCart cart, int shippingAddressId)
        {
            try
            {
                var customer = GetCustomerByLoginName(User.Identity.Name);
                var shippingAddress = addressRepo.GetItem(shippingAddressId);

                var order = Order.GetOrderFromCart(cart, customer, shippingAddress);

                orderRepo.AddItem(order);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Order/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Order/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Order/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Order/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
