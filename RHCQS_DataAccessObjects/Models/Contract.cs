using System;
using System.Collections.Generic;

namespace RHCQS_DataAccessObjects.Models;

public partial class Contract
{
    public Guid Id { get; set; }

    public Guid ProjectId { get; set; }

    public string? Name { get; set; }

    public string? CustomerName { get; set; }

    public string? ContractCode { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public int? ValidityPeriod { get; set; }

    public string? TaxCode { get; set; }

    public double? Area { get; set; }

    public string? UnitPrice { get; set; }

    public double? ContractValue { get; set; }

    public string? UrlFile { get; set; }

    public string? Note { get; set; }

    public bool? Deflag { get; set; }

    public virtual ICollection<BactchPayment> BactchPayments { get; set; } = new List<BactchPayment>();

    public virtual Project Project { get; set; } = null!;
}
