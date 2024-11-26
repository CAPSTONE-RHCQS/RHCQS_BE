using System;
using System.Collections.Generic;

namespace RHCQS_DataAccessObjects.Models;

public partial class QuotationUtility
{
    public Guid Id { get; set; }

    public Guid? UtilitiesItemId { get; set; }

    public Guid? FinalQuotationId { get; set; }

    public Guid? InitialQuotationId { get; set; }

    public string Name { get; set; } = null!;

    public double? Coefficient { get; set; }

    public double? Price { get; set; }

    public string? Description { get; set; }

    public DateTime? InsDate { get; set; }

    public DateTime? UpsDate { get; set; }

    public Guid UtilitiesSectionId { get; set; }

    public int? Quanity { get; set; }

    public virtual FinalQuotation? FinalQuotation { get; set; }

    public virtual InitialQuotation? InitialQuotation { get; set; }

    public virtual UtilitiesItem? UtilitiesItem { get; set; }

    public virtual UtilitiesSection UtilitiesSection { get; set; } = null!;
}
