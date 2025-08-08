using Xunit;
using TaskManagerApi.Controllers;
using Moq;
using TaskManagerApi.Interfaces;
using TaskManagerApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace TaskManager.Tests;

public class UnitTest1
{
    public class TaskControllerTests
{
    [Fact]
    public void Get_ReturnsAllTasks()
    {
        var mockRepo = new Mock<ITaskRepository>();
        mockRepo.Setup(repo => repo.GetAllTasksAsync()).Returns(new Task<IEnumerable<TaskItem>>(() => new List<TaskItem>
        {
            new TaskItem { Id = 1, Title = "Test Task 1", Status = TaskManagerApi.Models.TaskStatus.Incomplete },
            new TaskItem { Id = 2, Title = "Test Task 2", Status = TaskManagerApi.Models.TaskStatus.Complete }
        }));

        var controller = new TaskController(mockRepo.Object);
        var result = controller.GetTasks();

        Assert.IsType<OkObjectResult>(result.Result);
    }
}

    [Fact]
    public void GetById_ReturnsTask_WhenTaskExists()
    {
        var mockRepo = new Mock<ITaskRepository>();
        mockRepo.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(new TaskItem { Id = 1, Title = "Test Task", Status = TaskManagerApi.Models.TaskStatus.Incomplete });

        var controller = new TaskController(mockRepo.Object);
        var result = controller.GetTask(1);

        Assert.IsType<OkObjectResult>(result.Result);
    }

    [Fact]
    public void GetById_ReturnsNotFound_WhenTaskDoesNotExist()
    {
        var mockRepo = new Mock<ITaskRepository>();
        mockRepo.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync((TaskItem)null);

        var controller = new TaskController(mockRepo.Object);
        var result = controller.GetTask(1);

        Assert.IsType<NotFoundResult>(result.Result);
    }
}
