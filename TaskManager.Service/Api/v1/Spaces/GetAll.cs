﻿using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using TaskManager.Service.Database;

namespace TaskManager.Service.Api.v1.Spaces;

[HttpGet("spaces")]
public class GetAll(IMapper _mapper, TaskManagerContext _context) : Endpoint<EmptyRequest, IEnumerable<SpaceDTO>>
{
    public override async Task HandleAsync(EmptyRequest request, CancellationToken ct)
    {
        var dbSpaces = await _context.Spaces.ToListAsync(ct);

        var response = _mapper.Map<List<SpaceDTO>>(dbSpaces);
        await SendAsync(response, StatusCode.OK, ct);
    }
}
