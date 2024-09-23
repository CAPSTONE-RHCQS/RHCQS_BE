using System;
using System.Collections.Generic;

namespace RHCQS_DataAccessObjects.Models;

public partial class PackageType
{
    public Guid Id { get; set; }

    public string? Name { get; set; }

    public DateTime? InsDate { get; set; }

    public virtual ICollection<Package> Packages { get; set; } = new List<Package>();
}
