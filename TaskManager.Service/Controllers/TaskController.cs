using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManager.Service.Database;
using TaskManager.Service.Database.Models;

namespace TaskManager.Service.Controllers;

[Route("tasks")]
[ApiController]
public class TaskController : ControllerBase
{
    private readonly TaskManagerContext _context;

    public TaskController(TaskManagerContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult GetTasks()
    {
        var space = _context.Spaces.Where(x => x.Id == "2f03707c-fbc1-4c98-9caf-d2a648f315b8")
            .Include(x => x.Tasks)
                .ThenInclude(x => x.ChildTasks)
            .FirstOrDefault();

        return Ok("DONE");
    }
}
