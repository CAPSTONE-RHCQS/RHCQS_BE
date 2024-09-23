using System;
using System.Collections.Generic;

namespace RHCQS_DataAccessObjects.Models;

public partial class PackageMaterial
{
    public Guid Id { get; set; }

    public Guid MaterialSectionId { get; set; }

    public Guid PackageDetailId { get; set; }

    public DateTime? InsDate { get; set; }

    public virtual MaterialSection MaterialSection { get; set; } = null!;

    public virtual PackageDetail PackageDetail { get; set; } = null!;
}
