using System;
using System.Collections.Generic;

namespace RHCQS_DataAccessObjects.Models;

public partial class ConstructionWork
{
    public Guid Id { get; set; }

    public string? WorkName { get; set; }

    public double? UnitPrice { get; set; }

    public Guid? MaterialId { get; set; }

    public Guid? LaborId { get; set; }

    public Guid? ConstructionId { get; set; }

    public DateTime? InsDate { get; set; }

    public string? Unit { get; set; }

    public virtual ConstructionItem? Construction { get; set; }
}
