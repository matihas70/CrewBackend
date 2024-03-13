using System;
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

    public byte Activated { get; set; }

    public virtual ICollection<ActivateAccountRequest> ActivateAccountRequests { get; set; } = new List<ActivateAccountRequest>();

    public virtual ICollection<GroupsPost> GroupsPosts { get; set; } = new List<GroupsPost>();

    public virtual ICollection<Session> Sessions { get; set; } = new List<Session>();

    public virtual ICollection<UserEducation> UserEducations { get; set; } = new List<UserEducation>();

    public virtual ICollection<UsersGroup> UsersGroups { get; set; } = new List<UsersGroup>();

    public virtual ICollection<UsersNotification> UsersNotifications { get; set; } = new List<UsersNotification>();
}
