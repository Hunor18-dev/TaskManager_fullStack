using TaskManagerApi.Models;

namespace TaskManagerApi.Interfaces
{
    public interface ITaskManager
    {
        Task<IEnumerable<TaskItem>> GetTasksAsync();
        Task<TaskItem?> GetTaskAsync(int id);
        Task<TaskItem> CreateTaskAsync(TaskItem taskItem);
        Task<bool> UpdateTaskAsync(int id, TaskItem taskItem);
        Task<bool> DeleteTaskAsync(int id);
        Task<bool> UpdateTaskStatusAsync(int id, Models.TaskStatus status);
        Task<bool> ReorderTasksAsync(List<TaskOrderDto> order);
    }
}
