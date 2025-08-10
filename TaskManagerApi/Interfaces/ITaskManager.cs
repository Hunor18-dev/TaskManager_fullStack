using TaskManagerApi.Models;

namespace TaskManagerApi.Interfaces
{
    public interface ITaskManager
    {
        /// <summary>
        /// Retrieves all tasks.
        /// </summary>
        Task<IEnumerable<TaskItem>> GetTasksAsync();

        /// <summary>
        /// Retrieves a specific task by its ID.
        /// </summary>
        Task<TaskItem?> GetTaskAsync(int id);

        /// <summary>
        /// Creates a new task.
        /// </summary>
        Task<TaskItem> CreateTaskAsync(TaskItem taskItem);

        /// <summary>
        /// Updates an existing task.
        /// </summary>
        Task<bool> UpdateTaskAsync(int id, TaskItem taskItem);

        /// <summary>
        /// Deletes a task.
        /// </summary>
        Task<bool> DeleteTaskAsync(int id);

        /// <summary>
        /// Updates the status of a task.
        /// </summary>
        Task<bool> UpdateTaskStatusAsync(int id, Models.TaskStatus status);

        /// <summary>
        /// Reorders tasks.
        /// </summary>
        Task<bool> ReorderTasksAsync(List<TaskOrderDto> order);
    }
}
