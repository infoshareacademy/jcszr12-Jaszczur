using Microsoft.AspNetCore.Mvc;
using TutorLizard.BusinessLogic.Interfaces.Services;
using TutorLizard.BusinessLogic.Models.DTOs.Requests;
using TutorLizard.BusinessLogic.Models.DTOs.Responses;

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
            if(studentId is null)
            {
                return View();
            }
            AvailableScheduleForAdRequest request = new(adId, (int)studentId, AvailableScheduleForAdRequest.RequestStatus.Accepted);

            AvailableScheduleForAdResponse response = new();

            if (request.Status == AvailableScheduleForAdRequest.RequestStatus.Accepted)
            {
                response.IsAccepted = true;
            }
            else
            {
                response.IsAccepted = false;
            }

            return View(response);
        }
    }
}
