namespace TaskManager.Service.Api.v1.Tasks;

public record UpdateTaskRequest(
    string Id,
    string Name,
    string Description,
    string? ParentTaskId,
    TaskStatusDTO Status,
    TaskTypeDTO Type
);
