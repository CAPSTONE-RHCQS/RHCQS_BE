using System;
using System.Collections.Generic;

namespace RHCQS_DataAccessObjects.Models;

public partial class Blog
{
    public Guid Id { get; set; }

    public Guid AccountId { get; set; }

    public string? Heading { get; set; }

    public string? SubHeading { get; set; }

    public string? Context { get; set; }

    public string? ImgUrl { get; set; }

    public DateTime? InsDate { get; set; }

    public DateTime? UpsDate { get; set; }

    public virtual Account Account { get; set; } = null!;
}
