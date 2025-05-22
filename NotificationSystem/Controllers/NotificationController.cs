using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using NotificationSystem.Data;
using NotificationSystem.Models;
using NotificationSystem.NotificationService;

namespace NotificationSystem.Controllers
{
    [Authorize]
    public class NotificationController : Controller
    {
        private readonly IHubContext<NotificationHub> _notificationHub;
        private readonly OfficeCanteenDBContext _canteenContext;

        public NotificationController(IHubContext<NotificationHub> notificationHub)
        {
            _notificationHub = notificationHub;
        }

        public IActionResult SendNotificationsForm()
        {
            return View();
        }

        public async Task<IActionResult> SendNotification(string message, string target, string targetValue)
        {
            if (string.IsNullOrWhiteSpace(message))
                return View();

            if (target == "all")
            {
                await _notificationHub.Clients.All.SendAsync("ReceiveNotification", message);
            }
            else if (target == "user" && !string.IsNullOrWhiteSpace(targetValue))
            {
                await _notificationHub.Clients.User(targetValue).SendAsync("ReceiveNotification", message);
            }
            else if (target == "role" && !string.IsNullOrWhiteSpace(targetValue))
            {
                await _notificationHub.Clients.Group(targetValue).SendAsync("ReceiveNotification", message);
            }

            TempData["Sent"] = "Notification sent successfully!";
            return RedirectToAction("SendNotificationsForm");
        }
    }
}