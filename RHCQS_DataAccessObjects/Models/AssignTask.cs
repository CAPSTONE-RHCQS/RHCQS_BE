using System;
using System.Collections.Generic;

namespace RHCQS_DataAccessObjects.Models;

public partial class AssignTask
{
    public Guid Id { get; set; }

    public Guid AccountId { get; set; }

    public string? Name { get; set; }

    public string? Status { get; set; }

    public DateTime? InsDate { get; set; }

    public virtual Account Account { get; set; } = null!;

    public virtual ICollection<HouseDesignDrawing> HouseDesignDrawings { get; set; } = new List<HouseDesignDrawing>();
}
