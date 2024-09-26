using System;
using System.Collections.Generic;

namespace RHCQS_DataAccessObjects.Models;

public partial class Project
{
    public Guid Id { get; set; }

    public Guid? AccountId { get; set; }

    public string? Name { get; set; }

    public string? Type { get; set; }

    public string? Status { get; set; }

    public DateTime? InsDate { get; set; }

    public DateTime? UpsDate { get; set; }

    public string? ProjectCode { get; set; }

    public virtual Account? Account { get; set; }

    public virtual ICollection<AssignTask> AssignTasks { get; set; } = new List<AssignTask>();

    public virtual ICollection<Contract> Contracts { get; set; } = new List<Contract>();

    public virtual ICollection<DetailedQuotation> DetailedQuotations { get; set; } = new List<DetailedQuotation>();

    public virtual ICollection<InitialQuotation> InitialQuotations { get; set; } = new List<InitialQuotation>();
}
