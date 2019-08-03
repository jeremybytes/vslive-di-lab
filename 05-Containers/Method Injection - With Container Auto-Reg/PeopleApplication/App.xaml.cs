using Ninject;
using PeopleLibrary;
using System;
using System.Data;
using System.Linq;
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

            var assembly = typeof(DefaultPersonFormatter).Assembly;

            var formatterTypes = from type in assembly.GetTypes()
                               where !type.IsAbstract &&
                                     type.Name.EndsWith("Formatter")
                               select type;

            foreach (Type type in formatterTypes)
            {
                Container.Bind<IPersonFormatter>().To(type).InTransientScope();
            }
        }
    }
}
