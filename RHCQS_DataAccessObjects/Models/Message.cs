using System;
using System.Collections.Generic;

namespace RHCQS_DataAccessObjects.Models;

public partial class Message
{
    public Guid Id { get; set; }

    public string? MessageContent { get; set; }

    public DateTime? SendAt { get; set; }

    public Guid RoomId { get; set; }

    public Guid? CreatedBy { get; set; }

    public virtual Account? CreatedByNavigation { get; set; }

    public virtual Room Room { get; set; } = null!;
}
