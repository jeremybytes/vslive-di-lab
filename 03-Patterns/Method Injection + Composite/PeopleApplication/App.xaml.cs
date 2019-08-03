using PeopleLibrary;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace PeopleApplication
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var formatters = new List<IPersonFormatter>()
            {
                new DefaultPersonFormatter(),
                new FamilyNamePersonFormatter(),
                new GivenNamePersonFormatter(),
                new FullNamePersonFormatter(),
                new CompositePersonFormatter(
                    new List<IPersonFormatter>{ new GivenNamePersonFormatter(), new FamilyNamePersonFormatter() }),
            };
            Application.Current.MainWindow = new MainWindow(formatters);
            Application.Current.MainWindow.Show();
        }
    }
}
