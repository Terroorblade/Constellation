using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using WebApplication1.Models;

namespace WebApplication1.Pages.Users
{
    [Authorize(Roles = "admin,user")]
    public class TestResultsModel : PageModel
    {
        private readonly ConsttestContext _context;

        public TestResultsModel(ConsttestContext context)
        {
            _context = context;
        }

        public List<SphereResultViewModel> SphereResults { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int userId)
        {
            var user = await _context.Users
                .Include(u => u.UserSphereSatisfactions)
                .ThenInclude(uss => uss.SphereIdsNavigation)
                .FirstOrDefaultAsync(u => u.IdUser == userId);

            if (user == null)
            {
                return NotFound("Пользователь не найден.");
            }

            SphereResults = user.UserSphereSatisfactions
                .Select(uss => new SphereResultViewModel
                {
                    SphereName = uss.SphereIdsNavigation?.Name ?? "Неизвестная сфера",
                    SatisfactionLevel = uss.SatisfactionLevel ?? 0
                })
                .ToList();

            return Page();
        }

        public class SphereResultViewModel
        {
            public string SphereName { get; set; }
            public double SatisfactionLevel { get; set; }
        }
    }
}
