using FastEndpoints;
using TaskManager.Service.Database;

namespace TaskManager.Service.Api.v1.Labels;

[HttpGet("labels/{Id}")]
public class GetById(IMapper _mapper, TaskManagerContext _context) : Endpoint<GetLabelByIdRequest, LabelDTO>
{
    public override async Task HandleAsync(GetLabelByIdRequest request, CancellationToken ct)
    {
        var dbLabel = await _context.Labels.FindAsync([request.Id], ct);
        if (dbLabel == null)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        var response = _mapper.Map<LabelDTO>(dbLabel);
        await SendAsync(response, StatusCode.OK, ct);
    }
}
