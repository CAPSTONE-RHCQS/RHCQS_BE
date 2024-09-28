using System;
using System.Collections.Generic;

namespace RHCQS_DataAccessObjects.Models;

public partial class UtilitiesItem
{
    public Guid Id { get; set; }

    public Guid SectionId { get; set; }

    public string? Name { get; set; }

    public double? Coefficient { get; set; }

    public DateTime? InsDate { get; set; }

    public DateTime? UpsDate { get; set; }

    public virtual ICollection<QuotationUtility> QuotationUtilities { get; set; } = new List<QuotationUtility>();

    public virtual UtilitiesSection Section { get; set; } = null!;
}
