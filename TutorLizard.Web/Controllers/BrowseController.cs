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
        return View();
    }
}
