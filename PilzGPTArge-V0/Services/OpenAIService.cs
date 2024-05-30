using PilzGPTArge_V0.Models;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace PilzGPTArge_V0.Services
{
    public class OpenAIService
    {
        private readonly HttpClient _client;

        public OpenAIService(HttpClient client)
        {
            _client = client;
        }

        public async Task<string> GetChatCompletionAsync(CompletionRequest messages)
        {
            foreach (var msg in messages.messages)
            {
                foreach (var contents in msg.content)
                {
                    if (contents is TextContent textContent)
                    {
                        Console.WriteLine($"Role: {msg.role}, Content: {textContent.text}");
                    }
                    else if (contents is ImageContent imageContent)
                    {
                        Console.WriteLine($"Role: {msg.role}, Content: {imageContent.image_url}");
                    }
                }
            }

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
            request.Headers.Add("Authorization", "Bearer sk-proj-hFCuOx3cuO8O9ckOqiMoT3BlbkFJyrj8jMbaIPbC9PVM1G3l");
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            request.Content = content;

            var response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();


            return await response.Content.ReadAsStringAsync();
        }
    }
}
