using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

using SenNotes.Common.Messages;
using SenNotes.Common.Models;
using SenNotes.Services.IServices;



namespace SenNotes.ViewModels
{
    public partial class TaskModelUpdateWindowVm : ObservableObject
    {
        [ObservableProperty]
        private TaskModel? _taskModel;

        private readonly ITaskModelService _taskModelService;
        

        public TaskModelUpdateWindowVm(ITaskModelService taskModelService)
        {
            _taskModelService = taskModelService;
            WeakReferenceMessenger
                .Default.Register<TaskModelMessage>(this, (w, m) =>
            {
                TaskModel = m.TaskModel;
            });
        }
        
        [RelayCommand]
        private async Task UpdateTaskModel()
        {
            try
            {
                if (TaskModel != null)
                {
                    await _taskModelService.UpdateTask(TaskModel);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
            WeakReferenceMessenger.Default.Send(new TaskHasUpdateMessage());
            
        }
    }
}