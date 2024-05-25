namespace TaskManager.Service.Api.v1.Labels;

public record LabelDTO(
    string Id,
    string Name,
    string Description,
    string SpaceId
);
