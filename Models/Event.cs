using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace КурсоваяБД5.Models;

public partial class Event
{
    [Key]public int EventId { get; set; }

    public string? Name { get; set; } = null!;

    public string? Description { get; set; }

    public DateOnly? EventDate { get; set; } = null!;

    public string? Priority { get; set; } = null!;

    public bool? Status { get; set; } = null!;

    public int? EventSchedule { get; set; }

    public virtual DailySchedule? EventScheduleNavigation { get; set; }=null!;
}
