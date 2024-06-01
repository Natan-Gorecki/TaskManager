namespace TaskManager.Service.Api.v1.TimeEntries;

public record DeleteTimeEntryRequest(
    string Id,
    string TaskId
);
