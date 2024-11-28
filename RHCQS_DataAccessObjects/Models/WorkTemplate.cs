using System;
using System.Collections.Generic;

namespace RHCQS_DataAccessObjects.Models;

public partial class WorkTemplate
{
    public Guid Id { get; set; }

    public Guid? ConstructionItemsId { get; set; }

    public Guid? PackageId { get; set; }

    public DateTime? InsDate { get; set; }

    public virtual ConstructionItem? ConstructionItems { get; set; }

    public virtual Package? Package { get; set; }

    public virtual ICollection<QuotationItem> QuotationItems { get; set; } = new List<QuotationItem>();
}
