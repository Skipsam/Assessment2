using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Assignment2.App.ViewModel
{
    // RelayCommand is a reusable command class used by the MVVM pattern.
    // It allows buttons and other UI controls to execute ViewModel methods
    // without placing logic inside the View code-behind.
    public class RelayCommand : ICommand
    {
        // Stores the method that should run when the command executes.
        private readonly Action execute;
        // Stores the method used to determine whether the command is currently allowed to execute.
        private readonly Func<bool>? canExecute;

        // Constructor receives the execute action and an optional validation method.
        public RelayCommand(Action execute, Func<bool>? canExecute = null)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }
        
        // Raised when command availability changes.
        // WPF listens to this event to enable or disable buttons automatically.
        public event EventHandler? CanExecuteChanged;
        
        // Determines whether the command is currently allowed to execute.
        // If no validation method exists, the command is always enabled.
        public bool CanExecute(object? parameter)
        {
            return canExecute == null || canExecute();
        }

        // Executes the command action.
        public void Execute(object? parameter)
        {
            execute();
        }

        // Triggers WPF to re-check whether the command can execute.
        // This is commonly used after form data changes.
        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
