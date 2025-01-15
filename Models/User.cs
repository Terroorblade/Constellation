using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace WebApplication1.Models;

public partial class User 
{
    public int IdUser { get; set; }
    public string? IdentityUserId { get; set; }

    public string Username { get; set; } = null!;

    public string Email { get; set; } = null!;

    public DateOnly? Birthday { get; set; }

    public string Password { get; set; } = null!;

    public virtual ICollection<DailySchedule> DailySchedules { get; set; } = new List<DailySchedule>();
    public virtual ICollection<Goal> Goals{ get; set; } = new List<Goal>();
     public virtual ICollection<Habit> Habits { get; set; } = new List<Habit>();

    public virtual ICollection<UserSphereSatisfaction> UserSphereSatisfactions { get; set; } = new List<UserSphereSatisfaction>();
    public virtual IdentityUser IdentityUser { get; set; } // Навигационное свойство
}
