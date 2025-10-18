using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

using SenNotes.Common.Models;

using Color = System.Windows.Media.Color;
using TaskStatus = SenNotes.Common.Models.TaskStatus;
using UserControl = System.Windows.Controls.UserControl;

namespace SenNotes.Components
{
    public partial class TaskCard : UserControl
    {

        
        public static readonly DependencyProperty BgColorProperty =
            DependencyProperty.Register(
                nameof(BgColor),
                typeof(Color),
                typeof(TaskCard),
                new PropertyMetadata(Colors.ForestGreen)
            );
        
        public Color BgColor
        {
            get => (Color)GetValue(BgColorProperty);
            set => SetValue(BgColorProperty, value);
        }
        public static readonly DependencyProperty TaskModelProperty =
            DependencyProperty.Register(
                "TaskModel",
                typeof(TaskModel),
                typeof(TaskCard),
                new PropertyMetadata(null, OnTaskModelPropertyChanged)
            );

        private static void OnTaskModelPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not TaskCard control)
                return;
            
            if (e.NewValue is not TaskModel model)
                return;
            switch (model.Status)
            {
                case TaskStatus.Easy:
                    control.BgColor = Colors.ForestGreen;
                    break;
                case TaskStatus.Notice:
                    control.BgColor = Colors.Orange;
                    break;
                case TaskStatus.Emergency:
                    control.BgColor = Colors.Purple;
                    break;
                case TaskStatus.Overdue:
                    control.BgColor = Colors.DarkRed;
                    break;
                default:
                    control.BgColor = Colors.Black;
                    break;
            }
            
            // 根据值修改颜色
            // if (value is >= 0 and <= 20)
            //     control.BgColor = Colors.ForestGreen;
            // else if (value is > 20 and <= 50)
            //     control.BgColor = Colors.Orange;
            // else if (value is > 50 and < 100)
            //     control.BgColor = Colors.Purple;
            // else
            //     control.BgColor = Colors.DarkRed;
        }

        public TaskModel? TaskModel
        {
            get { return (TaskModel)GetValue(TaskModelProperty); }
            set { SetValue(TaskModelProperty, value); }
        }
        public TaskCard()
        {
            InitializeComponent();
        }
    }
}