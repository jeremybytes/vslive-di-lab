using OrderTaker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OrderTaker.MVC.Binders
{
    public class ShoppingCartModelBinder : IModelBinder
    {
        private const string sessionKey = "ShoppingCart";

        public object BindModel(ControllerContext controllerContext, 
            ModelBindingContext bindingContext)
        {
            ShoppingCart cart = (ShoppingCart)controllerContext.HttpContext.Session[sessionKey];
            if(cart == null)
            {
                cart = new ShoppingCart();
                controllerContext.HttpContext.Session[sessionKey] = cart;
            }
            return cart;
        }
    }
}