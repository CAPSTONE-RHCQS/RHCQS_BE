using System;
using System.Collections.Generic;

namespace RHCQS_DataAccessObjects.Models;

public partial class Material
{
    public Guid Id { get; set; }

    public Guid SupplierId { get; set; }

    public Guid MaterialTypeId { get; set; }

    public string? Name { get; set; }

    public int? InventoryQuantity { get; set; }

    public double? Price { get; set; }

    public string? Unit { get; set; }

    public string? Size { get; set; }

    public string? Shape { get; set; }

    public string? ImgUrl { get; set; }

    public string? Description { get; set; }

    public DateTime? InsDate { get; set; }

    public DateTime? UpsDate { get; set; }

    public string? UnitPrice { get; set; }

    public bool? IsAvailable { get; set; }

    public virtual ICollection<MaterialSection> MaterialSections { get; set; } = new List<MaterialSection>();

    public virtual MaterialType MaterialType { get; set; } = null!;

    public virtual ICollection<QuotationMaterial> QuotationMaterials { get; set; } = new List<QuotationMaterial>();

    public virtual Supplier Supplier { get; set; } = null!;
}
