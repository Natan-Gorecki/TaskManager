using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using TaskManager.Service.Database;

namespace TaskManager.Service.Api.v1.TimeEntries;

[HttpPut("tasks/{TaskId}/time-entries/{Id}")]
public class Update(IMapper _mapper, TaskManagerContext _context) : Endpoint<TimeEntryDTO, TimeEntryDTO>
{
    public override async Task HandleAsync(TimeEntryDTO request, CancellationToken ct)
    {
        var dbTimeEntry = await _context.TimeEntries.FirstOrDefaultAsync(x => x.Id == request.Id && x.TaskId == request.TaskId, ct);
        if (dbTimeEntry is null)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        dbTimeEntry = _mapper.Map(request, dbTimeEntry);
        await _context.SaveChangesAsync(ct);

        var response = _mapper.Map<TimeEntryDTO>(dbTimeEntry);
        await SendAsync(response, StatusCode.OK, ct);
    }
}
