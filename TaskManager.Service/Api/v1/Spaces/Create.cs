using FastEndpoints;
using TaskManager.Service.Database;
using TaskManager.Service.Database.Models;

namespace TaskManager.Service.Api.v1.Spaces;

[HttpPost("spaces")]
public class Create(IMapper _mapper, TaskManagerContext _context) : Endpoint<CreateSpaceRequest, SpaceDTO>
{
    public override async Task HandleAsync(CreateSpaceRequest request, CancellationToken cancellationToken)
    {
        if (_context.Spaces.Any(x => x.Key == request.Key))
        {
            ThrowError($"Space with '{request.Key}' key exists.", StatusCode.Conflict);
        }

        var dbSpace = _mapper.Map<DbSpace>(request);
        dbSpace.Id = Guid.NewGuid().ToString();
        await _context.AddAsync(dbSpace, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        var response = _mapper.Map<SpaceDTO>(dbSpace);
        await SendAsync(response, StatusCode.Created, cancellationToken);
    }
}
