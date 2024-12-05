using System;
using System.Collections.Generic;

namespace RHCQS_DataAccessObjects.Models;

public partial class HouseDesignVersion
{
    public Guid Id { get; set; }

    public string? Name { get; set; }

    public double Version { get; set; }

    public DateTime? InsDate { get; set; }

    public Guid HouseDesignDrawingId { get; set; }

    public string? Note { get; set; }

    public DateTime? UpsDate { get; set; }

    public Guid? RelatedDrawingId { get; set; }

    public Guid? PreviousDrawingId { get; set; }

    public string? Reason { get; set; }

    public bool Deflag { get; set; }

    public bool? Confirmed { get; set; }

    public virtual HouseDesignDrawing HouseDesignDrawing { get; set; } = null!;

    public virtual ICollection<Medium> Media { get; set; } = new List<Medium>();
}
