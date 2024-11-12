using System;
using System.Collections.Generic;

namespace RHCQS_DataAccessObjects.Models;

public partial class Room
{
    public Guid Id { get; set; }

    public Guid? SenderId { get; set; }

    public Guid? ReceiverId { get; set; }

    public DateTime? InsDate { get; set; }

    public virtual ICollection<Message> Messages { get; set; } = new List<Message>();

    public virtual Account? Receiver { get; set; }

    public virtual Account? Sender { get; set; }
}
