using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TutorLizard.BusinessLogic.Interfaces.Services;
using TutorLizard.BusinessLogic.Models.DTOs;
using TutorLizard.BusinessLogic.Models.DTOs.Requests;
using TutorLizard.BusinessLogic.Models.DTOs.Responses;

namespace TutorLizard.Web.Controllers;
[Authorize]
public class StudentController : Controller
{
    private readonly IStudentService _studentService;
    private readonly IUserAuthenticationService _userAuthenticationService;

    public StudentController(IStudentService studentService,
                             IUserAuthenticationService userAuthenticationService)
    {
        _studentService = studentService;
        _userAuthenticationService = userAuthenticationService;
    }
    public IActionResult Index()
    {
        return View();
    }

    public async Task<IActionResult> AcceptedAds()
    {
        try
        {
            int? studentId = _userAuthenticationService.GetLoggedInUserId();
            if (studentId is null)
            {
                return RedirectToAction("AccessDenied", "User");
            }

            StudentsAcceptedAdsRequest request = new(studentId);

            StudentsAcceptedAdsResponse response = await _studentService.ViewAcceptedAds(request);

            return View(response);
        }

        catch
        {
            return RedirectToAction("Error", "Home");
        }
    }

    public async Task<IActionResult> AdRequests(IFormCollection buttonAction)
    {
        try
        {
            int? studentId = _userAuthenticationService.GetLoggedInUserId();
            if (studentId is null)
            {
                return RedirectToAction("AccessDenied", "User");
            }

            StudentsAdRequestsRequest request = new(studentId);

            StudentsAdRequestsResponse response = await _studentService.ViewAdRequests(request);

            return View(response);
        }

        catch
        {
            return RedirectToAction("AccessDenied", "User");
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateScheduleItemRequest(int scheduleItemRequestId, int scheduleItemId)
    {

        int? studentId = _userAuthenticationService.GetLoggedInUserId();
        if (studentId is null)
        {
            // TODO - show failure notification
            return RedirectToAction(actionName: "Schedule", controllerName: "Browse", routeValues: new { id = scheduleItemId });
        }
        CreateScheduleItemRequestRequest request = new()
        {
            ScheduleItemRequestId = scheduleItemRequestId,
            StudentId = (int)studentId
        };

        // TODO - replace mock response with call to _tutorService
        CreateScheduleItemRequestResponse response = await _studentService.CreateScheduleItemRequest(request);

        if (response.Success)
        {
            // TODO - show success notification

        }
        else
        {
            // TODO - show failure notification

        }

        return RedirectToAction(actionName: "Schedule", controllerName: "Browse", routeValues: new { id = scheduleItemId });
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateAdRequest(CreateAdRequestRequest request)
    {
        if (ModelState.IsValid == false)
        {
            return RedirectToAction(nameof(Index));
        }

        int? studentId = _userAuthenticationService.GetLoggedInUserId();
        if (studentId is null)
        {
            return RedirectToAction(nameof(Index));
        }
        request.StudentId = (int)studentId;

        // TODO - replace mock data with call to _studentService
        CreateAdRequestResponse response = new()
        {
            Success = true,
        };

        if (response.Success)
        {
            // TODO - Show success notification

        }
        else
        {
            // TODO - Show failure notification

        }

        // TODO - redirect to details of the ad
        return RedirectToAction(nameof(Index));
    }
}
