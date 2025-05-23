﻿using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace NotificationSystem.NotificationService
{
    public class NameIdentifierUserIdProvider : IUserIdProvider
    {
        public string GetUserId(HubConnectionContext connection)
        {
            return connection.User?.FindFirst(ClaimTypes.Email)?.Value;
        }
    }
}
