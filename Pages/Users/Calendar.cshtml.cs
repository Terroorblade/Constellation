using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

[Route("Calendar")]
public class CalendarController : Controller
{
    private readonly ConsttestContext _context;
    private readonly ILogger<CalendarController> _logger;
    private readonly UserManager<IdentityUser> _userManager;

    public CalendarController(ConsttestContext context, UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    [HttpGet("GetEvents")]
    public IActionResult GetEvents()
    {
        
       var userId = User.Identity?.Name; // Получение текущего пользователя
        var user = _context.Users
        .Include(u => u.DailySchedules)
        .ThenInclude(ds => ds.Events)
        .Include(u => u.DailySchedules)
        .ThenInclude(ds => ds.HabitOfTheDays)
        .FirstOrDefault(u => u.Username == userId);


        // var user = _context.Users.FirstOrDefault(u => u.Username == userId);

        if (user == null) return Unauthorized();

        // Получение событий
      var events = user.DailySchedules
        .SelectMany(ds => ds.Events)
        .Select(e => new
        {
            id = e.EventId,
            title = e.Name,
            start = e.EventDate.HasValue 
    ? e.EventDate.Value.ToDateTime(TimeOnly.MinValue).ToString("yyyy-MM-dd") 
    : null,
            description = e.Description,
            priority = e.Priority,
            status = e.Status,
            type = "event"
        }).AsEnumerable();

        // Получение привычек
    var habits = user.DailySchedules
        .SelectMany(ds => ds.HabitOfTheDays)
        .Select(h => new
        {
            id = h.HabitDayId,
            title = _context.Habits
                        .Where(hb => hb.HabitId == h.HabitDay)
                        .Select(hb => hb.Name)
                        .FirstOrDefault() ?? "",
           start = h.ScheduleDayNavigation.ScheduleData.HasValue 
    ? h.ScheduleDayNavigation.ScheduleData.Value.ToDateTime(TimeOnly.MinValue).ToString("yyyy-MM-dd") 
    : null,
            description = "Привычка",
            priority = "N/A",
            status = h.Status,
            type = "habit"
        }).AsEnumerable();


   var combined = events.Concat(habits);
        return Json(combined);
    }

    [HttpPost("AddEvent")]
public IActionResult AddEvent([FromBody] Event model)
{
    var userId = User.Identity?.Name;
    var user = _context.Users.FirstOrDefault(u => u.Username == userId);

    if (user == null) return Unauthorized();

    var schedule = _context.DailySchedules
        .FirstOrDefault(ds => ds.UserSchedule == user.IdUser && ds.ScheduleData == model.EventDate);

    if (schedule == null)
    {
        schedule = new DailySchedule
        {
            ScheduleData = model.EventDate,
            UserSchedule = user.IdUser
        };
        _context.DailySchedules.Add(schedule);
        _context.SaveChanges();
    }

    var newEvent = new Event
    {
        Name = model.Name,
        Description = model.Description,
        EventDate = model.EventDate,
        Priority = model.Priority,
        EventSchedule = schedule.ScheduleId
    };
    _context.Events.Add(newEvent);
    _context.SaveChanges();

    return Ok();
}
[HttpPost("UpdateEventStatus")]
public IActionResult UpdateEventStatus(int eventId, bool status)
{
    var userId = User.Identity?.Name;
    var user = _context.Users.FirstOrDefault(u => u.Username == userId);

    if (user == null) return Unauthorized();

    var eventToUpdate = _context.Events.FirstOrDefault(e => e.EventId == eventId);
    if (eventToUpdate == null) return NotFound();

    eventToUpdate.Status = status;
    _context.SaveChanges();

    return Ok();
}
[HttpPost("DeleteEvent")]
public IActionResult DeleteEvent([FromBody] int eventId)
{
    var userId = User.Identity?.Name;
    var user = _context.Users.FirstOrDefault(u => u.Username == userId);

    if (user == null) return Unauthorized();

    var eventToDelete = _context.Events.FirstOrDefault(e => e.EventId == eventId);
    if (eventToDelete == null) return NotFound();

    // Удаляем событие
    _context.Events.Remove(eventToDelete);
    _context.SaveChanges();

    return Ok();
}
public class UpdateHabitStatusRequest
{
    public int HabitId { get; set; }
    public DateOnly SelectedDate { get; set; }
    public bool Status { get; set; }
}

[HttpPost("UpdateHabitStatus")]
public IActionResult UpdateHabitStatus([FromBody] UpdateHabitStatusRequest request)
{
    var userId = User.Identity?.Name;
    var user = _context.Users.FirstOrDefault(u => u.Username == userId);

    if (user == null) return Unauthorized();
 var selectedDate = request.SelectedDate;
    var habitDayToUpdate = _context.HabitOfTheDays
        .Include(h => h.ScheduleDayNavigation)
        .FirstOrDefault(h => h.HabitDayId == request.HabitId && h.ScheduleDayNavigation.ScheduleData == selectedDate);

    if (habitDayToUpdate == null) return NotFound();

    habitDayToUpdate.Status = request.Status;
    _context.SaveChanges();

    return Ok();
}
[HttpPost("DeleteHabit")]
public IActionResult DeleteHabit(int habitId, DateOnly scheduleDate)
{
    var userId = User.Identity?.Name;
    var user = _context.Users.FirstOrDefault(u => u.Username == userId);

    if (user == null) return Unauthorized();

    var habitDayToDelete = _context.HabitOfTheDays
        .Include(h => h.ScheduleDayNavigation)
        .FirstOrDefault(h => h.HabitDay == habitId && h.ScheduleDayNavigation.ScheduleData == scheduleDate);

    if (habitDayToDelete == null) return NotFound();

    _context.HabitOfTheDays.Remove(habitDayToDelete);
    _context.SaveChanges();

    return Ok();
}
}