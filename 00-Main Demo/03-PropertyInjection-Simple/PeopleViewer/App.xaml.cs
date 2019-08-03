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
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            ComposeObjects();
            Application.Current.MainWindow.Show();
        }

        private static void ComposeObjects()
        {
            var wrappedReader = new CSVReader();
            var reader = new CachingReader(wrappedReader);
            var viewModel = new PeopleViewModel(reader);
            Application.Current.MainWindow = new MainWindow(viewModel);
        }
    }
}
