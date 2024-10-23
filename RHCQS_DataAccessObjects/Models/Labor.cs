using System;
using System.Collections.Generic;

namespace RHCQS_DataAccessObjects.Models;

public partial class Labor
{
    public Guid Id { get; set; }

    public string? Name { get; set; }

    public double? Price { get; set; }

    public DateTime? InsDate { get; set; }

    public DateTime? UpsDate { get; set; }

    public bool? Deflag { get; set; }

    public string? Type { get; set; }

    public virtual ICollection<PackageLabor> PackageLabors { get; set; } = new List<PackageLabor>();

    public virtual ICollection<QuotationLabor> QuotationLabors { get; set; } = new List<QuotationLabor>();
}
