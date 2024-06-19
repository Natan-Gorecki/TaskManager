using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using TaskManager.Service.Database;

namespace TaskManager.Service.Api.v1.TimeEntries;

[HttpGet("tasks/{TaskId}/time-entries")]
public class GetAllForTask(IMapper _mapper, TaskManagerContext _context) : Endpoint<GetAllForTaskRequest, IEnumerable<TimeEntryDTO>>
{
    public override async Task HandleAsync(GetAllForTaskRequest request, CancellationToken ct)
    {
        var dbTimeEntries = await _context.TimeEntries.Where(x => x.TaskId == request.TaskId).ToListAsync(ct);

        var response = _mapper.Map<List<TimeEntryDTO>>(dbTimeEntries);
        await SendAsync(response, StatusCode.OK, ct);
    }
}
