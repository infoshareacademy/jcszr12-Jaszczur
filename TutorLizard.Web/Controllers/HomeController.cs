using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Globalization;
using TutorLizard.Web.Models;

namespace TutorLizard.Web.Controllers;
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult Data()
    {
        return View();
    }
    public IActionResult AdminPanel()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
    public IActionResult ChangeLanguage(string language, string returnUrl)
    {
        if (!string.IsNullOrEmpty(language))
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(language);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(language);
        }
        else
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en");
        }

        Response.Cookies.Append("Language", language);

        if (returnUrl is null)
            return RedirectToAction("Index", "Home");
        
        return Redirect(returnUrl);
    }
}
