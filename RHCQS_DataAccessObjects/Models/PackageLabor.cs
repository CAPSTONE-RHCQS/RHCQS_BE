using System;
using System.Collections.Generic;

namespace RHCQS_DataAccessObjects.Models;

public partial class PackageLabor
{
    public Guid Id { get; set; }

    public Guid PackageDetailId { get; set; }

    public Guid LaborId { get; set; }

    public double? Price { get; set; }

    public int? Quantity { get; set; }

    public DateTime? InsDate { get; set; }

    public virtual Labor Labor { get; set; } = null!;

    public virtual PackageDetail PackageDetail { get; set; } = null!;
}
