using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using PilzGPTArge_V0.Services;
using System.Threading.Tasks;
using PilzGPTArge_V0.Controllers;
using PilzGPTArge_V0.Models.Database;
using PilzGPTArge_V0.Models;
using System.Text.Json;
using PilzGPTArge_V0.Helper;
using PilzGPTArge_V0.Models.Assistant;

[EnableCors]
[Route("api/[controller]")]
[ApiController]
public class OpenAIController : ControllerBase
{
    private readonly OpenAIService _openAIService;

    public OpenAIController(OpenAIService openAIService)
    {
        _openAIService = openAIService;
    }

    [HttpPost("create-assistant")]
    public async Task<IActionResult> CreateAssistant([FromBody] CreateAssistant request)
    {
        if (request == null || string.IsNullOrEmpty(request.Model) || string.IsNullOrEmpty(request.Name))
        {
            return BadRequest("Model and Name are required fields.");
        }

        var result = await _openAIService.CreateAssistantAsync(request);
        return Ok(result);
    }

    [HttpPost("list-assistants")]
    public async Task<IActionResult> ListAssistants([FromBody] ListAssistants request)
    {
        if (request == null)
        {
            return BadRequest("Request body is required.");
        }

        try
        {
            // Construct the query string from the request body
            string queryString = $"order={request.Order}&limit={request.Limit}";

            // Call the service method with the constructed query string
            var result = await _openAIService.ListAssistantsAsync(queryString);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpDelete("delete-assistant")]
    public async Task<IActionResult> DeleteAssistant([FromBody] string assistantId)
    {
        if (string.IsNullOrEmpty(assistantId))
        {
            return BadRequest("Assistant ID is required.");
        }

        try
        {
            var result = await _openAIService.DeleteAssistantAsync(assistantId);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }




    [HttpGet("getCompletion")]
    public async Task<IActionResult> GetCompletion(int chatid)
    {

        // ChatsController'ı kullanarak ChatMessages metodunu çağır
        ChatsController getChats = new ChatsController();
        var result = getChats.ChatMessages(chatid);

        // IActionResult sonucunu işle
        if (result is OkObjectResult okResult)
        {
            var messages = okResult.Value as List<GetMessages>; // Dönüş türüne göre uygun türde değişken kullanın.
            if (messages == null)
            {
                return BadRequest("No such a message");
            }

            var requestMessages = new List<RequestMessage>();
            foreach (var item in messages)
            {
                IContent content;
                if (item.Type == "text")
                {
                    content = new TextContent { text = item.MessageContent };
                }
                else if (item.Type == "image_url")
                {
                    // URL'yi uygun formatta oluşturma
                    content = new ImageContent {  image_url = new ImageUrl { url = item.MessageImage } };
                }
                else
                {
                    return BadRequest("Unsupported content type.");
                }
                requestMessages.Add(new RequestMessage
                {
                    role = item.RoleName,
                    content = new List<IContent> { content }
                });
            }


            var openAIRequest = new CompletionRequest
            {
                model = "gpt-4o",
                messages = requestMessages
            };

            using var context = new PilzGptContext(); // SQL DB context oluşturulur
            var resultFromOpenAI = await _openAIService.GetChatCompletionAsync(openAIRequest); // API'dan dönen mesaj
            var response = JsonSerializer.Deserialize<CompletionResponse>(resultFromOpenAI); // Json body deserialize etme
            if (response != null)
            {
                foreach (var choice in response.Choices)
                {
                    var chatResponse = new Message
                    {
                        ChatId = chatid,
                        RoleId = 2,
                        MessageType = "text",
                        MessageContent = choice.Message.Content,
                        SendDate = DateTime.Now,
                        ModelId = 1,
                        PromptToken = response.Usage.PromptTokens,
                        CompletionToken = response.Usage.CompletionTokens,
                        TotalTokens = response.Usage.TotalTokens,
                    };
                    context.Messages.Add(chatResponse);
                }
                context.SaveChanges();
            }
            var resultWithChatId = new
            {
                ChatId = chatid,
                CompletionResult = JsonSerializer.Deserialize<object>(resultFromOpenAI)
            };
            return Ok(resultWithChatId);
        }
        else if (result is NotFoundObjectResult notFoundResult)
        {
            return NotFound(notFoundResult.Value);
        }

        return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
    }



}
