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

    public string? Type { get; set; }

    public double? TotalPrice { get; set; }

    public virtual BactchPayment BatchPayment { get; set; } = null!;

    public virtual PaymentType PaymentType { get; set; } = null!;
}
