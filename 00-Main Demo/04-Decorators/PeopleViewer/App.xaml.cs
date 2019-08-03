using PeopleViewer.Presentation;
using PersonReader.SQL;
using PersonReader.CSV;
using PersonReader.Service;
using System.Windows;
using PersonReader.Decorators;
using PeopleViewer.Logging;
using System;

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
            var delay = new TimeSpan(0, 0, 3);
            var retryReader = new RetryReader(reader, delay);
            var logFilePath = AppDomain.CurrentDomain.BaseDirectory + "ExceptionLog.txt";
            var logger = new FileLogger(logFilePath);
            var loggingReader = new ExceptionLoggingReader(retryReader, logger);
            var cachingReader = new CachingReader(loggingReader);
            var viewModel = new PeopleViewModel(cachingReader);
            Application.Current.MainWindow = new MainWindow(viewModel);
        }

        private static void AlternateComposeObjects()
        {
            Application.Current.MainWindow =
                new MainWindow(
                    new PeopleViewModel(
                        new CachingReader(
                            new ExceptionLoggingReader(
                                new RetryReader(
                                    new ServiceReader(),
                                    new TimeSpan(0, 0, 3)),
                                new FileLogger(
                                    AppDomain.CurrentDomain.BaseDirectory + "log.txt")))));
        }
    }
}
