using System;
using System.Collections.Generic;

namespace RHCQS_DataAccessObjects.Models;

public partial class UltilitiesItem
{
    public Guid Id { get; set; }

    public Guid SectionId { get; set; }

    public string? Name { get; set; }

    public double? Coefficient { get; set; }

    public string? Description { get; set; }

    public DateTime? InsDate { get; set; }

    public DateTime? UpsDate { get; set; }

    public virtual ICollection<QuoationUltity> QuoationUltities { get; set; } = new List<QuoationUltity>();

    public virtual UltilitiesSection Section { get; set; } = null!;
}
