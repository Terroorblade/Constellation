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
public int? FilterGoalMonth { get; set; }
    public int? FilterGoalYear { get; set; }
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
[HttpGet("GetGoalDetails/{goalId}")]
public IActionResult GetGoalDetails(int goalId)
{
    var userId = User.Identity?.Name;
    var user = _context.Users.FirstOrDefault(u => u.Username == userId);

    if (user == null) return Unauthorized();

    // var goal = _context.Goals
    //     .Include(g => g.GoalSphere) // Включение информации о сфере жизни, если она связана
    //     .FirstOrDefault(g => g.GoalId == goalId && g.UserId == user.IdUser);
var goal = _context.Goals
    .Include(g => g.GoalSphereNavigation)
    .Include(g => g.Habits.Where(h => h.Status==false))
    .FirstOrDefault(g => g.GoalId == goalId);


    if (goal == null) return NotFound();

    // Формируем ответ
    var goalDetails = new
    {
        id = goal.GoalId,
        name = goal.Name,
        description = goal.Description,
        deadline = goal.Deadline?.ToString("yyyy-MM-dd"),
        sphere = goal.GoalSphere?.ToString() ?? "Не указана",
        status = goal.Status
    };

    return Json(goalDetails);
}

// Модель для запроса
public class CreateHabitRequest
{
    public int goalId { get; set; }
    public string habitName { get; set; }
    public string habitDescription { get; set; }
    public int habitFrequency { get; set; }
    public int UserId { get; set; }
}
[HttpPost("CreateHabitForGoal")]

public IActionResult CreateHabitForGoal([FromBody]CreateHabitRequest request)
{
     var userId = User.Identity?.Name;
    var user = _context.Users.FirstOrDefault(u => u.Username == userId);

    if (user == null) return Unauthorized();

    var habitsCount = _context.Habits.Count(h => h.GoalHabitNavigation.UserId == user.IdUser && !h.Status);
    if (habitsCount >= 3)
    {
        return BadRequest("Вы не можете создать более 3 активных привычек одновременно.");
    }

    var goal = _context.Goals.Include(g => g.Habits).FirstOrDefault(g => g.GoalId == request.goalId && g.UserId == user.IdUser);
    if (goal == null) return NotFound();

    // Создание привычки по цели
   var habit = new Habit
    {
        Name = request.habitName,
        Description = request.habitDescription,
        Frequency = request.habitFrequency,
        Status = false,
        GoalHabit = request.goalId,
          UserId = user.IdUser
    };

    _context.Habits.Add(habit);
    _context.SaveChanges();

     // Добавляем привычку в расписание в зависимости от указанной частоты
    for (int i = 0; i < 28; i++)
    {
        if (request.habitFrequency == 0 || request.habitFrequency < 0 || i % request.habitFrequency == 0)
        {
            var scheduleDate = DateOnly.FromDateTime(DateTime.Now.AddDays(i));
            var schedule = _context.DailySchedules.FirstOrDefault(ds => ds.UserSchedule == user.IdUser && ds.ScheduleData == scheduleDate);

            if (schedule == null)
            {
                schedule = new DailySchedule
                {
                    ScheduleData = scheduleDate,
                    UserSchedule = user.IdUser
                };
                _context.DailySchedules.Add(schedule);
                _context.SaveChanges();
            }

            var habitOfTheDay = new HabitOfTheDay
            {
                HabitDay = habit.HabitId,
                ScheduleDay = schedule.ScheduleId,
                Status = false
            };

            _context.HabitOfTheDays.Add(habitOfTheDay);
        }
    }

    _context.SaveChanges();

    return Ok();
}

}
public class GoalViewModel
{
    public int GoalId { get; set; }
    public string Name { get; set; }
    public DateTime Date { get; set; }
    public string Deadline { get; set; }
    public string Description { get; set; }
    public string Sphere { get; set; }
    public string Status { get; set; }
}

// Модель страницы целей
public class GoalsPageViewModel
{
    public int? FilterGoalMonth { get; set; }
    public int? FilterGoalYear { get; set; }
    public List<GoalViewModel> Goals { get; set; }
}