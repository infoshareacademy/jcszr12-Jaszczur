using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
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
                return Forbid();
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
                return Forbid();
            }

            StudentsAdRequestsRequest request = new(studentId);

            StudentsAdRequestsResponse response = await _studentService.ViewAdRequests(request);

            return View(response);
        }

        catch
        {
            return Forbid();
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateScheduleItemRequest(int scheduleItemId, int adId, bool isRemote)
    {

        int? studentId = _userAuthenticationService.GetLoggedInUserId();
        if (studentId is null)
        {
            // TODO - show failure notification
            return RedirectToAction(actionName: "Schedule", controllerName: "Browse", routeValues: new { id = scheduleItemId });
        }
        CreateScheduleItemRequestRequest request = new()
        {
            StudentId = (int)studentId,
            ScheduleItemId = scheduleItemId,
            IsRemote = isRemote
        };

        CreateScheduleItemRequestResponse response = await _studentService.CreateScheduleItemRequest(request);

        if (response.Success)
        {
            ViewBag.SuccessMessage = "Wysłano prośbę";
            return RedirectToAction("AdDetails", "Browse", new {id = adId });
        }
        else
        {
            ViewBag.ErrorMessage = "Wystąpił błąd";
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

        CreateAdRequestResponse response = await _studentService.CreateAdRequest(request);

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
    [HttpDelete]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CancelAdRequest(int adRequestId)
    {
        StudentCancelAdRequestRequest request = new StudentCancelAdRequestRequest(adRequestId);
        StudentCancelAdRequestResponse response = await _studentService.DeleteAdRequest(request);

        if (response.IsSuccessful)
        {
            // TODO - Show success notification
        }
        else
        {
            // TODO - Show failure notification
        }

        return RedirectToAction(nameof(Index));
    }
}
