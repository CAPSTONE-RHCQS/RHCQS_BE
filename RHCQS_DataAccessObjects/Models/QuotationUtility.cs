using System;
using System.Collections.Generic;

namespace RHCQS_DataAccessObjects.Models;

public partial class QuotationUtility
{
    public Guid Id { get; set; }

    public Guid UtilitiesItemId { get; set; }

    public Guid? FinalQuotationId { get; set; }

    public Guid? InitialQuotationId { get; set; }

    public string Name { get; set; } = null!;

    public double? Coefiicient { get; set; }

    public double? Price { get; set; }

    public string? Description { get; set; }

    public DateTime? InsDate { get; set; }

    public DateTime? UpsDate { get; set; }

    public virtual ICollection<FinalQuotation> FinalQuotations { get; set; } = new List<FinalQuotation>();

    public virtual InitialQuotation? InitialQuotation { get; set; }

    public virtual UtilitiesItem UtilitiesItem { get; set; } = null!;
}
