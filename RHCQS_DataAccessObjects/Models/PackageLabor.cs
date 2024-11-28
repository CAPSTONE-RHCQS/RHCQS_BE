using System;
using System.Collections.Generic;

namespace RHCQS_DataAccessObjects.Models;

public partial class PackageLabor
{
    public Guid Id { get; set; }

    public Guid LaborId { get; set; }

    public double? Price { get; set; }

    public DateTime? InsDate { get; set; }

    public Guid? PackageId { get; set; }

    public virtual Labor Labor { get; set; } = null!;

    public virtual Package? Package { get; set; }
}
