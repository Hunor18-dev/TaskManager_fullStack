using TaskManagerApi.Interfaces;
using TaskManagerApi.Models;

namespace TaskManagerApi.Managers
{
    public class TaskManager : ITaskManager
    {
        private readonly ITaskRepository _taskRepository;

        public TaskManager(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<IEnumerable<TaskItem>> GetTasksAsync()
        {
            return await _taskRepository.GetAllTasksAsync();
        }

        public async Task<TaskItem?> GetTaskAsync(int id)
        {
            return await _taskRepository.GetByIdAsync(id);
        }

        public async Task<TaskItem> CreateTaskAsync(TaskItem taskItem)
        {
            taskItem.CreatedAt = DateTime.UtcNow;
            var maxPosition = await _taskRepository.GetMaxPositionAsync();
            taskItem.Position = maxPosition + 1;
            await _taskRepository.CreateAsync(taskItem);
            return taskItem;
        }

        public async Task<bool> UpdateTaskAsync(int id, TaskItem taskItem)
        {
            var existingTask = await _taskRepository.GetByIdAsync(id);
            if (existingTask == null) return false;

            existingTask.Status = taskItem.Status;
            existingTask.Title = taskItem.Title;
            existingTask.Description = taskItem.Description;

            await _taskRepository.UpdateAsync(existingTask);
            return true;
        }

        public async Task<bool> DeleteTaskAsync(int id)
        {
            var task = await _taskRepository.GetByIdAsync(id);
            if (task == null) return false;

            await _taskRepository.DeleteAsync(id);
            return true;
        }

        public async Task<bool> UpdateTaskStatusAsync(int id, Models.TaskStatus status)
        {
            var task = await _taskRepository.GetByIdAsync(id);
            if (task == null) return false;

            task.Status = status;
            await _taskRepository.UpdateAsync(task);
            return true;
        }

        public async Task<bool> ReorderTasksAsync(List<TaskOrderDto> order)
        {
            if (order == null || !order.Any()) return false;

            var allTasks = await _taskRepository.GetAllTasksAsync();
            var tasksToUpdate = allTasks
                .Join(order, t => t.Id, o => o.Id, (t, o) =>
                {
                    t.Position = o.Position;
                    return t;
                })
                .ToList();

            if (!tasksToUpdate.Any()) return false;

            await _taskRepository.UpdateRangeAsync(tasksToUpdate);
            return true;
        }
    }
}
