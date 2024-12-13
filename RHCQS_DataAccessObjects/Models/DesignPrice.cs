using System;
using System.Collections.Generic;

namespace RHCQS_DataAccessObjects.Models;

public partial class DesignPrice
{
    public Guid Id { get; set; }

    public double? AreaFrom { get; set; }

    public double? AreaTo { get; set; }

    public double? Price { get; set; }

    public DateTime? InsDate { get; set; }

    public DateTime? UpsDate { get; set; }

    public virtual ICollection<Project> Projects { get; set; } = new List<Project>();
}
