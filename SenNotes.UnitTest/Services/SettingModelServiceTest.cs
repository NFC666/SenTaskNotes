using SenNotes.Common.Models;
using SenNotes.Services;
using SenNotes.Services.IServices;

using Xunit.Abstractions;

namespace SenNotes.UnitTest.Services
{
    public class SettingModelServiceTest
    {
        private readonly ITestOutputHelper _helper;

        private readonly ISettingsModelService _settingsModelService
            = new SettingsModelService();

        public SettingModelServiceTest(ITestOutputHelper helper)
        {
            _helper = helper;
        }

        [Fact]
        private async Task GetAllSettings_Should_Success()
        {
            var res = await _settingsModelService.GetAllSettings();
            Assert.NotNull(res);
        }

        [Fact]
        private async Task AddSetting_Should_Success()
        {
            var setting = new SettingModel()
            {
                ApiKey = "sk-nwjucxbqmkaziurzzrkpnnbfuoxnwodvmnlagzbniyjmhnwt",
                BaseUrl = "https://api.siliconflow.cn/"
            };
            var res = await _settingsModelService.AddSettings(setting);
            Assert.NotNull(res);
        }
    }
}