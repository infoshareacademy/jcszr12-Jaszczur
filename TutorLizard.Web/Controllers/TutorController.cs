using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TutorLizard.BusinessLogic.Extensions;
using TutorLizard.BusinessLogic.Interfaces.Data.Repositories;
using TutorLizard.BusinessLogic.Interfaces.Services;
using TutorLizard.BusinessLogic.Models;
using TutorLizard.Shared.Models.DTOs;
using TutorLizard.Shared.Models.DTOs.Requests;
using TutorLizard.Shared.Models.DTOs.Responses;
using TutorLizard.Web.Interfaces.Services;

namespace TutorLizard.Web.Controllers;
[Authorize]
public class TutorController : Controller
{
    private readonly ITutorService _tutorService;
    private readonly IUserAuthenticationService _userAuthenticationService;
    private readonly IDbRepository<Category> _categoryRepository;
    private readonly IUiMessagesService _uiMessagesService;

    public TutorController(ITutorService tutorService,
                           IUserAuthenticationService userAuthenticationService,
                           IDbRepository<Category> categoryRepository,
                           IUiMessagesService uiMessagesService)
    {
        _tutorService = tutorService;
        _userAuthenticationService = userAuthenticationService;
        _categoryRepository = categoryRepository;
        _uiMessagesService = uiMessagesService;
    }
    public IActionResult Index()
    {
        return View();
    }

    public async Task<IActionResult> CreateAd()
    {
        // TODO - move obtaining categories to some service
        await AddCategoriesToViewBag();

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

            CreateAdResponse response = await _tutorService.CrateAd(request);

            if (response.SuccessfullyCreated)
            {
                _uiMessagesService.ShowSuccessMessage("Ogłoszenie zostało dodane");
                return RedirectToAction("AdDetails", "Browse", new { id = response.CreatedAdId });
            }
            else
            {
                _uiMessagesService.ShowFailureMessage("Wystąpił błąd. Ogłoszenie nie zostało dodane.");
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

        return RedirectToAction("AdDetails", "Browse", new { id = adId });
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

            int? userId = _userAuthenticationService.GetLoggedInUserId();
            if (userId is null)
            {
                return RedirectToAction(nameof(Index));
            }
            request.UserId = (int)userId;
            CreateScheduleItemResponse response = await _tutorService.CreateScheduleItem(request);

            if (response.Success)
            {
                _uiMessagesService.ShowSuccessMessage("Utworzono termin");
            }
            else
            {
                _uiMessagesService.ShowFailureMessage("Wystąpił błąd. Tworzenie terminu nieudane.");
            }

            return RedirectToAction("AdDetails", "Browse", new { id = request.AdId });
        }
        catch
        {
            return View(request);
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AcceptScheduleItemRequest(int scheduleItemRequestId, int adId)
    {
        int? tutorId = _userAuthenticationService.GetLoggedInUserId();
        if (tutorId is null)
        {
            _uiMessagesService.ShowFailureMessage("Wystąpił bląd. Zgłoszenie na termin nie zostąło zaakceptowane.");
            return RedirectToAction(actionName: "AdDetails", controllerName: "Browse", routeValues: new { id = adId });
        }
        AcceptScheduleItemRequestRequest request = new()
        {
            ScheduleItemRequestId = scheduleItemRequestId,
            TutorId = (int)tutorId
        };

        AcceptScheduleItemRequestResponse response = await _tutorService.AcceptScheduleItemRequest(request);

        if (response.Success)
        {
            _uiMessagesService.ShowSuccessMessage("Zgłoszenie na termin zostało zaakceptowane.");
        }
        else
        {
            _uiMessagesService.ShowFailureMessage("Wystąpił bląd. Zgłoszenie na termin nie zostąło zaakceptowane.");
        }

        return RedirectToAction(actionName: "AdDetails", controllerName: "Browse", routeValues: new { id = adId });
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UnacceptScheduleItemRequest(int scheduleItemRequestId, int adId)
    {
        int? tutorId = _userAuthenticationService.GetLoggedInUserId();
        if (tutorId is null)
        {
            _uiMessagesService.ShowFailureMessage("Wystąpił błąd. Anulowanie akceptacji zgłoszenia na termin nieudane.");
            return RedirectToAction(actionName: "AdDetails", controllerName: "Browse", routeValues: new { id = adId });
        }
        UnacceptScheduleItemRequestRequest request = new()
        {
            ScheduleItemRequestId = scheduleItemRequestId,
            TutorId = (int)tutorId
        };

        UnacceptScheduleItemRequestResponse response = await _tutorService.UnacceptScheduleItemRequest(request);

        if (response.Success)
        {
            _uiMessagesService.ShowSuccessMessage("Akceptacja zgłoszenia na termin została anulowana.");
        }
        else
        {
            _uiMessagesService.ShowFailureMessage("Wystąpił błąd. Anulowanie akceptacji zgłoszenia na termin nieudane.");
        }

        return RedirectToAction(actionName: "AdDetails", controllerName: "Browse", routeValues: new { id = adId });
    }

    private async Task AddCategoriesToViewBag()
    {
        List<Category> categories = await _categoryRepository
                        .GetAll()
                        .ToListAsync();

        List<CategoryDto> categoryDtos = categories.Select(c => c.ToDto()).ToList();

        ViewBag.Categories = new SelectList(items: categoryDtos,
                                            dataValueField: nameof(CategoryDto.Id),
                                            dataTextField: nameof(CategoryDto.Name));
    }

    public async Task<IActionResult> ViewPendingAdRequests()
    {
        try
        {
            int? tutorId = _userAuthenticationService.GetLoggedInUserId();
            if (tutorId is null)
            {
                return Forbid();
            }

            GetTutorsPendingAdRequestsRequest request = new(tutorId);

            GetTutorsPendingAdRequestsResponse response = await _tutorService.GetTutorsPendingAdRequests(request);

            return View(response);
        }

        catch
        {
            return RedirectToAction("Error", "Home");
        }
    }

    [HttpPost]
    public async Task<IActionResult> UpdatePendingAdRequest(IFormCollection form, int adRequestId)
    {
        int? tutorId = _userAuthenticationService.GetLoggedInUserId();
        if (tutorId is null)
        {
            return Forbid();
        }

        UpdateAdRequestRequest request = new(adRequestId, form["replyMessage"]);

        try
        {
            if (!form["btnAccept"].IsNullOrEmpty())
                request.Action = UpdateAdRequestRequest.UpdateAction.Accept;
            else if (!form["btnReject"].IsNullOrEmpty())
                request.Action = UpdateAdRequestRequest.UpdateAction.Reject;

            var response = await _tutorService.UpdateAdRequest(request);
            if (!response.IsSuccessful)
            {
                if(request.Action == UpdateAdRequestRequest.UpdateAction.Accept)
                {
                    _uiMessagesService.ShowFailureMessage("Wystąpił błąd. Akceptacja zgłoszenia nieudana.");
                }
                else if (request.Action == UpdateAdRequestRequest.UpdateAction.Reject)
                {
                    _uiMessagesService.ShowFailureMessage("Wystąpił błąd. Odrzucenie zgłoszenia nieudane.");
                }
                RedirectToAction(nameof(AdRequest));
            }

            if(request.Action == UpdateAdRequestRequest.UpdateAction.Accept)
            {
                _uiMessagesService.ShowSuccessMessage("Ogłoszenie zaakceptowane.");
            }
            else if (request.Action == UpdateAdRequestRequest.UpdateAction.Reject)
            {
                _uiMessagesService.ShowSuccessMessage("Ogłoszenie odrzucone.");
            }

            return RedirectToAction("ViewPendingAdRequests");
        }
        catch
        {
            return RedirectToAction("Error", "Home");
        }
    }

    public async Task<IActionResult> ViewAllAdRequests()
    {
        try
        {
            int? tutorId = _userAuthenticationService.GetLoggedInUserId();
            if (tutorId is null)
            {
                return Forbid();
            }

            GetTutorsAllAdRequestsRequest request = new(tutorId);

            GetTutorsAllAdRequestsResponse response = await _tutorService.GetTutorsAllAdRequests(request);

            return View(response);
        }

        catch
        {
            return RedirectToAction("Error", "Home");
        }
    }

    public async Task<IActionResult> TutorsAdsList()
    {
        try
        {
            int? tutorId = _userAuthenticationService.GetLoggedInUserId();
            if (tutorId is null)
            {
                return Forbid();
            }

            GetTutorsAdsRequest request = new((int)tutorId);

            GetTutorsAdsResponse response = await _tutorService.GetTutorsAds(request);

            return View(response);
        }
        catch
        {
            return RedirectToAction("Error", "Home");
        }
    }
}
