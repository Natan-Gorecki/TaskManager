using FastEndpoints;
using TaskManager.Service.Database;
using TaskManager.Service.Database.Models;

namespace TaskManager.Service.Api.v1.TimeEntries;

[HttpPost("tasks/{TaskId}/time-entries")]
public class Create(IMapper _mapper, TaskManagerContext _context) : Endpoint<CreateTimeEntryRequest, TimeEntryDTO>
{
    public override async Task HandleAsync(CreateTimeEntryRequest request, CancellationToken ct)
    {
        var dbTask = await _context.Tasks.FindAsync([request.TaskId], ct);
        if (dbTask is null)
        {
            ThrowError($"Task with '{request.TaskId}' doesn't exist.", StatusCode.BadRequest);
        }

        var dbTimeEntry = _mapper.Map<DbTimeEntry>(request);
        dbTimeEntry.Id = Guid.NewGuid().ToString();
        await _context.AddAsync(dbTimeEntry, ct);
        await _context.SaveChangesAsync(ct);

        var response = _mapper.Map<TimeEntryDTO>(dbTimeEntry);
        await SendAsync(response, StatusCode.Created, ct);
    }
}
