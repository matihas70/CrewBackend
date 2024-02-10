using System;
using System.Collections.Generic;

namespace CrewBackend.Entities;

public partial class UserEducation
{
    public long Id { get; set; }

    public long UserId { get; set; }

    public string SchoolName { get; set; } = null!;

    public int Type { get; set; }

    public DateOnly DateFrom { get; set; }

    public DateOnly? DateTo { get; set; }

    public string? Field { get; set; }

    public string Degree { get; set; } = null!;

    public string? AdditionalInfo { get; set; }

    public virtual User User { get; set; } = null!;
}
