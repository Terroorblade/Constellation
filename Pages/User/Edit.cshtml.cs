using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using КурсоваяБД5.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace КурсоваяБД5.Pages.User
{
    [Authorize(Roles = "admin,default_user")]
    public class EditModel : PageModel
    {
        private readonly КурсоваяБД5.Models.FivesemestercswrkContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public EditModel(КурсоваяБД5.Models.FivesemestercswrkContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty]
        public Models.User Users { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            TempData["returnUrl"] = Request.Headers["Referer"].ToString();

            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FirstOrDefaultAsync(m => m.UserId == id);
            if (user == null)
            {
                return NotFound();
            }

            // проверка авторизации
            var userId = _userManager.GetUserId(User);
            if (user.IdentityUserId != userId && !User.IsInRole("admin"))
            {
                return Forbid();
            }

            Users = user;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Users).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(Users.UserId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            if (User.IsInRole("admin"))
            {
                var returnUrl = TempData["returnUrl"] as string;
                if (!string.IsNullOrEmpty(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                else
                {
                    return RedirectToPage("./");
                }
            }
            else
            {
                return RedirectToPage("./Profile", new { id = Users.UserId });
            }
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.UserId == id);
        }
    }
}