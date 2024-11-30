using System;
using System.Collections.Generic;

namespace RHCQS_DataAccessObjects.Models;

public partial class WorkTemplate
{
    public Guid Id { get; set; }

    public Guid? PackageId { get; set; }

    public DateTime? InsDate { get; set; }

    public Guid? ContructionWorkId { get; set; }

    public double? LaborCost { get; set; }

    public double? MaterialCost { get; set; }

    public double? MaterialFinishedCost { get; set; }

    public double? TotalCost { get; set; }

    public virtual ConstructionWork? ContructionWork { get; set; }

    public virtual Package? Package { get; set; }

    public virtual ICollection<QuotationItem> QuotationItems { get; set; } = new List<QuotationItem>();
}
