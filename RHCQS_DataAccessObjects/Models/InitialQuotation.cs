﻿using System;
using System.Collections.Generic;

namespace RHCQS_DataAccessObjects.Models;

public partial class InitialQuotation
{
    public Guid Id { get; set; }

    public Guid AccountId { get; set; }

    public Guid ProjectId { get; set; }

    public Guid PromotionId { get; set; }

    public Guid PackageId { get; set; }

    public Guid? QuotationUtilitiesId { get; set; }

    public double? Area { get; set; }

    public DateTime? TimeProcessing { get; set; }

    public DateTime? TimeRough { get; set; }

    public DateTime? TimeOthers { get; set; }

    public string? OthersAgreement { get; set; }

    public DateTime? InsDate { get; set; }

    public string? Status { get; set; }

    public string? Version { get; set; }

    public bool? IsTemplate { get; set; }

    public bool? Deflag { get; set; }

    public virtual Account Account { get; set; } = null!;

    public virtual ICollection<BactchPayment> BactchPayments { get; set; } = new List<BactchPayment>();

    public virtual Package Package { get; set; } = null!;

    public virtual Project Project { get; set; } = null!;

    public virtual Promotion Promotion { get; set; } = null!;

    public virtual QuoationUltity? QuotationUtilities { get; set; }
}
