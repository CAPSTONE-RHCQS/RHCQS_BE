using System;
using System.Collections.Generic;

namespace RHCQS_DataAccessObjects.Models;

public partial class Promotion
{
    public Guid Id { get; set; }

    public string? Code { get; set; }

    public Guid? AccountId { get; set; }

    public int? Value { get; set; }

    public DateTime? AvailableTime { get; set; }

    public DateTime? InsDate { get; set; }

    public DateTime? UpsDate { get; set; }

    public virtual ICollection<FinalQuotation> FinalQuotations { get; set; } = new List<FinalQuotation>();
}
