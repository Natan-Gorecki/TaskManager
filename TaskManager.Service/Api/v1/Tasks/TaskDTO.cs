namespace TaskManager.Service.Api.v1.Tasks;

public record TaskDTO(
    string Id,
    string Name,
    string Description,
    string? ParentTaskId,
    string SpaceId,
    TaskStatusDTO Status,
    TaskTypeDTO Type
);
