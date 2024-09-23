using System;
using System.Collections.Generic;

namespace RHCQS_DataAccessObjects.Models;

public partial class Supplier
{
    public Guid Id { get; set; }

    public string? Name { get; set; }

    public string? Email { get; set; }

    public string? ConstractPhone { get; set; }

    public string? ImgUrl { get; set; }

    public DateTime? InsDate { get; set; }

    public DateTime? UpsDate { get; set; }

    public bool? Deflag { get; set; }

    public string? ShortDescription { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<Material> Materials { get; set; } = new List<Material>();
}
