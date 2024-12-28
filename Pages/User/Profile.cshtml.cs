using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using КурсоваяБД5.Models; 
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace КурсоваяБД5.Pages.User
{
    [Authorize(Roles = "admin,default_user")]
    public class ProfileModel : PageModel
    {
        private readonly FivesemestercswrkContext _context; 
        private readonly ILogger<ProfileModel> _logger;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public ProfileModel(FivesemestercswrkContext context,ILogger<ProfileModel> logger,UserManager<IdentityUser> userManager,SignInManager<IdentityUser> signInManager)
        {
            _context = context;
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;

        }

        public Models.User Users { get; set; } = default;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FirstOrDefaultAsync(m => m.UserId == id);
            if (user == null)
            {
                return NotFound();
            }
            else
            {
                Users = user;
            }

            // проверка авторизации
            var userId = _userManager.GetUserId(User);
            if (user.IdentityUserId != userId && !User.IsInRole("admin"))
            {
                return Forbid();
            }

            return Page();
        }

    }
}
