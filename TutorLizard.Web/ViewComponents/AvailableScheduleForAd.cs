using Microsoft.AspNetCore.Mvc;
using TutorLizard.BusinessLogic.Interfaces.Services;
using TutorLizard.Shared.Models.DTOs.Requests;
using TutorLizard.Shared.Models.DTOs.Responses;

namespace TutorLizard.Web.ViewComponents
{
    [ViewComponent(Name = "AvailableScheduleForAd")]
    public class AvailableScheduleForAd : ViewComponent
    {
        private readonly IStudentService _studentService;
        private readonly IUserAuthenticationService _userAuthenticationService;

        public AvailableScheduleForAd(IStudentService studentService,
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
            AvailableScheduleForAdRequest request = new(adId, (int)studentId);

            AvailableScheduleForAdResponse response = await _studentService.GetAvailableScheduleForAd(request);

            return View(response);
        }
    }
}
