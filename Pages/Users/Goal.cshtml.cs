using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;
using Microsoft.AspNetCore.Identity;

[Route("Goals")]
public class GoalController : Controller
{
    private readonly ConsttestContext _context;
    private readonly ILogger<GoalController> _logger;
    private readonly UserManager<IdentityUser> _userManager;

    public GoalController(ConsttestContext context, UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    [HttpGet("GetGoals")]
    public IActionResult GetGoals()
    {
        var userId = User.Identity?.Name; // Получение текущего пользователя
        var user = _context.Users
            .Include(u => u.Goals)
            .FirstOrDefault(u => u.Username == userId);

        if (user == null) return Unauthorized();

        // Получение целей
        var goals = user.Goals.Select(g => new
        {
            id = g.GoalId,
            title = g.Name,
            // start = g.CreateDate.HasValue
            //     ? g.CreateDate.Value.ToString("yyyy-MM-dd")
            //     : null,
            end = g.Deadline.HasValue
                ? g.Deadline.Value.ToString("yyyy-MM-dd")
                : null,
            description = g.Description,
            sphere = g.GoalSphere.HasValue ? _context.SpheresOfLives.FirstOrDefault(s => s.SphereId == g.GoalSphere)?.Name : "Не указана",
            status = g.Status,
            type = "goal"
        }).AsEnumerable();

        return Json(goals);
    }

  [HttpPost("AddGoal")]
public IActionResult AddGoal([FromBody] Goal model)
{
    var userId = User.Identity?.Name;
    var user = _context.Users.FirstOrDefault(u => u.Username == userId);

    if (user == null) return Unauthorized();

    var newGoal = new Goal
    {
        Name = model.Name,
        Description = model.Description,
        Deadline = model.Deadline,
        Status = false, // Новая цель по умолчанию не выполнена
        GoalSphere = model.GoalSphere, // Установка сферы жизни
        UserId = user.IdUser
    };

    _context.Goals.Add(newGoal);
    _context.SaveChanges();

    return Ok();
}


    [HttpPost("UpdateGoalStatus")]
    public IActionResult UpdateGoalStatus(int goalId, bool status)
    {
        var userId = User.Identity?.Name;
        var user = _context.Users.FirstOrDefault(u => u.Username == userId);

        if (user == null) return Unauthorized();

        var goalToUpdate = _context.Goals.FirstOrDefault(g => g.GoalId == goalId);
        if (goalToUpdate == null) return NotFound();

        goalToUpdate.Status = status;
        _context.SaveChanges();

        return Ok();
    }

    [HttpPost("DeleteGoal")]
    public IActionResult DeleteGoal([FromBody] int goalId)
    {
        var userId = User.Identity?.Name;
        var user = _context.Users.FirstOrDefault(u => u.Username == userId);

        if (user == null) return Unauthorized();

        var goalToDelete = _context.Goals.FirstOrDefault(g => g.GoalId == goalId);
        if (goalToDelete == null) return NotFound();

        // Удаляем цель
        _context.Goals.Remove(goalToDelete);
        _context.SaveChanges();

        return Ok();
    }
}
