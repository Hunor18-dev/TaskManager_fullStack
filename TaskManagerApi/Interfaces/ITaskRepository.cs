using TaskManagerApi.Models;

namespace TaskManagerApi.Interfaces
{
    public interface ITaskRepository
    {
        Task<IEnumerable<TaskItem>> GetAllTasksAsync();
        Task<TaskItem?> GetByIdAsync(int id);
        Task CreateAsync(TaskItem taskItem);
        Task UpdateAsync(TaskItem taskItem);
        Task UpdateRangeAsync(IEnumerable<TaskItem> taskItems);
        Task DeleteAsync(int id);
        Task<int> GetMaxPositionAsync();
    }
}
