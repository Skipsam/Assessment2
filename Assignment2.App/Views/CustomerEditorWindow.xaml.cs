using Assignment2.App.ViewModel;
using System.Windows;

namespace Assignment2.App.Views
{
    public partial class CustomerEditorWindow : Window
    {
        public CustomerEditorWindow(CustomerEditorViewModel viewModel)
        {
            InitializeComponent();

            DataContext = viewModel;

            viewModel.SaveRequested += _ =>
            {
                DialogResult = true;
                Close();
            };

            viewModel.CancelRequested += () =>
            {
                DialogResult = false;
                Close();
            };
        }
    }
}