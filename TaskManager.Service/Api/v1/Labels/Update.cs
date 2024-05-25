using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using TaskManager.Service.Database;

namespace TaskManager.Service.Api.v1.Labels;

[HttpPut("labels/{Id}")]
public class Update(IMapper _mapper, TaskManagerContext _context) : Endpoint<UpdateLabelRequest, LabelDTO>
{
    public override async Task HandleAsync(UpdateLabelRequest request, CancellationToken ct)
    {
        var dbLabel = await _context.Labels.Where(x => x.Id == request.Id)
            .Include(x => x.Space)
            .FirstOrDefaultAsync(ct);

        if (dbLabel is null)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        if (dbLabel.Name != request.Name && _context.Labels.Any(x => x.Name == request.Name && x.SpaceId == dbLabel.SpaceId))
        {
            ThrowError($"Label with '{request.Name}' name exists for space '{dbLabel.Space!.Key}'.", StatusCode.Conflict);
        }

        dbLabel = _mapper.Map(request, dbLabel);
        await _context.SaveChangesAsync(ct);

        var response = _mapper.Map<LabelDTO>(dbLabel);
        await SendAsync(response, StatusCode.OK, ct);
    }
}
