namespace TaskManager.Service.Api.v1.TimeEntries;

public record TimeEntryDTO(
    string Id,
    DateTime StartTime,
    TimeSpan Duration,
    string? Description,
    string TaskId
);
