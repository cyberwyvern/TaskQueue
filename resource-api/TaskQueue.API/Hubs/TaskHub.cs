using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;
using TaskQueue.API.Util;

namespace TaskQueue.API.Hubs
{
    [Authorize]
    public class TaskHub : Hub<ITaskClient>
    {
        public override async Task OnConnectedAsync()
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, Context.User.GetId().ToString());
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, Context.User.GetId().ToString());
            await base.OnDisconnectedAsync(exception);
        }
    }
}