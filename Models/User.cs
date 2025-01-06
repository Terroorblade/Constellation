using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace WebApplication1.Models;

public partial class User 
{
    public int UserId { get; set; }
    public string? IdentityUserId { get; set; }

    public string Username { get; set; } = null!;

    public string Email { get; set; } = null!;

    public DateOnly? Birthday { get; set; }

    public string Password { get; set; } = null!;

    public virtual ICollection<DailySchedule> DailySchedules { get; set; } = new List<DailySchedule>();

    public virtual ICollection<UserSphereSatisfaction> UserSphereSatisfactions { get; set; } = new List<UserSphereSatisfaction>();
    // public AspNetUser? IdentityUser { get; set; } // Навигационное свойство
}
