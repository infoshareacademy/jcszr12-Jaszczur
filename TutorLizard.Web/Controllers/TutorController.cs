using Microsoft.AspNetCore.Mvc;
using TutorLizard.BusinessLogic.Services;

namespace TutorLizard.Web.Controllers;
public class TutorController : Controller
{
    private readonly ITutorService _tutorService;

    public TutorController(ITutorService tutorService)
    {
        _tutorService = tutorService;
    }
    public IActionResult Index()
    {
        return View();
    }
}
