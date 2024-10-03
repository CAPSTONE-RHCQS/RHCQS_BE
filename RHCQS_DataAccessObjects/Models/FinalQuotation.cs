using System;
using System.Collections.Generic;

namespace RHCQS_DataAccessObjects.Models;

public partial class FinalQuotation
{
    public Guid Id { get; set; }

    public Guid ProjectId { get; set; }

    public Guid? PromotionId { get; set; }

    public double? TotalPrice { get; set; }

    public string? Note { get; set; }

    public string? Version { get; set; }

    public DateTime? InsDate { get; set; }

    public DateTime? UpsDate { get; set; }

    public string? Status { get; set; }

    public bool? Deflag { get; set; }

    public Guid? QuotationUtilitiesId { get; set; }

    public Guid AccountId { get; set; }

    public virtual Account Account { get; set; } = null!;

    public virtual ICollection<BactchPayment> BactchPayments { get; set; } = new List<BactchPayment>();

    public virtual ICollection<EquimentItem> EquimentItems { get; set; } = new List<EquimentItem>();

    public virtual ICollection<FinalQuotationItem> FinalQuotationItems { get; set; } = new List<FinalQuotationItem>();

    public virtual Project Project { get; set; } = null!;

    public virtual Promotion? Promotion { get; set; }

    public virtual QuotationUtility? QuotationUtilities { get; set; }
}
