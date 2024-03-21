using Microsoft.AspNetCore.Mvc;
using TutorLizard.BusinessLogic.Interfaces.Services;

namespace TutorLizard.Web.Controllers;
public class BrowseController : Controller
{
    private readonly IBrowseService _browseService;

    public BrowseController(IBrowseService browseService)
    {
        _browseService = browseService;
    }
    public IActionResult Index()
    {
        return RedirectToAction(nameof(Ads));
    }

    public IActionResult Ads()
    {
        // TODO ask service for ads to show
        // TODO add pagination
        return View();
    }

    public IActionResult AdDetails(int id)
    {
        // TODO ask service for ad to show based on id
        return View();
    }

    public IActionResult Schedule()
    {
        // TODO get userId from HttpContext and ask service for ScheduleItems based on it
        return View();
    }
}
