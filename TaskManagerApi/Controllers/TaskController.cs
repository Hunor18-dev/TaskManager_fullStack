using Microsoft.AspNetCore.Mvc;
using TaskManagerApi.Interfaces;
using TaskManagerApi.Models;

namespace TaskManagerApi.Controllers
{
    [Route("api/tasks")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly ITaskManager _taskManager;

        public TaskController(ITaskManager taskManager)
        {
            _taskManager = taskManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetTasks()
        {
            return Ok(await _taskManager.GetTasksAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTask(int id)
        {
            var task = await _taskManager.GetTaskAsync(id);
            return task == null ? NotFound() : Ok(task);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTask([FromBody] TaskItem taskItem)
        {
            var createdTask = await _taskManager.CreateTaskAsync(taskItem);
            return CreatedAtAction(nameof(GetTask), new { id = createdTask.Id }, createdTask);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(int id, [FromBody] TaskItem taskItem)
        {
            return await _taskManager.UpdateTaskAsync(id, taskItem) ? NoContent() : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            return await _taskManager.DeleteTaskAsync(id) ? NoContent() : NotFound();
        }

        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateTaskStatus(int id, [FromBody] Models.TaskStatus status)
        {
            return await _taskManager.UpdateTaskStatusAsync(id, status) ? NoContent() : NotFound();
        }

        [HttpPost("reorder")]
        public async Task<IActionResult> ReorderTasks([FromBody] List<TaskOrderDto> order)
        {
            return await _taskManager.ReorderTasksAsync(order) ? NoContent() : BadRequest("Invalid reorder request.");
        }
    }
}
