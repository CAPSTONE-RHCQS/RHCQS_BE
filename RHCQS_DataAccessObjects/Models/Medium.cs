using System;
using System.Collections.Generic;

namespace RHCQS_DataAccessObjects.Models;

public partial class Medium
{
    public Guid Id { get; set; }

    public Guid HouseDesignDrawingId { get; set; }

    public string? Name { get; set; }

    public string? Url { get; set; }

    public DateTime? InsDate { get; set; }

    public DateTime? UpsDate { get; set; }

    public virtual HouseDesignDrawing HouseDesignDrawing { get; set; } = null!;
}
