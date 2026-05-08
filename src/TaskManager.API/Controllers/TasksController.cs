using Microsoft.AspNetCore.Mvc;
using TaskManager.Core.Entities;
using TaskManager.Core.Interfaces;

namespace TaskManager.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TasksController:ControllerBase
{
    private readonly ITaskService _taskService;

    public TasksController(ITaskService taskService)
    {
        _taskService = taskService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll() =>
        Ok(await _taskService.GetAllTasksAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id) =>
        Ok(await _taskService.GetTaskByIdAsync(id));

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] TaskItem task)
    {
        var created = await _taskService.CreateTaskAsync(task);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] TaskItem task)
    {
        task.Id = id;
        return Ok(await _taskService.UpdateTaskAsync(task));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _taskService.DeleteTaskAsync(id);
        return NoContent();
    }
}
