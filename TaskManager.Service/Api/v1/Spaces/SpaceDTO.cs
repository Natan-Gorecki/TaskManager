namespace TaskManager.Service.Api.v1.Spaces;

public record SpaceDTO(
    string Id,
    string Name,
    string Key,
    string Description
);
