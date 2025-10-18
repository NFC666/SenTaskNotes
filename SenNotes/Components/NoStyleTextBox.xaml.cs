using System.Windows;
using System.Windows.Media;

using UserControl = System.Windows.Controls.UserControl;

namespace SenNotes.Components
{
    public partial class NoStyleTextBox : UserControl
    {
        // 依赖属性
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(
                "Text",
                typeof(string),
                typeof(NoStyleTextBox),
                new PropertyMetadata(null)
            );
        public string? Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
        public static readonly DependencyProperty ForegroundProperty =
            DependencyProperty.Register(
                "Foreground",
                typeof(SolidColorBrush),
                typeof(NoStyleTextBox),
                new PropertyMetadata(new SolidColorBrush(Colors.White))
            );
        public SolidColorBrush? Foreground
        {
            get { return (SolidColorBrush)GetValue(ForegroundProperty); }
            set { SetValue(ForegroundProperty, value); }
        }
        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register(
                "Header",
                typeof(string),
                typeof(NoStyleTextBox),
                new PropertyMetadata(null)
            );
        public string? Header
        {
            get { return (string)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }
        
        public NoStyleTextBox()
        {
            InitializeComponent();
        }
    }
}