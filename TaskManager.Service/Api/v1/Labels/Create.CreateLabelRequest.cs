namespace TaskManager.Service.Api.v1.Labels;

public record CreateLabelRequest(
    string Name,
    string Description,
    string SpaceId
);
