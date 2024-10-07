using System;
using System.Collections.Generic;

namespace RHCQS_DataAccessObjects.Models;

public partial class UtilitiesSection
{
    public Guid Id { get; set; }

    public Guid UtilitiesId { get; set; }

    public string? Name { get; set; }

    public string? Status { get; set; }

    public DateTime? InsDate { get; set; }

    public DateTime? UpsDate { get; set; }

    public string? Description { get; set; }

    public virtual Utility Utilities { get; set; } = null!;

    public virtual ICollection<UtilitiesItem> UtilitiesItems { get; set; } = new List<UtilitiesItem>();
}
