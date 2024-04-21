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
