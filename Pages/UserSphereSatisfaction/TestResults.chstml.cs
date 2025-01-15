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

public int? FilterMonth { get; set; }
public int? FilterYear { get; set; }

public async Task<IActionResult> OnGetAsync(int userId, int? month, int? year)
{
    FilterMonth = month;
    FilterYear = year;

    var user = await _context.Users
        .Include(u => u.UserSphereSatisfactions)
        .ThenInclude(uss => uss.SphereIdsNavigation)
        .FirstOrDefaultAsync(u => u.IdUser == userId);

    if (user == null)
    {
        return NotFound("Пользователь не найден.");
    }

    var userSphereSatisfactions = user.UserSphereSatisfactions.AsQueryable();

    if (month.HasValue)
    {
        userSphereSatisfactions = userSphereSatisfactions.Where(uss => uss.TestDate.Month == month.Value);
    }

    if (year.HasValue)
    {
        userSphereSatisfactions = userSphereSatisfactions.Where(uss => uss.TestDate.Year == year.Value);
    }

    SphereResults = userSphereSatisfactions
        .GroupBy(uss => DateOnly.FromDateTime(uss.TestDate))
        .SelectMany(group => group.Select(uss => new SphereResultViewModel
        {
            TestDate = group.Key.ToString("dd.MM.yyyy"),
           SphereName = uss.SphereIdsNavigation != null ? uss.SphereIdsNavigation.Name : "Неизвестная сфера",
            SatisfactionLevel = uss.SatisfactionLevel ?? 0
        }))
        .ToList();

    return Page();
}


        public class SphereResultViewModel
        {
            public string TestDate { get; set; }
            public string SphereName { get; set; }
            public double SatisfactionLevel { get; set; }
        }
    }
}
