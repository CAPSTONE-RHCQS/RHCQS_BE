using System;
using System.Collections.Generic;

namespace RHCQS_DataAccessObjects.Models;

public partial class Promotion
{
    public Guid Id { get; set; }

    public string? Code { get; set; }

    public int? Value { get; set; }

    public DateTime? InsDate { get; set; }

    public DateTime? StartTime { get; set; }

    public string? Name { get; set; }

    public DateTime? ExpTime { get; set; }

    public bool? IsRunning { get; set; }

    public virtual ICollection<FinalQuotation> FinalQuotations { get; set; } = new List<FinalQuotation>();

    public virtual ICollection<InitialQuotation> InitialQuotations { get; set; } = new List<InitialQuotation>();

    public virtual ICollection<PackageMapPromotion> PackageMapPromotions { get; set; } = new List<PackageMapPromotion>();
}
