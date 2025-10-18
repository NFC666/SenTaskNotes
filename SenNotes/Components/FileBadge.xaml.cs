using System.Diagnostics;
using System.Windows;
using System.Windows.Media;

using SenNotes.Common.Models;

using Brush = System.Drawing.Brush;
using Color = System.Windows.Media.Color;
using MessageBox = System.Windows.MessageBox;
using UserControl = System.Windows.Controls.UserControl;

namespace SenNotes.Components
{
    public partial class FileBadge : UserControl
    {
        public static readonly DependencyProperty ForegroundProperty =
            DependencyProperty.Register(
                "Foreground",
                typeof(SolidColorBrush),
                typeof(FileBadge),
                new PropertyMetadata(new SolidColorBrush(Colors.Purple))
            );
        public SolidColorBrush? Foreground
        {
            get { return (SolidColorBrush)GetValue(ForegroundProperty); }
            set { SetValue(ForegroundProperty, value); }
        }
        
        
        public static readonly DependencyProperty FileSourceProperty =
            DependencyProperty.Register(
                "FileSource",
                typeof(FileSource),
                typeof(FileBadge),
                new PropertyMetadata(null)
            );
        public FileSource? FileSource
        {
            get { return (FileSource)GetValue(FileSourceProperty); }
            set { SetValue(FileSourceProperty, value); }
        }
        public FileBadge()
        {
            InitializeComponent();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = FileSource?.FilePath,
                    UseShellExecute = true // 关键：使用系统默认程序打开
                };
                Process.Start(psi);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"打开文件失败: {ex.Message}");
            }
        }
    }
}