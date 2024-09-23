using System;
using System.Collections.Generic;

namespace RHCQS_DataAccessObjects.Models;

public partial class PaymentType
{
    public Guid Id { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
