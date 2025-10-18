using System.Windows;

using CommunityToolkit.Mvvm.Messaging;

using SenNotes.Common.Messages;
using SenNotes.ViewModels;

namespace SenNotes.Views
{
    public partial class SettingsWindow : Window
    {
        public SettingsWindow(SettingsWindowViewModel  viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
            WeakReferenceMessenger.Default.Register<CloseSettingsWindow>(this, (w, m) =>
            {
                this.Close();
            });
        }
    }
}