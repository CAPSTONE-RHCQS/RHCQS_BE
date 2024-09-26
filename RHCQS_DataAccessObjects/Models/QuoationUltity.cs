using System;
using System.Collections.Generic;

namespace RHCQS_DataAccessObjects.Models;

public partial class QuoationUltity
{
    public Guid Id { get; set; }

    public Guid UltilitiesItemId { get; set; }

    public Guid DetailedQuotationId { get; set; }

    public Guid? InitialQuotationId { get; set; }

    public string Name { get; set; } = null!;

    public double? Coefiicient { get; set; }

    public double? Price { get; set; }

    public string? Description { get; set; }

    public DateTime? InsDate { get; set; }

    public DateTime? UpsDate { get; set; }

    public virtual ICollection<DetailedQuotation> DetailedQuotations { get; set; } = new List<DetailedQuotation>();

    public virtual UltilitiesItem UltilitiesItem { get; set; } = null!;
}
