using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class UserSphereSatisfaction
{
    public int SatisfactionId { get; set; }

    public double? SatisfactionLevel { get; set; }

    public int? UserSpheres { get; set; }

    public int? SphereIds { get; set; }

    public virtual SpheresOfLife? SphereIdsNavigation { get; set; } =null!;

    public virtual User? UserSpheresNavigation { get; set; }
}
