using System;
using System.Collections.Generic;

namespace CrewBackend.Entities;

public partial class UsersGroup
{
    public long Id { get; set; }

    public long GroupId { get; set; }

    public long UserId { get; set; }

    public int RoleId { get; set; }

    public virtual Group Group { get; set; } = null!;

    public virtual Role Role { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
