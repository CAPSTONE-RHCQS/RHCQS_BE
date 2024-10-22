using System;
using System.Collections.Generic;

namespace RHCQS_DataAccessObjects.Models;

public partial class Medium
{
    public Guid Id { get; set; }

    public Guid? HouseDesignVersionId { get; set; }

    public string? Name { get; set; }

    public string? Url { get; set; }

    public DateTime? InsDate { get; set; }

    public DateTime? UpsDate { get; set; }

    public Guid? SubTemplateId { get; set; }

    public Guid? PaymentId { get; set; }

    public Guid? DesignTemplateId { get; set; }

    public virtual DesignTemplate? DesignTemplate { get; set; }

    public virtual HouseDesignVersion? HouseDesignVersion { get; set; }

    public virtual Payment? Payment { get; set; }

    public virtual SubTemplate? SubTemplate { get; set; }
}
