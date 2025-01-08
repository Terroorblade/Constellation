using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
// using WebApplication1.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Data;
using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using WebApplication1;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ConsttestContext>(options =>
    options.UseNpgsql(connectionString)); 

builder.Services.AddRazorPages();

// builder.Services.AddDbContext<WebApplication1.Models.ConsttestContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("ConstellationConnection")));

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedEmail = false; // Отключить необходимость подтверждения email
})
    .AddEntityFrameworkStores<ConsttestContext>()
    .AddDefaultTokenProviders();

// builder.Services.AddIdentityCore<IdentityUser>(options => options.SignIn.RequireConfirmedEmail = false)
//     .AddRoles<IdentityRole>() 
//     .AddSignInManager()
//     .AddDefaultTokenProviders()
//     .AddEntityFrameworkStores<DbContext>();

// ссылка details
builder.Services.AddScoped<UseresService>();

builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute
(
    name: "default",
    pattern: "{controller=Users}/{action=LK}/{id?}"
);

app.MapRazorPages();

// Регистрируем роли и пользователя
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    var roles = new[] { "admin", "user" };

    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role)) await roleManager.CreateAsync(new IdentityRole(role));
    }
}

using (var scope = app.Services.CreateScope())
{
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

    var adminUser = userManager.FindByNameAsync("admin@mail.com").Result;

    if (adminUser == null)
    {
        adminUser = new IdentityUser { UserName = "admin@mail.com", Email = "admin@mail.com" };
        var result = userManager.CreateAsync(adminUser, "Password123!_").Result;

        if (result.Succeeded)
        {
            userManager.AddToRoleAsync(adminUser, "admin").Wait();
            Console.WriteLine("Admin user created successfully.");
        }
        else
        {
            foreach (var error in result.Errors)
            {
                Console.WriteLine(error.Description);
            }
        }
    }
    else
    {
        Console.WriteLine("Admin user already exists.");
    }
}


app.Run();
