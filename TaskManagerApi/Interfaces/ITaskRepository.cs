using TaskManagerApi.Models;

namespace TaskManagerApi.Interfaces
{
    public interface ITaskRepository
    {
        /// <summary>
        /// Retrieves all tasks.
        /// </summary>
        Task<IEnumerable<TaskItem>> GetAllTasksAsync();

        /// <summary>
        /// Retrieves a task by its ID.
        /// </summary>
        Task<TaskItem?> GetByIdAsync(int id);

        /// <summary>
        /// Creates a new task.
        /// </summary>
        Task CreateAsync(TaskItem taskItem);

        /// <summary>
        /// Updates an existing task.
        /// </summary>
        Task UpdateAsync(TaskItem taskItem);

        /// <summary>
        /// Updates a range of tasks.
        /// </summary>
        Task UpdateRangeAsync(IEnumerable<TaskItem> taskItems);

        /// <summary>
        /// Deletes a task.
        /// </summary>
        Task DeleteAsync(int id);

        /// <summary>
        /// Retrieves the maximum position of tasks.
        /// </summary>
        Task<int> GetMaxPositionAsync();
    }
}
