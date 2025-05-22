using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using NotificationSystem.Data;
using NotificationSystem.Models;
using NotificationSystem.NotificationService;
using NotificationSystem.ViewModels;
using System.Security.Claims;

namespace NotificationSystem.Controllers
{
    [Authorize]
    public class FoodOrderController : Controller
    {
        private readonly OfficeCanteenDBContext _canteenContext;
        private readonly UserManager<IdentityUser> _userContext;
        private readonly IHubContext<NotificationHub> _notificationHub;


        public FoodOrderController(OfficeCanteenDBContext canteenContext, UserManager<IdentityUser> userContext, IHubContext<NotificationHub> notificationHub)
        {
            _canteenContext = canteenContext;
            _userContext = userContext;
            _notificationHub = notificationHub;
        }

        public IActionResult PlaceOrderForm()
        {
            return View();
        }

        public IActionResult FoodOrderList()
        {
            var list = _canteenContext.FoodOrders;

            var viewModel = new List<FoodOrderListViewModel>();
            foreach (var item in list)
            {
                FoodOrderListViewModel data = new()
                {
                    FoodDetails = item,
                    UserName = _userContext.FindByIdAsync(item.UserId).Result.Email
                };

                viewModel.Add(data);
            }
            return View(viewModel);
        }
        public async Task<IActionResult> PlaceOrder(string ItemName, int Quantity, string Target, string TargetValue)
        {
            var order = new FoodOrder()
            {
                ItemName = ItemName,
                Quantity = Quantity,
                UserId = User.FindFirstValue(ClaimTypes.NameIdentifier),
                OrderedAt = DateTime.Now,
                Status = "Pending",
            };

            _canteenContext.FoodOrders.Add(order);
            await _canteenContext.SaveChangesAsync();

            if (order.Id != 0)
            {
                if (Target == "all")
                {
                    await _notificationHub.Clients.All.SendAsync("ReceiveNotification", $"{order.ItemName} x{order.Quantity} ordered!");
                }
                else if (Target == "user" && !string.IsNullOrWhiteSpace(TargetValue))
                {
                    await _notificationHub.Clients.User(TargetValue).SendAsync("ReceiveNotification", $"{order.ItemName} x{order.Quantity} ordered!");
                }
                else if (Target == "role" && !string.IsNullOrWhiteSpace(TargetValue))
                {
                    await _notificationHub.Clients.Group(TargetValue).SendAsync("ReceiveNotification", $"{order.ItemName} x{order.Quantity} ordered!");
                }
            }

            return RedirectToAction("PlaceOrderForm");
        }
    }
}
