using System;
using System.Collections.Generic;

namespace PilzGPTArge_V0.Models.Database;

public partial class Role
{
    public int Id { get; set; }

    public string? Role1 { get; set; }

    public virtual ICollection<Message> Messages { get; set; } = new List<Message>();
}
