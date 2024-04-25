using Microsoft.AspNetCore.Mvc;
using TutorLizard.BusinessLogic.Interfaces.Services;
using TutorLizard.BusinessLogic.Models.DTOs.Requests;
using TutorLizard.BusinessLogic.Models.DTOs.Responses;

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
        TutorsScheduleForAdRequest request = new()
        {
            AdId = adId,
            TutorId = (int)tutorId
        };

        // TODO - replace test data with call to _tutorService
        TutorsScheduleForAdResponse response = new()
        {
            AdId = adId,
            ScheduleItems =
            [
                new()
                {
                    Id = 1,
                    AdId = adId,
                    DateTime = DateTime.Now.AddHours(1),
                    Requests =
                    [
                        new()
                        {
                            Id = 1,
                            CanBeAccepted = true,
                            DateCreated = DateTime.Now.AddMinutes(-1),
                            IsAccepted = false,
                            IsRemote = true,
                            StudentId = 1,
                            StudentName = "Uczeń 1"
                        },
                        new()
                        {
                            Id = 2,
                            CanBeAccepted = true,
                            DateCreated = DateTime.Now.AddMinutes(-1),
                            IsAccepted = false,
                            IsRemote = false,
                            StudentId = 2,
                            StudentName = "Uczeń 2"
                        }
                    ]
                },
                new()
                {
                    Id = 2,
                    AdId = adId,
                    DateTime = DateTime.Now.AddHours(3),
                    Requests =
                    [
                        new()
                        {
                            Id = 3,
                            CanBeAccepted = false,
                            DateCreated = DateTime.Now.AddMinutes(-1),
                            IsAccepted = true,
                            IsRemote = true,
                            StudentId = 1,
                            StudentName = "Uczeń 1"
                        },new()
                        {
                            Id = 4,
                            CanBeAccepted = false,
                            DateCreated = DateTime.Now.AddMinutes(-1),
                            IsAccepted = false,
                            IsRemote = false,
                            StudentId = 2,
                            StudentName = "Uczeń 2"
                        }
                    ]
                },
                new()
                {
                    Id = 2,
                    AdId = adId,
                    DateTime = DateTime.Now.AddHours(4),
                    Requests = []
                },
            ]
        };

        return View(response);
    }
}
