using System;
using System.Collections.Generic;

namespace RHCQS_DataAccessObjects.Models;

public partial class Message
{
    public Guid SenderId { get; set; }

    public string? Context { get; set; }

    public DateTime? SendAt { get; set; }

    public Guid RoomId { get; set; }
}
