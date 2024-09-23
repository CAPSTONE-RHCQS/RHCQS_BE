using System;
using System.Collections.Generic;

namespace RHCQS_DataAccessObjects.Models;

public partial class InitialQuotationItem
{
    public Guid Id { get; set; }

    public string? Name { get; set; }

    public Guid? ConstructionItemId { get; set; }

    public Guid? SubConstruction { get; set; }

    public double? Area { get; set; }

    public double? Price { get; set; }

    public double? UnitPrice { get; set; }

    public DateTime? InsDate { get; set; }

    public DateTime? UpsDate { get; set; }

    public virtual ConstructionItem? ConstructionItem { get; set; }
}
