using System;
using System.Collections.Generic;

namespace RHCQS_DataAccessObjects.Models;

public partial class QuotationLabor
{
    public Guid Id { get; set; }

    public Guid QuotationItemId { get; set; }

    public Guid LaborId { get; set; }

    public double? LaborPrice { get; set; }

    public virtual Labor Labor { get; set; } = null!;

    public virtual QuotationItem QuotationItem { get; set; } = null!;
}
