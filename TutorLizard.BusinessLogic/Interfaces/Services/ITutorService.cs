using TutorLizard.BusinessLogic.Models;
using TutorLizard.BusinessLogic.Models.DTOs.Requests;
using TutorLizard.BusinessLogic.Models.DTOs.Responses;

namespace TutorLizard.BusinessLogic.Interfaces.Services;

public interface ITutorService
{
    Task<TutorsScheduleForAdResponse> GetTutorsScheduleForAd(TutorsScheduleForAdRequest request);
    Task<IsUserTheAdOwnerResponse> IsUserTheAdOwner(IsUserTheAdOwnerRequest request);
}