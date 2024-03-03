using System;
using System.Collections.Generic;

namespace CrewBackend.Entities;

public partial class GroupsPost
{
    public long Id { get; set; }

    public string Title { get; set; } = null!;

    public string Body { get; set; } = null!;

    public string? TaggedUsers { get; set; }

    public long CreatedBy { get; set; }

    public DateTime CreateDate { get; set; }

    public DateTime? EditDate { get; set; }

    public byte Deleted { get; set; }

    public DateTime? DeleteDate { get; set; }

    public long GroupId { get; set; }

    public virtual User CreatedByNavigation { get; set; } = null!;

    public virtual Group Group { get; set; } = null!;
}
