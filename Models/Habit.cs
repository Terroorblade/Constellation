using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class Habit
{
    public int HabitId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public int Frequency { get; set; }

    public DateTime? Reminder { get; set; }

    public bool Status { get; set; }

    public int? GoalHabit { get; set; }

    public virtual Goal? GoalHabitNavigation { get; set; }

    public virtual ICollection<HabitOfTheDay> HabitOfTheDays { get; set; } = new List<HabitOfTheDay>();
      // Добавляем связь с пользователем
        public int UserId { get; set; }
        public virtual User? User { get; set; }
}
