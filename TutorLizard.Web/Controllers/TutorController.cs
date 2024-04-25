using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
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
    public IActionResult UpdatePendingAdRequest(IFormCollection form, int adRequestId)
    {
        int? tutorId = _userAuthenticationService.GetLoggedInUserId();
        if (tutorId is null)
        {
            return RedirectToAction("AccessDenied", "User");
        }

        UpdateTutorsPendingAdRequestRequest request = new(adRequestId, form["replyMessage"]);

        UpdateTutorsPendingAdRequestResponse response = new();

        try
        {
            if (!form["btnAccept"].IsNullOrEmpty())
            {
                // TODO: Move accept logic to a service
                // _adRequestRepository.GetAdRequestById(adRequestId).IsAccepted = true;
            }

            if (!form["btnReject"].IsNullOrEmpty())
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
