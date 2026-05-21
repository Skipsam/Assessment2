using Assignment2.App.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Assignment2.App.Views
{
    public partial class AnimalEditorWindow : Window
    {
        public AnimalEditorWindow(AnimalEditorViewModel viewModel)
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
