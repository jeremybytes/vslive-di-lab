using Ninject;
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
        IKernel Container = new StandardKernel();

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            ConfigureContainer();
            ComposeObjects();
            Application.Current.MainWindow.Title = "DI with Ninject";
            Application.Current.MainWindow.Show();
        }

        private void ConfigureContainer()
        {
            Container.Bind<IPersonReader>().To<CachingReader>()
                .InSingletonScope()
                .WithConstructorArgument<IPersonReader>(Container.Get<ServiceReader>());
        }

        private void ComposeObjects()
        {
            Application.Current.MainWindow = Container.Get<MainWindow>();
        }
    }
}
