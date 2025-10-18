using System.Reflection;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

using SenNotes.Common.Helpers;
using SenNotes.Common.Models;
using SenNotes.Migrations.Context;
using SenNotes.Services.IServices;

namespace SenNotes.Services
{
    public class TaskModelService : ITaskModelService
    {
        private readonly string _dbPath = PathHelper.GetDbPath();
        private readonly AppDbContext _db = new AppDbContext();

        public TaskModelService()
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

        public async Task<List<TaskModel?>> GetAllTask()
        {
            return await _db.TaskModels.ToListAsync();
        }

        public async Task<TaskModel?> AddTask(TaskModel? taskModel)
        {
            var entity = await _db.TaskModels.AddAsync(taskModel);
            await _db.SaveChangesAsync();
            return entity.Entity;
        }

        public async Task<TaskModel> DeleteTask(int id)
        {
            var entity = await _db.TaskModels.FindAsync(id);
            if (entity != null)
            {
                _db.TaskModels.Remove(entity);
                await _db.SaveChangesAsync();
            }

            return entity;
        }

        public async Task<TaskModel> UpdateTask(TaskModel taskModel)
        {
            var entity = _db.TaskModels.Update(taskModel);
            await _db.SaveChangesAsync();
            return entity.Entity;
        }

        public async Task<TaskModel?> GetTask(int id)
        {
            return await _db.TaskModels.FindAsync(id);
        }
    }
}