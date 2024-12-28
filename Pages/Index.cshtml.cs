using Microsoft.AspNetCore.Mvc.RazorPages; 
using Microsoft.EntityFrameworkCore; 
using КурсоваяБД5.Models; 
using System.Collections.Generic; 
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Diagnostics;

namespace КурсоваяБД5.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly FivesemestercswrkContext _context; 

    public IndexModel(FivesemestercswrkContext context)
    {
        _context = context;
    }
//     public List<DailySchedule> DailySchedules { get; set; } = new List<DailySchedule>(); 
//     public List<Event> Events { get; set; } = new List<Event>(); 
//     public List<Goal> Goals { get; set; } = new List<Goal>(); 
//     public List<Habit> Habits { get; set; } = new List<Habit>(); 
//     public List<HabitOfTheDay> HabitOfTheDays { get; set; } = new List<HabitOfTheDay>(); 
//     public List<SpheresOfLife> SpheresOfLives { get; set; } = new List<SpheresOfLife>(); 
//     public List<User> Users { get; set; } = new List<User>(); 
//     public List<UserSphereSatisfaction> UserSphereSatisfactions { get; set; } = new List<UserSphereSatisfaction>(); 


//       public async Task OnGetAsync() 
//     { 
//         DailySchedules = await _context.DailySchedules.ToListAsync();
//         Users = await _context.Users.ToListAsync();
//         Events = await _context.Events
//         .Include(e => e.EventScheduleNavigation)
//         .ToListAsync();
//  //данные из таблицы Event 
//         Goals = await _context.Goals.ToListAsync(); 
//         Habits = await _context.Habits.ToListAsync();
//         HabitOfTheDays = await _context.HabitOfTheDays.ToListAsync();
    
//         UserSphereSatisfactions =   await _context.UserSphereSatisfactions.ToListAsync();
//         SpheresOfLives = await _context.SpheresOfLives.ToListAsync();
//     } 
// [BindProperty] public User NewUser{get;set;} = new User();
//      public async Task<IActionResult> OnGetAsync()
//     {
//         // Логика для GET-запроса
//         await Task.CompletedTask;
//         return Page();
//     }
//     public async Task<IActionResult> OnPostAsync()
//     {
//         if (!ModelState.IsValid)
//         {
//             return Page();
//         }

        
//         if (NewUser.Birthday.HasValue)
//         {
//             var today = DateTime.Today;
//             var user_birthday = NewUser.Birthday.Value.ToDateTime(TimeOnly.MinValue);
//             var user_age = today.Year-user_birthday.Year;

//             if(user_birthday>today.AddYears(-user_age))
//             {
//                 user_age--;
//             }
//             if(user_age<7)
//             {
//                 ModelState.AddModelError("NewUser.Birthday", "Регистрация возможна только для пользователей старше 7 лет.");
//                 return Page();
//             }
//         }

//         NewUser.PasswordHash = BCrypt.Net.BCrypt.HashPassword(NewUser.PasswordHash);
//         // Добавляем нового пользователя в контекст данных
//         _context.Users.Add(NewUser);
//         await _context.SaveChangesAsync();  // Сохраняем изменения в базе данных
//         TempData["SuccessMessage"] = "Регистрация прошла успешно!";
//         return RedirectToPage("./Index"); // Перенаправляем на другую страницу после сохранения
//     }



    public void OnGet()
    {

    }
}
