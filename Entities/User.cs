﻿using System;
using System.Collections.Generic;

namespace CrewBackend.Entities;

public partial class User
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public string Surname { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? Callname { get; set; }

    public string? Picture { get; set; }

    public DateTime CreateDate { get; set; }
}
