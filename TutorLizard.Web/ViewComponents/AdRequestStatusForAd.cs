using Microsoft.AspNetCore.Mvc;
using TutorLizard.BusinessLogic.Interfaces.Services;
using TutorLizard.Shared.Models.DTOs.Requests;
using TutorLizard.Shared.Models.DTOs.Responses;

namespace TutorLizard.Web.ViewComponents;
[ViewComponent(Name = nameof(AdRequestStatusForAd))]
public class AdRequestStatusForAd : ViewComponent
{
    private readonly IStudentService _studentService;
    private readonly IUserAuthenticationService _userAuthenticationService;

    public AdRequestStatusForAd(IStudentService studentService,
                                     IUserAuthenticationService userAuthenticationService)
    {
        _studentService = studentService;
        _userAuthenticationService = userAuthenticationService;
    }

    public async Task<IViewComponentResult> InvokeAsync(int adId)
    {
        int? studentId = _userAuthenticationService.GetLoggedInUserId();
        if (studentId is null)
        {
            return View();
        }

        AdRequestStatusRequest request = new()
        {
            AdId = adId,
            StudentId = (int)studentId
        };

        AdRequestStatusResponse response = await _studentService.ViewAdRequestStatus(request);

        if (!response.IsSuccessful)
            return View();

        return View(response);
    }
}
