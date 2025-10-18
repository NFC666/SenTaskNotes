// ISettingsModelService.cs
using SenNotes.Common.Models;

namespace SenNotes.Services.IServices
{
    public interface ISettingsModelService
    {
        Task<List<SettingModel?>> GetAllSettings();
        Task<SettingModel?> GetSettings(int id);
        Task<SettingModel?> AddSettings(SettingModel? settingsModel);
        Task<SettingModel?> UpdateSettings(SettingModel? settingsModel);
        Task<SettingModel?> DeleteSettings(int id);
    }
}
