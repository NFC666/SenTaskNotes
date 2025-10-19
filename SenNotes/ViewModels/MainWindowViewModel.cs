using System.Windows.Controls;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Microsoft.Extensions.DependencyInjection;

using SenNotes.Managers;
using SenNotes.Services;
using SenNotes.Views;

using Serilog;

using UserControl = System.Windows.Controls.UserControl;

namespace SenNotes.ViewModels
{
    public partial class MainWindowViewModel : ObservableObject
    {

        private readonly WindowManager  _windowManager;

        public MainWindowViewModel(WindowManager windowManager)
        {
            Log.Information("MainWindowViewModel启动了");
            _windowManager = windowManager;
        }
        public UserControl CurrentPage { get; set; } 
            = App.Service.GetRequiredService<MainView>();


        [RelayCommand]
        private void OpenSettings()
        {
            _windowManager.OpenWindowWithNoChrome<SettingsWindow>();
        }

    }
}