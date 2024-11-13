using System;
using System.Collections.Generic;

namespace RHCQS_DataAccessObjects.Models;

public partial class TemplateItem
{
    public Guid Id { get; set; }

    public Guid SubTemplateId { get; set; }

    public Guid ConstructionItemId { get; set; }

    public double? Area { get; set; }

    public string? Unit { get; set; }

    public DateTime? InsDate { get; set; }

    public Guid? SubConstructionId { get; set; }

    public double Price { get; set; }

    public virtual ConstructionItem ConstructionItem { get; set; } = null!;

    public virtual SubConstructionItem? SubConstruction { get; set; }

    public virtual SubTemplate SubTemplate { get; set; } = null!;
}
