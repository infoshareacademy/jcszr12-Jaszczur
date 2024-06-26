using Microsoft.AspNetCore.Mvc;
using TutorLizard.BusinessLogic.Interfaces.Services;
using TutorLizard.Shared.Models.DTOs.Requests;
using TutorLizard.Shared.Models.DTOs.Responses;

namespace TutorLizard.Web.ViewComponents;

[ViewComponent(Name = "TutorsScheduleForAd")]
public class TutorsScheduleForAd : ViewComponent
{
    private readonly ITutorService _tutorService;
    private readonly IUserAuthenticationService _userAuthenticationService;

    public TutorsScheduleForAd(ITutorService tutorService,
                               IUserAuthenticationService userAuthenticationService)
    {
        _tutorService = tutorService;
        _userAuthenticationService = userAuthenticationService;
    }

    public async Task<IViewComponentResult> InvokeAsync(int adId)
    {
        int? tutorId = _userAuthenticationService.GetLoggedInUserId();
        if (tutorId is null)
        {
            return View();
        }
        GetTutorsScheduleForAdRequest request = new()
        {
            AdId = adId,
            TutorId = (int)tutorId
        };

        GetTutorsScheduleForAdResponse response = await _tutorService.GetTutorsScheduleForAd(request);

        return View(response);
    }
}
