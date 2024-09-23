using System;
using System.Collections.Generic;

namespace RHCQS_DataAccessObjects.Models;

public partial class PackageHouse
{
    public Guid Id { get; set; }

    public Guid PackageId { get; set; }

    public Guid DesignTemplateId { get; set; }

    public string? ImgUrl { get; set; }

    public DateTime? InsDate { get; set; }

    public virtual DesignTemplate DesignTemplate { get; set; } = null!;

    public virtual Package Package { get; set; } = null!;
}
