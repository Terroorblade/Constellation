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

namespace WebApplication1.Pages.Users
{
    [Authorize(Roles = "admin,user")]
    public class DetailsModel : PageModel
    {
        private readonly WebApplication1.Models.ConsttestContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ILogger<DetailsModel> _logger;
        public DetailsModel(WebApplication1.Models.ConsttestContext context, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager,ILogger<DetailsModel> logger)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        public Models.User Users { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(string? id)
        {
            if (id == null)
            {
                _logger.LogWarning("Id пользователя не передан.");
                return NotFound();
            }

            var user = await _context.Users.FirstOrDefaultAsync(m => m.IdentityUserId == id);
            if (user == null)
            {
                _logger.LogWarning($"Пользователь с Id {id} не найден.");
                return NotFound();
            }
            else
            {
                 Users = user;
                 _logger.LogInformation($"Информация о пользователе с Id {id} загружена.");
            }
           var userId = _userManager.GetUserId(User);
        
            if (user.IdentityUserId != id && !User.IsInRole("admin"))
            {
                _logger.LogWarning("Пользователь не имеет доступа.");
                return Forbid();
            }
             return Page();
        }
    }
}