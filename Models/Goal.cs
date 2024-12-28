using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace КурсоваяБД5.Models;

public partial class Goal
{
     [Key]public int GoalId { get; set; }

    [Required, StringLength(50)]public string? Name { get; set; } = null!;

    [StringLength(100)] public string? Description { get; set; }

    public DateOnly? CreateDate { get; set; } = null!;

    public DateOnly? Deadline { get; set; } = null!;

    public bool? Status { get; set; } = null!;

    public int? GoalSphere { get; set; }

    public virtual SpheresOfLife? GoalSphereNavigation { get; set; } = null!;

    public virtual ICollection<Habit> Habits { get; set; } = new List<Habit>();
}
