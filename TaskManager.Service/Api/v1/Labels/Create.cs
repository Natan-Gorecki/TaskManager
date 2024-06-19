using FastEndpoints;
using TaskManager.Service.Database;
using TaskManager.Service.Database.Models;

namespace TaskManager.Service.Api.v1.Labels;

[HttpPost("labels")]
public class Create(IMapper _mapper, TaskManagerContext _context) : Endpoint<CreateLabelRequest, LabelDTO>
{
    public override async Task HandleAsync(CreateLabelRequest request, CancellationToken ct)
    {
        var dbSpace = await _context.Spaces.FindAsync([request.SpaceId], ct);
        if (dbSpace is null)
        {
            ThrowError($"Space with '{request.SpaceId}' id doesn't exist.", StatusCode.BadRequest);
        }

        if (_context.Labels.Any(x => x.Name == request.Name && x.SpaceId == request.SpaceId))
        {
            ThrowError($"Label with '{request.Name}' name exists for space '{dbSpace.Key}'.", StatusCode.Conflict);
        }

        var dbLabel = _mapper.Map<DbLabel>(request);
        dbLabel.Id = Guid.NewGuid().ToString();
        await _context.AddAsync(dbLabel, ct);
        await _context.SaveChangesAsync(ct);

        var response = _mapper.Map<LabelDTO>(dbLabel);
        await SendAsync(response, StatusCode.Created, ct);
    }
}
