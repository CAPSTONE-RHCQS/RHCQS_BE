using System;
using System.Collections.Generic;

namespace RHCQS_DataAccessObjects.Models;

public partial class QuotationItem
{
    public Guid Id { get; set; }

    public string? Unit { get; set; }

    public double? Weight { get; set; }

    public double? UnitPriceLabor { get; set; }

    public double? UnitPriceRough { get; set; }

    public double? UnitPriceFinished { get; set; }

    public double? TotalPriceLabor { get; set; }

    public double? TotalPriceRough { get; set; }

    public double? TotalPriceFinished { get; set; }

    public DateTime? InsDate { get; set; }

    public DateTime? UpsDate { get; set; }

    public Guid? FinalQuotationItemId { get; set; }

    public string? Note { get; set; }

    public Guid? WorkTemplateId { get; set; }

    public virtual FinalQuotationItem? FinalQuotationItem { get; set; }

    public virtual WorkTemplate? WorkTemplate { get; set; }
}
