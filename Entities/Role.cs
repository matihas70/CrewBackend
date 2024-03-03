using System;
using System.Collections.Generic;

namespace CrewBackend.Entities;

public partial class Role
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<UsersGroup> UsersGroups { get; set; } = new List<UsersGroup>();
}
