using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using TaskManager.Service.Database;
using TaskManager.Service.Database.Models;

namespace TaskManager.Service.Api.v1.Tasks;

[HttpPut("tasks/{Id}")]
public class Update(IMapper _mapper, TaskManagerContext _context) : Endpoint<UpdateTaskRequest, TaskDTO>
{
    public override async Task HandleAsync(UpdateTaskRequest request, CancellationToken ct)
    {
        var dbTask = await _context.Tasks
            .Include(x => x.Space)
            .FirstOrDefaultAsync(x => x.Id == request.Id, ct);
        if (dbTask is null)
        {
            await SendNotFoundAsync(ct);
            return;
        }
        await ValidateParentTaskAsync(request, dbTask.Space!, ct);

        dbTask = _mapper.Map(request, dbTask);
        await _context.SaveChangesAsync(ct);

        var response = _mapper.Map<TaskDTO>(dbTask);
        await SendAsync(response, StatusCode.OK, ct);
    }

    private async Task ValidateParentTaskAsync(UpdateTaskRequest request, DbSpace dbSpace, CancellationToken ct)
    {
        if (request.ParentTaskId is null)
        {
            return;
        }

        var parentTask = await _context.Tasks.FindAsync([request.ParentTaskId], ct);
        if (parentTask is null)
        {
            ThrowError($"Task with '{request.ParentTaskId}' id doesn't exist.", StatusCode.BadRequest);
        }

        if (parentTask.SpaceId != dbSpace.Id)
        {
            ThrowError($"Parent task must be within the same space '{dbSpace.Key}'.", StatusCode.BadRequest);
        }
    }
}
