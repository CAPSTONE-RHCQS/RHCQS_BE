using System;
using System.Collections.Generic;

namespace RHCQS_DataAccessObjects.Models;

public partial class InitialQuotation
{
    public Guid Id { get; set; }

    public Guid? AccountId { get; set; }

    public Guid ProjectId { get; set; }

    public Guid? PromotionId { get; set; }

    public double? Area { get; set; }

    public int? TimeProcessing { get; set; }

    public int? TimeRough { get; set; }

    public int? TimeOthers { get; set; }

    public string? OthersAgreement { get; set; }

    public DateTime? InsDate { get; set; }

    public string? Status { get; set; }

    public double Version { get; set; }

    public bool? IsTemplate { get; set; }

    public bool? Deflag { get; set; }

    public string? Note { get; set; }

    public virtual Account? Account { get; set; }

    public virtual ICollection<BactchPayment> BactchPayments { get; set; } = new List<BactchPayment>();

    public virtual ICollection<InitialQuotationItem> InitialQuotationItems { get; set; } = new List<InitialQuotationItem>();

    public virtual ICollection<PackageQuotation> PackageQuotations { get; set; } = new List<PackageQuotation>();

    public virtual Project Project { get; set; } = null!;

    public virtual ICollection<QuotationUtility> QuotationUtilities { get; set; } = new List<QuotationUtility>();
}
