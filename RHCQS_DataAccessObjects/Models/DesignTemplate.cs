using System;
using System.Collections.Generic;

namespace RHCQS_DataAccessObjects.Models;

public partial class DesignTemplate
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public int? NumberOfFloor { get; set; }

    public int? NumberOfBed { get; set; }

    public int? NumberOfFront { get; set; }

    public string? ImgUrl { get; set; }

    public DateTime? InsDate { get; set; }

    public double? TotalRough { get; set; }

    public virtual ICollection<Medium> Media { get; set; } = new List<Medium>();

    public virtual ICollection<PackageHouse> PackageHouses { get; set; } = new List<PackageHouse>();

    public virtual ICollection<SubTemplate> SubTemplates { get; set; } = new List<SubTemplate>();
}
