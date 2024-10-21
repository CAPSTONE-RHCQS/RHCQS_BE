using System;
using System.Collections.Generic;

namespace RHCQS_DataAccessObjects.Models;

public partial class QuotationSection
{
    public Guid Id { get; set; }

    public Guid PackageId { get; set; }

    public string? Name { get; set; }

    public DateTime? InsDate { get; set; }

    public DateTime? UpsDate { get; set; }

    public string? Status { get; set; }

    public string? Type { get; set; }

    public string? Note { get; set; }

    public int? ContractionTime { get; set; }

    public Guid? FinalQuotationItemId { get; set; }

    public virtual FinalQuotationItem? FinalQuotationItem { get; set; }

    public virtual Package Package { get; set; } = null!;

    public virtual ICollection<QuotationItem> QuotationItems { get; set; } = new List<QuotationItem>();
}
