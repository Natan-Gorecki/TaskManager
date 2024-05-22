using FastEndpoints;
using TaskManager.Service.Database;
using TaskManager.Service.Database.Models;

namespace TaskManager.Service.Api.v1.Spaces;

[HttpPut("/spaces/{Id}")]
public class Update(IMapper _mapper, TaskManagerContext _context) : Endpoint<SpaceDTO, SpaceDTO>
{
    public override async Task HandleAsync(SpaceDTO request, CancellationToken cancellationToken)
    {
        var dbSpace = await _context.Spaces.FindAsync([request.Id], cancellationToken);
        if (dbSpace is null)
        {
            await SendNotFoundAsync(cancellationToken);
            return;
        }

        dbSpace = _mapper.Map(request, dbSpace);
        await _context.SaveChangesAsync(cancellationToken);

        var response = _mapper.Map<SpaceDTO>(dbSpace);
        await SendAsync(response, StatusCode.OK, cancellationToken);
    }
}
