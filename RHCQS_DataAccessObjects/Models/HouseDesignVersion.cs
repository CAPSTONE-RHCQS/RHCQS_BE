using System;
using System.Collections.Generic;

namespace RHCQS_DataAccessObjects.Models;

public partial class HouseDesignVersion
{
    public Guid Id { get; set; }

    public string? Name { get; set; }

    public double? Version { get; set; }

    public string? Status { get; set; }

    public DateTime? InsDate { get; set; }

    public Guid? HouseDesignDrawingId { get; set; }

    public string? UpVersion { get; set; }

    public string? Note { get; set; }

    public virtual HouseDesignDrawing? HouseDesignDrawing { get; set; }
}
