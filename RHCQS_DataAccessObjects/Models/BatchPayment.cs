using System;
using System.Collections.Generic;

namespace RHCQS_DataAccessObjects.Models;

public partial class BatchPayment
{
    public Guid Id { get; set; }

    public Guid? ContractId { get; set; }

    public Guid? InitialQuotationId { get; set; }

    public DateTime? InsDate { get; set; }

    public Guid? FinalQuotationId { get; set; }

    public Guid PaymentId { get; set; }

    public string Status { get; set; } = null!;

    public int NumberOfBatch { get; set; }

    public virtual Contract? Contract { get; set; }

    public virtual FinalQuotation? FinalQuotation { get; set; }

    public virtual InitialQuotation? InitialQuotation { get; set; }

    public virtual Payment Payment { get; set; } = null!;
}
