using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;

namespace КурсоваяБД5.Models;

public partial class User
{
    [Key] public int UserId { get; set; }
    public string? IdentityUserId { get; set; }   //id из identity

    [Required(ErrorMessage = "Поле обязательно для заполнения"), StringLength(50)]public string? Username { get; set; } = null!;

    [Required, StringLength(50)] public string? Email { get; set; } = null!;

    public DateOnly? Birthday { get; set; }

    public string? PasswordHash { get; set; } = null!;

    public virtual ICollection<DailySchedule> DailySchedules { get; set; } = new List<DailySchedule>();

    public virtual ICollection<UserSphereSatisfaction> UserSphereSatisfactions { get; set; } = new List<UserSphereSatisfaction>();
}
