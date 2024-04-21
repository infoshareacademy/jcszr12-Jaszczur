using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TutorLizard.BusinessLogic.Extensions;
using TutorLizard.BusinessLogic.Interfaces.Data.Repositories;
using TutorLizard.BusinessLogic.Interfaces.Services;
using TutorLizard.BusinessLogic.Models;
using TutorLizard.BusinessLogic.Models.DTOs;
using TutorLizard.BusinessLogic.Models.DTOs.Requests;
using TutorLizard.BusinessLogic.Models.DTOs.Responses;
using TutorLizard.Web.Interfaces.Services;

namespace TutorLizard.Web.Controllers;
[Authorize]
public class TutorController : Controller
{
    private readonly ITutorService _tutorService;
    private readonly IUserAuthenticationService _userAuthenticationService;
    private readonly ICategoryRepository _categoryRepository;
    private readonly INotificationService _notificationService;

    public TutorController(ITutorService tutorService,
                           IUserAuthenticationService userAuthenticationService,
                           ICategoryRepository categoryRepository,
                           INotificationService notificationService)
    {
        _tutorService = tutorService;
        _userAuthenticationService = userAuthenticationService;
        _categoryRepository = categoryRepository;
        _notificationService = notificationService;
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

            int? tutorId = _userAuthenticationService.GetLoggedInUserId();
            if(tutorId is null)
            {
                return View(request);
            }
            request.TutorId = (int)tutorId;

            CreateAdResponse response = new()
            {
                SuccessfullyCreated = true,
                CreatedAdId = 1
            };

            if (response.SuccessfullyCreated)
            {
                _notificationService.ShowSuccessNotification("Ogłoszenie zostało dodane");
                // TODO redirect to created Ad's details
            }
            else
            {
                _notificationService.ShowFailureNotification("Wystąpił błąd. Ogłoszenie nie zostało dodane.");
            }
        }
        catch
        {
            return View(request);
        }

        return RedirectToAction(nameof(Index));
    }
}
