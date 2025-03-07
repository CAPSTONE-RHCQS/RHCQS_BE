﻿using System;
using System.Collections.Generic;

namespace RHCQS_DataAccessObjects.Models;

public partial class Contract
{
    public Guid Id { get; set; }

    public Guid ProjectId { get; set; }

    public Guid? ContractAppendix { get; set; }

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

    public double? RoughPackagePrice { get; set; }

    public double? FinishedPackagePrice { get; set; }

    public string Status { get; set; } = null!;

    public string Type { get; set; } = null!;

    public DateTime? InsDate { get; set; }

    public virtual ICollection<BatchPayment> BatchPayments { get; set; } = new List<BatchPayment>();

    public virtual Project Project { get; set; } = null!;
}
