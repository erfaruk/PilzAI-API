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

        [JsonPropertyName("tools")]
        public List<Tool> Tools { get; set; } = new List<Tool>();

        [JsonPropertyName("tool_resources")]
        public ToolResources ToolResources { get; set; } = new ToolResources();

        [JsonPropertyName("metadata")]
        public Dictionary<string, string> Metadata { get; set; } 

        [JsonPropertyName("temperature")]
        public double Temperature { get; set; } = 1.0;

        [JsonPropertyName("top_p")]
        public double TopP { get; set; } = 1.0;

        [JsonPropertyName("response_format")]
        public string ResponseFormat { get; set; } = "auto";
    }

    public class Tool
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("function")]
        public Function Function { get; set; }
    }

    public class Function
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("parameters")]
        public object Parameters { get; set; }
    }

    public class ToolResources
    {
        [JsonPropertyName("code_interpreter")]
        public CodeInterpreter CodeInterpreter { get; set; } = new CodeInterpreter();

        [JsonPropertyName("file_search")]
        public FileSearch FileSearch { get; set; } = new FileSearch();
    }

    public class CodeInterpreter
    {
        [JsonPropertyName("file_ids")]
        public List<string> FileIds { get; set; } = new List<string>();
    }

    public class FileSearch
    {
        [JsonPropertyName("vector_store_ids")]
        public List<string> VectorStoreIds { get; set; } = new List<string>();
    }
}
