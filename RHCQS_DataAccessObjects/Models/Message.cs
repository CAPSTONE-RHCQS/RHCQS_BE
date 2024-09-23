using System;
using System.Collections.Generic;

namespace RHCQS_DataAccessObjects.Models;

public partial class Message
{
    public Guid Id { get; set; }

    public string? Sender { get; set; }

    public string? Reciver { get; set; }

    public string? Context { get; set; }

    public DateTime? Time { get; set; }
}
