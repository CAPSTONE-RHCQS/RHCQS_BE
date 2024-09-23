using System;
using System.Collections.Generic;

namespace RHCQS_DataAccessObjects.Models;

public partial class MaterialType
{
    public Guid Id { get; set; }

    public string? Name { get; set; }

    public DateTime? InsDate { get; set; }

    public DateTime? UpsDate { get; set; }

    public bool? Deflag { get; set; }

    public virtual ICollection<Material> Materials { get; set; } = new List<Material>();
}
