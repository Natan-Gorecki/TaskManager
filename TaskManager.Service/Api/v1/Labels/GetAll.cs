using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using TaskManager.Service.Database;
using TaskManager.Service.Database.Models;

namespace TaskManager.Service.Api.v1.Labels;

[HttpGet("labels")]
public class GetAll(IMapper _mapper, TaskManagerContext _context) : Endpoint<GetAllLabelsRequest, IEnumerable<LabelDTO>>
{
    public override async Task HandleAsync(GetAllLabelsRequest request, CancellationToken ct)
    {
        List<DbLabel>? dbLabels = await GetDbLabelsAsync(request, ct);

        var response = _mapper.Map<List<LabelDTO>>(dbLabels);
        await SendAsync(response, StatusCode.OK, ct);
    }

    private Task<List<DbLabel>> GetDbLabelsAsync(GetAllLabelsRequest request, CancellationToken ct)
    {
        if (request.SpaceId != null)
        {
            return _context.Labels.Where(x => x.SpaceId == request.SpaceId).ToListAsync(ct);
        }
        return _context.Labels.ToListAsync(ct);
    }
}
