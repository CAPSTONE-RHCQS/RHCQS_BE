using System;
using System.Collections.Generic;

namespace RHCQS_DataAccessObjects.Models;

public partial class FinalQuotationItem
{
    public Guid Id { get; set; }

    public Guid FinalQuotationId { get; set; }

    public string? Name { get; set; }

    public string? Unit { get; set; }

    public string? Weight { get; set; }

    public double? UnitPriceLabor { get; set; }

    public double? UnitPriceRough { get; set; }

    public double? UnitPriceFinished { get; set; }

    public double? TotalPriceLabor { get; set; }

    public double? TotalPriceRough { get; set; }

    public double? TotalPriceFinished { get; set; }

    public DateTime? InsDate { get; set; }

    public Guid ConstructionItemId { get; set; }

    public virtual ConstructionItem ConstructionItem { get; set; } = null!;

    public virtual FinalQuotation FinalQuotation { get; set; } = null!;
}
