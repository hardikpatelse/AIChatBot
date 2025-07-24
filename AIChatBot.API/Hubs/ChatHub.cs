using Microsoft.AspNetCore.SignalR;

namespace AIChatBot.API.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendStatusUpdate(string connectionId, string status)
        {
            await Clients.Client(connectionId).SendAsync("ReceiveStatus", status);
        }

        public override async Task OnConnectedAsync()
        {
            Console.WriteLine($"SignalR Client connected: {Context.ConnectionId}");
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            Console.WriteLine($"SignalR Client disconnected: {Context.ConnectionId}");
            await base.OnDisconnectedAsync(exception);
        }
    }
}