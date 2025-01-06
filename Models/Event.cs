using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class Event
{
    public int EventId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public DateOnly? EventDate { get; set; }

    public string? Priority { get; set; }

    public bool Status { get; set; }

    public int? EventSchedule { get; set; }

    public virtual DailySchedule? EventScheduleNavigation { get; set; }
}
