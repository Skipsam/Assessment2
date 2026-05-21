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
    public class AnimalEditorViewModel : INotifyPropertyChanged
    {
        private readonly Animal animal;
        private Customer? selectedOwner;

        public AnimalEditorViewModel(Animal animal, IEnumerable<Customer> customers)
        {
            this.animal = animal;

            Customers = customers;

            SaveCommand = new RelayCommand(Save, CanSave);
            CancelCommand = new RelayCommand(Cancel);
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public event Action<Animal>? SaveRequested;
        public event Action? CancelRequested;

        public IEnumerable<Customer> Customers { get; }

        public string? Name
        {
            get => animal.Name;
            set
            {
                animal.Name = value;
                OnPropertyChanged();
                RefreshCommands();
            }
        }

        public string? Type
        {
            get => animal.Type;
            set
            {
                animal.Type = value;
                OnPropertyChanged();
                RefreshCommands();
            }
        }

        public string? Breed
        {
            get => animal.Breed;
            set
            {
                animal.Breed = value;
                OnPropertyChanged();
            }
        }

        public string? Sex
        {
            get => animal.Sex;
            set
            {
                animal.Sex = value;
                OnPropertyChanged();
                RefreshCommands();
            }
        }

        public Customer? SelectedOwner
        {
            get => selectedOwner;
            set
            {
                selectedOwner = value;
                animal.OwnerId = selectedOwner?.Id ?? 0;
                OnPropertyChanged();
                RefreshCommands();
            }
        }

        public ICommand SaveCommand { get; }

        public ICommand CancelCommand { get; }

        private bool CanSave()
        {
            return animal.CheckIfValid();
        }

        private void Save()
        {
            SaveRequested?.Invoke(animal);
        }

        private void Cancel()
        {
            CancelRequested?.Invoke();
        }

        private void RefreshCommands()
        {
            if (SaveCommand is RelayCommand command)
            {
                command.RaiseCanExecuteChanged();
            }
        }

        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
