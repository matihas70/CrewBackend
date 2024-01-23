using System;
using System.Collections.Generic;

namespace CrewBackend.Entities;

public partial class Session
{
    public Guid Id { get; set; }

    public long UserId { get; set; }

    public DateTime CreateDate { get; set; }

    public virtual User User { get; set; } = null!;
}
