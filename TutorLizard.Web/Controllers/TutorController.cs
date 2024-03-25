using Microsoft.AspNetCore.Mvc;

namespace TutorLizard.Web.Controllers;
public class TutorController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
