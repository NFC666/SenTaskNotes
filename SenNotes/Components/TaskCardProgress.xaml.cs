using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

using UserControl = System.Windows.Controls.UserControl;


namespace SenNotes.Components
{
    public partial class TaskCardProgress : UserControl
    {
        // public static readonly DependencyProperty BgColorProperty =
        //     DependencyProperty.Register(
        //         nameof(BgColor),
        //         typeof(Color),
        //         typeof(TaskCardProgress),
        //         new PropertyMetadata(Colors.Transparent)
        //     );
        //
        // public Color BgColor
        // {
        //     get => (Color)GetValue(BgColorProperty);
        //     set => SetValue(BgColorProperty, value);
        // }

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(
                "Value",
                typeof(double),
                typeof(TaskCardProgress),
                new PropertyMetadata(1.0)
            );

        // private static void OnValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        // {
        //     if (d is not TaskCardProgress control)
        //         return;
        //
        //     var value = (double)e.NewValue;
        //
        //     // 根据值修改颜色
        //     if (value is >= 0 and <= 20)
        //         control.BgColor = Colors.ForestGreen;
        //     else if (value is > 20 and <= 50)
        //         control.BgColor = Colors.Orange;
        //     else if (value is > 50 and <= 100)
        //         control.BgColor = Colors.Purple;
        //     else
        //         control.BgColor = Colors.DarkRed;
        // }

        public double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public static readonly DependencyProperty ProgressStartTimeProperty =
            DependencyProperty.Register(
                "ProgressStartTime",
                typeof(DateTime),
                typeof(TaskCardProgress),
                new PropertyMetadata(DateTime.MinValue)
            );

        public DateTime? ProgressStartTime
        {
            get { return (DateTime)GetValue(ProgressStartTimeProperty); }
            set { SetValue(ProgressStartTimeProperty, value); }
        }

        public static readonly DependencyProperty ProgressStopTimeProperty =
            DependencyProperty.Register(
                "ProgressStopTime",
                typeof(DateTime),
                typeof(TaskCardProgress),
                new PropertyMetadata(DateTime.MaxValue)
            );
        


        public DateTime? ProgressStopTime
        {
            get { return (DateTime)GetValue(ProgressStopTimeProperty); }
            set { SetValue(ProgressStopTimeProperty, value); }
        }

        public TaskCardProgress()
        {
            InitializeComponent();
        }
    }
}