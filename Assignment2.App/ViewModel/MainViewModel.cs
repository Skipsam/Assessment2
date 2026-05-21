using Assignment2.App.BusinessLayer;
using Assignment2.App.ViewModel;
using Assignment2.App.Views;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Assignment2.App.ViewModel
{
    // MainViewModel controls the main customer screen.
    // It exposes customer data and commands for the View to bind to.
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly CustomerService customerService;
        private Customer? selectedCustomer;

        // Constructor receives the customer service through dependency injection.
        public MainViewModel(CustomerService customerService)
        {
            this.customerService = customerService;
           
            // Loads existing customers into an ObservableCollection so the View updates automatically.
            Customers = new ObservableCollection<Customer>(
                customerService.FindCustomers(string.Empty));
            
            // Commands used by buttons in the View.
            AddCustomerCommand = new RelayCommand(AddCustomer);
            EditCustomerCommand = new RelayCommand(EditCustomer, () => SelectedCustomer != null);
        }
        // Required by WPF data binding to notify the View when properties change.
        public event PropertyChangedEventHandler? PropertyChanged;

        // Customer list displayed by the main View.
        public ObservableCollection<Customer> Customers { get; }

        // The customer currently selected in the View.
        public Customer? SelectedCustomer
        {
            get => selectedCustomer;
            set
            {
                selectedCustomer = value;
                OnPropertyChanged();
                
                // Re-checks whether Edit should be enabled.
                if (EditCustomerCommand is RelayCommand command)
                {
                    command.RaiseCanExecuteChanged();
                }
            }
        }

        // Command bound to the Add Customer button.
        public ICommand AddCustomerCommand { get; }
        // Command bound to the Edit Customer button.
        public ICommand EditCustomerCommand { get; }

        // Creates a new customer and opens the editor window.
        private void AddCustomer()
        {
            var customer = customerService.AddCustomer();

            var editorViewModel = new CustomerEditorViewModel(customer);
            var editorWindow = new CustomerEditorWindow(editorViewModel);

            var result = editorWindow.ShowDialog();

            if (result == true)
            {
                Customers.Add(customer);
                customerService.Save("data");
            }
            else
            {
                customerService.Customers.Remove(customer);
            }
        }
        
        // Opens the editor window for the currently selected customer.
        private void EditCustomer()
        {
            if (SelectedCustomer == null)
            {
                return;
            }

            var editorViewModel = new CustomerEditorViewModel(SelectedCustomer);
            var editorWindow = new CustomerEditorWindow(editorViewModel);

            var result = editorWindow.ShowDialog();

            if (result == true)
            {
                customerService.Save("data");
                OnPropertyChanged(nameof(Customers));
            }
        }
       
        // Raises the PropertyChanged event so the View refreshes bound values.
        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}