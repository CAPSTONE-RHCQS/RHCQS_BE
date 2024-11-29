using System;
using System.Collections.Generic;

namespace RHCQS_DataAccessObjects.Models;

public partial class MaterialSection
{
    public Guid Id { get; set; }

    public string? Name { get; set; }

    public DateTime? InsDate { get; set; }

    public string Code { get; set; } = null!;

    public virtual ICollection<ConstructionWorkResource> ConstructionWorkResources { get; set; } = new List<ConstructionWorkResource>();

    public virtual ICollection<Material> Materials { get; set; } = new List<Material>();
}
