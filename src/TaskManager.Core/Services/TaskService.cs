using TaskManager.Core.Entities;
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

    public Task<TaskItem?> GetTaskByIdAsync(int id) =>
        _repository.GetByIdAsync(id);

    public Task<TaskItem> CreateTaskAsync(TaskItem task) =>
        _repository.CreateAsync(task);

    public async Task<TaskItem?> UpdateTaskAsync(TaskItem task)
    {
        if (task.IsCompleted && task.CompletedAt == null)
            task.CompletedAt = DateTime.UtcNow;

        return await _repository.UpdateAsync(task);
    }

    public Task<bool> DeleteTaskAsync(int id) =>
        _repository.DeleteAsync(id);
}
