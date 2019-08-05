using PeopleViewer.Common;
using PeopleViewer.Presentation;
using PersonReader.SQL;
using PersonReader.CSV;
using PersonReader.Service;
using System.Windows;
using PersonReader.Decorators;
using Ninject;
using System;

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
            Container.Bind<IPersonReader>().To<SQLReader>();

            // Service Reader configuration
            var serviceReaderUri = new ServiceReaderUri("http://localhost:9874");
            Container.Bind<ServiceReaderUri>().ToConstant(serviceReaderUri);

            // CSV Reader Configuration
            var csvFilePath =
                new CSVReaderFilePath(AppDomain.CurrentDomain.BaseDirectory + "People.txt");
            Container.Bind<CSVReaderFilePath>().ToConstant(csvFilePath);

            // SQL Reader configuration
            var sqlFileName = new SQLReaderDBFileName("people.db");
            Container.Bind<SQLReaderDBFileName>().ToConstant(sqlFileName);
        }

        private void ComposeObjects()
        {
            Application.Current.MainWindow = Container.Get<MainWindow>();
        }
    }
}
