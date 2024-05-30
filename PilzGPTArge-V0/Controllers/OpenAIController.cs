﻿using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using PilzGPTArge_V0.Services;
using System.Threading.Tasks;
using PilzGPTArge_V0.Controllers;
using PilzGPTArge_V0.Models.Database;
using PilzGPTArge_V0.Models;
using System.Text.Json;
using PilzGPTArge_V0.Helper;

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
                return BadRequest("Invalid message format.");
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
