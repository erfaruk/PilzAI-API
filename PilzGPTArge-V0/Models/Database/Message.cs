using System;
using System.Collections.Generic;

namespace PilzGPTArge_V0.Models.Database;

public partial class Message
{
    public int Id { get; set; }

    public int? RoleId { get; set; }

    public string? MessageType { get; set; }

    public string? MessageContent { get; set; }

    public byte[]? MessageImage { get; set; }

    public string? ImageFormat { get; set; }

    public DateTime? SendDate { get; set; }

    public int? ChatId { get; set; }

    public int? ModelId { get; set; }

    public int? CompletionToken { get; set; }

    public int? PromptToken { get; set; }

    public int? TotalTokens { get; set; }

    public virtual Chat? Chat { get; set; }

    public virtual Model? Model { get; set; }

    public virtual Role? Role { get; set; }
}
