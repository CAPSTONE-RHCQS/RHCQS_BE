using System;
using System.Collections.Generic;

namespace RHCQS_DataAccessObjects.Models;

public partial class Project
{
    public Guid Id { get; set; }

    public Guid? CustomerId { get; set; }

    public string? Name { get; set; }

    public string? Type { get; set; }

    public string? Status { get; set; }

    public DateTime? InsDate { get; set; }

    public DateTime? UpsDate { get; set; }

    public string? ProjectCode { get; set; }

    public string? Address { get; set; }

    public double? Area { get; set; }

    public bool? IsDrawing { get; set; }

    public string? CustomerName { get; set; }

    public virtual ICollection<AssignTask> AssignTasks { get; set; } = new List<AssignTask>();

    public virtual ICollection<Contract> Contracts { get; set; } = new List<Contract>();

    public virtual Customer? Customer { get; set; }

    public virtual ICollection<FinalQuotation> FinalQuotations { get; set; } = new List<FinalQuotation>();

    public virtual ICollection<HouseDesignDrawing> HouseDesignDrawings { get; set; } = new List<HouseDesignDrawing>();

    public virtual ICollection<InitialQuotation> InitialQuotations { get; set; } = new List<InitialQuotation>();
}
