using System;
using System.Windows;
using System.Windows.Forms;
using SenNotes.ViewModels;
using MessageBox = System.Windows.MessageBox; // 避免命名冲突

namespace SenNotes.Views
{
    public partial class MainWindow : Window
    {
        private NotifyIcon _notifyIcon;
        private bool _isExit = false;

        public MainWindow(MainWindowViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
            InitNotifyIcon();
        }

        private void InitNotifyIcon()
        {
            _notifyIcon = new NotifyIcon
            {
                Icon = new System.Drawing.Icon("Resources/AppIcon.ico"), // 项目根目录下放个图标
                Visible = true,
                Text = "SenNotes 已在后台运行"
            };

            // 右键菜单
            var menu = new ContextMenuStrip();
            menu.Items.Add("打开主界面", null, ShowMainWindow);
            menu.Items.Add("退出", null, ExitApp);
            _notifyIcon.ContextMenuStrip = menu;

            // 双击图标恢复窗口
            _notifyIcon.DoubleClick += (s, e) => ShowMainWindow(s, e);
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void Window_StateChanged(object sender, EventArgs e)
        {
            if (WindowState == WindowState.Minimized)
            {
                Hide();
                _notifyIcon.ShowBalloonTip(1000, "SenNotes", "程序已最小化到托盘", ToolTipIcon.Info);
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!_isExit)
            {
                e.Cancel = true;
                Hide();
                _notifyIcon.ShowBalloonTip(1000, "SenNotes", "程序继续在后台运行", ToolTipIcon.Info);
            }
        }

        private void ShowMainWindow(object sender, EventArgs e)
        {
            Show();
            WindowState = WindowState.Normal;
            Activate();
        }

        private void ExitApp(object sender, EventArgs e)
        {
            _isExit = true;
            _notifyIcon.Visible = false;
            System.Windows.Application.Current.Shutdown();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox
                .Show("确定要退出吗？", "提示", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                this.Close();
            }
        }

    }
}
