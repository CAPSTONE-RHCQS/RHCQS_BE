using System;
using System.Collections.Generic;

namespace RHCQS_DataAccessObjects.Models;

public partial class Room
{
    public Guid Id { get; set; }

    public Guid? CustomeId { get; set; }

    public Guid? SaleId { get; set; }

    public DateTime? InsDate { get; set; }

    public virtual Account? Custome { get; set; }

    public virtual Account? Sale { get; set; }
}
