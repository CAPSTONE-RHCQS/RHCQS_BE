using System;
using System.Collections.Generic;

namespace RHCQS_DataAccessObjects.Models;

public partial class AssignTask
{
    public Guid Id { get; set; }

    public Guid AccountId { get; set; }

    public Guid? ProjectId { get; set; }

    public DateTime? InsDate { get; set; }

    public virtual Account Account { get; set; } = null!;

    public virtual Project? Project { get; set; }
}
