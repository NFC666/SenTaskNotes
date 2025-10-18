using System.Windows;

using Microsoft.Extensions.DependencyInjection;

namespace SenNotes.Managers
{
    public class WindowManager
    {
        private readonly Dictionary<Type, Window> _windows = new();
        
        public T OpenWindowWithNoChrome<T>() where T : Window
        { 
            var type = typeof(T);
            var window = App.Service.GetRequiredService<T>();
            WindowStyleConfig(type,window);
            if (_windows.TryGetValue(type, out Window? w))
            {
                w.Activate();
                return window;
            }
            _windows[type] = window;
            window.Show();
            return window;
        }
        private void WindowStyleConfig(Type T, Window window)
        {
            window.WindowStyle = WindowStyle.None;
            window.ResizeMode = ResizeMode.CanResizeWithGrip;
            window.AllowsTransparency = true;
            window.MouseLeftButtonDown += (_, _) => window.DragMove();
            window.Closed += (_, _) => _windows.Remove(T);
        }

    }
}