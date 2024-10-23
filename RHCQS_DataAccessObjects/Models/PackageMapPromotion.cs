using System;
using System.Collections.Generic;

namespace RHCQS_DataAccessObjects.Models;

public partial class PackageMapPromotion
{
    public Guid Id { get; set; }

    public Guid PackageId { get; set; }

    public Guid PromotionId { get; set; }

    public DateTime? InsDate { get; set; }

    public virtual Package Package { get; set; } = null!;

    public virtual Promotion Promotion { get; set; } = null!;
}
