using WebApplication1.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Pages.Users
{
    public class RegisterModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public RegisterModel(UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        [BindProperty]
        public User Input { get; set; } = new User();

        [BindProperty]
        public string Role { get; set; } = "default_user";  // Default role

        public async Task<IActionResult> OnGetAsync()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var user = new User
                {
                    Username = Input.Username,
                    Email = Input.Email,
                    Birthday = Input.Birthday
                };

                var result = await _userManager.CreateAsync(user, "Password123!_");

                if (result.Succeeded)
                {
                    // Assign role
                    await _userManager.AddToRoleAsync(user, Role);

                    // Sign the user in
                    await _signInManager.SignInAsync(user, isPersistent: false);

                    // Redirect based on role
                    if (Role == "admin")
                    {
                        return RedirectToPage("/Index");
                    }
                    else
                    {
                        return RedirectToPage("/Index");
                    }
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
