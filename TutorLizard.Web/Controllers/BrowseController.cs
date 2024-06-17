using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TutorLizard.BusinessLogic.Interfaces.Services;
using TutorLizard.BusinessLogic.Models.DTOs;
using TutorLizard.BusinessLogic.Models.DTOs.Requests;
using TutorLizard.BusinessLogic.Models.DTOs.Responses;
using TutorLizard.Web.Interfaces.Services;
using TutorLizard.Web.Models;

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
        _pageSize = 2;
    }
    public IActionResult Index()
    {
        return RedirectToAction(nameof(Ads));
    }

    public async Task<IActionResult> Ads(int id = 1)
    {
        // TODO customize routing, so that parameter is page, not id
        int pageNumber = id > 0 ? id : 1;
        int pageSize = _pageSize > 0 ? _pageSize : 1;
        GetBrowseAdsPageRequest request = new(pageNumber, pageSize, new());
        return await HandleGetBrowseAdsPageRequest(request, pageNumber);
    }

    [HttpPost]
    public async Task<IActionResult> Page([FromForm] GoToAdsPageViewModel model, int id = 1)
    {
        int pageNumber = id > 0 ? id : 1;
        int pageSize = _pageSize > 0 ? _pageSize : 1;
        GetBrowseAdsPageRequest request = new(pageNumber, pageSize, model.SearchCriteria);
        return await HandleGetBrowseAdsPageRequest(request, pageNumber);
    }

    [HttpPost]
    public async Task<IActionResult> Search([FromForm] AdSearchCriteriaDto searchCriteria)
    {
        int pageNumber = 1;
        int pageSize = _pageSize > 0 ? _pageSize : 1;
        GetBrowseAdsPageRequest request = new(pageNumber, pageSize, searchCriteria);
        return await HandleGetBrowseAdsPageRequest(request, pageNumber);
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
    public async Task<IActionResult> Schedule()
    {
        int? userId = _userAuthenticationService.GetLoggedInUserId();
        if (userId is null)
        {
            return RedirectToAction(nameof(Index));
        }

        GetUsersScheduleRequest request = new()
        {
            UserId = (int)userId
        };

        GetUsersScheduleResponse response = await _browseService.GetUsersSchedule(request);
        return View(response);
    }

    private async Task<IActionResult> HandleGetBrowseAdsPageRequest(GetBrowseAdsPageRequest request, int pageNumber)
    {
        GetBrowseAdsPageResponse response = await _browseService.GetBrowseAdsPage(request);

        if (response.Success == false)
        {
            _uiMessagesService.ShowFailureMessage("Wystąpił błąd. Nie udało się się załadować ogłoszeń.");
            return RedirectToAction("Index", "Home");
        }

        await AddCategoriesToViewBag();

        return View(nameof(Ads), response);
    }

    private async Task AddCategoriesToViewBag()
    {
        GetCategoriesRequest request = new();
        GetCategoriesResponse response = await _categoryService.GetCategories(request);

        ViewBag.Categories = new SelectList(items: response.Categories,
                                            dataValueField: nameof(CategoryDto.Id),
                                            dataTextField: nameof(CategoryDto.Name));
    }
}
