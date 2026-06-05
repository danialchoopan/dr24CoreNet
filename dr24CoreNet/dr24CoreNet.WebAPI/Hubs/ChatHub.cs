using Microsoft.AspNetCore.SignalR;

namespace dr24CoreNet.WebAPI.Hubs;

public class ChatHub : Hub
{
    public async Task SendMessage(string appointmentId, string user, string message)
    {
        // Broadcast to specific appointment group
        await Clients.Group(appointmentId).SendAsync("ReceiveMessage", user, message);
    }

    public async Task JoinChat(string appointmentId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, appointmentId);
    }
}
