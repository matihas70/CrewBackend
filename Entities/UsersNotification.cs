using System;
using System.Collections.Generic;

namespace CrewBackend.Entities;

public partial class UsersNotification
{
    public long Id { get; set; }

    public long UserId { get; set; }

    public int NotifyId { get; set; }

    public bool Seen { get; set; }

    public string? Details { get; set; }

    public virtual Notification Notify { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
