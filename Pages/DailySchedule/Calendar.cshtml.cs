// using Microsoft.AspNetCore.Mvc;
// using System;
// using System.Linq;
// using WebApplication1.Models;

// [Route("Calendar")]
// public class CalendarController : Controller
// {
//     private readonly ConsttestContext _context;

//     public CalendarController(ConsttestContext context)
//     {
//         _context = context;
//     }

//     [HttpGet("GetEvents")]
//     public IActionResult GetEvents()
//     {
//         var userId = User.Identity?.Name; // Получение текущего пользователя
//         var user = _context.Users.FirstOrDefault(u => u.Username == userId);

//         if (user == null) return Unauthorized();

//         // Получение событий
//         var events = _context.Events
//             .Where(e => e.EventSchedule == user.IdUser)
//             .Select(e => new
//             {
//                 id = e.EventId,
//                 title = e.Name,
//                 start = e.EventDate.ToString("yyyy-MM-dd"),
//                 description = e.Description
//             });

//         // Получение привычек
//         var habits = _context.HabitOfTheDays
//             .Where(h => h.ScheduleDay == user.IdUser)
//             .Select(h => new
//             {
//                 id = h.HabitDayId,
//                 title = _context.Habits.FirstOrDefault(hb => hb.HabitId == h.HabitDay)?.Name,
//                 start = h.HabitDay.ToString("yyyy-MM-dd"),
//                 description = "Привычка"
//             });

//         var combined = events.Concat(habits);
//         return Json(combined);
//     }
// }
