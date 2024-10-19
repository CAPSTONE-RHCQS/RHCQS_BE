using System;
using System.Collections.Generic;

namespace RHCQS_DataAccessObjects.Models;

public partial class Payment
{
    public Guid Id { get; set; }

    public Guid PaymentTypeId { get; set; }

    public Guid BatchPaymentId { get; set; }

    public string? Status { get; set; }

    public DateTime? InsDate { get; set; }

    public DateTime? UpsDate { get; set; }

    public double? TotalPrice { get; set; }

    public virtual BatchPayment BatchPayment { get; set; } = null!;

    public virtual ICollection<Medium> Media { get; set; } = new List<Medium>();

    public virtual PaymentType PaymentType { get; set; } = null!;
}
