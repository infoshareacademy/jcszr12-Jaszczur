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

    public IActionResult AcceptedAds()
    {
        try
        {
            int? studentId = _userAuthenticationService.GetLoggedInUserId();
            if (studentId is null)
            {
                return RedirectToAction("AccessDenied", "User");
            }

            StudentsAcceptedAdsRequest request = new(studentId);

            StudentsAcceptedAdsResponse response = new()
            {
                // The data is only for tests
                Ads = [new AdListItemDto(1, 1, "Jan", "Maths", "Matematyka", "opis", 1, "Math", 60, "Warszawa", true)]
            };
            return View(response);
        }

        catch
        {
            return RedirectToAction("Error", "Home");
        }
    }
    
    public IActionResult AdRequests()
    {
        try
        {
            int? studentId = _userAuthenticationService.GetLoggedInUserId();
            if(studentId is null)
            {
                return RedirectToAction("AccessDenied", "User");
            }

            StudentsAdRequestsRequest request = new(studentId);

            StudentsAdRequestsResponse response = new()
            {
                //data for tests only
                AdRequests = [new AdRequestsListDto(1, 1, 1, false, "xyz", "yxz", false)]
            };
            return View(response);
        }

        catch
        {
            return RedirectToAction("AccessDenied", "User");
        }

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
}
