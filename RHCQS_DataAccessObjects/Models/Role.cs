using System;
using System.Collections.Generic;

namespace RHCQS_DataAccessObjects.Models;

public partial class Role
{
    public Guid Id { get; set; }

    public string? RoleName { get; set; }

    public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();
}
