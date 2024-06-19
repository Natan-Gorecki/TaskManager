namespace TaskManager.Service.Api.v1.TimeEntries;

public record CreateTimeEntryRequest(
    DateTime StartTime,
    TimeSpan Duration,
    string? Description,
    string TaskId
);
