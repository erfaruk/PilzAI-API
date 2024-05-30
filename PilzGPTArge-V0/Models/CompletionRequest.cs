using Azure.Core;
using System.Text.Json.Serialization;

namespace PilzGPTArge_V0.Models
{
    public class CompletionRequest
    {
        public string model { get; set; }
        public List<RequestMessage> messages { get; set; }
    }

    public class RequestMessage
    {
        [JsonPropertyName("role")]
        public string role { get; set; }

        [JsonPropertyName("content")]
        [JsonConverter(typeof(ContentListJsonConverter))]
        public List<IContent> content { get; set; }
    }

    public interface IContent { }

    public class TextContent : IContent
    {
        [JsonPropertyName("type")]
        public string type { get; set; } = "text";

        [JsonPropertyName("text")]
        public string text { get; set; }
    }

    public class ImageContent : IContent
    {
        [JsonPropertyName("type")]
        public string type { get; set; } = "image_url";

        [JsonPropertyName("image_url")]
        public ImageUrl image_url { get; set; }
    }

    public class ImageUrl
    {
        [JsonPropertyName("url")]
        public string url { get; set; }
    }

}
