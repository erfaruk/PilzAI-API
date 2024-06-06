using PilzGPTArge_V0.Models;
using PilzGPTArge_V0.Models.Assistant;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace PilzGPTArge_V0.Services
{
    public class OpenAIService
    {
        private readonly HttpClient _client;

        private readonly string apiKey = "sk-proj-hFCuOx3cuO8O9ckOqiMoT3BlbkFJyrj8jMbaIPbC9PVM1G3l";

        public OpenAIService(HttpClient client)
        {
            _client = client;
        }

        public async Task<string> GetChatCompletionAsync(CompletionRequest messages)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true,
                Converters = { new ContentListJsonConverter() },
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };

            var jsonContent = JsonSerializer.Serialize(messages, options);

            Console.WriteLine(jsonContent); // Verify the JSON content //Fonksiyona giren mesajı JSON çevirme
            var request = new HttpRequestMessage(HttpMethod.Post, "https://api.openai.com/v1/chat/completions");
            request.Headers.Add("Authorization", $"Bearer {apiKey}");
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            request.Content = content;

            var response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();


            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> CreateAssistantAsync(CreateAssistant request)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true,
                Converters = { new ContentListJsonConverter() },
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };

            var jsonContent = JsonSerializer.Serialize(request, options);
            var httpRequest = new HttpRequestMessage(HttpMethod.Post, "https://api.openai.com/v1/assistants");
            httpRequest.Headers.Add("Authorization", $"Bearer {apiKey}");
            httpRequest.Headers.Add("OpenAI-Beta", "assistants=v2");
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            httpRequest.Content = content;

            var response = await _client.SendAsync(httpRequest);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> ListAssistantsAsync(string queryString)
        {
            var url = $"https://api.openai.com/v1/assistants?{queryString}";
            var httpRequest = new HttpRequestMessage(HttpMethod.Get, url);
            httpRequest.Headers.Add("Authorization", $"Bearer {apiKey}");
            httpRequest.Headers.Add("OpenAI-Beta", "assistants=v2");

            var response = await _client.SendAsync(httpRequest);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }
    }
}
