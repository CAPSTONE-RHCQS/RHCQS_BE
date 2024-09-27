using System;
using System.Collections.Generic;

namespace RHCQS_DataAccessObjects.Models;

public partial class Package
{
    public Guid Id { get; set; }

    public Guid PackageTypeId { get; set; }

    public string? PackageName { get; set; }

    public string? Unit { get; set; }

    public double? Price { get; set; }

    public string? Status { get; set; }

    public DateTime? InsDate { get; set; }

    public DateTime? UpsDate { get; set; }

    public virtual ICollection<InitialQuotation> InitialQuotations { get; set; } = new List<InitialQuotation>();

    public virtual ICollection<PackageDetail> PackageDetails { get; set; } = new List<PackageDetail>();

    public virtual ICollection<PackageHouse> PackageHouses { get; set; } = new List<PackageHouse>();

    public virtual ICollection<PackageQuotation> PackageQuotations { get; set; } = new List<PackageQuotation>();

    public virtual PackageType PackageType { get; set; } = null!;

    public virtual ICollection<QuotationSection> QuotationSections { get; set; } = new List<QuotationSection>();
}
