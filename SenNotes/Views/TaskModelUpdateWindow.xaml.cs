using System.Windows;

using CommunityToolkit.Mvvm.Messaging;

using SenNotes.Common.Messages;
using SenNotes.Managers;
using SenNotes.ViewModels;

namespace SenNotes.Views
{
    public partial class TaskModelUpdateWindow : Window
    {

        public TaskModelUpdateWindow(TaskModelUpdateWindowVm  viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            WeakReferenceMessenger.Default.Unregister<TaskModelMessage>(this);

        }
    }
}