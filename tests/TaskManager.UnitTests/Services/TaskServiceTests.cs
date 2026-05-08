using Moq;
using FluentAssertions;
using TaskManager.Core.Exceptions;
using TaskManager.Core.Entities;
using TaskManager.Core.Interfaces;
using TaskManager.Core.Services;

namespace TaskManager.UnitTests.Services;

public class TaskServiceTests
{
    private readonly Mock<ITaskRepository> _mockRepo;
    private readonly TaskService _taskService;

    public TaskServiceTests()
    {
        _mockRepo = new Mock<ITaskRepository>();
        _taskService = new TaskService(_mockRepo.Object);
    }

    #region GetAllTasksAsync
    [Fact]
    public async Task GetAllTasksAsync_ShouldReturnAllTasks()
    {
        // Arrange
        var tasks = new List<TaskItem>
    {
        new() { Id = 1, Title = "Task 1" },
        new() { Id = 2, Title = "Task 2" }
    };
        _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(tasks);

        // Act
        var result = await _taskService.GetAllTasksAsync();

        // Assert
        result.Should().HaveCount(2);
    }
    #endregion

    #region GetTaskByIdAsync
    [Fact]
    public async Task GetTaskByIdAsync_WhenTaskExists_ShouldReturnTask()
    {
        // Arrange
        var task = new TaskItem { Id = 1, Title = "Task 1" };
        _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(task);

        // Act
        var result = await _taskService.GetTaskByIdAsync(1);

        // Assert
        result.Should().NotBeNull();
        result!.Title.Should().Be("Task 1");
    }

    [Fact]
    public async Task GetTaskByIdAsync_WhenTaskNotFound_ShouldThrowNotFoundException() 
    {
        // Arrange
        _mockRepo.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((TaskItem?)null);

        // Act
        var act = async() => await _taskService.GetTaskByIdAsync(99);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }
    #endregion

    #region CreateTaskAsync
    [Fact]
    public async Task CreateTaskAsync_ShouldReturnCreatedTask() 
    {
        // Arrange
        var task = new TaskItem { Title = "New Task" };
        _mockRepo.Setup(r => r.CreateAsync(task)).ReturnsAsync(task);
        
        // Act
        var result = await _taskService.CreateTaskAsync(task);
        
        // Assert
        result.Should().NotBeNull();
        result.Title.Should().Be("New Task");
    }
    #endregion

    #region UpdateTaskAsync
    [Fact]
    public async Task UpdateTaskAsync_WhenCompleted_ShouldSetCompletedAt() 
    {
        // Arrange
        var task = new TaskItem { Id = 1, Title = "Task 1", IsCompleted = true };
        _mockRepo.Setup(r => r.GetByIdAsync(1))
             .ReturnsAsync(task);

        _mockRepo.Setup(r => r.UpdateAsync(It.IsAny<TaskItem>()))
            .ReturnsAsync((TaskItem t) => t);

        // Act
        var result = await _taskService.UpdateTaskAsync(task);

        // Assert
        result.Should().NotBeNull();
        result.CompletedAt.Should().NotBeNull();
    }

    [Fact]
    public async Task UpdateTaskAsync_WhenTaskNotFound_ShouldThrowNotFoundException() 
    {
        // Arrange
        var task = new TaskItem { Id = 99, Title = "Not Exist" };
        _mockRepo.Setup(r => r.UpdateAsync(task)).ReturnsAsync((TaskItem?)null);
        
        // Act
        var act = async() => await _taskService.UpdateTaskAsync(task);
        
        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }
    #endregion

    #region DeleteTaskAsync
    [Fact]
    public async Task DeleteTaskAsync_WhenTaskExists_ShouldComplete() 
    {
        // Arrange
        var task = new TaskItem { Id = 1, Title = "Task 1" };
        _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(task);
        _mockRepo.Setup(r => r.DeleteAsync(1)).ReturnsAsync(true);
        
        // Act
        var act = async() => await _taskService.DeleteTaskAsync(1);
        
        // Assert
        await act.Should().NotThrowAsync();
    }

    [Fact]
    public async Task DeleteTaskAsync_WhenTaskNotFound_ShouldThrowNotFoundException() 
    {
        // Arrange
        _mockRepo.Setup(r => r.DeleteAsync(99)).ReturnsAsync(false);
        
        // Act
        var act = async() => await _taskService.DeleteTaskAsync(99);
        
        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }
    #endregion
}
