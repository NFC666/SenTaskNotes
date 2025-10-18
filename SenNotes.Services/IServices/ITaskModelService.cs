using SenNotes.Common.Models;

namespace SenNotes.Services.IServices
{
    public interface ITaskModelService
    {
        //增删改查
        Task<List<TaskModel?>> GetAllTask();
        Task<TaskModel?> AddTask(TaskModel? taskModel);
        Task<TaskModel> DeleteTask(int id);
        Task<TaskModel> UpdateTask(TaskModel taskModel);
        Task<TaskModel?> GetTask(int id);
    }
}