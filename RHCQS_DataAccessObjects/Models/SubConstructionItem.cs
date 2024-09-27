using System;
using System.Collections.Generic;

namespace RHCQS_DataAccessObjects.Models;

public partial class SubConstructionItem
{
    public Guid Id { get; set; }

    public Guid ConstructionItemsId { get; set; }

    public string? Name { get; set; }

    public double? Coefficient { get; set; }

    public string? Unit { get; set; }

    public DateTime? InsDate { get; set; }

    public virtual ConstructionItem ConstructionItems { get; set; } = null!;
}
