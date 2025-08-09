
using TaskManagerApi.Models;
namespace TaskManagerApi.Interfaces;

public interface ITaskRepository
{
    Task<IEnumerable<TaskItem>> GetAllTasksAsync();
    Task<TaskItem?> GetByIdAsync(int id);
    Task CreateAsync(TaskItem taskItem);
    Task UpdateAsync(TaskItem taskItem);
    Task DeleteAsync(int id);
    Task<int> GetMaxPosition();
}