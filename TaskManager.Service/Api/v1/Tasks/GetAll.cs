using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using TaskManager.Service.Database;

namespace TaskManager.Service.Api.v1.Tasks;

[HttpGet("tasks")]
public class GetAll(IMapper _mapper, TaskManagerContext _context) : Endpoint<EmptyRequest, IEnumerable<TaskDTO>>
{
    public override async Task HandleAsync(EmptyRequest request, CancellationToken ct)
    {
        var dbTasks = await _context.Tasks.ToListAsync(ct);

        var response = _mapper.Map<List<TaskDTO>>(dbTasks);
        await SendAsync(response, StatusCode.OK, ct);
    }
}
