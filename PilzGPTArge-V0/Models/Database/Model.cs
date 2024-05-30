using System;
using System.Collections.Generic;

namespace PilzGPTArge_V0.Models.Database;

public partial class Model
{
    public int Id { get; set; }

    public string? ModelName { get; set; }

    public virtual ICollection<Message> Messages { get; set; } = new List<Message>();
}
