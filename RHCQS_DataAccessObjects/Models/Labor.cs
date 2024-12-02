using System;
using System.Collections.Generic;

namespace RHCQS_DataAccessObjects.Models;

public partial class Labor
{
    public Guid Id { get; set; }

    public string? Name { get; set; }

    public double? Price { get; set; }

    public DateTime? InsDate { get; set; }

    public DateTime? UpsDate { get; set; }

    public bool? Deflag { get; set; }

    public string? Type { get; set; }

    public string? Code { get; set; }

    public virtual ICollection<ConstructionWorkResource> ConstructionWorkResources { get; set; } = new List<ConstructionWorkResource>();

    public virtual ICollection<PackageLabor> PackageLabors { get; set; } = new List<PackageLabor>();
}
