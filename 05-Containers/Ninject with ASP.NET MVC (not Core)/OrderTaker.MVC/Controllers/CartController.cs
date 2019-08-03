using OrderTaker.Models;
using OrderTaker.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OrderTaker.MVC.Controllers
{
    public class CartController : Controller
    {
        private IRepository<Product> repository;

        public CartController(IRepository<Product> repository)
        {
            this.repository = repository;
        }

        public RedirectToRouteResult AddToCart(ShoppingCart cart, int id, string returnUrl)
        {
            Product product = repository.GetItem(id);

            if (product != null)
                cart.AddItem(product, 1);

            return RedirectToAction("Index", "Product");
        }

        public PartialViewResult Summary(ShoppingCart cart)
        {
            return PartialView(cart);
        }

        public ActionResult Checkout(ShoppingCart cart, string returnUrl)
        {
            if (cart.CartItems.Count() > 0)
            {
                return RedirectToAction("Create", "Order");
            }
            return Redirect(returnUrl);
        }

        //
        // GET: /Cart/
        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /Cart/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Cart/Create
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Cart/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Cart/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Cart/Edit/5
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
        // GET: /Cart/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Cart/Delete/5
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
