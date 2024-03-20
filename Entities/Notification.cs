using System;
using System.Collections.Generic;

namespace CrewBackend.Entities;

public partial class Notification
{
    public int Id { get; set; }

    public string Subject { get; set; } = null!;

    public string Body { get; set; } = null!;

    public string Name { get; set; } = null!;

    public virtual ICollection<UsersNotification> UsersNotifications { get; set; } = new List<UsersNotification>();
}
