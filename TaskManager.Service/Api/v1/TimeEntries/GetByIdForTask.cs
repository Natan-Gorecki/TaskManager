using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using TaskManager.Service.Database;

namespace TaskManager.Service.Api.v1.TimeEntries;

[HttpGet("tasks/{TaskId}/time-entries/{Id}")]
public class GetByIdForTask(IMapper _mapper, TaskManagerContext _context) : Endpoint<GetByIdForTaskRequest, TimeEntryDTO>
{
    public override async Task HandleAsync(GetByIdForTaskRequest request, CancellationToken ct)
    {
        var dbTimeEntry = await _context.TimeEntries.FirstOrDefaultAsync(x => x.Id == request.Id && x.TaskId == request.TaskId, ct);
        if (dbTimeEntry is null)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        var response = _mapper.Map<TimeEntryDTO>(dbTimeEntry);
        await SendAsync(response, StatusCode.OK, ct);
    }
}
