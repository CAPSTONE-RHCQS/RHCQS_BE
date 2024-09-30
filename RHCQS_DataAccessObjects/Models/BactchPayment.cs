using System;
using System.Collections.Generic;

namespace RHCQS_DataAccessObjects.Models;

public partial class BactchPayment
{
    public Guid Id { get; set; }

    public Guid ContractId { get; set; }

    public double? Price { get; set; }

    public DateTime? PaymentDate { get; set; }

    public DateTime? PaymentPhase { get; set; }

    public Guid IntitialQuotationId { get; set; }

    public string? Percents { get; set; }

    public DateTime? InsDate { get; set; }

    public Guid FinalQuotationId { get; set; }

    public virtual Contract Contract { get; set; } = null!;

    public virtual FinalQuotation FinalQuotation { get; set; } = null!;

    public virtual InitialQuotation IntitialQuotation { get; set; } = null!;

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
