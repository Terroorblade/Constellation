using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication1.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using WebApplication1.Models;

namespace WebApplication1.Pages
{
    public class TestModel : PageModel
    {
        private readonly ConsttestContext _context;

        public TestModel(ConsttestContext context)
        {
            _context = context;
        }

        public List<SpheresOfLife> Spheres { get; set; } = new List<SpheresOfLife>();

        [BindProperty]
        public Dictionary<int, WebApplication1.Models.UserSphereSatisfaction> SphereSatisfactions { get; set; } = new Dictionary<int, WebApplication1.Models.UserSphereSatisfaction>();

public async Task OnGetAsync()
{
    // Загружаем все сферы жизни
    Spheres = await _context.SpheresOfLives.ToListAsync();

    // ID текущего пользователя
    var userId = User.Identity.Name;
    var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == userId);

    if (user != null)
    {
        // Загружаем данные удовлетворенности пользователя из базы
        var existingSatisfactions = await _context.UserSphereSatisfactions
            .Where(s => s.UserSpheres == user.IdUser)
            .ToListAsync();

        foreach (var sphere in Spheres)
        {
            // Если данные существуют, загружаем их, иначе устанавливаем значение по умолчанию
            var satisfaction = existingSatisfactions.FirstOrDefault(s => s.SphereIds == sphere.SphereId);
            SphereSatisfactions[sphere.SphereId] = satisfaction ?? new WebApplication1.Models.UserSphereSatisfaction
            {
                SatisfactionLevel = 0
            };
        }
    }
    else
    {
        // Если пользователь не найден, создаем пустой словарь
        foreach (var sphere in Spheres)
        {
            SphereSatisfactions[sphere.SphereId] = new WebApplication1.Models.UserSphereSatisfaction
            {
                SatisfactionLevel = 0
            };
        }
    }
}



        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                // Если модель не валидна, загружаем сферы жизни для повторного отображения
                Spheres = await _context.SpheresOfLives.ToListAsync();
                return Page();
            }

            // ID текущего пользователя
            var userId = User.Identity.Name; // Предполагается, что в Identity хранится UserName
            var user = _context.Users.FirstOrDefault(u => u.Username == userId);

            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Пользователь не найден.");
                Spheres = await _context.SpheresOfLives.ToListAsync();
                return Page();
            }

            foreach (var satisfaction in SphereSatisfactions)
            {
                var satisfactionEntity = new WebApplication1.Models.UserSphereSatisfaction
                {
                    SatisfactionLevel = satisfaction.Value.SatisfactionLevel,
                    UserSpheres = user.IdUser,
                    SphereIds = satisfaction.Key,
                    TestDate = DateTime.UtcNow

                };

                _context.UserSphereSatisfactions.Add(satisfactionEntity);
            }

            await _context.SaveChangesAsync();

            TempData["Message"] = "Данные успешно сохранены!";
            // return RedirectToPage("/Users/Details");
               return RedirectToPage("/Users/Details");
        }
    }
}
