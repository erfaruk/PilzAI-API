using System.Text.Json.Serialization;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PilzGPTArge_V0.Models.Assistant
{
    public class CreateAssistant
    {
        [JsonPropertyName("model")]
        public string Model { get; set; } = "gpt-4o";

        [JsonPropertyName("name")]
        public string Name { get; set; } = "";

        [JsonPropertyName("description")]
        public string Description { get; set; } = "";

        [JsonPropertyName("instructions")]
        public string Instructions { get; set; } = "";

        [JsonPropertyName("temperature")]
        public double Temperature { get; set; } = 0.7;

        [JsonPropertyName("top_p")]
        public double TopP { get; set; } = 1.0;

        [JsonPropertyName("response_format")]
        public string ResponseFormat { get; set; } = "auto";
    }


}
