using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NotificationSystem.Models;
using NotificationSystem.NotificationService;

namespace NotificationSystem.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    [Authorize]
    public IActionResult Index()
    {
        return View();
    }


    public IActionResult TotalConnections()
    {
        var users = UserConnectionManager.GetConnectedUserCount();

        return View("TotalConnections",  users);
    }

    public IActionResult GetConnectionCount()
    {
        var users = UserConnectionManager.GetConnectedUserCount();

        return PartialView("TotalConnections", users);
    }

    [Authorize]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()    
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
