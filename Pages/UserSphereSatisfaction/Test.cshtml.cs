
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
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace WebApplication1.Pages.Users
{
    [Authorize(Roles = "admin,user")]
    public class TestingModel : PageModel
    {
         private readonly ConsttestContext _context;
    private readonly UserManager<IdentityUser> _userManager;

    public TestingModel(ConsttestContext context, UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    [BindProperty]
    public TestViewModel TestModel { get; set; } = new();

    public async Task<IActionResult> OnGetAsync()
    {
        var userId = _userManager.GetUserId(User);
        if (userId == null)
        {
            return RedirectToPage("/Identity/Account/Login");
        }

        var user = await _context.Users
            .Include(u => u.UserSphereSatisfactions)
            .ThenInclude(uss => uss.SphereIdsNavigation)
            .FirstOrDefaultAsync(u => u.IdentityUserId == userId);

        if (user == null)
        {
            return NotFound("User not found.");
        }

        TestModel.IdentityUserId = user.IdentityUserId;

        // Пример вопросов
        TestModel.Questions = new List<SphereQuestion>
        {
            new() { SphereName = "Саморазвитие", QuestionText = "Как вы оцениваете свои успехи в изучении нового?" },
            new() { SphereName = "Саморазвитие", QuestionText = "Насколько вы довольны временем, которое уделяете своему развитию?" },

            new() { SphereName = "Здоровье", QuestionText = "Как вы оцениваете ваше физическое состояние?" },
            new() { SphereName = "Здоровье", QuestionText = "Как вы оцениваете вашу энергию?" },

            new() { SphereName = "Отдых", QuestionText = "Насколько вы довольны качеством вашего отдыха?" },
            new() { SphereName = "Отдых", QuestionText = "Как вы оцениваете время, которое вы уделяете отдыху?" },

            new() { SphereName = "Окружение", QuestionText = "Как вы оцениваете поддержку, которую получаете от окружения?" },
            new() { SphereName = "Окружение", QuestionText = "Насколько вы довольны отношениями с близкими людьми?" },

            new() { SphereName = "Любовь", QuestionText = "Как вы оцениваете ваши романтические отношения?" },
            new() { SphereName = "Любовь", QuestionText = "Насколько вы довольны эмоциональной близостью в отношениях?" },

            new() { SphereName = "Карьера", QuestionText = "Как вы оцениваете свою профессиональную реализацию?" },
            new() { SphereName = "Карьера", QuestionText = "Как вы оцениваете ваш карьерный рост?" },

            new() { SphereName = "Финансы", QuestionText = "Как вы оцениваете вашу финансовую стабильность?" },
            new() { SphereName = "Финансы", QuestionText = "Насколько вы довольны уровнем ваших доходов?" },

            new() { SphereName = "Духовность", QuestionText = "Как вы оцениваете связь с вашими внутренними ценностями?" },
            new() { SphereName = "Духовность", QuestionText = "Насколько вы довольны временем, которое вы уделяете духовным практикам?" },        
        };

        return Page();
    }

[HttpPost("SaveTestResults")]
public IActionResult SaveTestResults([FromBody] TestViewModel testModel)
{
    var identityUserId = _userManager.GetUserId(User);
    if (identityUserId == null)
    {
        return NotFound("User not found.");
    }

    // Получаем пользователя синхронно
    var user = _context.Users
        .Include(u => u.UserSphereSatisfactions)
        .FirstOrDefault(u => u.IdentityUserId == identityUserId);

    if (user == null)
    {
        return NotFound("User not found.");
    }

    // Группируем вопросы по SphereName
    var sphereResults = testModel.CalculateSphereSatisfaction();

    foreach (var (sphereName, satisfactionLevel) in sphereResults)
    {
        Console.WriteLine($"Сфера: {sphereName}, Уровень удовлетворенности: {satisfactionLevel}");

        // Получаем список всех вопросов по данной сфере
        var questionsInSphere = testModel.Questions.Where(q => q.SphereName == sphereName).ToList();
        
        // Находим или создаем запись о удовлетворенности для текущей сферы
        var existingSatisfaction = user.UserSphereSatisfactions
            .FirstOrDefault(uss => uss.SphereIdsNavigation.Name == sphereName);

        double newSatisfactionLevel = satisfactionLevel / questionsInSphere.Count;

        if (existingSatisfaction != null)
        {
            // Если запись существует, обновляем ее
            existingSatisfaction.SatisfactionLevel = (double)Math.Round(newSatisfactionLevel, 2); // округление до 2 знаков после запятой
        }
        else
        {
            // Если записи нет, создаем новую
            var sphere = _context.SpheresOfLives
                .FirstOrDefault(s => s.Name == sphereName);

            if (sphere == null) continue;

            var newSatisfaction = new WebApplication1.Models.UserSphereSatisfaction
            {
                UserSpheres = user.IdUser, // Используем IdUser у пользователя
                SphereIds = sphere.SphereId, // Используем SphereId у сферы
                SatisfactionLevel = (double)Math.Round(newSatisfactionLevel, 2) // округляем до 2 знаков
            };

            _context.UserSphereSatisfactions.Add(newSatisfaction);
        }
    }

    // Сохраняем изменения в базе данных
    _context.SaveChanges();

    return new StatusCodeResult(200);  // Возвращаем статус 200 OK
}



    public class SphereQuestion
{
    public string SphereName { get; set; } = string.Empty;
    public string QuestionText { get; set; } = string.Empty;
 public short SatisfactionLevel { get; set; } = 0;
}

    public class TestViewModel
{
    //  public int IdUser { get; set; }
    public string IdentityUserId { get; set; }
    public List<SphereQuestion> Questions { get; set; } = new List<SphereQuestion>();

   public Dictionary<string, double> CalculateSphereSatisfaction()
{
    return Questions
        .GroupBy(q => q.SphereName)
        .ToDictionary(
            g => g.Key,
            g => 
            {
                // Вычисляем средний уровень удовлетворенности и ограничиваем его диапазоном 0-100
                var averageSatisfaction = g.Average(q => q.SatisfactionLevel);
                return Math.Clamp(averageSatisfaction, 0, 10); // Math.Clamp ограничивает значение
            }
        );
}
}

}
}