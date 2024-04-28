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
        int pageNumber = id > 0 ? id : 1;
        int pageSize = _pageSize > 0 ? _pageSize : 1;
        GetBrowseAdsPageRequest request = new(pageNumber, _pageSize);

        GetBrowseAdsPageResponse response = await _browseService.GetBrowseAdsPage(request);

        if (response.Success == false)
        {
            // TODO Add failure notification
            return RedirectToAction("Index", "Home");
        }

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

        AdDetailsResponse? response = await _browseService.GetAdDetails(request);

        if (response is null)
        {
            return RedirectToAction(nameof(Ads));
        }

        return View(response);
    }

    [Authorize]
    public IActionResult Schedule()
    {
        // TODO get userId from HttpContext and ask service for ScheduleItems based on it
        int? userId = _userAuthenticationService.GetLoggedInUserId();
        if (userId is null)
        {
            return RedirectToAction(nameof(Index));
        }

        UsersScheduleRequest request = new()
        {
            UserId = (int)userId
        };

        // TODO - replace mock data with all to service
        UsersScheduleResponse response = new()
        {
            StudentsSchedule =
            [
                new()
                {
                    Id = 1,
                    AdId = 1,
                    AdTitle = "Tytuł ogłoszenia 1",
                    DateTime = DateTime.Now.AddDays(1),
                    Status = StudentsScheduleItemSummaryDto.RequestStatus.Accepted,
                    TutorName = "Nauczyciel 1"
                },
                new()
                {
                    Id = 2,
                    AdId = 11,
                    AdTitle = "Tytuł ogłoszenia 11",
                    DateTime = DateTime.Now.AddDays(11),
                    Status = StudentsScheduleItemSummaryDto.RequestStatus.Rejected,
                    TutorName = "Nauczyciel 2"
                },
                new()
                {
                    Id = 3,
                    AdId = 111,
                    AdTitle = "Tytuł ogłoszenia 111",
                    DateTime = DateTime.Now.AddDays(5),
                    Status = StudentsScheduleItemSummaryDto.RequestStatus.Pending,
                    TutorName = "Nauczyciel 3"
                }
            ],
            TutorsSchedule =
            [
                new()
                {
                    Id = 4,
                    AdId = 2,
                    AcceptedStudentsName = null,
                    AdTitle = "Tytuł ogłoszenia 2",
                    DateTime = DateTime.Now.AddHours(1),
                    RequestCount = 0,
                },
                new()
                {
                    Id = 5,
                    AdId = 22,
                    AcceptedStudentsName = "Uczeń 1138",
                    AdTitle = "Tytuł ogłoszenia 22",
                    DateTime = DateTime.Now.AddHours(2),
                    RequestCount = 15,
                },
            ]
        };
        return View(response);
    }
}
