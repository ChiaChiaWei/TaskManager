using System.Net;
using System.Net.Http.Json;
using TaskManager.Core.Entities;
using FluentAssertions;

namespace TaskManager.IntegrationTests.Controllers;

public class TasksControllerTests:IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public TasksControllerTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    #region Get 
    [Fact]
    public async Task GetAll_ShouldReturnOk()
    {
        // Act
        var response = await _client.GetAsync("/api/tasks");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetById_WhenTaskExists_ShouldReturnOk()
    {
        // Arrange
        var task = new TaskItem { Title = "Test Task" };
        await _client.PostAsJsonAsync("/api/tasks", task);
        
        // Act
        var response = await _client.GetAsync("/api/tasks/1");
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetById_WhenTaskNotFound_ShouldReturnNotFound()
    {
        // Act
        var response = await _client.GetAsync("/api/tasks/999");
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    #endregion

    #region Create
    [Fact]
    public async Task Create_ShouldReturnCreated()
    {
        // Arrange
        var task = new TaskItem { Title = "New Task" };
        
        // Act
        var response = await _client.PostAsJsonAsync("/api/tasks", task);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }
    #endregion

    #region Update
    [Fact]
    public async Task Update_WhenTaskExists_ShouldReturnOk()
    {
        // Arrange
        var task = new TaskItem { Title = "Original Task" };
        await _client.PostAsJsonAsync("/api/tasks", task);

        // Act
        var updatedTask = new TaskItem { Title = "Updated Task" };
        var response = await _client.PutAsJsonAsync("/api/tasks/1", updatedTask);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Update_WhenTaskNotFound_ShouldReturnNotFound()
    {
        // Arrange
        var updatedTask = new TaskItem { Title = "Updated Task", IsCompleted = true };
        
        // Act
        var response = await _client.PutAsJsonAsync("/api/tasks/999", updatedTask);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    #endregion

    #region Delete
    [Fact]
    public async Task Delete_WhenTaskExists_ShouldReturnNoContent()
    {
        // Arrange
        var task = new TaskItem { Title = "Task to Delete" };
        await _client.PostAsJsonAsync("/api/tasks", task);

        // Act
        var response = await _client.DeleteAsync("/api/tasks/1");
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task Delete_WhenTaskNotFound_ShouldReturnNotFound()
    {
        // Act
        var response = await _client.DeleteAsync("/api/tasks/999");
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    #endregion
}
