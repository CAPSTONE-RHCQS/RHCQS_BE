using System;
using System.Collections.Generic;

namespace RHCQS_DataAccessObjects.Models;

public partial class QuotationItem
{
    public Guid Id { get; set; }

    public Guid QuotationSectionId { get; set; }

    public string? ProjectComponent { get; set; }

    public double? Area { get; set; }

    public string? Unit { get; set; }

    public double? Coefficient { get; set; }

    public double? Weight { get; set; }

    public double? LaborCost { get; set; }

    public double? MaterialPrice { get; set; }

    public DateTime? InsDate { get; set; }

    public DateTime? UpsDate { get; set; }

    public virtual ICollection<QuotationLabor> QuotationLabors { get; set; } = new List<QuotationLabor>();

    public virtual ICollection<QuotationMaterial> QuotationMaterials { get; set; } = new List<QuotationMaterial>();

    public virtual QuotationSection QuotationSection { get; set; } = null!;
}
