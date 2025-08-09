using Xunit;
using Moq;
using FluentAssertions;
using TaskManagerApi.Interfaces;
using TaskManagerApi.Managers;
using TaskManagerApi.Models;

public class TaskManagerTests
{
    private readonly Mock<ITaskRepository> _repoMock;
    private readonly TaskManager _manager;

    public TaskManagerTests()
    {
        _repoMock = new Mock<ITaskRepository>();
        _manager = new TaskManager(_repoMock.Object);
    }

    [Fact]
    public async Task CreateTaskAsync_ShouldAssignPositionAndCreatedAt()
    {
        // Arrange
        var newTask = new TaskItem { Title = "Test" };
        _repoMock.Setup(r => r.GetMaxPositionAsync()).ReturnsAsync(5);

        // Act
        var result = await _manager.CreateTaskAsync(newTask);

        // Assert
        result.Position.Should().Be(6);
        result.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
        _repoMock.Verify(r => r.CreateAsync(It.IsAny<TaskItem>()), Times.Once);
    }

    [Fact]
    public async Task ReorderTasksAsync_ShouldUpdatePositions()
    {
        // Arrange
        var existingTasks = new List<TaskItem>
        {
            new TaskItem { Id = 1, Position = 1 },
            new TaskItem { Id = 2, Position = 2 }
        };
        var newOrder = new List<TaskOrderDto>
        {
            new TaskOrderDto { Id = 1, Position = 2 },
            new TaskOrderDto { Id = 2, Position = 1 }
        };
        _repoMock.Setup(r => r.GetAllTasksAsync()).ReturnsAsync(existingTasks);

        // Act
        var result = await _manager.ReorderTasksAsync(newOrder);

        // Assert
        result.Should().BeTrue();
        existingTasks.First(t => t.Id == 1).Position.Should().Be(2);
        existingTasks.First(t => t.Id == 2).Position.Should().Be(1);
        _repoMock.Verify(r => r.UpdateRangeAsync(It.IsAny<IEnumerable<TaskItem>>()), Times.Once);
    }

    [Fact]
    public async Task UpdateTaskAsync_ShouldReturnFalse_WhenTaskNotFound()
    {
        _repoMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((TaskItem)null);

        var result = await _manager.UpdateTaskAsync(1, new TaskItem());
        result.Should().BeFalse();
    }

    [Fact]
    public async Task DeleteTaskAsync_ShouldReturnFalse_WhenTaskNotFound()
    {
        _repoMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((TaskItem)null);

        var result = await _manager.DeleteTaskAsync(1);
        result.Should().BeFalse();
    }

    [Fact]
    public async Task DeleteTaskAsync_ShouldReturnTrue_WhenTaskIsDeleted()
    {
        _repoMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(new TaskItem());

        var result = await _manager.DeleteTaskAsync(1);
        result.Should().BeTrue();
    }

}
