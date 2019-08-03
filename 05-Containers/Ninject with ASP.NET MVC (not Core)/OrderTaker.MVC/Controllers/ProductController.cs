using OrderTaker.Models;
using OrderTaker.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OrderTaker.MVC.Controllers
{
    public class ProductController : Controller
    {
        private IRepository<Product> repository;

        public ProductController(IRepository<Product> repository)
        {
            this.repository = repository;
        }

        //
        // GET: /Product/
        public ActionResult Index()
        {
            var model = repository.GetItems().OrderBy(p => p.ProductName);
            return View(model);
        }

        public ActionResult SearchProducts(string searchString)
        {
            //var model = repository.GetItems().Where(p => p.ProductName.Contains(searchString));
            var model = repository.SearchByName(searchString);
            return View("Index", model);
        }

        //
        // GET: /Product/Details/5
        public ActionResult Details(int id)
        {
            var model = repository.GetItem(id);
            if (model == null)
            {
                RedirectToAction("Index");
            }

            //if (model == null)
            //{
            //    Response.Write("Unable to locate Product");
            //    return null;
            //}
            return View(model);
        }

        //
        // GET: /Product/Create
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Product/Create
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
        // GET: /Product/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Product/Edit/5
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
        // GET: /Product/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Product/Delete/5
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
