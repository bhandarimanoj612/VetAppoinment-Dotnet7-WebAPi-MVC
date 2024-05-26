using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace VetAppoinment.Hubs
{
   
    public class ChatHub : Hub
    {
        public async Task SendMessage(string senderUsername, string receiverUsername, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", senderUsername, receiverUsername, message);
        }
    }
}
