namespace TaskManager.Service.Api.v1.Spaces;

public record CreateSpaceRequest(
    string Name,
    string Key,
    string Description
);
