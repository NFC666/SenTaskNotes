using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

using Newtonsoft.Json;

using SenNotes.Common.Helpers;
using SenNotes.Common.Models;

namespace SenNotes.Migrations.Context
{

    public class AppDbContext : DbContext
    {
        public DbSet<TaskModel> TaskModels { get; set; }
        public DbSet<SettingModel> SettingModels { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var dbPath = PathHelper.GetDbPath();
            optionsBuilder.UseSqlite($"Data Source={PathHelper.GetDbPath()}");
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var converter = new ValueConverter<List<FileSource>, string>(
                v => JsonConvert.SerializeObject(v),   // 写入数据库时序列化
                v => JsonConvert.DeserializeObject<List<FileSource>>(v) ?? new List<FileSource>()  // 读取时反序列化
            );

            modelBuilder.Entity<TaskModel>()
                .Property(t => t.Files)
                .HasConversion(converter);
        }
    }
}