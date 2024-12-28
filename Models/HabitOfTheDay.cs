using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace КурсоваяБД5.Models;

public partial class HabitOfTheDay
{
     [Key]public int HabitDayId { get; set; }

    public bool Status { get; set; }

    public int? HabitDay { get; set; }

    public int? ScheduleDay { get; set; }

    public virtual Habit? HabitDayNavigation { get; set; } = null!;

    public virtual DailySchedule? ScheduleDayNavigation { get; set; } = null!;
}
