using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class SpheresOfLife
{
    public int SphereId { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Goal> Goals { get; set; } = new List<Goal>();

    public virtual ICollection<UserSphereSatisfaction> UserSphereSatisfactions { get; set; } = new List<UserSphereSatisfaction>();
}
