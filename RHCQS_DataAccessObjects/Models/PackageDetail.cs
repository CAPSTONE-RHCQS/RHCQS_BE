using System;
using System.Collections.Generic;

namespace RHCQS_DataAccessObjects.Models;

public partial class PackageDetail
{
    public Guid Id { get; set; }

    public Guid PackageId { get; set; }

    public string? Type { get; set; }

    public DateTime? InsDate { get; set; }

    public virtual Package Package { get; set; } = null!;

    public virtual ICollection<PackageLabor> PackageLabors { get; set; } = new List<PackageLabor>();

    public virtual ICollection<PackageMaterial> PackageMaterials { get; set; } = new List<PackageMaterial>();
}
