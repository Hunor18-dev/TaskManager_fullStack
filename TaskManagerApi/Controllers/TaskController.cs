using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagerApi.Interfaces;
using TaskManagerApi.Models;

namespace TaskManagerApi.Controllers
{
    /*remove authorization for unit testing*/
    [Authorize]
    [Route("api/tasks")]
    [ApiController]
    [Produces("application/json")]
    public class TaskController : ControllerBase
    {
        private readonly ITaskManager _taskManager;

        public TaskController(ITaskManager taskManager)
        {
            _taskManager = taskManager;
        }

        /// <summary>
        /// Retrieves all tasks ordered by position.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<TaskItem>), 200)]
        public async Task<IActionResult> GetTasks()
        {
            return Ok(await _taskManager.GetTasksAsync());
        }

        /// <summary>
        /// Retrieves a specific task by its ID.
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(TaskItem), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetTask(int id)
        {
            var task = await _taskManager.GetTaskAsync(id);
            return task == null ? NotFound() : Ok(task);
        }

        /// <summary>
        /// Creates a new task.
        /// </summary>
        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(TaskItem), 201)]
        public async Task<IActionResult> CreateTask([FromBody] TaskItem taskItem)
        {
            var createdTask = await _taskManager.CreateTaskAsync(taskItem);
            if(createdTask == null)
            {
                return BadRequest("Task creation failed.");
            }
            return CreatedAtAction(nameof(GetTask), new { id = createdTask.Id }, createdTask);
        }

        /// <summary>
        /// Updates an existing task by ID.
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateTask(int id, [FromBody] TaskItem taskItem)
        {
            return await _taskManager.UpdateTaskAsync(id, taskItem) ? NoContent() : NotFound();
        }

        /// <summary>
        /// Deletes a task by ID.
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteTask(int id)
        {
            return await _taskManager.DeleteTaskAsync(id) ? NoContent() : NotFound();
        }

        /// <summary>
        /// Updates only the status of a task.
        /// </summary>
        [HttpPut("{id}/status")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateTaskStatus(int id, [FromBody] Models.TaskStatus status)
        {
            return await _taskManager.UpdateTaskStatusAsync(id, status) ? NoContent() : NotFound();
        }

        /// <summary>
        /// Reorders tasks based on the provided order list.
        /// </summary>
        [HttpPost("reorder")]
        [Consumes("application/json")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> ReorderTasks([FromBody] List<TaskOrderDto> order)
        {
            return await _taskManager.ReorderTasksAsync(order) ? NoContent() : BadRequest("Invalid reorder request.");
        }
    }
}
