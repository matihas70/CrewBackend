using System;
using System.Collections.Generic;

namespace CrewBackend.Entities;

public partial class ActivateAccountRequest
{
    public Guid Id { get; set; }

    public long UserId { get; set; }

    public DateTime ExpirationDate { get; set; }

    public virtual User User { get; set; } = null!;
}
