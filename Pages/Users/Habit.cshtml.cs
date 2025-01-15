using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;
using Microsoft.AspNetCore.Identity;

[Route("Habits")]
public class HabitController : Controller
{
    private readonly ConsttestContext _context;
    private readonly UserManager<IdentityUser> _userManager;

    public HabitController(ConsttestContext context, UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

[HttpGet("GetHabits")]
public IActionResult GetHabits()
{
    var userId = User.Identity?.Name;
    var user = _context.Users
        .Include(u => u.Habits)  // Include the habits directly
        .FirstOrDefault(u => u.Username == userId);

    if (user == null) return Unauthorized();

    // Fetch the habits, including their status and other details
    var habits = user.Habits.Select(h => new
    {
        id = h.HabitId,
        name = h.Name,
        status = h.Status,
        description = h.Description,
        frequency = h.Frequency,
        type = "habit"
    }).ToList();  // Ensure it's evaluated as a list

    return Json(habits);
}

    // // Add a new habit
    // [HttpPost("AddHabit")]
    // public IActionResult AddHabit([FromBody] CreateHabitRequest request)
    // {
    //     var userId = User.Identity?.Name;
    //     var user = _context.Users.FirstOrDefault(u => u.Username == userId);

    //     if (user == null) return Unauthorized();

    //     // Check the number of active habits
    //     var activeHabitsCount = _context.Habits.Count(h => h.UserId == user.IdUser && !h.Status);
    //     if (activeHabitsCount >= 3)
    //     {
    //         return BadRequest("You cannot create more than 3 active habits simultaneously.");
    //     }

    //     // Create the habit
    //     var habit = new Habit
    //     {
    //         Name = request.habitName,
    //         Description = request.habitDescription,
    //         Frequency = request.habitFrequency,
    //         Status = false, // New habits are inactive by default
    //         UserId = user.IdUser
    //     };

    //     _context.Habits.Add(habit);
    //     _context.SaveChanges();

    //     return Ok();
    // }

    // // Update habit status (completed or not)
    // [HttpPost("UpdateHabitStatus")]
    // public IActionResult UpdateHabitStatus(int habitId, bool status)
    // {
    //     var userId = User.Identity?.Name;
    //     var user = _context.Users.FirstOrDefault(u => u.Username == userId);

    //     if (user == null) return Unauthorized();

    //     var habitToUpdate = _context.Habits.FirstOrDefault(h => h.HabitId == habitId && h.UserId == user.IdUser);
    //     if (habitToUpdate == null) return NotFound();

    //     habitToUpdate.Status = status;
    //     _context.SaveChanges();

    //     return Ok();
    // }

    // Delete a habit and all associated calendar entries
 [HttpPost("DeleteHabit")]
public IActionResult DeleteHabit([FromBody] int habitId)
{
    var userId = User.Identity?.Name;
    var user = _context.Users.FirstOrDefault(u => u.Username == userId);

    if (user == null) return Unauthorized();

    var habitToDelete = _context.Habits.FirstOrDefault(h => h.HabitId == habitId);
    if (habitToDelete == null) return NotFound();

    // Удаляем связанные записи HabitOfTheDay
    var habitOfTheDayEntries = _context.HabitOfTheDays.Where(h => h.HabitDay == habitId);
    _context.HabitOfTheDays.RemoveRange(habitOfTheDayEntries);

    // Удаляем саму привычку
    _context.Habits.Remove(habitToDelete);
    _context.SaveChanges();

    return Ok();
}

// Update habit status (completed or not)
[HttpPost("UpdateHabitStatus")]
public IActionResult UpdateHabitStatus([FromBody] UpdateHabitStatusRequest request)
{
    try
    {
        var userId = User.Identity?.Name;
        var user = _context.Users.FirstOrDefault(u => u.Username == userId);

        if (user == null) return Unauthorized();

        // Find the habit to update
        var habit = _context.Habits.FirstOrDefault(h => h.HabitId == request.HabitId && h.UserId == user.IdUser);
        if (habit == null)
        {
            return NotFound("Habit not found");
        }

        // Update the habit status
        habit.Status = request.Status;
        _context.SaveChanges();

        // If the status is true (completed), remove the corresponding DailySchedule records
     // If the status is true (completed), remove the corresponding DailySchedule records
if (request.Status)
{
   // First, fetch the related habit of the day entries
   var habitOfTheDayEntries = _context.HabitOfTheDays
                                       .Where(h => h.HabitDay == request.HabitId)
                                       .ToList(); // This fetches the habit of the day entries

   // Then, extract the schedule ids that need to be deleted
   var scheduleIdsToDelete = habitOfTheDayEntries.Select(h => h.ScheduleDay).ToList();

   // Now, fetch the DailySchedules to delete based on those schedule IDs
   var recordsToDelete = _context.DailySchedules
       .Where(ds => scheduleIdsToDelete.Contains(ds.ScheduleId))
       .ToList(); // Now this query can be translated to SQL

   // Remove the related DailySchedule records
   _context.DailySchedules.RemoveRange(recordsToDelete);
   _context.SaveChanges();
}


        return Ok("Habit status updated and associated records removed if completed.");
    }
    catch (Exception ex)
    {
        return StatusCode(500, $"Error during update: {ex.Message}");
    }
}


public class CreateHabitRequest
{
    public string habitName { get; set; }
    public string habitDescription { get; set; }
    public int habitFrequency { get; set; }  // Frequency in days
}
public class UpdateHabitStatusRequest
{
    public int HabitId { get; set; }
    public bool Status { get; set; }
    public DateTime? SelectedDate { get; set; }
}

}