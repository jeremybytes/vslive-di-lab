using Ninject;
using OrderTaker.Models;
using OrderTaker.Repository.Interface;
using OrderTaker.Repository.ParameterizedSQL;
using System;
using System.Web.Mvc;
using System.Web.Routing;

namespace OrderTaker.MVC.Infrastructure
{
    public class NinjectControllerFactory : DefaultControllerFactory
    {
        private IKernel ninjectKernel;

        public NinjectControllerFactory()
        {
            ninjectKernel = new StandardKernel();
            AddBindings();
        }

        protected override IController GetControllerInstance(
            RequestContext requestContext, Type controllerType)
        {
            return controllerType == null
                ? null
                : (IController)ninjectKernel.Get(controllerType);
        }

        private void AddBindings()
        {
            ninjectKernel.Bind<IRepository<Address>>().To<AddressRepository>();
            ninjectKernel.Bind<IRepository<Customer>>().To<CustomerRepository>();
            ninjectKernel.Bind<IRepository<Product>>().To<ProductRepository>();
            ninjectKernel.Bind<IRepository<Order>>().To<OrderRepository>();
            ninjectKernel.Bind<IRepository<OrderItem>>().To<OrderItemRepository>();
        }
    }
}