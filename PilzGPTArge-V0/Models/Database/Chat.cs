using System;
using System.Collections.Generic;

namespace PilzGPTArge_V0.Models.Database;

public partial class Chat
{
    public int Id { get; set; }

    public string? ChatTitle { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? ChangedDate { get; set; }

    public virtual ICollection<Message> Messages { get; set; } = new List<Message>();
}
