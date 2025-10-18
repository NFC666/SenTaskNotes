using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using SenNotes.Common.Messages;
using SenNotes.Common.Models;
using SenNotes.Repository.Factories;
using SenNotes.Services.IServices;

using MessageBox = System.Windows.MessageBox;

namespace SenNotes.ViewModels
{
    public partial class SettingsWindowViewModel : ObservableObject
    {
        private readonly ISettingsModelService _settingsModelService;

        [ObservableProperty]
        private SettingModel aiSettings = new SettingModel();

        [ObservableProperty]
        private List<SettingModel> settingsModels = new();

        public SettingsWindowViewModel(ISettingsModelService settingsModelService)
        {
            _settingsModelService = settingsModelService;
            _ = InitializeAsync();
        }

        private async Task InitializeAsync()
        {
            var settings = await _settingsModelService.GetAllSettings();
            SettingsModels = settings ?? new List<SettingModel>();

            // 如果数据库中没有设置，创建一个新的
            if (SettingsModels.Count == 0)
            {
                AiSettings = new SettingModel();
            }
            else
            {
                AiSettings = SettingsModels.First();
            }
        }

        [RelayCommand]
        private async Task SetHttpFactoryInfo()
        {
            if (AiSettings is null)
            {
                MessageBox.Show("设置不能为空！");
                return;
            }

            try
            {
                HttpFactory.ConfigClient(AiSettings.ApiKey, AiSettings.BaseUrl);

                if (AiSettings.Id == 0)
                {
                    // 新增
                    await _settingsModelService.AddSettings(AiSettings);
                    SettingsModels.Add(AiSettings);
                }
                else
                {
                    // 更新
                    await _settingsModelService.UpdateSettings(AiSettings);
                }

                MessageBox.Show("设置成功");
                WeakReferenceMessenger.Default.Send(new CloseSettingsWindow());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"出现问题！{ex.Message}");
            }
        }
    }
}
