using Microsoft.AspNetCore.Mvc;
using TutorLizard.BusinessLogic.Extensions;
using TutorLizard.BusinessLogic.Interfaces.Services;
using TutorLizard.BusinessLogic.Models;
using TutorLizard.BusinessLogic.Models.DTOs;

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

    public IActionResult Ads(int page = 1)
    {
        // TODO ask service for ads to show
        // TODO add pagination

        List<AdDto> ads = [
            new Ad(1, 1, "temat", "tytuł", "opis", 1, 10, "Warszawa", true).ToDto(),
            new Ad(2, 1, "temat", "tytuł", "opis", 2, 10, "Kraków", true).ToDto(),
            new Ad(3, 1, "temat", "tytuł", "opis", 1, 10, "Gdańsk", true).ToDto(),
            new Ad(4, 1, "temat", "tytuł", "opis", 2, 10, "Kosmos", true).ToDto(),
            new Ad(5, 1, "temat", "tytuł", "opis", 1, 10, "Wieś", true).ToDto(),
            ];

        return View(ads);
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
