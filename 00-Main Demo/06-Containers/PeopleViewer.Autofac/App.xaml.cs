using Autofac;
using Autofac.Features.ResolveAnything;
using PeopleViewer.Common;
using PeopleViewer.Presentation;
using PersonReader.SQL;
using PersonReader.CSV;
using PersonReader.Service;
using System.Windows;
using PersonReader.Decorators;

namespace PeopleViewer
{
    public partial class App : Application
    {
        IContainer Container;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            ConfigureContainer();
            ComposeObjects();
            Application.Current.MainWindow.Title = "DI with Autofac";
            Application.Current.MainWindow.Show();
        }

        private void ConfigureContainer()
        {
            var builder = new ContainerBuilder();

            // READER TYPE OPTION #1 - No Decorator
            //builder.RegisterType<ServiceReader>().As<IPersonReader>()
            //    .SingleInstance();

            // READER TYPE OPTION #2 - With Decorator
            builder.RegisterType<ServiceReader>().Named<IPersonReader>("reader")
                .SingleInstance();
            builder.RegisterDecorator<IPersonReader>(
                (c, inner) => new CachingReader(inner), fromKey: "reader");

            // OTHER TYPES OPTION #1 - Explicit Registration
            //builder.RegisterType<MainWindow>().InstancePerDependency();
            //builder.RegisterType<PeopleViewModel>().InstancePerDependency();

            // OTHER TYPES OPTION #2 - Automatic Registration
            builder.RegisterSource(new AnyConcreteTypeNotAlreadyRegisteredSource());

            Container = builder.Build();
        }

        private void ComposeObjects()
        {
            Application.Current.MainWindow = Container.Resolve<MainWindow>();
        }
    }
}
