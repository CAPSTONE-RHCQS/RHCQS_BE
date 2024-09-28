using System;
using System.Collections.Generic;

namespace RHCQS_DataAccessObjects.Models;

public partial class DetailedQuotation
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

    public Guid QuotationUlititiesId { get; set; }

    public Guid AccountId { get; set; }

    public virtual Account Account { get; set; } = null!;

    public virtual ICollection<DetailedQuotationItem> DetailedQuotationItems { get; set; } = new List<DetailedQuotationItem>();

    public virtual Project Project { get; set; } = null!;

    public virtual Promotion? Promotion { get; set; }

    public virtual QuotationUtility QuotationUlitities { get; set; } = null!;
}
