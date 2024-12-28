using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.RazorPages; 
using Microsoft.EntityFrameworkCore; 
using Microsoft.AspNetCore.Mvc.Rendering;
using КурсоваяБД5.Models; 
using System.Collections.Generic; 
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;


namespace КурсоваяБД5.Pages.User{
public class RegisterModel : PageModel
{
    private readonly КурсоваяБД5.Models.FivesemestercswrkContext _context;
    private readonly SignInManager<IdentityUser> _signInManager;
    
    public RegisterModel( КурсоваяБД5.Models.FivesemestercswrkContext context, SignInManager<IdentityUser> signInManager)
    {
        _context = context;
        _signInManager = signInManager;
    }

    [BindProperty] public Models.User Users { get; set; } = default!;
   
    //get запрос - получение данных
    [BindProperty(SupportsGet = true)] public string UserId { get; set; }

    public async Task<IActionResult> OnGetAsync(string userId)
    {
        UserId = userId;

        if (string.IsNullOrEmpty(UserId))
        {
            return Forbid(); // запрет доступа, если нет параметра в запросе
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }
        Users.IdentityUserId = UserId;

        _context.Users.Add(Users);
        await _context.SaveChangesAsync();

        //авторизация
        // авторизация пользователя
        if (!User.IsInRole("admin"))
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.IdentityUserId == UserId);
            if (user != null)
            {
                var identityUser = new IdentityUser
            {
                Id = user.IdentityUserId,
                UserName = user.Username,
                Email = user.Email
            };
            await _signInManager.SignInAsync(identityUser, isPersistent: false);
        }
        }

        if (User.IsInRole("admin"))
        {
            return RedirectToPage("./Index"); // перенаправление в список customer для администратора
        }
        else
        {
            return RedirectToPage("/Index"); // перенаправление на главную страницу для обычного пользователя
        }

    }
    public void OnGet() { }

    public IActionResult OnPost(string Username, string Email, string Password, DateOnly Birthday)
    {
        // Логика регистрации пользователя
        TempData["SuccessMessage"] = "Регистрация успешна!";
        return RedirectToPage("Index");
    }
}
}