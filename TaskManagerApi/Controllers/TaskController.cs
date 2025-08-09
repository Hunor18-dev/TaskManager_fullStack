using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagerApi.Interfaces;
using TaskManagerApi.Models;

namespace TaskManagerApi.Controllers;

[Route("api/tasks")]
[ApiController]
public class TaskController : ControllerBase
{
    private readonly ITaskRepository _taskRepository;
    public TaskController(ITaskRepository taskRepository)
    {
        this._taskRepository = taskRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetTasks()
    {
        var tasks = await this._taskRepository.GetAllTasksAsync();
        return Ok(tasks);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTask(int id)
    {
        var task = await this._taskRepository.GetByIdAsync(id);
        if (task == null)
        {
            return NotFound();
        }
        return Ok(task);
    }

    [HttpPost]
    public async Task<IActionResult> CreateTask([FromBody] TaskManagerApi.Models.TaskItem taskItem)
    {
        taskItem.CreatedAt = DateTime.UtcNow; // enforce backend time
        var maxPosition = await this._taskRepository.GetMaxPosition();
        taskItem.Position = maxPosition + 1;
        await this._taskRepository.CreateAsync(taskItem);
        return CreatedAtAction(nameof(GetTask), new { id = taskItem.Id }, taskItem);
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTask(int id, TaskItem taskItem)
    {
        if (id != taskItem.Id)
        {
            return BadRequest("Task ID mismatch.");
        }

        var existingTask = await this._taskRepository.GetByIdAsync(id);
        if (existingTask == null)
        {
            return NotFound();
        }

        existingTask.Status = taskItem.Status;
        try
        {
            await this._taskRepository.UpdateAsync(existingTask);
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!this._taskExists(id))
            {
                return NotFound();
            }
            throw;
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTask(int id)
    {
        var taskItem = await this._taskRepository.GetByIdAsync(id);
        if (taskItem == null)
        {
            return NotFound();
        }

        await this._taskRepository.DeleteAsync(id);
        return NoContent();
    }

    [HttpPut("{id}/status")]
    public async Task<IActionResult> UpdateTaskStatus(int id, [FromBody] Models.TaskStatus statusUpdate)
    {
        var taskItem = await this._taskRepository.GetByIdAsync(id);
        if (taskItem == null)
        {
            return NotFound();
        }

        taskItem.Status = statusUpdate;
        await this._taskRepository.UpdateAsync(taskItem);
        return NoContent();
    }

    [HttpPost("reorder")]
    public async Task<IActionResult> ReorderTasks([FromBody] List<TaskOrderDto> order)
    {
        foreach (var item in order)
        {
            var task = await this._taskRepository.GetByIdAsync(item.Id);
            if (task != null) task.Position = item.Position;
            await this._taskRepository.UpdateAsync(task);
        }
        return NoContent();
    }

    private bool _taskExists(int id)
    {
        return this._taskRepository.GetByIdAsync(id) != null;
    }
}