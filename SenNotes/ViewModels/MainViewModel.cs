using System.Collections.ObjectModel;
using System.Windows;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;



using Microsoft.Extensions.DependencyInjection;

using SenNotes.Common.Messages;
using SenNotes.Common.Models;
using SenNotes.Extensions;
using SenNotes.Managers;
using SenNotes.Services;
using SenNotes.Services.IServices;
using SenNotes.Views;

using Serilog;

using MessageBox = System.Windows.MessageBox;
using TaskStatus = SenNotes.Common.Models.TaskStatus;

namespace SenNotes.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {

        [ObservableProperty] private bool _isEnable = true;
        [ObservableProperty] private bool _isLoading = false;
        [ObservableProperty] private string[]? _filePaths;


        partial void OnFilePathsChanged(string[]? value)
        {
            if (value is null) return;
            AddFiles(value);
        }

        [ObservableProperty] private string _message = "请总结";
        [ObservableProperty] private ObservableCollection<FileSource> _fileSources = new();
        [ObservableProperty] private ObservableCollection<TaskModel> _tasks = new();

        private readonly WindowManager _windowManager = new WindowManager();
        private readonly ITaskModelService _taskModelService;
        private AiAssistService _aiAssistService;
        private readonly ToastService _toastService;

        public MainViewModel(
            ITaskModelService taskModelService
            , ToastService toastService)
        {
            Log.Information("MainViewModel启动了");
            _taskModelService = taskModelService;
            _toastService = toastService;
            _ = InitAsync();
            WeakReferenceMessenger.Default.Register<TaskHasUpdateMessage>(this
                , async (_, _) => await InitAsync());
        }


        private async Task InitAsync()
        {
            Log.Information("InitAsync开始执行");
            Tasks.Clear();
            try
            {
                _aiAssistService = App.Service.GetRequiredService<AiAssistService>();
                var task = await _taskModelService.GetAllTask();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Log.Error(ex.Message);
            }
            finally
            {
                Tasks = new ObservableCollection<TaskModel>();

                SortTasks();
                NotifyEmergencyTask();
            }
        }

        private void NotifyEmergencyTask()
        {
            // 获得紧急任务
            var emergencyTasks = Tasks
                .Where(task => task.Status is TaskStatus.Emergency or TaskStatus.Overdue)
                .ToList();

            if (emergencyTasks.Count == 0)
                return;

            // 调用 ToastService
            var toastService = new ToastService();
            toastService.Show(emergencyTasks);
        }


        private void SortTasks()
        {
            Tasks.Sort((x, y)
                => y.Progress.CompareTo(x.Progress));
        }

        //选择文件命令，要求只能选择Img类型，txt 类型，doc 类型，pdf 类型
        [RelayCommand]
        private void SelectFiles()
        {
            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.Multiselect = true;
            dialog.Filter = "图片文件|*.png;*.jpg;*.jpeg;*.gif|文本文件|*.txt|Word文件|*.doc;*.docx|PDF文件|*.pdf";
            if (dialog.ShowDialog() == false)
                return;
            AddFiles(dialog.FileNames);
        }

        private void AddFiles(string[] filePaths)
        {
            foreach (var filePath in filePaths)
            {
                switch (filePath)
                {
                    case string path when path.EndsWith(".png")
                                          || path.EndsWith(".jpg")
                                          || path.EndsWith(".jpeg")
                                          || path.EndsWith(".gif"):
                        FileSources.Add(new FileSource() { FileType = FileType.Img, FilePath = filePath });
                        break;
                    case string path when path.EndsWith(".txt"):
                        FileSources.Add(new FileSource() { FileType = FileType.Text, FilePath = filePath });
                        break;
                    case string path when path.EndsWith(".doc")
                                          || path.EndsWith(".docx"):
                        FileSources.Add(new FileSource() { FileType = FileType.Doc, FilePath = filePath });
                        break;
                    case string path when path.EndsWith(".pdf"):
                        FileSources.Add(new FileSource() { FileType = FileType.Pdf, FilePath = filePath });
                        break;
                }
            }
        }

        [RelayCommand]
        private void RemoveFile(FileSource fileSource)
        {
            try
            {
                FileSources.Remove(fileSource);
            }
            catch (Exception ex)
            {
                MessageBox.Show("删除文件出现错误：" + ex.Message);
                Log.Error(ex.Message);
            }
        }

        [RelayCommand]
        private async Task EnterKeyDown()
        {
            try
            {
                StartLoading();
                var tasks = await _aiAssistService
                    .InCloudFromFileSourcesAndMessageAsync([..FileSources]
                        , Message);
                foreach (var t in tasks)
                {
                    Tasks.Add(t);
                    await _taskModelService.AddTask(t);
                }

                SortTasks();
                StopLoading();
            }
            catch (Exception ex)
            {
                Log.Error("分析结果出错！" + ex.Message + "\n可以尝试重新分析");
                MessageBox.Show("分析结果出错！" + ex.Message + "\n可以尝试重新分析");
            }
        }

        private void StartLoading()
        {
            IsEnable = false;
            IsLoading = true;
        }

        private void StopLoading()
        {
            IsEnable = true;
            IsLoading = false;
        }

        [RelayCommand]
        private async Task RemoveTask(TaskModel taskModel)
        {
            try
            {
                var result = MessageBox
                    .Show("确定要删除吗？", "提示", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    await _taskModelService.DeleteTask(taskModel.Id);
                    Tasks.Remove(taskModel);
                }

                SortTasks();
            }
            catch (Exception ex)
            {
                Log.Error("删除任务失败" + ex.Message);
                MessageBox.Show("删除任务失败" + ex.Message);
            }
        }

        [RelayCommand]
        private void UpdateTask(TaskModel taskModel)
        {
            _windowManager.OpenWindowWithNoChrome<TaskModelUpdateWindow>();
            WeakReferenceMessenger.Default.Send(new TaskModelMessage(taskModel));
        }
    }
}