using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace КурсоваяБД5.Models;

public partial class Habit
{
     [Key]public int HabitId { get; set; }

    public string? Name { get; set; } = null!;

    public string? Description { get; set; }

    public TimeSpan? Frequency { get; set; } = null!;

    public DateTime? Reminder { get; set; } = null!;

    public bool? Status { get; set; } = null!;

    public int? GoalHabit { get; set; }

    public virtual Goal? GoalHabitNavigation { get; set; } = null!;

    public virtual ICollection<HabitOfTheDay> HabitOfTheDays { get; set; } = new List<HabitOfTheDay>();
}
