// SettingsModelService.cs
using System.Reflection;

using Microsoft.EntityFrameworkCore;

using SenNotes.Common.Helpers;
using SenNotes.Common.Models;
using SenNotes.Migrations.Context;
using SenNotes.Services.IServices;

namespace SenNotes.Services
{
    public class SettingsModelService : ISettingsModelService
    {
        private readonly string _dbPath = PathHelper.GetDbPath();
        private readonly AppDbContext _db = new AppDbContext();

        public SettingsModelService()
        {
            InitDb();
        }

        private void InitDb()
        {
            var dbDirectory = Path.GetDirectoryName(_dbPath);
            if (!Directory.Exists(dbDirectory))
            {
                Directory.CreateDirectory(dbDirectory);
            }

            if (!File.Exists(_dbPath))
            {
                using var stream = Assembly.GetExecutingAssembly()
                    .GetManifestResourceStream("model.db");
                if (stream == null)
                {
                    return;
                }

                using var fileStream = new FileStream(_dbPath, FileMode.Create, FileAccess.Write);
                stream.CopyTo(fileStream);
            }
        }

        public async Task<List<SettingModel?>> GetAllSettings()
        {
            return await _db.SettingModels.ToListAsync();
        }

        public async Task<SettingModel?> GetSettings(int id)
        {
            return await _db.SettingModels.FindAsync(id);
        }

        public async Task<SettingModel?> AddSettings(SettingModel? settingsModel)
        {
            var entity = await _db.SettingModels.AddAsync(settingsModel);
            await _db.SaveChangesAsync();
            return entity.Entity;
        }

        public async Task<SettingModel?> UpdateSettings(SettingModel? settingsModel)
        {
            if (settingsModel == null)
                return null;
            var entity = _db.SettingModels.Update(settingsModel);
            await _db.SaveChangesAsync();
            return entity.Entity;
        }

        public async Task<SettingModel?> DeleteSettings(int id)
        {
            var entity = await _db.SettingModels.FindAsync(id);
            if (entity != null)
            {
                _db.SettingModels.Remove(entity);
                await _db.SaveChangesAsync();
            }

            return entity;
        }
    }
}
