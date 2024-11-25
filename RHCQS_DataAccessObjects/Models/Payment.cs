using System;
using System.Collections.Generic;

namespace RHCQS_DataAccessObjects.Models;

public partial class Payment
{
    public Guid Id { get; set; }

    public Guid PaymentTypeId { get; set; }

    public DateTime? InsDate { get; set; }

    public DateTime? UpsDate { get; set; }

    public double? TotalPrice { get; set; }

    public DateTime? PaymentDate { get; set; }

    public DateTime? PaymentPhase { get; set; }

    public string? Unit { get; set; }

    public int? Percents { get; set; }

    public string? Description { get; set; }

    public bool? IsConfirm { get; set; }

    public int? Priority { get; set; }

    public virtual ICollection<BatchPayment> BatchPayments { get; set; } = new List<BatchPayment>();

    public virtual ICollection<Medium> Media { get; set; } = new List<Medium>();

    public virtual PaymentType PaymentType { get; set; } = null!;
}
