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

namespace TutorLizard.Web.Controllers;
[Authorize]
public class TutorController : Controller
{
    private readonly ITutorService _tutorService;
    private readonly IUserAuthenticationService _userAuthenticationService;
    private readonly ICategoryRepository _categoryRepository;

    public TutorController(ITutorService tutorService,
                           IUserAuthenticationService userAuthenticationService,
                           ICategoryRepository categoryRepository)
    {
        _tutorService = tutorService;
        _userAuthenticationService = userAuthenticationService;
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
            if (ModelState.IsValid == false)
                return View(request);

            int? tutorId = _userAuthenticationService.GetLoggedInUserId();
            if (tutorId is null)
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
                // TODO show notification
                // TODO redirect to created Ad's details
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

    public async Task<IActionResult> CreateScheduleItem(int id)
    {
        int adId = id;
        int? userId = _userAuthenticationService.GetLoggedInUserId();
        if (userId is null)
        {
            return RedirectToAction(nameof(Index));
        }
        IsUserTheAdOwnerRequest request = new IsUserTheAdOwnerRequest(adId, userId);
        IsUserTheAdOwnerResponse response = await _tutorService.IsUserTheAdOwner(request);

        if (response.IsOwner)
        {
            CreateScheduleItemRequest model = new CreateScheduleItemRequest
            {
                AdId = adId
            };

            return View(model);
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateScheduleItem(CreateScheduleItemRequest request)
    {
        try
        {
            if (ModelState.IsValid == false)
            {
                return View(request);
            }

            int? userId = request.UserId;
            if (userId is null)
            {
                return RedirectToAction(nameof(Index));
            }
            CreateScheduleItemResponse response = new()
            {
                Success = true,
                CreatedItemId = 1,
            };

            if (response.Success)
            {
                // TODO
                return RedirectToAction(nameof(Index));
            }
            else
            {
                // TODO
                return RedirectToAction(nameof(Index));
            }
        }
        catch
        {
            return View(request);
        }

        return RedirectToAction(nameof(Index));
    }
}
