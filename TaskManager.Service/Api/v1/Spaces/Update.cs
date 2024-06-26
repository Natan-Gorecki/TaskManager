﻿using FastEndpoints;
using TaskManager.Service.Database;

namespace TaskManager.Service.Api.v1.Spaces;

[HttpPut("spaces/{Id}")]
public class Update(IMapper _mapper, TaskManagerContext _context) : Endpoint<SpaceDTO, SpaceDTO>
{
    public override async Task HandleAsync(SpaceDTO request, CancellationToken ct)
    {
        var dbSpace = await _context.Spaces.FindAsync([request.Id], ct);
        if (dbSpace is null)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        if (dbSpace.Key != request.Key && _context.Spaces.Any(x => x.Key == request.Key))
        {
            ThrowError($"Space with '{request.Key}' key exists.", StatusCode.Conflict);
        }

        dbSpace = _mapper.Map(request, dbSpace);
        await _context.SaveChangesAsync(ct);

        var response = _mapper.Map<SpaceDTO>(dbSpace);
        await SendAsync(response, StatusCode.OK, ct);
    }
}
