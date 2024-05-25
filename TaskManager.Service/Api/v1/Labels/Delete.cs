using FastEndpoints;
using TaskManager.Service.Database;

namespace TaskManager.Service.Api.v1.Labels;

[HttpDelete("labels/{Id}")]
public class Delete(TaskManagerContext _context) : Endpoint<DeleteLabelRequest, EmptyResponse>
{
    public override async Task HandleAsync(DeleteLabelRequest request, CancellationToken ct)
    {
        var dbLabel = await _context.Labels.FindAsync([request.Id], ct);
        if (dbLabel is null)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        _context.Labels.Remove(dbLabel);
        await _context.SaveChangesAsync(ct);

        await SendNoContentAsync(ct);
    }
}
