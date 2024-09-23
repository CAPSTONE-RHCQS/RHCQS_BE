using System;
using System.Collections.Generic;

namespace RHCQS_DataAccessObjects.Models;

public partial class MaterialSection
{
    public Guid Id { get; set; }

    public Guid MaterialId { get; set; }

    public string? Name { get; set; }

    public DateTime? InsDate { get; set; }

    public virtual Material Material { get; set; } = null!;

    public virtual ICollection<PackageMaterial> PackageMaterials { get; set; } = new List<PackageMaterial>();
}
