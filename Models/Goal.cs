using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class Goal
{
    public int GoalId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    // public DateOnly? CreateDate { get; set; }

    public DateOnly? Deadline { get; set; }

    public bool Status { get; set; }

    public int? GoalSphere { get; set; }

    public virtual SpheresOfLife? GoalSphereNavigation { get; set; }

    public virtual ICollection<Habit> Habits { get; set; } = new List<Habit>();
     // Добавляем связь с пользователем
        public int UserId { get; set; }
        public virtual User? User { get; set; }
}
