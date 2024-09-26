using System;
using System.Collections.Generic;

namespace RHCQS_DataAccessObjects.Models;

public partial class Account
{
    public Guid Id { get; set; }

    public Guid? RoleId { get; set; }

    public string? Email { get; set; }

    public string? Username { get; set; }

    public string? ImageUrl { get; set; }

    public string? PasswordHasd { get; set; }

    public string? PhoneNumber { get; set; }

    public DateOnly? DateOfBirth { get; set; }

    public DateTime? InsDate { get; set; }

    public DateTime? UpsDate { get; set; }

    public bool? Deflag { get; set; }

    public virtual ICollection<AssignTask> AssignTasks { get; set; } = new List<AssignTask>();

    public virtual ICollection<Blog> Blogs { get; set; } = new List<Blog>();

    public virtual ICollection<InitialQuotation> InitialQuotations { get; set; } = new List<InitialQuotation>();

    public virtual ICollection<Project> Projects { get; set; } = new List<Project>();

    public virtual Role? Role { get; set; }
}
