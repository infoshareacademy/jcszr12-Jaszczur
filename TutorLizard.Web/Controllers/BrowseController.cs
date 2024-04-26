using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TutorLizard.BusinessLogic.Enums;
using TutorLizard.BusinessLogic.Interfaces.Services;
using TutorLizard.BusinessLogic.Models.DTOs;
using TutorLizard.BusinessLogic.Models.DTOs.Requests;
using TutorLizard.BusinessLogic.Models.DTOs.Responses;

namespace TutorLizard.Web.Controllers;
public class BrowseController : Controller
{
    private readonly IBrowseService _browseService;
    private readonly IUserAuthenticationService _userAuthenticationService;
    private readonly int _pageSize;

    public BrowseController(IBrowseService browseService, IUserAuthenticationService userAuthenticationService)
    {
        _browseService = browseService;
        _userAuthenticationService = userAuthenticationService;
        _pageSize = 10;
    }
    public IActionResult Index()
    {
        return RedirectToAction(nameof(Ads));
    }

    public async Task<IActionResult> Ads(int id = 1)
    {
        // TODO ask service for ads to show (using request)

        // TODO customize routing, so that parameter is page, not id
        int pageNumber = id;
        GetBrowseAdsPageRequest request = new(pageNumber, _pageSize);

        GetBrowseAdsPageResponse response = await _browseService.GetBrowseAdsPage(request);

        return View(response);
    }
    [Authorize]
    public async Task<IActionResult> AdDetails(int id)
    {
        var userId = _userAuthenticationService.GetLoggedInUserId();
        if (userId is null)
        {
            return RedirectToAction(nameof(Ads));
        }

        AdDetailsRequest request = new()
        {
            AdId = id,
            UserId = (int)userId,
        };

        // TODO replace with actual all to IBrowseService
        
        AdDetailsResponse response = new()
        {
            AdId = id,
            TutorId = 1,
            TutorName = "Nauczyciel 1",
            Title = "tytuł",
            CategoryId = 1,
            CategoryName = "Matematyka",
            Subject = "tematyka",
            Location = "Warszawa",
            Price = 100m,
            IsRemote = true,
            Description = "opis",
            UserRelationship = AdToUserRelationship.PendingStudent
        };

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

        UsersScheduleRequest request = new()
        {
            UserId = (int)userId
        };

        UsersScheduleResponse response = await _browseService.GetUsersSchedule(request);
        return View(response);
    }
}
