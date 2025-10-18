using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using SenNotes.ViewModels;

using UserControl = System.Windows.Controls.UserControl;

namespace SenNotes.Views
{
    public partial class MainView : UserControl
    {
        public MainView(MainViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }


    }
}