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
        private readonly AnimalService animalService;
        private Animal? selectedAnimal;

        // Constructor receives the customer service and animal service through dependency injection.
        public MainViewModel(CustomerService customerService,AnimalService animalService)
        {
            this.customerService = customerService;
            this.animalService = animalService;
           
            // Loads existing customers into an ObservableCollection so the View updates automatically.
            Customers = new ObservableCollection<Customer>(
                customerService.FindCustomers(string.Empty));
            Animals = new ObservableCollection<Animal>(animalService.Animals);

            // Commands used by buttons in the View.
            AddCustomerCommand = new RelayCommand(AddCustomer);
            EditCustomerCommand = new RelayCommand(EditCustomer, () => SelectedCustomer != null);

            AddAnimalCommand = new RelayCommand(AddAnimal);
            EditAnimalCommand = new RelayCommand(EditAnimal, () => SelectedAnimal != null); 
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
        
        public ICommand AddCustomerCommand { get; }
        // Command bound to the Edit Customer button.
        public ICommand EditCustomerCommand { get; }

        public ObservableCollection<Animal> Animals { get; }

        public Animal? SelectedAnimal
        {
            get => selectedAnimal;
            set
            {
                selectedAnimal = value;
                OnPropertyChanged();

                if (EditAnimalCommand is RelayCommand command)
                {
                    command.RaiseCanExecuteChanged();
                }
            }
        }
        public ICommand AddAnimalCommand { get; }

        public ICommand EditAnimalCommand { get; }

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

        private void AddAnimal()
        {
            var animal = animalService.AddAnimal();

            var editorViewModel = new AnimalEditorViewModel(animal, customerService.Customers);
            var editorWindow = new AnimalEditorWindow(editorViewModel);

            var result = editorWindow.ShowDialog();

            if (result == true)
            {
                Animals.Add(animal);
                animalService.Save("data");
            }
            else
            {
                animalService.Animals.Remove(animal);
            }
        }

        private void EditAnimal()
        {
            if (SelectedAnimal == null)
            {
                return;
            }

            var editorViewModel = new AnimalEditorViewModel(SelectedAnimal, customerService.Customers);
            var editorWindow = new AnimalEditorWindow(editorViewModel);

            var result = editorWindow.ShowDialog();

            if (result == true)
            {
                animalService.Save("data");
                OnPropertyChanged(nameof(Animals));
            }
        }

        // Raises the PropertyChanged event so the View refreshes bound values.
        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}