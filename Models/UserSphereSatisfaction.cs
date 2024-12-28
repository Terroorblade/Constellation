using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace КурсоваяБД5.Models;

public partial class UserSphereSatisfaction
{
     [Key]public int SatisfactionId { get; set; }

    public short? SatisfactionLevel { get; set; }

    public int? UserSpheres { get; set; }

    public int? SphereIds { get; set; }

    public virtual SpheresOfLife? SphereIdsNavigation { get; set; } = null!;

    public virtual User? UserSpheresNavigation { get; set; } = null!;
}
