using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using КурсоваяБД5.Models;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using BCrypt.Net;

public class LoginModel : PageModel
{
    private readonly FivesemestercswrkContext _context;

    public LoginModel(FivesemestercswrkContext context)
    {
        _context = context;
    }

    [BindProperty]
    public string Email { get; set; }
    [BindProperty]
    public string Password { get; set; }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync(string email, string password)
{
    // Найти пользователя в базе данных
    var user = _context.Users.FirstOrDefault(u => u.Email == email);

    if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
    {
        ModelState.AddModelError(string.Empty, "Неверный email или пароль.");
        return Page();
    }

    // Создать claims для пользователя
    var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, user.Username),
        new Claim(ClaimTypes.Email, user.Email),
        new Claim("UserId", user.UserId.ToString())
    };

    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

    var authProperties = new AuthenticationProperties
    {
        IsPersistent = true, // Запомнить пользователя
        ExpiresUtc = DateTimeOffset.UtcNow.AddDays(7)
    };

    // Выполнить вход
    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

    return RedirectToPage("/Profile");
}
public async Task<IActionResult> OnPostLogoutAsync()
{
    await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    return RedirectToPage("/Index");

}

}
