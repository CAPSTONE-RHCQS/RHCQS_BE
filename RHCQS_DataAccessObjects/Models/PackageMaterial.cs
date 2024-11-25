using System;
using System.Collections.Generic;

namespace RHCQS_DataAccessObjects.Models;

public partial class PackageMaterial
{
    public Guid Id { get; set; }

    public Guid PackageDetailId { get; set; }

    public DateTime? InsDate { get; set; }

    public Guid? MaterialId { get; set; }

    public virtual Material? Material { get; set; }

    public virtual PackageDetail PackageDetail { get; set; } = null!;
}
