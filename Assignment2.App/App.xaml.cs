using Assignment2.App.BusinessLayer;
using Assignment2.App.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Assignment2.App
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            IDataStore store = new CSVStore();
            store.Load("data");

            var customerService = new CustomerService(store);
            var animalService = new AnimalService(store);
            var mainViewModel = new MainViewModel(customerService, animalService);

            var mainWindow = new Views.MainWindow
            {
                DataContext = mainViewModel
            };

            mainWindow.Show();
        }
    }
}
