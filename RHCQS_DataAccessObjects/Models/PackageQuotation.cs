using System;
using System.Collections.Generic;

namespace RHCQS_DataAccessObjects.Models;

public partial class PackageQuotation
{
    public Guid Id { get; set; }

    public Guid PackageId { get; set; }

    public Guid InitialQuotationId { get; set; }

    public string? Type { get; set; }

    public DateTime? InsDate { get; set; }

    public virtual InitialQuotation InitialQuotation { get; set; } = null!;

    public virtual Package Package { get; set; } = null!;
}
