using System;
using System.Collections.Generic;

namespace RHCQS_DataAccessObjects.Models;

public partial class Utility
{
    public Guid Id { get; set; }

    public string? Name { get; set; }

    public string? Type { get; set; }

    public string? Status { get; set; }

    public DateTime? InsDate { get; set; }

    public DateTime? UpsDate { get; set; }

    public virtual ICollection<UtilitiesSection> UtilitiesSections { get; set; } = new List<UtilitiesSection>();
}
