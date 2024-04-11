using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using TutorLizard.BusinessLogic.Extensions;
using TutorLizard.BusinessLogic.Interfaces.Data.Repositories;
using TutorLizard.BusinessLogic.Interfaces.Services;
using TutorLizard.BusinessLogic.Models;
using TutorLizard.BusinessLogic.Models.DTOs;
using TutorLizard.BusinessLogic.Models.DTOs.Requests;
using TutorLizard.BusinessLogic.Models.DTOs.Responses;

namespace TutorLizard.Web.Controllers;
public class TutorController : Controller
{
    private readonly ITutorService _tutorService;
    private readonly ICategoryRepository _categoryRepository;

    public TutorController(ITutorService tutorService, ICategoryRepository categoryRepository)
    {
        _tutorService = tutorService;
        _categoryRepository = categoryRepository;
    }
    public IActionResult Index()
    {
        return View();
    }

    public async Task<IActionResult> CreateAd()
    {
        // TODO - move obtaining categories to some service
        List<CategoryDto> categories = _categoryRepository
            .GetAllCategories()
            .Select(c => c.ToDto())
            .ToList();
        ViewBag.Categories = new SelectList(items: categories,
                                            dataValueField: nameof(Category.Id),
                                            dataTextField: nameof(Category.Name));

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateAd(CreateAdRequest request)
    {
        try
        {
            if(ModelState.IsValid == false)
                return View(request);

            var identity = User.Identity as ClaimsIdentity;

            string? nameIdentifier = identity?
                .Claims?
                .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?
                .Value;

            if(int.TryParse(nameIdentifier, out int tutorId) == false)
            {
                return View(request);
            }

            request.TutorId = tutorId;

            CreateAdResponse response = new()
            {
                SuccessfullyCreated = true,
                CreatedAdId = 1
            };

            if (response.SuccessfullyCreated)
            {
                // TODO show notification
            }
            else
            {
                // TODO show notification
            }
        }
        catch
        {
            return View(request);
        }

        return RedirectToAction(nameof(Index));
    }
}
