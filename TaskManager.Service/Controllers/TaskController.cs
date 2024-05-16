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
        var space = _context.Spaces.FirstOrDefault();

        var singleEpic = _context.Tasks.FirstOrDefault(x => x.Id == "IT-001");

        var epic = _context.Tasks.Where(x => x.Id == "IT-001")
            .Include(x => x.ChildTasks)
            .FirstOrDefault();

        var firstTask = _context.Tasks.Where(x => x.Id == "IT-003")
            .Include(x => x.ParentTask)
            .FirstOrDefault();

        return Ok("DONE");
    }
}
