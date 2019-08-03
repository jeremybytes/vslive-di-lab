using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(OrderTaker.MVC.Startup))]
namespace OrderTaker.MVC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
