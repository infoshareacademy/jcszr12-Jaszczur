using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TutorLizard.BusinessLogic.Interfaces.Services;
using TutorLizard.Shared.Models.DTOs.Requests;
using TutorLizard.Shared.Models.DTOs.Responses;
using TutorLizard.Web.Interfaces.Services;

namespace TutorLizard.Web.Controllers;
[Authorize]
public class StudentController : Controller
{
    private readonly IStudentService _studentService;
    private readonly IUserAuthenticationService _userAuthenticationService;
    private readonly IUiMessagesService _uiMessagesService;

    public StudentController(IStudentService studentService,
                             IUserAuthenticationService userAuthenticationService,
                             IUiMessagesService uiMessagesService)
    {
        _studentService = studentService;
        _userAuthenticationService = userAuthenticationService;
        _uiMessagesService = uiMessagesService;
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

            GetStudentsAcceptedAdsRequest request = new(studentId);

            GetStudentsAcceptedAdsResponse response = await _studentService.GetStudentsAcceptedAds(request);

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

            GetStudentsAdRequestsRequest request = new(studentId);

            GetStudentsAdRequestsResponse response = await _studentService.GetStudentsAdRequests(request);

            return View(response);
        }

        catch
        {
            return Forbid();
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateScheduleItemRequest(int scheduleItemId, int adId)
    {

        int? studentId = _userAuthenticationService.GetLoggedInUserId();
        if (studentId is null)
        {
            _uiMessagesService.ShowFailureMessage("Wystąpił błąd. Wysłanie zgłoszenia do terminu nieudane.");
            return RedirectToAction(actionName: "Schedule", controllerName: "Browse", routeValues: new { id = scheduleItemId });
        }
        CreateScheduleItemRequestRequest request = new()
        {
            StudentId = (int)studentId,
            ScheduleItemId = scheduleItemId
        };

        CreateScheduleItemRequestResponse response = await _studentService.CreateScheduleItemRequest(request);

        if (response.Success)
        {
            _uiMessagesService.ShowSuccessMessage("Zgłoszenie do terminu wysłane.");
            return RedirectToAction("AdDetails", "Browse", new { id = adId });
        }
        else
        {
            _uiMessagesService.ShowFailureMessage("Wystąpił błąd. Wysłanie zgłoszenia do terminu nieudane.");
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
            _uiMessagesService.ShowSuccessMessage("Zgłoszenie do ogłoszenia wysłane.");
        }
        else
        {
            _uiMessagesService.ShowFailureMessage("Wystąpił błąd. Wysłanie zgłoszenia do ogłoszenia nieudane.");
        }

        return RedirectToAction("AdDetails", "Browse", new { id = request.AdId });
    }
    [HttpDelete]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CancelAdRequest(int adRequestId)
    {
        DeleteAdRequestRequest request = new DeleteAdRequestRequest(adRequestId);
        DeleteAdRequestResponse response = await _studentService.DeleteAdRequest(request);

        if (response.IsSuccessful)
        {
            _uiMessagesService.ShowSuccessMessage("Anulowano zgłoszenie do ogłoszenia.");
        }
        else
        {
            _uiMessagesService.ShowFailureMessage("Wystąpił błąd. Anulowanie zgłoszenia do ogłoszenia nieudane.");
        }

        return RedirectToAction(nameof(Index));
    }
}
