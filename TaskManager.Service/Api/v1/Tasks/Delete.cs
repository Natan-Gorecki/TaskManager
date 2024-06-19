using FastEndpoints;
using TaskManager.Service.Database;

namespace TaskManager.Service.Api.v1.Tasks;

[HttpDelete("tasks/{Id}")]
public class Delete(TaskManagerContext _context) : Endpoint<DeleteTaskRequest, EmptyResponse>
{
    public override async Task HandleAsync(DeleteTaskRequest request, CancellationToken ct)
    {
        var dbTask = await _context.Tasks.FindAsync([request.Id], ct);
        if (dbTask is null)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        _context.Tasks.Remove(dbTask);
        await _context.SaveChangesAsync(ct);

        await SendNoContentAsync(ct);
    }
}
