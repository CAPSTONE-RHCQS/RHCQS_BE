using System;
using System.Collections.Generic;

namespace RHCQS_DataAccessObjects.Models;

public partial class FinalQuotation
{
    public Guid Id { get; set; }

    public Guid ProjectId { get; set; }

    public Guid? PromotionId { get; set; }

    public double? TotalPrice { get; set; }

    public string? Note { get; set; }

    public double? Version { get; set; }

    public DateTime? InsDate { get; set; }

    public DateTime? UpsDate { get; set; }

    public string? Status { get; set; }

    public bool? Deflag { get; set; }

    public string? ReasonReject { get; set; }

    public bool? IsDraft { get; set; }

    public virtual ICollection<BatchPayment> BatchPayments { get; set; } = new List<BatchPayment>();

    public virtual ICollection<EquipmentItem> EquipmentItems { get; set; } = new List<EquipmentItem>();

    public virtual ICollection<FinalQuotationItem> FinalQuotationItems { get; set; } = new List<FinalQuotationItem>();

    public virtual ICollection<Medium> Media { get; set; } = new List<Medium>();

    public virtual Project Project { get; set; } = null!;

    public virtual Promotion? Promotion { get; set; }

    public virtual ICollection<QuotationUtility> QuotationUtilities { get; set; } = new List<QuotationUtility>();
}
