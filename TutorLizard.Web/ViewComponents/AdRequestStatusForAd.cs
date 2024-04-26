using Microsoft.AspNetCore.Mvc;
using TutorLizard.BusinessLogic.Models.DTOs.Requests;
using TutorLizard.BusinessLogic.Interfaces.Services;
using TutorLizard.BusinessLogic.Models.DTOs.Responses;

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

        // TODO - replace mock data with call to _studentService
        AdRequestStatusResponse response = new()
        {
            Id = 1,
            AdId = adId,
            DateCreated = DateTime.Now.AddMinutes(-30),
            Message = "Twoja wiadomość do nauczyciela",
            ReplyMessage = "Odpowiedź od nauczyciela",
            ReviewDate = DateTime.Now,
            Status = AdRequestStatusResponse.RequestStatus.Pending
        };

        return View(response);
    }
}
