using System;
using System.Collections.Generic;

namespace RHCQS_DataAccessObjects.Models;

public partial class Ultility
{
    public Guid Id { get; set; }

    public string? Name { get; set; }

    public string? Type { get; set; }

    public string? Status { get; set; }

    public DateTime? InsDate { get; set; }

    public DateTime? UpsDate { get; set; }

    public virtual ICollection<UltilitiesSection> UltilitiesSections { get; set; } = new List<UltilitiesSection>();
}
