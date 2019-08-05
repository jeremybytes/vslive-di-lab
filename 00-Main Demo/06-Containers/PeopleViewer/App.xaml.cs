using PeopleViewer.Presentation;
using PersonReader.SQL;
using PersonReader.CSV;
using PersonReader.Service;
using System.Windows;
using System;
using PersonReader.Decorators;
using PeopleViewer.Logging;

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
            var reader = new ServiceReader();
            var cachingReader = new CachingReader(reader);
            var viewModel = new PeopleViewModel(cachingReader);
            Application.Current.MainWindow = new MainWindow(viewModel);
        }
    }
}
