using Microsoft.AspNetCore.Mvc;
using TutorLizard.BusinessLogic.Interfaces.Services;
using TutorLizard.Shared.Models.DTOs.Requests;

namespace TutorLizard.Web.ViewComponents;

[ViewComponent(Name = nameof(CreateRequestForAd))]
public class CreateRequestForAd : ViewComponent
{
    private readonly IStudentService _studentService;
    private readonly IUserAuthenticationService _userAuthenticationService;

    public CreateRequestForAd(IStudentService studentService,
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

        CreateAdRequestRequest request = new()
        {
            AdId = adId,
        };

        return View(request);
    }
}
