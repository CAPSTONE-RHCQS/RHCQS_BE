using System;
using System.Collections.Generic;

namespace RHCQS_DataAccessObjects.Models;

public partial class ConstructionWorkResource
{
    public Guid Id { get; set; }

    public Guid? ConstructionWorkId { get; set; }

    public Guid? MaterialSectionId { get; set; }

    public double? MaterialSectionNorm { get; set; }

    public Guid? LaborId { get; set; }

    public double? LaborNorm { get; set; }

    public DateTime? InsDate { get; set; }

    public virtual ConstructionWork? ConstructionWork { get; set; }

    public virtual Labor? Labor { get; set; }

    public virtual MaterialSection? MaterialSection { get; set; }

    public virtual ICollection<WorkTemplate> WorkTemplates { get; set; } = new List<WorkTemplate>();
}
