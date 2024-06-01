namespace TaskManager.Service.Api.v1.TimeEntries;

public record GetByIdForTaskRequest(
    string Id,
    string TaskId
);
