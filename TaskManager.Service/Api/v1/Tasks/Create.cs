using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using TaskManager.Service.Database;
using TaskManager.Service.Database.Models;

namespace TaskManager.Service.Api.v1.Tasks;

[HttpPost("tasks")]
public class Create(IMapper _mapper, TaskManagerContext _context) : Endpoint<CreateTaskRequest, TaskDTO>
{
    public override async Task HandleAsync(CreateTaskRequest request, CancellationToken ct)
    {
        var dbSpace = await _context.Spaces.FindAsync([request.SpaceId], ct);
        if (dbSpace is null)
        {
            ThrowError($"Space with '{request.SpaceId}' id doesn't exist.", StatusCode.BadRequest);
        }
        await ValidateParentTaskAsync(request, dbSpace, ct);

        var dbTask = _mapper.Map<DbTask>(request);
        dbTask.Id = await GenerateTaskId(dbSpace, ct);
        await _context.AddAsync(dbTask, ct);
        await _context.SaveChangesAsync(ct);

        var response = _mapper.Map<TaskDTO>(dbTask);
        await SendAsync(response, StatusCode.OK, ct);
    }

    private async Task ValidateParentTaskAsync(CreateTaskRequest request, DbSpace dbSpace, CancellationToken ct)
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

        if (parentTask.SpaceId != request.SpaceId)
        {
            ThrowError($"Parent task must be within the same space '{dbSpace.Key}'.", StatusCode.BadRequest);
        }
    }

    private async Task<string> GenerateTaskId(DbSpace dbSpace, CancellationToken ct)
    {
        var lastTaskId = await _context.Tasks
            .Where(x => x.SpaceId == dbSpace.Id)
            .OrderByDescending(x => x.Id)
            .Select(x => x.Id)
            .FirstOrDefaultAsync(ct);

        if (lastTaskId is null)
        {
            return $"{dbSpace.Key}-1";
        }

        var pattern = @"-(\d+)$";
        Match match = Regex.Match(lastTaskId, pattern);
        if (!match.Success)
        {
            ThrowError("Failed to generate id for a new task.", StatusCode.InternalServerError);
        }

        var taskNumber = int.Parse(match.Groups[1].Value);
        return $"{dbSpace.Key}-{taskNumber + 1}";
    }
}
