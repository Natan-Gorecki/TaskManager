using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using TaskManager.Service.Database;

namespace TaskManager.Service.Api.v1.TimeEntries;

[HttpDelete("tasks/{TaskId}/time-entries/{Id}")]
public class Delete(TaskManagerContext _context) : Endpoint<DeleteTimeEntryRequest, EmptyResponse>
{
    public override async Task HandleAsync(DeleteTimeEntryRequest request, CancellationToken ct)
    {
        var dbTimeEntry = await _context.TimeEntries.FirstOrDefaultAsync(x => x.Id == request.Id && x.TaskId == request.TaskId, ct);
        if (dbTimeEntry is null)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        _context.TimeEntries.Remove(dbTimeEntry);
        await _context.SaveChangesAsync(ct);

        await SendNoContentAsync(ct);
    }
}
