using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TutorLizard.BusinessLogic.Interfaces.Services;
using TutorLizard.Shared.Models.DTOs;
using TutorLizard.Shared.Models.DTOs.Requests;
using TutorLizard.Shared.Models.DTOs.Responses;
using TutorLizard.Web.Interfaces.Services;
using TutorLizard.Blazor.Models;
using TutorLizard.Web.Extensions;
using TutorLizard.Blazor.Extensions;

namespace TutorLizard.Web.Controllers;
public class BrowseController : Controller
{
    private readonly IBrowseService _browseService;
    private readonly IUserAuthenticationService _userAuthenticationService;
    private readonly IUiMessagesService _uiMessagesService;
    private readonly ICategoryService _categoryService;
    private readonly int _pageSize;

    public BrowseController(IBrowseService browseService,
                            IUserAuthenticationService userAuthenticationService,
                            IUiMessagesService uiMessagesService,
                            ICategoryService categoryService)
    {
        _browseService = browseService;
        _userAuthenticationService = userAuthenticationService;
        _uiMessagesService = uiMessagesService;
        _categoryService = categoryService;
        _pageSize = 10;
    }
    public IActionResult Index()
    {
        return RedirectToAction(nameof(Ads));
    }

    public async Task<IActionResult> Ads(int id = 1, [FromQuery] string? search = "")
    {
        // TODO customize routing, so that parameter is page, not id
        int pageNumber = id > 0 ? id : 1;
        int pageSize = _pageSize > 0 ? _pageSize : 1;

        AdSearchCriteriaDto? searchCriteria = search?.ToAdSearchCriteriaDto();
        searchCriteria ??= new();

        GetBrowseAdsPageRequest request = new(pageNumber, pageSize, searchCriteria);

        GetBrowseAdsPageResponse response = await _browseService.GetBrowseAdsPage(request);

        if (response.Success == false)
        {
            _uiMessagesService.ShowFailureMessage("Wystąpił błąd. Nie udało się się załadować ogłoszeń.");
            return RedirectToAction("Index", "Home");
        }

        await AddCategoriesToViewBag();

        return View(nameof(Ads), response);
    }

    [HttpPost]
    public IActionResult Search([FromForm] string searchCriteria)
    {
        var model = searchCriteria.DeserializeAdSearchCriteriaViewModel();
        string search = model?.ToDto().ToBase64String() ?? "";

        return RedirectToAction(nameof(Ads), "Browse", new { search });
    }

    [Authorize]
    public async Task<IActionResult> AdDetails(int id)
    {
        var userId = _userAuthenticationService.GetLoggedInUserId();
        if (userId is null)
        {
            return RedirectToAction(nameof(Ads));
        }

        GetAdDetailsRequest request = new()
        {
            AdId = id,
            UserId = (int)userId,
        };

        GetAdDetailsResponse? response = await _browseService.GetAdDetails(request);

        if (response is null)
        {
            return RedirectToAction(nameof(Ads));
        }

        return View(response);
    }

    [Authorize]
    public async Task<IActionResult> Schedule([FromQuery] int year, [FromQuery] int month)
    {
        if (year == 0)
        {
            year = DateTime.Now.Year;
        }
        if (month < 1 || month > 12)
        {
            month = DateTime.Now.Month;
        }

        int? userId = _userAuthenticationService.GetLoggedInUserId();
        if (userId is null)
        {
            return RedirectToAction(nameof(Index));
        }

        GetUsersScheduleRequest request = new()
        {
            UserId = (int)userId,
            Month = month,
            Year = year
        };

        GetUsersScheduleResponse response = await _browseService.GetUsersSchedule(request);
        return View(response);
    }

    private async Task AddCategoriesToViewBag()
    {
        GetCategoriesRequest request = new();
        GetCategoriesResponse response = await _categoryService.GetCategories(request);

        ViewBag.Categories = response.Categories;
    }
}
