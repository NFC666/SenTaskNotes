using System.Windows;
using Microsoft.Xaml.Behaviors;

using DataFormats = System.Windows.DataFormats;
using DragDropEffects = System.Windows.DragDropEffects;
using DragEventArgs = System.Windows.DragEventArgs;

namespace SenNotes.Behaviors
{
    public class FileDragBehavior : Behavior<FrameworkElement>
    {
        public static readonly DependencyProperty FilesProperty =
            DependencyProperty.Register(
                nameof(Files),
                typeof(string[]),
                typeof(FileDragBehavior),
                new PropertyMetadata(null));

        public string[] Files
        {
            get => (string[])GetValue(FilesProperty);
            set => SetValue(FilesProperty, value);
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.AllowDrop = true;
            AssociatedObject.DragOver += OnDragOver;
            AssociatedObject.Drop += OnDrop; // ← 添加 Drop 事件
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.DragOver -= OnDragOver;
            AssociatedObject.Drop -= OnDrop;
        }

        private void OnDragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effects = DragDropEffects.Copy;
            else
                e.Effects = DragDropEffects.None;

            e.Handled = true;
        }

        private void OnDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                var files = (string[])e.Data.GetData(DataFormats.FileDrop);
                Files = files; // ← 核心：设置依赖属性，通知绑定更新
            }
        }
    }
}