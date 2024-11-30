using System;
using System.Collections.Generic;

namespace RHCQS_DataAccessObjects.Models;

public partial class ConstructionWork
{
    public Guid Id { get; set; }

    public string? WorkName { get; set; }

    public Guid? ConstructionId { get; set; }

    public DateTime? InsDate { get; set; }

    public string? Unit { get; set; }

    public string? Code { get; set; }

    public virtual ConstructionItem? Construction { get; set; }

    public virtual ICollection<ConstructionWorkResource> ConstructionWorkResources { get; set; } = new List<ConstructionWorkResource>();

    public virtual ICollection<WorkTemplate> WorkTemplates { get; set; } = new List<WorkTemplate>();
}
