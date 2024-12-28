using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace КурсоваяБД5.Models;

public partial class DailySchedule
{
    [Key] public int ScheduleId { get; set; }

    public DateOnly? ScheduleData { get; set; }

    public int? UserSchedule { get; set; }

    public virtual ICollection<Event> Events { get; set; } = new List<Event>();

    public virtual ICollection<HabitOfTheDay> HabitOfTheDays { get; set; } = new List<HabitOfTheDay>();

    public virtual User? UserScheduleNavigation { get; set; } =null!;
}
