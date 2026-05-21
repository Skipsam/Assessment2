using Assignment2.App.BusinessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Assignment2.App.ViewModel
{
    // CustomerEditorViewModel acts as the connection between the CustomerEditorView
    // and the Customer model. It exposes bindable properties and commands so the
    // View does not require business logic code behind.
    public class CustomerEditorViewModel :INotifyPropertyChanged
    {
        private readonly Customer customer;

        // Constructor receives the customer model and creates commands
        // for saving and cancelling edits.
        public CustomerEditorViewModel(Customer customer)
        {
            this.customer = customer;

            SaveCommand = new RelayCommand(Save, CanSave);
            CancelCommand = new RelayCommand(Cancel);
        }
        // Required for WPF data binding so the UI updates automatically
        // when property values change.
        public event PropertyChangedEventHandler? PropertyChanged;

        // Raised when the user saves the customer.
        // The View listens for this event and closes the window.
        public event Action<Customer>? SaveRequested;
        // Raised when the user cancels editing.
        public event Action? CancelRequested;

        // Bindable property for the customer's first name.
        public string? FirstName
        {
            get => customer.FirstName;
            set
            {
                customer.FirstName = value;
                // Notify the View that the property changed.
                OnPropertyChanged();
                // Re-check whether the Save button should be enabled.
                RefreshCommands();
            }
        }
        //Other bindable properties follow a similar format
        public string? Surname
        {
            get => customer.Surname;
            set
            {
                customer.Surname = value;
                OnPropertyChanged();
                RefreshCommands();
            }
        }

        public string? PhoneNumber
        {
            get => customer.PhoneNumber;
            set
            {
                customer.PhoneNumber = value;
                OnPropertyChanged();
                RefreshCommands();
            }
        }

        public string? Address
        {
            get => customer.Address;
            set
            {
                customer.Address = value;
                OnPropertyChanged();
            }
        }

        // Command bound to the Save button in the View.
        public ICommand SaveCommand { get; }

        // Command bound to the Cancel button in the View.
        public ICommand CancelCommand { get; }

        // Determines whether the Save command can execute.
        // Save is only enabled when the customer contains valid data.
        private bool CanSave()
        {
            return customer.CheckIfValid();
        }
        
        // Executes when the Save button is pressed.
        // Raises an event so the View can respond appropriately.
        private void Save()
        {
            SaveRequested?.Invoke(customer);
        }
        
        // Executes when the Cancel button is pressed.
        private void Cancel()
        {
            CancelRequested?.Invoke();
        }

        // Refreshes command availability after customer data changes.
        // This ensures the Save button enables/disables correctly.
        private void RefreshCommands()
        {
            if (SaveCommand is RelayCommand command)
            {
                command.RaiseCanExecuteChanged();
            }
        }
        
        // Raises the PropertyChanged event so bound UI elements update automatically.
        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
