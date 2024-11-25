using System;
using System.Collections.Generic;

namespace RHCQS_DataAccessObjects.Models;

public partial class MaterialSection
{
    public Guid Id { get; set; }

    public string? Name { get; set; }

    public DateTime? InsDate { get; set; }

    public string Code { get; set; } = null!;

    public string? Type { get; set; }

    public virtual ICollection<Material> Materials { get; set; } = new List<Material>();
}
