using System;
using System.Collections.Generic;

namespace RHCQS_DataAccessObjects.Models;

public partial class EquimentItem
{
    public Guid Id { get; set; }

    public string? Name { get; set; }

    public string? Unit { get; set; }

    public int? Quantity { get; set; }

    public double? UnitOfMaterial { get; set; }

    public double? TotalOfMaterial { get; set; }

    public string? Note { get; set; }

    public Guid FinalQuotationId { get; set; }

    public virtual FinalQuotation FinalQuotation { get; set; } = null!;
}
