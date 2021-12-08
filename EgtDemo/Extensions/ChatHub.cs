using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EgtDemo.Extensions
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user + "says", message);
        }

        public async Task AddToGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

            await Clients.Group(groupName).SendAsync("ReceiveMessage", $"{Context.ConnectionId} has joined the group", $"{groupName}.");
        }

        public Task SendMessageToGroup(string groupName, string message)
        {
            return Clients.Group(groupName).SendAsync("ReceiveMessage", $"{Context.ConnectionId}", $"{message}");
        }


        public async Task RemoveFromGroup(string groupName)
        {
            await Clients.Group(groupName).SendAsync("ReceiveMessage", $"{Context.ConnectionId} has left the group", $"{groupName}.");
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
        }

        public Task SendPrivateMessage(string user, string message)
        {
            return Clients.User(user).SendAsync("ReceiveMessage", user, message);
        }
    }
}
