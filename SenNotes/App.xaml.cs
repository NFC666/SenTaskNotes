using CommunityToolkit.Mvvm.Messaging;
using MaterialDesignThemes.Wpf;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Windows;
using System.Windows.Threading;

using SenNotes.Common.Global;
using SenNotes.Managers;
using SenNotes.Services;
using SenNotes.Services.IServices;
using SenNotes.ViewModels;
using SenNotes.Views;

using Application = System.Windows.Application;

namespace SenNotes;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private static readonly WindowManager _windowManager = new();
    public static IServiceProvider Service { get; private set; }
    [STAThread]
    private static void Main(string[] args)
    {
        MainAsync(args).GetAwaiter().GetResult();
    }

    public App()
    {
        Properties["AppName"] = "SenNotes";
    }

    private static async Task MainAsync(string[] args)
    {
        using IHost host = CreateHostBuilder(args).Build();
        await host.StartAsync().ConfigureAwait(true);

        Service = host.Services;
        
        App app = new();
        app.InitializeComponent();

        
        _windowManager.OpenWindowWithNoChrome<MainWindow>();
        app.Run();


        await host.StopAsync().ConfigureAwait(true);
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((hostBuilderContext, configurationBuilder)
                => configurationBuilder.AddUserSecrets(typeof(App).Assembly))
            .ConfigureServices((hostContext, services) =>
            {

                services.AddSingleton<MainWindow>();
                services.AddSingleton<MainWindowViewModel>();
                services.AddSingleton<MainView>();
                services.AddSingleton<MainViewModel>();
                
                services.AddScoped<AiAssistService>();
                services.AddTransient<ITaskModelService,TaskModelService>();
                services.AddTransient<ISettingsModelService,SettingsModelService>();
                services.AddTransient<ToastService>();
                services.AddTransient<WindowManager>();
                
                
                // 绑定配置文件中的 AiInfo 节点
                services.Configure<AiInfo>(
                    hostContext.Configuration.GetSection("AiInfo"));
                
                services.AddTransient<TaskModelUpdateWindow>();
                services.AddTransient<TaskModelUpdateWindowVm>();
                services.AddTransient<SettingsWindow>();
                services.AddTransient<SettingsWindowViewModel>();

                services.AddSingleton<WeakReferenceMessenger>();
                services.AddSingleton<IMessenger, WeakReferenceMessenger>(provider =>
                    provider.GetRequiredService<WeakReferenceMessenger>());

                services.AddSingleton(_ => Current.Dispatcher);

                services.AddTransient<ISnackbarMessageQueue>(provider =>
                {
                    Dispatcher dispatcher = provider.GetRequiredService<Dispatcher>();
                    return new SnackbarMessageQueue(TimeSpan.FromSeconds(3.0), dispatcher);
                });
            });
}