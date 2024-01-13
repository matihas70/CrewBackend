using System;
using System.Collections.Generic;

namespace CrewBackend.Entities;

public partial class AppSetting
{
    public string? Email { get; set; }

    public string? EmailLogin { get; set; }

    public string? EmailPassword { get; set; }

    public byte? EmailSsl { get; set; }

    public string? EmailHost { get; set; }

    public int? EmailPort { get; set; }
}
