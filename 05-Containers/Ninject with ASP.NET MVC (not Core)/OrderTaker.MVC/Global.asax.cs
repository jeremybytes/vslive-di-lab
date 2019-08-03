using OrderTaker.Models;
using OrderTaker.MVC.Binders;
using OrderTaker.MVC.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace OrderTaker.MVC
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            ControllerBuilder.Current.SetControllerFactory(
                new NinjectControllerFactory());
            ModelBinders.Binders.Add(typeof(ShoppingCart), new ShoppingCartModelBinder());
        }

        void Application_Error(object sender, EventArgs e)
        {
            Server.ClearError();
            Response.Write("Something Bad Happened");
        }
    }
}
