using TaskManager.Core.Entities;
using TaskManager.Core.Exceptions;
using TaskManager.Core.Interfaces;

namespace TaskManager.Core.Services;

public class TaskService : ITaskService
{
    private readonly ITaskRepository _repository;

    public TaskService(ITaskRepository repository)
    {
        _repository = repository;
    }

    public Task<IEnumerable<TaskItem>> GetAllTasksAsync() =>
        _repository.GetAllAsync();

    public async Task<TaskItem> GetTaskByIdAsync(int id) 
    {
        var task = await _repository.GetByIdAsync(id);
        if (task == null)
            throw new NotFoundException("Task", id);
        return task;
    }

    public async Task<TaskItem> CreateTaskAsync(TaskItem task) 
    {
        if (string.IsNullOrEmpty(task.Title))
            throw new ValidationException("Title is required.");
        return await _repository.CreateAsync(task);
    }

    public async Task<TaskItem?> UpdateTaskAsync(TaskItem task)
    {
        if (string.IsNullOrEmpty(task.Title))
            throw new ValidationException("Title is required.");

        var existingTask = await _repository.GetByIdAsync(task.Id);
        if (existingTask == null)
            throw new NotFoundException("Task", task.Id);

        if (task.IsCompleted && task.CompletedAt == null)
            task.CompletedAt = DateTime.UtcNow;

        return await _repository.UpdateAsync(task) 
            ?? throw new NotFoundException("Task", task.Id);
    }

    public async Task DeleteTaskAsync(int id) 
    { 
        var existingTask = await _repository.GetByIdAsync(id);
        if (existingTask==null)
            throw new NotFoundException("Task", id);

        await _repository.DeleteAsync(id); 
    }
        
}
