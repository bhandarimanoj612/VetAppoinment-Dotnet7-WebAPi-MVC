using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using VetAppoinment.Hubs;
using VetAppoinment.Models.Chat;
using VetAppoinment.Models.Entities;
using VetAppoinment.Models.Interfaces;

namespace VetAppoinment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private static List<Message> _messages = new List<Message>();
        private readonly IAuthService _authService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHubContext<ChatHub> _hubContext;
        public MessageController(UserManager<ApplicationUser> userManager,IAuthService authService, IHubContext<ChatHub> hubContext)
        {
            _authService = authService;
            _hubContext = hubContext;
            _userManager = userManager;
        }

        [HttpGet("{receiverUsername}")]
        public IActionResult GetMessages(string receiverUsername)
        {
            var messages = _messages.Where(m => m.ReceiverUsername == receiverUsername).ToList();
            return Ok(messages);
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage(Message message)
        {
            // Check if sender and receiver usernames exist
            var senderExists = await _authService.GetUserDetailsByUserNameAsync(message.SenderUsername) != null;
            var receiverExists = await _authService.GetUserDetailsByUserNameAsync(message.ReceiverUsername) != null;

            if (!senderExists || !receiverExists)
            {
                return BadRequest("Sender or receiver username not found");
            }

            message.Timestamp = DateTime.UtcNow;
            _messages.Add(message);
            return Ok("Message sent successfully");
        }

        [HttpPost,Route("SendMessageByRealTime")]
        public async Task<IActionResult> SendMessageByRealTime(Message message)
        {
            // Check if sender and receiver usernames exist
            var senderExists = await _userManager.FindByNameAsync(message.SenderUsername) != null;
            var receiverExists = await _userManager.FindByNameAsync(message.ReceiverUsername) != null;

            if (!senderExists || !receiverExists)
            {
                return BadRequest("Sender or receiver username not found");
            }

            message.Timestamp = DateTime.UtcNow;
            _messages.Add(message);

            // Send the message to SignalR clients
            await _hubContext.Clients.All.SendAsync("ReceiveMessage", message.SenderUsername, message.ReceiverUsername, message.Content);

            return Ok("Message sent successfully");
        }

    }
}
