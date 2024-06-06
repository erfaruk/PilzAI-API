using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using PilzGPTArge_V0.Helper;
using PilzGPTArge_V0.Models;
using PilzGPTArge_V0.Models.Database;

namespace PilzGPTArge_V0.Controllers
{
    [EnableCors]
    [ApiController]
    [Route("[controller]")]
    public class ChatsController : Controller
    {
        [HttpGet]
        [Route("[action]")]
        public IActionResult Chats()
        {
            try
            {
                using var context = new PilzGptContext();
                var chats = context.Chats.Select(c => new
                {
                    c.Id,
                    c.ChatTitle,
                    c.CreatedDate,
                    c.ChangedDate,

                }).ToList();

                return Ok(chats);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

        }


        [HttpGet]
        [Route("[action]")]
        public IActionResult ChatMessages(int chatId)
        {
            try
            {
                using var context = new PilzGptContext();
                var chat = context.Chats.Find(chatId);
                if (chat != null)
                {
                    //var messagess = chat.Messages;
                    var messages = (from m in context.Messages
                                    join r in context.Roles on m.RoleId equals r.Id
                                    where m.ChatId == chatId
                                    orderby m.SendDate
                                    select new GetMessages
                                    {
                                        MessageId = m.Id,
                                        ChatId = m.ChatId,
                                        RoleId = m.RoleId,
                                        RoleName = r.Role1 != null ? r.Role1.Trim() : null,
                                        Type = m.MessageType != null ? m.MessageType.Trim() : null,
                                        MessageContent = m.MessageContent,
                                        MessageImage = ImageHelper.ByteToString(m.MessageImage),
                                        ImageType = m.ImageFormat,
                                        SendDate = m.SendDate,
                                    }).ToList();
                    return Ok(messages);
                }
                else
                {
                    return NotFound(new { Message = "Chat not found" });
                }
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

        }

        [HttpPost]
        [Route("[action]")]
        public int AddChat(string chatName, DateTime creaDate)
        {
            try
            {
                using var context = new PilzGptContext();
                var chat = new Chat
                {
                    ChatTitle = chatName,
                    CreatedDate = creaDate,
                };
                context.Chats.Add(chat);
                context.SaveChanges();
                return chat.Id;
            }

            catch
            {
                throw new Exception("Internal Server Error");
            }

        }

        [HttpPost]
        [Route("[action]")]
        public IActionResult AutoAddChat(string message)
        {
            using var context = new PilzGptContext();                               //SQL DBcontext oluşturulur
            string createdTitle = CreateTtitle.GenerateSummaryTitle(message);       //gönderilen mesajdan başlık üretir
            int addedChatId = AddChat(createdTitle, DateTime.Now);                  //Yeni chat sql tablosuna eklenir
                                                                                    //Chat Eklendikten sonra send edilen mesajı Tablya ekleme
            AddMessagetoChat(message, addedChatId);
            return Ok(addedChatId);
        }

        [HttpPost]
        [Route("[action]")]
        public IActionResult AutoAddChatwMeida([FromBody] string image)
        {
            using var context = new PilzGptContext();                                               //SQL DBcontext oluşturulur
            string createdTitle = CreateTtitle.GenerateSummaryTitle("Fotoğraf Özetleme İsteği");    //gönderilen mesajdan başlık üretir
            int addedChatId = AddChat(createdTitle, DateTime.Now);                                  //Yeni chat sql tablosuna eklenir
                                                                                                    //Chat Eklendikten sonra send edilen mesajı Tablya ekleme
            AddImageChatDirect(image , addedChatId );
            return Ok(addedChatId);
        }

        [HttpPost]
        [Route("[action]")]
        public IActionResult AddMessagetoChat(string message, int chatId)
        {
            try
            {
                using var context = new PilzGptContext();
                var sendMessage = new Message
                {
                    ChatId = chatId,
                    RoleId = 1,
                    MessageType = "text",
                    MessageContent = message,
                    SendDate = DateTime.Now,
                    ModelId = 1,
                    PromptToken = null,
                    CompletionToken = null,
                    TotalTokens = null,
                };
                context.Messages.Add(sendMessage);
                context.SaveChanges();
                return Ok();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }


        }

        [HttpPost]
        [Route("[action]")]
        public IActionResult AddImageChat([FromBody] string image, int chatId)
        {
            try
            {
                using var context = new PilzGptContext();
                var sendMessage = new Message
                {
                    ChatId = chatId,
                    RoleId = 1,
                    MessageType="image_url",
                    ImageFormat = ImageHelper.GetImageFormat(ImageHelper.StringToByte(image)),
                    MessageImage = ImageHelper.StringToByte(image),
                    SendDate = DateTime.Now,
                    ModelId = 1,
                    PromptToken = null,
                    CompletionToken = null,
                    TotalTokens = null,
                };
                context.Messages.Add(sendMessage);
                context.SaveChanges();
                return Ok();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }


        }

        [HttpPost]
        [Route("[action]")]
        public IActionResult AddImageChatDirect(string image, int chatId)
        {
            try
            {
                using var context = new PilzGptContext();
                var sendMessage = new Message
                {
                    ChatId = chatId,
                    RoleId = 1,
                    MessageType = "image_url",
                    ImageFormat = ImageHelper.GetImageFormat(ImageHelper.StringToByte(image)),
                    MessageImage = ImageHelper.StringToByte(image),
                    SendDate = DateTime.Now,
                    ModelId = 1,
                    PromptToken = null,
                    CompletionToken = null,
                    TotalTokens = null,
                };
                context.Messages.Add(sendMessage);
                context.SaveChanges();
                return Ok();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }


        }


    }
}
