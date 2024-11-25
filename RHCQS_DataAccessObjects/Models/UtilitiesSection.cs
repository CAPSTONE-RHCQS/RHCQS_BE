using System;
using System.Collections.Generic;

namespace RHCQS_DataAccessObjects.Models;

public partial class UtilitiesSection
{
    public Guid Id { get; set; }

    public Guid UtilitiesId { get; set; }

    public string? Name { get; set; }

    public bool? Deflag { get; set; }

    public DateTime? InsDate { get; set; }

    public DateTime? UpsDate { get; set; }

    public string? Description { get; set; }

    public double? UnitPrice { get; set; }

    public string? Unit { get; set; }

    public virtual ICollection<QuotationUtility> QuotationUtilities { get; set; } = new List<QuotationUtility>();

    public virtual UtilityOption Utilities { get; set; } = null!;

    public virtual ICollection<UtilitiesItem> UtilitiesItems { get; set; } = new List<UtilitiesItem>();
}
