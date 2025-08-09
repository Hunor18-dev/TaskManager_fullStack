using Xunit;
using Moq;
using FluentAssertions;
using TaskManagerApi.Interfaces;
using TaskManagerApi.Managers;
using TaskManagerApi.Models;
using TaskManagerApi.Controllers;
using Microsoft.AspNetCore.Mvc;

public class TaskControllerTests
{
    private readonly Mock<ITaskManager> _taskManagerMock;
    private readonly TaskController _controller;

    public TaskControllerTests()
    {
        _taskManagerMock = new Mock<ITaskManager>();
        _controller = new TaskController(_taskManagerMock.Object);
    }

    [Fact]
    public async Task GetTasks_ReturnsOkResult_WithListOfTasks()
    {
        // Arrange
        var tasks = new List<TaskItem>
        {
            new TaskItem { Id = 1, Title = "Task 1" },
            new TaskItem { Id = 2, Title = "Task 2" }
        };
        _taskManagerMock.Setup(m => m.GetTasksAsync()).ReturnsAsync(tasks);

        // Act
        var result = await _controller.GetTasks();

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult.Value.Should().BeEquivalentTo(tasks);
    }

    [Fact]
    public async Task GetTask_ReturnsOkResult_WhenTaskExists()
    {
        // Arrange
        var task = new TaskItem { Id = 1, Title = "Task 1" };
        _taskManagerMock.Setup(m => m.GetTaskAsync(1)).ReturnsAsync(task);

        // Act
        var result = await _controller.GetTask(1);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult.Value.Should().BeEquivalentTo(task);
    }

    [Fact]
    public async Task GetTask_ReturnsNotFound_WhenTaskDoesNotExist()
    {
        // Arrange
        _taskManagerMock.Setup(m => m.GetTaskAsync(1)).ReturnsAsync((TaskItem)null);

        // Act
        var result = await _controller.GetTask(1);

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task CreateTask_ReturnsCreatedResult_WithNewTask()
    {
        // Arrange
        var newTask = new TaskItem { Title = "Task 1" };
        _taskManagerMock.Setup(m => m.CreateTaskAsync(newTask)).ReturnsAsync(newTask);

        // Act
        var result = await _controller.CreateTask(newTask);

        // Assert
        result.Should().BeOfType<CreatedAtActionResult>();
        var createdResult = result as CreatedAtActionResult;
        createdResult.Value.Should().BeEquivalentTo(newTask);
    }

    [Fact]
    public async Task CreateTask_ReturnsBadRequest_WhenModelStateIsInvalid()
    {
        // Arrange
        var newTask = new TaskItem { Title = "Task 1" };
        _controller.ModelState.AddModelError("Title", "Required");

        // Act
        var result = await _controller.CreateTask(newTask);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Fact]
    public async Task CreateTask_ReturnsBadRequest_WhenTaskCreationFails()
    {
        // Arrange
        var newTask = new TaskItem { Title = "Task 1" };
        _taskManagerMock.Setup(m => m.CreateTaskAsync(newTask)).ReturnsAsync((TaskItem)null);

        // Act
        var result = await _controller.CreateTask(newTask);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Fact]
    public async Task UpdateTask_ReturnsNoContent_WhenTaskIsUpdated()
    {
        // Arrange
        var updatedTask = new TaskItem { Id = 1, Title = "Updated Task" };
        _taskManagerMock.Setup(m => m.UpdateTaskAsync(1, updatedTask)).ReturnsAsync(true);

        // Act
        var result = await _controller.UpdateTask(1, updatedTask);

        // Assert
        result.Should().BeOfType<NoContentResult>();
    }

    [Fact]
    public async Task UpdateTask_ReturnsNotFound_WhenTaskDoesNotExist()
    {
        // Arrange
        var updatedTask = new TaskItem { Id = 1, Title = "Updated Task" };
        _taskManagerMock.Setup(m => m.UpdateTaskAsync(1, updatedTask)).ReturnsAsync(false);

        // Act
        var result = await _controller.UpdateTask(1, updatedTask);

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task DeleteTask_ReturnsNoContent_WhenTaskIsDeleted()
    {
        // Arrange
        _taskManagerMock.Setup(m => m.DeleteTaskAsync(1)).ReturnsAsync(true);

        // Act
        var result = await _controller.DeleteTask(1);

        // Assert
        result.Should().BeOfType<NoContentResult>();
    }

    [Fact]
    public async Task DeleteTask_ReturnsNotFound_WhenTaskDoesNotExist()
    {
        // Arrange
        _taskManagerMock.Setup(m => m.DeleteTaskAsync(1)).ReturnsAsync(false);

        // Act
        var result = await _controller.DeleteTask(1);

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }

}
