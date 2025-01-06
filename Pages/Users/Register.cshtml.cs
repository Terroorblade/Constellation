using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication1.Models;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Pages.Users
{
    public class RegisterModel : PageModel
    {
        private readonly ConsttestContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public RegisterModel(ConsttestContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [Display(Name = "Username")]
            public string Username { get; set; }
        }

        public string UserId { get; set; }

        public async Task<IActionResult> OnGetAsync(string userId)
        {
            UserId = userId;

            if (string.IsNullOrEmpty(UserId))
            {
                return Forbid(); // запрет доступа, если нет параметра в запросе
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string userId)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(userId);

                if (user == null)
                {
                    return NotFound($"Unable to find user with ID '{userId}'.");
                }

                user.UserName = Input.Username;
                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    // Insert user data into the Users table
                    var newUser = new User
                    {
                        IdentityUserId = user.Id,
                        Username = user.UserName,
                        Email = user.Email,
                        Password = user.PasswordHash // Handle password securely if needed
                    };

                    _context.Users.Add(newUser);
                    await _context.SaveChangesAsync();

                    return RedirectToPage("/Index"); // Redirect after registration completion
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return Page();
        }
    }
}
