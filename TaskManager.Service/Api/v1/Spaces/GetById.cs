using FastEndpoints;
using TaskManager.Service.Database;

namespace TaskManager.Service.Api.v1.Spaces;

[HttpGet("spaces/{Id}")]
public class GetById(IMapper _mapper, TaskManagerContext _context) : Endpoint<GetSpaceByIdRequest, SpaceDTO>
{
    public override async Task HandleAsync(GetSpaceByIdRequest request, CancellationToken cancellationToken)
    {
        var dbSpace = await _context.Spaces.FindAsync([request.Id], cancellationToken);
        if (dbSpace is null)
        {
            await SendNotFoundAsync(cancellationToken);
            return;
        }

        var response = _mapper.Map<SpaceDTO>(dbSpace);
        await SendAsync(response, StatusCode.OK, cancellationToken);
    }
}
