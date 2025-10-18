

using Microsoft.Toolkit.Uwp.Notifications;

using SenNotes.Common.Models;

namespace SenNotes.Services
{
    public class ToastService
    {
        public void Show(List<TaskModel>? tasks)
        {
            if (tasks == null || tasks.Count == 0)
                return;

            // 生成通知内容
            var title = "⚠️ 紧急任务提醒";
            var bodyLines = tasks
                .Take(3).Select(t => $"• {t.TaskTitle} ({t.Status})").ToList();
            if (tasks.Count > 3)
                bodyLines.Add($"...以及其他 {tasks.Count - 3} 个任务");

            var body = string.Join("\n", bodyLines);

            // 创建并显示通知
            new ToastContentBuilder()
                .AddArgument("action", "viewTasks")
                .AddText(title)
                .AddText(body)
                .Show();
        }
    }
}