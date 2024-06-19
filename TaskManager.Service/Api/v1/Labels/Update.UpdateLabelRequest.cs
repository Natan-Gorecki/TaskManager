namespace TaskManager.Service.Api.v1.Labels;

public record UpdateLabelRequest(
    string Id,
    string Name,
    string Description
);
