using System;
using System.Collections.Generic;

namespace RHCQS_DataAccessObjects.Models;

public partial class ConstructionItem
{
    public Guid Id { get; set; }

    public string? Name { get; set; }

    public double? Coefficient { get; set; }

    public string? Unit { get; set; }

    public DateTime? InsDate { get; set; }

    public DateTime? UpsDate { get; set; }

    public string? Type { get; set; }

    public bool? IsFinalQuotation { get; set; }

    public string? Code { get; set; }

    public virtual ICollection<ConstructionWork> ConstructionWorks { get; set; } = new List<ConstructionWork>();

    public virtual ICollection<InitialQuotationItem> InitialQuotationItems { get; set; } = new List<InitialQuotationItem>();

    public virtual ICollection<SubConstructionItem> SubConstructionItems { get; set; } = new List<SubConstructionItem>();

    public virtual ICollection<TemplateItem> TemplateItems { get; set; } = new List<TemplateItem>();
}
