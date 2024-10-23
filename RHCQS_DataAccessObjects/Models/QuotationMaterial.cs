using System;
using System.Collections.Generic;

namespace RHCQS_DataAccessObjects.Models;

public partial class QuotationMaterial
{
    public Guid Id { get; set; }

    public Guid QuotationItemId { get; set; }

    public Guid MaterialId { get; set; }

    public string? Unit { get; set; }

    public double? MaterialPrice { get; set; }

    public virtual Material Material { get; set; } = null!;

    public virtual QuotationItem QuotationItem { get; set; } = null!;
}
