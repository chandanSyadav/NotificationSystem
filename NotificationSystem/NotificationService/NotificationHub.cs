﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace NotificationSystem.NotificationService
{
    public class NotificationHub : Hub
    {
        private static readonly Dictionary<string, string> ConnectedUsers = new();

        [Authorize]
        public override async Task OnConnectedAsync()
        {
            var userId = Context.UserIdentifier; // Based on NameIdentifier claim
            var userName = Context.User?.Identity?.Name;
            var userRole = Context.User?.FindFirst(ClaimTypes.Role)?.Value;

            if (!string.IsNullOrEmpty(userId))
            {
                ConnectedUsers[userId] = Context.ConnectionId;
                if (!string.IsNullOrEmpty(userRole))
                {
                    await Groups.AddToGroupAsync(Context.ConnectionId, userRole);
                }
            }

            UserConnectionManager.AddConnection(userId, Context.ConnectionId);

            await base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            var userId = Context.UserIdentifier;
            if (!string.IsNullOrEmpty(userId))
            {
                ConnectedUsers.Remove(userId);
            }

            UserConnectionManager.RemoveConnection(userId, Context.ConnectionId);

            return base.OnDisconnectedAsync(exception);
        }
    }
}
