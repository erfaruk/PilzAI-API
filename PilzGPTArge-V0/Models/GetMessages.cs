namespace PilzGPTArge_V0.Models
{
    public class GetMessages
    {
        public int MessageId { get; set; }

        public int? ChatId { get; set; }

        public int? RoleId { get; set; }

        public string? RoleName { get; set; }

        public string? Type { get; set; }

        public string? MessageContent { get; set; }

        public string? MessageImage { get; set; }

        public string? ImageType { get; set; }

        public DateTime? SendDate { get; set; }

    }
}
