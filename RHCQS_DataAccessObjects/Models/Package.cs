using System;
using System.Collections.Generic;

namespace RHCQS_DataAccessObjects.Models;

public partial class Package
{
    public Guid Id { get; set; }

    public string? PackageName { get; set; }

    public string? Unit { get; set; }

    public double? Price { get; set; }

    public string? Status { get; set; }

    public DateTime? InsDate { get; set; }

    public DateTime? UpsDate { get; set; }

    public string? Type { get; set; }

    public virtual ICollection<PackageHouse> PackageHouses { get; set; } = new List<PackageHouse>();

    public virtual ICollection<PackageLabor> PackageLabors { get; set; } = new List<PackageLabor>();

    public virtual ICollection<PackageMapPromotion> PackageMapPromotions { get; set; } = new List<PackageMapPromotion>();

    public virtual ICollection<PackageMaterial> PackageMaterials { get; set; } = new List<PackageMaterial>();

    public virtual ICollection<PackageQuotation> PackageQuotations { get; set; } = new List<PackageQuotation>();

    public virtual ICollection<WorkTemplate> WorkTemplates { get; set; } = new List<WorkTemplate>();
}
