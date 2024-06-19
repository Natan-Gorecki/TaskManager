using FastEndpoints;
using TaskManager.Service.Database;

namespace TaskManager.Service.Api.v1.Tasks;

[HttpGet("tasks/{Id}")]
public class GetById(IMapper _mapper, TaskManagerContext _context) : Endpoint<GetTaskByIdRequest, TaskDTO>
{
    public override async Task HandleAsync(GetTaskByIdRequest request, CancellationToken ct)
    {
        var dbTask = await _context.Tasks.FindAsync([request.Id], ct);
        if (dbTask is null)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        var response = _mapper.Map<TaskDTO>(dbTask);
        await SendAsync(response, StatusCode.OK, ct);
    }
}
