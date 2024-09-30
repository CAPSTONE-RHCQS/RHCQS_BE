using System;
using System.Collections.Generic;

namespace RHCQS_DataAccessObjects.Models;

public partial class Customer
{
    public Guid Id { get; set; }

    public string? Email { get; set; }

    public string? Username { get; set; }

    public string? ImgUrl { get; set; }

    public string? PasswordHash { get; set; }

    public string? PhoneNumber { get; set; }

    public DateTime? DateOfBirth { get; set; }

    public DateTime? InsDate { get; set; }

    public DateTime? UpsDate { get; set; }

    public bool? Deflag { get; set; }

    public virtual ICollection<Project> Projects { get; set; } = new List<Project>();
}
