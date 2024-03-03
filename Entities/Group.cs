using System;
using System.Collections.Generic;

namespace CrewBackend.Entities;

public partial class Group
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Picture { get; set; }

    public long CreatedBy { get; set; }

    public DateTime CreateDate { get; set; }

    public virtual ICollection<GroupsPost> GroupsPosts { get; set; } = new List<GroupsPost>();

    public virtual ICollection<UsersGroup> UsersGroups { get; set; } = new List<UsersGroup>();
}
