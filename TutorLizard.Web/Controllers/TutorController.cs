using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
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
    private readonly IDbRepository<Category> _categoryRepository;

    public TutorController(ITutorService tutorService,
                           IUserAuthenticationService userAuthenticationService,
                           IDbRepository<Category> categoryRepository)
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

            int? userId = _userAuthenticationService.GetLoggedInUserId();
            if (userId is null)
            {
                return RedirectToAction(nameof(Index));
            }
            request.UserId = (int)userId;
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
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AcceptScheduleItemRequest(int scheduleItemRequestId, int adId)
    {

        int? tutorId = _userAuthenticationService.GetLoggedInUserId();
        if (tutorId is null)
        {
            // TODO - show failure notification
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
            // TODO - show success notification

        }
        else
        {
            // TODO - show failure notification

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
            // TODO - show failure notification
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
            // TODO - show success notification

        }
        else
        {
            // TODO - show failure notification

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
                return RedirectToAction("AccessDenied", "User");
            }

            TutorsPendingAdRequestsRequest request = new(tutorId);

            TutorsPendingAdRequestsResponse response = await _tutorService.ViewAllPendingAdRequests(request);

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
            return RedirectToAction("AccessDenied", "User");
        }

        UpdateTutorsPendingAdRequestRequest request = new(adRequestId, form["replyMessage"]);

        try
        {
            if (!form["btnAccept"].IsNullOrEmpty())
                request.Action = UpdateTutorsPendingAdRequestRequest.UpdateAction.Accept;
            else if (!form["btnReject"].IsNullOrEmpty())
                request.Action = UpdateTutorsPendingAdRequestRequest.UpdateAction.Reject;

            var response = await _tutorService.UpdateAdRequest(request);
            if (!response.IsSuccessful)
                RedirectToAction(nameof(AdRequest));

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
                return RedirectToAction("AccessDenied", "User");
            }

            TutorAllAdRequestsRequest request = new(tutorId);

            TutorAllAdRequestsResponse response = await _tutorService.ViewAllAdRequests(request);

            return View(response);
        }

        catch
        {
            return RedirectToAction("Error", "Home");
        }
    }

    public IActionResult TutorsAdsList()
    {
        try
        {
            int? tutorId = _userAuthenticationService.GetLoggedInUserId();
            if (tutorId is null)
            {
                return RedirectToAction("AccessDenied", "User");
            }

            TutorsAdsRequest request = new(tutorId);

            TutorsAdsResponse response = new()
            {
                // TODO inject actual data, this is only for tests

                AdList = new List<AdListItemDto>
                {
                    new AdListItemDto(
                        id: 1,
                        tutorId: 101,
                        tutorName: "Anna",
                        subject: "Matematyka",
                        title: "Korepetycje z matematyki",
                        description: "Lekcje matematyki dla uczniów szkół średnich.",
                        categoryId: 2,
                        categoryName: "Mathematics",
                        price: 50.0m,
                        location: "Warszawa",
                        isRemote: true
                    ),
                    new AdListItemDto(
                        id: 2,
                        tutorId: 102,
                        tutorName: "Piotr",
                        subject: "Fizyka",
                        title: "Korepetycje z fizyki",
                        description: "Lekcje fizyki dla uczniów szkół średnich.",
                        categoryId: 3,
                        categoryName: "Physics",
                        price: 60.0m,
                        location: "Kraków",
                        isRemote: false
                    ),
                    new AdListItemDto(
                        id: 3,
                        tutorId: 103,
                        tutorName: "Michał",
                        subject: "Chemia",
                        title: "Korepetycje z chemii",
                        description: "Lekcje chemii dla uczniów szkół średnich.",
                        categoryId: 4,
                        categoryName: "Chemistry",
                        price: 70.0m,
                        location: "Gdańsk",
                        isRemote: true
                    )
                }
            };
            return View(response);
        }
        catch
        {
            return RedirectToAction("AccessDenied", "User");
        }
    }



}
