using System;
using System.Collections.Generic;

namespace RHCQS_DataAccessObjects.Models;

public partial class UltilitiesSection
{
    public Guid Id { get; set; }

    public Guid UltilitiesId { get; set; }

    public string? Name { get; set; }

    public string? Status { get; set; }

    public DateTime? InsDate { get; set; }

    public DateTime? UpsDate { get; set; }

    public virtual Ultility Ultilities { get; set; } = null!;

    public virtual ICollection<UltilitiesItem> UltilitiesItems { get; set; } = new List<UltilitiesItem>();
}
