using System;
using System.Collections.Generic;

namespace RHCQS_DataAccessObjects.Models;

public partial class HouseDesignDrawing
{
    public Guid Id { get; set; }

    public Guid ProjectId { get; set; }

    public string? Name { get; set; }

    public int? Step { get; set; }

    public string? Status { get; set; }

    public string? Type { get; set; }

    public bool? IsCompany { get; set; }

    public DateTime? InsDate { get; set; }

    public Guid? AssignTaskId { get; set; }

    public virtual AssignTask? AssignTask { get; set; }

    public virtual ICollection<HouseDesignVersion> HouseDesignVersions { get; set; } = new List<HouseDesignVersion>();

    public virtual ICollection<Medium> Media { get; set; } = new List<Medium>();

    public virtual Project Project { get; set; } = null!;
}
