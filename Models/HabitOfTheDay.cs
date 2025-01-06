using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class HabitOfTheDay
{
    public int HabitDayId { get; set; }

    public bool Status { get; set; }

    public int? HabitDay { get; set; }

    public int? ScheduleDay { get; set; }

    public virtual Habit? HabitDayNavigation { get; set; }

    public virtual DailySchedule? ScheduleDayNavigation { get; set; }
}
