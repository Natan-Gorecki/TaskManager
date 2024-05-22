using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using TaskManager.Service.Database;

namespace TaskManager.Service.Api.v1.Spaces;

[HttpGet("/spaces")]
public class List(IMapper _mapper, TaskManagerContext _context) : EndpointWithoutRequest<IEnumerable<SpaceDTO>>
{
    public override async Task HandleAsync(CancellationToken cancellationToken)
    {
        var dbSpaces = await _context.Spaces.ToListAsync(cancellationToken);
        var spaces = _mapper.Map<List<SpaceDTO>>(dbSpaces);
        Response = spaces;
    }
}
