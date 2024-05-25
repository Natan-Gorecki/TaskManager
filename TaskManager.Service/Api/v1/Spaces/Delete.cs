using FastEndpoints;
using TaskManager.Service.Database;

namespace TaskManager.Service.Api.v1.Spaces;

[HttpDelete("spaces/{Id}")]
public class Delete(TaskManagerContext _context) : Endpoint<DeleteSpaceRequest, EmptyResponse>
{
    public override async Task HandleAsync(DeleteSpaceRequest request, CancellationToken ct)
    {
        var dbSpace = await _context.Spaces.FindAsync([request.Id], ct);
        if (dbSpace is null)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        _context.Spaces.Remove(dbSpace);
        await _context.SaveChangesAsync(ct);

        await SendNoContentAsync(ct);
    }
}
