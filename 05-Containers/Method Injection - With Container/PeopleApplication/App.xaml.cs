using Ninject;
using PeopleLibrary;
using System.Windows;

namespace PeopleApplication
{
    public partial class App : Application
    {
        IKernel Container;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            ConfigureContainer();
            Application.Current.MainWindow = Container.Get<MainWindow>();
            Application.Current.MainWindow.Show();
        }

        public void ConfigureContainer()
        {
            Container = new StandardKernel();
            Container.Bind<IPersonFormatter>().To<DefaultPersonFormatter>().InTransientScope();
            Container.Bind<IPersonFormatter>().To<FamilyNamePersonFormatter>().InTransientScope();
            Container.Bind<IPersonFormatter>().To<GivenNamePersonFormatter>().InTransientScope();
            //Container.Bind<IPersonFormatter>().To<FullNamePersonFormatter>().InTransientScope();
        }
    }
}
