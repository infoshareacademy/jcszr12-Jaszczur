using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.IdentityModel.Tokens;
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
    //private readonly IAdRequestRepository _adRequestRepository;

    public TutorController(ITutorService tutorService,
                           IUserAuthenticationService userAuthenticationService,
                           ICategoryRepository categoryRepository/*,
                           IAdRequestRepository adRequestRepository*/)
    {
        _tutorService = tutorService;
        _userAuthenticationService = userAuthenticationService;
        _categoryRepository = categoryRepository;
        //_adRequestRepository = adRequestRepository;
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

    public IActionResult ViewPendingAdRequests()
    {
        try
        {
            int? tutorId = _userAuthenticationService.GetLoggedInUserId();
            if (tutorId is null)
            {
                return RedirectToAction("AccessDenied", "User");
            }

            TutorsPendingAdRequestsRequest request = new(tutorId);

            TutorsPendingAdRequestsResponse response = new()
            {
                // The data is only for tests
                AdRequests = [new AdRequestsListDto(1, 1, 1, false, "message", "reply message", true),
                    new AdRequestsListDto(2, 22, 2, true, "message", "reply message", false)]
            };
            return View(response);
        }

        catch
        {
            return RedirectToAction("Error", "Home");
        }
    }
    [HttpPost]
    public IActionResult UpdatePendingAdRequest(IFormCollection buttonAction, int adRequestId)
    {
        int? tutorId = _userAuthenticationService.GetLoggedInUserId();
        if (tutorId is null)
        {
            return RedirectToAction("AccessDenied", "User");
        }

        UpdateTutorsPendingAdRequestRequest request = new(adRequestId);

        UpdateTutorsPendingAdRequestResponse response = new();

        try
        {
            if (!buttonAction["btnAccept"].IsNullOrEmpty())
            {
                // TODO: Move accept logic to a service
                // _adRequestRepository.GetAdRequestById(adRequestId).IsAccepted = true;
            }

            if (!buttonAction["btnReject"].IsNullOrEmpty())
            {
                // TODO: Move reject logic to a service
                // var result = _adRequestRepository.GetAdRequestById(adRequestId);
                // _adRequestRepository.GetAllAdRequests().Remove(result);
            }
            return RedirectToAction("ViewPendingAdRequests");
        }
        catch
        {
            return RedirectToAction("Error", "Home");
        }
    }
}
