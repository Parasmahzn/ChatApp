using Microsoft.AspNetCore.SignalR;

namespace ChatApp.Server.Hubs
{
    public class ChatHub : Hub
    {
        private static Dictionary<string, string> Users = new();
        public override async Task OnConnectedAsync()
        {
            string username = Context.GetHttpContext().Request
                .Query["username"];
            Users.Add(Context.ConnectionId, username);

            await AddMessageToChat(string.Empty, $"{username} has Entered!");
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            string username = Users.FirstOrDefault(u => u.Key == Context.ConnectionId).Value;
            await AddMessageToChat(String.Empty, $"{username} has Left!");
        }

        public async Task AddMessageToChat(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}
