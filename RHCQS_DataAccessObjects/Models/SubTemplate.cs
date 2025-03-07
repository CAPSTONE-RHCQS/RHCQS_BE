﻿using System;
using System.Collections.Generic;

namespace RHCQS_DataAccessObjects.Models;

public partial class SubTemplate
{
    public Guid Id { get; set; }

    public Guid DesignTemplateId { get; set; }

    public double BuildingArea { get; set; }

    public double? FloorArea { get; set; }

    public DateTime? InsDate { get; set; }

    public string Size { get; set; } = null!;

    public string? ImgUrl { get; set; }

    public double TotalRough { get; set; }

    public virtual DesignTemplate DesignTemplate { get; set; } = null!;

    public virtual ICollection<Medium> Media { get; set; } = new List<Medium>();

    public virtual ICollection<TemplateItem> TemplateItems { get; set; } = new List<TemplateItem>();
}
