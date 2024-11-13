using System;
using System.Collections.Generic;

namespace RHCQS_DataAccessObjects.Models;

public partial class Account
{
    public Guid Id { get; set; }

    public Guid RoleId { get; set; }

    public string? Email { get; set; }

    public string? Username { get; set; }

    public string? ImageUrl { get; set; }

    public string? PasswordHash { get; set; }

    public string? PhoneNumber { get; set; }

    public DateOnly? DateOfBirth { get; set; }

    public DateTime? InsDate { get; set; }

    public DateTime? UpsDate { get; set; }

    public bool? Deflag { get; set; }

    public virtual ICollection<AssignTask> AssignTasks { get; set; } = new List<AssignTask>();

    public virtual ICollection<Blog> Blogs { get; set; } = new List<Blog>();

    public virtual ICollection<Customer> Customers { get; set; } = new List<Customer>();

    public virtual ICollection<HouseDesignDrawing> HouseDesignDrawings { get; set; } = new List<HouseDesignDrawing>();

    public virtual Role Role { get; set; } = null!;

    public virtual ICollection<Room> RoomReceivers { get; set; } = new List<Room>();

    public virtual ICollection<Room> RoomSenders { get; set; } = new List<Room>();
}
