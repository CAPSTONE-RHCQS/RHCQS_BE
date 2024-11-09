using System;
using System.Collections.Generic;

namespace RHCQS_DataAccessObjects.Models;

public partial class FinalQuotationItem
{
    public Guid Id { get; set; }

    public Guid FinalQuotationId { get; set; }

    public DateTime? InsDate { get; set; }

    public Guid ConstructionItemId { get; set; }

    public Guid? SubContructionId { get; set; }

    public virtual FinalQuotation FinalQuotation { get; set; } = null!;

    public virtual ICollection<QuotationItem> QuotationItems { get; set; } = new List<QuotationItem>();
}
