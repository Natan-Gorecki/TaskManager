using FastEndpoints;
using TaskManager.Service.Database;
using TaskManager.Service.Database.Models;

namespace TaskManager.Service.Api.v1.Spaces;

[HttpPost("/spaces")]
public class Create(IMapper _mapper, TaskManagerContext _context) : Endpoint<CreateSpaceRequest, SpaceDTO>
{
    public override async Task HandleAsync(CreateSpaceRequest request, CancellationToken cancellationToken)
    {
        var dbSpace = _mapper.Map<DbSpace>(request);
        dbSpace.Id = Guid.NewGuid().ToString();
        await _context.AddAsync(dbSpace, cancellationToken);

        var response = _mapper.Map<SpaceDTO>(dbSpace);
        await SendAsync(response, StatusCode.Created, cancellationToken);
    }
}
